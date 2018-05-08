using System;
using System.Linq;
using System.Windows.Controls;

namespace ProfileManager.Modules
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public static FrameworkUAS.Object.AppData myAppData { get; set; }

        public Home(FrameworkUAS.Object.AppData appData)
        {
            InitializeComponent();
            myAppData = appData;
        }
    }
}
