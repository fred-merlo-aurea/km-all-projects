using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Data;
using FrameworkUAS.Object;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using WPF = Core_AMS.Utilities.WPF;

namespace ControlCenter.Modules
{
    /// <summary>
    /// Interaction logic for PublisherManagement.xaml
    /// </summary>
    public partial class ClientManagement : UserControl
    {
        private const string CurrentClientNameCannotBeBlank = "Current client name cannot be blank.";
        private const string CurrentDisplayNameCannotBeBlank = "Current display name cannot be blank.";
        private const string CurrentClientCodeCannotBeBlank = "Current client code cannot be blank.";
        private const string CurrentClientTestDbConnectionStringCannotBeBlank = "Current client test db connection string cannot be blank.";
        private const string CurrentClientLiveDbConnectionStringCannotBeBlank = "Current client live db connection string cannot be blank.";
        private const string CurrentAccountManagerEmailsCannotBeBlank = "Current account manager emails cannot be blank.";
        private const string CurrentClientNameHasBeenUsedPleaseProvideUniqueClientName = "Current client name has been used. Please provide a unique client name.";
        private const string CurrentDisplayNameHasBeenUsedPleaseProvideUniqueDisplayName = "Current display name has been used. Please provide a unique display name.";
        private const string CurrentClientCodeHasBeenUsedPleaseProvideUniqueClientCode = "Current client code has been used. Please provide a unique client code.";

        private KMPlatform.Entity.Client myClient { get; set; }
        private KMPlatform.Entity.Client originalClient { get; set; }
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> userData { get; set; }
        private List<KMPlatform.Entity.Client> clientList { get; set; }
        private Dictionary<int, bool> ClientHasService { get; set; }
        private Guid AccessKey;

        public ClientManagement()
        {
            Window parentWindow = Application.Current.MainWindow;
            if (AppData.CheckParentWindowUid(parentWindow.Uid))
            {
                //only want this available to users that belong to KM
                if (AppData.IsKmUser() == true)
                {
                    InitializeComponent();
                    //LoadPopUp();
                    
                    AccessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
                    userData = FrameworkServices.ServiceClient.UAS_ClientClient();
                    clientList = userData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
                    grdClients.ItemsSource = clientList;
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageAccessDenied();
                }
            }
        }

        #region ButtonMethods
        public void PublicationDetails_Click(object sender, MouseButtonEventArgs e)
        {
            Window w = Core_AMS.Utilities.WPF.GetMainWindow();
            StackPanel sp = Core_AMS.Utilities.WPF.FindChild<StackPanel>(w, "spModule");
            KMPlatform.Entity.Client temp = (KMPlatform.Entity.Client)this.grdClients.SelectedItem;
            if (temp.ClientCode != null && temp.ClientCode != "")
            {
                UserControl pub = new PublicationManagement(temp.ClientCode);
                sp.Children.Clear();
                sp.Children.Add(pub);
            }
        }

        public void rbCancel(object sender, RoutedEventArgs e)
        {
            grdClients.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
            grdClients.SelectedItem = null;
        }

