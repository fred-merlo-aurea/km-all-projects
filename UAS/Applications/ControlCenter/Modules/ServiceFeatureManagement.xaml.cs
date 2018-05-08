using FrameworkUAS.Object;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Data;

namespace ControlCenter.Modules
{
    /// <summary>
    /// Interaction logic for ServiceFeatureManagement.xaml
    /// </summary>
    public partial class ServiceFeatureManagement : UserControl
    {
        #region Page Variables
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IServiceFeature> serviceWorker { get; set; }
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IService> servWorker { get; set; }
        private List<KMPlatform.Entity.User> kmUsers { get; set; }               
        public List<KMPlatform.Entity.Service> services { get; set; }
        public KMPlatform.Entity.Service currentService { get; set; }

        public List<ServiceFeatureContainer> SFCList = new List<ServiceFeatureContainer>();

        FrameworkUAS.Service.Response<List<KMPlatform.Entity.ServiceFeature>> sfResponse = new FrameworkUAS.Service.Response<List<KMPlatform.Entity.ServiceFeature>>();


        #endregion

        public ServiceFeatureManagement(KMPlatform.Entity.Service myService = null)
        {
            Window parentWindow = Application.Current.MainWindow;
            if (AppData.CheckParentWindowUid(parentWindow.Uid))
            {
                //only want this available to users that belong to KM
                if (AppData.IsKmUser() == true)
                {
                    InitializeComponent();
                    if (myService != null)
                        currentService = myService;

                    LoadData();
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageAccessDenied();
                }
            }
        }
        private void LoadData()
        {
            #region Service
            KMPlatform.Entity.ClientGroup cgKM = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.ClientGroups.SingleOrDefault(x => x.ClientGroupName.Equals("Knowledge Marketing"));

            FrameworkServices.ServiceClient<UAS_WS.Interface.IUser> userWorker = FrameworkServices.ServiceClient.UAS_UserClient();
            kmUsers = userWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, cgKM.ClientGroupID, false).Result;

            FrameworkServices.ServiceClient<UAS_WS.Interface.IService> sWorker = FrameworkServices.ServiceClient.UAS_ServiceClient();
            services = sWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
            if (currentService != null)
                cbServiceID.ItemsSource = services.Where(x => x.ServiceID == currentService.ServiceID).ToList();
            else
                cbServiceID.ItemsSource = services;

            cbServiceID.DisplayMemberPath = "ServiceName";
            cbServiceID.SelectedValuePath = "ServiceID";
            #endregion
            #region Grid Sort
            //startUp = false;
            SortDescriptor sort = new SortDescriptor();
            sort.Member = "ServiceID";
            sort.SortDirection = ListSortDirection.Ascending;
            #endregion
            #region ServiceFeature and Grid
            grdServiceFeature.SortDescriptors.Add(sort);
            serviceWorker = FrameworkServices.ServiceClient.UAS_ServiceFeatureClient();                        
            sfResponse = serviceWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);

            foreach (KMPlatform.Entity.ServiceFeature sf in sfResponse.Result)
            {
                ServiceFeatureContainer sfc = new ServiceFeatureContainer(sf);

                //KMPlatform.Entity.User user = kmUsers.FirstOrDefault(x => x.UserID == campaign.AddedBy);
                KMPlatform.Entity.Service serv = services.FirstOrDefault(x => x.ServiceID == sfc.ServiceID);
                if (serv != null)
                    sfc.ServiceName = serv.ServiceName;

                SFCList.Add(sfc);
            }
            grdServiceFeature.ItemsSource = SFCList;
            int i = 0;
            //if (sfResponse.Result != null && sfResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            //{
            //    if (currentService != null)
            //        grdServiceFeature.ItemsSource = sfResponse.Result.Where(x => x.ServiceID == currentService.ServiceID).ToList();
            //    else
            //        grdServiceFeature.ItemsSource = sfResponse.Result;

