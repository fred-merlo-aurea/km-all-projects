using FrameworkUAS.Object;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Data;

namespace ControlCenter.Modules
{
    /// <summary>
    /// Interaction logic for MenuFeatureMgmt.xaml
    /// </summary>
    public partial class MenuFeatureMgmt : UserControl
    {
        //private FrameworkServices.ServiceClient<UAS_WS.Interface.IMenuFeature> menuFeatureWorker { get; set; }
        private List<KMPlatform.Entity.User> kmUsers { get; set; }        
        //public List<KMPlatform.Entity.MenuFeature> menus { get; set; }
        public MenuFeatureMgmt()
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
        private void LoadData()
        {
            //startUp = false;
            SortDescriptor sort = new SortDescriptor();
            sort.Member = "MenuName";
            sort.SortDirection = ListSortDirection.Ascending;
            grdMenuFeature.SortDescriptors.Add(sort);
            //menuFeatureWorker = FrameworkServices.ServiceClient.UAS_MenuFeatureClient();
            //menus = menuFeatureWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
            //grdMenuFeature.ItemsSource = menus;

            KMPlatform.Entity.ClientGroup cgKM = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.ClientGroups.SingleOrDefault(x => x.ClientGroupName.Equals("Knowledge Marketing"));

            FrameworkServices.ServiceClient<UAS_WS.Interface.IUser> userWorker = FrameworkServices.ServiceClient.UAS_UserClient();
            kmUsers = userWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, cgKM.ClientGroupID, false).Result;            
        }
        private void lstEnabled_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grdMenuFeature != null && grdMenuFeature.Columns.Count > 0)
            {
                Telerik.Windows.Controls.GridViewColumn isEnabledColumn = grdMenuFeature.Columns["IsActive"];

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
                    grdMenuFeature.FilterDescriptors.SuspendNotifications();
                    isEnabledColumn.ClearFilters();
                    grdMenuFeature.FilterDescriptors.ResumeNotifications();
                }
            }

        }
        private void rbCancel_Click(object sender, RoutedEventArgs e)
        {
            grdMenuFeature.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
            this.grdMenuFeature.SelectedItem = null;
        }
        private void grdMenuFeature_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (this.grdMenuFeature.SelectedItem != null)
            {
                grdMenuFeature.ScrollIntoView(this.grdMenuFeature.SelectedItem);
                grdMenuFeature.UpdateLayout();
                grdMenuFeature.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.VisibleWhenSelected;
            }
        }
        private void rdForm_EditEnded(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndedEventArgs e)
        {
            if (e.EditAction == Telerik.Windows.Controls.Data.DataForm.EditAction.Cancel)
                grdMenuFeature.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
            else
            {
                //EditAction == Commit
                //save changes
                //KMPlatform.Entity.MenuFeature myMenuFeature = (KMPlatform.Entity.MenuFeature)grdMenuFeature.SelectedItem;
                #region Check Values
                //List<KMPlatform.Entity.MenuFeature> featureList = menuFeatureWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
                //if (string.IsNullOrEmpty(myMenuFeature.FeatureName))
                //{
                //    Core_AMS.Utilities.WPF.MessageError("Current brand name cannot be blank.");
                //    grdMenuFeature.SelectedItem = featureList.FirstOrDefault(x => x.MenuFeatureID == myMenuFeature.MenuFeatureID);
                //    grdMenuFeature.Rebind();
                //    return;
                //}                             
                //if (featureList != null && featureList.FirstOrDefault(x => x.MenuFeatureID == myMenuFeature.MenuFeatureID).FeatureName != myMenuFeature.FeatureName)
                //{                                        
                //    if (featureList.FirstOrDefault(x => x.FeatureName.Equals(myMenuFeature.FeatureName, StringComparison.CurrentCultureIgnoreCase) && x.MenuFeatureID != myMenuFeature.MenuFeatureID) != null)
                //    {
                //        Core_AMS.Utilities.WPF.MessageError("Current menu feature name has been used. Please provide a unique menu feature name.");
                //        grdMenuFeature.SelectedItem = featureList.FirstOrDefault(x => x.MenuFeatureID == myMenuFeature.MenuFeatureID);
                //        grdMenuFeature.Rebind();
                //        return;
                //    }                    
                //}                
                #endregion
                //menuFeatureWorker = FrameworkServices.ServiceClient.UAS_MenuFeatureClient();
                //myMenuFeature.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                //menuFeatureWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myMenuFeature);

                grdMenuFeature.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
            }
            this.grdMenuFeature.SelectedItem = null;
        }
        private void grdMenuFeature_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if (e.Row is GridViewRow && !(e.Row is GridViewNewRow) && kmUsers != null)
            {
                //KMPlatform.Entity.MenuFeature service = e.DataElement as KMPlatform.Entity.MenuFeature;
                //KMPlatform.Entity.User user = kmUsers.Single(x => x.UserID == service.CreatedByUserID);

                //TextBlock tbCreatedBy = Core_AMS.Utilities.WPF.FindChild<TextBlock>(e.Row, "tbCreatedByName");
                //TextBlock tbUpdatedBy = Core_AMS.Utilities.WPF.FindChild<TextBlock>(e.Row, "tbUpdatedByName");

                //if (tbCreatedBy != null)
                //{
                //    if (user != null)
                //    {
                //        if (service.UpdatedByUserID.HasValue == true && service.CreatedByUserID == service.UpdatedByUserID)
                //        {
                //            tbCreatedBy.Text = user.FullName;
                //            if (tbUpdatedBy != null) tbUpdatedBy.Text = user.FullName;
                //        }
                //        else if (service.UpdatedByUserID.HasValue == false && service.CreatedByUserID != service.UpdatedByUserID)
                //        {
                //            tbCreatedBy.Text = user.FullName;
                //            if (tbUpdatedBy != null) tbUpdatedBy.Text = string.Empty;
                //        }
                //        }
                //    }
                //}
            }
        }
        private void rdForm_Loaded(object sender, RoutedEventArgs e)
        {
            RadDataForm rdForm = sender as RadDataForm;

            //KMPlatform.Entity.MenuFeature myBrand = (KMPlatform.Entity.MenuFeature)rdForm.DataContext;            
        }

        private void rbNewMenuFeature_Click(object sender, RoutedEventArgs e)
        {
            this.Background = System.Windows.Media.Brushes.DarkGray;
            lstActive.IsEnabled = false;
            rbNewMenuFeature.IsEnabled = false;
            grdMenuFeature.IsEnabled = false;
            grdMenuFeature.Background = System.Windows.Media.Brushes.DarkGray;
            rwNew.Visibility = System.Windows.Visibility.Visible;            
        }
        private void grdMenuFeature_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
            //KMPlatform.Entity.MenuFeature myBrand = (KMPlatform.Entity.MenuFeature)e.NewObject;
        }
        #region Add new service

        private void ResetNewWindow()
        {
            tbFeatureName.Clear();
            cbIsActive.IsChecked = false;            
        }
        private void btnNewSave_Click(object sender, RoutedEventArgs e)
        {
            //KMPlatform.Entity.MenuFeature newMenuFeature = new KMPlatform.Entity.MenuFeature();
                                    
            //newMenuFeature.FeatureName = Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(tbFeatureName.Text.Trim());    
            //newMenuFeature.IsActive = cbIsActive.IsChecked.Value;                                    

            #region Check Values
            //if (string.IsNullOrEmpty(newMenuFeature.FeatureName))
            //{
            //    Core_AMS.Utilities.WPF.MessageError("Must provide a menu feature name.");
            //    return;
            //}
            //else
            //{
            //    List<KMPlatform.Entity.MenuFeature> brandList = menuFeatureWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
            //    if (brandList.FirstOrDefault(x => x.FeatureName.Equals(newMenuFeature.FeatureName, StringComparison.CurrentCultureIgnoreCase)) != null)
            //    {
            //        Core_AMS.Utilities.WPF.MessageError("Must provide a unique menu feature name that hasn't been used.");
            //        return;
            //    }
            //}
           
            //#endregion
            
            //newMenuFeature.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            //newMenuFeature.DateCreated = DateTime.Now;
            //newMenuFeature.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            //newMenuFeature.DateUpdated = DateTime.Now;            

            //newMenuFeature.MenuFeatureID = menuFeatureWorker.Proxy.Save(AppData.myAppData.AuthorizedUser.AuthAccessKey, newMenuFeature).Result;
            //if (newMenuFeature.MenuFeatureID > 0)
            //{
            //    ResetNewWindow();

            //    List<KMPlatform.Entity.MenuFeature> menus = (List<KMPlatform.Entity.MenuFeature>)grdMenuFeature.ItemsSource;
            //    menus.Add(newMenuFeature);
            //    grdMenuFeature.ItemsSource = null;
            //    grdMenuFeature.ItemsSource = menus;

            //    CloseWindow();
            //    this.grdMenuFeature.SelectedItem = null;
            //}
            //else
            //    Core_AMS.Utilities.WPF.MessageServiceError();
            #endregion

        }
        private void CloseWindow()
        {
            this.Background = System.Windows.Media.Brushes.Transparent;
            lstActive.IsEnabled = true;
            rbNewMenuFeature.IsEnabled = true;
            grdMenuFeature.IsEnabled = true;
            grdMenuFeature.Background = System.Windows.Media.Brushes.Transparent;

            rwNew.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void btnNewCancel_Click(object sender, RoutedEventArgs e)
        {
            ResetNewWindow();
            //List<KMPlatform.Entity.MenuFeature> services = (List<KMPlatform.Entity.MenuFeature>)grdMenuFeature.ItemsSource;
            //grdMenuFeature.ItemsSource = services;
            CloseWindow();
            this.grdMenuFeature.SelectedItem = null;
        }
        #endregion
    }
}
