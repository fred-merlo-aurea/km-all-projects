using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.Serialization;
using Telerik.Windows.Controls;
using FrameworkUAD.Entity;

namespace ControlCenter.Modules
{
    /// <summary>
    /// Interaction logic for PublicationManagement.xaml
    /// </summary>
    public partial class PublicationManagement : UserControl
    {
        #region VARIABLES
        //private int pubID = 0;
        //private string pubName;
        //private FrameworkUAD.Entity.Product myPub { get; set; }
        //private FrameworkUAD.Entity.Product originalPub { get; set; }
        //private List<FrameworkUAD.Entity.Product> publications { get; set; }
        //private KMPlatform.Entity.Client pub;
        //FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> publisherWorker = FrameworkServices.ServiceClient.UAS_ClientClient();
        //FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> publicationWorker = FrameworkServices.ServiceClient.UAD_ProductClient();
        //#endregion

        //#region STANDARD METHODS
        //public PublicationManagement(string code = "")
        //{
        //    InitializeComponent();
        //    LoadData(code);
        //}
        
        //private void LoadData(string code = "")
        //{            
        //    if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > 0)
        //    {
        //        if (string.IsNullOrEmpty(code))
        //            pub = publisherWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result.FirstOrDefault(x => x.ClientID == FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID);
        //        else
        //            pub = publisherWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result.FirstOrDefault(x => x.ClientID == FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID && x.ClientCode.Equals(code, StringComparison.CurrentCultureIgnoreCase));

        //        if (pub != null)
        //        {
        //            lbPubName.Content = pub.DisplayName;
        //            publications = publicationWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, pub).Result;
        //            if (publications.Count > 0)
        //            {
        //                grdPublications.ItemsSource = publications;
        //                pubID = pub.ClientID;
        //                pubName = pub.ClientName;
        //                dbPublishers.Visibility = System.Windows.Visibility.Hidden;
        //                dbPublishers.IsEnabled = false;
        //                lbPubName.Content = pub.ClientName;
        //                lbPubName.Visibility = System.Windows.Visibility.Visible;
        //            }
        //            else
        //            {
        //                Core_AMS.Utilities.WPF.Message("No Publications found under Publisher.", MessageBoxButton.OK, MessageBoxImage.Exclamation, "No Publication Data");
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            Core_AMS.Utilities.WPF.Message("Publisher currently not found.", MessageBoxButton.OK, MessageBoxImage.Exclamation, "No Publisher Data");
        //            rbnew.IsEnabled = false;
        //            grdPublications.IsEnabled = false;
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        Core_AMS.Utilities.WPF.Message("No client previously selected.", MessageBoxButton.OK, MessageBoxImage.Exclamation, "Publisher Not Selected");
        //        rbnew.IsEnabled = false;
        //        grdPublications.IsEnabled = false;
        //        return;
        //    }
        //}
        //#endregion        

        //#region BUTTON CLICKS
        //public void rbCancel(object sender, RoutedEventArgs e)
        //{
        //    grdPublications.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
        //    grdPublications.SelectedItem = null;
        //}

        //public void rbnewPublication_Click(object sender, RoutedEventArgs e)
        //{
        //    if (pubID != 0)
        //    {
        //        var newWindow = new Windows.CreateNewPublication(pubID, pubName);
        //        newWindow.Show();
        //    }
        //    else
        //    {
        //        MessageBoxResult confirm = MessageBox.Show("Please select a Publisher from the drop-down menu.", "No Publisher Selected", MessageBoxButton.OK);
        //        if (confirm == MessageBoxResult.OK)
        //        {
        //            return;
        //        }
        //    }
        //}
        //#endregion

        //#region COMBO BOX SELECTION
        //private void dbPublishers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    int id = 0;
        //    if (dbPublishers.SelectedValue.ToString() != null)
        //        int.TryParse(dbPublishers.SelectedValue.ToString(), out id);

        //    if (id > 0)
        //    {
        //        KMPlatform.Entity.Client client = new KMPlatform.Entity.Client();
        //        client = publisherWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result.SingleOrDefault(x => x.ClientID == id);
        //        pubName = client.ClientName;
        //        pubID = id;
        //        //LoadTiles(pubID);
        //        List<FrameworkUAD.Entity.Product> pubs = publicationWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, client).Result;
        //        if (pubs.Count > 0)
        //            grdPublications.ItemsSource = pubs;
        //        else
        //        {
        //            grdPublications.ItemsSource = null;
        //            Core_AMS.Utilities.WPF.Message("No Publications found under Publisher.", MessageBoxButton.OK, MessageBoxImage.Exclamation, "No Publication Data");
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        Core_AMS.Utilities.WPF.Message("Error loading publication data.", MessageBoxButton.OK, MessageBoxImage.Exclamation, "No Publication Data");
        //        return;
        //    }
        //}
        //#endregion