            //}
            //else
            //    Core_AMS.Utilities.WPF.MessageError("An unexpected error occured during a service request, please try again.  If the problem persists pelase contact Customer Support.");
            #endregion

        }
        private void rbCancel_Click(object sender, RoutedEventArgs e)
        {
            grdServiceFeature.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
            this.grdServiceFeature.SelectedItem = null;
        }
        private void lstEnabled_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grdServiceFeature != null && grdServiceFeature.Columns.Count > 0)
            {
                Telerik.Windows.Controls.GridViewColumn isEnabledColumn = grdServiceFeature.Columns["IsEnabled"];

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

                    isEnabledFilter.ResumeNotifications();
                }
                else
                {
                    grdServiceFeature.FilterDescriptors.SuspendNotifications();
                    isEnabledColumn.ClearFilters();
                    grdServiceFeature.FilterDescriptors.ResumeNotifications();
                }
            }

        }

        #region Grid
        private void grdServiceFeatureSelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            grdServiceFeature.ScrollIntoView(this.grdServiceFeature.SelectedItem);
            grdServiceFeature.UpdateLayout();
            grdServiceFeature.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.VisibleWhenSelected;
        }
        private void rdForm_EditEnded(object sender, Telerik.Windows.Controls.Data.DataForm.EditEndedEventArgs e)
        {
            if (e.EditAction == Telerik.Windows.Controls.Data.DataForm.EditAction.Cancel)
                grdServiceFeature.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
            else
            {
                //EditAction == Commit
                //save changes
                KMPlatform.Entity.ServiceFeature myServ = (KMPlatform.Entity.ServiceFeature)grdServiceFeature.SelectedItem;
                #region Check Values
                sfResponse = serviceWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
                List<KMPlatform.Entity.ServiceFeature> sfList = sfResponse.Result.Where(x => x.ServiceFeatureID == myServ.ServiceFeatureID).ToList();
                if (string.IsNullOrEmpty(myServ.SFName))
                {
                    Core_AMS.Utilities.WPF.MessageError("Current service feature name cannot be blank.");
                    grdServiceFeature.SelectedItem = sfList.FirstOrDefault(x => x.ServiceFeatureID == myServ.ServiceFeatureID);
                    grdServiceFeature.Rebind();
                    return;
                }
                if (string.IsNullOrEmpty(myServ.SFCode))
                {
                    Core_AMS.Utilities.WPF.MessageError("Current service feature code cannot be blank.");
                    grdServiceFeature.SelectedItem = sfList.FirstOrDefault(x => x.ServiceFeatureID == myServ.ServiceFeatureID);
                    grdServiceFeature.Rebind();
                    return;
                }                
                if (sfList != null && sfList.FirstOrDefault(x => x.ServiceFeatureID == myServ.ServiceFeatureID).SFName != myServ.SFName)
                {                                        
                    if (sfList.FirstOrDefault(x => x.SFName.Equals(myServ.SFName, StringComparison.CurrentCultureIgnoreCase) && x.ServiceFeatureID != myServ.ServiceFeatureID) != null)
                    {
                        Core_AMS.Utilities.WPF.MessageError("Current service feature name has been used. Please provide a unique service feature name.");
                        grdServiceFeature.SelectedItem = sfList.FirstOrDefault(x => x.ServiceFeatureID == myServ.ServiceFeatureID);
                        grdServiceFeature.Rebind();
                        return;
                    }                    
                }
                if (sfList != null && sfList.FirstOrDefault(x => x.ServiceFeatureID == myServ.ServiceFeatureID).SFCode != myServ.SFCode)
                {       
                    if (sfList.FirstOrDefault(x =>  x.SFCode.Equals(myServ.SFCode, StringComparison.CurrentCultureIgnoreCase) && x.ServiceFeatureID != myServ.ServiceFeatureID) != null)
                    {
                        Core_AMS.Utilities.WPF.MessageError("Current service feature code has been used. Please provide a unique service feature code.");
                        grdServiceFeature.SelectedItem = sfList.FirstOrDefault(x => x.ServiceFeatureID == myServ.ServiceFeatureID);
                        grdServiceFeature.Rebind();
                        return;
                    }                    
                }
                #endregion
                serviceWorker = FrameworkServices.ServiceClient.UAS_ServiceFeatureClient();
                myServ.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                serviceWorker.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myServ);

                grdServiceFeature.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.Collapsed;
            }
            this.grdServiceFeature.SelectedItem = null;
        }
        private void grdServiceFeature_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if (e.Row is GridViewRow && !(e.Row is GridViewNewRow) && kmUsers != null)
           {
                ServiceFeatureContainer service = e.DataElement as ServiceFeatureContainer;
                KMPlatform.Entity.User user = null;
                if (kmUsers.Exists(x => x.UserID == service.CreatedByUserID))
                    user = kmUsers.Single(x => x.UserID == service.CreatedByUserID);

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
            //RadDataForm rdForm = sender as RadDataForm;

            //KMPlatform.Entity.ServiceFeature myServ = (KMPlatform.Entity.ServiceFeature)rdForm.DataContext;

            //DataFormComboBoxField cbServiceID = Core_AMS.Utilities.WPF.FindChild<DataFormComboBoxField>(rdForm, "cbEditApplications");

            //if (cbEditApplications != null)
            //{
            //    cbEditApplications.ItemsSource = applications;
            //    KMPlatform.Entity.Application myApp = applications.Single(x => x.ApplicationID == myServ.DefaultApplicationID);
            //    cbEditApplications.SelectedIndex = applications.IndexOf(myApp);
            //}
        }

        private void grdServiceFeature_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (this.grdServiceFeature.SelectedItem != null)
            {
                grdServiceFeature.ScrollIntoView(this.grdServiceFeature.SelectedItem);
                grdServiceFeature.UpdateLayout();
                grdServiceFeature.RowDetailsVisibilityMode = Telerik.Windows.Controls.GridView.GridViewRowDetailsVisibilityMode.VisibleWhenSelected;
            }
        }
        private void grdServiceFeature_AddingNewDataItem(object sender, Telerik.Windows.Controls.GridView.GridViewAddingNewEventArgs e)
        {
            KMPlatform.Entity.Service myServ = (KMPlatform.Entity.Service)e.NewObject;
        }
        #endregion
        private void rbNewServiceFeature_Click(object sender, RoutedEventArgs e)
        {
            this.Background = System.Windows.Media.Brushes.DarkGray;
            lstEnabled.IsEnabled = false;
            rbNewServiceFeature.IsEnabled = false;
            grdServiceFeature.IsEnabled = false;
            grdServiceFeature.Background = System.Windows.Media.Brushes.DarkGray;
            rwNew.Visibility = System.Windows.Visibility.Visible;
        }

        #region Add new service
        private void cbIsAdditionalCost_Checked(object sender, RoutedEventArgs e)
        {
            if (cbIsAdditionalCost.IsChecked == true)
                spRateDuration.Visibility = System.Windows.Visibility.Visible;
            else
                spRateDuration.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void cbIsAdditionalCost_Unchecked(object sender, RoutedEventArgs e)
        {
            if (cbIsAdditionalCost.IsChecked == true)
                spRateDuration.Visibility = System.Windows.Visibility.Visible;
            else
                spRateDuration.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void btnNewSave_Click(object sender, RoutedEventArgs e)
        {
            if (cbServiceID.SelectedValue != null)
            {
                KMPlatform.Entity.ServiceFeature newService = new KMPlatform.Entity.ServiceFeature();
                int serviceId = 0;
                int.TryParse(cbServiceID.SelectedValue.ToString(), out serviceId);
                newService.SFName = Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(tbServiceName.Text.Trim());
                newService.Description = Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(tbDescription.Text.Trim());
                newService.SFCode = Core_AMS.Utilities.StringFunctions.CleanStringSqlInjection(tbCode.Text.Trim());

                #region Check Values
                if (string.IsNullOrEmpty(newService.SFName))
                {
                    Core_AMS.Utilities.WPF.MessageError("Must provide a service feature name.");
                    return;
                }
                else
                {
                    sfResponse = serviceWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
                    List<KMPlatform.Entity.ServiceFeature> sfList = sfResponse.Result.Where(x => x.ServiceID == serviceId).ToList();
                    if (sfList != null && sfList.FirstOrDefault(x => x.ServiceID == serviceId && x.SFName.Equals(newService.SFName, StringComparison.CurrentCultureIgnoreCase)) != null)
                    {
                        Core_AMS.Utilities.WPF.MessageError("Must provide a unique service feature name that hasn't been used for the selected service.");
                        return;
                    }
                }

                if (string.IsNullOrEmpty(newService.SFCode))
                {
                    Core_AMS.Utilities.WPF.MessageError("Must provide a service feature code.");
                    return;
                }
                else
                {
                    sfResponse = serviceWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey);
                    List<KMPlatform.Entity.ServiceFeature> sfList = sfResponse.Result.Where(x => x.ServiceID == serviceId).ToList();
                    if (sfList != null && sfList.FirstOrDefault(x => x.ServiceID == serviceId && x.SFCode.Equals(newService.SFCode, StringComparison.CurrentCultureIgnoreCase)) != null)
                    {
                        Core_AMS.Utilities.WPF.MessageError("Must provide a unique service feature code that hasn't been used for the selected service.");
                        return;
                    }
                }
                #endregion

                newService.DisplayOrder = (int)nudOrder.Value;
                newService.IsEnabled = cbIsEnabled.IsChecked.Value;
                newService.IsAdditionalCost = cbIsAdditionalCost.IsChecked.Value;
                newService.KMAdminOnly = cbKMAdminOnly.IsChecked.Value;
                newService.DefaultRate = (decimal)nudDefaultRate.Value;
                newService.DefaultDurationInMonths = (int)nudDefaultDurationInMonths.Value;
                newService.DateCreated = DateTime.Now;
                newService.CreatedByUserID = AppData.myAppData.AuthorizedUser.User.UserID;                
                newService.ServiceID = serviceId;

                currentService = services.FirstOrDefault(x => x.ServiceID == newService.ServiceID);

                FrameworkUAS.Service.Response<int> resp = serviceWorker.Proxy.SaveReturnId(AppData.myAppData.AuthorizedUser.AuthAccessKey, newService);
                if (resp.Result != null)
                {
                    newService.ServiceFeatureID = resp.Result;
                    #region Save Service if has service features
                    if (currentService.HasFeatures == false)
                    {
                        currentService.HasFeatures = true;
                        currentService.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                        currentService.DateUpdated = DateTime.Now;
                        servWorker = FrameworkServices.ServiceClient.UAS_ServiceClient();   
                        currentService.ServiceID = servWorker.Proxy.Save(AppData.myAppData.AuthorizedUser.AuthAccessKey, currentService).Result;
                    }
                    #endregion

                    List<KMPlatform.Entity.ServiceFeature> serviceFeatures = (List<KMPlatform.Entity.ServiceFeature>)grdServiceFeature.ItemsSource;
                    serviceFeatures.Add(newService);

                    grdServiceFeature.ItemsSource = null;
                    grdServiceFeature.ItemsSource = serviceFeatures;
                }

                ResetNewWindow();
                CloseWindow();               
            }
            else
                Core_AMS.Utilities.WPF.MessageError("Please select a Service.");
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
            cbKMAdminOnly.IsChecked = false;
        }
        private void CloseWindow()
        {
            this.Background = System.Windows.Media.Brushes.Transparent;
            lstEnabled.IsEnabled = true;
            rbNewServiceFeature.IsEnabled = true;
            grdServiceFeature.IsEnabled = true;
            grdServiceFeature.Background = System.Windows.Media.Brushes.Transparent;

            rwNew.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void btnNewCancel_Click(object sender, RoutedEventArgs e)
        {
            ResetNewWindow();
            List<KMPlatform.Entity.ServiceFeature> services = (List<KMPlatform.Entity.ServiceFeature>)grdServiceFeature.ItemsSource;
            grdServiceFeature.ItemsSource = services;
            CloseWindow();
        }
        #endregion
    }
}
    [Serializable]
    [DataContract]
    public class ServiceFeatureContainer
    {
        public ServiceFeatureContainer() { }
        public ServiceFeatureContainer(KMPlatform.Entity.ServiceFeature ServiceFeature)
        {
            //int
            CreatedByUserID = ServiceFeature.CreatedByUserID;
            DefaultDurationInMonths = ServiceFeature.DefaultDurationInMonths;
            DisplayOrder = ServiceFeature.DisplayOrder;
            ServiceFeatureID = ServiceFeature.ServiceFeatureID;
            ServiceID = ServiceFeature.ServiceID;
            UpdatedByUserID = ServiceFeature.UpdatedByUserID;

            //string
            Description = ServiceFeature.Description;
            ServiceName = string.Empty;
            SFCode = ServiceFeature.SFCode;
            SFName = ServiceFeature.SFName;

            //bool
            IsAdditionalCost = ServiceFeature.IsAdditionalCost;
            IsEnabled = ServiceFeature.IsEnabled;
            KMAdminOnly = ServiceFeature.KMAdminOnly;

            //decimal
            DefaultRate = ServiceFeature.DefaultRate;

            //datetime
            DateCreated = ServiceFeature.DateCreated;
            DateUpdated = ServiceFeature.DateUpdated;
        }
        #region Properties
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int ServiceFeatureID { get; set; }
        [DataMember]
        public int ServiceID { get; set; }
        [DataMember]
        public int DisplayOrder { get; set; }
        [DataMember]
        public int DefaultDurationInMonths { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
       
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string SFCode { get; set; }
        [DataMember]
        public string SFName { get; set; }
        [DataMember]
        public String ServiceName { get; set; }


        [DataMember]
        public bool IsEnabled { get; set; }
        [DataMember]
        public bool IsAdditionalCost { get; set; }
        [DataMember]
        public bool KMAdminOnly { get; set; }

        [DataMember]
        public decimal DefaultRate { get; set; }

        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        #endregion  
}
