using Core.ADMS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Deployment.Application;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Telerik.Windows.Controls;

namespace AMS_Desktop.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Home : Window
    {
        public string AppName
        {
            get
            {
                return Path.GetFileNameWithoutExtension(Application.ResourceAssembly.Location).Replace("_", " ");
            }
        }
        private bool isFirstLoad { get; set; }
        private bool isLoggedOut { get; set; }
        private static AutoResetEvent resetEvent = new AutoResetEvent(false);
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> clientW = FrameworkServices.ServiceClient.UAS_ClientClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IClientFTP> clientftpW = FrameworkServices.ServiceClient.UAS_ClientFTPClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IClientCustomProcedure> clientCustomW = FrameworkServices.ServiceClient.UAS_ClientCustomProcedureClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.ISourceFile> sourceFileW = FrameworkServices.ServiceClient.UAS_SourceFileClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> productW = FrameworkServices.ServiceClient.UAD_ProductClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.ISecurityGroup> securityGrpW = FrameworkServices.ServiceClient.UAS_SecurityGroupClient();
        private ObservableCollection<ApplicationIcon> _apps = new ObservableCollection<ApplicationIcon>();
        public ObservableCollection<ApplicationIcon> Apps { get { return _apps; } }
        private bool isShowServiceScreen = false;

        #region Classes
        public class ApplicationIcon : INotifyPropertyChanged
        {
            public ApplicationIcon(int appID, string img, string name)
            {
                this.ApplicationID = appID;
                this.ImageSource = img;
                this.DisplayName = name;
            }
            public int ApplicationID { get; set; }
            public string ImageSource { get; set; }
            public string DisplayName { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;
        }
        #endregion

        public Home(bool firstLoad)
        {
            InitializeComponent();
            this.Width = 370;
            this.Height = 420;
            isFirstLoad = firstLoad;
            //sgList = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.SecurityGroups;

            if (isFirstLoad == true)
            {
                isLoggedOut = false;
            }
            ShowServices();
            BindClientGroup();
            this.Uid = Guid.NewGuid().ToString();
            FrameworkUAS.Object.AppData.myAppData.ParentWindowUid = this.Uid;

            lbUser.Content = "Welcome - " + FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.FullName;

            #region Activity Monitor
            InputManager.Current.PreProcessInput += OnActivity;
            _activityTimer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(120), IsEnabled = true };
            _activityTimer.Tick += OnInactivity;
            _closeActivityTimer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(1), IsEnabled = true };
            _closeActivityTimer.Tick += OnCloseActivity;
            _closeActivityTimer.Stop();
            #endregion

            if (TaskbarItemInfo != null && TaskbarItemInfo.Description != null)
            {
                TaskbarItemInfo.Description = AppName;
                if (AppName == "AMS Desktop QA")
                    TaskbarItemInfo.Overlay = (System.Windows.Media.DrawingImage) this.FindResource("QAImage");
                else if (AppName == "AMS Desktop BugFix")
                    TaskbarItemInfo.Overlay = (System.Windows.Media.DrawingImage) this.FindResource("BugFixImage");
            }
        }
        #region Activity Monitor
        private readonly DispatcherTimer _activityTimer;
        private DispatcherTimer _closeActivityTimer;
        private Point _inactiveMousePosition = new Point(0, 0);

        void OnInactivity(object sender, EventArgs e)
        {
            // remember mouse position
            _inactiveMousePosition = Mouse.GetPosition(HomePanel);
            _closeActivityTimer.Start();

            MessageBoxResult messageResult = MessageBox.Show(this, "The application has been inactive for 2 or more hours.  To continue working, please click OK.  Otherwise the application will close momentarily.");

            if (messageResult == MessageBoxResult.OK)
            {
                _closeActivityTimer.Stop();
                _activityTimer.Stop();
                _activityTimer.Start();
            }
        }

        void OnCloseActivity(object sender, EventArgs e)
        {
            //Close Batches
            ((App) Application.Current).CloseBatches();

            //Close other windows
            ((App) Application.Current).CloseOpenWindows();

            //log user out
            new System.Threading.Tasks.Task(() => { LogUserOut(); }).Start();
            try { this.Close(); }
            catch { }
        }
        void OnActivity(object sender, PreProcessInputEventArgs e)
        {
            InputEventArgs inputEventArgs = e.StagingItem.Input;

            if (inputEventArgs is MouseEventArgs || inputEventArgs is KeyboardEventArgs)
            {
                if (e.StagingItem.Input is MouseEventArgs)
                {
                    MouseEventArgs mouseEventArgs = (MouseEventArgs) e.StagingItem.Input;

                    // no button is pressed and the position is still the same as the application became inactive
                    if (mouseEventArgs.LeftButton == MouseButtonState.Released &&
                        mouseEventArgs.RightButton == MouseButtonState.Released &&
                        mouseEventArgs.MiddleButton == MouseButtonState.Released &&
                        mouseEventArgs.XButton1 == MouseButtonState.Released &&
                        mouseEventArgs.XButton2 == MouseButtonState.Released &&
                        _inactiveMousePosition == mouseEventArgs.GetPosition(HomePanel))
                        return;
                }

                _activityTimer.Stop();
                _activityTimer.Start();
            }
        }

        #endregion
        public void BindClientGroup()
        {
            if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser != null)
            {
                cbClientGroup.ItemsSource = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.ClientGroups.OrderBy(x => x.ClientGroupName).ToList();
                cbClientGroup.DisplayMemberPath = "ClientGroupName";
                cbClientGroup.SelectedValuePath = "ClientGroupID";

                if (isFirstLoad == true)
                {
                    cbClientGroup.SelectedValue = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.ClientGroupID;

                    int clientGroupId = 0;
                    if (cbClientGroup.SelectedValue != null)
                        int.TryParse(cbClientGroup.SelectedValue.ToString(), out clientGroupId);

                    KMPlatform.Entity.ClientGroup cg = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.ClientGroups.Single(x => x.ClientGroupID == clientGroupId);
                    cbClient.ItemsSource = cg.Clients.Where(x => x.IsAMS == true).OrderBy(x => x.DisplayName).ToList();
                    cbClient.DisplayMemberPath = "DisplayName";
                    cbClient.SelectedValuePath = "ClientID";

                    //cbClient.SelectedValue = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
                    cbClient.SelectedIndex = 0;
                    isFirstLoad = false;
                }
            }
        }
        private void cbClientGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser != null)
            {
                if (e.AddedItems.Count > 0 && e.RemovedItems.Count > 0)
                {
                    CloseBatches();
                }
                int clientGroupId = 0;
                KMPlatform.Entity.ClientGroup cg = new KMPlatform.Entity.ClientGroup();
                KMPlatform.Entity.ClientGroup appCG = new KMPlatform.Entity.ClientGroup();
                if (cbClientGroup.SelectedItem != null)
                {
                    cg = cbClientGroup.SelectedItem as KMPlatform.Entity.ClientGroup;
                    clientGroupId = cg.ClientGroupID;
                    appCG = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.ClientGroups.Where(x => x.ClientGroupID == clientGroupId).FirstOrDefault();
                }
                List<KMPlatform.Entity.SecurityGroup> sg = new List<KMPlatform.Entity.SecurityGroup>();

                if (cg.ClientGroupID > 0)
                {
                    FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup = appCG;
                    //sgList = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.SecurityGroups;
                    cbClient.ItemsSource = cg.Clients.Where(x => x.IsAMS == true).OrderBy(x => x.DisplayName).ToList();
                    cbClient.DisplayMemberPath = "DisplayName";
                    cbClient.SelectedValuePath = "ClientID";

                    cbClient.SelectedIndex = 0;

                    //KMPlatform.Entity.Service fulfillment = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentSecurityGroup.Services.FirstOrDefault(x => x.ServiceCode.Equals(KMPlatform.Enums.Services.FULFILLMENT.ToString()));
                    if (KMPlatform.BusinessLogic.User.HasService(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User, KMPlatform.Enums.Services.FULFILLMENT))
                    {
                        // Find spModule
                        DockPanel sp = Core_AMS.Utilities.WPF.FindChild<DockPanel>(this, "spModule");
                        // Find spSearchInfo StackPanel
                        Circulation.Modules.Search spSearch = Core_AMS.Utilities.WPF.FindChild<Circulation.Modules.Search>(sp, "CircSearch");
                        // If spSearchInfo is found then reset.  

                        if (spSearch != null)
                        {
                            spSearch.ClearSearch();
                            spSearch.LoadPublication(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID);

                            //if (cg.ClientGroupName.Equals("All Clients") && FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.IsKmUser == true)
                            //    cbClient.SelectedValue = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients.SingleOrDefault(x => x.ClientCode.Equals("KM")).ClientID;
                        }
                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageError("An unexpected error occurred during client group selection. Please try again. If the problem persists, please contact Customer Support.");
                }
            }
        }
        private void cbClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int clientId = -1;
            KMPlatform.Entity.Client c = new KMPlatform.Entity.Client();
            BackgroundWorker bw = new BackgroundWorker();
            busy.BusyContent = "Loading Client information...";
            if (cbClient.SelectedItem != null)
            {
                if (e.AddedItems.Count > 0 && e.RemovedItems.Count > 0)
                {
                    CloseBatches();
                }
                busy.IsBusy = true;
                c = cbClient.SelectedItem as KMPlatform.Entity.Client;
                //KMPlatform.Entity.Client appClient = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient;
                bw.DoWork += (o, ea) =>
                {
                    clientId = c.ClientID;
                    if (clientId > 0 && !FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties.ContainsKey(clientId))
                    {
                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties.Add(clientId, new FrameworkUAS.Object.ClientAdditionalProperties());
                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[clientId].SourceFilesList = new List<FrameworkUAS.Entity.SourceFile>();
                        //FrameworkUAS.BusinessLogic.SourceFile sfWrk = new FrameworkUAS.BusinessLogic.SourceFile();
                        //FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[clientId].SourceFilesList = sfWrk.Select(clientId, true, false);
                        try
                        {
                            //sourceFileW = new FrameworkServices.ServiceClient<UAS_WS.Interface.ISourceFile>();
                            var sfResp = sourceFileW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, clientId, true, false);
                            if (sfResp.Result != null)
                                FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[clientId].SourceFilesList = sfResp.Result;
                        }
                        catch { }
                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[clientId].ClientCustomProceduresList = new List<FrameworkUAS.Entity.ClientCustomProcedure>();
                        var ccpResp = clientCustomW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, clientId);
                        if (ccpResp != null)
                            FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[clientId].ClientCustomProceduresList = ccpResp.Result;
                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[clientId].ClientFtpDirectoriesList = new List<FrameworkUAS.Entity.ClientFTP>();
                        var ftpResp = clientftpW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, clientId);
                        if (ftpResp != null)
                            FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[clientId].ClientFtpDirectoriesList = ftpResp.Result;
                        List<FrameworkUAD.Entity.Product> pubs = productW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, c.ClientConnections).Result;
                        if (pubs != null)
                        {
                            foreach (FrameworkUAD.Entity.Product p in pubs)
                            {
                                KMPlatform.Object.Product pub = new KMPlatform.Object.Product(p.PubID, p.PubName, p.PubCode, clientId, p.IsActive, p.AllowDataEntry, p.IsUAD, p.IsCirc, p.UseSubGen);

                                if (!c.Products.Contains(pub))
                                    c.Products.Add(pub);
                            }
                        }
                        //FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.ClientAdditionalProperties[clientId].Products = productW.Proxy.Select()
                    }
                };
                bw.RunWorkerCompleted += (o, ea) =>
                {
                    busy.IsBusy = false;
                    if (clientId > -1)
                    {
                        if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID != c.ClientID)
                            FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient = c;
                        if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup != null)
                        {
                            if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient == null)
                            {
                                Core_AMS.Utilities.WPF.MessageError("An unexpected error occurred during client selection. Please try again. If the problem persists, please contact Customer Support.");
                                return;
                            }
                            int sgid = -1;
                            if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserClientSecurityGroupMaps != null && !FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.IsPlatformAdministrator)
                            {
                                int userID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                                KMPlatform.Entity.UserClientSecurityGroupMap ucsgm = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserClientSecurityGroupMaps.FirstOrDefault(x => x.ClientID == clientId && x.UserID == FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID);
                                if (ucsgm != null)
                                    sgid = ucsgm.SecurityGroupID;
                                if (sgid > -1 && sgid != FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentSecurityGroup.SecurityGroupID) //&& FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentSecurityGroup == null)
                                    FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentSecurityGroup = securityGrpW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey,
                                        userID, clientId, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.IsKMStaff).Result;
                                //sgList.FirstOrDefault(x => x.SecurityGroupID == sgid);
                            }
                            else if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserClientSecurityGroupMaps == null)
                            {
                                Core_AMS.Utilities.WPF.MessageError("An unexpected error occurred during client selection. Please try again. If the problem persists, please contact Customer Support.");
                                return;
                            }
                        }
                        else
                        {
                            Core_AMS.Utilities.WPF.MessageError("An unexpected error occurred during client selection. Please try again. If the problem persists, please contact Customer Support.");
                            return;
                        }
                    }

                    KMPlatform.Entity.Service fulfillment = null;
                    if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentSecurityGroup != null)
                        fulfillment = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentSecurityGroup.Services.SingleOrDefault(x => x.ServiceCode.Equals(KMPlatform.Enums.Services.FULFILLMENT.ToString(), StringComparison.CurrentCultureIgnoreCase));
                    else
                    {
                        Core_AMS.Utilities.WPF.MessageError("An unexpected error occurred during client selection. Please try again. If the problem persists, please contact Customer Support.");
                        return;
                    }

                    #region Update various pages if they are open
                    // Clear search page and select appropriate publications
                    if (KMPlatform.BusinessLogic.User.HasService(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User, KMPlatform.Enums.Services.FULFILLMENT) && cbClient.SelectedValue != null)
                    {
                        if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.HasPaid == true && FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.UseSubGen == true)
                        {
                            FrameworkServices.ServiceClient<SubGen_WS.Interface.ISubGenUtils> sgu = FrameworkServices.ServiceClient.SubGen_SubGenUtilsClient();
                            string appEnviron = System.Configuration.ConfigurationManager.AppSettings["AppEnvironment"].ToString();
                            if (appEnviron.Equals("DEV", StringComparison.CurrentCultureIgnoreCase) || appEnviron.Equals("QA", StringComparison.CurrentCultureIgnoreCase))
                                FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.SubGenLoginToken = sgu.Proxy.GetTestingLoginToken(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey,
                                    FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User, false).Result;
                            else
                                FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.SubGenLoginToken = sgu.Proxy.GetLoginToken(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey,
                                    FrameworkSubGen.Entity.Enums.GetClient(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.DisplayName), FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User, false).Result;
                        }

                        // Find spModule
                        DockPanel sp = this.FindName("spModule") as DockPanel;
                        // Find spSearchInfo StackPanel
                        if (sp == null)
                        {
                            Core_AMS.Utilities.WPF.MessageError("An unexpected error occurred during client selection. Please try again. If the problem persists, please contact Customer Support.");
                            return;
                        }
                        Circulation.Modules.Search spSearch = Core_AMS.Utilities.WPF.FindChild<Circulation.Modules.Search>(sp, "CircSearch");
                        FilterControls.FilterWrapper spReports = Core_AMS.Utilities.WPF.FindChild<FilterControls.FilterWrapper>(sp, "CircReports");

                        // If spSearchInfo is found then reset.  
                        if (spSearch != null)
                        {
                            //if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients.Exists(x => x.ClientCode.Equals("KM")) == true)
                            //{
                            //    KMPlatform.Entity.Client cc = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.Clients.FirstOrDefault(x => x.ClientCode.Equals("KM"));
                            //    if (cc != null && clientId == cc.ClientID)
                            //        spSearch.ClearPublication();
                            //}

                            //spSearch.ClearSearch();
                            //if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient != null)
                            //    spSearch.LoadPublication(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID);
                            //else
                            //{
                            //    Core_AMS.Utilities.WPF.MessageError("An unexpected error occurred during client selection. Please try again. If the problem persists, please contact Customer Support.");
                            //    return;
                            //}
                            spModule.Children.Clear();
                            Circulation.Modules.Search circSearch = new Circulation.Modules.Search();
                            circSearch.Name = "CircSearch";
                            spModule.Children.Add(circSearch);
                            Application.Current.MainWindow.Title = "AMS - Circulation - Search";

                        }
                        else if (spReports != null)
                        {
                            Application.Current.MainWindow.Title = "AMS - Circulation - Circulation Explorer";
                            spModule.Children.Clear();
                            WpfControls.DataFetcher df = new WpfControls.DataFetcher(true, false);
                            FilterControls.FilterWrapper explore = new FilterControls.FilterWrapper(df);
                            explore.Name = "CircReports";
                            //Circulation_Explorer.Modules.Home explore = new Circulation_Explorer.Modules.Home(FrameworkUAS.Object.AppData.myAppData);
                            spModule.Children.Add(explore);
                        }
                        else
                        {
                            StackPanel spControls = Core_AMS.Utilities.WPF.FindChild<StackPanel>(sp, "spControls");
                            if (spControls != null)
                            {
                                UserControl uc = spControls.FindChildByType<UserControl>();

                                if (uc != null)
                                {
                                    Type userControlType = uc.GetType();
                                    var ctor = userControlType.GetConstructors()[0];
                                    var defaultValues = ctor.GetParameters().Select(p => p.DefaultValue).ToArray();
                                    UserControl instance = (UserControl) Activator.CreateInstance(userControlType, defaultValues);
                                    spControls.Children.Clear();
                                    spControls.Children.Add(instance);
                                }
                            }
                        }
                        if (Application.Current.Windows.Count > 1)
                        {
                            foreach (Window w in Application.Current.Windows)
                            {
                                Type t = w.GetType();
                                if (t.FullName.Equals("Circulation.Windows.Popout")) //If a Popout window with History is open, we need to reset it.
                                {
                                    DockPanel spModule = w.FindName("spModule") as DockPanel;
                                    spModule.Children.Clear();
                                    Circulation.Modules.History history = new Circulation.Modules.History();
                                    spModule.Children.Add(history);
                                }
                            }
                        }
                        else if (sp != null)
                        {
                            List<UserControl> uc = sp.ChildrenOfType<UserControl>().ToList();
                            foreach (UserControl uc2 in uc)
                            {
                                if (uc != null && uc2.Name == "ucOpenClose")
                                {
                                    sp.Children.Clear();
                                    Circulation.Modules.OpenClose oc = new Circulation.Modules.OpenClose();
                                    sp.Children.Add(oc);
                                }
                            }
                        }

                        #endregion
                    }
                };
                bw.RunWorkerAsync();
            }
        }
        private void CloseBatches()
        {
            FrameworkServices.ServiceClient<UAD_WS.Interface.IBatch> worker = FrameworkServices.ServiceClient.UAD_BatchClient();
            List<FrameworkUAD.Entity.Batch> openBatches = worker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID, true, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;

            try
            {
                if (openBatches.Count > 0)
                    worker.Proxy.CloseBatches(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            }
            catch (Exception ex)
            {
                FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
                Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
                KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                int logClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
                string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                alClient.Proxy.LogCriticalError(accessKey, formatException, this.GetType().Name.ToString() + ".OnCloseActivity", app, string.Empty, logClientId);
            }

            foreach (FrameworkUAD.Entity.Batch b in openBatches)
            {
                FrameworkUAS.Object.Batch batchInList = FrameworkUAS.Object.AppData.myAppData.BatchList.Where(x => x.BatchID == b.BatchID).FirstOrDefault();
                if (batchInList != null)
                    FrameworkUAS.Object.AppData.myAppData.BatchList.Remove(batchInList);

                MessageBox.Show("Your batches have been finalized.");
            }
        }

        public void ShowServices()
        {
            isShowServiceScreen = true;
            spClientGroupClient.Visibility = System.Windows.Visibility.Hidden;

            #region Window Setup
            homeMenu.Visibility = Visibility.Collapsed;
            HomeLogo.Visibility = Visibility.Visible;
            grdApps.Visibility = System.Windows.Visibility.Visible;

            this.Width = 520;//380
            this.Height = 440;//420

            TitleBar.Style = this.FindResource("GrayBar") as Style;
            spModule.Visibility = Visibility.Collapsed;

            TitleBarImage.Visibility = Visibility.Collapsed;
            this.SystemBanner.Source = null;

            Style imgstyle = this.FindResource("AMSLogoWindowHome") as Style;
            HomeLogo.Style = imgstyle;
            HomeLogo.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            HomeTop.Style = this.FindResource("HomeTopGrid") as Style;
            #endregion

            Apps.Clear();
            foreach (KMPlatform.Entity.Service s in FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentSecurityGroup.Services)
            {
                foreach (KMPlatform.Entity.Application a in s.Applications)
                {
                    if (a.ApplicationName.Equals(KMPlatform.BusinessLogic.Enums.Applications.Circulation.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (!Apps.Select(x => x.DisplayName).Contains(a.ApplicationName))
                            Apps.Add(new ApplicationIcon(a.ApplicationID, "/ImageLibrary;Component/Images/AppSystemLogos/CirculationNew256.png", a.ApplicationName));
                    }
                    else if (a.ApplicationName.Equals(KMPlatform.BusinessLogic.Enums.Applications.UAD.ToString().Replace("_", " "), StringComparison.CurrentCultureIgnoreCase) &&
                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.IsKMStaff)
                    {
                        if (!Apps.Select(x => x.DisplayName).Contains("Unified Audience Database"))
                            Apps.Add(new ApplicationIcon(a.ApplicationID, "/ImageLibrary;Component/Images/AppSystemLogos/UADNew256.png", "Unified Audience Database"));
                    }
                    else if (a.ApplicationName.Equals(KMPlatform.BusinessLogic.Enums.Applications.Control_Center.ToString().Replace("_", " "), StringComparison.CurrentCultureIgnoreCase) &&
                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.IsKMStaff)
                    {
                        if (!Apps.Select(x => x.DisplayName).Contains(a.ApplicationName))
                            Apps.Add(new ApplicationIcon(a.ApplicationID, "/ImageLibrary;Component/Images/AppSystemLogos/ControlCenterNew256.png", a.ApplicationName));
                    }
                    else if (a.ApplicationName.Equals(KMPlatform.BusinessLogic.Enums.Applications.Email.ToString().Replace("_", " "), StringComparison.CurrentCultureIgnoreCase) &&
                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.IsKMStaff)
                    {
                        if (!Apps.Select(x => x.DisplayName).Contains("Email Marketing"))
                            Apps.Add(new ApplicationIcon(a.ApplicationID, "/ImageLibrary;Component/Images/AppSystemLogos/Email256.png", "Email Marketing"));
                    }
                    else if (a.ApplicationName.Equals(KMPlatform.BusinessLogic.Enums.Applications.Forms.ToString().Replace("_", " "), StringComparison.CurrentCultureIgnoreCase) &&
                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.IsKMStaff)
                    {
                        if (!Apps.Select(x => x.DisplayName).Contains(a.ApplicationName))
                            Apps.Add(new ApplicationIcon(a.ApplicationID, "/ImageLibrary;Component/Images/AppSystemLogos/Forms256.png", a.ApplicationName));
                    }
                    else if (a.ApplicationName.Equals(KMPlatform.BusinessLogic.Enums.Applications.Profile_Manager.ToString().Replace("_", " "), StringComparison.CurrentCultureIgnoreCase) &&
                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.IsKMStaff)
                    {
                        if (!Apps.Select(x => x.DisplayName).Contains(a.ApplicationName))
                            Apps.Add(new ApplicationIcon(a.ApplicationID, "/ImageLibrary;Component/Images/AppSystemLogos/ManagerNew256.png", a.ApplicationName));
                    }
                }
            }
        }

        public Button AddSystemButton(KMPlatform.Entity.Application app)
        {
            Button appBtn = new Button();
            appBtn.Click += AppBtn_Click;

            appBtn.Name = "btn" + app.ApplicationName.Replace(" ", "");
            Style style = this.FindResource(app.ApplicationName) as Style;
            appBtn.Style = style;
            appBtn.CommandParameter = app.ApplicationID;

            return appBtn;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Unlock the subscriber record based on userid (circ)
            FrameworkServices.ServiceClient<UAD_WS.Interface.IProductSubscription> subscriberWorker = FrameworkServices.ServiceClient.UAD_ProductSubscriptionClient();
            FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> productWorker = FrameworkServices.ServiceClient.UAD_ProductClient();
            FrameworkUAS.Service.Response<bool> saveResp = new FrameworkUAS.Service.Response<bool>();

            if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections.ClientLiveDBConnectionString.Length > 0 ||
                   FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections.ClientTestDBConnectionString.Length > 0)
            {
                subscriberWorker.Proxy.UpdateLock(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, 0, false, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            }
            for (int i = 0; i < FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.OpenedClients.Count; i++)
            {
                KMPlatform.Entity.Client c = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.OpenedClients[i];
                saveResp = productWorker.Proxy.UpdateLock(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, c.ClientConnections, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID);
                if (saveResp != null && saveResp.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.OpenedClients.RemoveAt(i);
            }

            //log user out
            new System.Threading.Tasks.Task(() => { LogUserOut(); }).Start();

            foreach (Window w in Application.Current.Windows)
            {
                try
                {
                    if (w != this)
                        w.Close();
                }
                catch { }
            }
        }

        #region Theme Selector
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.T && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                ThemeSelector ts = new ThemeSelector();
                ts.Show();
            }
        }
        #endregion

        private void Window_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        /// <summary>
        /// x button in upper right corner of window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> productWorker = FrameworkServices.ServiceClient.UAD_ProductClient();
            FrameworkServices.ServiceClient<UAD_WS.Interface.IBatch> worker = FrameworkServices.ServiceClient.UAD_BatchClient();
            List<FrameworkUAD.Entity.Batch> openBatches = worker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID, true, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;

            FrameworkUAS.Service.Response<bool> saveResp = new FrameworkUAS.Service.Response<bool>();
            for (int i = 0; i < FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.OpenedClients.Count; i++)
            {
                KMPlatform.Entity.Client c = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.OpenedClients[i];
                saveResp = productWorker.Proxy.UpdateLock(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, c.ClientConnections, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID);
                if (saveResp != null && saveResp.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.OpenedClients.RemoveAt(i);
            }

            if (FrameworkUAS.Object.AppData.myAppData.OpenWindowCount > 0)
            {
                foreach (Window win in Application.Current.Windows)
                {
                    if (win.GetType() == typeof(Circulation.Windows.Popout))
                        win.Close();
                }
            }

            if (openBatches != null && openBatches.Count > 0)
            {
                MessageBoxResult r = Core_AMS.Utilities.WPF.MessageResult("User cannot log out until open batches are finalized.  Clicking Yes will automatically close your open batches.  Clicking No will require you to manually close open batches before logging out.", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, "Finalize Open Batches");
                if (r == MessageBoxResult.Yes)
                {
                    foreach (FrameworkUAD.Entity.Batch b in openBatches)
                    {
                        b.IsActive = false;
                        b.DateFinalized = DateTime.Now;
                        worker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, b, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                        FrameworkUAS.Object.Batch batchInList = FrameworkUAS.Object.AppData.myAppData.BatchList.Where(x => x.BatchID == b.BatchID).FirstOrDefault();
                        if (batchInList != null)
                        {
                            FrameworkUAS.Object.AppData.myAppData.BatchList.Remove(batchInList);
                        }
                    }

                    if (isShowServiceScreen == false)
                        returnHome(false);
                    else
                        this.Close();
                }
                else
                {
                    //Open new window for History?
                    Circulation.Modules.History circHistory = new Circulation.Modules.History(true);
                    Circulation.Helpers.Common.OpenCircPopoutWindow(circHistory);
                    return;
                }
            }
            else
            {
                if (isShowServiceScreen == false)
                    returnHome(false);
                else
                    this.Close();
            }
        }

        public void LogUserOut()
        {
            if (isLoggedOut == false)
            {
                FrameworkServices.ServiceClient<UAS_WS.Interface.IUserAuthorizationLog> ualWorker = FrameworkServices.ServiceClient.UAS_UserAuthorizationLogClient();
                ualWorker.Proxy.LogOut(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.UserAuthLogId);
                isLoggedOut = true;
            }
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

        private void AppBtn_Click(object sender, RoutedEventArgs e)
        {
            int appID = -1;
            Button appBtn = (Button) sender;
            int.TryParse(appBtn.CommandParameter.ToString(), out appID);

            e.Handled = true;

            if (appID > 0)
            {
                spClientGroupClient.Visibility = System.Windows.Visibility.Visible;

                KMPlatform.Entity.Application app = new KMPlatform.Entity.Application();
                foreach (KMPlatform.Entity.Service s in FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentSecurityGroup.Services)
                {
                    if (s.Applications.Exists(x => x.ApplicationID == appID))
                    {
                        app = s.Applications.Single(x => x.ApplicationID == appID);
                        break;
                    }
                }

                homeMenu.Visibility = System.Windows.Visibility.Visible;
                grdApps.Visibility = System.Windows.Visibility.Collapsed;
                spModule.Visibility = Visibility.Visible;

                homeMenu.LoadMenu(app);

                //set menu to just button clicked
                switch (app.ApplicationName)
                {
                    case "Control Center":
                        isShowServiceScreen = false;
                        spModule.Children.Clear();
                        ControlCenter.Modules.Home ccHome = new ControlCenter.Modules.Home();
                        this.SystemBanner.Source = new BitmapImage(new Uri(app.IconFullName, UriKind.RelativeOrAbsolute));
                        spModule.Children.Add(ccHome);
                        break;
                    case "Circulation":
                        //get SugGen Login credentials
                        //if a KM user don't login in yet
                        //if user is not KM and client has paid records log into subgen - get login token : if client not have paid records do not log into subgen

                        //if(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.IsKmStaff == false && FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.HasPaid == true)
                        //{
                        //    if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.HasPaid == true)
                        //    {
                        //        //FrameworkSubGen.SubGenUtils sgu = new FrameworkSubGen.SubGenUtils();
                        //        FrameworkServices.ServiceClient<SubGen_WS.Interface.ISubGenUtils> sgu = FrameworkServices.ServiceClient.SubGen_SubGenUtilsClient();
                        //        string appEnviron = System.Configuration.ConfigurationManager.AppSettings["AppEnvironment"].ToString();
                        //        if (appEnviron.Equals("DEV", StringComparison.CurrentCultureIgnoreCase))
                        //            FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.SubGenLoginToken = sgu.Proxy.GetTestingLoginToken(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, 
                        //                FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User, false).Result;
                        //        else
                        //            FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.SubGenLoginToken = sgu.Proxy.GetLoginToken(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey,
                        //                FrameworkSubGen.Entity.Enums.GetClient(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.DisplayName), FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User, false).Result;
                        //    }
                        //}

                        isShowServiceScreen = false;
                        spModule.Children.Clear();
                        Circulation.Modules.Home cfhome = new Circulation.Modules.Home();
                        this.SystemBanner.Source = new BitmapImage(new Uri(app.IconFullName, UriKind.RelativeOrAbsolute));
                        spModule.Children.Add(cfhome);
                        break;
                    //case "Profile Manager":
                    //    isShowServiceScreen = false;
                    //    spModule.Children.Clear();
                    //    ProfileManager.Modules.Home pmHome = new ProfileManager.Modules.Home(FrameworkUAS.Object.AppData.myAppData);
                    //    this.SystemBanner.Source = new BitmapImage(new Uri(app.IconFullName, UriKind.RelativeOrAbsolute));
                    //    spModule.Children.Add(pmHome);
                    //    break;
                    case "UAD":
                        isShowServiceScreen = false;
                        spModule.Children.Clear();
                        Modules.UADHome uadhome = new Modules.UADHome(FrameworkUAS.Object.AppData.myAppData);
                        this.SystemBanner.Source = new BitmapImage(new Uri(app.IconFullName, UriKind.RelativeOrAbsolute));
                        spModule.Children.Add(uadhome);
                        break;
                }
            }
            this.Width = 1050;
            this.Height = 780;
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;

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
            grdApps.Visibility = System.Windows.Visibility.Collapsed;
            HomeLogo.Visibility = Visibility.Collapsed;
        }
        public static System.Drawing.Rectangle GetScreenFrom(Window window)
        {
            WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);
            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.FromHandle(windowInteropHelper.Handle);
            return screen.WorkingArea;
        }

        #region AppData access
        public FrameworkUAS.Object.AppData GetAppData()
        {
            return FrameworkUAS.Object.AppData.myAppData;
        }
        public void UpdateAppData(FrameworkUAS.Object.AppData myAppData)
        {
            FrameworkUAS.Object.AppData.myAppData = myAppData;
        }
        #endregion

        public class AutoClosingMessageBox
        {
            System.Threading.Timer _timeoutTimer;
            string _caption;
            AutoClosingMessageBox(string text, string caption, int timeout)
            {
                _caption = caption;
                _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                    null, timeout, System.Threading.Timeout.Infinite);
                MessageBox.Show(text, caption);
            }
            public static void Show(string text, string caption, int timeout)
            {
                new AutoClosingMessageBox(text, caption, timeout);
            }
            void OnTimerElapsed(object state)
            {
                IntPtr mbWnd = FindWindow(null, _caption);
                if (mbWnd != IntPtr.Zero)
                    SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                _timeoutTimer.Dispose();
            }
            const int WM_CLOSE = 0x0010;
            [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
            static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
            [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        }

        public void returnHome(bool closeApp)
        {
            if (closeApp == false)
            {
                Window w = Core_AMS.Utilities.WPF.GetMainWindow();
                Windows.Home home = (Windows.Home) w;
                DockPanel spModule = Core_AMS.Utilities.WPF.FindChild<DockPanel>(home, "spModule");
                spModule.Children.Clear();

                home.Width = 380;
                home.Height = 420;
                home.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;

                if (System.Windows.Forms.SystemInformation.MonitorCount > 1)
                {
                    System.Drawing.Rectangle workingArea = GetScreenFrom(home);
                    home.Left = (workingArea.Width - home.Width) / 2 + workingArea.Left;
                    home.Top = (workingArea.Height - home.Height) / 2 + workingArea.Top;
                }
                else
                {
                    home.Left = (SystemParameters.WorkArea.Width - home.Width) / 2 + SystemParameters.WorkArea.Left;
                    home.Top = (SystemParameters.WorkArea.Height - home.Height) / 2 + SystemParameters.WorkArea.Top;
                }
                home.ShowServices();
            }
        }

        private void ReLoadProduct()
        {
            BackgroundWorker bw = new BackgroundWorker();

            FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> productWorker = FrameworkServices.ServiceClient.UAD_ProductClient();
            FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> productResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
            List<FrameworkUAD.Entity.Product> prdList = new List<FrameworkUAD.Entity.Product>();

            bw.DoWork += (o, ea) =>
            {
                productResponse = productWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            };

            bw.RunWorkerCompleted += (o, ea) =>
            {
                if (productResponse.Result != null && productResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    prdList = productResponse.Result;

                foreach (var p in FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.ClientGroups)
                {
                    foreach (var c in p.Clients)
                    {
                        if (c.ClientID == FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID)
                        {
                            c.Products = new List<KMPlatform.Object.Product>();

                            foreach (var pl in prdList)
                            {
                                c.Products.Add(new KMPlatform.Object.Product
                                {
                                    AllowDataEntry = pl.AllowDataEntry,
                                    ClientID = pl.ClientID,
                                    IsActive = pl.IsActive,
                                    IsCirc = pl.IsCirc,
                                    IsUAD = pl.IsUAD,
                                    ProductCode = pl.PubCode,
                                    ProductID = pl.PubID,
                                    ProductName = pl.PubName
                                });
                            }
                        }
                    }
                }
            };

            bw.RunWorkerAsync();
        }

        public Dictionary<string, string> BaseDirectories
        {
            get { return BaseDirs.GetBaseDirectories(); }
        }

        private FrameworkUAS.Object.UserAuthorization UserAuthorization(string user, string pass)
        {
            FrameworkUAS.Object.UserAuthorization uAuth = new FrameworkUAS.Object.UserAuthorization();
            FrameworkServices.ServiceClient<UAS_WS.Interface.IServerVariable> svClient = FrameworkServices.ServiceClient.UAS_ServerVariableClient();
            FrameworkServices.ServiceClient<UAS_WS.Interface.IUserAuthorization> uaClient = FrameworkServices.ServiceClient.UAS_UserAuthorizationClient();

            KMPlatform.Object.ServerVariable sv = new KMPlatform.Object.ServerVariable();
            FrameworkUAS.Service.Response<KMPlatform.Object.ServerVariable> svResponse = new FrameworkUAS.Service.Response<KMPlatform.Object.ServerVariable>();
            svResponse = svClient.Proxy.GetServerVariables();
            if (svResponse.Result != null && svResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                sv = svResponse.Result;
            else
                Core_AMS.Utilities.WPF.MessageServiceError();

            svClient.Close();

            string ipAddress = string.Empty;
            if (!string.IsNullOrEmpty(sv.REMOTE_ADDR))
                ipAddress = sv.REMOTE_ADDR;
            else
                ipAddress = uaClient.Proxy.GetIpAddress().Result;

            string appVersion = string.Empty;
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
                if (ad != null)
                {
                    if (ad.CurrentVersion != null)
                        appVersion = ad.CurrentVersion.ToString();
                }
            }
            else
                appVersion = "DEV";

            FrameworkUAS.Object.Encryption ec = new FrameworkUAS.Object.Encryption();
            ec.PlainText = pass;
            FrameworkUAS.BusinessLogic.Encryption e = new FrameworkUAS.BusinessLogic.Encryption();
            ec = e.Encrypt(ec);

            FrameworkUAS.Service.Response<KMPlatform.Object.UserAuthorization> uaResponse = new FrameworkUAS.Service.Response<KMPlatform.Object.UserAuthorization>();
            uaResponse = uaClient.Proxy.Login(user, ec.EncryptedText, ec.SaltValue, "AMS", ipAddress, sv, appVersion, false);
            if (uaResponse.Result != null)
                uAuth = new FrameworkUAS.Object.UserAuthorization(uaResponse.Result);
            else
            {
                Core_AMS.Utilities.WPF.MessageServiceError();
            }

            if (uAuth.IsAuthenticated == true)
            {
                FrameworkServices.ServiceClient<UAS_WS.Interface.IApplication> appB = FrameworkServices.ServiceClient.UAS_ApplicationClient();
                List<KMPlatform.Entity.Application> apps = appB.Proxy.Select(uAuth.AuthAccessKey).Result;
                if (apps != null)
                {
                    foreach (KMPlatform.Entity.Application app in apps)
                    {
                        if (!Directory.Exists(BaseDirectories["Applications"] + "\\" + app.ApplicationName))
                            Directory.CreateDirectory(BaseDirectories["Applications"] + "\\" + app.ApplicationName);
                    }
                }
            }
            return uAuth;
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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

        private void HomeTop_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                if (max)
                {
                    max = false;
                    this.WindowState = System.Windows.WindowState.Normal;
                    e.Handled = true;
                }
                else
                {
                    max = true;
                    this.WindowState = System.Windows.WindowState.Maximized;
                    e.Handled = true;
                }
            }
        }
    }
}
