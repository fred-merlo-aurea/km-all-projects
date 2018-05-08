using FrameworkUAS.Object;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Data;

namespace ControlCenter.Modules
{
    /// <summary>
    /// Interaction logic for MenuMgmt.xaml
    /// </summary>
    public partial class MenuManagement : UserControl
    {
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IMenu> menuWorker { get; set; }
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IApplication> appData = FrameworkServices.ServiceClient.UAS_ApplicationClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IServiceFeature> serviceWorker = FrameworkServices.ServiceClient.UAS_ServiceFeatureClient();
        private List<KMPlatform.Entity.User> kmUsers { get; set; }        
        public List<KMPlatform.Entity.Menu> menus { get; set; }
        private List<KMPlatform.Entity.Application> appList = new List<KMPlatform.Entity.Application>();
        private List<KMPlatform.Entity.ServiceFeature> serviceList = new List<KMPlatform.Entity.ServiceFeature>();
        private List<MenuContainer> MenuContainerList = new List<MenuContainer>();

        public MenuManagement()
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
            grdMenu.SortDescriptors.Add(sort);
            menuWorker = FrameworkServices.ServiceClient.UAS_MenuClient();
            menus = menuWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
           

            appList = appData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
            serviceList = serviceWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;

            KMPlatform.Entity.ClientGroup cgKM = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.ClientGroups.SingleOrDefault(x => x.ClientGroupName.Equals("Knowledge Marketing"));