        //#region GRID
        //public void PublicationChangeCheck(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndedEventArgs e)
        //{            
        //    Telerik.Windows.Controls.RadDataForm rdForm = (Telerik.Windows.Controls.RadDataForm)sender;
        //    myPub = (FrameworkUAD.Entity.Product)rdForm.CurrentItem;

        //    bool hasChanged = false;

        //    if (!myPub.PubName.Equals(originalPub.PubName))
        //        hasChanged = true;
        //    if (!myPub.PubCode.Equals(originalPub.PubCode))
        //        hasChanged = true;
        //    if (!myPub.IsImported.Equals(originalPub.IsImported))
        //        hasChanged = true;
        //    if (!myPub.IsActive.Equals(originalPub.IsActive))
        //        hasChanged = true;
        //    if (!myPub.AllowDataEntry.Equals(originalPub.AllowDataEntry))
        //        hasChanged = true;

        //    if (hasChanged == true)
        //        myPub.PubID = publicationWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myPub,pub).Result;

        //    grdPublications.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
        //}        

        //private void grdPublications_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        //{            
        //    //grdPublications.ScrollIntoView(this.grdPublications.SelectedItem);
        //    //originalPub = (FrameworkUAD.Entity.Product)this.grdPublications.SelectedItem;
        //    //originalPub = publicationWorker.Proxy.SelectPublication(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, originalPub.PublicationID).Result;
        //    //grdPublications.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.VisibleWhenSelected;
        //    //grdPublications.UpdateLayout();
        //}
        //private void grdPublications_RowDetailsVisibilityChanged(object sender, Telerik.Windows.Controls.GridView.GridViewRowDetailsEventArgs e)
        //{
        //    //grdPublications.ScrollIntoView(this.grdPublications.SelectedItem);
        //    //originalPub = (FrameworkUAD.Entity.Product)this.grdPublications.SelectedItem;
        //    //originalPub = publicationWorker.Proxy.SelectPublication(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, originalPub.PublicationID).Result;
        //    //grdPublications.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.VisibleWhenSelected;
        //}
        #endregion                           
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> clientWorker { get; set; }        
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> publisherWorker { get; set; }
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IUser> userWorker { get; set; }
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> publicationWorker { get; set; }        
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProductTypes> pubTypeWorker { get; set; }
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IFrequency> freqWorker { get; set; }
        private List<KMPlatform.Entity.User> kmUsers { get; set; } 
        private List<KMPlatform.Entity.Client> clientList { get; set; }        
        private List<FrameworkUAD.Entity.Product> publicationList { get; set; }
        private List<FrameworkUAD.Entity.ProductTypes> pubTypeList { get; set; }
        private List<FrameworkUAD.Entity.Frequency> freqList { get; set; }
        private KMPlatform.Entity.Client myClient { get; set; }
        private KMPlatform.Entity.Client mySelectedClient { get; set; }
        private List<ProductContainer> prodContainer = new List<ProductContainer>();

        public PublicationManagement(string code = "")
        {
            Window parentWindow = Application.Current.MainWindow;
            if (FrameworkUAS.Object.AppData.CheckParentWindowUid(parentWindow.Uid))
            {
                //only want this available to users that belong to KM
                if (FrameworkUAS.Object.AppData.IsKmUser() == true)
                {
                    InitializeComponent();                    
                    clientWorker = FrameworkServices.ServiceClient.UAS_ClientClient();
                    publisherWorker = FrameworkServices.ServiceClient.UAS_ClientClient();
                    userWorker = FrameworkServices.ServiceClient.UAS_UserClient();
                    publicationWorker = FrameworkServices.ServiceClient.UAD_ProductClient();
                    pubTypeWorker = FrameworkServices.ServiceClient.UAD_ProductTypesClient();
                    freqWorker = FrameworkServices.ServiceClient.UAD_FrequencyClient();
                    mySelectedClient = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient;
                    LoadData();
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageAccessDenied();
                }
            }
        }

