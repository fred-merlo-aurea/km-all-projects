using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FrameworkUAS.Object;
using System.Collections.ObjectModel;

namespace ControlCenter.Modules
{
    /// <summary>
    /// Interaction logic for ClientGroupMgmt.xaml
    /// </summary>
    public partial class ClientGroupMgmt : UserControl
    {
        #region Services/Responses
        FrameworkServices.ServiceClient<UAS_WS.Interface.IClientGroup> clientGroupW = FrameworkServices.ServiceClient.UAS_ClientGroupClient();
        FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> clientW = FrameworkServices.ServiceClient.UAS_ClientClient();
        FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeData = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        FrameworkUAS.Service.Response<int> intResponse = new FrameworkUAS.Service.Response<int>();
        #endregion
        #region Entities/Lists
        List<KMPlatform.Entity.ClientGroup> clientGroupList = new List<KMPlatform.Entity.ClientGroup>();
        List<KMPlatform.Entity.Client> clientList = new List<KMPlatform.Entity.Client>();
        List<string> activeTypes = new List<string>();
        List<FrameworkUAD_Lookup.Entity.Code> colorList { get; set; }
        List<KMPlatform.Entity.User> userList = new List<KMPlatform.Entity.User>();
        private Guid AccessKey;
        #endregion
        #region Properties, Classes, & Enums
        private ObservableCollection<ClientGroupContainer> _clientGroups = new ObservableCollection<ClientGroupContainer>();
        public ObservableCollection<ClientGroupContainer> ClientGroups { get { return _clientGroups; } }
        public class ClientGroupContainer : INotifyPropertyChanged
        {
            private int _clientGroupID;
            private int _masterClientID;
            private string _groupName;
            private string _description;
            private string _color;
            private bool _isActive;
            private DateTime _dateCreated;
            private DateTime? _dateUpdated;
            private int _createdByUserID;
            private bool _infoChanged;
            private bool _isOpen;
            private ObservableCollection<KMPlatform.Entity.Client> _clientList = new ObservableCollection<KMPlatform.Entity.Client>();
            private ObservableCollection<FrameworkUAD_Lookup.Entity.Code> _colors = new ObservableCollection<FrameworkUAD_Lookup.Entity.Code>();

