//#define DEBUG_IMPORTER

using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Noesis;
using System.IO;
using System.Collections.Generic;

[CustomEditor(typeof(NoesisFont))]
public class NoesisFontEditor: Editor
{
    struct Face
    {   
        public int index;
        public string family;
        public Noesis.FontWeight weight;
        public Noesis.FontStyle style;
        public Noesis.FontStretch stretch;
    }

    private List<Face> _faces = new List<Face>();
    private int _index;
    private Noesis.View _view;
    private Noesis.View _viewIcon;
    private UnityEngine.Rendering.CommandBuffer _commands;

    private static bool IsGL()
    {
        var type = SystemInfo.graphicsDeviceType;

      #if UNITY_2023_1_OR_NEWER
        return type == GraphicsDeviceType.OpenGLES3 || type == GraphicsDeviceType.OpenGLCore;
      #else
        return type == GraphicsDeviceType.OpenGLES2 || type == GraphicsDeviceType.OpenGLES3
            || type == GraphicsDeviceType.OpenGLCore;
      #endif
    }

    private void AddRun(TextBlock text, int size, FontFamily family)
    {
        text.Inlines.Add(new Run(size.ToString() + " "));

        Run run = new Run("The quick brown fox jumps over the lazy dog. 1234567890");
        run.FontSize = size;
        run.FontFamily = family;
        run.FontWeight = _faces[_index].weight;
        run.FontStyle = _faces[_index].style;
        run.FontStretch = _faces[_index].stretch;
        text.Inlines.Add(run);

        text.Inlines.Add(new LineBreak());
    }

    private FrameworkElement GetRoot()
    {
        StackPanel root = new StackPanel();
        root.Background = new SolidColorBrush(Colors.White);

        if (_faces.Count > 0 && target != null)
        {
            _index = System.Math.Min(_index, _faces.Count - 1);
            NoesisFont font = (NoesisFont)target;
            FontFamily family = new FontFamily(System.IO.Path.GetDirectoryName(font.uri) + "/#" + _faces[_index].family);

            TextBlock text = new TextBlock();
            text.Margin = new Thickness(2);
            text.Foreground = new SolidColorBrush(Colors.Black);
 
            AddRun(text, 12, family);
            AddRun(text, 18, family);
            AddRun(text, 24, family);
            AddRun(text, 36, family);
            AddRun(text, 48, family);
            AddRun(text, 60, family);
            AddRun(text, 72, family);

            root.Children.Add(text);
        }

        return root;
    }

    private FrameworkElement GetRootIcon()
    {
        Noesis.Grid root = new Noesis.Grid();
        root.Background = new SolidColorBrush(Colors.White);

        Viewbox box = new Viewbox();
        box.Margin = new Thickness(10);

        if (_faces.Count > 0 && target != null)
        {
            NoesisFont font = (NoesisFont)target;

            TextBlock text = new TextBlock();
            text.Foreground = new SolidColorBrush(Colors.Black);
            text.FontFamily = new FontFamily(System.IO.Path.GetDirectoryName(font.uri) + "/#" + _faces[0].family);
            text.FontWeight = _faces[0].weight;
            text.FontStyle = _faces[0].style;
            text.FontStretch = _faces[0].stretch;
            text.Text = "Abg";

            box.Child = text;
        }
        
        root.Children.Add(box);
        return root;
    }

    private void CreateView()
    {
        try
        {
            FrameworkElement root = GetRoot();
            _view = Noesis.GUI.CreateView(root);
            _view.SetFlags(IsGL() ? 0 : RenderFlags.FlipY);

            NoesisRenderer.RegisterView(_view, _commands);
            Graphics.ExecuteCommandBuffer(_commands);
            _commands.Clear();
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogException(e);
        }
    }

    private void CreateViewIcon()
    {
        try
        {
            FrameworkElement root = GetRootIcon();
            _viewIcon = Noesis.GUI.CreateView(root);
            _viewIcon.SetFlags(IsGL() ? 0 : RenderFlags.FlipY);

            NoesisRenderer.RegisterView(_viewIcon, _commands);
            Graphics.ExecuteCommandBuffer(_commands);
            _commands.Clear();
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogException(e);
        }
    }

    private void RegisterFont(NoesisFont font)
    {
        using (MemoryStream stream = new MemoryStream(font.content))
        {
            _index = 0;
            _faces.Clear();
            Noesis.GUI.EnumFontFaces(stream, (index_, family_, weight_, style_, stretch_) =>
            {
                _faces.Add(new Face() { index = index_, family = family_, weight = weight_, style = style_, stretch = stretch_ } );
            });
        }

        if (_faces.Count > 0)
        {
            NoesisFontProvider.instance.Register(font.uri, font);
        }
    }

    public void RegisterFont()
    {
        if (target != null)
        {
            NoesisFont font = (NoesisFont)target;

            if (font.uri != null)
            {
                NoesisUnity.Init();
                RegisterFont(font);
            }
        }
    }

