using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FrameworkUAS.Object;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ControlCenter.Modules
{
    /// <summary>
    /// Interaction logic for SecurityGroupManagement.xaml
    /// </summary>
    public partial class SecurityGroupManagement : UserControl, INotifyPropertyChanged
    {
        #region Services/Responses
        FrameworkServices.ServiceClient<UAS_WS.Interface.IApplication> appWorker = FrameworkServices.ServiceClient.UAS_ApplicationClient();
        //FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationSecurityGroupMap> appSecurityGroupMapData = FrameworkServices.ServiceClient.UAS_ApplicationSecurityGroupMapClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IMenu> menuData = FrameworkServices.ServiceClient.UAS_MenuClient();
        //FrameworkServices.ServiceClient<UAS_WS.Interface.IMenuSecurityGroupMap> menuMapData = FrameworkServices.ServiceClient.UAS_MenuSecurityGroupMapClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.ISecurityGroup> securityGroupData = FrameworkServices.ServiceClient.UAS_SecurityGroupClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IClientGroupSecurityGroupMap> clientGroupSecurityW = FrameworkServices.ServiceClient.UAS_ClientGroupSecurityGroupMapClient();
        FrameworkUAS.Service.Response<int> saveResponse = new FrameworkUAS.Service.Response<int>();
        FrameworkUAS.Service.Response<KMPlatform.Entity.SecurityGroup> securityGroupResponse = new FrameworkUAS.Service.Response<KMPlatform.Entity.SecurityGroup>();
        #endregion
        #region Entities/Lists
        private Window NewSecurityGroup;
        List<KMPlatform.Entity.Menu> menus = new List<KMPlatform.Entity.Menu>();
        //List<KMPlatform.Entity.MenuSecurityGroupMap> menuMaps = new List<KMPlatform.Entity.MenuSecurityGroupMap>();
        List<KMPlatform.Entity.Application> apps = new List<KMPlatform.Entity.Application>();
        //List<KMPlatform.Entity.ApplicationSecurityGroupMap> appSecurityMaps = new List<KMPlatform.Entity.ApplicationSecurityGroupMap>();
        List<KMPlatform.Entity.SecurityGroup> securityGroups = new List<KMPlatform.Entity.SecurityGroup>();
        List<FrameworkUAS.Entity.ClientGroupSecurityGroupMap> clientGroupSecurityMaps = new List<FrameworkUAS.Entity.ClientGroupSecurityGroupMap>();
        private KMPlatform.Entity.SecurityGroup CurrentSecurityGroup { get; set; }
        private Guid AccessKey;
        #endregion
        #region Properties
        private ObservableCollection<ApplicationSecurity> _applicationSecurityList = new ObservableCollection<ApplicationSecurity>();
        private ObservableCollection<ApplicationSecurityRoot> _rootSecurityNodes = new ObservableCollection<ApplicationSecurityRoot>();
        private ObservableCollection<KMPlatform.Entity.SecurityGroup> _securityGroups = new ObservableCollection<KMPlatform.Entity.SecurityGroup>();
        private int _securityGroupID = 0;
        public ObservableCollection<ApplicationSecurityRoot> RootSecurityNodes { get { return _rootSecurityNodes; } }
        public ObservableCollection<KMPlatform.Entity.SecurityGroup> SecurityGroups { get { return _securityGroups; } }
        public int SelectedSecurityGroupID
        {
            get { return _securityGroupID; }
            set
            {
                _securityGroupID = value;
                if (_securityGroupID > 0)
                {
                    btnSave.Visibility = System.Windows.Visibility.Visible;
                    RootSecurityNodes.Clear();
                    //foreach (KMPlatform.Entity.ApplicationSecurityGroupMap ascg in appSecurityMaps.Where(x => x.SecurityGroupID == _securityGroupID))
                    //{
                    //    KMPlatform.Entity.Application a = apps.Where(x => x.ApplicationID == ascg.ApplicationID).FirstOrDefault();
                    //    if (a == null) //This Application is not available for selected ClientGroup Security Settings.
                    //        continue;
                    //    List<ApplicationSecurity> rootApp = new List<ApplicationSecurity>();
                    //    List<MenuSecurity> tempMenus = new List<MenuSecurity>();
                    //    foreach (KMPlatform.Entity.Menu m in menus.Where(x => x.IsActive == true && x.ApplicationID == a.ApplicationID))
                    //    {
                    //        KMPlatform.Entity.MenuSecurityGroupMap msgm = menuMaps.Where(x => x.MenuID == m.MenuID && x.SecurityGroupID == _securityGroupID).FirstOrDefault();
                    //        if (msgm != null)
                    //            tempMenus.Add(new MenuSecurity(msgm, m.MenuName, m.MenuID, _securityGroupID));
                    //        else
                    //            tempMenus.Add(new MenuSecurity(new KMPlatform.Entity.MenuSecurityGroupMap(), m.MenuName, m.MenuID, _securityGroupID));
                    //    }
                    //    //ApplicationSecurityList.Add(new ApplicationSecurity(ascg, a.ApplicationName, tempMenus));
                    //    rootApp.Add(new ApplicationSecurity(ascg, a.ApplicationName, a.ApplicationID, _securityGroupID, tempMenus));
                    //    RootSecurityNodes.Add(new ApplicationSecurityRoot(a.ApplicationID, rootApp));
                    //}
                }
                if (null != this.PropertyChanged)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("SecurityGroupID"));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        #region Classes
        public class MenuSecurity : INotifyPropertyChanged
        {
            private int _menuSecurityGroupMapID;
            private int _securityGroupID;
            private int _menuID;
            private string _menuName;
            private bool _hasAccess;
            private bool _isActive;
            private DateTime _dateCreated;
            private int _createdByUserID;
            public int MenuSecurityGroupMapID { get { return _menuSecurityGroupMapID; } }
            public int SecurityGroupID { get { return _securityGroupID; } }
            public int MenuID { get { return _menuID; } }
            public string MenuName { get { return _menuName; } }
            public bool HasAccess
            {
                get { return _hasAccess; }
                set
                {
                    _hasAccess = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("HasAccess"));
                    }
                }
            }
            //public KMPlatform.Entity.MenuSecurityGroupMap GetMenuEntity()
            //{
            //    KMPlatform.Entity.MenuSecurityGroupMap rtnItem = new KMPlatform.Entity.MenuSecurityGroupMap();
            //    rtnItem.MenuSecurityGroupMapID = _menuSecurityGroupMapID;
            //    rtnItem.MenuID = _menuID;
            //    rtnItem.HasAccess = _hasAccess;
            //    rtnItem.SecurityGroupID = _securityGroupID;
            //    if (_menuSecurityGroupMapID > 0)
            //    {
            //        rtnItem.DateUpdated = DateTime.Now;
            //        rtnItem.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            //        rtnItem.IsActive = _isActive;
            //    }
            //    else
            //    {
            //        rtnItem.DateCreated = _dateCreated;
            //        rtnItem.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            //        rtnItem.IsActive = true;
            //    }

            //    return rtnItem;
            //}

            //public MenuSecurity(KMPlatform.Entity.MenuSecurityGroupMap msgm, string name, int menuID, int securityGroupID)
            //{
            //    _menuSecurityGroupMapID = msgm.MenuSecurityGroupMapID;
            //    _securityGroupID = securityGroupID;
            //    _menuID = menuID;
            //    _hasAccess = msgm.HasAccess;
            //    _isActive = msgm.IsActive;
            //    _dateCreated = msgm.DateCreated;
            //    _createdByUserID = msgm.CreatedByUserID;
            //    _menuName = name;
            //}
            public event PropertyChangedEventHandler PropertyChanged;
        }
        public class ApplicationSecurity : INotifyPropertyChanged
        {
            private int _applicationID;
            private int _applicationSecurityGroupMapID;
            private int _securityGroupID;
            private string _applicationName;
            private bool _hasAccess;
            private DateTime _dateCreated;
            private int _createdByUserID;
            private ObservableCollection<MenuSecurity> _menus = new ObservableCollection<MenuSecurity>();
            public int ApplicationID { get { return _applicationID; } }
            public int SecurityGroupID { get { return _securityGroupID; } }
            public string ApplicationName { get { return _applicationName; } }
            public bool HasAccess
            {
                get { return _hasAccess; }
                set
                {
                    if (_hasAccess != value)
                    {
                        _hasAccess = value;
                        if (_hasAccess)
                        {
                            if (_menus.Where(x => x.HasAccess == false).Count() == _menus.Count())
                            {
                                foreach (MenuSecurity m in Menus)
                                {
                                    m.PropertyChanged -= Item_PropertyChanged;
                                    m.HasAccess = true;
                                    m.PropertyChanged += Item_PropertyChanged;
                                }
                            }
                        }
                        else
                        {
                            //if (_menus.Where(x => x.HasAccess == true).Count() == _menus.Count())
                            //{
                            foreach (MenuSecurity m in Menus)
                            {
                                m.PropertyChanged -= Item_PropertyChanged;
                                m.HasAccess = false;
                                m.PropertyChanged += Item_PropertyChanged;
                            }
                            //}
                        }
                        if (null != this.PropertyChanged)
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("HasAccess"));
                        }
                    }
                }
            }
            public ObservableCollection<MenuSecurity> Menus { get { return _menus; } }
            public void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if(e.PropertyName == "HasAccess")
                {
                    var test = _menus.Where(x => x.HasAccess == true).Count();
                    if (_menus.Where(x => x.HasAccess == true).Count() >= 1)
                        this.HasAccess = true;
                    else
                        this.HasAccess = false;
                }
            }
            //public KMPlatform.Entity.ApplicationSecurityGroupMap GetAppSecurityEntity()
            //{
            //    KMPlatform.Entity.ApplicationSecurityGroupMap rtnItem = new KMPlatform.Entity.ApplicationSecurityGroupMap();
            //    rtnItem.ApplicationID = _applicationID;
            //    rtnItem.ApplicationSecurityGroupMapID = _applicationSecurityGroupMapID;
            //    rtnItem.HasAccess = _hasAccess;
            //    rtnItem.SecurityGroupID = _securityGroupID;
            //    if (_applicationSecurityGroupMapID > 0)
            //    {
            //        rtnItem.DateUpdated = DateTime.Now;
            //        rtnItem.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            //    }
            //    else
            //    {
            //        rtnItem.DateCreated = _dateCreated;
            //        rtnItem.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            //    }

            //    return rtnItem;
            //}

            //public ApplicationSecurity(KMPlatform.Entity.ApplicationSecurityGroupMap a, string appName, int appID, int securityGroupID, List<MenuSecurity> menus)
            //{
            //    _applicationID = appID;
            //    _applicationSecurityGroupMapID = a.ApplicationSecurityGroupMapID;
            //    _applicationName = appName;
            //    _securityGroupID = securityGroupID;
            //    _hasAccess = a.HasAccess;
            //    _dateCreated = a.DateCreated;
            //    _menus = new ObservableCollection<MenuSecurity>(menus);
            //    foreach (MenuSecurity m in _menus)
            //        m.PropertyChanged += Item_PropertyChanged;
            //    if(_hasAccess)
            //    {
            //        //foreach (MenuSecurity ms in _menus)
            //        //    ms.HasAccess = true;
            //    }
            //    if (_menus.Where(x => x.HasAccess == true).Count() >= 1)
            //        _hasAccess = true;
            //}
            public event PropertyChangedEventHandler PropertyChanged;
        }
        public  class ApplicationSecurityRoot: INotifyPropertyChanged
        {
            private ObservableCollection<ApplicationSecurity> _applicationRoot = new ObservableCollection<ApplicationSecurity>();
            public int ApplicationID { get; set; }
            public ObservableCollection<ApplicationSecurity> ApplicationRoot
            {
                get { return _applicationRoot; }
                set { _applicationRoot = value; }
            }
            public ApplicationSecurityRoot(int id, List<ApplicationSecurity> app)
            {
                this.ApplicationID = id;
                this.ApplicationRoot = new ObservableCollection<ApplicationSecurity>(app);
            }
            public event PropertyChangedEventHandler PropertyChanged;
        }
        #endregion

        public SecurityGroupManagement()
        {
            InitializeComponent();

            //Only available to KM Users
            if (AppData.IsKmUser() == true)
            {
                AccessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;

                BackgroundWorker bw = new BackgroundWorker();
                busy.IsBusy = true;
                bw.DoWork += (o, ea) =>
                {
                    apps = appWorker.Proxy.Select(AccessKey).Result;
                    //appSecurityMaps = appSecurityGroupMapData.Proxy.Select(AccessKey).Result;
                    menus = menuData.Proxy.Select(AccessKey).Result.Where(x => x.IsActive == true).ToList();
                    //menuMaps = menuMapData.Proxy.Select(AccessKey).Result;
                    securityGroups = securityGroupData.Proxy.Select(AccessKey).Result;
                    //clientGroupSecurityMaps = clientGroupSecurityW.Proxy.SelectForClientGroup(AccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.ClientGroupID).Result;
                };
                bw.RunWorkerAsync();
                bw.RunWorkerCompleted += (o, ea) =>
                {
                    busy.IsBusy = false;
                    foreach (KMPlatform.Entity.SecurityGroup sg in securityGroups)
                    {
                        if (clientGroupSecurityMaps.Where(x => x.SecurityGroupID == sg.SecurityGroupID && x.IsActive == true).Count() > 0)
                            this.SecurityGroups.Add(sg);
                        //else
                        //    appSecurityMaps.RemoveAll(x => x.SecurityGroupID == sg.SecurityGroupID);
                    }
                    bw.Dispose();
                };
            }
            else
            {
                Core_AMS.Utilities.WPF.MessageAccessDenied();
            }
        }
        private void btnNewGroup_Click(object sender, RoutedEventArgs e)
        {
            Window groupPop = new Window();
            groupPop.Width = 300;
            groupPop.Height = 200;
            groupPop.Owner = Core_AMS.Utilities.WPF.GetMainWindow();
            groupPop.Title = "New Security Group";
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Vertical;
            System.Windows.Controls.Label lb = new System.Windows.Controls.Label();
            lb.FontWeight = FontWeights.Bold;
            lb.Content = "Security Group Name";
            lb.Margin = new Thickness(50, 0, 0, 0);
            sp.Children.Add(lb);
            TextBox tb = new TextBox();
            tb.Name = "tbNewGroup";
            tb.Width = 200;
            tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            sp.Children.Add(tb);
            Button btnSave = new Button();
            btnSave.Margin = new Thickness(0, 0, 50, 0);
            btnSave.Name = "btnSave";
            btnSave.Width = 50;
            btnSave.Content = "Save";
            //btnSave.Click += btnSave_Click;
            btnSave.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            btnSave.Margin = new Thickness(0, 20, 50, 0);
            sp.Children.Add(btnSave);

            groupPop.Content = sp;
            NewSecurityGroup = groupPop;
            groupPop.ShowDialog();
        }
        private void btnSave_Click2(object sender, RoutedEventArgs e)
        {
            Button btnSave = (Button)sender;
            Window pop = Core_AMS.Utilities.WPF.GetParentWindow(btnSave);
            TextBox tbNewGroup = Core_AMS.Utilities.WPF.FindChild<TextBox>(pop, "tbNewGroup");
            string newGroupName = tbNewGroup.Text;
            //FIX:  Pass valid ClientGroupID
            if (securityGroupData.Proxy.SecurityGroupNameExists(AccessKey, newGroupName, 0).Result)
            {
                tbNewGroup.Text = string.Empty;
                Core_AMS.Utilities.WPF.Message("Security Group Name - " + newGroupName + " - already exists. Please enter a new name.", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            else
            {
                KMPlatform.Entity.SecurityGroup sg = new KMPlatform.Entity.SecurityGroup();
                sg.CreatedByUserID = AppData.myAppData.AuthorizedUser.User.UserID;
                sg.DateCreated = DateTime.Now;
                sg.IsActive = true;
                sg.SecurityGroupName = newGroupName;
                //sg.SecurityGroupID = securityGroupData.Proxy.Save(AccessKey, sg).Result;

                securityGroups.Add(sg);

                //add to all apps but no access
                foreach (var a in apps)
                {
                    //KMPlatform.Entity.ApplicationSecurityGroupMap newAM = new KMPlatform.Entity.ApplicationSecurityGroupMap();
                    //newAM.ApplicationID = a.ApplicationID;
                    //newAM.CreatedByUserID = AppData.myAppData.AuthorizedUser.User.UserID;
                    //newAM.DateCreated = DateTime.Now;
                    //newAM.HasAccess = false;
                    //newAM.SecurityGroupID = sg.SecurityGroupID;
                    //newAM.@ApplicationSecurityGroupMapID = appSecurityGroupMapData.Proxy.Save(AccessKey, newAM).Result;

                    //appSecurityMaps.Add(newAM);
                }
                //add to all menus but no access
                foreach (var m in menus)
                {
                    //KMPlatform.Entity.MenuSecurityGroupMap newMM = new KMPlatform.Entity.MenuSecurityGroupMap();
                    //newMM.CreatedByUserID = AppData.myAppData.AuthorizedUser.User.UserID;
                    //newMM.DateCreated = DateTime.Now;
                    //newMM.HasAccess = false;
                    //newMM.IsActive = m.IsActive;
                    //newMM.MenuID = m.MenuID;
                    //newMM.SecurityGroupID = sg.SecurityGroupID;
                    //newMM.MenuSecurityGroupMapID = menuMapData.Proxy.Save(AccessKey, newMM).Result;

                    //menuMaps.Add(newMM);
                }

                //clbGroups.ItemsSource = null;

                //LoadGroups();
                //if all good
                NewSecurityGroup.Close();
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker bw = new BackgroundWorker();
            busy.IsBusy = true;
            bw.DoWork += (o, ea) =>
            {
                foreach (ApplicationSecurityRoot root in RootSecurityNodes)
                {
                    foreach (ApplicationSecurity appSec in root.ApplicationRoot)
                    {
                        //KMPlatform.Entity.ApplicationSecurityGroupMap asgm = appSec.GetAppSecurityEntity();
                        //if (asgm.ApplicationSecurityGroupMapID > 0)
                        //{
                        //    KMPlatform.Entity.ApplicationSecurityGroupMap original = appSecurityMaps.Where(x => x.ApplicationSecurityGroupMapID == asgm.ApplicationSecurityGroupMapID).FirstOrDefault();
                        //    if (original != null && original.HasAccess != asgm.HasAccess) //Only want to make call to the DB if an actual change has been made.
                        //    {
                        //        saveResponse = appSecurityGroupMapData.Proxy.Save(AccessKey, asgm);
                        //        if (saveResponse.Result == 0 && saveResponse.Status != FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                        //            Core_AMS.Utilities.WPF.MessageError("There was a problem saving Security Settings for the Application: " + appSec.ApplicationName);
                        //    }
                        //}
                        //else if (asgm.HasAccess)//New ApplicationSecurity Record.
                        //{
                        //    asgm.DateCreated = DateTime.Now;
                        //    asgm.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                        //    saveResponse = appSecurityGroupMapData.Proxy.Save(AccessKey, asgm);
                        //    if (saveResponse.Result == 0 && saveResponse.Status != FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                        //        Core_AMS.Utilities.WPF.MessageError("There was a problem saving Security Settings for the Application: " + appSec.ApplicationName);
                        //}
                        //foreach (MenuSecurity ms in appSec.Menus)
                        //{
                        //    KMPlatform.Entity.MenuSecurityGroupMap msgm = ms.GetMenuEntity();
                        //    if (msgm.MenuSecurityGroupMapID > 0)
                        //    {
                        //        KMPlatform.Entity.MenuSecurityGroupMap original = menuMaps.Where(x => x.MenuSecurityGroupMapID == msgm.MenuSecurityGroupMapID).FirstOrDefault();
                        //        if (original != null && original.HasAccess != msgm.HasAccess) //Only want to make call to the DB if an actual change has been made.
                        //        {
                        //            saveResponse = menuMapData.Proxy.Save(AccessKey, msgm);
                        //            if (saveResponse.Result == 0 && saveResponse.Status != FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                        //                Core_AMS.Utilities.WPF.MessageError("There was a problem saving Security Settings for the Menu Item: " + ms.MenuName);
                        //        }
                        //    }
                        //    else if (msgm.HasAccess)//New MenuSecurity Record.
                        //    {
                        //        msgm.DateCreated = DateTime.Now;
                        //        msgm.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                        //        saveResponse = menuMapData.Proxy.Save(AccessKey, msgm);
                        //        if (saveResponse.Result == 0 && saveResponse.Status != FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                        //            Core_AMS.Utilities.WPF.MessageError("There was a problem saving Security Settings for the Application: " + appSec.ApplicationName);
                        //    }
                        //}
                    }
                }
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                //Rebuild User Auth Security Group object so changes take effect immediately.
                //securityGroupResponse = securityGroupData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey,
                //     FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID);
                if (securityGroupResponse.Result != null && securityGroupResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentSecurityGroup = securityGroupResponse.Result;
                busy.IsBusy = false;
                Core_AMS.Utilities.WPF.Message("Save successful.");
                bw.Dispose();
            };
            bw.RunWorkerAsync();
        }
    }
}
