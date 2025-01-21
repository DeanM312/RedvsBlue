#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
#else
using System;
using System.Windows.Controls;
#endif

namespace RedVsBlue
{
    /// <summary>
    /// Interaction logic for RedVsBlueMainView.xaml
    /// </summary>
    public partial class RedVsBlueMainView : UserControl
    {
        public RedVsBlueMainView()
        {
            InitializeComponent();
        }

#if NOESIS
        private void InitializeComponent()
        {
            NoesisUnity.LoadComponent(this);
        }
#endif
    }
}
