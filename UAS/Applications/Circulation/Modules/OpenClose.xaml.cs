using Circulation.Helpers;
using FrameworkUAD.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Telerik.Windows.Controls;

namespace Circulation.Modules
{
    /// <summary>
    /// Interaction logic for OpenClose.xaml
    /// </summary>
    public partial class OpenClose : UserControl
    {
        #region ServiceCalls
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IActionBackUp> aBackUpData = FrameworkServices.ServiceClient.UAD_ActionBackUpClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IIssue> issueData = FrameworkServices.ServiceClient.UAD_IssueClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> pubData = FrameworkServices.ServiceClient.UAD_ProductClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> publisherData = FrameworkServices.ServiceClient.UAS_ClientClient();
        //private FrameworkServices.ServiceClient<UAD_WS.Interface.IPublication> pubData = FrameworkServices.ServiceClient.UAS_PublicationClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IUser> userData = FrameworkServices.ServiceClient.UAS_UserClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IWaveMailing> waveMailingData = FrameworkServices.ServiceClient.UAD_WaveMailingClient();
        #endregion
        #region Variables/Lists
        private Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
        private KMPlatform.Entity.Client publishers = new KMPlatform.Entity.Client();
        private List<FrameworkUAD.Entity.Product> publications = new List<FrameworkUAD.Entity.Product>();
        private List<KMPlatform.Entity.Client> clientList = new List<KMPlatform.Entity.Client>();
        private List<KMPlatform.Entity.User> userList = new List<KMPlatform.Entity.User>();
        private ObservableCollection<TileMenuItem> tiles = new ObservableCollection<TileMenuItem>();
        private DoubleAnimation animateOpacity = new DoubleAnimation();
        private DoubleAnimation resetOpacity = new DoubleAnimation();
        private KMPlatform.Entity.Client client = new KMPlatform.Entity.Client();
        private FrameworkUAD.Entity.Issue myIssue = new FrameworkUAD.Entity.Issue();
        private FrameworkUAD.Entity.Product myPub = new FrameworkUAD.Entity.Product();
        private List<FrameworkUAD.Entity.Product> productList = new List<FrameworkUAD.Entity.Product>();
        public ObservableCollection<IssuePermissions> issuePermissions = new ObservableCollection<IssuePermissions>();
        private bool isKM = false;
        #endregion
        #region ServiceResponses
        private FrameworkUAS.Service.Response<KMPlatform.Entity.Client> publisherResponse = new FrameworkUAS.Service.Response<KMPlatform.Entity.Client>();
        //private FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.Publication>> publicationResponse = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.Publication>>();
        private FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>> publisherListResponse = new FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> pubResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
        private FrameworkUAS.Service.Response<List<KMPlatform.Entity.User>> userResponse = new FrameworkUAS.Service.Response<List<KMPlatform.Entity.User>>();
        private FrameworkUAS.Service.Response<List<Issue>> issueResponse = new FrameworkUAS.Service.Response<List<Issue>>();
        private FrameworkUAS.Service.Response<bool> boolResponse = new FrameworkUAS.Service.Response<bool>();
        private FrameworkUAS.Service.Response<List<WaveMailing>> waveMailingResponse = new FrameworkUAS.Service.Response<List<WaveMailing>>();
        private FrameworkUAS.Service.Response<int> intResponse = new FrameworkUAS.Service.Response<int>();
        #endregion
        #region Classes & Enums
        public class IssuePermissions : INotifyPropertyChanged
        {
            private bool? _permission;
            public string Type { get; set; }
            public bool? Permission
            {
                get { return _permission; }
                set
                {
                    _permission = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Permission"));
                    }
                }
            }
            public FrameworkUAD_Lookup.Enums.IssuePermissionTypes TypeEnum { get; set; }

            public IssuePermissions(FrameworkUAD_Lookup.Enums.IssuePermissionTypes type, bool? permission)
            {
                this.Type = type.ToString().Replace('_', ' ');
                this.Permission = permission;
                this.TypeEnum = type;
            }

            public event PropertyChangedEventHandler PropertyChanged;
            //private FrameworkUAD_Lookup.Enums.IssuePermissionTypes issuePermissionTypes;
        }

        public class TileMenuItem : INotifyPropertyChanged
        {
            public TileType TileName { get; set; }
            public string DisplayName { get; set; }

            public TileMenuItem(TileType name)
            {
                this.TileName = name;
                this.DisplayName = name.ToString().Replace("_", " ");
            }
            public event PropertyChangedEventHandler PropertyChanged;
        }
        #endregion

