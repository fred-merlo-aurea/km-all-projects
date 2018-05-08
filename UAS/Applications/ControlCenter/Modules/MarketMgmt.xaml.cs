using FrameworkUAS.Object;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Data;

namespace ControlCenter.Modules
{
    /// <summary>
    /// Interaction logic for MarketMgmt.xaml
    /// </summary>
    public partial class MarketMgmt : UserControl
    {
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IMarket> marketWorker { get; set; }
        private List<KMPlatform.Entity.User> kmUsers { get; set; }        
        public List<FrameworkUAD.Entity.Market> markets { get; set; }
        public List<MarketContainer> marketContainer = new List<MarketContainer>();
        public List<FrameworkUAD.Entity.Brand> brands { get; set; }
        public MarketMgmt()
        {
            if (FrameworkUAS.Object.AppData.IsKmUser() == true)
            {
            if (!(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID > -1))
            {             
                Core_AMS.Utilities.WPF.Message("No client was selected. Please select a client.", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, "Missing Client Data");
                return;
            }
            Window parentWindow = Application.Current.MainWindow;
            if (AppData.CheckParentWindowUid(parentWindow.Uid))
            {
                //only want this available to users that belong to KM
                if (AppData.IsKmUser() == true)
                {
                    InitializeComponent();
                    LoadData();
                    this.DataContext = this;
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageAccessDenied();
                }
                }
            }
            else
            {
                Core_AMS.Utilities.WPF.MessageAccessDenied();
            }
        }
        private void LoadData()
        {
            //startUp = false;
            SortDescriptor sort = new SortDescriptor();
            sort.Member = "MarketName";
            sort.SortDirection = ListSortDirection.Ascending;
            grdMarket.SortDescriptors.Add(sort);
            marketWorker = FrameworkServices.ServiceClient.UAD_MarketClient();
            markets = marketWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;

            KMPlatform.Entity.ClientGroup cgKM = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.ClientGroups.SingleOrDefault(x => x.ClientGroupName.Equals("Knowledge Marketing"));
            FrameworkServices.ServiceClient<UAS_WS.Interface.IUser> userWorker = FrameworkServices.ServiceClient.UAS_UserClient();
            kmUsers = userWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, cgKM.ClientGroupID, false).Result;

            #region Brand
            FrameworkServices.ServiceClient<UAD_WS.Interface.IBrand> brandWorker = FrameworkServices.ServiceClient.UAD_BrandClient();
            brands = brandWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
            if (brands != null)
            {
                cbBrand.ItemsSource = brands;
                cbBrand.SelectedValuePath = "BrandID";
                cbBrand.DisplayMemberPath = "BrandName";
            }
            #endregion

