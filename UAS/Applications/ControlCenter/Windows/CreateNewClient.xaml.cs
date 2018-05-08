using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ControlCenter.Windows
{
    /// <summary>
    /// Interaction logic for CreateNewClient.xaml
    /// </summary>
    public partial class CreateNewClient : Window
    {
        public CreateNewClient()
        {
            LoadData();
            InitializeComponent();
        }

        private FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> userData { get; set; }
        private KMPlatform.Entity.Client newClient { get; set; }

        private void LoadData()
        {
            userData = FrameworkServices.ServiceClient.UAS_ClientClient();
            newClient = new KMPlatform.Entity.Client();
        }

        #region ButtonMethods
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string fName = tbName.Text.Trim();
            string testString = tbTestString.Text.Trim();
            string liveString = tbLiveString.Text.Trim();
            bool isActive = false;
            if(cbIsActive.IsChecked.HasValue)
                isActive = cbIsActive.IsChecked.Value;
            bool ignoreFiles = false;
            if (cbIgnore.IsChecked.HasValue)
                ignoreFiles = cbIgnore.IsChecked.Value;
            string managerEmails = tbManagerEmails.Text.Trim();
            string clientEmails = tbClientEmails.Text.Trim();
            string clientCode = tbCode.Text.Trim();
            DateTime todayDate = DateTime.Now;

            newClient.ClientName = fName;
            newClient.ClientCode = clientCode;
            newClient.ClientTestDBConnectionString = testString;
            newClient.ClientLiveDBConnectionString = liveString;
            newClient.IsActive = isActive;
            newClient.IgnoreUnknownFiles = ignoreFiles;
            newClient.AccountManagerEmails = managerEmails;
            newClient.ClientEmails = clientEmails;
            newClient.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            newClient.DateCreated = todayDate;

            newClient.ClientID = userData.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, newClient).Result;

            MessageBoxResult confirm = MessageBox.Show("New client has been added.", "Saved Client", MessageBoxButton.OK);
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            tbName.Text = string.Empty;
            tbTestString.Text = string.Empty;
            tbLiveString.Text = string.Empty;
            cbIsActive.IsChecked = false;
            cbIgnore.IsChecked = false;
            tbManagerEmails.Text = string.Empty;
            tbClientEmails.Text = string.Empty;
            this.Close();
        }

        private void Window_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult confirm = MessageBox.Show("Any unsaved data will be lost by leaving the page. Do you wish to continue?", "Unsaved Data", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                this.Close();
            }
            else
                return;
        }

        private bool max;
        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (max)
            {
                max = false;
                this.WindowState = System.Windows.WindowState.Normal;
            }
            else
            {
                max = true;
                this.WindowState = System.Windows.WindowState.Maximized;
            }

        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }
        #endregion
    }
}