        private void rbSearch_Click(object sender, RoutedEventArgs e)
        {
            String index = tbSearch.Text.Trim();
            List<KMPlatform.Entity.Client> clientResults = new List<KMPlatform.Entity.Client>();
            KMPlatform.BusinessLogic.Client clientData = new KMPlatform.BusinessLogic.Client();
            clientList = userData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
            ClientHasService = new Dictionary<int, bool>();
            busy.IsBusy = true;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (o, ea) =>
            {
                if (string.IsNullOrEmpty(index))
                clientResults = clientList;
            else
            {
                clientResults = clientData.Search(index, clientList);
            }
            if (clientResults != null)
            {
                foreach (KMPlatform.Entity.Client c in clientResults)
                {
                    FrameworkUAS.Service.Response<bool> serviceCheckResponse = new FrameworkUAS.Service.Response<bool>();
                    FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> clientC = FrameworkServices.ServiceClient.UAS_ClientClient();
                    serviceCheckResponse = clientC.Proxy.HasService(AccessKey, c.ClientID, KMPlatform.Enums.Services.FULFILLMENT,FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClientGroup.ClientGroupID);                    
                    ClientHasService.Add(c.ClientID, serviceCheckResponse.Result);
                }
            }
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                if (lstActive.SelectedIndex == 1)
                    clientResults = clientResults.Where(x => x.IsActive == true).ToList();
                else if (lstActive.SelectedIndex == 2)
                    clientResults = clientResults.Where(x => x.IsActive == false).ToList();

                grdClients.ItemsSource = null;
                SortDescriptor sort = new SortDescriptor();
                sort.Member = "ClientName";
                sort.SortDirection = ListSortDirection.Ascending;
                grdClients.SortDescriptors.Add(sort);
                grdClients.ItemsSource = clientResults;
                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();                      
        }

        private void rbNewClient_Click(object sender, RoutedEventArgs e)
        {
            //var newWindow = new Windows.CreateNewClient();
            //newWindow.Show();            
            this.Background = System.Windows.Media.Brushes.DarkGray;            
            lstActive.IsEnabled = false;
            rbNewClient.IsEnabled = false;
            rbSearch.IsEnabled = false;
            tbSearch.IsEnabled = false;
            grdClients.IsEnabled = false;
            grdClients.Background = System.Windows.Media.Brushes.DarkGray;
            rwNew.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string fName = tbName.Text.Trim();
            string dName = tbDispName.Text.Trim();
            string code = tbCode.Text.Trim();
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
            bool hasPaid = false;
            if (cbPaid.IsChecked.HasValue)
                hasPaid = cbPaid.IsChecked.Value;
            bool isKMClient = false;
            if (cbKMClient.IsChecked.HasValue)
                isKMClient = cbKMClient.IsChecked.Value;
            DateTime todayDate = DateTime.Now;

            KMPlatform.Entity.Client newClient = new KMPlatform.Entity.Client();

            newClient.ClientName = fName;
            newClient.DisplayName = dName;
            newClient.ClientCode = code;
            newClient.ClientTestDBConnectionString = testString;
            newClient.ClientLiveDBConnectionString = liveString;
            newClient.IsActive = isActive;
            newClient.IgnoreUnknownFiles = ignoreFiles;
            newClient.AccountManagerEmails = managerEmails;
            newClient.ClientEmails = clientEmails;
            newClient.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            newClient.DateCreated = todayDate;   
            newClient.HasPaid = hasPaid;
            newClient.IsKMClient = isKMClient;            

            #region Check Data
            #region Null/Empty
            if (string.IsNullOrEmpty(newClient.ClientName))
            {
                Core_AMS.Utilities.WPF.MessageError(CurrentClientNameCannotBeBlank);                    
                grdClients.Rebind();
                return;
            }
            if (string.IsNullOrEmpty(newClient.DisplayName))
            {
                Core_AMS.Utilities.WPF.MessageError(CurrentDisplayNameCannotBeBlank);                    
                grdClients.Rebind();
                return;
            }
            if (string.IsNullOrEmpty(newClient.ClientCode))
            {
                Core_AMS.Utilities.WPF.MessageError(CurrentClientCodeCannotBeBlank);                    
                grdClients.Rebind();
                return;
            }
            if (string.IsNullOrEmpty(newClient.ClientTestDBConnectionString))
            {
                Core_AMS.Utilities.WPF.MessageError(CurrentClientTestDbConnectionStringCannotBeBlank);                    
                grdClients.Rebind();
                return;
            }
            if (string.IsNullOrEmpty(newClient.ClientLiveDBConnectionString))
            {
                Core_AMS.Utilities.WPF.MessageError(CurrentClientLiveDbConnectionStringCannotBeBlank);                    
                grdClients.Rebind();
                return;
            }
            if (string.IsNullOrEmpty(newClient.AccountManagerEmails))
            {
                Core_AMS.Utilities.WPF.MessageError(CurrentAccountManagerEmailsCannotBeBlank);                    
                grdClients.Rebind();
                return;
            }
            #endregion
            #region Duplicate
            clientList = userData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
            if (clientList != null && clientList.FirstOrDefault(x => x.ClientName.Equals(newClient.ClientName, StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                Core_AMS.Utilities.WPF.MessageError("Must provide a unique client name that hasn't been used.");
                return;
            }
            if (clientList != null && clientList.FirstOrDefault(x => x.DisplayName.Equals(newClient.DisplayName, StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                Core_AMS.Utilities.WPF.MessageError("Must provide a unique display name that hasn't been used.");
                return;
            }
            if (clientList != null && clientList.FirstOrDefault(x => x.ClientCode.Equals(newClient.ClientCode, StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                Core_AMS.Utilities.WPF.MessageError("Must provide a unique client code that hasn't been used.");
                return;
            }
            #endregion
            #endregion

            newClient.ClientID = userData.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, newClient).Result;
            if (newClient.ClientID > 0)
            {
                ResetNewWindow();

                List<KMPlatform.Entity.Client> clients = new List<KMPlatform.Entity.Client>();
                if (grdClients.ItemsSource != null)                
                    clients = (List<KMPlatform.Entity.Client>)grdClients.ItemsSource;                    
                
                clients.Add(newClient);
                grdClients.ItemsSource = null;
                grdClients.ItemsSource = clients;

                CloseWindow();
                this.grdClients.SelectedItem = null;
            }
            else
                Core_AMS.Utilities.WPF.MessageServiceError();

        }

        private void ResetNewWindow()
        {
            tbName.Clear();
            tbDispName.Clear();
            tbCode.Clear();
            tbTestString.Clear();
            tbLiveString.Clear();
            cbIsActive.IsChecked = false;
            cbIgnore.IsChecked = false;
            tbManagerEmails.Clear();
            tbClientEmails.Clear();
            cbPaid.IsChecked = false;
            cbKMClient.IsChecked = false;
        }

        private void CloseWindow()
        {
            this.Background = System.Windows.Media.Brushes.Transparent;
            lstActive.IsEnabled = true;
            rbNewClient.IsEnabled = true;
            rbSearch.IsEnabled = true;
            tbSearch.IsEnabled = true;
            grdClients.IsEnabled = true;
            grdClients.Background = System.Windows.Media.Brushes.Transparent;
            rwNew.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ResetNewWindow();
            //List<KMPlatform.Entity.Service> services = (List<KMPlatform.Entity.Service>)grdClients.ItemsSource;
            //grdClients.ItemsSource = services;
            CloseWindow();
            this.grdClients.SelectedItem = null;
        }

        private void rbClientDetails(object sender, RoutedEventArgs e)
        {
            if (this.Parent.GetType() == typeof(DockPanel))
            {
                if (this.grdClients.SelectedItem != null)
                {
                    KMPlatform.Entity.Client c = (KMPlatform.Entity.Client)this.grdClients.SelectedItem;
                    DockPanel sp = (DockPanel)this.Parent;
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

        #region GridWork
        private void ClientChangeCheck(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndedEventArgs e)
        {
            var radDataForm = sender as RadDataForm;
            myClient = radDataForm?.CurrentItem as KMPlatform.Entity.Client;
            if (myClient == null)
            {
                return;
            }

            var hasChanged = ClientHasChanged();

            if (!ClientPropertiesValid())
            {
                grdClients.Rebind();
                return;
            }

            if (!CheckNoDuplicates())
            {
                grdClients.Rebind();
                return;
            }

            if (hasChanged)
            {
                myClient.UpdatedByUserID = AppData.myAppData.AuthorizedUser.User.UserID;
                myClient.DateUpdated = DateTime.Now;
                myClient.ClientID = GetClientId();
            }
            else
            {
                grdClients.Rebind();
                return;
            }

            grdClients.SelectedItem = null;
            grdClients.RowDetailsVisibilityMode = GridViewRowDetailsVisibilityMode.Collapsed;
        }

        private bool CheckNoDuplicates()
        {
            if (clientList == null || clientList.All(x => x.ClientID != myClient.ClientID))
            {
                return true;
            }

            var clientWithSameId = clientList.First(x => x.ClientID == myClient.ClientID);

            if (clientWithSameId.ClientName != myClient.ClientName)
            {
                if (clientList.Any(
                    x => x.ClientName.Equals(myClient.ClientName, StringComparison.CurrentCultureIgnoreCase)
                         && x.ClientID != myClient.ClientID))
                {
                    WPF.MessageError(CurrentClientNameHasBeenUsedPleaseProvideUniqueClientName);
                    return false;
                }
            }

            if (clientWithSameId.DisplayName != myClient.DisplayName)
            {
                if (clientList.Any(
                    x => x.DisplayName.Equals(myClient.DisplayName, StringComparison.CurrentCultureIgnoreCase) 
                         && x.ClientID != myClient.ClientID))
                {
                    WPF.MessageError(CurrentDisplayNameHasBeenUsedPleaseProvideUniqueDisplayName);
                    return false;
                }
            }

            if (clientWithSameId.ClientCode != myClient.ClientCode)
            {
                if (clientList.Any(
                    x => x.ClientCode.Equals(myClient.ClientCode, StringComparison.CurrentCultureIgnoreCase)
                         && x.ClientID != myClient.ClientID))
                {
                    WPF.MessageError(CurrentClientCodeHasBeenUsedPleaseProvideUniqueClientCode);
                    return false;
                }
            }

            return true;
        }

        private bool ClientPropertiesValid()
        {
            if (string.IsNullOrWhiteSpace(myClient.ClientName))
            {
                WPF.MessageError(CurrentClientNameCannotBeBlank);
                return false;
            }

            if (string.IsNullOrWhiteSpace(myClient.DisplayName))
            {
                WPF.MessageError(CurrentDisplayNameCannotBeBlank);
                return false;
            }

            if (string.IsNullOrWhiteSpace(myClient.ClientCode))
            {
                WPF.MessageError(CurrentClientCodeCannotBeBlank);
                return false;
            }

            if (string.IsNullOrWhiteSpace(myClient.ClientTestDBConnectionString))
            {
                WPF.MessageError(CurrentClientTestDbConnectionStringCannotBeBlank);
                return false;
            }

            if (string.IsNullOrWhiteSpace(myClient.ClientLiveDBConnectionString))
            {
                WPF.MessageError(CurrentClientLiveDbConnectionStringCannotBeBlank);
                return false;
            }

            if (string.IsNullOrWhiteSpace(myClient.AccountManagerEmails))
            {
                WPF.MessageError(CurrentAccountManagerEmailsCannotBeBlank);
                return false;
            }

            return true;
        }

        private bool ClientHasChanged()
        {
            return !myClient.ClientName.Equals(originalClient.ClientName)
                   || !myClient.ClientCode.Equals(originalClient.ClientCode)
                   || !myClient.DisplayName.Equals(originalClient.DisplayName)
                   || !myClient.ClientTestDBConnectionString.Equals(originalClient.ClientTestDBConnectionString)
                   || !myClient.ClientLiveDBConnectionString.Equals(originalClient.ClientLiveDBConnectionString)
                   || !myClient.IsActive.Equals(originalClient.IsActive) 
                   || !myClient.IgnoreUnknownFiles.Equals(originalClient.IgnoreUnknownFiles)
                   || !myClient.AccountManagerEmails.Equals(originalClient.AccountManagerEmails)
                   || !myClient.ClientEmails.Equals(originalClient.ClientEmails)
                   || !myClient.HasPaid.Equals(originalClient.HasPaid) 
                   || !myClient.IsKMClient.Equals(originalClient.IsKMClient);
        }

        private int GetClientId()
        {
            return userData.Proxy.Save(AppData.myAppData.AuthorizedUser.AuthAccessKey, myClient).Result;
        }

        private void grdClients_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
            KMPlatform.Entity.Client myClient = (KMPlatform.Entity.Client)e.NewObject;
        }
        private void grdClients_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (grdClients.SelectedItem != null)
            {
                grdClients.ScrollIntoView(this.grdClients.SelectedItem);
                grdClients.UpdateLayout();
                Telerik.Windows.Controls.RadGridView rdGrid = (Telerik.Windows.Controls.RadGridView)sender;
                grdClients.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.VisibleWhenSelected;
                KMPlatform.Entity.Client c = (KMPlatform.Entity.Client)rdGrid.SelectedItem;
                originalClient = userData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, c.ClientID).Result;
            }
        }
        private void grdClients_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if (e.Row is GridViewRow && !(e.Row is GridViewNewRow))
            {
                KMPlatform.Entity.Client gridItem = e.DataElement as KMPlatform.Entity.Client;
                CheckBox chkHasFulfillment = Core_AMS.Utilities.WPF.FindChild<CheckBox>(e.Row, "chkHasFulfillment");
                Image pubEnable = Core_AMS.Utilities.WPF.FindChild<Image>(e.Row, "pubEnable");

                //check each client to see if they have the Fulfillment Service                                
                var serviceCheckResponse = false;
                if (ClientHasService != null && ClientHasService.Count > 0)
                {
                    if (ClientHasService.ContainsKey(gridItem.ClientID))
                    {
                        serviceCheckResponse = ClientHasService[gridItem.ClientID];
                    }
                }

                if(serviceCheckResponse == true || serviceCheckResponse == false)
                {
                    if (chkHasFulfillment != null)                    
                        chkHasFulfillment.IsEnabled = serviceCheckResponse;                    

                    if (pubEnable != null)
                        pubEnable.IsEnabled = serviceCheckResponse;
                    
                }
                else
                {
                    if (chkHasFulfillment != null)
                        chkHasFulfillment.IsEnabled = false;

                    if (pubEnable != null)
                        pubEnable.IsEnabled = false;

                }
            }
        }
        #endregion
    }
}
