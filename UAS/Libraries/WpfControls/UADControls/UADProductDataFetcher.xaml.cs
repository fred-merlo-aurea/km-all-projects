using FrameworkUAS.Entity;
using FrameworkUAD.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;
using WpfControls.Helpers;

namespace WpfControls.UADControls
{
    /// <summary>
    /// Interaction logic for UADProductDataFetcher.xaml
    /// </summary>
    public partial class UADProductDataFetcher : UserControl
    {
        #region ServiceCalls
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.IAction> actionData = FrameworkServices.ServiceClient.UAD_Lookup_ActionClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICategoryCode> catCodeData = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> clientData = FrameworkServices.ServiceClient.UAS_ClientClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeData = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICodeType> codeTypeData = FrameworkServices.ServiceClient.UAD_Lookup_CodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IFilter> filterData = FrameworkServices.ServiceClient.UAD_FilterClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IFilterDetail> filterDetailData = FrameworkServices.ServiceClient.UAS_FilterDetailClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IFilterDetailSelectedValue> filterDetailValuesData = FrameworkServices.ServiceClient.UAS_FilterDetailSelectedValueClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> productData = FrameworkServices.ServiceClient.UAD_ProductClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IReports> reportData = FrameworkServices.ServiceClient.UAD_ReportsClient();
        //private FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscriptionResponseMap> subResponseMapData = FrameworkServices.ServiceClient.UAD_SubscriptionResponseMapClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ITransactionCode> transCodeData = FrameworkServices.ServiceClient.UAD_Lookup_TransactionCodeClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscriptionDetail> subsDetailData = FrameworkServices.ServiceClient.UAD_SubscriptionDetailClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscription> subscriptionData = FrameworkServices.ServiceClient.UAD_SubscriptionClient();
        #endregion
        #region ServiceResponse
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Action>> actionResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Action>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>> catCodeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>>();
        private FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>> clientResponse = new FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> codeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CodeType>> codeTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CodeType>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Filter>> filterResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Filter>>();
        private FrameworkUAS.Service.Response<List<FilterDetail>> filterDetailResponse = new FrameworkUAS.Service.Response<List<FilterDetail>>();
        private FrameworkUAS.Service.Response<List<FilterDetailSelectedValue>> filterDetailSelectedValuedResponse = new FrameworkUAS.Service.Response<List<FilterDetailSelectedValue>>();
        private FrameworkUAS.Service.Response<int> intResponse = new FrameworkUAS.Service.Response<int>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>> productResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Product>>();
        private FrameworkUAS.Service.Response<List<int>> subCountResponse = new FrameworkUAS.Service.Response<List<int>>();
        //private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.SubscriptionResponseMap>> subResponseMapResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.SubscriptionResponseMap>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>> transCodeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Subscription>> subscriptionResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Subscription>>();
        #endregion
        #region Variables/Lists
        private Guid accessKey;
        private RadBusyIndicator busy;
        private KMPlatform.Entity.Client myClient = new KMPlatform.Entity.Client();
        private FrameworkUAD.Entity.Product myProduct = new FrameworkUAD.Entity.Product();
        //private Publication myPublication = new Publication();
        private FilterOperations fo = new FilterOperations();

        public ObservableCollection<Helpers.Common.FilterCriteria> filterCriteria = new ObservableCollection<Helpers.Common.FilterCriteria>();

        private List<FrameworkUAD.Entity.Subscription> subscriptions = new List<FrameworkUAD.Entity.Subscription>();
        private List<int> subscriptionIDs = new List<int>();
        //private List<FrameworkUAD.Entity.SubscriptionResponseMap> subscriptionResponseList = new List<FrameworkUAD.Entity.SubscriptionResponseMap>();
        private List<FrameworkUAD.Entity.Filter> filterList = new List<FrameworkUAD.Entity.Filter>();

        private List<FrameworkUAD_Lookup.Entity.Action> actionList = new List<FrameworkUAD_Lookup.Entity.Action>();
        private HashSet<int> activeTransactionCodes = new HashSet<int>();
        private HashSet<int> activePaidTransactionCodes = new HashSet<int>();
        private HashSet<int> activeActions = new HashSet<int>();
        private HashSet<int> activePaidActions = new HashSet<int>();
        private HashSet<int> activeCats = new HashSet<int>();
        private HashSet<int> activePaidCats = new HashSet<int>();
        private HashSet<int> activeSubscriptionIDs = new HashSet<int>();
        private HashSet<int> activeSubscriberIDs = new HashSet<int>();

        //private int myProductID = -1;
        //private int productID = -1;
        //private int publicationID = -1;
        //private Publication publication = new Publication();
        private int groupID = -1;
        //private int tempSubID = -1;
        #endregion

        public UADProductDataFetcher()
        {
            InitializeComponent();

            accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
            grdFilterInfo.ItemsSource = filterCriteria;
        }

