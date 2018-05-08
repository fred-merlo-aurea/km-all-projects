using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ControlCenter.Modules.ClientControls
{
    /// <summary>
    /// Interaction logic for Configuration.xaml
    /// </summary>
    public partial class Configuration : UserControl
    {
        private Windows.Helper parentWin { get; set; }
        public Configuration(Windows.Helper win, int id)
        {
            InitializeComponent();
            LoadTabs(win, id);
        }

        private void LoadTabs(Windows.Helper win, int id)
        {
            parentWin = win;
            var tabCustom = new Telerik.Windows.Controls.RadTabItem();
            var tabFTP = new Telerik.Windows.Controls.RadTabItem();
            var tabSpecial = new Telerik.Windows.Controls.RadTabItem();
            tabCustom.Header = "Custom Procedures";
            tabCustom.Content = new CustomProcedures(id);
            tabFTP.Header = "FTP Sites";
            //tabFTP.Content = new FTPSites(id);
            tabSpecial.Header = "Special Files";
            tabSpecial.Content = new SpecialFiles(id);
            tcClientDetails.Items.Add(tabCustom);
            tcClientDetails.Items.Add(tabFTP);
            tcClientDetails.Items.Add(tabSpecial);
        }

        #region buttonMethods
        public void btnClose_Click(object sender, RoutedEventArgs e)
        {
            parentWin = (Windows.Helper)this.Parent;
            parentWin.Close();
        }

        private bool max;
        public void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            parentWin = (Windows.Helper)this.Parent;
            if (max)
            {
                max = false;
                parentWin.WindowState = System.Windows.WindowState.Normal;
            }
            else
            {
                max = true;
                parentWin.WindowState = System.Windows.WindowState.Maximized;
            }

        }

        public void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            parentWin = (Windows.Helper)this.Parent;
            parentWin.WindowState = System.Windows.WindowState.Minimized;
        }
        #endregion
    }
}