    public void UnregisterFont()
    {
        if (target != null && _faces.Count > 0)
        {
            NoesisFont font = (NoesisFont)target;

            if (font.uri != null)
            {
                NoesisFontProvider.instance.Unregister(font.uri);
            }
        }

        _faces.Clear();
    }

    public void OnEnable()
    {
        if (_commands == null)
        {
            _commands = new UnityEngine.Rendering.CommandBuffer();
        }

        RegisterFont();
    }

    public void OnDisable()
    {
        if (_view != null)
        {
            NoesisRenderer.UnregisterView(_view, _commands);
            Graphics.ExecuteCommandBuffer(_commands);
            _commands.Clear();
            _view.Content?.Dispose();
            _view.Dispose();
            _view = null;
        }

        if (_viewIcon != null)
        {
            NoesisRenderer.UnregisterView(_viewIcon, _commands);
            Graphics.ExecuteCommandBuffer(_commands);
            _commands.Clear();
            _viewIcon.Content?.Dispose();
            _viewIcon.Dispose();
            _viewIcon = null;
        }

        UnregisterFont();
    }

    public override void OnInspectorGUI()
    {
        bool enabled = UnityEngine.GUI.enabled;
        UnityEngine.GUI.enabled = true;

        foreach (var face in _faces)
        {
            EditorGUILayout.BeginFoldoutHeaderGroup(true, $"Face {face.index}");
            EditorGUILayout.LabelField("Family", face.family, EditorStyles.textField);
            EditorGUILayout.LabelField("Weight", face.weight.ToString(), EditorStyles.textField);
            EditorGUILayout.LabelField("Style", face.style.ToString(), EditorStyles.textField);
            EditorGUILayout.LabelField("Stretch", face.stretch.ToString(), EditorStyles.textField);
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        UnityEngine.GUI.enabled = enabled;
    }

    private bool CanRender()
    {
        return NoesisSettings.Get().previewEnabled && _faces.Count > 0;
    }

    public override bool HasPreviewGUI()
    {
        if (!CanRender())
        {
            return false;
        }

        if (_view == null)
        {
            CreateView();
        }

        if (_view == null || _view.Content == null)
        {
            return false;
        }

        return true;
    }

    public override void OnPreviewGUI(UnityEngine.Rect r, GUIStyle background)
    {
        if (Event.current.type == EventType.Repaint)
        {
            if (CanRender())
            {
                if (r.width > 4 && r.height > 4)
                {
                    if (_view != null && _view.Content != null)
                    {
                        UnityEngine.GUI.DrawTexture(r, RenderPreview(_view, (int)r.width, (int)r.height));
                    }
                }
            }
        }
    }

    public override void OnPreviewSettings()
    {
        string[] options = new string[_faces.Count];
        int[] values = new int[_faces.Count];

        for (int i = 0; i < _faces.Count; i++)
        {
            options[i] = "Face " + i;
            values[i] = i;
        }

        int index = EditorGUILayout.IntPopup(_index, options, values);
        if (index != _index && _view != null)
        {
            NoesisRenderer.UnregisterView(_view, _commands);
            Graphics.ExecuteCommandBuffer(_commands);
            _commands.Clear();

            _index = index;
            CreateView();
        }
    }

    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        if (CanRender())
        {
            if (_viewIcon == null)
            {
                CreateViewIcon();
            }

            if (_viewIcon != null && _viewIcon.Content != null)
            {
                #if DEBUG_IMPORTER
                    Debug.Log($"=> RenderStaticPreview {assetPath}");
                #endif

                RenderTexture rt = RenderPreview(_viewIcon, width, height);

                if (rt != null)
                {
                    RenderTexture prev = RenderTexture.active;
                    RenderTexture.active = rt;

                    Texture2D tex = new Texture2D(width, height);
                    tex.ReadPixels(new UnityEngine.Rect(0, 0, width, height), 0, 0);
                    tex.Apply(true);

                    RenderTexture.active = prev;
                    return tex;
                }
            }
        }

        return null;
    }

    private RenderTexture RenderPreview(Noesis.View view, int width, int height)
    {
        try
        {
            if (CanRender() && view != null && view.Content != null)
            {
                NoesisRenderer.SetRenderSettings();

                view.SetSize(width, height);
                view.Update(0.0f);

                NoesisRenderer.UpdateRenderTree(view, _commands);
                NoesisRenderer.RenderOffscreen(view, _commands, false);

                RenderTexture rt = RenderTexture.GetTemporary(width, height, 24, RenderTextureFormat.Default, RenderTextureReadWrite.Default, 8);
                _commands.SetRenderTarget(rt);
                _commands.ClearRenderTarget(true, true, UnityEngine.Color.clear, 0.0f);
                NoesisRenderer.RenderOnscreen(view, false, _commands, true, false);

                Graphics.ExecuteCommandBuffer(_commands);
                _commands.Clear();

                RenderTexture.ReleaseTemporary(rt);
                return rt;
            }
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogException(e);
        }

        return null;
    }
}