            public string GroupName
            {
                get { return _groupName; }
                set
                {
                    _groupName = value;
                    CheckChanges();
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("GroupName"));
                    }
                }
            }
            public int MasterClientID
            {
                get { return _masterClientID; }
                set
                {
                    _masterClientID = value;
                    CheckChanges();
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("MasterClientID"));
                    }
                }
            }
            public string Description
            {
                get { return _description; }
                set
                {
                    _description = value;
                    CheckChanges();
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Description"));
                    }
                }
            }
            public string Color
            {
                get { return _color; }
                set
                {
                    if (value != null)
                        _color = value.First().ToString().ToUpper() + value.Substring(1);
                    else
                        _color = value;
                    CheckChanges();
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Color"));
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
                    if(null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("DateUpdated"));
                    }
                }
            }
            public KMPlatform.Entity.ClientGroup OriginalGroup { get; set; }
            public ObservableCollection<KMPlatform.Entity.Client> ClientList { get { return _clientList; } }
            public ObservableCollection<FrameworkUAD_Lookup.Entity.Code> Colors { get { return _colors; } }
            public bool InfoChanged
            {
                get { return _infoChanged; }
                set
                {
                    _infoChanged = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("InfoChanged"));
                    }
                }
            }
            public bool IsOpen
            {
                get { return _isOpen; }
                set
                {
                    _isOpen = value;
                    RevertChanges();
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IsOpen"));
                    }
                }
            }
            public void RevertChanges()
            {
                this.DateUpdated = OriginalGroup.DateUpdated;
                this.GroupName = OriginalGroup.ClientGroupName;
                this.Description = OriginalGroup.ClientGroupDescription;
                this.Color = OriginalGroup.Color;
                this.IsActive = OriginalGroup.IsActive;
                _dateCreated = OriginalGroup.DateCreated;
            }
            public void CheckChanges()
            {
                bool infoChanged = false;
                if (_groupName != OriginalGroup.ClientGroupName)
                    infoChanged = true;
                if (_description != OriginalGroup.ClientGroupDescription)
                    infoChanged = true;
                if (!_color.Equals(OriginalGroup.Color, StringComparison.CurrentCultureIgnoreCase))
                    infoChanged = true;
                if (_isActive != OriginalGroup.IsActive)
                    infoChanged = true;

                if (this.InfoChanged != infoChanged)
                    this.InfoChanged = infoChanged;
            }
            public KMPlatform.Entity.ClientGroup GetClientGroupEntity()
            {
                KMPlatform.Entity.ClientGroup rtnItem = new KMPlatform.Entity.ClientGroup();
                rtnItem.ClientGroupDescription = _description;
                rtnItem.ClientGroupID = _clientGroupID;
                rtnItem.ClientGroupName = _groupName;
                rtnItem.Color = _color;
                rtnItem.IsActive = _isActive;

                if(_masterClientID > 0)
                {
                    this.DateUpdated = DateTime.Now;
                    rtnItem.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                }
                else
                {
                    this.DateCreated = DateTime.Now;
                    rtnItem.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                }

                return rtnItem;
            }

            public ClientGroupContainer(KMPlatform.Entity.ClientGroup cg, List<KMPlatform.Entity.Client> clients, List<FrameworkUAD_Lookup.Entity.Code> colors)
            {
                _clientGroupID = cg.ClientGroupID;
                _groupName = cg.ClientGroupName;
                _description = cg.ClientGroupDescription;
                if (cg.Color != null)
                    _color = cg.Color.First().ToString().ToUpper() + cg.Color.Substring(1);
                else
                    _color = cg.Color;
                _isActive = cg.IsActive;
                _dateCreated = cg.DateCreated;
                _dateUpdated = cg.DateUpdated;
                _createdByUserID = cg.CreatedByUserID;
                _isOpen = false;
                _clientList = new ObservableCollection<KMPlatform.Entity.Client>(clients);
                _colors = new ObservableCollection<FrameworkUAD_Lookup.Entity.Code>(colors);
                this.OriginalGroup = cg;
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

        public ClientGroupMgmt()
        {
            if (AppData.IsKmUser() == true)
            {
                InitializeComponent();             
                AccessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
                LoadData();
            }
            else
            {
                Core_AMS.Utilities.WPF.MessageAccessDenied();
            }
        }
        private void LoadData()
        {
            activeTypes.Add(ActiveType.All.ToString());
            activeTypes.Add(ActiveType.Active.ToString());
            activeTypes.Add(ActiveType.Not_Active.ToString().Replace("_", " "));
            lstActive.ItemsSource = activeTypes;                   
            busy.IsBusy = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                clientGroupList = clientGroupW.Proxy.SelectClient(AccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID).Result;
                colorList = codeData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Color_Wheel).Result;                        
                //userList = userWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
                clientList = clientW.Proxy.SelectForClientGroup(AccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.ClientGroupID).Result;
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                foreach (KMPlatform.Entity.ClientGroup cg in clientGroupList)
                    ClientGroups.Add(new ClientGroupContainer(cg, clientList, colorList));
                                   
                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();                      
        }
        private void lstEnabled_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            this.ClientGroups.Clear();
            string item = lb.SelectedItem as string;
            if (item == ActiveType.All.ToString())
            {
                foreach (KMPlatform.Entity.ClientGroup c in clientGroupList)
                    this.ClientGroups.Add(new ClientGroupContainer(c, clientList, colorList));
            }
            else if (item == ActiveType.Active.ToString())
            {
                foreach (KMPlatform.Entity.ClientGroup c in clientGroupList.Where(x => x.IsActive == true))
                    this.ClientGroups.Add(new ClientGroupContainer(c, clientList, colorList));
            }
            else
            {
                foreach (KMPlatform.Entity.ClientGroup c in clientGroupList.Where(x => x.IsActive == false))
                    this.ClientGroups.Add(new ClientGroupContainer(c, clientList, colorList));
            }
        }

        #region ButtonMethods 

        private void rbNewClientGroup_Click(object sender, RoutedEventArgs e)
        {         
            this.Background = System.Windows.Media.Brushes.DarkGray;            
            lstActive.IsEnabled = false;
            rbNewClientGroup.IsEnabled = false;
            grdClientGroup.IsEnabled = false;
            grdClientGroup.Background = System.Windows.Media.Brushes.DarkGray;
            //rwNew.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnNewSave_Click(object sender, RoutedEventArgs e)
        {
            string clientGroupName = tbClientGroupName.Text.Trim();
            string desc = tbClientGroupDescription.Text.Trim();
            bool isActive = false;
            if (cbIsActive.IsChecked.HasValue)
                isActive = cbIsActive.IsChecked.Value;
            int masterClientID = -1;
            int.TryParse(rcbMasterClientID.SelectedValue.ToString(), out masterClientID);
            string color = "";
            color = rcbColor.SelectedValue.ToString();

            DateTime todayDate = DateTime.Now;
            KMPlatform.Entity.ClientGroup newClientGroup = new KMPlatform.Entity.ClientGroup();

            //newClientGroup.MasterClientID = masterClientID;
            newClientGroup.ClientGroupName = clientGroupName;
            newClientGroup.ClientGroupDescription = desc;
            newClientGroup.Color = color;
            newClientGroup.IsActive = isActive;
            newClientGroup.DateCreated = todayDate;
            newClientGroup.DateUpdated = todayDate;
            newClientGroup.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            newClientGroup.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;            

            #region Check Data
            #region Null/Empty
            if (!(masterClientID > -1))
            {
                Core_AMS.Utilities.WPF.MessageError("Must select a master client.");                    
                grdClientGroup.Rebind();
                return;
            }
            if (string.IsNullOrEmpty(newClientGroup.ClientGroupName))
            {
                Core_AMS.Utilities.WPF.MessageError("Current client group name cannot be blank.");                    
                grdClientGroup.Rebind();
                return;
            }
            if (string.IsNullOrEmpty(newClientGroup.Color))
            {
                Core_AMS.Utilities.WPF.MessageError("Must select a color.");                    
                grdClientGroup.Rebind();
                return;
            }
            #endregion
            #region Duplicate
            //clientGroupList = userData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
            if (clientGroupList != null && clientGroupList.FirstOrDefault(x => x.ClientGroupName.Equals(newClientGroup.ClientGroupName, StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                Core_AMS.Utilities.WPF.MessageError("Must provide a unique client group name that hasn't been used.");
                return;
            }
            #endregion
            #endregion

            //newClientGroup.ClientGroupID = userData.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, newClientGroup).Result;
            if (newClientGroup.ClientGroupID > 0)
            {
                ResetNewWindow();

                List<KMPlatform.Entity.ClientGroup> clients = new List<KMPlatform.Entity.ClientGroup>();
                if (grdClientGroup.ItemsSource != null)                
                    clients = (List<KMPlatform.Entity.ClientGroup>)grdClientGroup.ItemsSource;                    
                
                clients.Add(newClientGroup);
                grdClientGroup.ItemsSource = null;
                grdClientGroup.ItemsSource = clients;

                CloseWindow();
                this.grdClientGroup.SelectedItem = null;
            }
            else
                Core_AMS.Utilities.WPF.MessageServiceError();

        }

        private void btnNewCancel_Click(object sender, RoutedEventArgs e)
        {
            ResetNewWindow();
            List<KMPlatform.Entity.ClientGroup> clientGroups = (List<KMPlatform.Entity.ClientGroup>)grdClientGroup.ItemsSource;
            grdClientGroup.ItemsSource = clientGroups;
            CloseWindow();
            this.grdClientGroup.SelectedItem = null;
        }

        private void ResetNewWindow()
        {
            rcbMasterClientID.SelectedIndex = -1;
            tbClientGroupName.Clear();
            tbClientGroupDescription.Clear();
            rcbColor.SelectedIndex = -1;
            cbIsActive.IsChecked = false;
        }

        private void CloseWindow()
        {
            this.Background = System.Windows.Media.Brushes.Transparent;
            lstActive.IsEnabled = true;
            rbNewClientGroup.IsEnabled = true;
            grdClientGroup.IsEnabled = true;
            grdClientGroup.Background = System.Windows.Media.Brushes.Transparent;
           // rwNew.Visibility = System.Windows.Visibility.Collapsed;
        }        

        private void rbClientDetails(object sender, RoutedEventArgs e)
        {
            if (this.Parent.GetType() == typeof(StackPanel))
            {
                if (this.grdClientGroup.SelectedItem != null)
                {
                    KMPlatform.Entity.Client c = (KMPlatform.Entity.Client)this.grdClientGroup.SelectedItem;
                    StackPanel sp = (StackPanel)this.Parent;
                    sp.Children.Clear();
                    ControlCenter.Modules.ClientControls.FTPSites ftps = new ClientControls.FTPSites(c);
                    sp.Children.Add(ftps);
                }
                else                
                    Core_AMS.Utilities.WPF.MessageError("No client selected. Please try again. If the problem persists, please contact Customer Support.");

            }
            else
                Core_AMS.Utilities.WPF.MessageError("An unexpected error occurred during when loading service features. Please try again. If the problem persists, please contact Customer Support.");

        }
        #endregion
        private void grdClientGroup_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            foreach (ClientGroupContainer c in this.ClientGroups.Where(x => x.IsOpen))
                c.IsOpen = false;

            ClientGroupContainer cg = grdClientGroup.SelectedItem as ClientGroupContainer;
            if (cg != null)
            {
                grdClientGroup.ScrollIntoView(cg);
                grdClientGroup.UpdateLayout();
                cg.IsOpen = true;
            }
        }               

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            ClientGroupContainer cg = btn.DataContext as ClientGroupContainer;
            if (cg != null)
            {
                KMPlatform.Entity.ClientGroup group = cg.GetClientGroupEntity();
                string required = "";
                //if (group.MasterClientID <= -1)
                //    required = "Master Client ID, ";
                if (string.IsNullOrEmpty(group.ClientGroupName))
                    required += "Client Group Name, ";
                if (string.IsNullOrEmpty(group.Color))
                    required += "Color";
                required = required.TrimEnd(' ');
                required = required.TrimEnd(',');
                if(required.Length > 0)
                    Core_AMS.Utilities.WPF.MessageError("The following fields can not be blank: " + required + ".");                
                else if (clientGroupList != null && !clientGroupList.Where(x => x.ClientGroupID != group.ClientGroupID).Select(x => x.ClientGroupName).Contains(group.ClientGroupName))
                {
                    intResponse = clientGroupW.Proxy.Save(AccessKey, group);
                    if (intResponse.Result > 0 && intResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    {
                        Core_AMS.Utilities.WPF.Message("Save successful.");
                        int index = clientGroupList.FindIndex(x => x.ClientGroupID == group.ClientGroupID);
                        if(index > -1)
                        {
                            clientGroupList.RemoveAt(index);
                            clientGroupList.Insert(index, group);
                        }
                        cg.OriginalGroup = group;
                        cg.InfoChanged = false;
                        cg.IsOpen = false;
                        grdClientGroup.SelectedItem = null;
                    }
                }
                else
                    Core_AMS.Utilities.WPF.MessageError("This Client Group Name is already being used.");
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            ClientGroupContainer cg = btn.DataContext as ClientGroupContainer;
            if (cg != null)
            {
                cg.IsOpen = false;
                grdClientGroup.SelectedItem = null;
            }
        }      
    }
}