        #region Load
        public void LoadData()
        {
            myClient = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient;

            codeResponse = codeData.Proxy.Select(accessKey, FrameworkUAD_Lookup.Enums.CodeType.Filter_Group);
            if (Helpers.Common.CheckResponse(codeResponse.Result, codeResponse.Status))
            {
                FrameworkUAD_Lookup.Entity.Code code = codeResponse.Result.Where(x => x.CodeName.Replace(" ", "").Trim() == FrameworkUAD_Lookup.Enums.FilterGroupTypes.Issue_Splits.ToString().Replace("_", "").Trim()).FirstOrDefault();
                groupID = code.DisplayOrder;
            }

            actionResponse = actionData.Proxy.Select(accessKey);
            if (Common.CheckResponse(actionResponse.Result, actionResponse.Status))
                actionList = actionResponse.Result;

            //filterResponse = filterData.Proxy.Select(accessKey, myClient.ClientConnections);
            //if (Common.CheckResponse(filterResponse.Result, filterResponse.Status))
            //    filterList = filterResponse.Result.Where(x => x.IsDeleted == false).ToList();

            //subscriptionIDs = subscriptionData.Proxy.SelectIDs(accessKey, myClient).Result;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UserControl uc = this.ParentOfType<UserControl>();
            busy = uc.FindChildByType<RadBusyIndicator>();
        }
        #endregion

        #region UI Events
        private void btnAddCriteria_Click(object sender, RoutedEventArgs e)
        {
            Helpers.FilterOperations.FilterContainer fc = new Helpers.FilterOperations.FilterContainer();
            ObservableCollection<Helpers.FilterOperations.DisplayedFilterDetail> details = new ObservableCollection<FilterOperations.DisplayedFilterDetail>();
            Helpers.FilterOperations fo = new Helpers.FilterOperations();
            IssueSplit split = new IssueSplit();
            fc.Filter.IsActive = true;
            fc.Filter.FilterGroupID = 5; //TEMP WAY TO TRACK DIFFERENCE BETWEEN 1.Regular Filters, 2.Add Kill Filters, 3.Split Filters

            Window w = Window.GetWindow(this);
            int tmpProductID = 0;
            DynamicUADProductDemographics dynProd = w.FindChildByType<DynamicUADProductDemographics>();
            tmpProductID = dynProd.ProductID;
            StandardUADDemographics filters = w.FindChildByType<StandardUADDemographics>();
            fc.Filter.ProductId = tmpProductID;
            UADControls.ExtraFiltersTabControl extraFilters = w.FindChildByType<ExtraFiltersTabControl>();
            AdHocUADFilters adHocFilters = extraFilters.adhocs;
            ActivityFilter activities = extraFilters.activities;

            fc.FilterDetails.AddRange(filters.GetSelection());
            Helpers.FilterOperations.FilterDetailContainer fdc = new Helpers.FilterOperations.FilterDetailContainer();
            fdc = dynProd.GetSelection();
            if (fdc.Values.Count > 0)
                fc.FilterDetails.Add(dynProd.GetSelection());
            fc.FilterDetails.AddRange(adHocFilters.GetSelection());
            fc.FilterDetails.AddRange(activities.GetSelection());

            int count = 0;
            List<int> subIDs = new List<int>();
            BackgroundWorker bw = new BackgroundWorker();
            busy.IsBusy = true;
            bw.DoWork += (o, ea) =>
            {
                details = fo.GetDisplayFilterDetail(fc);
                FrameworkUAD.Object.Reporting obj = fo.GetReportingObjectFromContainer(fc, tmpProductID);
                obj.SearchType = "ProductView";
                subCountResponse = reportData.Proxy.SelectSubCountUAD(accessKey, obj, myClient.ClientConnections);
                if (Common.CheckResponse(subCountResponse.Result, subCountResponse.Status))
                {
                    subIDs = subCountResponse.Result;
                }
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                split.IssueSplitCount = count;
                split.IssueSplitName = (filterCriteria.Count + 1).ToString();
                filterCriteria.Add(new Helpers.Common.FilterCriteria((filterCriteria.Count + 1).ToString(), fc, details, subIDs));
                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();
        }

        private void btnReports_Click(object sender, RoutedEventArgs e)
        {
            //List<string> t = filterCriteria.Select(x => x.FilterName).ToList();
            //WindowsAndDialogs.PopOut win = new WindowsAndDialogs.PopOut();
            //win.Title = "Build Reports";
            //win.Content = new UADControls.UADFilterReports(filterCriteria);
            //win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //win.Show();
        }

        private void Delete_Filter(object sender, MouseButtonEventArgs e)
        {
            Image me = sender as Image;
            Helpers.Common.FilterCriteria container = me.DataContext as Helpers.Common.FilterCriteria;
            filterCriteria.Remove(container);
        }
        #endregion

        public void CalculateReport(List<UADFilterReports.Expression> expressions)
        {
            
        }
    }
}
