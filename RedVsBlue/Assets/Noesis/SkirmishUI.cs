using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using UnityEngine.SceneManagement;

public class SkirmishUI : MonoBehaviour, INotifyPropertyChanged
{
    public NoesisEventCommand _startCommand;
    public NoesisEventCommand StartCommand { get => _startCommand; }

    public bool Red { get; set; }
    public bool Blue { get; set; }

    public bool Map1 { get; set; }
    public bool Map2 { get; set; }
    public bool Map3 { get; set; }


    public event PropertyChangedEventHandler PropertyChanged;

    void Start()
    {
        NoesisView view = GetComponent<NoesisView>();
        view.Content.DataContext = this;
    }

    public void OnStart()
    {
        if (Red)
        {
            DataManager.team = "Faction2";
        }
        else
        {
            DataManager.team = "Faction1";
        }

        if (Map1)
        {
            SceneManager.LoadScene("BigMap", LoadSceneMode.Single);
        }
    }

    public void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
