using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Telerik.Windows.Controls;

namespace ControlCenter.Modules
{
    /// <summary>
    /// Interaction logic for UserManagement.xaml
    /// </summary>
    public partial class UserManagement : UserControl
    {
        #region Services & Responses
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IUser> userWorker = FrameworkServices.ServiceClient.UAS_UserClient();
        private FrameworkUAS.Service.Response<int> intResponse = new FrameworkUAS.Service.Response<int>();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IUserClientSecurityGroupMap> UCSGroupWorker = FrameworkServices.ServiceClient.UAS_UserClientSecurityGroupMapClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IClientGroupUserMap> cgumWorker = FrameworkServices.ServiceClient.UAS_ClientGroupUserMapClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.ISecurityGroup> sgWorker = FrameworkServices.ServiceClient.UAS_SecurityGroupClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IClientGroup> cgWorker = FrameworkServices.ServiceClient.UAS_ClientGroupClient();
        #endregion
        #region Entities/Lists
        List<KMPlatform.Entity.User> allUsers = new List<KMPlatform.Entity.User>();
        List<KMPlatform.Entity.ClientGroup> clientGroups = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.ClientGroups;
        List<KMPlatform.Entity.Client> clients = new List<KMPlatform.Entity.Client>();
        List<string> activeTypes = new List<string>();
        private ObservableCollection<UserContainer> _users = new ObservableCollection<UserContainer>();
        public ObservableCollection<UserContainer> Users { get { return _users; } }
        List<KMPlatform.Entity.UserClientSecurityGroupMap> ucsGroupList = new List<KMPlatform.Entity.UserClientSecurityGroupMap>();
        List<FrameworkUAS.Entity.ClientGroupUserMap> cgumList = new List<FrameworkUAS.Entity.ClientGroupUserMap>();
        List<KMPlatform.Entity.SecurityGroup> securityGroups = new List<KMPlatform.Entity.SecurityGroup>();
        private Guid AccessKey;
        DoubleAnimation opacityAnimate = new DoubleAnimation()
        {
            To = 0.5,
            Duration = TimeSpan.FromSeconds(.4)
        };
        DoubleAnimation opacityReverseAnimate = new DoubleAnimation()
        {
            To = 1,
            Duration = TimeSpan.FromSeconds(.4)
        };
        #endregion
        #region Classes & Enums
        public class UserContainer : INotifyPropertyChanged
        {
            #region Private
            private int _userID;
            private string _firstName;
            private string _lastName;
            private int _defaultClientGroupID;
            private int _defaultClientID;
            private int _securityGroupID;
            private string _defaultClientGroup;
            private string _defaultClient;
            private string _userName;
            private string _passWord;
            private string _salt;
            private string _email;
            private bool _isActive;
            private bool _isReadOnly;
            private Guid _key;
            private bool _isKeyValid;
            private bool _isOpen;
            private bool _infoChanged;
            private bool _copyEmail;
            private DateTime _dateCreated;
            private DateTime? _dateUpdated;
            private int _createdByUserID;
            private int? _updatedByUserID;
            private string _createdByUser;
            private string _updatedByUser;
            private ObservableCollection<KMPlatform.Entity.Client> _clientList = new ObservableCollection<KMPlatform.Entity.Client>();
            private ObservableCollection<KMPlatform.Entity.ClientGroup> _clientGroupList = new ObservableCollection<KMPlatform.Entity.ClientGroup>();
            private ObservableCollection<KMPlatform.Entity.SecurityGroup> _securityGroups = new ObservableCollection<KMPlatform.Entity.SecurityGroup>();
            #endregion
            public string FirstName
            {
                get { return _firstName; }
                set
                {
                    _firstName = value;
                    CheckChanges();
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("FirstName"));
                    }
                }
            }
            public string LastName
            {
                get { return _lastName; }
                set
                {
                    _lastName = value;
                    CheckChanges();
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("LastName"));
                    }
                }
            }
            public int UserID
            {
                get { return _userID; }
                set
                {
                    _userID = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("UserID"));
                    }
                }
            }
            public int? DefaultClientGroupID
            {
                get { return _defaultClientGroupID; }
                set
                {
                    _defaultClientGroupID = (value ?? 1);
                    string name = _clientGroupList.Where(x => x.ClientGroupID == _defaultClientGroupID).Select(x => x.ClientGroupName).FirstOrDefault();
                    if (name != null && name != "")
                        this.DefaultClientGroup = name;
                    CheckChanges();
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("DefaultClientGroupID"));
                    }
                }
            }
            public int? DefaultClientID
            {
                get { return _defaultClientID; }
                set
                {
                    _defaultClientID = (value ?? 1);
                    string name = _clientList.Where(x => x.ClientID == _defaultClientID).Select(x => x.ClientName).FirstOrDefault();
                    if (name != null && name != "")
                        this.DefaultClient = name;
                    CheckChanges();
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("DefaultClientID"));
                    }
                }
            }
            public int? SecurityGroupID
            {
                get { return _securityGroupID; }
                set
                {
                    _securityGroupID = (value ?? 1);                    
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("SecurityGroupID"));
                    }
                }
            }
            public string DefaultClientGroup
            {
                get { return _defaultClientGroup; }
                set
                {
                    _defaultClientGroup = value;                    
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("DefaultClientGroup"));
                    }
                }
            }
            public string DefaultClient
            {
                get { return _defaultClient; }
                set
                {
                    _defaultClient = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("DefaultClient"));
                    }
                }
            }
            public string UserName
            {
                get { return _userName; }
                set
                {
                    _userName = value;
                    CheckChanges();
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("UserName"));
                    }
                }
            }
            public string PassWord
            {
                get { return _passWord; }
                set
                {
                    _passWord = value;
                    CheckChanges();
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("PassWord"));
                    }
                }
            }
            public string Salt
            {
                get { return _salt; }
                set
                {
                    _salt = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Salt"));
                    }
                }
            }
            public string Email
            {
                get { return _email; }
                set
                {
                    _email = value;
                    if (_copyEmail)
                        this.UserName = _email;
                    CheckChanges();
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Email"));
                    }
                }
            }
            public bool IsReadOnly
            {
                get { return _isReadOnly; }
                set
                {
                    _isReadOnly = value;
                    CheckChanges();
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IsReadOnly"));
                    }
                }
            }
            public bool IsActive
            {
                get { return _isActive; }
                set
                {
                    _isActive = value;
                    CheckChanges();
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IsActive"));
                    }
                }
            }
            public bool IsOpen
            {
                get { return _isOpen; }
                set
                {
                    _isOpen = value;
                    if (_isOpen == false)
                        RevertChanges();
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IsOpen"));
                    }
                }
            }
            public bool CopyEmailToUserName
            {
                get { return _copyEmail; }
                set
                {
                    _copyEmail = value;
                    if (_copyEmail)
                        this.UserName = _email;
                    else
                        this.UserName = "";
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("CopyEmailToUserName"));
                    }
                }
            }
            public Guid AccessKey
            {
                get { return _key; }
                set
                {
                    _key = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("AccessKey"));
                    }
                }
            }
            public bool IsKeyValid
            {
                get { return _isKeyValid; }
                set
                {
                    _isKeyValid = value;
                    CheckChanges();
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IsKeyValid"));
                    }
                }
            }
            public bool InfoChanged
            {
                get { return _infoChanged; }
                set
                {
                    _infoChanged = value;
                    if (_infoChanged)
                    {
                        this.DateUpdated = DateTime.Now;
                        this.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                    }
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("InfoChanged"));
                    }
                }
            }
            public DateTime DateCreated
            {
                get { return _dateCreated; }
                set
                {
                    _dateCreated = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("DateCreated"));
                    }
                }
            }
            public DateTime? DateUpdated
            {
                get { return _dateUpdated; }
                set
                {
                    _dateUpdated = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("DateUpdated"));
                    }
                }
            }
            public int CreatedByUserID
            {
                get { return _createdByUserID; }
                set
                {
                    _createdByUserID = value;                    
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("CreatedByUserID"));
                    }
                }
            }
            public int? UpdatedByUserID
            {
                get { return _updatedByUserID; }
                set
                {
                    _updatedByUserID = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("UpdatedByUserID"));
                    }
                }
            }
            public KMPlatform.Entity.User OriginalUser { get; set; }
            public ObservableCollection<KMPlatform.Entity.Client> ClientList { get { return _clientList; } }
            public ObservableCollection<KMPlatform.Entity.ClientGroup> ClientGroups { get { return _clientGroupList; } }
            public ObservableCollection<KMPlatform.Entity.SecurityGroup> SecurityGroups { get { return _securityGroups; } }

            public UserContainer(KMPlatform.Entity.User u, List<KMPlatform.Entity.ClientGroup> clientGroups, List<KMPlatform.Entity.Client> clients, List<KMPlatform.Entity.SecurityGroup> groups = null)
            {
                _clientGroupList = new ObservableCollection<KMPlatform.Entity.ClientGroup>(clientGroups);
                _clientList = new ObservableCollection<KMPlatform.Entity.Client>(clients);
                if (groups != null)
                    _securityGroups = new ObservableCollection<KMPlatform.Entity.SecurityGroup>(groups);
                _firstName = u.FirstName;
                _lastName = u.LastName;
                _userID = u.UserID;
                _defaultClientGroupID = u.DefaultClientGroupID;
                _defaultClientID = u.DefaultClientID;
                if (_defaultClientGroupID > 0)
                    _defaultClientGroup = _clientGroupList.Where(x => x.ClientGroupID == _defaultClientGroupID).Select(x => x.ClientGroupName).FirstOrDefault();
                _defaultClient = _clientList.Where(x => x.ClientID == _defaultClientID).Select(x => x.ClientName).FirstOrDefault();
                _userName = u.UserName;
                _passWord = u.Password;
                _salt = u.Salt;
                _email = u.EmailAddress;
                _isActive = u.IsActive;
                _key = u.AccessKey;
                _isKeyValid = u.IsAccessKeyValid;
                _copyEmail = false;
                _dateCreated = u.DateCreated;
                _dateUpdated = u.DateUpdated;
                _createdByUserID = u.CreatedByUserID;
                _updatedByUserID = u.UpdatedByUserID;
                OriginalUser = u;                
            }
            public KMPlatform.Entity.User GetUserEntity()
            {
                KMPlatform.Entity.User user = new KMPlatform.Entity.User();
                user.AccessKey = _key;
                user.CreatedByUserID = _createdByUserID;
                user.DateCreated = _dateCreated;
                user.DateUpdated = _dateUpdated;
                user.DefaultClientGroupID = _defaultClientGroupID;
                user.DefaultClientID = _defaultClientID;
                user.EmailAddress = _email;
                user.FirstName = _firstName;
                user.IsAccessKeyValid = _isKeyValid;
                user.IsActive = _isActive;
                user.LastName = _lastName;
                user.Password = _passWord;
                user.Salt = _salt;
                user.UpdatedByUserID = _updatedByUserID;
                user.UserID = _userID;
                user.UserName = _userName;

                return user;
            }
            public void RevertChanges()
            {
                this.DateUpdated = OriginalUser.DateUpdated;
                this.DefaultClientGroupID = OriginalUser.DefaultClientGroupID;
                this.DefaultClientID = OriginalUser.DefaultClientID;
                this.Email = OriginalUser.EmailAddress;
                this.FirstName = OriginalUser.FirstName;
                this.IsActive = OriginalUser.IsActive;
                this.LastName = OriginalUser.LastName;
                this.PassWord = OriginalUser.Password;
                this.UpdatedByUserID = OriginalUser.UpdatedByUserID;
                this.UserName = OriginalUser.UserName;
                this.IsKeyValid = OriginalUser.IsAccessKeyValid;
            }
            public void CheckChanges()
            {
                bool infoChanged = false;
                if (_firstName != OriginalUser.FirstName)
                    infoChanged = true;
                if (_lastName != OriginalUser.LastName)
                    infoChanged = true;
                if (_defaultClientGroupID != OriginalUser.DefaultClientGroupID)
                    infoChanged = true;
                if (_defaultClientID != OriginalUser.DefaultClientID)
                    infoChanged = true;
                if (_userName != OriginalUser.UserName)
                    infoChanged = true;
                if (_passWord != OriginalUser.Password)
                    infoChanged = true;
                if (_email != OriginalUser.EmailAddress)
                    infoChanged = true;
                if (_isActive != OriginalUser.IsActive)
                    infoChanged = true;
                if (_isKeyValid != OriginalUser.IsAccessKeyValid)
                    infoChanged = true;

                if (this.InfoChanged != infoChanged)
                    this.InfoChanged = infoChanged;
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }
        public enum ActiveType
        {
            All,
            Active,
            Not_Active
        }
        #endregion

        public UserManagement()
        {
            if (FrameworkUAS.Object.AppData.IsKmUser() == true)
            {
            AccessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;

            InitializeComponent();

            activeTypes.Add(ActiveType.All.ToString());
            activeTypes.Add(ActiveType.Active.ToString());
            activeTypes.Add(ActiveType.Not_Active.ToString().Replace("_", " "));
            lstActive.ItemsSource = activeTypes;

            userWorker = FrameworkServices.ServiceClient.UAS_UserClient();
            UCSGroupWorker = FrameworkServices.ServiceClient.UAS_UserClientSecurityGroupMapClient();
            cgumWorker = FrameworkServices.ServiceClient.UAS_ClientGroupUserMapClient();
            sgWorker = FrameworkServices.ServiceClient.UAS_SecurityGroupClient();

            clientGroups = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.ClientGroups;
            foreach (KMPlatform.Entity.ClientGroup cg in clientGroups)
            {
                foreach(KMPlatform.Entity.Client c in cg.Clients)
                {
                    if (!clients.Select(x => x.ClientID).Contains(c.ClientID))
                        clients.Add(c);
                }
            }
            cbNewGroupID.ItemsSource = clientGroups;

            FrameworkUAS.Service.Response<List<KMPlatform.Entity.User>> resp = userWorker.Proxy.Select(AccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.ClientGroupID, false);
            if (resp.Result != null)
            {
                allUsers = resp.Result;
                foreach (KMPlatform.Entity.User u in allUsers)
                    Users.Add(new UserContainer(u, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.ClientGroups, clients));
            }

            ucsGroupList = UCSGroupWorker.Proxy.Select(AccessKey).Result.ToList();
            //cgumList = cgumWorker.Proxy.Select(AccessKey).Result;
            securityGroups = sgWorker.Proxy.Select(AccessKey).Result.ToList();
        }
            else
            {
                Core_AMS.Utilities.WPF.MessageAccessDenied();
            }
        }

        #region DataGrid
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = tbSearch.Text;
            if (searchText != "")
            {
                this.Users.Clear();
                foreach(KMPlatform.Entity.User u in allUsers.Where(x => x.UserName.ToLower().Contains(searchText.ToLower())))
                    this.Users.Add(new UserContainer(u, clientGroups, clients));
            }
        }
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            this.Users.Clear();
            foreach (KMPlatform.Entity.User u in allUsers)
                this.Users.Add(new UserContainer(u, clientGroups, clients));
        }
        private void lstActive_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            this.Users.Clear();
            string item = lb.SelectedItem as string;
            if (item == ActiveType.All.ToString())
            {
                foreach (KMPlatform.Entity.User u in allUsers)
                    this.Users.Add(new UserContainer(u, clientGroups, clients));
            }
            else if (item == ActiveType.Active.ToString())
            {
                foreach (KMPlatform.Entity.User u in allUsers.Where(x => x.IsActive == true))
                    this.Users.Add(new UserContainer(u, clientGroups, clients));
            }
            else
            {
                foreach (KMPlatform.Entity.User u in allUsers.Where(x => x.IsActive == false))
                    this.Users.Add(new UserContainer(u, clientGroups, clients));
            }
        }
        private void grdUser_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            foreach (UserContainer u in this.Users.Where(x => x.IsOpen))
                ToggleUserVisibility(u);

            UserContainer uc = grdUser.SelectedItem as UserContainer;
            if (uc != null)
            {
                grdUser.ScrollIntoView(uc);
                grdUser.UpdateLayout();
                ToggleUserVisibility(uc);
            }
        }
        #endregion

        #region New User Profile
        private void btnCreateUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            grdNewUser.DataContext = null;
            UserContainer uc = new UserContainer(new KMPlatform.Entity.User(), clientGroups, clients, securityGroups);
            uc.AccessKey = Guid.NewGuid();
            grdNewUser.DataContext = uc;
            grdUser.IsEnabled = false;
            btnCreateUser.IsEnabled = false;
            btnSearch.IsEnabled = false;
            btnReset.IsEnabled = false;
            grdEdit.BeginAnimation(Grid.OpacityProperty, opacityAnimate);
            grdNewUser.Opacity = 1;
                cbNewClientID.ItemsSource = clients;
            }
            catch (Exception ex)
            {
                string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
        }
        }
        private void cbNewClientID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbNewClientID.SelectedValue != null)
            {
                int clientId = -1;
                int.TryParse(cbNewClientID.SelectedValue.ToString(), out clientId);
                if (clientId > 0)
                {
                    //have to get the groups for the client selected
                    cgWorker = FrameworkServices.ServiceClient.UAS_ClientGroupClient();
                    List<KMPlatform.Entity.ClientGroup> clientGroups = new List<KMPlatform.Entity.ClientGroup>();
                    clientGroups = cgWorker.Proxy.SelectClient(AccessKey, clientId, true).Result;
                    cbNewGroupID.ItemsSource = clientGroups;
                }
            }
        }
        private void cbNewGroupID_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (cbNewGroupID.SelectedValue != null)
            {
                int clientGroupID = -1;
                int.TryParse(cbNewGroupID.SelectedValue.ToString(), out clientGroupID);
                if (clientGroupID > -1)
                {
                    if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.ClientGroups.Exists(x => x.ClientGroupID == clientGroupID))
                    {
                        List<KMPlatform.Entity.Client> clients = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.ClientGroups.Single(x => x.ClientGroupID == clientGroupID).Clients;
                        cbNewClientID.ItemsSource = clients;
                    }
                }
            }
        }
        private void btnNewSave_Click(object sender, RoutedEventArgs e)
        {
            RadButton btn = sender as RadButton;
            UserContainer uc = btn.DataContext as UserContainer;
            if (uc == null)
                return;
            bool isValidEmail = Core_AMS.Utilities.StringFunctions.isEmail(uc.Email);

            if (string.IsNullOrEmpty(uc.FirstName) || string.IsNullOrEmpty(uc.LastName) || string.IsNullOrEmpty(uc.Email) || string.IsNullOrEmpty(uc.UserName) || string.IsNullOrEmpty(uc.PassWord) ||
                uc.DefaultClientGroupID == 0 || uc.SecurityGroupID == 0)
            {
                Core_AMS.Utilities.WPF.Message("Please complete all required fields to save User.", MessageBoxButton.OK, MessageBoxImage.Error, "Save Aborted");
            }
            else
            {
                if (Users.FirstOrDefault(x => x.UserName.Equals(uc.UserName, StringComparison.CurrentCultureIgnoreCase)) != null)
                {
                    Core_AMS.Utilities.WPF.MessageError("This User Name has already been used.");
                    return;
                }

                if (isValidEmail == false)
                {
                    Core_AMS.Utilities.WPF.Message("Email Address provided is not valid.  Please make corrections and submit again.", MessageBoxButton.OK, MessageBoxImage.Error, "Save Aborted");
                }
                else
                {
                    userWorker = FrameworkServices.ServiceClient.UAS_UserClient();
                    FrameworkUAS.Service.Response<bool> emailCheckResponse = userWorker.Proxy.EmailExist(AccessKey, uc.Email);

                    if ((emailCheckResponse.Result == true || emailCheckResponse.Result == false) && emailCheckResponse.Result == false)
                    {
                        KMPlatform.Entity.User newUser = uc.GetUserEntity();
                        newUser.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                        newUser.DateCreated = DateTime.Now;

                        FrameworkUAS.Service.Response<int> newUserResp = userWorker.Proxy.Save(AccessKey, newUser);
                        if (newUserResp.Result != null && newUserResp.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                        {
                            newUser.UserID = newUserResp.Result;
                            UserContainer newUC = new UserContainer(newUser, clientGroups, clients);

                            //insert to ClientGroupUserMap
                            FrameworkUAS.Entity.ClientGroupUserMap cgum = new FrameworkUAS.Entity.ClientGroupUserMap();
                            cgum.ClientGroupUserMapID = 0;
                            cgum.ClientGroupID = (uc.DefaultClientGroupID ?? 0);
                            cgum.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                            cgum.DateCreated = DateTime.Now;
                            cgum.IsActive = true;
                            cgum.UserID = newUser.UserID;
                            cgumWorker.Proxy.Save(AccessKey, cgum);

                            //insert into UserClientSecurityGroupMap
                            KMPlatform.Entity.UserClientSecurityGroupMap ucsGroupMap = new KMPlatform.Entity.UserClientSecurityGroupMap();
                            ucsGroupMap.UserClientSecurityGroupMapID = 0;
                            ucsGroupMap.ClientID = (uc.DefaultClientID ?? 0);
                            ucsGroupMap.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                            ucsGroupMap.DateCreated = DateTime.Now;
                            ucsGroupMap.DateUpdated = null;
                            ucsGroupMap.IsActive = true;
                            ucsGroupMap.UserID = newUser.UserID;
                            ucsGroupMap.UpdatedByUserID = null;
                            ucsGroupMap.SecurityGroupID = (uc.SecurityGroupID ?? 0);
                            UCSGroupWorker.Proxy.Save(AccessKey, ucsGroupMap);

                            Users.Add(newUC);

                            Core_AMS.Utilities.WPF.Message("Save successful.");
                        }
                        else
                        {
                            Core_AMS.Utilities.WPF.MessageError("There was an issue saving the User.");
                        }
                        CloseWindow();
                    }
                    else
                    {
                        Core_AMS.Utilities.WPF.Message("This Email Address is already in use by another user.", MessageBoxButton.OK, MessageBoxImage.Error, "Email Already In Use");
                    }
                }
            }
        }
        private void btnNewCancel_Click(object sender, RoutedEventArgs e)
        {
            grdNewUser.DataContext = null;
            CloseWindow();
        }
        private void CloseWindow()
        {
            grdUser.IsEnabled = true;
            btnCreateUser.IsEnabled = true;
            btnSearch.IsEnabled = true;
            btnReset.IsEnabled = true;
            grdEdit.BeginAnimation(Grid.OpacityProperty, opacityReverseAnimate);
            grdNewUser.Opacity = 0;
            grdUser.SelectedItem = null;
        }
        #endregion

        #region User Grid Operations
        private void cbClientGroup_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RadComboBox rcb = (RadComboBox)sender;
            UserContainer uc = rcb.DataContext as UserContainer;
            int groupID = -1;
            int.TryParse(rcb.SelectedIndex.ToString(), out groupID);
            if (groupID > -1 && uc != null)
            {
                KMPlatform.Entity.ClientGroup cg = uc.ClientGroups.Where(x => x.ClientGroupID == groupID).FirstOrDefault();
                if (cg != null)
                {
                    List<KMPlatform.Entity.Client> clients = uc.ClientGroups.Single(x => x.ClientGroupID == cg.ClientGroupID).Clients;
                    uc.ClientList.Clear();
                    foreach (KMPlatform.Entity.Client c in clients)
                        uc.ClientList.Add(c);                  
                }
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            UserContainer uc = btn.DataContext as UserContainer;
            if (uc != null)
            {
                ToggleUserVisibility(uc);
                grdUser.SelectedItem = null;
            }
        }      
        private void ToggleUserVisibility(UserContainer uc)
        {
            if (uc.IsOpen)
            {
                uc.IsOpen = false;
                //uc.RevertChanges();
            }
            else
            {
                uc.IsOpen = true;
            }
        }
        private void grdUser_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //GridViewRow row = sender as GridViewRow;
            //if (row != null)
            //{
            //    UserContainer u = row.DataContext as UserContainer;
            //    if (u != null)
            //    {
            //        ToggleUserVisibility(u);
            //    }
            //}
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            UserContainer uc = btn.DataContext as UserContainer;
            if (uc != null)
            {
                KMPlatform.Entity.User user = uc.GetUserEntity();
                intResponse = userWorker.Proxy.Save(AccessKey, user);
                if (intResponse.Result > 0 && intResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    Core_AMS.Utilities.WPF.Message("Save successful.");
                    uc.OriginalUser = user;
                    uc.InfoChanged = false;
                    uc.IsOpen = false;
                    grdUser.SelectedItem = null;
                }
            }
        }   
        #endregion


    }
}