            foreach (FrameworkUAD.Entity.Market market in markets)
            {
                MarketContainer mc = new MarketContainer(market);
                KMPlatform.Entity.User user = kmUsers.FirstOrDefault(x => x.UserID == market.CreatedByUserID);

                if (user != null)
                {
                    mc.CreatedByName = user.FullName;

                    user = kmUsers.FirstOrDefault(x => x.UserID == market.UpdatedByUserID);
                    if (user != null)
                        mc.UpdatedByName = user.FullName;
                }

                marketContainer.Add(mc);
            }
            grdMarket.ItemsSource = marketContainer;
          
        }        
        private void rbCancel_Click(object sender, RoutedEventArgs e)
        {
            grdMarket.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
            this.grdMarket.SelectedItem = null;
        }
        private void grdMarket_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (this.grdMarket.SelectedItem != null)
            {
                grdMarket.ScrollIntoView(this.grdMarket.SelectedItem);
                grdMarket.UpdateLayout();
                grdMarket.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.VisibleWhenSelected;
            }
        }
        private void rdForm_EditEnded(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndedEventArgs e)
        {
            if (e.EditAction == Telerik.Windows.Controls.Data.DataForm.EditAction.Cancel)
                grdMarket.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
            else
            {
                //EditAction == Commit
                //save changes
                MarketContainer myMarket = (MarketContainer)grdMarket.SelectedItem;
                #region Check Values
                if (string.IsNullOrEmpty(myMarket.MarketName))
                {
                    Core_AMS.Utilities.WPF.MessageError("Current market name cannot be blank.");
                    grdMarket.SelectedItem = marketContainer.FirstOrDefault(x => x.MarketID == myMarket.MarketID);
                    grdMarket.Rebind();
                    return;
                }                             
                if (markets != null && markets.FirstOrDefault(x => x.MarketID == myMarket.MarketID).MarketName != myMarket.MarketName)
                {                                        
                    if (markets.FirstOrDefault(x => x.MarketName.Equals(myMarket.MarketName, StringComparison.CurrentCultureIgnoreCase) && x.MarketID != myMarket.MarketID) != null)
                    {
                        Core_AMS.Utilities.WPF.MessageError("Current market name has been used. Please provide a unique market name.");
                        grdMarket.SelectedItem = marketContainer.FirstOrDefault(x => x.MarketID == myMarket.MarketID);
                        grdMarket.Rebind();
                        return;
                    }                    
                }                
                #endregion

                FrameworkUAD.Entity.Market saveMarket = new FrameworkUAD.Entity.Market();
                #region Get Market Variables
                saveMarket.MarketID = myMarket.MarketID;
                saveMarket.MarketName = myMarket.MarketName;
                saveMarket.MarketXML = myMarket.MarketXML;
                saveMarket.BrandID = myMarket.BrandID;
                saveMarket.DateCreated = myMarket.DateCreated;
                saveMarket.DateUpdated = myMarket.DateUpdated;
                saveMarket.CreatedByUserID = myMarket.CreatedByUserID;
                saveMarket.UpdatedByUserID = myMarket.UpdatedByUserID;
                #endregion

                marketWorker = FrameworkServices.ServiceClient.UAD_MarketClient();
                myMarket.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                saveMarket.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                marketWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, saveMarket);

                KMPlatform.Entity.User user = kmUsers.Single(x => x.UserID == saveMarket.UpdatedByUserID);
                if (user != null)
                    myMarket.UpdatedByName = user.FullName;
                myMarket.DateUpdated = DateTime.Now;

                markets = marketWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
                grdMarket.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
            }
            this.grdMarket.SelectedItem = null;
        }
        //private void grdMarket_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        //{
        //    if (e.Row is GridViewRow && !(e.Row is GridViewNewRow) && kmUsers != null)
        //    {
        //        FrameworkUAD.Entity.Market service = e.DataElement as FrameworkUAD.Entity.Market;
        //        KMPlatform.Entity.User user = kmUsers.Single(x => x.UserID == service.CreatedByUserID);

        //        TextBlock tbCreatedBy = Core_AMS.Utilities.WPF.FindChild<TextBlock>(e.Row, "tbCreatedByName");
        //        TextBlock tbUpdatedBy = Core_AMS.Utilities.WPF.FindChild<TextBlock>(e.Row, "tbUpdatedByName");

        //        if (tbCreatedBy != null)
        //        {
        //            if (user != null)
        //            {
        //                if (service.UpdatedByUserID.HasValue == true && service.CreatedByUserID == service.UpdatedByUserID)
        //                {
        //                    tbCreatedBy.Text = user.FullName;
        //                    if (tbUpdatedBy != null) tbUpdatedBy.Text = user.FullName;
        //                }
        //                else if (service.UpdatedByUserID.HasValue == false && service.CreatedByUserID != service.UpdatedByUserID)
        //                {
        //                    tbCreatedBy.Text = user.FullName;
        //                    if (tbUpdatedBy != null) tbUpdatedBy.Text = string.Empty;
        //                }
        //                else if (service.UpdatedByUserID.HasValue == true || service.CreatedByUserID != service.UpdatedByUserID)
        //                {
        //                    tbCreatedBy.Text = user.FullName;
        //                    KMPlatform.Entity.User updatedUser = kmUsers.Single(x => x.UserID == service.UpdatedByUserID);
        //                    if (tbUpdatedBy != null) tbUpdatedBy.Text = updatedUser.FullName;
        //                }
        //            }
        //        }
        //    }
        //}

        //private void rdForm_Loaded(object sender, RoutedEventArgs e)
        //{
        //    RadDataForm rdForm = sender as RadDataForm;

        //    FrameworkUAD.Entity.Market myBrand = (FrameworkUAD.Entity.Market)rdForm.DataContext;            
        //}

        private void rbNewMarket_Click(object sender, RoutedEventArgs e)
        {
            this.Background = System.Windows.Media.Brushes.DarkGray;            
            rbNewMarket.IsEnabled = false;
            grdMarket.IsEnabled = false;
            grdMarket.Background = System.Windows.Media.Brushes.DarkGray;
            rwNew.Visibility = System.Windows.Visibility.Visible;            
        }
        //private void grdMarket_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        //{
        //    MarketContainer myBrand = (MarketContainer)e.NewObject;
        //}
        #region Add new service

        private void ResetNewWindow()
        {
            tbMarketName.Clear();
            tbMarketXML.Clear();          
        }
        private void btnNewSave_Click(object sender, RoutedEventArgs e)
        {
            //FrameworkServices.ServiceClient<UAD_WS.Interface.IBrand> brandWorker = FrameworkServices.ServiceClient.UAD_BrandClient();
            //FrameworkUAD.Entity.Brand myBrand = brandWorker.FirstOrDefault(x => x.BrandName.Equals(cbbrand.selectedvalue, StringComparison.CurrentCultureIgnoreCase));
            FrameworkUAD.Entity.Market newMarket = new FrameworkUAD.Entity.Market();
            

            newMarket.MarketName = Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(tbMarketName.Text.Trim());             
            newMarket.MarketXML = Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(tbMarketXML.Text.Trim());            
            newMarket.BrandID = (int)cbBrand.SelectedValue;

            #region Check Values
            if (string.IsNullOrEmpty(newMarket.MarketName))
            {
                Core_AMS.Utilities.WPF.MessageError("Must provide a market name.");
                return;
            }
            else
            {
                if (markets.FirstOrDefault(x => x.MarketName.Equals(newMarket.MarketName, StringComparison.CurrentCultureIgnoreCase)) != null)
                {
                    Core_AMS.Utilities.WPF.MessageError("Must provide a unique market name that hasn't been used.");
                    return;
                }
            }
           
            #endregion
            
            newMarket.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            newMarket.DateCreated = DateTime.Now;
            newMarket.MarketID = marketWorker.Proxy.Save(AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, newMarket).Result;
            
            if (newMarket.MarketID > 0)
            {
                ResetNewWindow();
                MarketContainer mc = new MarketContainer(newMarket);
                KMPlatform.Entity.User user = kmUsers.Single(x => x.UserID == newMarket.CreatedByUserID);

                if (user != null)
                    mc.CreatedByName = user.FullName;

                marketContainer.Add(mc);
                grdMarket.ItemsSource = null;
                grdMarket.ItemsSource = marketContainer;

                markets = marketWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
                CloseWindow();
                this.grdMarket.SelectedItem = null;
            }
            else
                Core_AMS.Utilities.WPF.MessageServiceError();
        }
        private void CloseWindow()
        {
            this.Background = System.Windows.Media.Brushes.Transparent;            
            rbNewMarket.IsEnabled = true;
            grdMarket.IsEnabled = true;
            grdMarket.Background = System.Windows.Media.Brushes.Transparent;

            rwNew.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void btnNewCancel_Click(object sender, RoutedEventArgs e)
        {
            ResetNewWindow();
            CloseWindow();
            this.grdMarket.SelectedItem = null;
        }
        #endregion
    }

    [Serializable]
    [DataContract]
    public class MarketContainer : FrameworkUAD.Entity.Market
    {
        
        public MarketContainer(FrameworkUAD.Entity.Market market)
        {
            MarketID = market.MarketID;
            MarketName = market.MarketName;
            MarketXML = market.MarketXML;
            BrandID = market.BrandID;
            DateCreated = market.DateCreated;
            DateUpdated = market.DateUpdated;
            CreatedByUserID = market.CreatedByUserID;
            UpdatedByUserID = market.UpdatedByUserID;
            CreatedByName = string.Empty;
            UpdatedByName = string.Empty;

        }
        #region Properties
        [DataMember]
        public int MarketID { get; set; }
        [DataMember]
        public string MarketName { get; set; }
        [DataMember]
        public string MarketXML { get; set; }
        [DataMember]
        public int BrandID { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        [DataMember]
        public string CreatedByName { get; set; }
        [DataMember]
        public string UpdatedByName { get; set; }
        #endregion
    }
}