            FrameworkServices.ServiceClient<UAS_WS.Interface.IUser> userWorker = FrameworkServices.ServiceClient.UAS_UserClient();
            kmUsers = userWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, cgKM.ClientGroupID, false).Result;
            MenuContainerList.Clear();
            foreach(KMPlatform.Entity.Menu menu in menus)
            {
                MenuContainer mc = new MenuContainer(menu);

                KMPlatform.Entity.ServiceFeature mySF = serviceList.FirstOrDefault(x => x.ServiceFeatureID == menu.ServiceFeatureID);
                if(mySF != null)
                {
                    mc.ServiceFeatureName = mySF.SFName;
                }

                KMPlatform.Entity.Application app = appList.FirstOrDefault(x => x.ApplicationID == menu.ApplicationID);
                if(app != null)
                {
                    mc.ApplicationName = app.ApplicationName;
                    
                }

                KMPlatform.Entity.Menu parentMenu = menus.FirstOrDefault(x => x.MenuID == menu.ParentMenuID);
                if(parentMenu != null)
                {
                    mc.ParentMenuName = parentMenu.MenuName;
                }

                KMPlatform.Entity.User createdBy = kmUsers.FirstOrDefault(x => x.UserID == menu.CreatedByUserID);
                if(createdBy != null)
                {
                    mc.CreatedByName = createdBy.FullName;
                }

                KMPlatform.Entity.User updatedBy = kmUsers.FirstOrDefault(x => x.UserID == menu.CreatedByUserID);
                if (createdBy != null)
                {
                    mc.UpdatedByName = updatedBy.FullName;
                }

                MenuContainerList.Add(mc);
            }
            grdMenu.ItemsSource = MenuContainerList;
        }
        private void lstEnabled_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grdMenu != null && grdMenu.Columns.Count > 0)
            {
                Telerik.Windows.Controls.GridViewColumn isEnabledColumn = grdMenu.Columns["IsActive"];

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
                    grdMenu.FilterDescriptors.SuspendNotifications();
                    isEnabledColumn.ClearFilters();
                    grdMenu.FilterDescriptors.ResumeNotifications();
                }
            }

        }
        private void rbCancel_Click(object sender, RoutedEventArgs e)
        {
            grdMenu.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
            this.grdMenu.SelectedItem = null;
        }
        private void grdMenu_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (this.grdMenu.SelectedItem != null)
            {
                grdMenu.ScrollIntoView(this.grdMenu.SelectedItem);
                grdMenu.UpdateLayout();
                grdMenu.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.VisibleWhenSelected;
            }
        }
        private void rdForm_EditEnded(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndedEventArgs e)
        {
            if (e.EditAction == Telerik.Windows.Controls.Data.DataForm.EditAction.Cancel)
                grdMenu.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
            else
            {
                //EditAction == Commit
                //save changes
                MenuContainer selectedMenu = (MenuContainer)grdMenu.SelectedItem;
                RadDataForm rdForm = sender as RadDataForm;

                RadComboBox cbApp = Core_AMS.Utilities.WPF.FindControl<RadComboBox>(rdForm, "cbApp");
                RadComboBox cbSF = Core_AMS.Utilities.WPF.FindControl<RadComboBox>(rdForm, "cbSF");
                RadComboBox cbPMenu = Core_AMS.Utilities.WPF.FindControl<RadComboBox>(rdForm, "cbPMenu");

                selectedMenu.ApplicationID = (int)cbApp.SelectedValue;

                if (cbSF.SelectedValue != null)
                {
                    selectedMenu.ServiceFeatureID = (int)cbSF.SelectedValue;
                }

                selectedMenu.ParentMenuID = (int)cbPMenu.SelectedValue;

                KMPlatform.Entity.Menu myMenu = selectedMenu.ToMenu();
                #region Check Values
                List<KMPlatform.Entity.Menu> servList = menuWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
                if (string.IsNullOrEmpty(myMenu.MenuName))
                {
                    Core_AMS.Utilities.WPF.MessageError("Current menu name cannot be blank.");
                    grdMenu.SelectedItem = servList.FirstOrDefault(x => x.MenuID == myMenu.MenuID);
                    grdMenu.Rebind();
                    return;
                }                             
                else if (servList != null && servList.FirstOrDefault(x => x.MenuID == myMenu.MenuID).MenuName != myMenu.MenuName)
                {                                        
                    if (servList.FirstOrDefault(x => x.MenuName.Equals(myMenu.MenuName, StringComparison.CurrentCultureIgnoreCase) && x.MenuID != myMenu.MenuID) != null)
                    {
                        Core_AMS.Utilities.WPF.MessageError("Current menu name has been used. Please provide a unique menu name.");
                        grdMenu.SelectedItem = servList.FirstOrDefault(x => x.MenuID == myMenu.MenuID);
                        grdMenu.Rebind();
                        return;
                    }                    
                }
                else
                {
                    menuWorker.Proxy.Save(AppData.myAppData.AuthorizedUser.AuthAccessKey, myMenu);
                    LoadData();
                    grdMenu.Rebind();
                }
                #endregion

                grdMenu.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
                
            }
            this.grdMenu.SelectedItem = null;
        }
        private void grdMenu_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if (e.Row is GridViewRow && !(e.Row is GridViewNewRow) && kmUsers != null)
            {
                MenuContainer service = e.DataElement as MenuContainer;
                KMPlatform.Entity.User user = kmUsers.Single(x => x.UserID == service.CreatedByUserID);

                TextBlock tbCreatedBy = Core_AMS.Utilities.WPF.FindChild<TextBlock>(e.Row, "tbCreatedByName");
                TextBlock tbUpdatedBy = Core_AMS.Utilities.WPF.FindChild<TextBlock>(e.Row, "tbUpdatedByName");

                if (tbCreatedBy != null)
                {
                    if (user != null)
                    {
                        if (service.UpdatedByUserID.HasValue == true && service.CreatedByUserID == service.UpdatedByUserID)
                        {
                            tbCreatedBy.Text = user.FullName;
                            if (tbUpdatedBy != null) tbUpdatedBy.Text = user.FullName;
                        }
                        else if (service.UpdatedByUserID.HasValue == false && service.CreatedByUserID != service.UpdatedByUserID)
                        {
                            tbCreatedBy.Text = user.FullName;
                            if (tbUpdatedBy != null) tbUpdatedBy.Text = string.Empty;
                        }
                        else if (service.UpdatedByUserID.HasValue == true || service.CreatedByUserID != service.UpdatedByUserID)
                        {
                            tbCreatedBy.Text = user.FullName;
                            KMPlatform.Entity.User updatedUser = kmUsers.Single(x => x.UserID == service.UpdatedByUserID);
                            if (tbUpdatedBy != null) tbUpdatedBy.Text = updatedUser.FullName;
                        }
                    }
                }
            }
        }
        private void rdForm_Loaded(object sender, RoutedEventArgs e)
        {
            RadDataForm rdForm = sender as RadDataForm;
            MenuContainer myMenu = (MenuContainer)rdForm.DataContext;

            RadComboBox cbApp = Core_AMS.Utilities.WPF.FindControl<RadComboBox>(rdForm, "cbApp");
            RadComboBox cbSF = Core_AMS.Utilities.WPF.FindControl<RadComboBox>(rdForm, "cbSF");
            RadComboBox cbPMenu = Core_AMS.Utilities.WPF.FindControl<RadComboBox>(rdForm, "cbPMenu");

            if (appList != null)
            {
                cbApp.ItemsSource = appList;
                cbApp.DisplayMemberPath = "ApplicationName";
                cbApp.SelectedValuePath = "ApplicationID";

                cbApp.SelectedValue = myMenu.ApplicationID;
            }

            if (serviceList != null)
            {
                cbSF.ItemsSource = serviceList;
                cbSF.DisplayMemberPath = "SFName";
                cbSF.SelectedValuePath = "ServiceFeatureID";

                cbSF.SelectedValue = myMenu.ServiceFeatureID;
            }

            if (menus != null)
            {
                cbPMenu.ItemsSource = menus;
                cbPMenu.DisplayMemberPath = "MenuName";
                cbPMenu.SelectedValuePath = "MenuID";

                cbPMenu.SelectedValue = myMenu.ParentMenuID;
            }
        }

        private void rbNewMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Background = System.Windows.Media.Brushes.DarkGray;
            lstActive.IsEnabled = false;
            rbNewMenu.IsEnabled = false;
            grdMenu.IsEnabled = false;
            grdMenu.Background = System.Windows.Media.Brushes.DarkGray;
            rwNew.Visibility = System.Windows.Visibility.Visible;

            cbApplication.ItemsSource = appList;
            cbApplication.DisplayMemberPath = "ApplicationName";
            cbApplication.SelectedValuePath = "ApplicationID";

            cbServiceFeature.ItemsSource = serviceList;
            cbServiceFeature.DisplayMemberPath = "SFName";
            cbServiceFeature.SelectedValuePath = "ServiceFeatureID";

        }
        private void grdMenu_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
            KMPlatform.Entity.Menu myBrand = (KMPlatform.Entity.Menu)e.NewObject;
        }
        #region Add new service

        private void ResetNewWindow()
        {
            cbApplication.SelectedValue = -1;
            cbIsServiceFeature.IsChecked = false;
            cbServiceFeature.SelectedValue = -1;
            tbMenuName.Clear();
            tbDescription.Clear();
            cbIsParent.IsChecked = false;
            cbParentMenu.SelectedValue = -1;            
            tbURL.Clear();
            cbIsActive.IsChecked = false;
            nudMenuOrder.Value = 1;
            cbIsActive.IsChecked = false;
            tbImagePath.Clear();
        }
        private void btnNewSave_Click(object sender, RoutedEventArgs e)
        {
            KMPlatform.Entity.Menu newMenu = new KMPlatform.Entity.Menu();
                        
            int appID = 0;
            int.TryParse(cbApplication.SelectedValue.ToString(), out appID);
            newMenu.ApplicationID = appID;
            newMenu.IsServiceFeature = cbIsServiceFeature.IsChecked.Value;
            int sfID = 0;
            int.TryParse(cbServiceFeature.SelectedValue.ToString(), out sfID);
            newMenu.ServiceFeatureID = sfID;
            newMenu.MenuName = Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(tbMenuName.Text.Trim());
            newMenu.Description = Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(tbDescription.Text.Trim());
            newMenu.IsParent = cbIsParent.IsChecked.Value;
            int pmID = 0;
            int.TryParse(cbParentMenu.SelectedValue.ToString(), out pmID);
            newMenu.ParentMenuID = pmID;
            newMenu.URL = Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(tbURL.Text.Trim());
            newMenu.IsActive = cbIsActive.IsChecked.Value;            
            newMenu.MenuOrder = (int)nudMenuOrder.Value;
            newMenu.HasFeatures = cbIsActive.IsChecked.Value;
            newMenu.ImagePath = Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(tbImagePath.Text.Trim());

            #region Check Values
            if (string.IsNullOrEmpty(newMenu.MenuName))
            {
                Core_AMS.Utilities.WPF.MessageError("Must provide a menu name.");
                return;
            }
            else
            {
                List<KMPlatform.Entity.Menu> brandList = menuWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
                if (brandList.FirstOrDefault(x => x.MenuName.Equals(newMenu.MenuName, StringComparison.CurrentCultureIgnoreCase)) != null)
                {
                    Core_AMS.Utilities.WPF.MessageError("Must provide a unique menu name that hasn't been used.");
                    return;
                }
            }
           
            #endregion
            
            newMenu.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            newMenu.DateCreated = DateTime.Now;
            newMenu.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            newMenu.DateUpdated = DateTime.Now;            

            newMenu.MenuID = menuWorker.Proxy.Save(AppData.myAppData.AuthorizedUser.AuthAccessKey, newMenu).Result;
            if (newMenu.MenuID > 0)
            {
                ResetNewWindow();
                LoadData();

                CloseWindow();
                this.grdMenu.SelectedItem = null;
            }
            else
                Core_AMS.Utilities.WPF.MessageServiceError();
            
        }
        private void CloseWindow()
        {
            this.Background = System.Windows.Media.Brushes.Transparent;
            lstActive.IsEnabled = true;
            rbNewMenu.IsEnabled = true;
            grdMenu.IsEnabled = true;
            grdMenu.Background = System.Windows.Media.Brushes.Transparent;

            rwNew.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void btnNewCancel_Click(object sender, RoutedEventArgs e)
        {
            ResetNewWindow();
            List<KMPlatform.Entity.Menu> services = (List<KMPlatform.Entity.Menu>)grdMenu.ItemsSource;
            grdMenu.ItemsSource = services;
            CloseWindow();
            this.grdMenu.SelectedItem = null;
        }
        #endregion

        private void cbApplication_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbParentMenu.ItemsSource = menus.Where(x => x.ApplicationID == (int)cbApplication.SelectedValue);
            cbParentMenu.DisplayMemberPath = "MenuName";
            cbParentMenu.SelectedValuePath = "MenuID";
        }

        private void rdForm_BeginningEdit(object sender, CancelEventArgs e)
        {
            RadDataForm rdForm = sender as RadDataForm;

            RadComboBox cbApp = Core_AMS.Utilities.WPF.FindControl<RadComboBox>(rdForm, "cbApp");
            RadComboBox cbSF = Core_AMS.Utilities.WPF.FindControl<RadComboBox>(rdForm, "cbSF");
            RadComboBox cbPMenu = Core_AMS.Utilities.WPF.FindControl<RadComboBox>(rdForm, "cbPMenu");

            cbApp.IsEnabled = true;
            cbSF.IsEnabled = true;
            cbPMenu.IsEnabled = true;
        }
    }

    [Serializable]
    [DataContract]
    public class MenuContainer
    {
        public KMPlatform.Entity.Menu ToMenu ()
        {
            KMPlatform.Entity.Menu newMenu = new KMPlatform.Entity.Menu();

            newMenu.ApplicationID = this.ApplicationID;
            newMenu.CreatedByUserID = this.CreatedByUserID;
            newMenu.DateCreated = this.DateCreated;
            newMenu.DateUpdated = this.DateUpdated;
            newMenu.Description = this.Description;
            newMenu.HasFeatures = this.HasFeatures;
            newMenu.ImagePath = this.ImagePath;
            newMenu.IsActive = this.IsActive;
            newMenu.IsParent = this.IsParent;
            newMenu.IsServiceFeature = this.IsServiceFeature;
            //newMenu.MenuFeatures = this.MenuFeatures;
            newMenu.MenuID = this.MenuID;
            newMenu.MenuName = this.MenuName;
            newMenu.MenuOrder = this.MenuOrder;
            newMenu.ParentMenuID = this.ParentMenuID;
            newMenu.ServiceFeatureID = this.ServiceFeatureID;
            newMenu.UpdatedByUserID = this.UpdatedByUserID;
            newMenu.URL = this.URL;

            return newMenu;
        }
        public MenuContainer() { }
        public MenuContainer(KMPlatform.Entity.Menu menu)
        {

            ApplicationID = menu.ApplicationID; 
            CreatedByUserID = menu.CreatedByUserID;
            DateCreated = menu.DateCreated;
            DateUpdated = menu.DateUpdated;
            Description = menu.Description;
            HasFeatures = menu.HasFeatures;
            ImagePath = menu.ImagePath;
            IsActive = menu.IsActive;
            IsParent = menu.IsParent;
            IsServiceFeature = menu.IsServiceFeature;
            //MenuFeatures = menu.MenuFeatures;
            MenuID = menu.MenuID;
            MenuName = menu.MenuName;
            MenuOrder = menu.MenuOrder;
            ParentMenuID = menu.ParentMenuID;
            ServiceFeatureID = menu.ServiceFeatureID; 
            UpdatedByUserID = menu.UpdatedByUserID;
            URL = menu.URL;
            
            ApplicationName = string.Empty;
            CreatedByName = string.Empty;
            ParentMenuName = string.Empty;
            ServiceFeatureName = string.Empty; 
            UpdatedByName = string.Empty;
        }
        #region Properties
        [DataMember]
        public int ApplicationID { get; set; }

        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool HasFeatures { get; set; }
        [DataMember]
        public string ImagePath { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public bool IsParent { get; set; }
        [DataMember]
        public bool IsServiceFeature { get; set; }
        //[DataMember]
        //public List<KMPlatform.Entity.MenuFeature> MenuFeatures { get; set; }
        [DataMember]
        public int MenuID { get; set; }
        [DataMember]
        public string MenuName { get; set; }
        [DataMember]
        public int MenuOrder { get; set; }
        [DataMember]
        public int ParentMenuID { get; set; }
        [DataMember]
        public int ServiceFeatureID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        [DataMember]
        public string URL{ get; set; }

        [DataMember]
        public string ApplicationName { get; set; }
        [DataMember]
        public string CreatedByName { get; set; }
        [DataMember]
        public string ParentMenuName { get; set; }
        [DataMember]
        public string ServiceFeatureName { get; set; }
        [DataMember]
        public string UpdatedByName { get; set; }
        #endregion
    }
}
