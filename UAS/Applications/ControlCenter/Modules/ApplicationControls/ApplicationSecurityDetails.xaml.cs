using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ControlCenter.Modules.ApplicationControls
{
    /// <summary>
    /// Interaction logic for ApplicationSecurityDetails.xaml
    /// </summary>
    public partial class ApplicationSecurityDetails : UserControl
    {
        private Windows.Helper parentWin { get; set; }
        //private List<KMPlatform.Entity.ApplicationSecurityGroupMap> appRecords { get; set; }
        //private FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationSecurityGroupMap> appWorker  { get; set; }
        private FrameworkServices.ServiceClient<UAS_WS.Interface.ISecurityGroup> securityData { get; set; }

        public ApplicationSecurityDetails(Windows.Helper win, int id)
        {
            InitializeComponent();
            LoadData(win, id);
        }

        public void LoadData(Windows.Helper win, int id)
        {
            parentWin = win;
            securityData = FrameworkServices.ServiceClient.UAS_SecurityGroupClient();
            //appWorker = FrameworkServices.ServiceClient.UAS_ApplicationSecurityGroupMapClient();
            //appRecords = appWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result.Where(x => x.ApplicationID == id).ToList();
            grdCustom.ItemsSource = null;
            //grdCustom.ItemsSource = appRecords;
            grdCustom.Rebind();
        }


        public void btnClose_Click(object sender, RoutedEventArgs e)
        {
            parentWin = (Windows.Helper)this.Parent;
            parentWin.Close();
        }

        bool max = false;
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
    }
}
