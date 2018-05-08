using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Circulation.Modules;
using System.Windows.Interop;
using System.ComponentModel;

namespace Circulation.Windows
{
    /// <summary>
    /// Interaction logic for Popout.xaml
    /// </summary>
    public partial class Popout : Window, INotifyPropertyChanged
    {
        private SubscriptionContainer _subContainer;
        private bool _ignoreSaveMessage = false;
        #region Properties
        public SubscriptionContainer SubContainer
        {
            get { return _subContainer; }
            set
            {
                _subContainer = value;
            }
        }
        public bool IgnoreSaveMessage
        {
            //IgnoreSaveMessage determines whether or not the user is presented with a save warning when they try to close the window. Ignoring it allows the program to force close the window.
            get { return _ignoreSaveMessage; }
            set
            {
                _ignoreSaveMessage = value;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public Popout(UserControl uc)
        {
            InitializeComponent();           
            //Home.myAppData = appData;

            if (uc.GetType() == typeof(History))
                this.Title = "Batch History";
            else if (uc.GetType() == typeof(SubscriptionContainer))
            {
                this.Title = "Subscription";
                this.SubContainer = (SubscriptionContainer)uc;
            }
                
            BannerSetUp();
            spModule.Children.Add(uc);        
        }

        #region ButtonMethods
        private void Window_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            //FrameworkServices.ServiceClient<UAD_WS.Interface.IProductSubscription> worker = FrameworkServices.ServiceClient.UAD_ProductSubscriptionClient();

            //if (SubscriptionContainer)
            //{
            //    MessageBoxResult confirm = MessageBox.Show("Any unsaved data will be lost by leaving the page. Do you wish to continue?", "Unsaved Data", MessageBoxButton.YesNo);
            //    if (confirm == MessageBoxResult.Yes)
            //    {
            //        if (NewSubscription == true)
            //        {
            //            Home.myAppData.NewSubscriptionOpened = false;
            //            DockPanel sp = Core_AMS.Utilities.WPF.FindChild<DockPanel>(this, "spModule");

            //            SubscriptionContainer scc = Core_AMS.Utilities.WPF.FindChild<SubscriptionContainer>(sp, "mainControl");

            //            //worker.Proxy.DeleteSubscriber(Home.myAppData.AuthorizedUser.AuthAccessKey, SubscriberID);
            //        }
            //        else if (SubscriberID > 0 && lockStatus == false)
            //        {
            //            // Users can view record when its locked but can't edit the record
            //            // Unlock record only if it wasn't locked in the first place
            //            worker.Proxy.UpdateLock(Home.myAppData.AuthorizedUser.AuthAccessKey, SubscriberID, false, Home.myAppData.AuthorizedUser.User.UserID, Home.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);                
            //        }

            //        if (Home.myAppData.OpenWindowCount > 0)
            //        {
            //            Home.myAppData.OpenWindowCount--;
            //        }

            //        this.Close();
            //    }
            //    else
            //        return;
            //}
            //else
            //{
            //    this.Close();
            //}

            //if (SuggestMatch)
            //{
            //    spModule.Children.Remove(control);
            //    this.Close();
            //}
            
        }
        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Maximized)
            {
                this.WindowState = System.Windows.WindowState.Normal;
            }
            else
            {
                this.WindowState = System.Windows.WindowState.Maximized;
            }

        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }
        #endregion
        private void BannerSetUp()
        {
            spModule.Children.Clear();
            spModule.Visibility = Visibility.Visible;
            this.SystemBanner.Source = new BitmapImage(new Uri(@"/ImageLibrary;component/Images/Banners/CirculationRevised.jpg", UriKind.RelativeOrAbsolute));
            this.Width = 1090;
            this.Height = 780;
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            if (System.Windows.Forms.SystemInformation.MonitorCount > 1)
            {
                System.Drawing.Rectangle workingArea = GetScreenFrom(this);
                this.Left = (workingArea.Width - this.Width) / 2 + workingArea.Left;
                this.Top = (workingArea.Height - this.Height) / 2 + workingArea.Top;
            }
            else
            {
                this.Left = (SystemParameters.WorkArea.Width - this.Width) / 2 + SystemParameters.WorkArea.Left;
                this.Top = (SystemParameters.WorkArea.Height - this.Height) / 2 + SystemParameters.WorkArea.Top;
            }
            TitleBarImage.Visibility = Visibility.Visible;
            TitleBarImage.Source = new BitmapImage(new Uri(@"/ImageLibrary;component/Images/Banners/TempSolidBlue.jpg", UriKind.RelativeOrAbsolute));
        }
        public static System.Drawing.Rectangle GetScreenFrom(Window window)
        {
            WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);
            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.FromHandle(windowInteropHelper.Handle);
            return screen.WorkingArea;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int userID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            FrameworkServices.ServiceClient<UAD_WS.Interface.IProductSubscription> worker = FrameworkServices.ServiceClient.UAD_ProductSubscriptionClient();
            if (this.SubContainer != null)
            {
                if (this.SubContainer.InfoChanged == true && this.IgnoreSaveMessage == false)
                {
                    if (this.SubContainer.Saved == false)
                    {
                        #region Close Not From Saved
                        MessageBoxResult confirm = MessageBox.Show("Any unsaved data will be lost by leaving the page. Do you wish to continue?", "Unsaved Data", MessageBoxButton.YesNo);
                        if (confirm == MessageBoxResult.Yes)
                        {
                            if (this.SubContainer.IsNewSubscription == true)
                            {
                                FrameworkUAS.Object.AppData.myAppData.NewSubscriptionOpened = false;
                            }
                            else if (this.SubContainer.PubSubscriptionID > 0 && this.SubContainer.LockedByUser == userID)
                            {
                                worker.Proxy.UpdateLock(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, this.SubContainer.PubSubscriptionID, false, userID, 
                                    FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                            }

                            if (FrameworkUAS.Object.AppData.myAppData.OpenWindowCount > 0)
                            {
                                FrameworkUAS.Object.AppData.myAppData.OpenWindowCount--;
                            }
                        }
                        else
                        {
                            e.Cancel = true;
                            return;
                        }
                        #endregion
                    }
                    else
                    {
                        #region Closed From Save
                        if (this.SubContainer.IsNewSubscription == true)
                        {
                            FrameworkUAS.Object.AppData.myAppData.NewSubscriptionOpened = false;
                        }
                        else if (this.SubContainer.PubSubscriptionID > 0 && this.SubContainer.LockedByUser == userID)
                        {
                            worker.Proxy.UpdateLock(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, this.SubContainer.PubSubscriptionID, false, userID, 
                                FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                        }
                        #endregion
                    }
                }
                if (FrameworkUAS.Object.AppData.myAppData.OpenWindowCount > 0)
                {
                    FrameworkUAS.Object.AppData.myAppData.OpenWindowCount--;
                }
                if (this.SubContainer.IsNewSubscription == false && this.SubContainer.PubSubscriptionID > 0 && this.SubContainer.LockedByUser == userID)
                {
                    worker.Proxy.UpdateLock(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, this.SubContainer.PubSubscriptionID, false, userID, 
                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                }
                else if (this.SubContainer.IsNewSubscription == true)
                {
                    FrameworkUAS.Object.AppData.myAppData.NewSubscriptionOpened = false;
                }
            }
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                if (this.WindowState == System.Windows.WindowState.Maximized)
                {
                    this.WindowState = System.Windows.WindowState.Normal;
                    e.Handled = true;
                }
                else
                {
                    this.WindowState = System.Windows.WindowState.Maximized;
                    e.Handled = true;
                }
            }

        }
    }
}
