using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using FrameworkUAS.Object;

namespace ControlCenter.Windows
{
    /// <summary>
    /// Interaction logic for CreateNewApplication.xaml
    /// </summary>
    public partial class CreateNewApplication : Window
    {
         public CreateNewApplication()
        {
            Window parentWindow = Application.Current.MainWindow;
            if (AppData.CheckParentWindowUid(parentWindow.Uid))
            {
                //only want this available to users that belong to KM
                if (AppData.IsKmUser() == true)
                {
                    InitializeComponent();
                    LoadData();
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageAccessDenied();
                }
            }
        }

        private KMPlatform.BusinessLogic.Application appData { get; set; }
        private KMPlatform.Entity.Application newApp { get; set; }

        private void LoadData()
        {
            appData = new KMPlatform.BusinessLogic.Application();
            newApp = new KMPlatform.Entity.Application();
        }

        #region ButtonMethods
        public void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string appName = tbName.Text.Trim();
            string appCode = tbAppCode.Text.Trim();
            string defaultView = tbDefaultView.Text.Trim();
            bool isActive = false;
            if (cbIsActive.IsChecked.HasValue)
                isActive = cbIsActive.IsChecked.Value;
            string iconName = tbIconName.Text.Trim();
            DateTime todayDate = DateTime.Now;

            newApp.ApplicationName = appName;
            newApp.ApplicationCode = appCode;
            newApp.DefaultView = defaultView;
            newApp.IsActive = isActive;
            newApp.IconFullName = iconName;
            newApp.DateCreated = todayDate;
            newApp.CreatedByUserID = AppData.myAppData.AuthorizedUser.User.UserID;
            newApp.ApplicationID = appData.Save(newApp);
            MessageBoxResult confirm = MessageBox.Show("New application has been added.", "Saved Application", MessageBoxButton.OK);
            this.Close();
        }

        public void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            tbName.Text = string.Empty;
            tbAppCode.Text = string.Empty;
            tbDefaultView.Text = string.Empty;
            cbIsActive.IsChecked = false;
            tbIconName.Text = string.Empty;
            this.Close();
        }

        public void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult confirm = MessageBox.Show("Any unsaved data will be lost by leaving the page. Do you wish to continue?", "Unsaved Data", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                this.Close();
            }
            else
                return;
        }

        bool max = false;
        public void btnMaximize_Click(object sender, RoutedEventArgs e)
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

        public void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        public void Window_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        #endregion
    }
}
