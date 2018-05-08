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
    /// Interaction logic for ServiceManagement.xaml
    /// </summary>
    public partial class ServiceManagement : UserControl
    {
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IService> serviceWorker { get; set; }
        private List<KMPlatform.Entity.User> kmUsers { get; set; }
        public List<KMPlatform.Entity.Application> applications { get; set; }
        public List<KMPlatform.Entity.Service> services { get; set; }
        public ServiceManagement()
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
            sort.Member = "ServiceName";
            sort.SortDirection = ListSortDirection.Ascending;
            grdService.SortDescriptors.Add(sort);
            serviceWorker = FrameworkServices.ServiceClient.UAS_ServiceClient();
            services = serviceWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
            grdService.ItemsSource = services;

            KMPlatform.Entity.ClientGroup cgKM = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.ClientGroups.SingleOrDefault(x => x.ClientGroupName.Equals("Knowledge Marketing"));

            FrameworkServices.ServiceClient<UAS_WS.Interface.IUser> userWorker = FrameworkServices.ServiceClient.UAS_UserClient();
            kmUsers = userWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, cgKM.ClientGroupID, false).Result;

            FrameworkServices.ServiceClient<UAS_WS.Interface.IApplication> appWorker = FrameworkServices.ServiceClient.UAS_ApplicationClient();
            applications = appWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
        }
        private void lstEnabled_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grdService != null && grdService.Columns.Count > 0)
            {
                Telerik.Windows.Controls.GridViewColumn isEnabledColumn = grdService.Columns["IsEnabled"];

                string isEnabled = lstEnabled.SelectedItem.ToString().Replace("System.Windows.Controls.ListBoxItem: ", "");
                //All, Enabled, Not Enabled

                if (!isEnabled.Equals("All"))
                {
                    if (isEnabled.Equals("Enabled")) isEnabled = "true";
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
                    grdService.FilterDescriptors.SuspendNotifications();
                    isEnabledColumn.ClearFilters();
                    grdService.FilterDescriptors.ResumeNotifications();
                }
            }

        }
        private void rbCancel_Click(object sender, RoutedEventArgs e)
        {
            grdService.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
            this.grdService.SelectedItem = null;
        }
        private void grdService_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (this.grdService.SelectedItem != null)
            {
                grdService.ScrollIntoView(this.grdService.SelectedItem);
                grdService.UpdateLayout();
                grdService.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.VisibleWhenSelected;
            }
        }
        private void rdForm_EditEnded(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndedEventArgs e)
        {
            if (e.EditAction == Telerik.Windows.Controls.Data.DataForm.EditAction.Cancel)
                grdService.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
            else
            {
                //EditAction == Commit
                //save changes
                KMPlatform.Entity.Service myServ = (KMPlatform.Entity.Service)grdService.SelectedItem;
                #region Check Values
                List<KMPlatform.Entity.Service> servList = serviceWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
                if (string.IsNullOrEmpty(myServ.ServiceName))
                {
                    Core_AMS.Utilities.WPF.MessageError("Current service name cannot be blank.");
                    grdService.SelectedItem = servList.FirstOrDefault(x => x.ServiceID == myServ.ServiceID);
                    grdService.Rebind();
                    return;
                }
                if (string.IsNullOrEmpty(myServ.ServiceCode))
                {
                    Core_AMS.Utilities.WPF.MessageError("Current service code cannot be blank.");
                    grdService.SelectedItem = servList.FirstOrDefault(x => x.ServiceID == myServ.ServiceID);
                    grdService.Rebind();
                    return;
                }                
                if (servList != null && servList.FirstOrDefault(x => x.ServiceID == myServ.ServiceID).ServiceName != myServ.ServiceName)
                {                                        
                    if (servList.FirstOrDefault(x => x.ServiceName.Equals(myServ.ServiceName, StringComparison.CurrentCultureIgnoreCase) && x.ServiceID != myServ.ServiceID) != null)
                    {
                        Core_AMS.Utilities.WPF.MessageError("Current service name has been used. Please provide a unique service name.");
                        grdService.SelectedItem = servList.FirstOrDefault(x => x.ServiceID == myServ.ServiceID);
                        grdService.Rebind();
                        return;
                    }                    
                }
                if (servList != null && servList.FirstOrDefault(x => x.ServiceID == myServ.ServiceID).ServiceCode != myServ.ServiceCode)
                {       
                    if (servList.FirstOrDefault(x => x.ServiceCode.Equals(myServ.ServiceCode, StringComparison.CurrentCultureIgnoreCase) && x.ServiceID != myServ.ServiceID) != null)
                    {
                        Core_AMS.Utilities.WPF.MessageError("Current service code has been used. Please provide a unique service code.");
                        grdService.SelectedItem = servList.FirstOrDefault(x => x.ServiceID == myServ.ServiceID);
                        grdService.Rebind();
                        return;
                    }                    
                }
                #endregion
                serviceWorker = FrameworkServices.ServiceClient.UAS_ServiceClient();
                myServ.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                serviceWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myServ);

                grdService.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
            }
            this.grdService.SelectedItem = null;
        }
        private void grdService_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if (e.Row is GridViewRow && !(e.Row is GridViewNewRow) && kmUsers != null)
            {
                KMPlatform.Entity.Service service = e.DataElement as KMPlatform.Entity.Service;
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

            KMPlatform.Entity.Service myServ = (KMPlatform.Entity.Service)rdForm.DataContext;

            DataFormComboBoxField cbEditApplications = Core_AMS.Utilities.WPF.FindChild<DataFormComboBoxField>(rdForm, "cbEditApplications");

            if (cbEditApplications != null)
            {
                cbEditApplications.ItemsSource = applications;
                KMPlatform.Entity.Application myApp = applications.Single(x => x.ApplicationID == myServ.DefaultApplicationID);
                cbEditApplications.SelectedIndex = applications.IndexOf(myApp);
            }
        }


        private void rbServiceFeatures_Click(object sender, RoutedEventArgs e)
        {            
            if (this.Parent.GetType() == typeof(StackPanel))
            {
                if (this.grdService.SelectedItem != null)
                {
                    KMPlatform.Entity.Service s = (KMPlatform.Entity.Service)this.grdService.SelectedItem;
                    StackPanel sp = (StackPanel)this.Parent;
                    sp.Children.Clear();
                    ControlCenter.Modules.ServiceFeatureManagement sfm = new ServiceFeatureManagement(s);
                    sp.Children.Add(sfm);
                }
                else
                {
                    StackPanel sp = (StackPanel)this.Parent;
                    sp.Children.Clear();
                    ControlCenter.Modules.ServiceFeatureManagement sfm = new ServiceFeatureManagement();
                    sp.Children.Add(sfm);
                }
            }
            else
                Core_AMS.Utilities.WPF.MessageError("An unexpected error occurred during when loading service features. Please try again. If the problem persists, please contact Customer Support.");

        }
        private void rbNewService_Click(object sender, RoutedEventArgs e)
        {
            this.Background = System.Windows.Media.Brushes.DarkGray;
            lstEnabled.IsEnabled = false;
            rbNewService.IsEnabled = false;
            grdService.IsEnabled = false;
            grdService.Background = System.Windows.Media.Brushes.DarkGray;
            rwNew.Visibility = System.Windows.Visibility.Visible;
            //rwNew.ShowDialog();
        }
        private void grdService_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
            KMPlatform.Entity.Service myServ = (KMPlatform.Entity.Service)e.NewObject;
        }
        #region Add new service
        private void cbIsAdditionalCost_Checked(object sender, RoutedEventArgs e)
        {
            if(cbIsAdditionalCost.IsChecked == true) 
                spRateDuration.Visibility= System.Windows.Visibility.Visible;
            else
                spRateDuration.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void cbIsAdditionalCost_UnChecked(object sender, RoutedEventArgs e)
        {
            if(cbIsAdditionalCost.IsChecked == true) 
                spRateDuration.Visibility= System.Windows.Visibility.Visible;
            else
                spRateDuration.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void ResetNewWindow()
        {
            tbServiceName.Clear();
            tbDescription.Clear();
            tbCode.Clear();
            nudOrder.Value = 1;
            cbIsEnabled.IsChecked = false;
            cbIsAdditionalCost.IsChecked = false;
            nudDefaultRate.Value = 0;
            nudDefaultDurationInMonths.Value = 0;            
            cbDefaultApplicationID.SelectedIndex = -1;
            spRateDuration.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void btnNewSave_Click(object sender, RoutedEventArgs e)
        {
            KMPlatform.Entity.Service newService = new KMPlatform.Entity.Service();

            newService.ServiceName = Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(tbServiceName.Text.Trim());
            newService.Description = Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(tbDescription.Text.Trim());
            newService.ServiceCode = Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(tbCode.Text.Trim());

            #region Check Values
            if (string.IsNullOrEmpty(newService.ServiceName))
            {
                Core_AMS.Utilities.WPF.MessageError("Must provide a service name.");
                return;
            }
            else
            {
                List<KMPlatform.Entity.Service> servList = serviceWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
                if (servList.FirstOrDefault(x => x.ServiceName.Equals(newService.ServiceName, StringComparison.CurrentCultureIgnoreCase)) != null)
                {
                    Core_AMS.Utilities.WPF.MessageError("Must provide a unique service name that hasn't been used.");
                    return;
                }
            }

            if (string.IsNullOrEmpty(newService.ServiceCode))
            {
                Core_AMS.Utilities.WPF.MessageError("Must provide a service code.");
                return;
            }
            else
            {
                List<KMPlatform.Entity.Service> servList = serviceWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
                if (servList.FirstOrDefault(x => x.ServiceCode.Equals(newService.ServiceCode, StringComparison.CurrentCultureIgnoreCase)) != null)
                {
                    Core_AMS.Utilities.WPF.MessageError("Must provide a unique service code that hasn't been used.");
                    return;
                }
            }
            #endregion

            newService.DisplayOrder = (int)nudOrder.Value;
            newService.IsEnabled = cbIsEnabled.IsChecked.Value;
            newService.IsAdditionalCost = cbIsAdditionalCost.IsChecked.Value;
            newService.HasFeatures = false;
            newService.DefaultRate = (decimal)nudDefaultRate.Value;
            newService.DefaultDurationInMonths = (int)nudDefaultDurationInMonths.Value;
            newService.DefaultApplicationID = (int)cbDefaultApplicationID.SelectedValue;
            newService.DateCreated = DateTime.Now;
            newService.CreatedByUserID = AppData.myAppData.AuthorizedUser.User.UserID;

            newService.ServiceID = serviceWorker.Proxy.Save(AppData.myAppData.AuthorizedUser.AuthAccessKey, newService).Result;
            if (newService.ServiceID > 0)
            {
                ResetNewWindow();

                List<KMPlatform.Entity.Service> services = (List<KMPlatform.Entity.Service>)grdService.ItemsSource;
                services.Add(newService);
                grdService.ItemsSource = null;
                grdService.ItemsSource = services;

                CloseWindow();
                this.grdService.SelectedItem = null;
            }
            else
                Core_AMS.Utilities.WPF.MessageServiceError();
            
        }
        private void CloseWindow()
        {
            this.Background = System.Windows.Media.Brushes.Transparent;
            lstEnabled.IsEnabled = true;
            rbNewService.IsEnabled = true;
            grdService.IsEnabled = true;
            grdService.Background = System.Windows.Media.Brushes.Transparent;

            rwNew.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void btnNewCancel_Click(object sender, RoutedEventArgs e)
        {
            ResetNewWindow();
            List<KMPlatform.Entity.Service> services = (List<KMPlatform.Entity.Service>)grdService.ItemsSource;
            grdService.ItemsSource = services;
            CloseWindow();
            this.grdService.SelectedItem = null;
        }
        #endregion

       
    }
}