        private void LoadData(string code = "")
        {            
            if (FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID >= 0)
            {
                if (string.IsNullOrEmpty(code))
                    myClient = publisherWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result.FirstOrDefault(x => x.ClientID == FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID);
                else
                    myClient = publisherWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result.FirstOrDefault(x => x.ClientID == FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID && x.ClientCode.Equals(code, StringComparison.CurrentCultureIgnoreCase));

                pubTypeList = pubTypeWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, mySelectedClient.ClientConnections).Result;
                freqList = freqWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
                kmUsers = userWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;

                rcbFrequencyID.ItemsSource = freqList;
                rcbPubTypeID.ItemsSource = pubTypeList;

                if (myClient != null)
                {
                    lbPubName.Content = myClient.DisplayName;
                    publicationList = publicationWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myClient.ClientConnections).Result;

                    foreach (FrameworkUAD.Entity.Product prod in publicationList)
                    {
                        ProductContainer p = new ProductContainer(prod);

                        KMPlatform.Entity.User user = kmUsers.FirstOrDefault(x => x.UserID == prod.CreatedByUserID);
                        p.CreatedByName = user.FullName;

                        user = kmUsers.FirstOrDefault(x => x.UserID == prod.UpdatedByUserID);
                        if (user != null)
                            p.UpdatedByName = user.FullName;

                        FrameworkUAD.Entity.ProductTypes pt = pubTypeList.FirstOrDefault(x => x.PubTypeID == prod.PubTypeID);
                        if (pt != null && !(string.IsNullOrEmpty(pt.PubTypeDisplayName)))
                            p.PubType = pubTypeList.FirstOrDefault(x => x.PubTypeID == prod.PubTypeID).PubTypeDisplayName;

                        FrameworkUAD.Entity.Frequency f = freqList.FirstOrDefault(x => x.FrequencyID == prod.FrequencyID);
                        if (f != null && !(string.IsNullOrEmpty(f.FrequencyName)))
                            p.Frequency = freqList.FirstOrDefault(x => x.FrequencyID == prod.FrequencyID).FrequencyName;

                        prodContainer.Add(p);
                    }

                    if (prodContainer.Count > 0)
                    {
                        grdPublications.ItemsSource = prodContainer;
                        grdPublications.Rebind();
                        lbPubName.Content = myClient.ClientName;
                        lbPubName.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        Core_AMS.Utilities.WPF.Message("No Publications found under Publisher.", MessageBoxButton.OK, MessageBoxImage.Exclamation, "No Publication Data");
                        return;
                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.Message("Publisher currently not found.", MessageBoxButton.OK, MessageBoxImage.Exclamation, "No Publisher Data");
                    rbNewPublication.IsEnabled = false;
                    grdPublications.IsEnabled = false;
                    return;
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.Message("No client previously selected.", MessageBoxButton.OK, MessageBoxImage.Exclamation, "Publisher Not Selected");
                rbNewPublication.IsEnabled = false;
                grdPublications.IsEnabled = false;
                return;
            }
        }

