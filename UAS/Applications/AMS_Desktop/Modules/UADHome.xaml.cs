using System;
using System.Linq;
using System.Windows.Controls;

namespace AMS_Desktop.Modules
{
    /// <summary>
    /// Interaction logic for UADHome.xaml
    /// </summary>
    public partial class UADHome : UserControl
    {
        public static FrameworkUAS.Object.AppData myAppData { get; set; }
        public UADHome(FrameworkUAS.Object.AppData appData)
        {
            InitializeComponent();
            myAppData = appData;
        }
    }
}