        public OpenClose()
        {
            InitializeComponent();

            accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
            client = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient;
            isKM = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.IsKmUser;
            icTiles.ItemsSource = tiles;
            myPub = new Product();

            animateOpacity = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(.4)
            };
            resetOpacity = new DoubleAnimation
            {
                To = 1,
                Duration = TimeSpan.FromSeconds(.4)
            };

            KMPlatform.Entity.Client c = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient;
            if (!FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.OpenedClients.Contains(c))
                FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.OpenedClients.Add(c);
            pubResponse = pubData.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            myPub = new Product();
            if (Helpers.Common.CheckResponse(pubResponse.Result, pubResponse.Status))
            {
                productList = pubResponse.Result.Where(x => x.IsCirc == true && x.IsActive == true).ToList();
                rcbPublication.ItemsSource = productList.OrderBy(x => x.PubCode);
                rcbPublication.DisplayMemberPath = "PubCode";
                rcbPublication.SelectedValuePath = "PubID";
            }
            userResponse = userData.Proxy.Select(accessKey, false);
            if (Helpers.Common.CheckResponse(userResponse.Result, userResponse.Status))
            {
                userList = userResponse.Result;
            }
        }

        #region ButtonClicks
        private void btnUnlock_Click(object sender, RoutedEventArgs e)
        {
            RadButton btnMe = sender as RadButton;
            string target = "";
            bool result = false;
            IssuePermissions i = btnMe.DataContext as IssuePermissions;
            KMPlatform.Entity.Client c = client;

            switch (i.TypeEnum)
            {
                #region Data Entry
                case FrameworkUAD_Lookup.Enums.IssuePermissionTypes.Data_Entry:
                    target = "Data Entry";
                    result = BuildMessage(target, true);
                    if (result)
                    {
                        myPub.AllowDataEntry = true;
                        myPub.AddRemoveAllowed = false;
                        myPub.ClientImportAllowed = false;
                        myPub.KMImportAllowed = false;
                        myPub.DateUpdated = DateTime.Now;
                        myPub.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                        pubData.Proxy.Save(accessKey, myPub, c.ClientConnections);
                        myIssue.IsClosed = false;
                        myIssue.DateOpened = DateTime.Now;
                        myIssue.OpenedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                        issueData.Proxy.Save(accessKey, myIssue, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                        i.Permission = true;

                        foreach (IssuePermissions ip in issuePermissions)
                        {
                            if (ip.TypeEnum != FrameworkUAD_Lookup.Enums.IssuePermissionTypes.Data_Entry)
                                ip.Permission = false;
                        }

                        animateOpacity.Completed += (s, _) =>
                        {
                            tiles.Clear();
                            //tiles.Add(new TileMenuItem(TileType.File_Status));
                            icTiles.BeginAnimation(ItemsControl.OpacityProperty, resetOpacity);
                        };
                        icTiles.BeginAnimation(ItemsControl.OpacityProperty, animateOpacity);
                    }
                    else
                        return;
                    break;
                #endregion
                #region External Imports
                case FrameworkUAD_Lookup.Enums.IssuePermissionTypes.External_Import:
                    if (myPub.AllowDataEntry == false && myPub.AddRemoveAllowed == false && myIssue.IsClosed == false)
                    {
                        target = "External Imports";
                        result = BuildMessage(target, true);
                        if (result)
                        {
                            myPub.ClientImportAllowed = true;
                            myPub.DateUpdated = DateTime.Now;
                            myPub.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                            pubData.Proxy.Save(accessKey, myPub, c.ClientConnections);
                            i.Permission = true;
                        }
                        else
                            return;
                    }
                    else
                        System.Windows.MessageBox.Show("Data entry needs to be closed for this product before you can begin list imports.", "Warning", MessageBoxButton.OK);
                    break;
                #endregion
                #region Internal Imports
                case FrameworkUAD_Lookup.Enums.IssuePermissionTypes.Internal_Import:
                    if (myPub.AllowDataEntry == false && myPub.AddRemoveAllowed == false && myIssue.IsClosed == false)
                    {
                        target = "Internal Imports";
                        result = BuildMessage(target, true);
                        if (result)
                        {
                            myPub.KMImportAllowed = true;
                            myPub.DateUpdated = DateTime.Now;
                            myPub.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                            pubData.Proxy.Save(accessKey, myPub, c.ClientConnections);
                            i.Permission = true;
                        }
                        else
                            return;
                    }
                    else
                        System.Windows.MessageBox.Show("Data entry needs to be closed for this product before you can begin list imports.", "Warning", MessageBoxButton.OK);
                    break;
                #endregion
                #region Add Remove
                case FrameworkUAD_Lookup.Enums.IssuePermissionTypes.Add_Remove:
                    if (myPub.ClientImportAllowed == false && myPub.KMImportAllowed == false && myPub.AllowDataEntry == false && myIssue.IsClosed == false)
                    {
                        target = "Add/Removes";
                        result = BuildMessage(target, true);
                        if (result)
                        {
                            myPub.AddRemoveAllowed = true;
                            myPub.DateUpdated = DateTime.Now;
                            myPub.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                            pubData.Proxy.Save(accessKey, myPub, c.ClientConnections);
                            i.Permission = true;
                        }
                        else
                            return;
                    }
                    else
                        System.Windows.MessageBox.Show("Data Entry, External and Internal imports need to be closed for this product before you can begin Add/Removes.", "Warning", MessageBoxButton.OK);
                    break;
                #endregion
            }
        }
        private void btnLock_Click(object sender, RoutedEventArgs e)
        {
            RadButton btnMe = sender as RadButton;
            string target = "";
            bool result = false;
            IssuePermissions i = btnMe.DataContext as IssuePermissions;
            KMPlatform.Entity.Client c = client;
            switch (i.TypeEnum)
            {
                #region Data Entry
                case FrameworkUAD_Lookup.Enums.IssuePermissionTypes.Data_Entry:
                    target = "Data Entry";
                    result = BuildMessage(target, false);
                    if (result)
                    {
                        myPub.AllowDataEntry = false;
                        myPub.ClientImportAllowed = true;
                        myPub.KMImportAllowed = true;
                        myPub.DateUpdated = DateTime.Now;
                        myPub.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                        pubData.Proxy.Save(accessKey, myPub, c.ClientConnections);
                        i.Permission = false;

                        foreach (IssuePermissions ip in issuePermissions)
                        {
                            if (ip.TypeEnum == FrameworkUAD_Lookup.Enums.IssuePermissionTypes.External_Import)
                                ip.Permission = true;
                            if (ip.TypeEnum == FrameworkUAD_Lookup.Enums.IssuePermissionTypes.Internal_Import)
                                ip.Permission = true;
                        }

                        animateOpacity.Completed += (s, _) =>
                        {
                            tiles.Clear();
                            //tiles.Add(new TileMenuItem(TileType.File_Status));                            
                            tiles.Add(new TileMenuItem(TileType.Import_File));
                            tiles.Add(new TileMenuItem(TileType.Edit_File_Mapping));
                            tiles.Add(new TileMenuItem(TileType.Record_Update));
                            icTiles.BeginAnimation(ItemsControl.OpacityProperty, resetOpacity);
                        };
                        icTiles.BeginAnimation(ItemsControl.OpacityProperty, animateOpacity);

                        if (FrameworkUAS.Object.AppData.myAppData.OpenWindowCount > 0)
                        {
                            foreach (Window win in Application.Current.Windows)
                            {
                                if (win.GetType() == typeof(Circulation.Windows.Popout))
                                {
                                    Circulation.Windows.Popout pop = win as Circulation.Windows.Popout;
                                    pop.IgnoreSaveMessage = true;
                                    pop.Close();
                                }
                            }
                        }

                    }
                    else
                        return;
                    break;
                #endregion
                #region External Imports
                case FrameworkUAD_Lookup.Enums.IssuePermissionTypes.External_Import:
                    if (myPub.AllowDataEntry == false && myPub.AddRemoveAllowed == false)
                    {
                        target = "External Imports";
                        result = BuildMessage(target, false);
                        if (result)
                        {
                            myPub.ClientImportAllowed = false;
                            i.Permission = false;

                            if (myPub.KMImportAllowed == false)
                            {
                                myPub.AddRemoveAllowed = true;
                                busy.IsBusy = true;
                                busy.IsIndeterminate = true;
                                BackgroundWorker bw = new BackgroundWorker();
                                bw.DoWork += (o, ea) =>
                                {
                                    aBackUpData.Proxy.Bulk_Insert(accessKey, myPub.PubID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                                };
                                bw.RunWorkerCompleted += (o, ea) =>
                                {
                                    foreach (IssuePermissions ip in issuePermissions)
                                    {
                                        if (ip.TypeEnum == FrameworkUAD_Lookup.Enums.IssuePermissionTypes.Add_Remove)
                                        {
                                            ip.Permission = true;
                                        }
                                    }
                                    animateOpacity.Completed += (s, _) =>
                                    {
                                        tiles.Clear();
                                        //tiles.Add(new TileMenuItem(TileType.File_Status));                                        
                                        tiles.Add(new TileMenuItem(TileType.Add_Remove));
                                        icTiles.BeginAnimation(ItemsControl.OpacityProperty, resetOpacity);
                                    };
                                    icTiles.BeginAnimation(ItemsControl.OpacityProperty, animateOpacity);
                                    busy.IsBusy = false;
                                };
                                bw.RunWorkerAsync();
                            }

                            myPub.DateUpdated = DateTime.Now;
                            myPub.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                            pubData.Proxy.Save(accessKey, myPub, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                        }
                        else
                            return;
                    }
                    else
                        System.Windows.MessageBox.Show("Data entry needs to be closed for this product before you can close list imports.", "Warning", MessageBoxButton.OK);
                    break;
                #endregion
                #region Internal Imports
                case FrameworkUAD_Lookup.Enums.IssuePermissionTypes.Internal_Import:
                    if (myPub.AllowDataEntry == false && myPub.AddRemoveAllowed == false)
                    {
                        target = "Internal Imports";
                        result = BuildMessage(target, false);
                        if (result)
                        {
                            myPub.KMImportAllowed = false;
                            i.Permission = false;

                            if (myPub.ClientImportAllowed == false)
                            {
                                myPub.AddRemoveAllowed = true;
                                busy.IsBusy = true;
                                busy.IsIndeterminate = true;
                                BackgroundWorker bw = new BackgroundWorker();
                                bw.DoWork += (o, ea) =>
                                {
                                    aBackUpData.Proxy.Bulk_Insert(accessKey, myPub.PubID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                                };
                                bw.RunWorkerCompleted += (o, ea) =>
                                {
                                    foreach (IssuePermissions ip in issuePermissions)
                                    {
                                        if (ip.TypeEnum == FrameworkUAD_Lookup.Enums.IssuePermissionTypes.Add_Remove)
                                        {
                                            ip.Permission = true;
                                        }
                                    }
                                    animateOpacity.Completed += (s, _) =>
                                    {
                                        tiles.Clear();
                                        //tiles.Add(new TileMenuItem(TileType.File_Status));                                        
                                        tiles.Add(new TileMenuItem(TileType.Add_Remove));
                                        icTiles.BeginAnimation(ItemsControl.OpacityProperty, resetOpacity);
                                    };
                                    icTiles.BeginAnimation(ItemsControl.OpacityProperty, animateOpacity);
                                    busy.IsBusy = false;
                                };
                                bw.RunWorkerAsync();
                            }

                            myPub.DateUpdated = DateTime.Now;
                            myPub.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                            pubData.Proxy.Save(accessKey, myPub, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                        }
                        else
                            return;
                    }
                    else
                        System.Windows.MessageBox.Show("Data entry needs to be closed for this product before you can close list imports.", "Warning", MessageBoxButton.OK);
                    break;
                #endregion
                #region Add Remove
                case FrameworkUAD_Lookup.Enums.IssuePermissionTypes.Add_Remove:
                    if (myPub.KMImportAllowed == false && myPub.ClientImportAllowed == false && myPub.AllowDataEntry == false)
                    {
                        if (myPub.HasPaidRecords && myPub.UseSubGen)
                        {
                            MessageBoxResult mb = MessageBox.Show("Would you like to close the paid subscriptions and advance to the next issue at this time? This action is not reversible. Are you sure you want to continue?", "Warning", MessageBoxButton.YesNo);
                            if (mb == MessageBoxResult.Yes)
                            {
                                //FrameworkSubGen.BusinessLogic.API methods can be called directly
                                //FrameworkSubGen.BusinessLogic need to go through Service layer

                                FrameworkSubGen.Entity.Enums.Client sgClient = FrameworkSubGen.Entity.Enums.GetClient(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.FtpFolder);
                                FrameworkSubGen.BusinessLogic.Publication pWrk = new FrameworkSubGen.BusinessLogic.Publication();
                                FrameworkSubGen.Entity.Publication sgPub = pWrk.SelectKmPubId(myPub.PubID, myPub.ClientID);

                                FrameworkSubGen.BusinessLogic.API.MailingList mlW = new FrameworkSubGen.BusinessLogic.API.MailingList();
                                //this call here is the one in SG that closes and advances issue
                                //int mailing_id = mlW.GenerateList(sgClient, sgPub.publication_id);

                                //INSERT the above parameters and result id into GenerateList Table.
                                //List<FrameworkSubGen.Entity.SubscriptionList> subs = mlW.GetSubscriptionList(sgClient, mailing_id);

                                //INSERT resulting list into DB.
                                //Compare list to subscribers in the UAD. If there are missing subscribers, do a sync.
                                //TEMP CODE
                            }
                        }
                        target = "Add/Removes";
                        //result = BuildMessage(target, false);
                        result = true;
                        if (result)
                        {
                            myPub.AddRemoveAllowed = false;
                            myPub.DateUpdated = DateTime.Now;
                            myPub.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                            //pubData.Proxy.Save(accessKey, myPub,c);
                            pubData.Proxy.Save(accessKey, myPub, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                            myIssue.IsClosed = true;
                            myIssue.DateClosed = DateTime.Now;
                            myIssue.ClosedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                            issueData.Proxy.Save(accessKey, myIssue, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                            i.Permission = false;

                            animateOpacity.Completed += (s, _) =>
                            {
                                tiles.Clear();
                                //tiles.Add(new TileMenuItem(TileType.File_Status));
                                tiles.Add(new TileMenuItem(TileType.Import_Comps));
                                tiles.Add(new TileMenuItem(TileType.Edit_File_Mapping));
                                tiles.Add(new TileMenuItem(TileType.Issue_Splits));
                                icTiles.BeginAnimation(ItemsControl.OpacityProperty, resetOpacity);
                            };
                            icTiles.BeginAnimation(ItemsControl.OpacityProperty, animateOpacity);
                        }
                        else
                            return;
                    }
                    else
                        System.Windows.MessageBox.Show("List imports need to be closed for this product before you can close Add/Removes.", "Warning", MessageBoxButton.OK);
                    break;
                #endregion
            }
        }
        private void btnNewIssue_Click(object sender, RoutedEventArgs e)
        {
            if (txtNewIssueCode.Text != "" && txtNewIssueName.Text != "")
            {
                Issue i = new Issue();
                i.PublicationId = myPub.PubID;
                i.OpenedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                i.IssueName = txtNewIssueName.Text;
                i.IssueCode = txtNewIssueCode.Text;
                i.DateOpened = DateTime.Now;
                i.DateCreated = DateTime.Now;
                i.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                intResponse = issueData.Proxy.Save(accessKey, i, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                myPub.AllowDataEntry = true;
                myPub.ClientImportAllowed = false;
                myPub.KMImportAllowed = false;
                myPub.AddRemoveAllowed = false;
                myPub.IsOpenCloseLocked = false;
                myPub.DateUpdated = DateTime.Now;
                myPub.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                intResponse = pubData.Proxy.Save(accessKey, myPub, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                if (intResponse.Result > 0 && intResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    System.Windows.MessageBox.Show("New issue created successfully.", "New Issue", MessageBoxButton.OK);
                    spNewIssue.Visibility = System.Windows.Visibility.Collapsed;
                    txtNewIssueName.Text = "";
                    txtNewIssueCode.Text = "";
                    UpdateIssueDetails(myPub.PubID);
                }
            }
            else
                MessageBox.Show("Please enter a Issue Name and Issue Code");
        }
        #endregion

        #region ChecksAndSelections
        private void rcbPublication_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RadComboBox rcb = sender as RadComboBox;
            int publicationID = -1;

            if (rcb.SelectedItem != null)
            {
                int.TryParse(rcb.SelectedValue.ToString(), out publicationID);
                UpdateIssueDetails(publicationID);
            }
        }
        #endregion

        #region Helper Methods
        private bool BuildMessage(string target, bool unlock)
        {
            string msg = "";
            if (unlock == true)
                msg = "Are you sure you want to open " + target + "?";
            else
                msg = "Are you sure you want to close " + target + "?";

            MessageBoxResult messBox = System.Windows.MessageBox.Show(msg, "Warning", MessageBoxButton.YesNo);
            if (messBox == MessageBoxResult.Yes)
                return true;
            else if (messBox == MessageBoxResult.No)
                return false;

            return false;
        }
        private void PopOutWindow(System.Windows.Controls.UserControl uc, string windowTitle)
        {
            Windows.Popout win = new Windows.Popout(uc);
            win.Title = windowTitle;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ShowDialog();
        }
        private void WpfControlPopOutWindow(System.Windows.Controls.UserControl uc, string windowTitle)
        {
            WpfControls.WindowsAndDialogs.PopOut win = new WpfControls.WindowsAndDialogs.PopOut(KMPlatform.BusinessLogic.Enums.Applications.Circulation, uc);
            win.Title = windowTitle;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ShowDialog();
        }
        private void UpdateIssueDetails(int publicationID, bool fromIssueSplits = false)
        {
            pubResponse = new FrameworkUAS.Service.Response<List<Product>>();
            pubResponse = pubData.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if (Helpers.Common.CheckResponse(pubResponse.Result, pubResponse.Status))
            {
                productList = pubResponse.Result.Where(x => x.IsCirc == true && x.IsActive == true).OrderBy(x => x.PubCode).ToList();
            }

            spNewIssue.Visibility = Visibility.Collapsed;
            spIssueDetails.Visibility = System.Windows.Visibility.Hidden;
            spWaveDetails.Visibility = System.Windows.Visibility.Hidden;
            icIssuePermissions.ItemsSource = null;
            issuePermissions.Clear();

            if (fromIssueSplits == false)
            {

                FrameworkUAD.Entity.Product prevPub = new FrameworkUAD.Entity.Product();
                if (myPub != null)
                {
                    int currentPubID = myPub.PubID;
                    prevPub = productList.Where(x => x.PubID == currentPubID).FirstOrDefault();
                }

                myPub = productList.Where(x => x.PubID == publicationID).FirstOrDefault();
                if (myPub != null && (myPub.IsOpenCloseLocked == false || myPub.IsOpenCloseLocked == null))
                {
                    if (prevPub != null)
                    {
                        prevPub.IsOpenCloseLocked = false;
                        prevPub.DateUpdated = DateTime.Now;
                        prevPub.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                        pubData.Proxy.Save(accessKey, prevPub, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                    }
                    myPub.DateUpdated = DateTime.Now;
                    myPub.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                    myPub.IsOpenCloseLocked = true;
                    pubData.Proxy.Save(accessKey, myPub, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                    issueResponse = issueData.Proxy.SelectForPublication(accessKey, publicationID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                    if (Helpers.Common.CheckResponse(issueResponse.Result, issueResponse.Status))
                    {
                        myIssue = issueResponse.Result.Where(x => x.IsComplete == false).FirstOrDefault();
                        #region Existing Issue
                        if (myIssue != null)
                        {
                            waveMailingResponse = waveMailingData.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                            if (Common.CheckResponse(waveMailingResponse.Result, waveMailingResponse.Status))
                            {
                                WaveMailing wm = waveMailingResponse.Result.Where(x => x.IssueID == myIssue.IssueId).LastOrDefault();
                                if (wm != null)
                                {
                                    txtWaveNumber.Text = (++wm.WaveNumber).ToString();
                                    spWaveDetails.Visibility = System.Windows.Visibility.Visible;
                                }
                            }
                            tiles.Clear();
                            //tiles.Add(new TileMenuItem(TileType.File_Status));
                            txtIssueName.Text = myIssue.IssueName;
                            txtOpened.Text = myIssue.DateCreated.ToString();
                            txtUpdated.Text = myPub.DateUpdated.ToString();
                            spIssueDetails.Visibility = System.Windows.Visibility.Visible;
                            issuePermissions.Add(new IssuePermissions(FrameworkUAD_Lookup.Enums.IssuePermissionTypes.Data_Entry, myPub.AllowDataEntry));
                            issuePermissions.Add(new IssuePermissions(FrameworkUAD_Lookup.Enums.IssuePermissionTypes.External_Import, myPub.ClientImportAllowed));
                            issuePermissions.Add(new IssuePermissions(FrameworkUAD_Lookup.Enums.IssuePermissionTypes.Internal_Import, myPub.KMImportAllowed));
                            issuePermissions.Add(new IssuePermissions(FrameworkUAD_Lookup.Enums.IssuePermissionTypes.Add_Remove, myPub.AddRemoveAllowed));
                            icIssuePermissions.ItemsSource = issuePermissions;

                            //CIRCTOUAD CHANGE BACK TO UAD PUB

                            if (myPub.ClientImportAllowed == true || myPub.KMImportAllowed == true)
                            {
                                tiles.Add(new TileMenuItem(TileType.Edit_File_Mapping));
                                tiles.Add(new TileMenuItem(TileType.Import_File));
                                tiles.Add(new TileMenuItem(TileType.Record_Update));
                            }
                            else if (myPub.AddRemoveAllowed == true)
                                tiles.Add(new TileMenuItem(TileType.Add_Remove));
                            else if (myPub.AllowDataEntry == false && myIssue.IsClosed == true)
                            {
                                tiles.Add(new TileMenuItem(TileType.Edit_File_Mapping));
                                tiles.Add(new TileMenuItem(TileType.Import_Comps));
                                tiles.Add(new TileMenuItem(TileType.Issue_Splits));
                            }

                            icTiles.BeginAnimation(ItemsControl.OpacityProperty, resetOpacity);
                        }
                        #endregion
                        #region First Issue
                        else
                        {
                            tiles.Clear();
                            MessageBoxResult result = System.Windows.MessageBox.Show("You do not have an issue assigned to this product yet. Would you like to create one now?", "Alert", MessageBoxButton.YesNo);
                            if (result == MessageBoxResult.Yes)
                            {
                                spNewIssue.Visibility = System.Windows.Visibility.Visible;
                                txtNewIssueName.Focus();
                            }
                            else
                            {
                                myPub.IsOpenCloseLocked = false;
                                myPub.DateUpdated = DateTime.Now;
                                myPub.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                                pubData.Proxy.Save(accessKey, myPub, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                                myPub = null;
                                rcbPublication.SelectedItem = null;
                            }
                        }
                        #endregion
                    }
                }
                else
                {
                    if (prevPub != null)
                    {
                        prevPub.DateUpdated = DateTime.Now;
                        prevPub.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                        prevPub.IsOpenCloseLocked = false;
                        pubData.Proxy.Save(accessKey, prevPub, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                    }
                    tiles.Clear();
                    FrameworkUAD.Entity.Product thisPub = productList.Where(x => x.PubID == publicationID).FirstOrDefault();
                    if (thisPub != null)
                    {
                        string user = "";
                        string email = "";
                        if (userList != null && userList.Count(x => x.UserID == thisPub.UpdatedByUserID) > 0)
                        {
                            user = userList.FirstOrDefault(x => x.UserID == thisPub.UpdatedByUserID).UserName;
                            email = "(" + userList.FirstOrDefault(x => x.UserID == thisPub.UpdatedByUserID).EmailAddress + ")";
                        }

                        MessageBox.Show("The Open Close page for this product is currently in use by another user " + user + " " + email + ". Please wait for the user to close the page.", "Warning", MessageBoxButton.OK);
                    }
                    myPub = null;
                    rcbPublication.SelectedItem = null;
                }
            }
            else
            {
                myPub = productList.Where(x => x.PubID == publicationID).FirstOrDefault();
                issueResponse = issueData.Proxy.SelectForPublication(accessKey, publicationID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                if (Helpers.Common.CheckResponse(issueResponse.Result, issueResponse.Status))
                {
                    myIssue = issueResponse.Result.Where(x => x.IsComplete == false).FirstOrDefault();
                    #region Existing Issue
                    if (myIssue != null)
                    {
                        waveMailingResponse = waveMailingData.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                        if (Common.CheckResponse(waveMailingResponse.Result, waveMailingResponse.Status))
                        {
                            WaveMailing wm = waveMailingResponse.Result.Where(x => x.IssueID == myIssue.IssueId).LastOrDefault();
                            if (wm != null)
                            {
                                txtWaveNumber.Text = (++wm.WaveNumber).ToString();
                                spWaveDetails.Visibility = System.Windows.Visibility.Visible;
                            }
                        }
                        tiles.Clear();
                        //tiles.Add(new TileMenuItem(TileType.File_Status));
                        txtIssueName.Text = myIssue.IssueName;
                        txtOpened.Text = myIssue.DateCreated.ToString();
                        txtUpdated.Text = myPub.DateUpdated.ToString();
                        spIssueDetails.Visibility = System.Windows.Visibility.Visible;
                        issuePermissions.Add(new IssuePermissions(FrameworkUAD_Lookup.Enums.IssuePermissionTypes.Data_Entry, myPub.AllowDataEntry));
                        issuePermissions.Add(new IssuePermissions(FrameworkUAD_Lookup.Enums.IssuePermissionTypes.External_Import, myPub.ClientImportAllowed));
                        issuePermissions.Add(new IssuePermissions(FrameworkUAD_Lookup.Enums.IssuePermissionTypes.Internal_Import, myPub.KMImportAllowed));
                        issuePermissions.Add(new IssuePermissions(FrameworkUAD_Lookup.Enums.IssuePermissionTypes.Add_Remove, myPub.AddRemoveAllowed));
                        icIssuePermissions.ItemsSource = issuePermissions;

                        if (myPub.ClientImportAllowed == true || myPub.KMImportAllowed == true)
                        {
                            tiles.Add(new TileMenuItem(TileType.Edit_File_Mapping));
                            tiles.Add(new TileMenuItem(TileType.Import_File));
                        }
                        else if (myPub.AddRemoveAllowed == true)
                            tiles.Add(new TileMenuItem(TileType.Add_Remove));
                        else if (myPub.AllowDataEntry == false && myIssue.IsClosed == true)
                        {
                            tiles.Add(new TileMenuItem(TileType.Edit_File_Mapping));
                            tiles.Add(new TileMenuItem(TileType.Import_Comps));
                            tiles.Add(new TileMenuItem(TileType.Issue_Splits));
                        }

                        icTiles.BeginAnimation(ItemsControl.OpacityProperty, resetOpacity);
                    }
                    #endregion
                }
            }
        }
        #endregion

        private void Tile_Open(object sender, RoutedEventArgs e)
        {
            //busy.IsBusy = true;
            //busy.IsIndeterminate = true;
            //BackgroundWorker bw = new BackgroundWorker();
            //bw.DoWork += (o, ea) =>
            //{
            Button me = sender as Button;
            TileMenuItem tm = me.DataContext as TileMenuItem;
            switch (tm.TileName)
            {
                //case TileType.File_Status:
                //    Modules.FileStatus s = new FileStatus(myIssue, myPub);
                //    Windows.PlainPopout pop = new Windows.PlainPopout(s);
                //    pop.MinHeight = 500;
                //    pop.MinWidth = 500;
                //    //pop.spContent.Children.Add(s);
                //    pop.Title = tm.DisplayName;
                //    pop.SizeToContent = SizeToContent.WidthAndHeight;
                //    pop.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                //    pop.Show();
                //    pop.Activate();
                //    break;
                case TileType.Record_Update:
                    WpfControls.RecordUpdate ru = new WpfControls.RecordUpdate(client, myPub.PubID);                    
                    FilterControls.FilterWrapper rufw = new FilterControls.FilterWrapper(ru);
                    rufw.MyViewModel.Initialize(myPub.PubID);
                    PopOutWindow(rufw, tm.DisplayName);
                    break;
                case TileType.Edit_File_Mapping:
                    FileMapperWizard.Modules.FMUniversal fmu = new FileMapperWizard.Modules.FMUniversal(true, true, client);
                    WpfControlPopOutWindow(fmu, tm.DisplayName);
                    break;
                case TileType.Import_File:
                    DataImportExport.Modules.CircImport i = new DataImportExport.Modules.CircImport(myPub);
                    PopOutWindow(i, tm.DisplayName);
                    break;
                case TileType.Add_Remove:
                    //Modules.AddRemove ar = new AddRemove(myIssue);
                    WpfControls.AddRemoveDataFetcher ardf = new WpfControls.AddRemoveDataFetcher(myPub.PubID);
                    FilterControls.FilterWrapper fw = new FilterControls.FilterWrapper(ardf);
                    fw.MyViewModel.Initialize(myPub.PubID);
                    PopOutWindow(fw, tm.DisplayName);
                    break;
                case TileType.Issue_Splits:
                    WpfControls.IssueSplitDataFetcher isp = new WpfControls.IssueSplitDataFetcher(myIssue.IssueId, myPub);
                    FilterControls.FilterWrapper fwi = new FilterControls.FilterWrapper(isp);
                    fwi.MyViewModel.Initialize(myPub.PubID);
                    PopOutWindow(fwi, tm.DisplayName);
                    UpdateIssueDetails(myIssue.PublicationId, true);
                    break;
                case TileType.Import_Comps:
                    DataImportExport.Modules.CircImport ci = new DataImportExport.Modules.CircImport(myPub);
                    PopOutWindow(ci, tm.DisplayName);
                    break;
            }
            //};
            //bw.RunWorkerCompleted += (o, ea) =>
            //{
            //    busy.IsBusy = false;
            //};
            //bw.RunWorkerAsync();
        }
    }
    public enum TileType
    {
        //File_Status,
        Record_Update,
        Edit_File_Mapping,
        Import_File,
        Add_Remove,
        Issue_Splits,
        Import_Comps
    }
}
