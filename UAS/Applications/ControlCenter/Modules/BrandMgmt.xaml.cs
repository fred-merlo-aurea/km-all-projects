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
    /// Interaction logic for BrandMgmt.xaml
    /// </summary>
    public partial class BrandMgmt : UserControl
    {
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IBrand> brandWorker { get; set; }
        private List<KMPlatform.Entity.User> kmUsers { get; set; }        
        public List<FrameworkUAD.Entity.Brand> brands { get; set; }
        public List<BrandContainer> brandContainer = new List<BrandContainer>();
        public BrandMgmt()
        {
            if (FrameworkUAS.Object.AppData.IsKmUser() == true)
            {
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
            sort.Member = "BrandName";
            sort.SortDirection = ListSortDirection.Ascending;
            grdBrand.SortDescriptors.Add(sort);
            brandWorker = FrameworkServices.ServiceClient.UAD_BrandClient();
            brands = brandWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;

            KMPlatform.Entity.ClientGroup cgKM = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.ClientGroups.SingleOrDefault(x => x.ClientGroupName.Equals("Knowledge Marketing"));
            FrameworkServices.ServiceClient<UAS_WS.Interface.IUser> userWorker = FrameworkServices.ServiceClient.UAS_UserClient();
            kmUsers = userWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, cgKM.ClientGroupID, false).Result;

            foreach (FrameworkUAD.Entity.Brand brand in brands)
            {
                BrandContainer bc = new BrandContainer(brand);
                KMPlatform.Entity.User user = kmUsers.FirstOrDefault(x => x.UserID == brand.CreatedUserID);

                if (user != null)
                {
                    bc.CreatedByName = user.FullName;

                    user = kmUsers.FirstOrDefault(x => x.UserID == brand.UpdatedUserID);
                    if (user != null)
                        bc.UpdatedByName = user.FullName;
                }

                brandContainer.Add(bc);
            }
            
            grdBrand.ItemsSource = brandContainer;
        }
        private void lstEnabled_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grdBrand != null && grdBrand.Columns.Count > 0)
            {
                Telerik.Windows.Controls.GridViewColumn isEnabledColumn = grdBrand.Columns["IsDeleted"];

                string isEnabled = lstEnabled.SelectedItem.ToString().Replace("System.Windows.Controls.ListBoxItem: ", "");
                //All, Enabled, Not Enabled

                if (!isEnabled.Equals("All"))
                {
                    if (isEnabled.Equals("Deleted")) isEnabled = "true";
                    else isEnabled = "false";

                    Telerik.Windows.Controls.GridView.IColumnFilterDescriptor isEnabledFilter = isEnabledColumn.ColumnFilterDescriptor;
                    // Suspend the notifications to avoid multiple data engine updates
                    isEnabledFilter.SuspendNotifications();

                    // This is the same as the end user configuring the upper field filter.
                    isEnabledFilter.FieldFilter.Filter1.Operator = Telerik.Windows.Data.FilterOperator.IsEqualTo;
                    isEnabledFilter.FieldFilter.Filter1.Value = isEnabled;
                    isEnabledFilter.FieldFilter.Filter1.IsCaseSensitive = false;

                    // This is the same as the end user changing the logical operator between the two field filters.
                    //countryFilter.FieldFilter.LogicalOperator = Telerik.Windows.Data.FilterCompositionLogicalOperator.Or;

                    // This is the same as the end user configuring the lower field filter.
                    //countryFilter.FieldFilter.Filter2.Operator = Telerik.Windows.Data.FilterOperator.Contains;
                    //countryFilter.FieldFilter.Filter2.Value = "stan";
                    //countryFilter.FieldFilter.Filter2.IsCaseSensitive = true;

                    // Resume the notifications to force the data engine to update the filter.
                    isEnabledFilter.ResumeNotifications();
                }
                else
                {
                    grdBrand.FilterDescriptors.SuspendNotifications();
                    isEnabledColumn.ClearFilters();
                    grdBrand.FilterDescriptors.ResumeNotifications();
                }
            }

        }
        private void rbCancel_Click(object sender, RoutedEventArgs e)
        {
            grdBrand.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
            this.grdBrand.SelectedItem = null;
        }
        private void grdBrand_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (this.grdBrand.SelectedItem != null)
            {
                grdBrand.ScrollIntoView(this.grdBrand.SelectedItem);
                grdBrand.UpdateLayout();
                grdBrand.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.VisibleWhenSelected;
            }
        }
        private void rdForm_EditEnded(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndedEventArgs e)
        {
            if (e.EditAction == Telerik.Windows.Controls.Data.DataForm.EditAction.Cancel)
                grdBrand.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
            else
            {
                //EditAction == Commit
                //save changes
                BrandContainer myBrand = (BrandContainer)grdBrand.SelectedItem;
                #region Check Values
                if (string.IsNullOrEmpty(myBrand.BrandName))
                {
                    Core_AMS.Utilities.WPF.MessageError("Current brand name cannot be blank.");
                    grdBrand.SelectedItem = brands.FirstOrDefault(x => x.BrandID == myBrand.BrandID);
                    grdBrand.Rebind();
                    return;
                }                             
                if (brands != null && brands.FirstOrDefault(x => x.BrandID == myBrand.BrandID).BrandName != myBrand.BrandName)
                {                                        
                    if (brands.FirstOrDefault(x => x.BrandName.Equals(myBrand.BrandName, StringComparison.CurrentCultureIgnoreCase) && x.BrandID != myBrand.BrandID) != null)
                    {
                        Core_AMS.Utilities.WPF.MessageError("Current brand name has been used. Please provide a unique service name.");
                        grdBrand.SelectedItem = brands.FirstOrDefault(x => x.BrandID == myBrand.BrandID);
                        grdBrand.Rebind();
                        return;
                    }                    
                }                
                #endregion

                FrameworkUAD.Entity.Brand saveBrand = new FrameworkUAD.Entity.Brand();
                #region Get Brand Variables
                saveBrand.BrandID = myBrand.BrandID;
                saveBrand.BrandName = myBrand.BrandName;
                saveBrand.Logo = myBrand.Logo;
                saveBrand.IsBrandGroup = myBrand.IsBrandGroup;
                saveBrand.IsDeleted = myBrand.IsDeleted;
                saveBrand.CreatedUserID = myBrand.CreatedUserID;
                saveBrand.CreatedDate = myBrand.CreatedDate;
                saveBrand.UpdatedUserID = myBrand.UpdatedUserID;
                saveBrand.UpdatedDate = myBrand.UpdatedDate;
                #endregion

                brandWorker = FrameworkServices.ServiceClient.UAD_BrandClient();
                myBrand.UpdatedUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                saveBrand.UpdatedUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                brandWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, saveBrand);

                KMPlatform.Entity.User user = kmUsers.FirstOrDefault(x => x.UserID == saveBrand.UpdatedUserID);
                if (user != null)
                    myBrand.UpdatedByName = user.FullName;
                myBrand.UpdatedDate = DateTime.Now;

                brands = brandWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
                grdBrand.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
            }
            this.grdBrand.SelectedItem = null;
        }
        //private void grdBrand_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        //{
        //    if (e.Row is GridViewRow && !(e.Row is GridViewNewRow) && kmUsers != null)
        //    {
        //        FrameworkUAD.Entity.Brand service = e.DataElement as FrameworkUAD.Entity.Brand;
        //        KMPlatform.Entity.User user = kmUsers.Single(x => x.UserID == service.CreatedUserID);

        //        TextBlock tbCreatedBy = Core_AMS.Utilities.WPF.FindChild<TextBlock>(e.Row, "tbCreatedByName");
        //        TextBlock tbUpdatedBy = Core_AMS.Utilities.WPF.FindChild<TextBlock>(e.Row, "tbUpdatedByName");

        //        if (tbCreatedBy != null)
        //        {
        //            if (user != null)
        //            {
        //                if (service.UpdatedUserID.HasValue == true && service.CreatedUserID == service.UpdatedUserID)
        //                {
        //                    tbCreatedBy.Text = user.FullName;
        //                    if (tbUpdatedBy != null) tbUpdatedBy.Text = user.FullName;
        //                }
        //                else if (service.UpdatedUserID.HasValue == false && service.CreatedUserID != service.UpdatedUserID)
        //                {
        //                    tbCreatedBy.Text = user.FullName;
        //                    if (tbUpdatedBy != null) tbUpdatedBy.Text = string.Empty;
        //                }
        //                else if (service.UpdatedUserID.HasValue == true || service.CreatedUserID != service.UpdatedUserID)
        //                {
        //                    tbCreatedBy.Text = user.FullName;
        //                    KMPlatform.Entity.User updatedUser = kmUsers.Single(x => x.UserID == service.UpdatedUserID);
        //                    if (tbUpdatedBy != null) tbUpdatedBy.Text = updatedUser.FullName;
        //                }
        //            }
        //        }
        //    }
        //}

        //private void rdForm_Loaded(object sender, RoutedEventArgs e)
        //{
        //    RadDataForm rdForm = sender as RadDataForm;

        //    FrameworkUAD.Entity.Brand myBrand = (FrameworkUAD.Entity.Brand)rdForm.DataContext;            
        //}

        private void rbNewBrand_Click(object sender, RoutedEventArgs e)
        {
            this.Background = System.Windows.Media.Brushes.DarkGray;
            lstEnabled.IsEnabled = false;
            rbNewBrand.IsEnabled = false;
            grdBrand.IsEnabled = false;
            grdBrand.Background = System.Windows.Media.Brushes.DarkGray;
            rwNew.Visibility = System.Windows.Visibility.Visible;            
        }
        //private void grdBrand_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        //{
        //    FrameworkUAD.Entity.Brand myBrand = (FrameworkUAD.Entity.Brand)e.NewObject;
        //}
        #region Add new service

        private void ResetNewWindow()
        {
            tbBrandName.Clear();
            tbLogo.Clear();
            cbIsBrandGroup.IsChecked = false;
            cbIsDeleted.IsChecked = false;
        }
        private void btnNewSave_Click(object sender, RoutedEventArgs e)
        {
            FrameworkUAD.Entity.Brand newBrand = new FrameworkUAD.Entity.Brand();

            newBrand.BrandName = Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(tbBrandName.Text.Trim());             
            newBrand.Logo = Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(tbBrandName.Text.Trim());
            newBrand.IsBrandGroup = cbIsBrandGroup.IsChecked.Value;
            newBrand.IsDeleted = cbIsDeleted.IsChecked.Value;

            #region Check Values
            if (string.IsNullOrEmpty(newBrand.BrandName))
            {
                Core_AMS.Utilities.WPF.MessageError("Must provide a brand name.");
                return;
            }
            else
            {
                if (brands.FirstOrDefault(x => x.BrandName.Equals(newBrand.BrandName, StringComparison.CurrentCultureIgnoreCase)) != null)
                {
                    Core_AMS.Utilities.WPF.MessageError("Must provide a unique brand name that hasn't been used.");
                    return;
                }
            }
           
            #endregion
            
            newBrand.CreatedUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            newBrand.CreatedDate = DateTime.Now;

            newBrand.BrandID = brandWorker.Proxy.Save(AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, newBrand).Result;
            if (newBrand.BrandID > 0)
            {
                ResetNewWindow();

                BrandContainer bc = new BrandContainer(newBrand);
                KMPlatform.Entity.User user = kmUsers.FirstOrDefault(x => x.UserID == newBrand.CreatedUserID);

                if (user != null)
                    bc.CreatedByName = user.FullName;

                brandContainer.Add(bc);
                grdBrand.ItemsSource = null;
                grdBrand.ItemsSource = brandContainer;

                brands = brandWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
                CloseWindow();
                this.grdBrand.SelectedItem = null;
            }
            else
                Core_AMS.Utilities.WPF.MessageServiceError();
            
        }
        private void CloseWindow()
        {
            this.Background = System.Windows.Media.Brushes.Transparent;
            lstEnabled.IsEnabled = true;
            rbNewBrand.IsEnabled = true;
            grdBrand.IsEnabled = true;
            grdBrand.Background = System.Windows.Media.Brushes.Transparent;

            rwNew.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void btnNewCancel_Click(object sender, RoutedEventArgs e)
        {
            ResetNewWindow();
            CloseWindow();
            this.grdBrand.SelectedItem = null;
        }
        #endregion
    }

    [Serializable]
    [DataContract]
    public class BrandContainer
    {
        public BrandContainer(FrameworkUAD.Entity.Brand brand)
        {
            BrandID = brand.BrandID;
            BrandName = brand.BrandName;
            Logo = brand.Logo;
            IsBrandGroup = brand.IsBrandGroup;
            IsDeleted = brand.IsDeleted;
            CreatedUserID = brand.CreatedUserID;
            CreatedDate = brand.CreatedDate;
            UpdatedUserID = brand.UpdatedUserID;
            UpdatedDate = brand.UpdatedDate;
            CreatedByName = string.Empty;
            UpdatedByName = string.Empty;
        }
        #region Properties
        [DataMember]
        public int BrandID { get; set; }
        [DataMember]
        public string BrandName { get; set; }
        [DataMember]
        public string Logo { get; set; }
        [DataMember]
        public bool IsBrandGroup { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public string CreatedByName { get; set; }
        [DataMember]
        public string UpdatedByName { get; set; }
        #endregion
    }
}
