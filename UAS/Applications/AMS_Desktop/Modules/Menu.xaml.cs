using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Circulation.Helpers;
using System.Windows.Interop;
using System.Windows.Navigation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Collections.ObjectModel;

namespace AMS_Desktop.Modules
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : UserControl
    {
        private KMPlatform.Entity.Application CurrentApp;
        private Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
        private ObservableCollection<MenuContainer> _menus = new ObservableCollection<MenuContainer>();
        public ObservableCollection<MenuContainer> Menus { get { return _menus; } }
        public class MenuContainer
        {
            public MenuContainer(int id, int appID, string name, string url, List<KMPlatform.Entity.Menu> items)
            {
                this.MenuID = id;
                this.ApplicationID = appID;
                this.MenuName = name.ToUpper();
                this.URL = url;
                if (items != null)
                {
                    this.ChildItems = new List<MenuContainer>();
                    items.ForEach(x => { this.ChildItems.Add(new MenuContainer(x.MenuID, appID, x.MenuName, x.URL, null)); });
                }
            }
            public int MenuID { get; set; }
            public int ApplicationID { get; set; }
            public string URL { get; set; }
            public string MenuName { get; set; }
            public List<MenuContainer> ChildItems { get; set; }
        }
        public Menu()
        {
            InitializeComponent();
        }

        public void LoadMenu(KMPlatform.Entity.Application app)
        {
            Menus.Clear();
            Menus.Add(new MenuContainer(-1, app.ApplicationID, "HOME", "HOME", null));
            if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser != null && app.Menus != null)
            {
                foreach (KMPlatform.Entity.Menu m in app.Menus.Where(x => x.IsParent).OrderBy(x => x.MenuOrder))
                {
                    List<KMPlatform.Entity.Menu> childList = app.Menus.Where(x => x.IsActive == true && x.IsParent == false && x.ParentMenuID == m.MenuID).OrderBy(x => x.MenuOrder).ToList();
                    Menus.Add(new MenuContainer(m.MenuID, app.ApplicationID, m.MenuName, m.URL, childList));
                }
                CurrentApp = app;
                //List<MenuItem> remIndexes = new List<MenuItem>();
                //foreach (MenuItem menItem in menu.Items)
                //{
                //    if (menItem.Name != "miHome")
                //        remIndexes.Add(menItem);
                //}
                //foreach (MenuItem i in remIndexes)
                //    menu.Items.Remove(i);

                //List<KMPlatform.Entity.Menu> parentList = app.Menus.Where(x => x.IsActive == true && x.IsParent == true).OrderBy(x => x.MenuOrder).ToList();
                //List<KMPlatform.Entity.Menu> childList = app.Menus.Where(x => x.IsActive == true && x.IsParent == false).ToList();

                //foreach (KMPlatform.Entity.Menu m in parentList)
                //{
                //    if (m.MenuName == "Circulation")
                //        continue;

                //    MenuItem mi = new MenuItem();
                //    if (m.IsParent == false)
                //        mi = GetMenuItem(m);
                //    else
                //        mi = GetMenuItem(m, childList);

                //    menu.Items.Add(mi);
                //}
            }

            try
            {
                MenuContainer mcHelp = new MenuContainer(-1, app.ApplicationID, "HELP", "HELP", null);
                mcHelp.ChildItems = new List<MenuContainer>();
                mcHelp.ChildItems.Add(new MenuContainer(-1, app.ApplicationID, "CHECK FOR UPDATE", "CHECK FOR UPDATE", null));
                mcHelp.ChildItems.Add(new MenuContainer(-1, app.ApplicationID, "RESTORE PREVIOUS VERSION", "RESTORE PREVIOUS VERSION", null));
                Menus.Add(mcHelp);
            }
            catch(Exception ex)
            {
                string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            }
            Menus.Add(new MenuContainer(-1, app.ApplicationID, "LOG OUT", "LOG OUT", null));
            //menu.ItemsSource = Menus;
            //AddLogOff();
        }
        //private MenuItem GetMenuItem(KMPlatform.Entity.Menu m, bool isChild = false)
        //{
        //    MenuItem mi = new MenuItem();
        //    mi.Name = "mi" + m.MenuName.Replace(" ", "_");
        //    mi.Click += MenuItem_Click;
        //    mi.CommandParameter = m.MenuID;
        //    mi.Tag = m.URL;

        //    StackPanel sp = new StackPanel();
        //    sp.Orientation = Orientation.Horizontal;
        //    sp.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
        //    if (!string.IsNullOrEmpty(m.ImagePath))
        //    {
        //        Image img = new System.Windows.Controls.Image
        //        {
        //            Source = new BitmapImage(new Uri(m.ImagePath, UriKind.Relative))
        //        };
        //        sp.Children.Add(img);
        //    }
        //    ContentPresenter cp = new ContentPresenter();
        //    cp.Content = m.MenuName.ToUpper();

        //    sp.Children.Add(cp);

        //    if (isChild)
        //    {
        //        Style style = this.FindResource("SubMenuItem") as Style;
        //        mi.Style = style;
        //    }

        //    mi.Header = sp;
        //    return mi;
        //}
        //private MenuItem GetMenuItem(KMPlatform.Entity.Menu m, List<KMPlatform.Entity.Menu> allChildren)
        //{
        //    List<KMPlatform.Entity.Menu> pmChildren = allChildren.Where(x => x.ParentMenuID == m.MenuID).ToList();

        //    MenuItem mi = new MenuItem();
        //    mi.Name = "mi" + m.MenuName.Replace(" ", "_");
        //    mi.Click += MenuItem_Click;
        //    mi.CommandParameter = m.MenuID;
        //    mi.Tag = m.URL;

        //    StackPanel sp = new StackPanel();
        //    sp.Orientation = Orientation.Horizontal;
        //    sp.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
        //    if (!string.IsNullOrEmpty(m.ImagePath))
        //    {
        //        Image img = new System.Windows.Controls.Image
        //        {
        //            Source = new BitmapImage(new Uri(m.ImagePath, UriKind.Relative))
        //        };
        //        sp.Children.Add(img);
        //    }
        //    ContentPresenter cp = new ContentPresenter();
        //    cp.Content = m.MenuName.ToUpper();

        //    var setter = new Setter()
        //    {
        //        Property = Control.ForegroundProperty,
        //        Value = Brushes.SteelBlue
        //    };

        //    var trigger = new Trigger()
        //    {
        //        Property = UIElement.IsFocusedProperty,
        //        Value = true,
        //        Setters = { setter },               
        //    };
        //    var style = new Style()
        //    {
        //        Triggers = { trigger },                
        //    };

        //    sp.Children.Add(cp);

        //    Separator sep = new Separator();
        //    sp.Children.Add(sep);

        //    mi.Header = sp;
        //    mi.Style = style;
        //    foreach (KMPlatform.Entity.Menu child in pmChildren)
        //    {
        //        if (child.IsParent == false)
        //            mi.Items.Add(GetMenuItem(child, true));
        //        else
        //            mi.Items.Add(GetMenuItem(child, pmChildren));
        //    }

        //    return mi;
        //}
        private void AddLogOff(List<KMPlatform.Entity.Menu> m)
        {
            MenuItem mi = new MenuItem();
            mi.Name = "miLogOut";
            //mi.Click += LogOut_Click;
            mi.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            mi.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            sp.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            sp.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;

            Image img = new Image();
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri("..\\Images\\user_32xMD.png", UriKind.Relative);
            bi3.EndInit();
            img.Source = bi3;
            img.Height = 20;
            img.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            img.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            sp.Children.Add(img);

            ContentPresenter cp = new ContentPresenter();
            cp.Content = "LOG OUT";
            cp.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            sp.Children.Add(cp);

            mi.Header = sp;

            //if (!menu.Items.Contains(mi))
            //menu.Items.Add(mi);
            //m.Add(mi);
        }

        private void AddLogOff()
        {
            MenuItem mi = new MenuItem();
            mi.Name = "miLogOut";
            mi.Click += LogOut_Click;
            mi.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            mi.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            sp.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            sp.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;

            Image img = new Image();
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri("..\\Images\\user_32xMD.png", UriKind.Relative);
            bi3.EndInit();
            img.Source = bi3;
            img.Height = 20;
            img.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            img.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            sp.Children.Add(img);

            ContentPresenter cp = new ContentPresenter();
            cp.Content = "LOG OUT";
            cp.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            sp.Children.Add(cp);

            mi.Header = sp;

            if (!menu.Items.Contains(mi))
                menu.Items.Add(mi);
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            MenuItem mi = (MenuItem)sender;
            MenuContainer mc = mi.DataContext as MenuContainer;
            //int menuID = 0;
            //int.TryParse(mi.CommandParameter.ToString(), out menuID);
            string menuURL = mc.URL;

            if (CurrentApp.ApplicationName.Equals("UAD"))
            {
                KMPlatform.Entity.Service s = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentSecurityGroup.Services.Where(x => x.Applications.Exists(y => y.ApplicationID == CurrentApp.ApplicationID)).First();
                Windows.Home w = Helpers.AppUtilities.GetMainWindow();
                w.grdApps.Visibility = System.Windows.Visibility.Hidden;
                switch (mc.MenuName)
                {
                    case "DQM":
                        //app = s.Applications.SingleOrDefault(x => x.ApplicationName.Equals("DQM"));
                        //LoadMenu(app);
                        w.spModule.Children.Clear();
                        DQM.Modules.ADMS_FTP_FileUpload dqm = new DQM.Modules.ADMS_FTP_FileUpload();
                        w.spModule.Children.Add(dqm);
                        break;
                    case "FILE MAPPER":
                        //app = s.Applications.SingleOrDefault(x => x.ApplicationName.Equals("File Mapper"));
                        w.spModule.Children.Clear();
                        FileMapperWizard.Modules.MainSelection fmw = new FileMapperWizard.Modules.MainSelection(FrameworkUAS.Object.AppData.myAppData);
                        w.spModule.Children.Add(fmw);
                        break;
                    //case "ADMS DASHBOARD":
                    //    //app = s.Applications.SingleOrDefault(x => x.ApplicationName.Equals("UAD Dashboard"));
                    //    //LoadMenu(app);
                    //    w.spModule.Children.Clear();
                    //    ADMS_Dashboard.Modules.Home mon = new ADMS_Dashboard.Modules.Home(FrameworkUAS.Object.AppData.myAppData);
                    //    w.spModule.Children.Add(mon);
                    //    break;
                    case "REPORTS":
                            Application.Current.MainWindow.Title = "AMS - Circulation - Circulation Explorer";
                            w.spModule.Children.Clear();
                            WpfControls.DataFetcher df = new WpfControls.DataFetcher(true,false);
                            FilterControls.FilterWrapper explore = new FilterControls.FilterWrapper(df);
                            explore.Name = "CircReports";
                            w.spModule.Children.Add(explore);
                        break;
                    #region Data Compare
                    case "DATA COMPARE":
                        //Windows.Home w = Helpers.AppUtilities.GetMainWindow();
                        w.grdApps.Visibility = System.Windows.Visibility.Hidden;
                        w.spModule.Children.Clear();
                        DataCompare.Modules.Home dcHome = new DataCompare.Modules.Home(FrameworkUAS.Object.AppData.myAppData);
                        w.spModule.Children.Add(dcHome);
                        break;
                    #endregion
                    case "HOME":
                    case "LOG OUT":
                        Home_Click();
                        break;
                    case "RESTORE PREVIOUS VERSION":
                        App.RestoreToPreviousVersion();
                        break;
                    case "CHECK FOR UPDATE":
                        App.OnCheckForUpdate();
                        break;
                }

            }
            else
            {
                if (mc.MenuName != null && mc.MenuName != "")
                {
                    Windows.Home w = Helpers.AppUtilities.GetMainWindow();
                    w.grdApps.Visibility = System.Windows.Visibility.Hidden;
                    FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> pubData = FrameworkServices.ServiceClient.UAD_ProductClient();
                    FrameworkUAS.Service.Response<bool> saveResp = new FrameworkUAS.Service.Response<bool>();
                    if (CurrentApp.ApplicationName == "Circulation" && menuURL != "Circulation.Modules.History")
                    {
                        if (!Circulation.Modules.Home.IsLoaded)
                        {
                            Core_AMS.Utilities.WPF.Message("Circulation data is still loading. Please wait for loading to complete before continuing.");
                            return;
                        }
                        for (int i = 0; i < FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.OpenedClients.Count; i++)
                        {
                            KMPlatform.Entity.Client c = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.OpenedClients[i];
                            saveResp = pubData.Proxy.UpdateLock(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, c.ClientConnections, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID);
                            if (saveResp != null && saveResp.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                                FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.OpenedClients.RemoveAt(i);
                        }
                    }

                    switch (menuURL)
                    {
                        case "HOME":
                            Home_Click();
                            break;
                        case "LOG OUT":
                            Home_Click();
                            break;
                        case "RESTORE PREVIOUS VERSION":
                            App.RestoreToPreviousVersion();
                            break;
                        case "CHECK FOR UPDATE":
                            App.OnCheckForUpdate();
                            break;
                        #region DQM
                        case "DQM.Modules.Home":
                            w.spModule.Children.Clear();
                            DQM.Modules.Home dqmHome = new DQM.Modules.Home();
                            w.spModule.Children.Add(dqmHome);
                            break;
                        case "DQM.Modules.DataLoader":
                            w.spModule.Children.Clear();
                            DQM.Modules.DataLoader dqmDataLoader = new DQM.Modules.DataLoader();
                            w.spModule.Children.Add(dqmDataLoader);
                            break;
                        case "DQM.Modules.UAD_FileValidator":
                            w.spModule.Children.Clear();
                            DQM.Modules.UAD_FileValidator dqmFV = new DQM.Modules.UAD_FileValidator();
                            w.spModule.Children.Add(dqmFV);
                            break;
                        case "FileMapperWizard.Modules.FMValidator":
                            w.spModule.Children.Clear();
                            FileMapperWizard.Modules.FMValidator fv = new FileMapperWizard.Modules.FMValidator(FrameworkUAS.Object.AppData.myAppData);
                            w.spModule.Children.Add(fv);
                            break;
                        case "DQM.Modules.ADMS_FTP_FileUpload":
                            w.spModule.Children.Clear();
                            DQM.Modules.ADMS_FTP_FileUpload dqmFU = new DQM.Modules.ADMS_FTP_FileUpload();
                            w.spModule.Children.Add(dqmFU);
                            break;
                        case "DQM.Modules.DataExport":
                            w.spModule.Children.Clear();
                            DQM.Modules.DataExport dqmDE = new DQM.Modules.DataExport();
                            w.spModule.Children.Add(dqmDE);
                            break;
                        case "DQM.Modules.DataImport":
                            w.spModule.Children.Clear();
                            DQM.Modules.DataImport dqmDI = new DQM.Modules.DataImport();
                            w.spModule.Children.Add(dqmDI);
                            break;
                        case "DataImportExport.Modules.UADExport":
                            w.spModule.Children.Clear();
                            DataImportExport.Modules.UADExport uadExport = new DataImportExport.Modules.UADExport();
                            w.spModule.Children.Add(uadExport);
                            break;
                        case "DataImportExport.Modules.UADImport":
                            w.spModule.Children.Clear();
                            DataImportExport.Modules.UADImport uadImport = new DataImportExport.Modules.UADImport();
                            w.spModule.Children.Add(uadImport);
                            break;
                        case "DataImportExport.Modules.UASExport":
                            w.spModule.Children.Clear();
                            DataImportExport.Modules.UASExport uasExport = new DataImportExport.Modules.UASExport();
                            w.spModule.Children.Add(uasExport);
                            break;
                        case "DataImportExport.Modules.UASImport":
                            w.spModule.Children.Clear();
                            DataImportExport.Modules.UASExport uasImport = new DataImportExport.Modules.UASExport();
                            w.spModule.Children.Add(uasImport);
                            break;
                        #endregion
                        #region Circulation
                        case "Circulation.Modules.History":
                            //w.spModule.Children.Clear();
                            Circulation.Modules.History circHistory = new Circulation.Modules.History();
                            Common.OpenCircPopoutWindow(circHistory);
                            //w.spModule.Children.Add(circHistory);
                            break;
                        case "Circulation.Modules.Search":
                            w.spModule.Children.Clear();
                            Circulation.Modules.Search circSearch = new Circulation.Modules.Search();
                            circSearch.Name = "CircSearch";
                            w.spModule.Children.Add(circSearch);
                            Application.Current.MainWindow.Title = "AMS - Circulation - Search";
                            break;
                        case "Circulation.Modules.Home":
                            w.spModule.Children.Clear();
                            Circulation.Modules.Home circHome = new Circulation.Modules.Home();
                            w.spModule.Children.Add(circHome);
                            Application.Current.MainWindow.Title = "AMS - Circulation";
                            break;
                        case "Circulation.Modules.Import":
                            w.spModule.Children.Clear();
                            DataImportExport.Modules.CircImport circulationImport = new DataImportExport.Modules.CircImport();
                            w.spModule.Children.Add(circulationImport);
                            Application.Current.MainWindow.Title = "AMS - Circulation - Circ Import";
                            break;
                        case "Circulation.Modules.Export":
                            w.spModule.Children.Clear();
                            DataImportExport.Modules.CircExport circulationExport = new DataImportExport.Modules.CircExport();
                            w.spModule.Children.Add(circulationExport);
                            Application.Current.MainWindow.Title = "AMS - Circulation - Circ Export";
                            break;
                        case "Circulation.Modules.Batch":
                            w.spModule.Children.Clear();
                            //Circulation.Modules.Batch circBatch = new Circulation.Modules.Batch(Helpers.AppUtilities.myAppDataCirc);
                            //w.spModule.Children.Add(circBatch);
                            break;
                        case "Circulation.Modules.OpenClose":
                            if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.IsKmUser && FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID == 1)
                                MessageBox.Show("Please select a client to view Open Close proccesses");
                            else
                            {
                                w.spModule.Children.Clear();
                                Circulation.Modules.OpenClose openClose = new Circulation.Modules.OpenClose();
                                w.spModule.Children.Add(openClose);
                                Application.Current.MainWindow.Title = "AMS - Circulation - Open Close";
                            }
                            break;
                        case "WpfControls.Reporting.ReportBuilder":
                            if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.IsKmUser && FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID == 1)
                                MessageBox.Show("Please select a client to view Report Builder");
                            else
                            {
                                w.spModule.Children.Clear();
                                WpfControls.Reporting.ReportBuilder rb = new WpfControls.Reporting.ReportBuilder(true,false);
                                w.spModule.Children.Add(rb);
                                Application.Current.MainWindow.Title = "AMS - Circulation - Report Builder";
                            }
                            break;
                        case "Circulation_Explorer.Modules.HomeDemo":
                            if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID != 0)
                            {
                                Application.Current.MainWindow.Title = "AMS - Circulation - Circulation Explorer";
                                w.spModule.Children.Clear();
                                WpfControls.DataFetcher df = new WpfControls.DataFetcher(true,false);
                                FilterControls.FilterWrapper explore = new FilterControls.FilterWrapper(df);
                                explore.Name = "CircReports";
                                //Circulation_Explorer.Modules.Home explore = new Circulation_Explorer.Modules.Home(FrameworkUAS.Object.AppData.myAppData);
                                w.spModule.Children.Add(explore);
                            }
                            else
                                MessageBox.Show("Please select a client.");
                            break;
                        case "Circulation_Explorer.Modules.HomeOld":
                            Application.Current.MainWindow.Title = "AMS - Circulation - Circulation Explorer";
                            w.spModule.Children.Clear();

                            #region Connect to Internet Code
                            if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > 0)
                            {
                                FrameworkServices.ServiceClient<UAS_WS.Interface.IUASBridgeECN> bridgeWorker = FrameworkServices.ServiceClient.UAS_UASBridgeECNClient();
                                List<FrameworkUAS.Entity.UASBridgeECN> bridgeClient = bridgeWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID).Result;
                                if (bridgeClient != null)
                                {
                                    FrameworkUAS.Entity.UASBridgeECN soloBridgeClient = bridgeClient.FirstOrDefault(x => x.ClientID == FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID);
                                    if (soloBridgeClient != null)
                                    {
                                        System.Configuration.Configuration config;
                                        config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                                        string ak = config.AppSettings.Settings["UASMasterAccessKey"].Value.ToString();
                                        try
                                        {
                                            config.Save(ConfigurationSaveMode.Modified);
                                        }
                                        catch (Exception ex)
                                        {
                                            FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
                                            Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
                                            KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                                            int logClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
                                            string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                                            alClient.Proxy.LogCriticalError(accessKey, formatException, this.GetType().Name.ToString() + ".MenuItem_Click", app, string.Empty, logClientId);
                                        }
                                    }
                                    else
                                    {
                                        Core_AMS.Utilities.WPF.Message("User not setup to view reports for that client.", MessageBoxButton.OK, MessageBoxImage.Information, "Invalid Access");
                                    }
                                }
                                else
                                {
                                    Core_AMS.Utilities.WPF.Message("User not setup to view reports.", MessageBoxButton.OK, MessageBoxImage.Information, "Invalid Access");
                                }
                            }
                            else
                            {
                                Core_AMS.Utilities.WPF.Message("No Client Selected.", MessageBoxButton.OK, MessageBoxImage.Information, "Invalid Operation");
                            }
                            #endregion
                            break;
                        case "FileMapperWizard.TileControls.CircSetup":
                            FileMapperWizard.Windows.FMWindow newMappingWindow = new FileMapperWizard.Windows.FMWindow("New Mapping");
                            StackPanel stackPanel = new StackPanel();
                            stackPanel.Children.Clear();
                            KMPlatform.Entity.Client client = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient;
                            FileMapperWizard.Modules.FMUniversal fmu = new FileMapperWizard.Modules.FMUniversal(false, true, client);
                            stackPanel.Children.Add(fmu);
                            newMappingWindow.ReplaceContent(stackPanel);
                            newMappingWindow.Title = "New Mapping";
                            newMappingWindow.SizeToContent = SizeToContent.WidthAndHeight;
                            newMappingWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                            newMappingWindow.ShowDialog();
                            newMappingWindow.Activate();
                            break;
                        case "Circulation.Modules.SubscriptionGenius":
                            if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.UseSubGen)
                            {
                                w.spModule.Children.Clear();
                                Circulation.Modules.SubscriptionGenius subGen = new Circulation.Modules.SubscriptionGenius(KMPlatform.BusinessLogic.Enums.SubGenControls.Reports);
                                Circulation.Windows.PlainPopout pop = new Circulation.Windows.PlainPopout(subGen);
                                pop.Show();
                                //w.spModule.Children.Add(subGen);
                                //Application.Current.MainWindow.Title = "AMS - Circulation - Paid Products";
                            }
                            else
                                Core_AMS.Utilities.WPF.Message("This client has not been set up for paid products.");
                            break;
                        #endregion
                        #region File Mapper Wizard
                        case "FileMapperWizard.Modules.MainSelection":
                            w.spModule.Children.Clear();
                            FileMapperWizard.Modules.MainSelection fmwMainSel = new FileMapperWizard.Modules.MainSelection(FrameworkUAS.Object.AppData.myAppData);
                            w.spModule.Children.Add(fmwMainSel);
                            break;
                        #endregion
                        #region Profile Manager
                        //case "ProfileManager.Modules.Home":
                        //    w.spModule.Children.Clear();
                        //    ProfileManager.Modules.Home pmHome = new ProfileManager.Modules.Home(FrameworkUAS.Object.AppData.myAppData);
                        //    w.spModule.Children.Add(pmHome);
                        //    break;
                        #endregion
                        #region ADMS Dashboard
                        //case "ADMS_Dashboard.Modules.Monitor":
                        //    w.spModule.Children.Clear();
                        //    ADMS_Dashboard.Modules.Home admsDB_Home = new ADMS_Dashboard.Modules.Home(FrameworkUAS.Object.AppData.myAppData);
                        //    w.spModule.Children.Add(admsDB_Home);
                        //    break;
                        #endregion
                        #region Control Center
                        case "ControlCenter.Modules.Home":
                            w.spModule.Children.Clear();
                            ControlCenter.Modules.Home ccHome = new ControlCenter.Modules.Home();
                            w.spModule.Children.Add(ccHome);
                            break;
                        case "ControlCenter.Modules.ApplicationManagement":
                            w.spModule.Children.Clear();
                            ControlCenter.Modules.ApplicationManagement ccApplicationManagement = new ControlCenter.Modules.ApplicationManagement();
                            w.spModule.Children.Add(ccApplicationManagement);
                            break;
                        case "ControlCenter.Modules.MenuManagement":
                            w.spModule.Children.Clear();
                            ControlCenter.Modules.MenuManagement ccMenuManagement = new ControlCenter.Modules.MenuManagement();
                            w.spModule.Children.Add(ccMenuManagement);
                            break;
                        case "ControlCenter.Modules.PublicationManagement":
                            w.spModule.Children.Clear();
                            ControlCenter.Modules.PublicationManagement ccPublicationManagement = new ControlCenter.Modules.PublicationManagement();
                            w.spModule.Children.Add(ccPublicationManagement);
                            break;
                        case "ControlCenter.Modules.ClientManagement":
                            w.spModule.Children.Clear();
                            ControlCenter.Modules.ClientManagement ccPublisherManagement = new ControlCenter.Modules.ClientManagement();
                            w.spModule.Children.Add(ccPublisherManagement);
                            break;
                        case "ControlCenter.Modules.SecurityGroupManagement":
                            w.spModule.Children.Clear();
                            ControlCenter.Modules.SecurityGroupManagement ccSecurityGroupManagement = new ControlCenter.Modules.SecurityGroupManagement();
                            w.spModule.Children.Add(ccSecurityGroupManagement);
                            break;
                        case "ControlCenter.Modules.UserManagement":
                            w.spModule.Children.Clear();
                            ControlCenter.Modules.UserManagement ccUserManagement = new ControlCenter.Modules.UserManagement();
                            w.spModule.Children.Add(ccUserManagement);
                            break;
                        case "ControlCenter.Modules.CodeSheetViewer":
                            w.spModule.Children.Clear();
                            ControlCenter.Modules.CodeSheetViewer ccCodeSheetViewer = new ControlCenter.Modules.CodeSheetViewer();
                            w.spModule.Children.Add(ccCodeSheetViewer);
                            break;
                        case "ControlCenter.Modules.ClientGroupMgmt":
                            w.spModule.Children.Clear();
                            ControlCenter.Modules.ClientGroupMgmt ccClientGroupMgmt = new ControlCenter.Modules.ClientGroupMgmt();
                            w.spModule.Children.Add(ccClientGroupMgmt);
                            break;
                        case "ControlCenter.Modules.ServiceManagement":
                            w.spModule.Children.Clear();
                            ControlCenter.Modules.ServiceManagement ccServiceManagement = new ControlCenter.Modules.ServiceManagement();
                            w.spModule.Children.Add(ccServiceManagement);
                            break;
                        case "ControlCenter.Modules.ServiceFeatureManagement":
                            w.spModule.Children.Clear();
                            ControlCenter.Modules.ServiceFeatureManagement ccServiceFeatureManagement = new ControlCenter.Modules.ServiceFeatureManagement();
                            w.spModule.Children.Add(ccServiceFeatureManagement);
                            break;
                        case "ControlCenter.Modules.MenuFeatureMgmt":
                            w.spModule.Children.Clear();
                            ControlCenter.Modules.MenuFeatureMgmt ccMenuFeatureMgmt = new ControlCenter.Modules.MenuFeatureMgmt();
                            w.spModule.Children.Add(ccMenuFeatureMgmt);
                            break;
                        case "ControlCenter.Modules.MarketMgmt":
                            w.spModule.Children.Clear();
                            ControlCenter.Modules.MarketMgmt ccMarketMgmt = new ControlCenter.Modules.MarketMgmt();
                            w.spModule.Children.Add(ccMarketMgmt);
                            break;
                        case "ControlCenter.Modules.BrandMgmt":
                            w.spModule.Children.Clear();
                            ControlCenter.Modules.BrandMgmt ccBrandMgmt = new ControlCenter.Modules.BrandMgmt();
                            w.spModule.Children.Add(ccBrandMgmt);
                            break;
                        case "ControlCenter.Modules.FilterMgmt":
                            w.spModule.Children.Clear();
                            ControlCenter.Modules.FilterMgmt ccFilterMgmt = new ControlCenter.Modules.FilterMgmt();
                            w.spModule.Children.Add(ccFilterMgmt);
                            break;
                        case "ControlCenter.Modules.CampaignMgmt":
                            w.spModule.Children.Clear();
                            ControlCenter.Modules.CampaignMgmt ccCampaignMgmt = new ControlCenter.Modules.CampaignMgmt();
                            w.spModule.Children.Add(ccCampaignMgmt);
                            break;
                        case "ControlCenter.Modules.CircCodeSheetViewer":
                            w.spModule.Children.Clear();
                            ControlCenter.Modules.CircCodeSheetViewer ccCircCodeSheetViewer = new ControlCenter.Modules.CircCodeSheetViewer();
                            w.spModule.Children.Add(ccCircCodeSheetViewer);
                            break;
                        case "ControlCenter.Modules.CircRulesMgmt":
                            w.spModule.Children.Clear();
                            ControlCenter.Modules.CircRulesMgmt circRulesMgmt = new ControlCenter.Modules.CircRulesMgmt();
                            w.spModule.Children.Add(circRulesMgmt);
                            break;
                        case "ControlCenter.Modules.UADDataRemovalMgmt":
                            w.spModule.Children.Clear();
                            ControlCenter.Modules.UADDataRemovalMgmt uadDataRemovalMgmt = new ControlCenter.Modules.UADDataRemovalMgmt();
                            w.spModule.Children.Add(uadDataRemovalMgmt);
                            break;
                        #endregion
                        #region UAD
                        //case "UAD_Explorer.Modules.Consensus":
                        //    w.spModule.Children.Clear();
                        //    UAD_Explorer.Modules.Home uadHome = new UAD_Explorer.Modules.Home();
                        //    uadHome.Name = "CircReports";
                        //    w.spModule.Children.Add(uadHome);
                        //    break;
                        //case "UAD_Explorer.Modules.Product":
                        //    w.spModule.Children.Clear();
                        //    UAD_Explorer.Modules.Product product = new UAD_Explorer.Modules.Product();
                        //    product.Name = "CircReports";
                        //    w.spModule.Children.Add(product);
                        //    break;
                        #endregion
                        #region Data Compare
                        case "DataCompare.Modules.Home":
                            w.spModule.Children.Clear();
                            DataCompare.Modules.Home dcHome = new DataCompare.Modules.Home(FrameworkUAS.Object.AppData.myAppData);
                            w.spModule.Children.Add(dcHome);
                            break;
                        #endregion
                    }
                }
            }
        }
        void circWQT_Navigated(object sender, NavigationEventArgs e)
        {
            WebBrowser circWQT = (WebBrowser)sender;
            SetSilent(circWQT, true); // make it silent
        }
        public static void SetSilent(WebBrowser browser, bool silent)
        {
            if (browser == null)
                throw new ArgumentNullException("browser");

            // get an IWebBrowser2 from the document
            IOleServiceProvider sp = browser.Document as IOleServiceProvider;
            if (sp != null)
            {
                Guid IID_IWebBrowserApp = new Guid("0002DF05-0000-0000-C000-000000000046");
                Guid IID_IWebBrowser2 = new Guid("D30C1661-CDAF-11d0-8A3E-00C04FC9E26E");

                object webBrowser;
                sp.QueryService(ref IID_IWebBrowserApp, ref IID_IWebBrowser2, out webBrowser);
                if (webBrowser != null)
                {
                    webBrowser.GetType().InvokeMember("Silent", BindingFlags.Instance | BindingFlags.Public | BindingFlags.PutDispProperty, null, webBrowser, new object[] { silent });
                }
            }
        }
        [System.Runtime.InteropServices.ComImport, Guid("6D5140C1-7436-11CE-8034-00AA006009FA"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IOleServiceProvider
        {
            [PreserveSig]
            int QueryService([In] ref Guid guidService, [In] ref Guid riid, [MarshalAs(UnmanagedType.IDispatch)] out object ppvObject);
        }
        private void miHome_Click(object sender, RoutedEventArgs e)
        {
            Home_Click();
        }
        private void Home_Click()
        {
            if (CurrentApp.ApplicationName == "Circulation")
            {
                if (!Circulation.Modules.Home.IsLoaded)
                {
                    Core_AMS.Utilities.WPF.Message("Circulation data is still loading. Please wait for loading to complete before continuing.");
                    return;
                }
                FrameworkUAS.Service.Response<bool> saveResp = new FrameworkUAS.Service.Response<bool>();
                FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> pubData = FrameworkServices.ServiceClient.UAD_ProductClient();
                for (int i = 0; i < FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.OpenedClients.Count; i++)
                {
                    KMPlatform.Entity.Client c = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.OpenedClients[i];
                    saveResp = pubData.Proxy.UpdateLock(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, c.ClientConnections, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID);
                    if (saveResp != null && saveResp.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.OpenedClients.RemoveAt(i);
                }

                FrameworkServices.ServiceClient<UAD_WS.Interface.IBatch> worker = FrameworkServices.ServiceClient.UAD_BatchClient();
                List<FrameworkUAD.Entity.Batch> openBatches = worker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID, true, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
                if (openBatches != null && openBatches.Count > 0)
                {
                    MessageBoxResult r = Core_AMS.Utilities.WPF.MessageResult("User cannot exit Circulation until open batches are finalized.  Clicking Yes will automatically close your open batches.  Clicking No will require you to manually close open batches before exiting.", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, "Finalize Open Batches");
                    if (r == MessageBoxResult.Yes)
                    {
                        worker.Proxy.CloseBatches(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                        foreach (FrameworkUAD.Entity.Batch b in openBatches)
                        {
                            //b.IsActive = false;
                            //b.DateFinalized = DateTime.Now;
                            //worker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, b, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                            FrameworkUAS.Object.Batch batchInList = FrameworkUAS.Object.AppData.myAppData.BatchList.Where(x => x.BatchID == b.BatchID).FirstOrDefault();
                            if (batchInList != null)
                            {
                                FrameworkUAS.Object.AppData.myAppData.BatchList.Remove(batchInList);
                            }
                        }
                    }
                    else
                    {
                        //Open new window for History?
                        Circulation.Modules.History circHistory = new Circulation.Modules.History(true);
                        Circulation.Helpers.Common.OpenCircPopoutWindow(circHistory);
                        //e.Handled = true;
                        return;
                    }
                }
                if (FrameworkUAS.Object.AppData.myAppData.OpenWindowCount > 0)
                {
                    foreach (Window win in Application.Current.Windows)
                    {
                        if (win.GetType() == typeof(Circulation.Windows.Popout))
                            win.Close();
                    }
                }
            }
            Window w = Core_AMS.Utilities.WPF.GetMainWindow();
            Windows.Home home = (Windows.Home)w;
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
        public static System.Drawing.Rectangle GetScreenFrom(Window window)
        {
            WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);
            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.FromHandle(windowInteropHelper.Handle);
            return screen.WorkingArea;
        }
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            // This part should bring users back to AMS Home screen

            if (CurrentApp.ApplicationName == "Circulation")
            {
                if (!Circulation.Modules.Home.IsLoaded)
                {
                    Core_AMS.Utilities.WPF.Message("Circulation data is still loading. Please wait for loading to complete before continuing.");
                    return;
                }
                FrameworkUAS.Service.Response<bool> saveResp = new FrameworkUAS.Service.Response<bool>();
                FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> pubData = FrameworkServices.ServiceClient.UAD_ProductClient();
                for (int i = 0; i < FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.OpenedClients.Count; i++)
                {
                    KMPlatform.Entity.Client c = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.OpenedClients[i];
                    saveResp = pubData.Proxy.UpdateLock(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, c.ClientConnections, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID);
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
            }
            FrameworkServices.ServiceClient<UAD_WS.Interface.IBatch> worker = FrameworkServices.ServiceClient.UAD_BatchClient();
            List<FrameworkUAD.Entity.Batch> openBatches = worker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID, true, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
            if (openBatches != null && openBatches.Count > 0 && CurrentApp.ApplicationName == "Circulation")
            {
                MessageBoxResult r = Core_AMS.Utilities.WPF.MessageResult("User cannot log out until open batches are finalized.  Clicking Yes will automatically close your open batches.  Clicking No will require you to manually close open batches before logging out.", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, "Finalize Open Batches");
                if (r == MessageBoxResult.Yes)
                {
                    worker.Proxy.CloseBatches(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                    foreach (FrameworkUAD.Entity.Batch b in openBatches)
                    {
                        //b.IsActive = false;
                        //b.DateFinalized = DateTime.Now;
                        //worker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, b, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                        FrameworkUAS.Object.Batch batchInList = FrameworkUAS.Object.AppData.myAppData.BatchList.Where(x => x.BatchID == b.BatchID).FirstOrDefault();
                        if (batchInList != null)
                        {
                            FrameworkUAS.Object.AppData.myAppData.BatchList.Remove(batchInList);
                        }
                    }

                    Home_Click();
                }
                else
                {
                    //Open new window for History?
                    Circulation.Modules.History circHistory = new Circulation.Modules.History(true);
                    Circulation.Helpers.Common.OpenCircPopoutWindow(circHistory);
                    //e.Handled = true;
                    return;
                }
            }
            else
            {
                Home_Click();
            }
            return;
        }
    }
}