        private void lstEnabled_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grdPublications != null && grdPublications.Columns.Count > 0)
            {
                Telerik.Windows.Controls.GridViewColumn isEnabledColumn = grdPublications.Columns["IsActive"];

                string isEnabled = lstActive.SelectedItem.ToString().Replace("System.Windows.Controls.ListBoxItem: ", "");
                //All, Enabled, Not Enabled

                if (!isEnabled.Equals("All"))
                {
                    if (isEnabled.Equals("Active")) isEnabled = "true";
                    else isEnabled = "false";

                    Telerik.Windows.Controls.GridView.IColumnFilterDescriptor isEnabledFilter = isEnabledColumn.ColumnFilterDescriptor;
                    // Suspend the notifications to avoid multiple data engine updates
                    isEnabledFilter.SuspendNotifications();

                    // This is the same as the end user configuring the upper field filter.
                    isEnabledFilter.FieldFilter.Filter1.Operator = Telerik.Windows.Data.FilterOperator.IsEqualTo;
                    isEnabledFilter.FieldFilter.Filter1.Value = isEnabled;
                    isEnabledFilter.FieldFilter.Filter1.IsCaseSensitive = false;

                    // Resume the notifications to force the data engine to update the filter.
                    isEnabledFilter.ResumeNotifications();
                }
                else
                {
                    grdPublications.FilterDescriptors.SuspendNotifications();
                    isEnabledColumn.ClearFilters();
                    grdPublications.FilterDescriptors.ResumeNotifications();
                }
            }

        }

        #region ButtonMethods        
        public void rbEditCancel_Click(object sender, RoutedEventArgs e)
        {
            grdPublications.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
            grdPublications.SelectedItem = null;
        }        

        private void rbNewPublication_Click(object sender, RoutedEventArgs e)
        {         
            this.Background = System.Windows.Media.Brushes.DarkGray;            
            lstActive.IsEnabled = false;
            rbNewPublication.IsEnabled = false;
            grdPublications.IsEnabled = false;
            grdPublications.Background = System.Windows.Media.Brushes.DarkGray;
            rwNew.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnNewSave_Click(object sender, RoutedEventArgs e)
        {
            #region Set Values
            string pubName = tbPubName.Text.Trim();
            bool istradeshow = false;
            if (cbistradeshow.IsChecked.HasValue)
                istradeshow = cbistradeshow.IsChecked.Value;
            string pubCode = tbPubCode.Text.Trim();
            int pubTypeID = 0;
            int.TryParse(rcbPubTypeID.SelectedValue.ToString(), out pubTypeID);            
            bool EnableSearching = false;
            if (cbEnableSearching.IsChecked.HasValue)
                EnableSearching = cbEnableSearching.IsChecked.Value;
            int score = (int)nudScore.Value;            
            int SortOrder = (int)nudSortOrder.Value;
            string YearStartDate = dtpYearStartDate.CurrentDateTimeText;
            string YearEndDate = dtpYearEndDate.CurrentDateTimeText;
            string issueDateValue = dtpIssueDate.SelectedDate.ToString();
            DateTime IssueDate;
            DateTime.TryParse(issueDateValue, out IssueDate);
            bool IsImported = false;
            if (cbIsImported.IsChecked.HasValue)
                IsImported = cbIsImported.IsChecked.Value;
            bool IsActive = false;
            if (cbIsActive.IsChecked.HasValue)
                IsActive = cbIsActive.IsChecked.Value;
            bool AllowDataEntry = false;
            if (cbAllowDataEntry.IsChecked.HasValue)
                AllowDataEntry = cbAllowDataEntry.IsChecked.Value;
            int FrequencyID = 0;
            if (rcbFrequencyID.SelectedValue != null)
                int.TryParse(rcbFrequencyID.SelectedValue.ToString(), out FrequencyID);
            bool KMImportAllowed = false;
            if (cbKMImportAllowed.IsChecked.HasValue)
                KMImportAllowed = cbKMImportAllowed.IsChecked.Value;            
            bool ClientImportAllowed = false;
            if (cbClientImportAllowed.IsChecked.HasValue)
                ClientImportAllowed = cbClientImportAllowed.IsChecked.Value;            
            bool AddRemoveAllowed = false;
            if (cbAddRemoveAllowed.IsChecked.HasValue)
                AddRemoveAllowed = cbAddRemoveAllowed.IsChecked.Value;
            bool IsUAD = false;
            if (cbIsUAD.IsChecked.HasValue)
                IsUAD = cbIsUAD.IsChecked.Value;
            bool IsCirc = false;
            if (cbIsCirc.IsChecked.HasValue)
                IsCirc = cbIsCirc.IsChecked.Value;
            #endregion
                        
            DateTime todayDate = DateTime.Now;
            FrameworkUAD.Entity.Product newPub = new FrameworkUAD.Entity.Product();
                        
            newPub.PubName = pubName;
            newPub.istradeshow = istradeshow;
            newPub.PubCode = pubCode;
            newPub.PubTypeID = pubTypeID;
            newPub.GroupID = 0;
            newPub.EnableSearching = EnableSearching;
            newPub.score = score;
            newPub.SortOrder = SortOrder;
            newPub.DateCreated = todayDate;
            newPub.DateUpdated = todayDate;
            newPub.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            newPub.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            newPub.ClientID = mySelectedClient.ClientID;
            newPub.YearStartDate = YearStartDate;
            newPub.YearEndDate = YearEndDate;
            newPub.IssueDate = IssueDate;
            newPub.IsImported = IsImported;
            newPub.IsActive = IsActive;
            newPub.AllowDataEntry = AllowDataEntry;
            newPub.FrequencyID = FrequencyID;
            newPub.KMImportAllowed = KMImportAllowed;
            newPub.ClientImportAllowed = ClientImportAllowed;
            newPub.AddRemoveAllowed = AddRemoveAllowed;
            newPub.AcsMailerInfoId = 0;
            newPub.IsUAD = IsUAD;
            newPub.IsCirc = IsCirc;
                        
            #region Check Data
            #region Validate Circ Product
            if (IsCirc == true)
            {
                if (string.IsNullOrEmpty(YearStartDate))
                {
                    Core_AMS.Utilities.WPF.Message("Please supply a Year Start Date.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Submission");
                    return;
                }

                if (string.IsNullOrEmpty(YearEndDate))
                {
                    Core_AMS.Utilities.WPF.Message("Please supply a Year End Date.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Submission");
                    return;
                }

                if (!(FrequencyID > 0))
                {
                    Core_AMS.Utilities.WPF.Message("Please supply a Frequency.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Invalid Submission");
                    return;
                }
            }
            #endregion
            #region Null/Empty
            if (string.IsNullOrEmpty(newPub.PubName))
            {
                Core_AMS.Utilities.WPF.MessageError("Current product name cannot be blank.");                    
                grdPublications.Rebind();
                return;
            }
            if (string.IsNullOrEmpty(newPub.PubCode))
            {
                Core_AMS.Utilities.WPF.MessageError("Current product code cannot be blank.");                    
                grdPublications.Rebind();
                return;
            }
            if (!(pubTypeID > 0))
            {
                Core_AMS.Utilities.WPF.MessageError("Must select a product type.");                    
                grdPublications.Rebind();
                return;
            }
            if (!(FrequencyID > 0))
            {
                Core_AMS.Utilities.WPF.MessageError("Must select a frequency type.");                    
                grdPublications.Rebind();
                return;
            }
            #endregion
            #region Duplicate
            publicationList = publicationWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myClient.ClientConnections).Result;
            if (publicationList != null && publicationList.FirstOrDefault(x => x.PubName.Equals(newPub.PubName, StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                Core_AMS.Utilities.WPF.MessageError("Must provide a unique product name that hasn't been used.");
                return;
            }
            if (publicationList != null && publicationList.FirstOrDefault(x => x.PubCode.Equals(newPub.PubCode, StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                Core_AMS.Utilities.WPF.MessageError("Must provide a unique product code that hasn't been used.");
                return;
            }
            #endregion
            #endregion

            newPub.PubID = publicationWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, newPub, myClient.ClientConnections).Result;
            if (newPub.PubID > 0)
            {
                ResetNewWindow();
                ProductContainer p = new ProductContainer(newPub);
                prodContainer.Add(p);
                grdPublications.Rebind();
                //broke because of a conversion error, created new product container and rebound instead - Micah
                //List<FrameworkUAD.Entity.Product> clients = new List<FrameworkUAD.Entity.Product>();
                //if (grdPublications.ItemsSource != null)
                //    clients = (List<Product>)grdPublications.ItemsSource;                    
                
                //clients.Add(newPub);
                //grdPublications.ItemsSource = null;
                //grdPublications.ItemsSource = clients;
                
                CloseWindow();
                this.grdPublications.SelectedItem = null;
            }
            else
                Core_AMS.Utilities.WPF.MessageServiceError();

        }

        private void btnNewCancel_Click(object sender, RoutedEventArgs e)
        {
            ResetNewWindow();
            grdPublications.Rebind();
            //Caused a conversion error, changed to rebind - Micah
            //List<FrameworkUAD.Entity.Product> clientGroups = (List<FrameworkUAD.Entity.Product>)grdPublications.ItemsSource;
            //grdPublications.ItemsSource = clientGroups;
            CloseWindow();
            this.grdPublications.SelectedItem = null;
        }

        private void ResetNewWindow()
        {            
            tbPubName.Clear();
            cbistradeshow.IsChecked = false;
            tbPubCode.Clear();
            rcbPubTypeID.SelectedIndex = -1;
            cbEnableSearching.IsChecked = false;
            nudScore.Value = 0;
            nudSortOrder.Value = 0;
            dtpYearStartDate.CurrentDateTimeText = "";
            dtpYearEndDate.CurrentDateTimeText = "";
            dtpIssueDate.CurrentDateTimeText = "";
            cbIsImported.IsChecked = false;
            cbIsActive.IsChecked = false;
            cbAllowDataEntry.IsChecked = false;
            rcbFrequencyID.SelectedIndex = -1;
            cbKMImportAllowed.IsChecked = false;
            cbClientImportAllowed.IsChecked = false;
            cbAddRemoveAllowed.IsChecked = false;
            cbIsUAD.IsChecked = false;
            cbIsCirc.IsChecked = false;
        }

        private void CloseWindow()
        {
            this.Background = System.Windows.Media.Brushes.Transparent;
            lstActive.IsEnabled = true;
            rbNewPublication.IsEnabled = true;
            grdPublications.IsEnabled = true;
            grdPublications.Background = System.Windows.Media.Brushes.Transparent;
            rwNew.Visibility = System.Windows.Visibility.Collapsed;
        }        

        private void rbClientDetails(object sender, RoutedEventArgs e)
        {
            if (this.Parent.GetType() == typeof(StackPanel))
            {
                if (this.grdPublications.SelectedItem != null)
                {
                    KMPlatform.Entity.Client c = (KMPlatform.Entity.Client)this.grdPublications.SelectedItem;
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

        #region GridWork
        private void rdForm_Loaded(object sender, RoutedEventArgs e)
        {
            RadDataForm rdForm = sender as RadDataForm;

            DataFormComboBoxField cbEditPubTypeID = Core_AMS.Utilities.WPF.FindChild<DataFormComboBoxField>(rdForm, "cbEditPubTypeID");
            if (cbEditPubTypeID != null)
                cbEditPubTypeID.ItemsSource = pubTypeList;

            DataFormComboBoxField cbEditFrequencyID = Core_AMS.Utilities.WPF.FindChild<DataFormComboBoxField>(rdForm, "cbEditFrequencyID");
            if (cbEditFrequencyID != null)
                cbEditFrequencyID.ItemsSource = freqList;
                       
        }
        private void rdForm_EditEnded(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndedEventArgs e)
        {
            Telerik.Windows.Controls.RadDataForm rdForm = (Telerik.Windows.Controls.RadDataForm)sender;
            ProductContainer myprodContainer = (ProductContainer)rdForm.CurrentItem;

            FrameworkUAD.Entity.Product myPub = new FrameworkUAD.Entity.Product();
            #region Product Variables
            myPub.PubID = myprodContainer.PubID;
            myPub.PubName = myprodContainer.PubName;
            myPub.istradeshow = myprodContainer.istradeshow;
            myPub.PubCode = myprodContainer.PubCode;
            myPub.PubTypeID = myprodContainer.PubTypeID;
            myPub.GroupID = myprodContainer.GroupID;
            myPub.EnableSearching = myprodContainer.EnableSearching;
            myPub.score = myprodContainer.score;
            myPub.SortOrder = myprodContainer.SortOrder;
            myPub.CodeSheets = myprodContainer.CodeSheets;
            myPub.ResponseGroups = myprodContainer.ResponseGroups;
            myPub.DateCreated = myprodContainer.DateCreated;
            myPub.CreatedByUserID = myprodContainer.CreatedByUserID;
            myPub.ClientID = myprodContainer.ClientID;
            myPub.AllowDataEntry = myprodContainer.AllowDataEntry;
            myPub.IsActive = myprodContainer.IsActive;
            myPub.IsCirc = myprodContainer.IsCirc;
            myPub.IsUAD = myprodContainer.IsUAD;
            myPub.YearStartDate = myprodContainer.YearStartDate;
            myPub.YearEndDate = myprodContainer.YearEndDate;
            myPub.IssueDate = myprodContainer.IssueDate;
            myPub.IsImported = myprodContainer.IsImported;
            myPub.FrequencyID = myprodContainer.FrequencyID;
            myPub.AcsMailerInfoId = myprodContainer.AcsMailerInfoId;
            myPub.IsOpenCloseLocked = myprodContainer.IsOpenCloseLocked;
            myPub.KMImportAllowed = myprodContainer.KMImportAllowed;
            myPub.ClientImportAllowed = myprodContainer.ClientImportAllowed;
            myPub.AddRemoveAllowed = myprodContainer.AddRemoveAllowed;
            #endregion

            if (myPub.IsOpenCloseLocked != null && myPub.IsOpenCloseLocked == true)
            {
                Core_AMS.Utilities.WPF.Message("The current product is locked and cannot be edited.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Product Locked");
                grdPublications.Rebind();
                return;
            }

            #region Check Data
            #region Null/Empty
            //if (string.IsNullOrEmpty(myClientGroup.Color))
            //{
            //    Core_AMS.Utilities.WPF.MessageError("Must select a color.");                    
            //    grdPublications.Rebind();
            //    return;
            //}
            #endregion
            #region Duplicate
            //if (clientGroupList != null && clientGroupList.FirstOrDefault(x => x.ClientGroupID == myClientGroup.ClientGroupID).ClientGroupName != myClientGroup.ClientGroupName)
            //{
            //    if (clientGroupList.FirstOrDefault(x => x.ClientGroupName.Equals(myClientGroup.ClientGroupName, StringComparison.CurrentCultureIgnoreCase) && x.ClientGroupID != myClientGroup.ClientGroupID) != null)
            //    {
            //        Core_AMS.Utilities.WPF.MessageError("Must provide a unique client group name that hasn't been used.");
            //        return;
            //    }
            //}
            #endregion
            #endregion

            myPub.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            myPub.DateUpdated = DateTime.Now;
            myPub.PubID = publicationWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myPub, myClient.ClientConnections).Result;
            
            grdPublications.SelectedItem = null;
            grdPublications.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
        }
        private void grdPublications_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
            FrameworkUAD.Entity.Product myClientGroup = (FrameworkUAD.Entity.Product)e.NewObject;
        }
        private void grdPublications_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (grdPublications.SelectedItem != null)
            {
                grdPublications.ScrollIntoView(this.grdPublications.SelectedItem);
                grdPublications.UpdateLayout();
                grdPublications.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.VisibleWhenSelected;                                
            }
        }
        //private void grdPublications_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        //{
        //    FrameworkUAD.Entity.Product prod = (FrameworkUAD.Entity.Product)e.DataElement;
        //    if (prod != null)
        //    {
        //        if (kmUsers != null)
        //        {
        //            KMPlatform.Entity.User user = kmUsers.FirstOrDefault(x => x.UserID == prod.CreatedByUserID);
        //            if (user != null)
        //            {
        //                TextBlock tbCreatedBy = Core_AMS.Utilities.WPF.FindChild<TextBlock>(e.Row, "tbCreatedByName");
        //                TextBlock tbUpdatedBy = Core_AMS.Utilities.WPF.FindChild<TextBlock>(e.Row, "tbUpdatedByName");

        //                if (tbCreatedBy != null)
        //                {
        //                    if (user != null)
        //                    {
        //                        if (prod.UpdatedByUserID.HasValue == true && prod.CreatedByUserID == prod.UpdatedByUserID)
        //                        {
        //                            tbCreatedBy.Text = user.FullName;
        //                            if (tbUpdatedBy != null) tbUpdatedBy.Text = user.FullName;
        //                        }
        //                        else if (prod.UpdatedByUserID.HasValue == false && prod.CreatedByUserID != prod.UpdatedByUserID)
        //                        {
        //                            tbCreatedBy.Text = user.FullName;
        //                            if (tbUpdatedBy != null) tbUpdatedBy.Text = string.Empty;
        //                        }
        //                        else if (prod.UpdatedByUserID.HasValue == true || prod.CreatedByUserID != prod.UpdatedByUserID)
        //                        {
        //                            tbCreatedBy.Text = user.FullName;
        //                            KMPlatform.Entity.User updatedUser = kmUsers.FirstOrDefault(x => x.UserID == prod.UpdatedByUserID);
        //                            if (tbUpdatedBy != null) tbUpdatedBy.Text = updatedUser.FullName;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        if (pubTypeList != null && freqList != null)
        //        {
        //            TextBlock tbPubType = Core_AMS.Utilities.WPF.FindChild<TextBlock>(e.Row, "tbPubType");
        //            TextBlock tbFrequency = Core_AMS.Utilities.WPF.FindChild<TextBlock>(e.Row, "tbFrequency");
        //            if (prod.PubTypeID != null)
        //            {
        //                if (pubTypeList.FirstOrDefault(x => x.PubTypeID == prod.PubTypeID) != null)
        //                {
        //                    if (tbPubType != null && string.IsNullOrEmpty(tbPubType.Text))
        //                    {
        //                        FrameworkUAD.Entity.ProductTypes pt = pubTypeList.FirstOrDefault(x => x.PubTypeID == prod.PubTypeID);
        //                        if (pt != null && !(string.IsNullOrEmpty(pt.PubTypeDisplayName)))
        //                            tbPubType.Text = pubTypeList.FirstOrDefault(x => x.PubTypeID == prod.PubTypeID).PubTypeDisplayName;
        //                    }

        //                }

        //            }
        //            if (prod.FrequencyID != null)
        //            {
        //                if (freqList.FirstOrDefault(x => x.FrequencyID == prod.FrequencyID) != null)
        //                {
        //                    if (tbFrequency != null && string.IsNullOrEmpty(tbFrequency.Text))
        //                    {
        //                        FrameworkUAD.Entity.Frequency f = freqList.FirstOrDefault(x => x.FrequencyID == prod.FrequencyID);
        //                        if (tbFrequency != null && f != null && !(string.IsNullOrEmpty(f.FrequencyName)))
        //                            tbFrequency.Text = freqList.FirstOrDefault(x => x.FrequencyID == prod.FrequencyID).FrequencyName;
        //                    }
        //                }

        //            }
        //        }
        //    }
        //}
        #endregion                
    }

    [Serializable]
    [DataContract]
    public class ProductContainer : FrameworkUAD.Entity.Product
    {
        public ProductContainer(FrameworkUAD.Entity.Product prod)
        {
            PubID = prod.PubID;
            PubName = prod.PubName;
            istradeshow = prod.istradeshow;
            PubCode = prod.PubCode;
            PubTypeID = prod.PubTypeID;
            PubType = string.Empty;
            GroupID = prod.GroupID;
            EnableSearching = prod.EnableSearching;
            score = prod.score;
            SortOrder = prod.SortOrder;
            CodeSheets = prod.CodeSheets;
            ResponseGroups = prod.ResponseGroups;
            DateCreated = prod.DateCreated;
            CreatedByUserID = prod.CreatedByUserID;
            UpdatedByUserID = prod.UpdatedByUserID;
            CreatedByName = string.Empty;
            UpdatedByName = string.Empty;
            ClientID = prod.ClientID;
            AllowDataEntry = prod.AllowDataEntry;
            IsActive = prod.IsActive;
            IsCirc = prod.IsCirc;
            IsUAD = prod.IsUAD;
            Frequency = string.Empty;
            YearStartDate = prod.YearStartDate;
            YearEndDate = prod.YearEndDate;
            IssueDate = prod.IssueDate;
            IsImported = prod.IsImported;
            FrequencyID = prod.FrequencyID;
            AcsMailerInfoId = prod.AcsMailerInfoId;
            IsOpenCloseLocked = prod.IsOpenCloseLocked;
            KMImportAllowed = prod.KMImportAllowed;
            ClientImportAllowed = prod.ClientImportAllowed;
            AddRemoveAllowed = prod.AddRemoveAllowed;
        }
        #region Properties
        [DataMember(Name = "ProductID")]
        public int PubID { get; set; }
        [DataMember(Name = "ProductName")]
        public string PubName { get; set; }
        [DataMember(Name = "IsTradeShow")]
        public bool istradeshow { get; set; }
        [DataMember(Name = "ProductCode")]
        public string PubCode { get; set; }
        [DataMember(Name = "ProductTypeID")]
        public int PubTypeID { get; set; }
        [DataMember(Name = "GroupID")]
        public int GroupID { get; set; }
        [DataMember(Name = "EnableSearching")]
        public bool EnableSearching { get; set; }
        [DataMember(Name = "Score")]
        public int score { get; set; }
        [DataMember]
        public int SortOrder { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        [DataMember]
        public int ClientID { get; set; }
	    [DataMember]
        public string YearStartDate { get; set; }
	    [DataMember]
        public string YearEndDate { get; set; }
	    [DataMember]
        public DateTime? IssueDate { get; set; }
	    [DataMember]
        public bool? IsImported { get; set; }
	    [DataMember]
        public bool IsActive { get; set; }
	    [DataMember]
        public bool AllowDataEntry { get; set; }
	    [DataMember]
        public int? FrequencyID { get; set; }
	    [DataMember]
        public bool? KMImportAllowed { get; set; }
	    [DataMember]
        public bool? ClientImportAllowed { get; set; }
	    [DataMember]
        public bool? AddRemoveAllowed { get; set; }
	    [DataMember]
        public int AcsMailerInfoId { get; set; }
        [DataMember]
        public bool? IsUAD { get; set; }
        [DataMember]
        public bool? IsCirc { get; set; }
        [DataMember]
        public bool? IsOpenCloseLocked { get; set; }
        [DataMember]
        public string CreatedByName { get; set; }
        [DataMember]
        public string PubType { get; set; }
        [DataMember]
        public string UpdatedByName { get; set; }
        [DataMember]
        public string Frequency { get; set; }
        #endregion

        [DataMember]
        public List<CodeSheet> CodeSheets { get; set; }
        [DataMember]
        public List<ResponseGroup> ResponseGroups { get; set; }
    }
}
