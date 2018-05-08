using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using FrameworkUAS.Object;

namespace ControlCenter.Modules.ClientControls
{
    /// <summary>
    /// Interaction logic for CreateNewClient.xaml
    /// </summary>
    public partial class CreateNewClient : UserControl
    {
        public CreateNewClient()
        {
            LoadData();
            InitializeComponent();
        }

        private FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> userData { get; set; }
        private KMPlatform.Entity.Client newClient { get; set; }
        private DoubleAnimation animateOpacity { get; set; }

        private void LoadData()
        {
            userData = FrameworkServices.ServiceClient.UAS_ClientClient();
            newClient = new KMPlatform.Entity.Client();
            animateOpacity = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(.4)
            };
        }

        #region ButtonMethods
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string fName = tbName.Text.Trim();
            string testString = tbTestString.Text.Trim();
            string liveString = tbLiveString.Text.Trim();
            bool isActive = false;
            if (cbIsActive.IsChecked.HasValue)
                isActive = cbIsActive.IsChecked.Value;
            bool ignoreFiles = false;
            if (cbIgnore.IsChecked.HasValue)
                ignoreFiles = cbIgnore.IsChecked.Value;
            string managerEmails = tbManagerEmails.Text.Trim();
            string clientEmails = tbClientEmails.Text.Trim();
            DateTime todayDate = DateTime.Now;

            newClient.ClientName = fName;
            newClient.ClientTestDBConnectionString = testString;
            newClient.ClientLiveDBConnectionString = liveString;
            newClient.IsActive = isActive;
            newClient.IgnoreUnknownFiles = ignoreFiles;
            newClient.AccountManagerEmails = managerEmails;
            newClient.ClientEmails = clientEmails;
            newClient.DateCreated = todayDate;
            //userData.Save(newClient);
            newClient.ClientID = userData.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, newClient).Result;

            AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients.Add(newClient);
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
            Canvas c = (Canvas)this.Parent;
            DragDropPopUp p = (DragDropPopUp)c.Parent;
            animateOpacity.Completed += (s, _) => p.IsOpen = false;
            this.BeginAnimation(UserControl.OpacityProperty, animateOpacity);
        }

        private void Window_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            //DragMove();
        }

        bool closeClick = false;
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (closeClick == false)
            {
                Canvas c = (Canvas)this.Parent;
                DragDropPopUp p = (DragDropPopUp)c.Parent;
                closeClick = true;
                MessageBoxResult confirm = MessageBox.Show("Any unsaved data will be lost by leaving the page. Do you wish to continue?", "Unsaved Data", MessageBoxButton.YesNo);
                if (confirm == MessageBoxResult.Yes)
                {
                    //Canvas c = (Canvas)this.Parent;
                    //DragDropPopUp p = (DragDropPopUp)c.Parent;
                    animateOpacity.Completed += (s, _) => p.IsOpen = false;
                    this.BeginAnimation(UserControl.OpacityProperty, animateOpacity);
                    closeClick = false;
                }
                else
                    return;
            }
        }
        #endregion
    }
}
