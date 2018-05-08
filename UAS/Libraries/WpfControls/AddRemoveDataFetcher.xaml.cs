using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core_AMS.Utilities;
using FrameworkUAD.Entity;
using KM.Common.Functions;
using Microsoft.Win32;
using ReportLibrary.Reports;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;
using WpfControls.Helpers;

namespace WpfControls
{
    /// <summary>
    /// Interaction logic for AddRemoveDataFetcher.xaml
    /// </summary>
    public partial class AddRemoveDataFetcher : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        #region ServiceCalls
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.IAction> actionData = FrameworkServices.ServiceClient.UAD_Lookup_ActionClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICategoryCode> catCodeW = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ITransactionCode> transCodeW = FrameworkServices.ServiceClient.UAD_Lookup_TransactionCodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ITransactionCodeType> transCodeTypeW = FrameworkServices.ServiceClient.UAD_Lookup_TransactionCodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICategoryCodeType> catCodeTypeW = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IReports> reportData = FrameworkServices.ServiceClient.UAD_ReportsClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IFilter> filterData = FrameworkServices.ServiceClient.UAS_FilterClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IFilterDetail> filterDetailData = FrameworkServices.ServiceClient.UAS_FilterDetailClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IFilterDetailSelectedValue> filterDetailValuesData = FrameworkServices.ServiceClient.UAS_FilterDetailSelectedValueClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscriberAddKill> subAddKillData = FrameworkServices.ServiceClient.UAD_SubscriberAddKillClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProductSubscription> subscriptionData = FrameworkServices.ServiceClient.UAD_ProductSubscriptionClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeData = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> productW = FrameworkServices.ServiceClient.UAD_ProductClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICountry> countryW = FrameworkServices.ServiceClient.UAD_Lookup_CountryClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IIssue> issueW = FrameworkServices.ServiceClient.UAD_IssueClient();
        #endregion
        #region ServiceResponse
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Action>> actionResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Action>>();
        private FrameworkUAS.Service.Response<bool> boolResponse = new FrameworkUAS.Service.Response<bool>();
        private FrameworkUAS.Service.Response<FrameworkUAD_Lookup.Entity.CategoryCode> catResponse = new FrameworkUAS.Service.Response<FrameworkUAD_Lookup.Entity.CategoryCode>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>> catListResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>>();
        private FrameworkUAS.Service.Response<FrameworkUAD_Lookup.Entity.TransactionCode> transResponse = new FrameworkUAS.Service.Response<FrameworkUAD_Lookup.Entity.TransactionCode>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCodeType>> transTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCodeType>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Report>> reportResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Report>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.Filter>> filterResponse = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.Filter>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FilterDetail>> filterDetailResponse = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FilterDetail>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FilterDetailSelectedValue>> filterDetailSelectedValuedResponse = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.FilterDetailSelectedValue>>();
        private FrameworkUAS.Service.Response<int> filterSaveResponse = new FrameworkUAS.Service.Response<int>();
        private FrameworkUAS.Service.Response<int> filterDetailSaveResponse = new FrameworkUAS.Service.Response<int>();
        private FrameworkUAS.Service.Response<int> filterDetailValueSaveResponse = new FrameworkUAS.Service.Response<int>();
        private FrameworkUAS.Service.Response<int> intResponse = new FrameworkUAS.Service.Response<int>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.SubscriberAddKill>> subAddKillResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.SubscriberAddKill>>();
        private FrameworkUAS.Service.Response<List<int>> subCountResponse = new FrameworkUAS.Service.Response<List<int>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductSubscription>> subscriptionResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ProductSubscription>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ActionProductSubscription>> actionSubscriptionResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ActionProductSubscription>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> codeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private FrameworkUAS.Service.Response<FrameworkUAD_Lookup.Entity.Code> codeSingleResponse = new FrameworkUAS.Service.Response<FrameworkUAD_Lookup.Entity.Code>();
        private FrameworkUAS.Service.Response<DataTable> dataTableResponse = new FrameworkUAS.Service.Response<DataTable>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Issue>> issueResponse = new FrameworkUAS.Service.Response<List<Issue>>();
        #endregion
        #region Variables/Lists
        Core_AMS.Utilities.FileFunctions ff = new FileFunctions();
        Helpers.FilterOperations fo = new Helpers.FilterOperations();
        private List<ActionProductSubscription> subscriptionList = new List<ActionProductSubscription>();
        private List<ActionProductSubscription> activeSubsList = new List<ActionProductSubscription>();
        private List<int> activeSubscriptionIDs = new List<int>();
        private List<int> subscriptionPool = new List<int>();
        private List<FrameworkUAD.Entity.Report> reports = new List<Report>();
        private int groupID = 0;
        private Guid accessKey;
        private int myPubID;
        //private int currentActive = 0;
        //private int currentPaid = 0;
        private int addCatCode = 0;
        private int killTransCode = 0;
        private int addTransCode = 0;
        private FrameworkUAD.Entity.Product myProduct = new Product();
        private RadBusyIndicator busy = new RadBusyIndicator();
        private ObservableCollection<AddRemoveDetail> lstAddRemoveDetail = new ObservableCollection<AddRemoveDetail>();
        private List<FrameworkUAD_Lookup.Entity.Country> countries = new List<FrameworkUAD_Lookup.Entity.Country>();
        private List<FrameworkUAD_Lookup.Entity.CategoryCode> catCodes = new List<FrameworkUAD_Lookup.Entity.CategoryCode>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCodeType> transCodeTypes = new List<FrameworkUAD_Lookup.Entity.TransactionCodeType>();
        private List<FrameworkUAD_Lookup.Entity.CategoryCodeType> catCodeTypes = new List<FrameworkUAD_Lookup.Entity.CategoryCodeType>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCode> transCodes = new List<FrameworkUAD_Lookup.Entity.TransactionCode>();
        private FrameworkUAD.Entity.Issue myIssue = new Issue();
        public ObservableCollection<AddRemoveDetail> AddRemoves { get { return lstAddRemoveDetail; } }
        private Totals _totals = new Totals(0,0,0,0,0,0,0,0);
        public Totals MyTotals { get { return _totals; } }
        #endregion
        #region Classes & Enums
        public class AddRemoveDetail : INotifyPropertyChanged
        {
            private int _desiredCount;
            public int ActualCount { get; set; }
            public int DesiredCount
            {
                get { return _desiredCount; }
                set
                {
                    _desiredCount = value;
                    if (_desiredCount == 0)
                        DesiredCount = 1;
                    else if (_desiredCount > SubscriptionList.Count)
                        _desiredCount = SubscriptionList.Count;
                    PendingSubscriptionList = GetNthSubscribers(SubscriptionList.Count, _desiredCount, SubscriptionList.Select(x=> x.PubSubscriptionID).ToList());
                    UpdateCounts();
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("DesiredCount"));
                    }                    
                }
            }
            public int ChangedFreeRecords { get; set; }
            public int ChangedFreeCopies { get; set; }
            public int ChangedPaidRecords { get; set; }
            public int ChangedPaidCopies { get; set; }
            public List<FrameworkUAD.Entity.ActionProductSubscription> SubscriptionList { get; set; }
            public List<int> PendingSubscriptionList { get; set; }
            public int AddKillID { get; set; }
            public List<FilterControls.Framework.FilterObject> Filters { get; set; }
            public AddRemoveType Type { get; set; }
            public ObservableCollection<Helpers.FilterOperations.DisplayedFilterDetail> FilterDetails { get; set; }
            public void UpdateCounts()
            {
                int paidRecords = 0;
                int freeRecords = 0;
                int paidCopies = 0;
                int freeCopies = 0;
                List<FrameworkUAD.Entity.ActionProductSubscription> pubs = this.SubscriptionList.Where(x => this.PendingSubscriptionList.Contains(x.PubSubscriptionID)).ToList();
                pubs.ForEach(x =>
                {
                    if (x.CategoryType == FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Free.ToString().Replace("_", " ") ||
                        x.CategoryType == FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Free.ToString().Replace("_", " "))
                    {
                        freeRecords++;
                        freeCopies += x.Copies;
                    }
                    else if (x.CategoryType == FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Paid.ToString().Replace("_", " ") ||
                        x.CategoryType == FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Paid.ToString().Replace("_", " "))
                    {
                        paidRecords++;
                        paidCopies += x.Copies;
                    }
                });

                this.ChangedFreeCopies = freeCopies;
                this.ChangedFreeRecords = freeRecords;
                this.ChangedPaidCopies = paidCopies;
                this.ChangedPaidRecords = paidRecords;
            }

            public AddRemoveDetail(ObservableCollection<FilterOperations.DisplayedFilterDetail> filterDetails, List<FrameworkUAD.Entity.ActionProductSubscription> subs, 
                ObservableCollection<FilterControls.Framework.FilterObject> filters, AddRemoveType type)
            {
                this.ActualCount = subs.Count;
                this.AddKillID = 0;
                this.FilterDetails = filterDetails;
                this.SubscriptionList = subs;
                this.Filters = filters.ToList();
                this.DesiredCount = subs.Count;
                this.Type = type;
            }
            public AddRemoveDetail() {}

            public event PropertyChangedEventHandler PropertyChanged;
        }
        public class Totals : INotifyPropertyChanged
        {
            private int _currentFreeRecords;
            private int _currentFreeCopies;
            private int _newFreeRecords;
            private int _newFreeCopies;
            private int _currentPaidRecords;
            private int _currentPaidCopies;
            private int _newPaidRecords;
            private int _newPaidCopies;

            public int CurrentFreeRecords
            {
                get { return _currentFreeRecords; }
                set
                {
                    _currentFreeRecords = value;
                    FirePropertyChanged(nameof(CurrentFreeRecords));
                }
            }

            public int CurrentFreeCopies
            {
                get { return _currentFreeCopies; }
                set
                {
                    _currentFreeCopies = value;
                    FirePropertyChanged(nameof(CurrentFreeCopies));
                }
            }

            public int NewFreeRecords
            {
                get { return _newFreeRecords; }
                set
                {
                    _newFreeRecords = value;
                    FirePropertyChanged(nameof(NewFreeRecords));
                }
            }

            public int NewFreeCopies
            {
                get { return _newFreeCopies; }
                set
                {
                    _newFreeCopies = value;
                    FirePropertyChanged(nameof(NewFreeCopies));
                }
            }

            public int CurrentPaidRecords
            {
                get { return _currentPaidRecords; }
                set
                {
                    _currentPaidRecords = value;
                    FirePropertyChanged(nameof(CurrentPaidRecords));
                }
            }

            public int CurrentPaidCopies
            {
                get { return _currentPaidCopies; }
                set
                {
                    _currentPaidCopies = value;
                    FirePropertyChanged(nameof(CurrentPaidCopies));
                }
            }

            public int NewPaidRecords
            {
                get { return _newPaidRecords; }
                set
                {
                    _newPaidRecords = value;
                    FirePropertyChanged(nameof(NewPaidRecords));
                }
            }

            public int NewPaidCopies
            {
                get { return _newPaidCopies; }
                set
                {
                    _newPaidCopies = value;
                    FirePropertyChanged(nameof(NewPaidCopies));
                }
            }

            public Totals(int currFreeRecords, int currFreeCopies, int newFreeRecords, int newFreeCopies, int currPaidRecords, int currPaidCopies, int newPaidCopies, int newPaidRecords)
            {
                this.CurrentFreeRecords = currFreeRecords;
                this.CurrentFreeCopies = currFreeCopies;
                this.NewFreeRecords = newFreeRecords;
                this.NewFreeCopies = newFreeCopies;
                this.CurrentPaidRecords = currPaidRecords;
                this.CurrentPaidCopies = currPaidCopies;
                this.NewPaidCopies = newPaidCopies;
                this.NewPaidRecords = newPaidRecords;
            }

            public event PropertyChangedEventHandler PropertyChanged;

            private void FirePropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
        public enum AddRemoveType
        {
            Add,
            Remove
        }
        public class MyTotalFunction : AggregateFunction<AddRemoveDetail, int>
        {
            public MyTotalFunction()
            {
                this.AggregationExpression = x => x.Where(y => y.Type == AddRemoveType.Add).Select(y => y.DesiredCount).Sum() - x.Where(y => y.Type == AddRemoveType.Remove).Select(y => y.DesiredCount).Sum();
                Caption = "Total Desired: ";                
            }
        }
        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (AddRemoveDetail newItem in e.NewItems)
                {
                    //Add listener for each item on PropertyChanged event
                    newItem.PropertyChanged += this.OnItemPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (AddRemoveDetail oldItem in e.OldItems)
                {
                    oldItem.PropertyChanged -= this.OnItemPropertyChanged;
                }
            }
            UpdateNewTotals();
        }
        void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateNewTotals();
        }
        #endregion

        public AddRemoveDataFetcher(int pubID)
        {
            InitializeComponent();
            accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
            myPubID = pubID;
            AddRemoves.CollectionChanged += OnCollectionChanged;
            LoadData();
            SetControls();
            grdAddRemoveDetail.Columns[3].AggregateFunctions.Add(new MyTotalFunction());
            catCodes = catCodeW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
            transCodeTypes = transCodeTypeW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
            transCodes = transCodeW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
            catCodeTypes = catCodeTypeW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;

        }

        #region Loading
        private void LoadAddKills(object sender, RoutedEventArgs e)
        {
            btnAdd.IsChecked = true;
            UserControl uc = this.ParentOfType<UserControl>();
            busy = uc.FindChildByType<RadBusyIndicator>();
        }
        public void LoadData()
        {
            actionSubscriptionResponse = subscriptionData.Proxy.SelectActionSubscription(accessKey, myPubID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if(Helpers.Common.CheckResponse(actionSubscriptionResponse.Result, actionSubscriptionResponse.Status))
            {
                subscriptionList = actionSubscriptionResponse.Result;
                activeSubsList = subscriptionList.Where(x => x.CategoryCodeValue != 70 && x.CategoryCodeValue != 71 && 
                    x.CategoryType != FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Paid.ToString().Replace("_", " ") &&
                    x.CategoryType != FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Free.ToString().Replace("_", " ") && 
                    x.TransactionType != FrameworkUAD_Lookup.Enums.TransactionCodeType.Free_Inactive.ToString().Replace("_", " ") &&
                    x.TransactionType != FrameworkUAD_Lookup.Enums.TransactionCodeType.Paid_Inactive.ToString().Replace("_", " ")).ToList();
                activeSubscriptionIDs = activeSubsList.Select(x => x.PubSubscriptionID).ToList();
            }
            catListResponse = catCodeW.Proxy.Select(accessKey);
            codeResponse = codeData.Proxy.Select(accessKey, FrameworkUAD_Lookup.Enums.CodeType.Filter_Group);
            if(Helpers.Common.CheckResponse(codeResponse.Result, codeResponse.Status))
            {
                groupID = codeResponse.Result.Where(x => x.CodeName == FrameworkUAD_Lookup.Enums.FilterGroupTypes.Add_Remove.ToString().Replace("_", " ")).Select(x=> x.CodeId).FirstOrDefault();
            }
            reportResponse = reportData.Proxy.Select_For_AddRemove_Reports(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, myPubID);
            if(Helpers.Common.CheckResponse(reportResponse.Result, reportResponse.Status))
            {
                reports = reportResponse.Result.Where(x=> x.ProductID == myPubID).ToList();
            }
            catResponse = catCodeW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, 1, 10);
            if (catResponse.Result != null && catResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                addCatCode = catResponse.Result.CategoryCodeID;
            transResponse = transCodeW.Proxy.SelectTransactionCodeValue(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, 10);
            if (transResponse.Result != null && transResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                addTransCode = transResponse.Result.TransactionCodeID;
            transResponse = transCodeW.Proxy.SelectTransactionCodeValue(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, 38);
            if (transResponse.Result != null && transResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                killTransCode = transResponse.Result.TransactionCodeID;
            myProduct = productW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myPubID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
            countries = countryW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey).Result;
            List<FrameworkUAD.Entity.Issue> issues = issueW.Proxy.SelectForPublication(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myPubID,
                FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;

            if(issues != null)
                myIssue = issues.Where(x => x.PublicationId == myPubID && x.IsComplete == false).FirstOrDefault();
        }
        public void SetControls()
        {
            subscriptionPool = subscriptionList.Select(x => x.PubSubscriptionID).ToList();
            rcbReport.ItemsSource = reports.OrderBy(x=> x.ReportName);
            rcbReport.DisplayMemberPath = "ReportName";
            rcbReport.SelectedValuePath = "ReportID";
            UpdateCurrentTotals();
            busy.IsBusy = false;
        }
        #endregion

        #region UI Events
        private void btnGenerateRecords_Click(object sender, RoutedEventArgs e)
        {
            Window w = Window.GetWindow(this);

            FilterControls.FilterWrapper fw = w.FindChildByType<FilterControls.FilterWrapper>();
            FilterControls.Framework.FiltersViewModel vm = fw.MyViewModel;

            if(vm.ActiveFilters.Where(x=> x.Name == FilterControls.Framework.Enums.FilterObjects.CategoryCode).Count() == 0)
            {
                MessageBox.Show("Please select Category Code(s).", "Alert", MessageBoxButton.OK);
                return;
            }

            BackgroundWorker bw = new BackgroundWorker();
            busy.IsBusy = true;
            busy.IsIndeterminate = true;
            List<int> subIDs = new List<int>();
            ObservableCollection<FilterOperations.DisplayedFilterDetail> details = new ObservableCollection<FilterOperations.DisplayedFilterDetail>();
            int count = 0;

            bw.DoWork += (ev, o) =>
            {
                FrameworkUAD.Object.ReportingXML obj = new FrameworkUAD.Object.ReportingXML();
                obj = vm.ActiveFiltersXML;
                subCountResponse = reportData.Proxy.SelectSubscriberCount(accessKey, obj.Filters, obj.AdHocFilters, false, false, 0, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            };
            bw.RunWorkerCompleted += (ev, o) =>
            {
                busy.IsBusy = false;
                if (Helpers.Common.CheckResponse(subCountResponse.Result, subCountResponse.Status))
                {
                    subIDs = subCountResponse.Result;
                }

                if (btnAdd.IsChecked == true)
                {
                    subIDs = subIDs.Except(activeSubscriptionIDs).ToList();
                    subIDs = subIDs.Intersect(subscriptionPool).ToList();
                    count = subscriptionList.Where(x => subIDs.Contains(x.PubSubscriptionID)).Select(x => x.Copies).Sum();
                }
                else if (btnRemove.IsChecked == true)
                {
                    subIDs = subIDs.Intersect(activeSubscriptionIDs).ToList();
                    subIDs = subIDs.Intersect(subscriptionPool).ToList();
                    count = subscriptionList.Where(x => subIDs.Contains(x.PubSubscriptionID)).Select(x => x.Copies).Sum();
                }

                if (count > 0)
                {
                    subscriptionPool = subscriptionPool.Except(subIDs).ToList();
                    if (btnAdd.IsChecked == true)
                    {
                        ObservableCollection<FilterControls.Framework.FilterObject> currentFilterObjects = new ObservableCollection<FilterControls.Framework.FilterObject>();
                        currentFilterObjects = vm.ActiveFilters.DeepClone();
                        this.AddRemoves.Add(new AddRemoveDetail(details, subscriptionList.Where(x => subIDs.Contains(x.PubSubscriptionID)).ToList(), currentFilterObjects, AddRemoveType.Add));
                    }
                    else if (btnRemove.IsChecked == true)
                    {
                        ObservableCollection<FilterControls.Framework.FilterObject> currentFilterObjects = new ObservableCollection<FilterControls.Framework.FilterObject>();
                        currentFilterObjects = vm.ActiveFilters.DeepClone();
                        this.AddRemoves.Add(new AddRemoveDetail(details, subscriptionList.Where(x => subIDs.Contains(x.PubSubscriptionID)).ToList(), currentFilterObjects, AddRemoveType.Remove));
                    }
                }
                else
                    MessageBox.Show("No records to display.");
            };
            bw.RunWorkerAsync();
        }
        private void Delete_AddRemove(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            AddRemoveDetail record = img.DataContext as AddRemoveDetail;
            this.AddRemoves.Remove(record);
            subscriptionPool.AddRange(record.SubscriptionList.Select(x=> x.PubSubscriptionID));

            #region OLD - KEEP
            //AddRemoveDetail total = lstAddRemoveDetail.Last();
            //lstAddRemoveDetail.Remove(lstAddRemoveDetail.Last());
            //lstAddRemoveDetail.Add(new AddRemoveDetail(total.AddsCount - record.AddsCount, total.RemovedCount - record.RemovedCount, null, null, null,true));

            //if (record.AddKillID > 0)
            //{
            //    BackgroundWorker bw = new BackgroundWorker();
            //    busy.IsBusy = true;
            //    bw.DoWork += (o, ea) =>
            //    {
            //        string xml = "";
            //        foreach (int id in record.SubscriberList)
            //        {
            //            xml += "<ID>" + id + "</ID>";
            //        }

            //        xml = "<XML><SUBSCRIBERS>" + xml + "</SUBSCRIBERS></XML>";
            //        subAddKillData.Proxy.UpdateSubscription(accessKey, record.AddKillID, myPubID, xml, true);
            //        subscriptionResponse = subscriptionData.Proxy.SelectPublication(accessKey, myPubID);
            //        if (Helpers.Common.CheckResponse(subscriptionResponse.Result, subscriptionResponse.Status))
            //            subscriptionList = subscriptionResponse.Result;
            //        FrameworkUAS.Entity.SubscriberAddKill adk = new FrameworkUAS.Entity.SubscriberAddKill();
            //        subAddKillResponse = subAddKillData.Proxy.Select(accessKey);
            //        if (Helpers.Common.CheckResponse(subAddKillResponse.Result, subAddKillResponse.Status))
            //        {
            //            adk = subAddKillResponse.Result.Where(x => x.AddKillID == record.AddKillID).FirstOrDefault();
            //            if (adk != null)
            //            {
            //                adk.IsActive = false;
            //                subAddKillData.Proxy.Save(accessKey, adk);
            //            }
            //        }
            //    };
            //    bw.RunWorkerCompleted += (o, ea) =>
            //    {
            //        UpdateNewActive();
            //        busy.IsBusy = false;
            //        MessageBox.Show("Add/Remove deleted. Records have been updated.", "Confirmation", MessageBoxButton.OK);
            //    };
            //    bw.RunWorkerAsync();
            //}
            #endregion
        }
        private void btnAdd_Checked(object sender, RoutedEventArgs e)
        {
            Window w = Window.GetWindow(this);

            FilterControls.FilterWrapper fw = w.FindChildByType<FilterControls.FilterWrapper>();
            FilterControls.Framework.FiltersViewModel vm = fw.MyViewModel;

            foreach(FilterControls.Framework.FilterObject fo in vm.Filters.Where(x=> x.FilterType == FilterControls.Framework.Enums.Filters.Standard).FirstOrDefault().Objects.Where(x=> x.Name == FilterControls.Framework.Enums.FilterObjects.CategoryCodeType ||
                x.Name == FilterControls.Framework.Enums.FilterObjects.CategoryCode || x.Name == FilterControls.Framework.Enums.FilterObjects.TransactionCodeType || 
                x.Name == FilterControls.Framework.Enums.FilterObjects.TransactionCode))
            {
                fo.RemoveSelection();
            }

            foreach(FilterControls.Framework.ListObject lo in vm.Filters.Where(x=> x.FilterType == FilterControls.Framework.Enums.Filters.Standard).FirstOrDefault().Objects.
                    Where(x=> x.Name == FilterControls.Framework.Enums.FilterObjects.CategoryCode).Cast<FilterControls.Framework.ListFilterObject>().FirstOrDefault().Options)
            {
                if (lo.DisplayValue.Contains("70"))
                    lo.Selected = true;
            }

            foreach (FilterControls.Framework.ListObject lo in vm.Filters.Where(x => x.FilterType == FilterControls.Framework.Enums.Filters.Standard).FirstOrDefault().Objects.
                    Where(x => x.Name == FilterControls.Framework.Enums.FilterObjects.TransactionCodeType).Cast<FilterControls.Framework.ListFilterObject>().FirstOrDefault().Options)
            {
                if (lo.Value == "1")
                    lo.Selected = true;
            }
        }
        private void btnRemove_Checked(object sender, RoutedEventArgs e)
        {
            Window w = Window.GetWindow(this);

            FilterControls.FilterWrapper fw = w.FindChildByType<FilterControls.FilterWrapper>();
            FilterControls.Framework.FiltersViewModel vm = fw.MyViewModel;

            foreach (FilterControls.Framework.FilterObject fo in vm.Filters.Where(x => x.FilterType == FilterControls.Framework.Enums.Filters.Standard).FirstOrDefault().Objects.Where(x => x.Name == FilterControls.Framework.Enums.FilterObjects.CategoryCodeType ||
                     x.Name == FilterControls.Framework.Enums.FilterObjects.CategoryCode || x.Name == FilterControls.Framework.Enums.FilterObjects.TransactionCodeType ||
                     x.Name == FilterControls.Framework.Enums.FilterObjects.TransactionCode))
            {
                fo.RemoveSelection();
            }

            foreach (FilterControls.Framework.ListObject lo in vm.Filters.Where(x => x.FilterType == FilterControls.Framework.Enums.Filters.Standard).FirstOrDefault().Objects.
                    Where(x => x.Name == FilterControls.Framework.Enums.FilterObjects.CategoryCodeType).Cast<FilterControls.Framework.ListFilterObject>().FirstOrDefault().Options)
            {
                if (lo.Value == "1")
                    lo.Selected = true;
            }

            foreach (FilterControls.Framework.ListObject lo in vm.Filters.Where(x => x.FilterType == FilterControls.Framework.Enums.Filters.Standard).FirstOrDefault().Objects.
                    Where(x => x.Name == FilterControls.Framework.Enums.FilterObjects.CategoryCode).Cast<FilterControls.Framework.ListFilterObject>().FirstOrDefault().Options)
            {
                if (lo.DisplayValue.Contains("70") || lo.DisplayValue.Contains("71"))
                    lo.Selected = false;
            }

            foreach (FilterControls.Framework.ListObject lo in vm.Filters.Where(x => x.FilterType == FilterControls.Framework.Enums.Filters.Standard).FirstOrDefault().Objects.
                    Where(x => x.Name == FilterControls.Framework.Enums.FilterObjects.TransactionCodeType).Cast<FilterControls.Framework.ListFilterObject>().FirstOrDefault().Options)
            {
                if (lo.Value == "1")
                    lo.Selected = true;
            }
        }
        private void btnDownloadDetails_Click(object sender, RoutedEventArgs e)
        {
            if (lstAddRemoveDetail.Count > 0)
            {
                string columns = "";
                List<int> subIDs = new List<int>();
                foreach (AddRemoveDetail ard in grdAddRemoveDetail.Items)
                {
                    subIDs.AddRange(ard.PendingSubscriptionList);
                }
                HashSet<int> hs = new HashSet<int>(subIDs);
                DataTable master = new DataTable();

                DownloadDetails dwnDetails = new DownloadDetails(myPubID);
                dwnDetails.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                dwnDetails.Check += value => columns = value;

                dwnDetails.ShowDialog();

                if (columns != "")
                {
                    busy.IsBusy = true;
                    busy.IsIndeterminate = true;
                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += (o, ea) =>
                    {
                        #region GetSubscribers
                        DataTable dt = new DataTable();
                        int rowProcessedCount = 0;
                        int index = 0;
                        int size = 2500;
                        while (master.Rows.Count < hs.Count)
                        {
                            if ((index + 2500) > subIDs.Count)
                                size = subIDs.Count - index;
                            List<int> temp = subIDs.GetRange(index, size);
                            index += 2500;
                            dt = subscriptionData.Proxy.SelectForExportStatic(accessKey, myPubID, columns, temp, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
                            rowProcessedCount += dt.Rows.Count;

                            dt.AcceptChanges();
                            master.Merge(dt);
                        }
                        #endregion
                    };
                    bw.RunWorkerCompleted += (o, ea) =>
                    {
                        if(master.Columns.Contains("PubSubscriptionID"))
                            master.Columns.Remove("PubSubscriptionID");
                        master.AcceptChanges();
                        busy.IsBusy = false;
                        RadGridView grd = new RadGridView();
                        grd.AutoGenerateColumns = true;
                        grd.ItemsSource = master;
                        string extension = "csv";
                        SaveFileDialog dialog = new SaveFileDialog()
                        {
                            DefaultExt = extension,
                            Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", extension, "CSV"),
                            FilterIndex = 1,
                        };
                        if (dialog.ShowDialog() == true)
                        {
                            using (Stream stream = dialog.OpenFile())
                            {
                                string test = grd.ToCsv();
                                StreamWriter sw = new StreamWriter(stream);

                                test = HtmlFunctions.StripTextFromHtml(test);

                                sw.Write(test);
                                sw.Flush();
                                sw.Close();
                            }
                        }
                        Core_AMS.Utilities.WPF.Message("Download complete.");
                    };
                    bw.RunWorkerAsync();
                }
            }
            else
                Core_AMS.Utilities.WPF.Message("Please generate filters before downloading.");
        }
        private void ApplyOrUpdate_AddKillRecords(object sender, RoutedEventArgs e)
        {
            RadButton me = sender as RadButton;
            AddRemoveDetail ard = me.DataContext as AddRemoveDetail;
            int addRemoveID;

            MessageBoxResult result = MessageBox.Show("Do you really want to apply these Adds/Removes to the database?", "Apply Add/Removes", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
                return;
            
            List<int> editList = ard.SubscriptionList.Select(x=> x.PubSubscriptionID).ToList();

            if (editList.Count > 0)
            {
                BackgroundWorker bw = new BackgroundWorker();
                busy.IsBusy = true;
                busy.BusyContent = "Loading...";
                busy.ProgressValue = 0;
                busy.IsIndeterminate = false;
                bw.DoWork += (o, ea) =>
                {

                    //if (ard.DesiredCount > editList.Count)
                    //{
                    //    ard.DesiredCount = editList.Count;
                    //}

                    #region Save SubAddRemove

                    FrameworkUAD.Entity.SubscriberAddKill s = new FrameworkUAD.Entity.SubscriberAddKill();
                    s.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                    s.PublicationID = myPubID;
                    s.AddKillCount = ard.DesiredCount;
                    s.Count = ard.ActualCount;
                    s.Type = ard.Type.ToString();

                    addRemoveID = subAddKillData.Proxy.Save(accessKey, s, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;

                    List<FrameworkUAD.Entity.SubscriberAddKillDetail> details = new List<SubscriberAddKillDetail>();

                    if (ard.Type == AddRemoveType.Add)
                    {
                        foreach (int i in ard.PendingSubscriptionList)
                            details.Add(new SubscriberAddKillDetail(addRemoveID, addCatCode, addTransCode, i));
                    }
                    else
                    {
                        foreach (int i in ard.PendingSubscriptionList)
                        {
                            int cat = 0;
                            FrameworkUAD.Entity.ActionProductSubscription a = subscriptionList.Where(x => x.PubSubscriptionID == i).FirstOrDefault();
                            if (a != null)
                                cat = a.PubCategoryID;
                            details.Add(new SubscriberAddKillDetail(addRemoveID, cat, killTransCode, i));
                        }
                    }

                    boolResponse = subAddKillData.Proxy.BulkInsertDetails(accessKey, details, addRemoveID,
                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                    #endregion

                    //List<int> LNth = getNth(editList.Count, ard.DesiredCount);
                    //List<int> ids = new List<int>();
                    List<ActionProductSubscription> workingList = new List<ActionProductSubscription>();
                    string xml = "<XML>";
                    //string xml = "<XML>";
                    //foreach (int n in LNth)
                    //{
                    //    var id = editList.Skip(n).Take(1);
                    //    ids.Add(id.ToList().FirstOrDefault());
                    //}

                    #region UpdateSubscriptions
                    HashSet<int> hsIDs = new HashSet<int>(ard.PendingSubscriptionList);
                    workingList = subscriptionList.Where(x => hsIDs.Contains(x.PubSubscriptionID)).ToList();
                    int total = workingList.Count;
                    int counter = 0;
                    int processedCount = 0;
                    int batchSize = 1000;

                    if (ard.Type == AddRemoveType.Add)
                    {
                        foreach (ActionProductSubscription sp in workingList)
                        {
                            xml = xml + "<Subscription><SubscriptionID>" + sp.PubSubscriptionID + "</SubscriptionID><PubCategoryID>" + addCatCode + "</PubCategoryID><PubTransactionID>" + 
                                addTransCode + "</PubTransactionID></Subscription>";

                            counter++;
                            processedCount++;
                            if (processedCount == total || counter == batchSize)
                            {
                                xml = xml + "</XML>";
                                subscriptionData.Proxy.SaveBulkActionIDUpdate(accessKey, xml, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                                counter = 0;
                                xml = "<XML>";
                                this.Dispatcher.BeginInvoke(new System.Action(() =>
                                {
                                    busy.ProgressValue = (int)Math.Round((double)(processedCount * 100) / total);
                                }));
                            }
                        }
                    }
                    else if(ard.Type == AddRemoveType.Remove)
                    {
                        foreach (ActionProductSubscription sp in workingList)
                        {
                            xml = xml + "<Subscription><SubscriptionID>" + sp.PubSubscriptionID + "</SubscriptionID><PubCategoryID>" + sp.PubCategoryID + 
                                "</PubCategoryID><PubTransactionID>" + killTransCode + "</PubTransactionID></Subscription>";

                            counter++;
                            processedCount++;
                            if (processedCount == total || counter == batchSize)
                            {
                                xml = xml + "</XML>";
                                subscriptionData.Proxy.SaveBulkActionIDUpdate(accessKey, xml, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                                counter = 0;
                                xml = "<XML>";
                                this.Dispatcher.BeginInvoke(new System.Action(() =>
                                {
                                    busy.ProgressValue = (int)Math.Round((double)(processedCount * 100) / total);
                                }));
                            }
                        }
                    }
                    actionSubscriptionResponse = subscriptionData.Proxy.SelectActionSubscription(accessKey, myPubID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                    if (Helpers.Common.CheckResponse(actionSubscriptionResponse.Result, actionSubscriptionResponse.Status))
                    {
                        subscriptionList = actionSubscriptionResponse.Result;
                        activeSubsList = subscriptionList.Where(x => x.CategoryCodeValue != 70 && x.CategoryCodeValue != 71 &&
                            x.CategoryType != FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Paid.ToString().Replace("_", " ") &&
                            x.CategoryType != FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Free.ToString().Replace("_", " ") &&
                            x.TransactionType != FrameworkUAD_Lookup.Enums.TransactionCodeType.Free_Inactive.ToString().Replace("_", " ") &&
                            x.TransactionType != FrameworkUAD_Lookup.Enums.TransactionCodeType.Paid_Inactive.ToString().Replace("_", " ")).ToList();
                        activeSubscriptionIDs = activeSubsList.Select(x => x.PubSubscriptionID).ToList();
                    }
                    #endregion

                    subscriptionPool.AddRange(ard.SubscriptionList.Select(x=> x.PubSubscriptionID));
                };
                bw.RunWorkerCompleted += (o, ea) =>
                {
                    this.AddRemoves.Remove(ard);
                    UpdateCurrentTotals();
                    busy.IsBusy = false;
                    busy.IsIndeterminate = true;
                    MessageBox.Show("Records updated.", "Confirmation", MessageBoxButton.OK);
                };
                bw.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("No unique records to process.", "Warning", MessageBoxButton.OK);
                return;
            }

        }
        private void rcbReport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RadComboBox rcb = sender as RadComboBox;
            FrameworkUAD.Entity.Report rpt = rcb.SelectedItem as FrameworkUAD.Entity.Report;
            if(rpt != null)
            {
                ReportUtilities.ProductId = myPubID;
                FrameworkUAD_Lookup.Entity.Code c = new FrameworkUAD_Lookup.Entity.Code();
                codeSingleResponse = codeData.Proxy.SelectCodeId(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, rpt.ReportTypeID);
                if (codeSingleResponse.Result != null && codeSingleResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    c = codeSingleResponse.Result;
                else
                    Core_AMS.Utilities.WPF.MessageServiceError();
                string catIDs = "";
                string transIDs = "";

                int catQFreeID = catCodeTypes.SingleOrDefault(x => x.CategoryCodeTypeName == FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Free.ToString().Replace("_", " ")).CategoryCodeTypeID;
                int catQPaidID = catCodeTypes.SingleOrDefault(x => x.CategoryCodeTypeName == FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Paid.ToString().Replace("_", " ")).CategoryCodeTypeID;
                int tranActiveFreeid = transCodeTypes.SingleOrDefault(x => x.TransactionCodeTypeName == FrameworkUAD_Lookup.Enums.TransactionCodeType.Free_Active.ToString().Replace("_", " ")).TransactionCodeTypeID;
                int tranActivePaidid = transCodeTypes.SingleOrDefault(x => x.TransactionCodeTypeName == FrameworkUAD_Lookup.Enums.TransactionCodeType.Paid_Active.ToString().Replace("_", " ")).TransactionCodeTypeID;


                foreach (FrameworkUAD_Lookup.Entity.CategoryCode cc in catCodes)
                {
                    if ((cc.CategoryCodeTypeID == catQFreeID || cc.CategoryCodeTypeID == catQPaidID) && cc.CategoryCodeValue != 70)
                        catIDs += cc.CategoryCodeID + ",";
                }
                catIDs = catIDs.TrimEnd(',');

                foreach (FrameworkUAD_Lookup.Entity.TransactionCode tc in transCodes)
                {
                    if (tc.TransactionCodeTypeID == tranActiveFreeid || tc.TransactionCodeTypeID == tranActivePaidid)
                    {
                        transIDs += tc.TransactionCodeID + ",";
                    }
                }
                transIDs = transIDs.TrimEnd(',');


                FrameworkUAD.Object.ReportingXML xml = fo.GetDefaultXMLFilter(myPubID, catIDs, transIDs);

                if (c != null)
                {
                    Telerik.Reporting.ReportSource rptSource = ReportUtilities.GetReportSource(c, rpt, countries, xml, myProduct.PubID, myProduct.PubName, 0, myIssue.IssueName);

                    rvReport.ReportSource = rptSource;
                    rvReport.RefreshReport();
                }
            }
        }
        #endregion

        #region Helper Methods
        private static List<int> getNth(int TotalRecords, int RequestedRecords)
        {
            List<int> listNth = new List<int>();

            if (RequestedRecords == 0)
                RequestedRecords = TotalRecords;

            double inccounter = (double)TotalRecords / RequestedRecords;

            double y = inccounter;

            for (; Math.Round(y, 2) <= TotalRecords; y = (y + inccounter))
            {
                listNth.Add(Convert.ToInt32(y - 1));

                if (listNth.Count == RequestedRecords)
                {
                    break;
                }
            }
            return listNth;
        }
        private static List<int> GetNthSubscribers(int totalRecords, int requestedRecords, List<int> idSourceList)
        {
            List<int> LNth = getNth(totalRecords, requestedRecords);
            List<int> ids = new List<int>();

            foreach (int n in LNth)
            {
                var id = idSourceList.Skip(n).Take(1);
                ids.Add(id.ToList().FirstOrDefault());
            }

            return ids;
        }
        private void UpdateCurrentTotals()
        {
            MyTotals.CurrentFreeCopies = 0;
            MyTotals.CurrentFreeRecords = 0;
            MyTotals.CurrentPaidCopies = 0;
            MyTotals.CurrentPaidRecords = 0;
            activeSubsList.ForEach(x =>
            {
                if (x.CategoryType == FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Free.ToString().Replace("_", " "))
                {
                    MyTotals.CurrentFreeRecords++;
                    MyTotals.CurrentFreeCopies += x.Copies;
                }
                else if (x.CategoryType == FrameworkUAD_Lookup.Enums.CategoryCodeType.Qualified_Paid.ToString().Replace("_", " "))
                {
                    MyTotals.CurrentPaidRecords++;
                    MyTotals.CurrentPaidCopies += x.Copies;
                }
            });
            UpdateNewTotals();
        }
        private void UpdateNewTotals()
        {
            MyTotals.NewFreeCopies = MyTotals.CurrentFreeCopies;
            MyTotals.NewFreeRecords = MyTotals.CurrentFreeRecords;
            MyTotals.NewPaidRecords = MyTotals.CurrentPaidRecords;
            MyTotals.NewPaidCopies = MyTotals.CurrentPaidCopies;
            foreach (AddRemoveDetail item in AddRemoves)
            {
                if (item.Type == AddRemoveType.Add)
                {
                    MyTotals.NewFreeCopies += item.ChangedFreeCopies;
                    MyTotals.NewFreeRecords += item.ChangedFreeRecords;
                    MyTotals.NewPaidRecords += item.ChangedPaidRecords;
                    MyTotals.NewPaidCopies += item.ChangedPaidCopies;
                }
                else
                {
                    MyTotals.NewFreeCopies -= item.ChangedFreeCopies;
                    MyTotals.NewFreeRecords -= item.ChangedFreeRecords;
                    MyTotals.NewPaidRecords -= item.ChangedPaidRecords;
                    MyTotals.NewPaidCopies -= item.ChangedPaidCopies;
                }
            }
            grdAddRemoveDetail.CalculateAggregates();
            //grdAddRemoveDetail.Columns[3].AggregateFunctions.Clear();
            //grdAddRemoveDetail.Columns[3].AggregateFunctions.Add(new MyTotalFunction());
        }
        #endregion

        private void PreviewAddKill_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker bw = new BackgroundWorker();
            busy.IsBusy = true;
            busy.BusyContent = "Loading...";
            busy.ProgressValue = 0;
            busy.IsIndeterminate = false;
            bw.DoWork += (o, ea) =>
            {
                boolResponse = subAddKillData.Proxy.ClearDetails(accessKey, myPubID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                if(boolResponse.Status != FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                    Core_AMS.Utilities.WPF.Message("There was an issue loading the preview data.");

                foreach (AddRemoveDetail ard in AddRemoves)
                {
                    int addRemoveID = 0;
                    FrameworkUAD.Entity.SubscriberAddKill s = new FrameworkUAD.Entity.SubscriberAddKill();
                    s.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                    s.PublicationID = myPubID;
                    s.AddKillCount = ard.DesiredCount;
                    s.Count = ard.ActualCount;
                    s.Type = ard.Type.ToString();

                    //addRemoveID = subAddKillData.Proxy.Save(accessKey, s, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
                    //ard.AddKillID = addRemoveID;

                    List<FrameworkUAD.Entity.SubscriberAddKillDetail> details = new List<SubscriberAddKillDetail>();

                    if(ard.Type == AddRemoveType.Add)
                    {
                        foreach (int i in ard.PendingSubscriptionList)
                            details.Add(new SubscriberAddKillDetail(addRemoveID, addCatCode, addTransCode, i));
                    }
                    else
                    {
                        foreach (int i in ard.PendingSubscriptionList)
                        {
                            int cat = 0;
                            FrameworkUAD.Entity.ActionProductSubscription a = subscriptionList.Where(x => x.PubSubscriptionID == i).FirstOrDefault();
                            if (a != null)
                                cat = a.PubCategoryID;
                            details.Add(new SubscriberAddKillDetail(addRemoveID, cat, killTransCode, i));
                        }
                    }

                    boolResponse = subAddKillData.Proxy.BulkInsertDetails(accessKey, details, addRemoveID,
                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                    if(boolResponse.Result == false || boolResponse.Status != FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                        Core_AMS.Utilities.WPF.Message("There was an issue loading the preview data.");
                }
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                busy.IsBusy = false;
                Core_AMS.Utilities.WPF.Message("Add Remove preview data has finished loading. Please use the 'Include Add Removes' parameter on the desired reports to view" + 
                                            " how the preview data will affect your reports.");
            };
            bw.RunWorkerAsync();
        }
    }
}
