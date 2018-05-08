using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core_AMS.Utilities;
using FrameworkUAD.Entity;
using FrameworkUAS.Entity;
using KM.Common.Functions;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using WpfControls.Helpers;
using KeyLineComputation = KM.Common.Functions.KeyLineComputation;

namespace WpfControls
{
    /// <summary>
    /// Interaction logic for IssueSplitDataFetcher.xaml
    /// </summary>
    public partial class IssueSplitDataFetcher : UserControl
    {
        #region ServiceCalls
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IAcsMailerInfo> mailerInfoData = FrameworkServices.ServiceClient.UAD_AcsMailerInfoClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.IAction> actionData = FrameworkServices.ServiceClient.UAD_Lookup_ActionClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IIssueArchiveProductSubscription> archSubscriptionData = FrameworkServices.ServiceClient.UAD_IssueArchiveProductSubscriptionClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IIssueArchiveProductSubscriptionDetail> archSubResponseMapData = FrameworkServices.ServiceClient.UAD_IssueArchiveProductSubscriptionDetailClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICategoryCode> catCodeData = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICategoryCodeType> catCodeTypeData = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeData = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICodeType> codeTypeData = FrameworkServices.ServiceClient.UAD_Lookup_CodeTypeClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IFilter> filterData = FrameworkServices.ServiceClient.UAS_FilterClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IFilterDetail> filterDetailData = FrameworkServices.ServiceClient.UAS_FilterDetailClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IFilterDetailSelectedValue> filterDetailValuesData = FrameworkServices.ServiceClient.UAS_FilterDetailSelectedValueClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IIssueComp> issueCompData = FrameworkServices.ServiceClient.UAD_IssueCompClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IIssueCompDetail> issueCompDetailData = FrameworkServices.ServiceClient.UAD_IssueCompDetailClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IIssue> issueData = FrameworkServices.ServiceClient.UAD_IssueClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> productData = FrameworkServices.ServiceClient.UAD_ProductClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> publisherData = FrameworkServices.ServiceClient.UAS_ClientClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IReports> reportData = FrameworkServices.ServiceClient.UAD_ReportsClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IIssueSplit> splitData = FrameworkServices.ServiceClient.UAD_IssueSplitClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.ISubscriberAddKill> subAddKillData = FrameworkServices.ServiceClient.UAD_SubscriberAddKillClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IPubSubscriptionDetail> psdWorker = FrameworkServices.ServiceClient.UAD_PubSubscriptionDetailClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProductSubscription> productSubscriptionWorker = FrameworkServices.ServiceClient.UAD_ProductSubscriptionClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ITransactionCode> transCodeData = FrameworkServices.ServiceClient.UAD_Lookup_TransactionCodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ITransactionCodeType> transCodeTypeData = FrameworkServices.ServiceClient.UAD_Lookup_TransactionCodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IWaveMailing> waveMailingData = FrameworkServices.ServiceClient.UAD_WaveMailingClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IWaveMailingDetail> wMDetailData = FrameworkServices.ServiceClient.UAD_WaveMailingDetailClient();
        #endregion
        #region ServiceResponse
        private FrameworkUAS.Service.Response<AcsMailerInfo> mailerInfoResponse = new FrameworkUAS.Service.Response<AcsMailerInfo>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Action>> actionResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Action>>();
        private FrameworkUAS.Service.Response<List<IssueArchiveProductSubscription>> archSubscriptionResponse = new FrameworkUAS.Service.Response<List<IssueArchiveProductSubscription>>();
        private FrameworkUAS.Service.Response<List<IssueArchiveProductSubscriptionDetail>> archsrmResponse = new FrameworkUAS.Service.Response<List<IssueArchiveProductSubscriptionDetail>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>> catCodeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCodeType>> catCodeTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCodeType>>();
        private FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>> clientResponse = new FrameworkUAS.Service.Response<List<KMPlatform.Entity.Client>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> codeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CodeType>> codeTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CodeType>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.CopiesProductSubscription>> copiesProductSubResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.CopiesProductSubscription>>();
        private FrameworkUAS.Service.Response<FrameworkUAD.Object.Counts> countsResponse = new FrameworkUAS.Service.Response<FrameworkUAD.Object.Counts>();
        private FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.Filter>> filterResponse = new FrameworkUAS.Service.Response<List<FrameworkUAS.Entity.Filter>>();
        private FrameworkUAS.Service.Response<List<FilterDetail>> filterDetailResponse = new FrameworkUAS.Service.Response<List<FilterDetail>>();
        private FrameworkUAS.Service.Response<List<FilterDetailSelectedValue>> filterDetailSelectedValuedResponse = new FrameworkUAS.Service.Response<List<FilterDetailSelectedValue>>();
        private FrameworkUAS.Service.Response<int> intResponse = new FrameworkUAS.Service.Response<int>();
        private FrameworkUAS.Service.Response<List<IssueComp>> issueCompResponse = new FrameworkUAS.Service.Response<List<IssueComp>>();
        private FrameworkUAS.Service.Response<List<IssueCompDetail>> issueCompDetailResponse = new FrameworkUAS.Service.Response<List<IssueCompDetail>>();
        private FrameworkUAS.Service.Response<List<Issue>> issueResponse = new FrameworkUAS.Service.Response<List<Issue>>();
        private FrameworkUAS.Service.Response<FrameworkUAD.Entity.Product> productResponse = new FrameworkUAS.Service.Response<FrameworkUAD.Entity.Product>();
        private FrameworkUAS.Service.Response<List<IssueSplit>> splitResponse = new FrameworkUAS.Service.Response<List<IssueSplit>>();
        private FrameworkUAS.Service.Response<List<SubscriberAddKill>> subAddKillResponse = new FrameworkUAS.Service.Response<List<SubscriberAddKill>>();
        private FrameworkUAS.Service.Response<List<int>> subCountResponse = new FrameworkUAS.Service.Response<List<int>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> qualResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private FrameworkUAS.Service.Response<List<ProductSubscriptionDetail>> psdResponse = new FrameworkUAS.Service.Response<List<ProductSubscriptionDetail>>();
        private FrameworkUAS.Service.Response<List<ProductSubscription>> prodSubResponse = new FrameworkUAS.Service.Response<List<ProductSubscription>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>> transCodeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCodeType>> transCodeTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCodeType>>();
        private FrameworkUAS.Service.Response<List<WaveMailing>> waveMailingResponse = new FrameworkUAS.Service.Response<List<WaveMailing>>();
        private FrameworkUAS.Service.Response<List<WaveMailingDetail>> wmDetialResponse = new FrameworkUAS.Service.Response<List<WaveMailingDetail>>();
        #endregion
        #region Variables/Lists
        private Guid accessKey;
        private RadBusyIndicator busy;
        private KMPlatform.Entity.Client myClient = new KMPlatform.Entity.Client();
        private FrameworkUAD.Entity.Product myProduct = new FrameworkUAD.Entity.Product();
        private FrameworkUAD.Entity.Issue myIssue = new Issue();
        private IssueComp myComp = new IssueComp();
        private AcsMailerInfo myAcsMailer = new AcsMailerInfo();
        private FilterOperations fo = new FilterOperations();

        private List<IssueSplit> splits = new List<IssueSplit>();
        private List<int> remainingPool = new List<int>();
        private List<int> remainingComps = new List<int>();
        //private Dictionary<int, int> subs = new Dictionary<int, int>();
        private List<int> compSubIDs = new List<int>();
        private List<CopiesProductSubscription> subscriptionList = new List<CopiesProductSubscription>();

        private List<IssueCompDetail> comps = new List<IssueCompDetail>();
        private List<FrameworkUAD.Entity.ProductSubscriptionDetail> subscriptionResponseList = new List<FrameworkUAD.Entity.ProductSubscriptionDetail>();
        private List<FrameworkUAS.Entity.Filter> filterList = new List<FrameworkUAS.Entity.Filter>();

        private List<FrameworkUAD_Lookup.Entity.Action> actionList = new List<FrameworkUAD_Lookup.Entity.Action>();
        private List<FrameworkUAD_Lookup.Entity.CategoryCodeType> catCodeTypeList = new List<FrameworkUAD_Lookup.Entity.CategoryCodeType>();
        private List<FrameworkUAD_Lookup.Entity.CategoryCode> catCodeList = new List<FrameworkUAD_Lookup.Entity.CategoryCode>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCodeType> tranCodeTypeList = new List<FrameworkUAD_Lookup.Entity.TransactionCodeType>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCode> tranCodeList = new List<FrameworkUAD_Lookup.Entity.TransactionCode>();
        private List<FrameworkUAD_Lookup.Entity.Code> filterTypes = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<string> filterNameList = new List<string>();
        private Dictionary<int, string> qualDict = new Dictionary<int, string>();
        private HashSet<int> activeTransactionCodes = new HashSet<int>();
        private HashSet<int> activePaidTransactionCodes = new HashSet<int>();
        private HashSet<int> activeActions = new HashSet<int>();
        private HashSet<int> activePaidActions = new HashSet<int>();
        private HashSet<int> activeCats = new HashSet<int>();
        private HashSet<int> activePaidCats = new HashSet<int>();
        private HashSet<int> activeSubscriptionIDs = new HashSet<int>();

        private ObservableCollection<IssueSplitContainer> splitsList = new ObservableCollection<IssueSplitContainer>();
        private ObservableCollection<IssueSplitContainer> filterCollection = new ObservableCollection<IssueSplitContainer>();
        private Dictionary<int, string> imbSeqs = new Dictionary<int, string>();
        private Dictionary<int, string> compImbSeqs = new Dictionary<int, string>();

        private int myIssueID = -1;
        private int myProductID = -1;
        private int myIssueCompID = -1;
        private int groupID = -1;
        private int tempSubID = -1;
        private int waveNumber = 0;
        private int maxIMB = 0;
        private RadGridView grdExporting = new RadGridView();
        private FrameworkUAD.Object.Counts myCounts;
        #endregion

        #region Classes
        //IssueSplitContainer holds all various information about a Split that is housed in a DataGrid.
        private class IssueSplitContainer : INotifyPropertyChanged
        {
            public IssueSplitContainer(IssueSplit split, List<FilterControls.Framework.FilterObject> filters, List<int> subscriberIDs, int productID)
            {
                this.IssueSplitId = split.IssueSplitId;
                this.IssueId = split.IssueId;
                this.IssueSplitCode = split.IssueSplitCode;
                this.IssueSplitName = split.IssueSplitName;
                this.IssueSplitCount = split.IssueSplitCount;
                this.RecordCount = subscriberIDs.Count;
                this.IssueSplit = split;
                this.FilterId = split.FilterId;
                this.KeyCode = split.KeyCode;
                this.IsActive = split.IsActive;
                this.Filters = filters;
                this.SubscriberIDs = subscriberIDs;
                this.DesiredCount = split.IssueSplitCount;
                this.IssueSplitDescription = split.IssueSplitDescription;
                this.IssueSplitRecords = split.IssueSplitRecords;
                this.ProductID = productID;
                this.NotExported = true;
                this.ChildSplits = new ObservableCollection<SplitChildContainer>();
            }
            public int IssueSplitId { get; set; }
            public int IssueId { get; set; }
            public string IssueSplitCode { get; set; }
            private string _IssueSplitName;
            private string _issueSplitDescription;
            public string IssueSplitName
            {
                get { return _IssueSplitName; }
                set
                {
                    _IssueSplitName = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IssueSplitName"));
                    }
                }
            }
            public string IssueSplitDescription
            {
                get { return _issueSplitDescription; }
                set
                {
                    _issueSplitDescription = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IssueSplitDescription"));
                    }
                }
            }
            public int IssueSplitCount { get; set; }
            public int IssueSplitRecords { get; set; }
            public int RecordCount { get; set; }
            public int FilterId { get; set; }
            public int ProductID { get; set; }
            private string _KeyCode;
            public string KeyCode
            {
                get { return _KeyCode; }
                set
                {
                    _KeyCode = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("KeyCode"));
                    }
                }
            }
            public bool IsActive { get; set; }
            private bool _NotExported;
            //After you export, NotExported is set to false. This controls additional code if you try to Uncheck an exported Split.
            public bool NotExported
            {
                get { return _NotExported; }
                set
                {
                    _NotExported = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("NotExported"));
                    }
                }
            }
            private bool _Save;
            //Save is a bound field to a CheckBox that states whether you want to Export the file or not.
            public bool Save
            {
                get { return _Save; }
                set
                {
                    _Save = value;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Save"));
                    }
                }
            }
            private int _DesiredCount;
            public int DesiredCount
            {
                get { return _DesiredCount; }
                set
                {
                    _DesiredCount = value;
                    if (_DesiredCount > this.SubscriberIDs.Count && _DesiredCount <= this.SubscriberIDs.Count)
                    {
                        //_DesiredCount = this.SubscriberIDs.Count;
                        //MessageBox.Show("You only have " + _DesiredCount.ToString() + " records available for this split. Please delete the moved records from other splits to reapply them to this split.");
                    }
                    else if (_DesiredCount > this.SubscriberIDs.Count)
                        _DesiredCount = this.SubscriberIDs.Count;
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("DesiredCount"));
                    }
                }
            }
            public IssueSplit IssueSplit { get; set; }
            public List<FilterControls.Framework.FilterObject> Filters { get; set; }
            public ObservableCollection<SplitChildContainer> ChildSplits { get; set; }
            public List<int> SubscriberIDs { get; set; }
            public IssueSplitReport GetReportEntity()
            {
                IssueSplitReport report = new IssueSplitReport(_IssueSplitName, _issueSplitDescription, _KeyCode, SubscriberIDs.Count, this.IssueSplitCount);

                return report;
            }
            public FrameworkUAD.Entity.IssueSplit GetIssueSplitEntity()
            {
                Core_AMS.Utilities.JsonFunctions jf = new JsonFunctions();
                IssueSplit i = new IssueSplit();
                i.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                i.DateCreated = DateTime.Now;
                i.FilterId = this.FilterId;
                i.IsActive = this.IsActive;
                i.IssueId = this.IssueId;
                i.IssueSplitCode = this.IssueSplitCode;
                i.IssueSplitCount = this.IssueSplitCount;
                i.IssueSplitRecords = this.IssueSplitRecords;
                i.IssueSplitDescription = this.IssueSplitDescription;
                i.IssueSplitName = this.IssueSplitName;
                i.KeyCode = this.KeyCode;

                FrameworkUAD.Entity.Filter filter = new FrameworkUAD.Entity.Filter();
                filter.FilterName = this.IssueSplitName;
                filter.FilterDetails = jf.ToJson<List<FilterControls.Framework.FilterObject>>(this.Filters);
                filter.ProductID = this.ProductID;
                filter.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                filter.DateCreated = DateTime.Now;

                return i;
            }
            public event PropertyChangedEventHandler PropertyChanged;
        }
        //SplitChildContainer is a list of Subscribers that you move from one Split to another. It contains info about destination and origin Splits.
        private class SplitChildContainer
        {
            public SplitChildContainer(string name, string parentName, string desc, int count, List<int> ids)
            {
                this.SplitName = name;
                this.SplitDescription = desc;
                this.SplitParent = parentName;
                this.SplitCount = count;
                this.SubscriberIDs = ids;
            }
            public string SplitName { get; set; }
            public string SplitDescription { get; set; }
            public int SplitCount { get; set; }
            public string SplitParent { get; set; }
            public List<int> SubscriberIDs { get; set; }
        }
        private class FilterCombinations
        {
            public FilterCombinations(List<int> criteria)
            {
                this.Criteria = criteria;
            }

            public List<int> Criteria { get; set; }
        }
        public class IssueSplitReport
        {
            public string SplitName { get; set; }
            public string SplitDescription { get; set; }
            public string KeyCode { get; set; }
            public int Records { get; set; }
            public int Copies { get; set; }

            public IssueSplitReport(string name, string description, string keyCode, int records, int copies)
            {
                this.SplitName = name;
                this.SplitDescription = description;
                this.KeyCode = keyCode;
                this.Records = records;
                this.Copies = copies;
            }
        }
        #endregion

        public IssueSplitDataFetcher(int issueID, FrameworkUAD.Entity.Product prod)
        {
            InitializeComponent();
            accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
            myIssueID = issueID;
            myProduct = prod;
            myProductID = prod.PubID;
            LoadData();
            SetControls();
        }

        #region Initial Control Loading
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UserControl uc = this.ParentOfType<UserControl>();
            busy = uc.FindChildByType<RadBusyIndicator>();
        }
        public void LoadData()
        {
            myClient = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient;

            codeResponse = codeData.Proxy.Select(accessKey, FrameworkUAD_Lookup.Enums.CodeType.Filter_Group);
            if (Helpers.Common.CheckResponse(codeResponse.Result, codeResponse.Status))
            {
                FrameworkUAD_Lookup.Entity.Code code = codeResponse.Result.Where(x => x.CodeName.Replace(" ", "").Trim() == FrameworkUAD_Lookup.Enums.FilterGroupTypes.Issue_Splits.ToString().Replace("_", "").Trim()).FirstOrDefault();
                groupID = code.CodeId;
            }

            productResponse = productData.Proxy.Select(accessKey, myProductID, myClient.ClientConnections);
            if (productResponse.Result != null)
            {
                FrameworkUAD.Entity.Product pub = productResponse.Result;
                myProductID = pub.PubID;
            }

            mailerInfoResponse = mailerInfoData.Proxy.SelectByID(accessKey, myProduct.AcsMailerInfoId, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if (mailerInfoResponse.Result != null && mailerInfoResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            {
                if (mailerInfoResponse.Result.ImbSeqCounter == 0)
                    maxIMB = 1;
                else
                    maxIMB = mailerInfoResponse.Result.ImbSeqCounter;
                myAcsMailer = mailerInfoResponse.Result;
            }

            waveMailingResponse = waveMailingData.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if (Common.CheckResponse(waveMailingResponse.Result, waveMailingResponse.Status))
                waveNumber = waveMailingResponse.Result.Where(x=> x.IssueID == myIssueID).Count();

            filterResponse = filterData.Proxy.Select(accessKey, myProductID);
            if (Common.CheckResponse(filterResponse.Result, filterResponse.Status))
            {
                filterList = filterResponse.Result.Where(x => x.IsActive == true).ToList();
                filterNameList = filterList.Select(x=> x.FilterName).ToList();
            }

            countsResponse = reportData.Proxy.SelectIssueSplitsActiveCounts(accessKey, myProductID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if (countsResponse.Result != null && countsResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            {
                myCounts = countsResponse.Result;
                qualDict = new Dictionary<int, string>(myCounts.FreeRecords + myCounts.PaidRecords);
                imbSeqs = new Dictionary<int, string>(myCounts.FreeRecords + myCounts.PaidRecords);
                compImbSeqs = new Dictionary<int, string>(myCounts.FreeRecords + myCounts.PaidRecords);
            }

            qualResponse = codeData.Proxy.Select(accessKey, FrameworkUAD_Lookup.Enums.CodeType.Qualification_Source);
            if(Helpers.Common.CheckResponse(qualResponse.Result, qualResponse.Status))
            {
                qualDict = qualResponse.Result.ToDictionary(x => x.CodeId, x => x.CodeValue);
            }

            copiesProductSubResponse = productSubscriptionWorker.Proxy.SelectAllActiveIDs(accessKey, myProductID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if(Helpers.Common.CheckResponse(copiesProductSubResponse.Result, copiesProductSubResponse.Status))
            {
                subscriptionList = copiesProductSubResponse.Result;
            }

            remainingPool = new List<int>(subscriptionList.Select(x=> x.PubSubscriptionID));

            issueResponse = issueData.Proxy.SelectForPublication(accessKey, myProductID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if(Helpers.Common.CheckResponse(issueResponse.Result, issueResponse.Status))
            {
                myIssue = issueResponse.Result.Where(x => x.IsComplete == false && x.PublicationId == myProductID).FirstOrDefault();
            }

            issueCompResponse = issueCompData.Proxy.SelectIssue(accessKey, myIssueID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if (Common.CheckResponse(issueCompResponse.Result, issueCompResponse.Status))
            {
                if (issueCompResponse.Result.Count > 0)
                {
                    int id = issueCompResponse.Result.Where(x=> x.IsActive == true).LastOrDefault().IssueCompId;
                    myComp = issueCompResponse.Result.Where(x => x.IsActive == true).FirstOrDefault();
                    myIssueCompID = id;
                    issueCompDetailResponse = issueCompDetailData.Proxy.Select(accessKey, id, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                    if (Common.CheckResponse(issueCompDetailResponse.Result, issueCompDetailResponse.Status))
                    {
                        tempSubID = remainingPool.Max();
                        tempSubID += 500000;
                        comps = issueCompDetailResponse.Result;

                        foreach (IssueCompDetail comp in comps)
                        {
                            ProductSubscription sp = new ProductSubscription(comp);
                            tempSubID++;
                            sp.PubSubscriptionID = tempSubID;
                            comp.PubSubscriptionID = tempSubID;
                            //allProductSubscriptions.Add(sp);
                            subscriptionList.Add(new CopiesProductSubscription() { PubSubscriptionID = tempSubID, Copies = comp.Copies});
                            //activeSubscriptionIDs.Add(tempSubID);
                            remainingPool.Add(tempSubID);
                            compSubIDs.Add(tempSubID);
                        }
                    }
                }
            }

            activeSubscriptionIDs = new HashSet<int>(subscriptionList.Select(x => x.PubSubscriptionID));

            #region OLD - If Users decide they want to save and re-use old splits
            //splitResponse = splitData.Proxy.SelectForIssueID(accessKey, issueID);
            //if (Common.CheckResponse(splitResponse.Result, splitResponse.Status))
            //{
            //    splits = splitResponse.Result.Where(x => x.IsActive == true).ToList();
            //    foreach(IssueSplit s in splits)
            //    {
            //        List<int> subIDs = new List<int>();
            //        Helpers.FilterOperations.FilterContainer fc = fo.GetFilterContainer(myAppData, s.FilterId, myProductID);
            //        FrameworkUAD.Object.Reporting obj = fo.GetReportingObjectFromContainer(fc, myProductID);
            //        subCountResponse = reportData.Proxy.SelectSubscriberCount(accessKey, obj, true);
            //        if (Common.CheckResponse(subCountResponse.Result, subCountResponse.Status))
            //        {
            //            subIDs = subCountResponse.Result.Where(x => activeSubscriberIDs.Contains(x)).ToList();
            //        }
            //        ObservableCollection<Helpers.FilterOperations.DisplayedFilterDetail> details = fo.GetDisplayFilterDetail(myAppData, fc);
            //        splitsList.Add(new IssueSplitContainer(s, fc, details, subIDs));
            //    }
            //}

            //foreach (IssueSplit split in splits)
            //{
            //    List<int> subIDs = new List<int>();
            //    FrameworkUAD.Object.Reporting r = fo.GetReportingObjectFromFilter(myAppData, split.FilterId, myProductID);
            //    subCountResponse = reportData.Proxy.SelectSubscriberCount(accessKey, r, true);                    
            //    if (Common.CheckResponse(subCountResponse.Result, subCountResponse.Status))
            //        subIDs = subCountResponse.Result.Where(x => activeSubscriberIDs.Contains(x)).ToList();

            //    remainingPool = remainingPool.Except(subIDs).ToList();
            //}
            #endregion
        }
        public void SetControls()
        {
            int subCount = 0;

            btnGenerateSplit.DataContext = this;
            subCount = subscriptionList.Select(x => x.Copies).Sum();
            int compCopies = comps.Select(x => x.Copies).Sum();

            grdSplitInfo.ItemsSource = splitsList;
            grdFilterInfo.ItemsSource = filterCollection;

            txtCurrent.Text = myCounts.FreeCopies.ToString();
            txtCurrentPaid.Text = myCounts.PaidCopies.ToString();
            txtCurrentPaidRecords.Text = myCounts.PaidRecords.ToString();
            txtCurrentRecords.Text = myCounts.FreeRecords.ToString();
            txtCompsRecords.Text = comps.Count.ToString();
            txtComps.Text = compCopies.ToString();

            List<string> extensions = new List<string>() { "PDF", "CSV" };
            rcbExtensions.ItemsSource = extensions;

            //COMMENTED OUT PLACED INTO A TEXTBLOCK DOESNT LIKE TO UPDATE ONCE ADDED ASSUMING A PROPERTYCHANGED ISSUE??? -JASON
            ////Creates an aggregate column that tells the user how many unique Subscribers have already been used in Splits (and how many remain in the available pool).
            //var aggregate = new AggregateFunction<IssueSplitContainer, int>()
            //{
            //    AggregationExpression = x =>
            //    subCount - x.Select(z => z.IssueSplitCount).Sum(),
            //    Caption = "Remaining: "
            //};
            //CopiesColumn.AggregateFunctions.Add(aggregate);
            ////grdSplitInfo.Columns[4].AggregateFunctions.Add(aggregate);
            if (busy != null)
                busy.IsBusy = false;
        }
        #endregion

        #region UI Events

        private void btnAddCriteria_Click(object sender, RoutedEventArgs e)
        {
            tabFilters.IsSelected = true;
            List<FilterControls.Framework.FilterObject> filters = new List<FilterControls.Framework.FilterObject>();
            Window w = Window.GetWindow(this);

            FilterControls.FilterWrapper fw = w.FindChildByType<FilterControls.FilterWrapper>();
            FilterControls.Framework.FiltersViewModel vm = fw.MyViewModel;

            vm.ActiveFilters.ToList().ForEach(x => filters.Add(x.DeepClone()));

            IssueSplit split = new IssueSplit();

            Enums.DialogResponses? response = null;
            string name = "";
            bool notExisting = false;
            while(!notExisting)
            {
                SaveCancelTextBoxDialog dialog = new SaveCancelTextBoxDialog("Confirmation", "Please enter a name for this Filter.");
                dialog.Answer += value => response = value;
                dialog.filterNameAnswer += value => name = value;
                dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                dialog.Owner = w;
                dialog.ShowDialog();
                if (response == Enums.DialogResponses.Cancel)
                {
                    dialog.Close();
                    return;
                }
                else if (response == Enums.DialogResponses.Save)
                {
                    if (!filterNameList.Contains(name))
                        notExisting = true;
                    else
                        Core_AMS.Utilities.WPF.Message("That filter name already exists. Please enter a different one.");
                }
            }
            int count = 0;
            List<int> subIDs = new List<int>();
            BackgroundWorker bw = new BackgroundWorker();
            busy.IsBusy = true;
            bw.DoWork += (o, ea) =>
            {
                FrameworkUAD.Object.ReportingXML obj = vm.ActiveFiltersXML;
                subCountResponse = reportData.Proxy.SelectSubscriberCount(accessKey, obj.Filters, obj.AdHocFilters, false, false, 0, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                if (Common.CheckResponse(subCountResponse.Result, subCountResponse.Status))
                {
                    subIDs = subCountResponse.Result.Where(x => activeSubscriptionIDs.Contains(x)).ToList();
                }
                if (comps.Count > 0 && !obj.Filters.Contains("Responses") && !obj.AdHocFilters.Contains("<FilterObjectType>AdHoc</FilterObjectType>")) //IssueCompDetails can not have Response or PubSubExtension data.
                {
                    subCountResponse = issueCompDetailData.Proxy.GetByFilter(accessKey, obj.Filters, obj.AdHocFilters, myIssueCompID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                    if (Common.CheckResponse(subCountResponse.Result, subCountResponse.Status))
                    {
                        List<int> temp = subCountResponse.Result;
                        List<int> ids = comps.Where(x => temp.Contains(x.IssueCompDetailId)).Select(x => x.PubSubscriptionID).ToList();
                        subIDs.AddRange(ids);
                    }
                }
                count = subscriptionList.Where(x => subIDs.Contains(x.PubSubscriptionID)).Select(x => x.Copies).Sum();
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                if (count > 0)
                {
                    split.IssueSplitCount = count;
                    split.IssueSplitName = name;
                    filterCollection.Add(new IssueSplitContainer(split, filters, subIDs, myProductID));
                }
                else
                    MessageBox.Show("No results to display.");

                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();
        }

        private void Generate_Splits(object sender, RoutedEventArgs e)
        {
            splitsList.Clear();
            List<int> dictionaryKeys = new List<int>();
            int counter = 0;
            ObservableCollection<IssueSplitContainer> tempSplitsList = new ObservableCollection<IssueSplitContainer>();
            Dictionary<int, IssueSplitContainer> filterDictionary = new Dictionary<int, IssueSplitContainer>();
            List<int> subPool = new List<int>();
            List<int> totalPool = new List<int>();

            BackgroundWorker bw = new BackgroundWorker();
            busy.IsBusy = true;
            busy.IsIndeterminate = true;
            Boolean error = false;
            bw.DoWork += (ev,o) =>
            {
                try
                {
                    foreach (IssueSplitContainer isc in filterCollection)
                    {
                        filterDictionary.Add(counter, isc);
                        dictionaryKeys.Add(counter);
                        subPool.AddRange(isc.SubscriberIDs);
                        counter++;
                    }
                    subPool = subPool.Distinct().ToList();
                    totalPool = subPool.Distinct().ToList();
                    //Finds all possible combinations between a number of objects.
                    List<FilterCombinations> combos = GetCombination(dictionaryKeys);
                    //Given all possible combinations, we see if there is any intersection between SubscriberIDs in those combinations.
                    //Example: With 2 splits, we have 3 possible combinations: 1, 2, 1 + 2
                    //Given those combinations, starting with the largest combination (1 + 2), we find all possible SubscriberIDs that intersect and remove them from remaining combinations as we
                    //work our way down the list.
                    foreach (FilterCombinations fc in combos)
                    {
                        List<FilterControls.Framework.FilterObject> filters = new List<FilterControls.Framework.FilterObject>();
                        List<int> subscriberPool = subPool;
                        List<int> subscriberTotalPool = totalPool;
                        String name = "";
                        foreach (int i in fc.Criteria)
                        {
                            IssueSplitContainer ctnr = filterDictionary[i];
                            filters.AddRange(ctnr.Filters);
                            name += ctnr.IssueSplitName + " + ";
                            subscriberPool = subscriberPool.Intersect(ctnr.SubscriberIDs).ToList();
                            subscriberTotalPool = subscriberTotalPool.Intersect(ctnr.SubscriberIDs).ToList();
                        }
                        name = name.TrimEnd(' ');
                        name = name.TrimEnd('+');
                        name = name.TrimEnd(' ');
                        subPool = subPool.Except(subscriberPool).ToList();
                        if (subscriberPool.Count > 0)
                        {
                            List<FilterControls.Framework.FilterObject> finalFilters = GetCombinedDetail(filters);
                            IssueSplit newSplit = new IssueSplit();
                            int count = subscriptionList.Where(x => subscriberPool.Contains(x.PubSubscriptionID)).Select(x => x.Copies).Sum();
                            newSplit.IssueSplitCount = count;
                            newSplit.IssueSplitRecords = subscriberPool.Count;
                            newSplit.IssueSplitId = 0;
                            newSplit.IsActive = true;
                            newSplit.IssueId = myIssueID;
                            newSplit.IssueSplitName = name;
                            IssueSplitContainer newContainer = new IssueSplitContainer(newSplit, finalFilters, subscriberPool, myProductID);
                            tempSplitsList.Add(newContainer);
                        }
                    }
                }
                catch (Exception ex)
                {
                    error = true;
                    MessageBox.Show("System Resource Issue:  Too many Issue Splits created to process successfully.   Reduce the number of Splits and use the Wave Mailing process in order to properly finalize your Issue.");
                    FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
                    Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
                    KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                    int logClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
                    string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                    alClient.Proxy.LogCriticalError(accessKey, formatException, this.GetType().Name.ToString() + ".IssueSplits", app, string.Empty, logClientId);
                }
            };
            bw.RunWorkerCompleted += (ev, o) =>
            {
                busy.IsBusy = false;
                foreach(IssueSplitContainer isc in tempSplitsList)
                {
                    splitsList.Add(isc);
                }
                //List<FrameworkUAD.Entity.IssueSplit> splits = new List<IssueSplit>();
                //splitsList.ToList().ForEach(x => splits.Add(x.GetIssueSplitEntity()));
                //splitData.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, splits, myIssueID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                tabSplits.IsSelected = true;
                productSubscriptionWorker.Proxy.UpdateRequesterFlags(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, myProductID, myIssueID,
                    FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);

                int subCount = 0;                
                subCount = subscriptionList.Select(x => x.Copies).Sum();

                int remaining = subCount - splitsList.Select(x => x.IssueSplitCount).Sum();
                txtRemaining.Text = "Remaining: " + remaining.ToString();
                if (!error)
                {
                    MessageBox.Show("Splits generated successfully.");
                }
                    
            };
            bw.RunWorkerAsync();
        }

        private void btnFinalizeIssue_Click(object sender, RoutedEventArgs e)
        {
            int origIMB = myAcsMailer.ImbSeqCounter;
            RadButton me = sender as RadButton;
            #region Finalize Issue
            if (me.Content.ToString() == "Finalize Issue")
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to finalize this issue? This will automatically open the next issue.", "Finalize Issue", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    BackgroundWorker bw = new BackgroundWorker();
                    if (busy != null)
                    {
                        busy.IsIndeterminate = false;
                        busy.IsBusy = true;
                    }
                    bw.DoWork += (o, ea) =>
                    {
                        //Archive the Splits and Filters used; Update IMBSEQ #'s; Archive all PubSubscription, PubSubscriptionDetail, and AdHocs for this Product; Update records that were changed in
                        //a Wave Mailing.
                        try
                        {
                            Save_Splits_And_Filters();
                            UpdateAcsMailerInfo();
                            ArchiveAll();
                            UpdateOriginalRecords();
                        }
                        catch (Exception ex)
                        {
                            FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
                            Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
                            KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                            int logClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
                            string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                            alClient.Proxy.LogCriticalError(accessKey, formatException, this.GetType().Name.ToString() + ".IssueSplits", app, string.Empty, logClientId);
                        }
                    };
                    bw.RunWorkerCompleted += (o, ea) =>
                    {

                        if (ValidateArchive())
                        {
                            if (busy != null)
                                busy.IsBusy = false;
                            Enums.DialogResponses? response = null;
                            string name = "";
                            string code = "";
                            WpfControls.WindowsAndDialogs.TwoTextBoxDialog dialog = new WindowsAndDialogs.TwoTextBoxDialog("Next Issue", "Subscriber information saved successfully. Enter name and code for next issue.", "Name: ", "Code: ", false);
                            dialog.Answer += value => response = value;
                            dialog.NameOneAnswer += value => name = value;
                            dialog.NameTwoAnswer += value => code = value;
                            dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                            dialog.ShowDialog();
                            if (response == Enums.DialogResponses.Save)
                            {
                                issueResponse = issueData.Proxy.SelectForPublication(accessKey, myProductID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                                if (Common.CheckResponse(issueResponse.Result, issueResponse.Status))
                                {
                                    Issue i = issueResponse.Result.Where(x => x.IssueId == myIssueID).FirstOrDefault();
                                    if (i != null)
                                    {
                                        i.IsComplete = true;
                                        i.DateComplete = DateTime.Now;
                                        i.CompleteByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                                        issueData.Proxy.Save(accessKey, i, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                                    }
                                    myComp.IsActive = false;
                                    if (myComp.IssueCompId > 0)
                                    {
                                        issueCompData.Proxy.Save(accessKey, myComp, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                                        issueCompDetailData.Proxy.Clear(accessKey, myComp.IssueCompId, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                                    }

                                    Issue nextIssue = new Issue();
                                    nextIssue.PublicationId = myProductID;
                                    nextIssue.IssueName = name;
                                    nextIssue.IssueCode = code;
                                    nextIssue.DateOpened = DateTime.Now;
                                    nextIssue.OpenedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                                    nextIssue.DateCreated = DateTime.Now;
                                    nextIssue.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                                    issueData.Proxy.Save(accessKey, nextIssue, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                                    Window w = Window.GetWindow(this);

                                    myProduct.AllowDataEntry = true;
                                    myProduct.DateUpdated = DateTime.Now;
                                    myProduct.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                                    productData.Proxy.Save(accessKey, myProduct, myClient.ClientConnections);

                                    MessageBox.Show("The new issue has been opened.", "Confirmation", MessageBoxButton.OK);
                                    w.Close();
                                }
                            }
                        }
                        else
                        {
                            //rollback, display error, and send email
                            Core_AMS.Utilities.WPF.MessageError("There was a problem finalizing the Issue.  If this problem persists, please contact Product Support. ");
                            RollBackIssue(origIMB);
                            if (busy != null)
                                busy.IsBusy = false;

                        }
                    };
                    bw.RunWorkerAsync();
                    
                }
                else
                    return;
            }
            #endregion
            #region Finalize Wave
            else
            {
                List<int> workingList = new List<int>();

                MessageBoxResult result = MessageBox.Show("Only the records contained in splits marked as 'Exported' will be saved in this Wave Mailing. Are you sure you want to continue?", "Warning", MessageBoxButton.YesNo);
                if(result == MessageBoxResult.No)
                {
                    return;
                }
                foreach (IssueSplitContainer split in grdSplitInfo.Items)
                {
                    if (split.NotExported == false)
                    {
                        workingList.AddRange(split.SubscriberIDs);
                    }
                }
                Enums.DialogResponses? response = null;
                string name = "";
                WpfControls.SaveCancelTextBoxDialog dialog = new SaveCancelTextBoxDialog("Save Wave Mailing", "Please enter a name for this Wave Mailing.");
                dialog.Answer += value => response = value;
                dialog.filterNameAnswer += value => name = value;
                dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                dialog.ShowDialog();
                if (response == Enums.DialogResponses.Cancel)
                {
                    dialog.Close();
                    name = "Wave";
                }
                else if (response == Enums.DialogResponses.Save)
                {
                    BackgroundWorker bw = new BackgroundWorker();
                    if (busy != null)
                    {
                        busy.IsIndeterminate = false;
                        busy.IsBusy = true;
                    }
                    bw.DoWork += (o, ea) =>
                    {
                        UpdateAcsMailerInfo();
                        workingList = workingList.Except(compSubIDs).ToList();
                        Save_Splits_And_Filters();
                        WaveMailing wm = new WaveMailing();
                        wm.WaveMailingName = name;
                        wm.WaveNumber = waveNumber + 1;
                        wm.IssueID = myIssueID;
                        wm.PublicationID = myProductID;
                        wm.DateCreated = DateTime.Now;
                        wm.CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                        intResponse = waveMailingData.Proxy.Save(accessKey, wm, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                        int total = workingList.Count;
                        int counter = 0;
                        int processedCount = 0;
                        int batchSize = 1000;
                        string xml = "<XML>";
                        if (Common.CheckResponse(intResponse.Result, intResponse.Status))
                        {
                            foreach (int i in workingList)
                            {
                                //We need to save the IMBSEQ numbers somewhere when doing a Wave Mailing because we do not Archive them until later. 
                                //So we use the PubSubscription table to store them temporarily.
                                string imb = "";
                                if (imbSeqs.Keys.Where(x=> x == i).Count() > 0)
                                {
                                    KeyValuePair<int, string> kv = imbSeqs.Where(x => x.Key == i).First();
                                    if (!kv.Equals(new KeyValuePair<int, string>()))
                                        imb = kv.Value;
                                }
                                xml = xml + "<S><ID>" + i + "</ID><IMB>" + imb + "</IMB></S>";

                                counter++;
                                processedCount++;
                                if (processedCount == total || counter == batchSize)
                                {
                                    xml = xml + "</XML>";
                                    productSubscriptionWorker.Proxy.SaveBulkWaveMailing(accessKey, xml, intResponse.Result, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                                    counter = 0;
                                    xml = "<XML>";
                                    this.Dispatcher.BeginInvoke(new System.Action(() =>
                                    {
                                        busy.ProgressValue = (int)Math.Round((double)(processedCount * 100) / total);
                                    }));
                                }
                            }
                            issueResponse = issueData.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                            if(Common.CheckResponse(issueResponse.Result, issueResponse.Status))
                            {
                                Issue myIssue = issueResponse.Result.Where(x => x.IssueId == myIssueID).FirstOrDefault();
                                if(myIssue != null)
                                {
                                    myIssue.DateOpened = DateTime.Now;
                                    myIssue.OpenedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                                    myIssue.IsClosed = false;
                                    myIssue.DateUpdated = DateTime.Now;
                                    issueData.Proxy.Save(accessKey, myIssue, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                                }
                            }

                        }
                    };
                    bw.RunWorkerCompleted += (o, ea) =>
                    {
                        MessageBox.Show("Wave Mailing saved successfully.", "Confirmation", MessageBoxButton.OK);
                        if (busy != null)
                        {
                            busy.IsBusy = false;
                            busy.IsIndeterminate = true;
                        }
                        Window w = Window.GetWindow(this);

                        myProduct.AllowDataEntry = true;
                        myProduct.DateUpdated = DateTime.Now;
                        myProduct.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
                        productData.Proxy.Save(accessKey, myProduct, myClient.ClientConnections);

                        w.Close();
                    };
                    bw.RunWorkerAsync();
                }
                else
                {
                    dialog.Close();
                    return;
                }
            #endregion
            }
        }

        private void btnRemoveChildSplit_Click(object sender, RoutedEventArgs e)
        {
            RadButton btn = sender as RadButton;
            SplitChildContainer scc = btn.DataContext as SplitChildContainer;

            Delete_ChildSplitContainer(scc);
            IssueSplitContainer parent = splitsList.Where(x => x.IssueSplitName == scc.SplitParent).FirstOrDefault();
            if(parent != null)
                parent.ChildSplits.Remove(scc);

            var rows = this.grdSplitInfo.ChildrenOfType<GridViewRow>();
            foreach (GridViewRow row in rows)
            {
                RadButton tmpBtn = row.Cells[0].Content as RadButton;
                tmpBtn.Content = "+";
                row.DetailsVisibility = Visibility.Collapsed;
            }
        }

        private void btnMoveRecords_Click(object sender, RoutedEventArgs e)
        {
            RadButton btn = sender as RadButton;
            IssueSplitContainer isc = btn.DataContext as IssueSplitContainer;

            int desiredDifference = isc.SubscriberIDs.Count - isc.DesiredCount;
            List<int> LNth = getNth(isc.SubscriberIDs.Count, desiredDifference);
            List<int> ids = new List<int>();

            foreach (int n in LNth)
            {
                var id = isc.SubscriberIDs.Skip(n).Take(1);
                ids.Add(id.ToList().FirstOrDefault());
            }
            string name = "";
            List<string> splits = splitsList.Select(x => x.IssueSplitName).Except(new List<string> { isc.IssueSplitName }).ToList();
            WindowsAndDialogs.SplitsOrganizer dialog = new WindowsAndDialogs.SplitsOrganizer(splits, ids.Count);
            dialog.Answer += value => name = value;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            dialog.ShowDialog();

            if (name != null && name != string.Empty)
            {
                isc.SubscriberIDs = isc.SubscriberIDs.Except(ids).ToList();
                IssueSplitContainer destSplit = splitsList.Where(x => x.IssueSplitName == name).FirstOrDefault();
                if(destSplit != null)
                {
                    destSplit.SubscriberIDs.AddRange(ids);
                    destSplit.ChildSplits.Add(new SplitChildContainer(isc.IssueSplitName, destSplit.IssueSplitName, isc.IssueSplitDescription, ids.Count, ids));
                }
            }

        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            List<string> defaultCols = new List<string>();
            //Special columns have extra processing that needs to happen before export. They are also in the default export format (if conditions are met).
            List<string> specialCols = new List<string>() { "ps.[ACSCode]", "ps.[MailerID]", "ps.[Keyline]", "ps.[Split Name]", "ps.[Split Description]", "ps.[KeyCode]" };
            List<string> removeCols = new List<string>();
            DataTable master = new DataTable();
            DataTable masterComps = new DataTable();

            ObservableCollection<ColumnExporter.NewColumn> cols = new ObservableCollection<ColumnExporter.NewColumn>();
            int exports = splitsList.Where(x => x.Save == true).Count();
            #region Multiple Export
            if (exports > 1)
            {
                MessageBoxResult result = MessageBox.Show("You have selected multiple splits for export. Any column changes you make will affect all export files. Do you want to continue?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    MessageBoxResult resultCols = MessageBox.Show("Would you like to select the columns to export?  If you select no, the default columns will be used.", "Select Columns", MessageBoxButton.YesNo);
                    if (resultCols == MessageBoxResult.Yes)
                    {
                        string fields = "";
                        DownloadDetails dd = new DownloadDetails(myProductID, true);
                        dd.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        dd.Check += value => fields = value;
                        dd.ShowDialog();

                        if (fields == "")
                            return;

                        defaultCols = fields.Split(',').ToList();

                        List<string> remove = defaultCols.Intersect(specialCols).ToList();
                        defaultCols.RemoveAll(x => remove.Contains(x));
                        specialCols.RemoveAll(x => !remove.Contains(x));
                    }
                    else
                    {
                        defaultCols = new List<string>()
                        {
                            "p.[Pubcode]","ps.[SequenceID]", "ps.[PubCategoryID]", "ps.[PubTransactionID]"," '' as [TempACSCode] "," '' as [tempkeyline]"," '' as [tempmailerid]",
                            "ps.[IMBSeq]","ps.[Copies]", "ps.[FirstName]", "ps.[LastName]", "ps.[Title]", "ps.[Company]","ps.[Address1]", "ps.[Address2]", "ps.[Address3]", "ps.[City]", "ps.[RegionCode]", "ps.[ZipCode]", "ps.[Plus4]", "ps.[Country]", "ps.[reqflag]",
                            "sp.[ExpireIssueDate]","ps.[Qualificationdate]","ps.[Exp_Qdate]"," '' as [tempkeycode] ",
                            "ps.[QSource]"," '' as [tempsplit] "," '' as [tempsplitdesc] ",
                            "ps.[Phone]","ps.[Fax]","ps.[Email]"

                        };
                    }

                    MessageBoxResult resultSub = MessageBox.Show("Would you like to combine columns to create new fields for the export file?", "Create Columns", MessageBoxButton.YesNo);
                    if (resultSub == MessageBoxResult.Yes)
                    {
                        List<string> editCols = new List<string>();
                        foreach (string s in defaultCols)
                        {
                            int left = s.IndexOf("[");
                            int right = s.IndexOf("]") - 1;
                            string edit = s.Substring(left + 1, right - left);
                            editCols.Add(edit);
                        }
                        cols = CreateColumns(editCols);
                    }
                }
                else
                    return;
            }
            #endregion
            #region Single Export
            else if(exports == 1)
            {
                MessageBoxResult resultCols = MessageBox.Show("Would you like to select the columns to export?  If you select no, the default columns will be used.", "Select Columns", MessageBoxButton.YesNo);
                if (resultCols == MessageBoxResult.Yes)
                {
                    string fields = "";
                    DownloadDetails dd = new DownloadDetails(myProductID, true);
                    dd.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    dd.Check += value => fields = value;
                    dd.ShowDialog();

                    if (fields == "")
                        return;

                    defaultCols = fields.Split(',').ToList();

                    List<string> remove = defaultCols.Intersect(specialCols).ToList();
                    defaultCols.RemoveAll(x => remove.Contains(x));
                    specialCols.RemoveAll(x => !remove.Contains(x));
                }
                else
                {
                    defaultCols = new List<string>()
                        {

                            "p.[Pubcode]","ps.[SequenceID]", "ps.[PubCategoryID]", "ps.[PubTransactionID]"," '' as [TempACSCode] "," '' as [tempkeyline]"," '' as [tempmailerid]",
                            "ps.[IMBSeq]","ps.[Copies]", "ps.[FirstName]", "ps.[LastName]", "ps.[Title]", "ps.[Company]","ps.[Address1]", "ps.[Address2]", "ps.[Address3]", "ps.[City]", "ps.[RegionCode]", "ps.[ZipCode]", "ps.[Plus4]", "ps.[Country]", "ps.[reqflag]", 
                            "sp.[ExpireIssueDate]","ps.[Qualificationdate]","ps.[Exp_Qdate]"," '' as [tempkeycode] ",
                            "ps.[QSource]"," '' as [tempsplit] "," '' as [tempsplitdesc] ",
                            "ps.[Phone]","ps.[Fax]","ps.[Email]"

                        };
                }

                MessageBoxResult resultSub = MessageBox.Show("Would you like to combine columns to create new fields for the export file?", "Create Columns", MessageBoxButton.YesNo);
                if (resultSub == MessageBoxResult.Yes)
                {
                    List<string> editCols = new List<string>();
                    foreach(string s in defaultCols)
                    {
                        int left = s.IndexOf("[");
                        int right = s.IndexOf("]") - 1;
                        string edit = s.Substring(left + 1, right - left);
                        editCols.Add(edit);
                    }
                    cols = CreateColumns(editCols);
                }
            }
            #endregion
            BackgroundWorker bw = new BackgroundWorker();
            busy.IsBusy = true;
            bw.DoWork += (o, ea) =>
            {
                #region GetSubscribers
                string tempCols = "";
                HashSet<int> hs = new HashSet<int>();
                HashSet<int> hsComp = new HashSet<int>();
                List<int> subIDs = new List<int>();
                List<int> compSubs = new List<int>();
                foreach (string s in defaultCols)
                {
                    tempCols += s + ",";
                }
                tempCols = tempCols.TrimEnd(',');
                foreach (IssueSplitContainer isc in splitsList.Where(x=> x.Save == true))
                {
                    subIDs.AddRange(isc.SubscriberIDs);
                }
                compSubs = compSubIDs.Intersect(subIDs).ToList();
                subIDs = subIDs.Except(compSubs).ToList();
                hs = new HashSet<int>(subIDs);
                hsComp = new HashSet<int>(comps.Where(x => compSubs.Contains(x.PubSubscriptionID)).Select(x => x.IssueCompDetailId));
                if (tempCols == "")
                {
                    MessageBox.Show("No columns were selected for export.", "Warning", MessageBoxButton.OK);
                    return;
                }
                #region ProductSubscription
                DataTable dt = new DataTable();
                int rowProcessedCount = 0;
                int index = 0;
                int size = 2500;
                int total = subIDs.Count;
                tempCols = "ps.[PubSubscriptionID]," + tempCols;
                if(total > 0)
                {
                    while (master.Rows.Count < hs.Count)
                    {
                        if ((index + 2500) > subIDs.Count)
                            size = subIDs.Count - index;
                        List<int> temp = subIDs.GetRange(index, size);
                        index += 2500;
                        FrameworkUAS.Service.Response<DataTable> wmProductSubscriptionResponse = new FrameworkUAS.Service.Response<DataTable>();
                        wmProductSubscriptionResponse = productSubscriptionWorker.Proxy.SelectForExportStatic(accessKey, myProductID, tempCols, temp, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                        if (wmProductSubscriptionResponse != null && wmProductSubscriptionResponse.Result != null)
                        {
                            dt = wmProductSubscriptionResponse.Result;
                            rowProcessedCount += dt.Rows.Count;

                            dt.AcceptChanges();
                            master.Merge(dt);
                        }
                    }
                    master.AcceptChanges();
                }
                #endregion

                foreach(DataRow dr in master.Rows)
                {
                    if (master.Columns.Contains("StatusUpdatedDate") && !string.IsNullOrEmpty(dr["StatusUpdatedDate"].ToString()))
                    {
                        dr["StatusUpdatedDate"] = DateTime.Parse(dr["StatusUpdatedDate"].ToString()).ToShortDateString();
                    }
                    if (master.Columns.Contains("ExpireIssueDate") && !string.IsNullOrEmpty(dr["ExpireIssueDate"].ToString()))
                    {
                        dr["ExpireIssueDate"] = DateTime.Parse(dr["ExpireIssueDate"].ToString()).ToShortDateString();
                    }
                    if (master.Columns.Contains("QualificationDate") && !string.IsNullOrEmpty(dr["QualificationDate"].ToString()))
                    {
                        dr["QualificationDate"] = String.Format("{0:MM/dd/yy}", DateTime.Parse(dr["QualificationDate"].ToString()));
                    }
                    if (master.Columns.Contains("Exp_QDate") && !string.IsNullOrEmpty(dr["Exp_QDate"].ToString()))
                    {
                        dr["Exp_QDate"] = DateTime.Parse(dr["Exp_QDate"].ToString()).ToShortDateString();
                    }
                    if (master.Columns.Contains("StartIssueDate") && !string.IsNullOrEmpty(dr["StartIssueDate"].ToString()))
                    {
                        dr["StartIssueDate"] = DateTime.Parse(dr["StartIssueDate"].ToString()).ToShortDateString();
                    }
                    if (master.Columns.Contains("Birthdate") && !string.IsNullOrEmpty(dr["Birthdate"].ToString()))
                    {
                        dr["Birthdate"] = DateTime.Parse(dr["Birthdate"].ToString()).ToShortDateString();
                    }
                    if (master.Columns.Contains("SubGenSubscriptionRenewDate") && !string.IsNullOrEmpty(dr["SubGenSubscriptionRenewDate"].ToString()))
                    {
                        dr["SubGenSubscriptionRenewDate"] = DateTime.Parse(dr["SubGenSubscriptionRenewDate"].ToString()).ToShortDateString();
                    }
                    if (master.Columns.Contains("SubGenSubscriptionExpireDate") && !string.IsNullOrEmpty(dr["SubGenSubscriptionExpireDate"].ToString()))
                    {
                        dr["SubGenSubscriptionExpireDate"] = DateTime.Parse(dr["SubGenSubscriptionExpireDate"].ToString()).ToShortDateString();
                    }
                    if (master.Columns.Contains("SubGenSubscriptionLastQualifiedDate") && !string.IsNullOrEmpty(dr["SubGenSubscriptionLastQualifiedDate"].ToString()))
                    {
                        dr["SubGenSubscriptionLastQualifiedDate"] = DateTime.Parse(dr["SubGenSubscriptionLastQualifiedDate"].ToString()).ToShortDateString();
                    }

                }
                master.AcceptChanges();

                #region Comps
                if (compSubs.Count > 0)
                {
                    FrameworkUAD.Entity.IssueCompDetail icd = new FrameworkUAD.Entity.IssueCompDetail();
                    List<System.Reflection.PropertyInfo> props = icd.GetType().GetProperties().ToList();
                    size = 2500;
                    index = 0;
                    total = hsComp.Count();
                    rowProcessedCount = 0;
                    tempCols = "";
                    foreach (string s in defaultCols)
                    {
                        //tempCols += s + ",";
                        string baseProp = s.Replace("ps.", "").Replace("[", "").Replace("]", "").Replace("sp.","").Replace("demos.", "");
                        if (s.Contains("ps."))
                        {
                            if (props.Select(x => x.Name.ToLower()).Contains(baseProp.ToLower()))
                                tempCols += s + ",";
                            else if (baseProp.ToLower().Equals("exp_qdate"))
                            {
                                tempCols += s + ",";
                            }
                            else if (baseProp.ToLower().Equals("qsource"))
                            {
                                tempCols += @" '''' as QSource,";
                            }
                            else if(baseProp.ToLower().Equals("categorycode") || baseProp.ToLower().Equals("transactioncode"))
                            {
                                tempCols += s + ",";
                            }
                        }
                        else if (s.Contains("Pubcode"))
                        {
                            tempCols += s + ",";
                        }
                        else if(s.StartsWith(@" '' as"))
                        {
                            tempCols += baseProp.Replace("''","''''") + ",";
                        }
                        else
                        {
                            if (baseProp.Equals("expireissuedate"))
                            {
                                tempCols += @" '''' as ExpireIssueDate,";
                            }
                            else
                            {
                                tempCols += @" '''' as [" + baseProp + "],";
                            }
                        }

                    }
                    tempCols = tempCols.TrimEnd(',');
                    tempCols = "IssueCompDetailId," + tempCols;
                    List<int> compIDs = new List<int>(hsComp);
                    while (rowProcessedCount < total)
                    {
                        if ((index + 2500) > compIDs.Count)
                            size = compIDs.Count - index;
                        List<int> temp = compIDs.GetRange(index, size);
                        index += 2500;
                        dt = issueCompDetailData.Proxy.SelectForExport(accessKey, myIssueID, tempCols, temp, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
                        rowProcessedCount += dt.Rows.Count;

                        dt.AcceptChanges();
                        masterComps.Merge(dt);
                    }
                    masterComps.AcceptChanges();
                    //Switch below to try parse
                    foreach (DataRow dr in masterComps.Rows)
                    {
                        try
                        {
                            if (masterComps.Columns.Contains("StatusUpdatedDate") && dr["StatusUpdatedDate"] != null)
                                dr["StatusUpdatedDate"] = DateTime.Parse(dr["StatusUpdatedDate"].ToString()).ToShortDateString();
                            if (masterComps.Columns.Contains("ExpireIssueDate") && dr["ExpireIssueDate"] != null)
                                dr["ExpireIssueDate"] = DateTime.Parse(dr["ExpireIssueDate"].ToString()).ToShortDateString();
                            if (masterComps.Columns.Contains("QualificationDate") && dr["QualificationDate"] != null)
                                dr["QualificationDate"] = DateTime.Parse(dr["QualificationDate"].ToString()).ToShortDateString();
                            if (masterComps.Columns.Contains("Exp_QDate") && dr["Exp_QDate"] != null)
                                dr["Exp_QDate"] = DateTime.Parse(dr["Exp_QDate"].ToString()).ToShortDateString();
                            if (masterComps.Columns.Contains("StartIssueDate") && dr["StartIssueDate"] != null)
                                dr["StartIssueDate"] = DateTime.Parse(dr["StartIssueDate"].ToString()).ToShortDateString();
                            if (masterComps.Columns.Contains("Birthdate") && dr["Birthdate"] != null)
                                dr["Birthdate"] = DateTime.Parse(dr["Birthdate"].ToString()).ToShortDateString();
                            if (masterComps.Columns.Contains("SubGenSubscriptionRenewDate") && dr["SubGenSubscriptionRenewDate"] != null)
                                dr["SubGenSubscriptionRenewDate"] = DateTime.Parse(dr["SubGenSubscriptionRenewDate"].ToString()).ToShortDateString();
                            if (masterComps.Columns.Contains("SubGenSubscriptionExpireDate") && dr["SubGenSubscriptionExpireDate"] != null)
                                dr["SubGenSubscriptionExpireDate"] = DateTime.Parse(dr["SubGenSubscriptionExpireDate"].ToString()).ToShortDateString();
                            if (masterComps.Columns.Contains("SubGenSubscriptionLastQualifiedDate") && dr["SubGenSubscriptionLastQualifiedDate"] != null)
                                dr["SubGenSubscriptionLastQualifiedDate"] = DateTime.Parse(dr["SubGenSubscriptionLastQualifiedDate"].ToString()).ToShortDateString();
                        }
                        catch
                        { }
                    }
                    masterComps.AcceptChanges();
                }
                #endregion

                #endregion

                #region ChangeColumns
                //If user combined/renamed any columns, that work happens here.
                if (cols.Count > 0)
                {
                    foreach (ColumnExporter.NewColumn n in cols)
                    {
                        string delimiter = "";

                        switch (n.Delimiter)
                        {
                            case "Comma":
                                delimiter = ",";
                                break;
                            case "Space":
                                delimiter = " ";
                                break;
                            case "None":
                                break;
                        }

                        string expression = "";
                        foreach (string s in n.Columns)
                        {
                            if (s != n.Columns.Last())
                                expression = expression + "IsNull(" + s + ", '' )" + " + '" + delimiter + "' + ";
                            else
                                expression = expression + "IsNull(" + s + ", '' )";

                            removeCols.Add(s);
                        }
                        if (subIDs.Count > 0)
                        {
                            master.Columns.Add(n.Name, typeof(string), expression);
                            master.AcceptChanges();
                        }
                    }
                }
                #endregion

                master = SetSpecialColumns(master, specialCols);
                masterComps = SetSpecialColumns(masterComps, specialCols);
            };
            #region Exporting
            bw.RunWorkerCompleted += (o, ea) =>
            {
                busy.IsBusy = false;
                grdExporting.AutoGenerateColumns = true;
                //MessageBoxResult result = MessageBox.Show("Do you want to reorder file columns?", "Reorder Columns", MessageBoxButton.YesNo);
                //if (result == MessageBoxResult.Yes)
                //{
                //    DataTable copy = master.Clone();
                //    WindowsAndDialogs.ColumnReorder cr = new WindowsAndDialogs.ColumnReorder(copy);
                //    cr.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                //    cr.Answer += value =>
                //    {
                //        foreach (KeyValuePair<string, int> kv in value)
                //        {
                //            master.Columns[kv.Key].SetOrdinal(kv.Value);
                //        }
                //    };
                //    cr.ShowDialog();
                //    copy.Dispose();
                //}
                foreach (IssueSplitContainer split in splitsList.Where(x => x.Save == true))
                {
                    grdExporting.ItemsSource = null;
                    DataTable masterClone = master.Copy();
                    DataTable masterCompClone = masterComps.Copy();
                    HashSet<int> hsTemp = new HashSet<int>(split.SubscriberIDs);
                    List<int> idTemp = new List<int>(compSubIDs.Intersect(split.SubscriberIDs));
                    HashSet<int> hsComp = new HashSet<int>(comps.Where(x => idTemp.Contains(x.PubSubscriptionID)).Select(x => x.IssueCompDetailId));

                    if (specialCols.Contains("ps.[Split Name]"))
                    {
                        DataColumn newColumn = new DataColumn("Split Name");
                        DataColumn newCompColumn = new DataColumn("Split Name");
                        newColumn.DefaultValue = split.IssueSplitName;
                        newCompColumn.DefaultValue = split.IssueSplitName;
                        masterClone.Columns.Add(newColumn);
                        masterCompClone.Columns.Add(newCompColumn);

                        if (masterClone.Columns.Contains("tempsplit"))
                        {
                            int keycodeindex = masterClone.Columns.IndexOf("tempsplit");
                            int createdIndex = masterClone.Columns.IndexOf("Split Name");
                            foreach (DataRow dr in masterClone.Rows)
                            {
                                dr[keycodeindex] = dr[createdIndex];
                            }
                            masterClone.Columns.Remove("Split Name");
                            masterClone.Columns["tempsplit"].ColumnName = "Split Name";
                        }

                        if (masterCompClone.Columns.Contains("tempsplit"))
                        {
                            int keycodeindex = masterCompClone.Columns.IndexOf("tempsplit");
                            int createdIndex = masterCompClone.Columns.IndexOf("Split Name");
                            foreach (DataRow dr in masterCompClone.Rows)
                            {
                                dr[keycodeindex] = dr[createdIndex];
                            }
                            masterCompClone.Columns.Remove("Split Name");
                            masterCompClone.Columns["tempsplit"].ColumnName = "Split Name";
                        }
                    }
                    if (specialCols.Contains("ps.[Split Description]"))
                    {
                        DataColumn newColumn = new DataColumn("Split Description");
                        DataColumn newCompColumn = new DataColumn("Split Description");
                        newColumn.DefaultValue = split.IssueSplitDescription;
                        newCompColumn.DefaultValue = split.IssueSplitDescription;
                        masterClone.Columns.Add(newColumn);
                        masterCompClone.Columns.Add(newCompColumn);

                        if (masterClone.Columns.Contains("tempsplitdesc"))
                        {
                            int keycodeindex = masterClone.Columns.IndexOf("tempsplitdesc");
                            int createdIndex = masterClone.Columns.IndexOf("Split Description");
                            foreach (DataRow dr in masterClone.Rows)
                            {
                                dr[keycodeindex] = dr[createdIndex];
                            }
                            masterClone.Columns.Remove("Split Description");
                            masterClone.Columns["tempsplitdesc"].ColumnName = "Split Description";
                        }

                        if (masterCompClone.Columns.Contains("tempsplitdesc"))
                        {
                            int keycodeindex = masterCompClone.Columns.IndexOf("tempsplitdesc");
                            int createdIndex = masterCompClone.Columns.IndexOf("Split Description");
                            foreach (DataRow dr in masterCompClone.Rows)
                            {
                                dr[keycodeindex] = dr[createdIndex];
                            }
                            masterCompClone.Columns.Remove("Split Description");
                            masterCompClone.Columns["tempsplitdesc"].ColumnName = "Split Description";
                        }
                    }
                    if (specialCols.Contains("ps.[KeyCode]"))
                    {
                        DataColumn newColumn = new DataColumn("KeyCode");
                        DataColumn newCompColumn = new DataColumn("KeyCode");
                        newColumn.DefaultValue = split.KeyCode;
                        newCompColumn.DefaultValue = split.KeyCode;
                        masterClone.Columns.Add(newColumn);
                        masterCompClone.Columns.Add(newCompColumn);

                        if (masterClone.Columns.Contains("tempkeycode"))
                        {
                            int keycodeindex = masterClone.Columns.IndexOf("tempkeycode");
                            int createdIndex = masterClone.Columns.IndexOf("KeyCode");
                            foreach(DataRow dr in masterClone.Rows)
                            {
                                dr[keycodeindex] = dr[createdIndex];
                            }
                            masterClone.Columns.Remove("KeyCode");
                            masterClone.Columns["tempkeycode"].ColumnName = "KeyCode";
                        }
                        if (masterCompClone.Columns.Contains("tempkeycode"))
                        {
                            int keycodeindex = masterCompClone.Columns.IndexOf("tempkeycode");
                            int createdIndex = masterCompClone.Columns.IndexOf("KeyCode");
                            foreach (DataRow dr in masterCompClone.Rows)
                            {
                                dr[keycodeindex] = dr[createdIndex];
                            }
                            masterCompClone.Columns.Remove("KeyCode");
                            masterCompClone.Columns["tempkeycode"].ColumnName = "KeyCode";
                        }
                    }

                    //This Section will update the SplitName and Description back to its origination
                    //foreach (SplitChildContainer childSplit in split.ChildSplits)
                    //{
                    //    //If any records came from a different Split, we want to make sure they are noted correctly in the output files.
                    //    masterClone.Rows.Cast<DataRow>().Where(r => childSplit.SubscriberIDs.Contains((int) r["PubSubscriptionID"])).ToList().ForEach(r =>
                    //    {
                    //        r["Split Description"] = childSplit.SplitDescription;
                    //        r["Split Name"] = childSplit.SplitName;
                    //    });
                    //    masterCompClone.Rows.Cast<DataRow>().Where(r => childSplit.SubscriberIDs.Contains((int) r["IssueCompDetailId"])).ToList().ForEach(r =>
                    //    {
                    //        r["Split Description"] = childSplit.SplitDescription;
                    //        r["Split Name"] = childSplit.SplitName;
                    //    });
                    //}

                    //Remove unneeded columns.
                    masterClone.Rows.Cast<DataRow>().Where(r => !hsTemp.Contains((int)r["PubSubscriptionID"])).ToList().ForEach(r => r.Delete());
                    if (masterClone.Columns.Contains("PubSubscriptionID"))
                        masterClone.Columns.Remove("PubSubscriptionID");
                    masterCompClone.Rows.Cast<DataRow>().Where(r => !hsComp.Contains((int)r["IssueCompDetailId"])).ToList().ForEach(r => r.Delete());
                    masterClone.AcceptChanges();





                    if (masterClone.Rows.Count >= 1)
                    {
                        masterClone.Merge(masterCompClone, false, MissingSchemaAction.Add);
                    }
                    else
                    {
                        masterClone = masterCompClone;
                    }

                    if (masterClone.Columns.Contains("IssueCompDetailId"))
                        masterClone.Columns.Remove("IssueCompDetailId");
                    if (masterClone.Columns.Contains("tempACSCode"))
                        masterClone.Columns["tempACSCode"].ColumnName = "ACSCode";
                    if (masterClone.Columns.Contains("tempkeyline"))
                        masterClone.Columns["tempkeyline"].ColumnName = "Keyline";
                    if (masterClone.Columns.Contains("tempmailerid"))
                        masterClone.Columns["tempmailerid"].ColumnName = "MailerID";
                    masterClone.AcceptChanges();

                    foreach (DataRow dr in masterClone.Rows)
                    {
                        if(String.IsNullOrEmpty(dr.ToString()))
                        {
                            masterClone.Rows.Remove(dr);
                        }
                    }
                    masterClone.AcceptChanges();

                    grdExporting.ItemsSource = masterClone;
                    foreach (string s in removeCols)
                    {
                        for (int i = grdExporting.Columns.Count - 1; i >= 0; i--)
                        {
                            if (grdExporting.Columns[i].Header.Equals(s))
                                grdExporting.Columns.RemoveAt(i);
                        }
                    }
                    string sanitizedName = Core_AMS.Utilities.StringFunctions.MakeValidFileName(split.IssueSplitName);
                    CreateCSVFile(sanitizedName);
                    split.NotExported = false;
                    split.Save = false;
                }
                MessageBox.Show("Exports completed and saved to User's Downloads folder.");
            };
            bw.RunWorkerAsync();
            #endregion
        }

        private void btn_ExpandRowDetail(object sender, RoutedEventArgs e)
        {
            RadButton btn = sender as RadButton;
            Telerik.Windows.Controls.GridView.GridViewRow grdRow = btn.ParentOfType<Telerik.Windows.Controls.GridView.GridViewRow>();
            if (btn.Content.ToString().Equals("+"))
            {
                grdRow.DetailsVisibility = Visibility.Visible;
                btn.Content = "-";
            }
            else
            {
                grdRow.DetailsVisibility = Visibility.Collapsed;
                btn.Content = "+";
            }
        }

        private void Delete_Split(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBoxResult.Cancel;
            if (myAcsMailer.MailerID > 0 && maxIMB > myAcsMailer.ImbSeqCounter)
            {
                result = MessageBox.Show("Are you sure you want to delete this split? This will reset the IMBSequence Counter to the value it was before exporting. Currently exported files will no longer be valid and need to be exported again. Are you sure you want to continue?", "Warning", MessageBoxButton.YesNo);
                if(result == MessageBoxResult.Yes)
                {
                    maxIMB = myAcsMailer.ImbSeqCounter;
                    if (maxIMB == 0)
                        maxIMB = 1;
                    imbSeqs.Clear();
                    compImbSeqs.Clear();
                }
            }
            else
            {
                result = MessageBox.Show("Are you sure you want to delete this split?", "Warning", MessageBoxButton.YesNo);
            }
            if (result == MessageBoxResult.Yes)
            {
                System.Windows.Controls.Image me = sender as System.Windows.Controls.Image;
                IssueSplitContainer container = me.DataContext as IssueSplitContainer;
                //IssueSplit split = container.IssueSplit;
                BackgroundWorker bw = new BackgroundWorker();
                busy.IsBusy = true;
                bw.DoWork += (o, ea) =>
                {
                    if (container.IssueSplitId > 0)
                    {
                        //split.IsActive = false;
                        //splitData.Proxy.Save(accessKey, split, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient);

                        //FrameworkUAS.Entity.Filter f = filterList.Where(x => x.FilterId == split.FilterId).FirstOrDefault();
                        //if (f != null)
                        //{
                        //    f.IsActive = false;
                        //    filterData.Proxy.Save(accessKey, f);
                        //}

                        //filterResponse = filterData.Proxy.Select(accessKey, myProductID);
                        //if (Common.CheckResponse(filterResponse.Result, filterResponse.Status))
                        //    filterList = filterResponse.Result.Where(x => x.IsActive == true).ToList();

                        //splitResponse = splitData.Proxy.SelectForIssueID(accessKey, myIssueID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient);
                        //if (Common.CheckResponse(splitResponse.Result, splitResponse.Status))
                        //    splits = splitResponse.Result.Where(x => x.IsActive == true).ToList();
                    }

                    //List<int> subIDs = new List<int>();
                    //if (Common.CheckResponse(subCountResponse.Result, subCountResponse.Status))
                    //{
                    //    subIDs = subCountResponse.Result.Where(x => activeSubscriptionIDs.Contains(x)).ToList();
                    //}

                    remainingPool.AddRange(container.SubscriberIDs);
                };

                bw.RunWorkerCompleted += (o, ea) =>
                {
                    foreach (SplitChildContainer scc in container.ChildSplits)
                    {
                        Delete_ChildSplitContainer(scc);
                    }
                    splitsList.Remove(container);
                    int subCount = 0;
                    subCount = subscriptionList.Select(x => x.Copies).Sum();

                    int remaining = subCount - splitsList.Select(x => x.IssueSplitCount).Sum();
                    txtRemaining.Text = "Remaining: " + remaining.ToString();
                    busy.IsBusy = false;
                };
                bw.RunWorkerAsync();
            }
        }

        private void Delete_Filter(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Image me = sender as System.Windows.Controls.Image;
            IssueSplitContainer container = me.DataContext as IssueSplitContainer;
            filterCollection.Remove(container);
        }

        private void Export_Checked(object sender, RoutedEventArgs e)
        {
            int exports = splitsList.Where(x => x.Save == true).Count();
            if (exports > 0)
            {
                this.btnExport.IsEnabled = true;
            }
            else
                this.btnExport.IsEnabled = false;
        }

        private void Reset_SplitExport(object sender, MouseButtonEventArgs e)
        {
            if (myAcsMailer.MailerID > 0 && maxIMB > myAcsMailer.ImbSeqCounter)
            {
                MessageBoxResult result = MessageBox.Show("Unchecking an export will reset the IMBSequence Counter to the value it was before exporting. Currently exported files will no longer be valid and need to be exported again. Are you sure you want to continue?", "Warning", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    maxIMB = myAcsMailer.ImbSeqCounter;
                    if (maxIMB == 0)
                        maxIMB = 1;
                    System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
                    IssueSplitContainer splitContainer = img.DataContext as IssueSplitContainer;
                    splitContainer.NotExported = true;
                    imbSeqs.Clear();
                    compImbSeqs.Clear();
                }
                else
                    return;
            }
            else
            {
                System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
                IssueSplitContainer splitContainer = img.DataContext as IssueSplitContainer;
                splitContainer.NotExported = true;
            }
        }
        #endregion

        #region Helper Methods
        private void CreateCSVFile(string splitName)
        {
            string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string pathDownload = System.IO.Path.Combine(pathUser, "Downloads");
            string filePath = pathDownload + "\\" + splitName + ".csv";
            if (File.Exists(filePath))
            {
                int i = 1;
                while (File.Exists(filePath))
                {
                    filePath = pathDownload + "\\" + splitName + "_" + i + ".csv";
                    i++;
                }
            }
            using (FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                string test = grdExporting.ToCsv();
                StreamWriter sw = new StreamWriter(stream);

                test = HtmlFunctions.StripTextFromHtmlForExport(test);

                sw.Write(test);
                sw.Flush();
                sw.Close();
            }
        }
        private ObservableCollection<ColumnExporter.NewColumn> CreateColumns(List<string> columns)
        {
            ObservableCollection<ColumnExporter.NewColumn> cols = new ObservableCollection<ColumnExporter.NewColumn>();
            ColumnExporter ce = new ColumnExporter(columns);
            ce.Answer += value => cols = value;
            ce.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ce.ShowDialog();
            return cols;
        }
        private void Delete_ChildSplitContainer(SplitChildContainer container)
        {
            IssueSplitContainer destSplit = splitsList.Where(x => x.IssueSplitName == container.SplitName).FirstOrDefault();
            IssueSplitContainer parentSplit = splitsList.Where(x => x.IssueSplitName == container.SplitParent).FirstOrDefault();
            if (destSplit != null)
            {
                destSplit.SubscriberIDs.AddRange(container.SubscriberIDs);
                destSplit.DesiredCount = destSplit.DesiredCount + container.SplitCount;
            }
            if(parentSplit != null)
            {
                parentSplit.SubscriberIDs.RemoveAll(x => container.SubscriberIDs.Contains(x));
            }
        }
        private void ArchiveAll()
        {
            this.Dispatcher.BeginInvoke(new System.Action(() =>
            {
                busy.BusyContent = "Archiving Records...";
                busy.IsIndeterminate = true;
            }));

            issueData.Proxy.ArchiveAll(accessKey, myProductID, myIssueID, imbSeqs, compImbSeqs, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
        }
        private bool ValidateArchive()
        {
            bool result = issueData.Proxy.ValidateArchive(accessKey, myProductID, myIssueID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections).Result;
            return result;
        }
        private void RollBackIssue(int origIMB)
        {
            issueData.Proxy.RollBackIssue(accessKey, myProductID, myIssueID,origIMB, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
        }
        private void UpdateOriginalRecords()
        {
            this.Dispatcher.BeginInvoke(new System.Action(() =>
            {
                busy.BusyContent = "Loading...";
                busy.IsIndeterminate = true;
            }));

            int counter = 0;
            while (counter < 2)
            {
                try
                {
                    wMDetailData.Proxy.UpdateOriginalSubInfo(accessKey, myProductID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                        FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
                    counter = 2;
                }
                catch (Exception)
                {
                    counter++;
                }
            }
        }
        private void UpdateAcsMailerInfo()
        {
            myAcsMailer.ImbSeqCounter = maxIMB;
            myAcsMailer.DateUpdated = DateTime.Now;
            myAcsMailer.UpdatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID;
            mailerInfoData.Proxy.Save(accessKey, myAcsMailer, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
        }
        private DataTable SetSpecialColumns(DataTable dt, List<string> cols)
        {
            int mailerID = myAcsMailer.MailerID;
            string acsCode = myAcsMailer.AcsCode;

            if (acsCode != null && acsCode != string.Empty)
            {
                if (cols.Contains("ps.[ACSCode]"))
                {
                    DataColumn newColumn = new DataColumn("ACSCode");
                    newColumn.DefaultValue = acsCode;
                    dt.Columns.Add(newColumn);

                    if (dt.Columns.Contains("TempACSCode"))
                    {
                        int keycodeindex = dt.Columns.IndexOf("TempACSCode");
                        int createdIndex = dt.Columns.IndexOf("ACSCode");
                        foreach (DataRow dr in dt.Rows)
                        {
                            dr[keycodeindex] = dr[createdIndex];
                        }
                        dt.Columns.Remove("ACSCode");
                        dt.Columns["TempACSCode"].ColumnName = "ACSCode";
                    }

                }
            }

            if(acsCode != null && acsCode != "")
            {
                if (cols.Contains("ps.[Keyline]") && dt.Columns.Contains("SequenceID"))
                {
                    dt.Columns.Add("Keyline");
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["SequenceID"].ToString() != "0")
                        {
                            var key = KeyLineComputation.Compute(dr["SequenceID"].ToString(), errorMessage => MessageBox.Show(errorMessage));
                            dr["Keyline"] = key;
                        }
                        if (dt.Columns.Contains("tempkeyline"))
                        {
                            int keycodeindex = dt.Columns.IndexOf("tempkeyline");
                            int createdIndex = dt.Columns.IndexOf("Keyline");
                            foreach (DataRow dr2 in dt.Rows)
                            {
                                dr2[keycodeindex] = dr2[createdIndex];
                            }
                            dt.Columns.Remove("Keyline");
                            dt.Columns["tempkeyline"].ColumnName = "Keyline";
                        }
                    }
                    if (dt.Columns.Contains("tempkeyline"))
                    {
                        int keycodeindex = dt.Columns.IndexOf("tempkeyline");
                        int createdIndex = dt.Columns.IndexOf("Keyline");
                        foreach (DataRow dr in dt.Rows)
                        {
                            dr[keycodeindex] = dr[createdIndex];
                        }
                        dt.Columns.Remove("Keyline");
                        dt.Columns["tempkeyline"].ColumnName = "Keyline";
                    }
                }
            }
            if (mailerID > 0)
            {
                if (dt.Columns.Contains("IMBSeq"))
                {
                    if (dt.Columns.Contains("PubSubscriptionID"))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string imb = CreateIMBSeq(maxIMB, mailerID.ToString());
                            int id = 0;
                            dr["IMBSeq"] = imb;
                            if(int.TryParse(dr["PubSubscriptionID"].ToString(), out id) == true && !imbSeqs.ContainsKey(id) && !imbSeqs.ContainsValue(imb))
                                imbSeqs.Add(id, imb);
                        }
                    }
                    else if (dt.Columns.Contains("IssueCompDetailId"))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string imb = CreateIMBSeq(maxIMB, mailerID.ToString());
                            int id = 0;
                            dr["IMBSeq"] = imb;
                            if (int.TryParse(dr["IssueCompDetailId"].ToString(), out id) == true && !compImbSeqs.ContainsKey(id) && !compImbSeqs.ContainsValue(imb))
                                compImbSeqs.Add(id, imb);
                        }
                    }
                }
            }
            if (cols.Contains("ps.[MailerID]"))
            {
                DataColumn newColumn = new DataColumn("MailerID");
                newColumn.DefaultValue = mailerID;
                dt.Columns.Add(newColumn);

                if (dt.Columns.Contains("tempmailerid"))
                {
                    int keycodeindex = dt.Columns.IndexOf("tempmailerid");
                    int createdIndex = dt.Columns.IndexOf("MailerID");
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr[keycodeindex] = dr[createdIndex];
                    }
                    dt.Columns.Remove("MailerID");
                    dt.Columns["tempmailerid"].ColumnName = "MailerID";
                }
            }
            if(dt.Columns.Contains("ExpireIssueDate"))
            {
                int Index = dt.Columns.IndexOf("ExpireIssueDate");
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[Index].ToString() == "'")
                        dr[Index] = "";
                }
            }
            if (dt.Columns.Contains("QSource"))
            {
                int Index = dt.Columns.IndexOf("QSource");
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[Index].ToString() == "'")
                        dr[Index] = "";
                }
            }

            return dt;
        }
        private void Save_Splits_And_Filters()
        {
            List<FrameworkUAD.Entity.IssueSplit> splits = new List<IssueSplit>();
            splitsList.ToList().ForEach(x=> splits.Add(x.GetIssueSplitEntity()));

            splitData.Proxy.Save(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, splits, myIssueID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
        }
        private bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
        private string CreateIMBSeq(int imb, string id)
        {
            int count = id.Length;
            int fill = 15 - count;

            string retIMB = imb.ToString();
            if (retIMB.Length > fill)
            {
                maxIMB = 2;
                retIMB = "1";
            }
            else
                maxIMB++;

            for(int i=retIMB.Length; i<fill; i++)
            {
                retIMB = "0" + retIMB;
            }

            return retIMB;
        }
        private List<FilterCombinations> GetCombination(List<int> list)
        {
            //Algorithm to find all possible combinations of a list of values.
            List<FilterCombinations> combos = new List<FilterCombinations>();
            double count = Math.Pow(2, list.Count);
            for (int i = 1; i <= count - 1; i++)
            {
                List<int> criteria = new List<int>();
                string str = Convert.ToString(i, 2).PadLeft(list.Count, '0');
                for (int j = 0; j < str.Length; j++)
                {
                    if (str[j] == '1')
                    {
                        criteria.Add(list[j]);
                    }
                }
                combos.Add(new FilterCombinations(criteria));
            }
            combos = combos.OrderByDescending(x => x.Criteria.Count).ToList();
            return combos;
        }
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
        private List<FilterControls.Framework.FilterObject> GetCombinedDetail(List<FilterControls.Framework.FilterObject> deets)
        {
            List<FilterControls.Framework.FilterObject> returnDetails = new List<FilterControls.Framework.FilterObject>();
            Dictionary<string, string> dictDetails = new Dictionary<string, string>();
            foreach(FilterControls.Framework.FilterObject fo in deets)
            {
                if (!returnDetails.Select(x => x.Name).Contains(fo.Name))
                {
                    if (fo is FilterControls.Framework.ListFilterObject)
                    {
                        FilterControls.Framework.ListFilterObject lfo = fo as FilterControls.Framework.ListFilterObject;
                        if (deets.Where(x => x.Name == fo.Name).Count() > 1)
                        {
                            List<FilterControls.Framework.ListFilterObject> filters = deets.Where(x => x.Name == fo.Name).Cast<FilterControls.Framework.ListFilterObject>().ToList();
                            List<FilterControls.Framework.ListObject> options = new List<FilterControls.Framework.ListObject>();
                            foreach(List<FilterControls.Framework.ListObject> lo in filters.Select(x=> x.SelectedOptions))
                            {
                                options.AddRange(lo);
                            }
                            options = options.GroupBy(x => x.Value).Select(x => x.First()).ToList();
                            lfo.SelectedOptions = options;
                            returnDetails.Add(lfo);
                        }
                        else
                            returnDetails.Add(lfo);
                    }
                    else
                    {
                        if (deets.Where(x => x.Name == fo.Name).Count() > 1)
                        {
                            List<FilterControls.Framework.FilterObject> subList = deets.Where(x => x.Name == fo.Name).Distinct().ToList();
                            returnDetails.AddRange(subList);
                        }
                        else
                            returnDetails.Add(fo);
                    }
                }
            }

            return returnDetails;
        }
        #endregion

        #region Report | Exports
        private void btnGenerateReport_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker bw = new BackgroundWorker();
            busy.IsBusy = true;
            busy.IsIndeterminate = true;
            DataTable reqSum = new DataTable();

            bw.DoWork += (ev, o) =>
            {
                DataTable reqs = reportData.Proxy.ReqFlagSummary(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, myProductID).Result;
                List<int> splitsSubs = new List<int>();
                List<int> compIDs = new List<int>();
                foreach (IssueSplitContainer isc in splitsList)
                {
                    //splitReports.Add(isc.GetReportEntity());
                    splitsSubs.AddRange(isc.SubscriberIDs);
                    compIDs.AddRange(comps.Where(x => isc.SubscriberIDs.Contains(x.PubSubscriptionID)).Select(x => x.IssueCompDetailId));
                }

                //Only keep records that are in the Splits. We use PubSubscriptionID to match.
                reqs.Rows.Cast<DataRow>().Where(r => !splitsSubs.Contains((int)r.ItemArray[4]) && (bool)r.ItemArray[5] == false).ToList().ForEach(r => r.Delete());
                reqs.AcceptChanges();
                reqs.Rows.Cast<DataRow>().Where(r => !compIDs.Contains((int)r.ItemArray[4]) && (bool)r.ItemArray[5] == true).ToList().ForEach(r => r.Delete());
                reqs.AcceptChanges();
                if (reqs.Columns.Contains("PubSubscriptionID"))
                    reqs.Columns.Remove("PubSubscriptionID");
                reqs.AcceptChanges();

                var query = from row in reqs.AsEnumerable()
                            group row by new { ReqValue = row.Field<string>("CodeValue"), Reqname = row.Field<string>("CodeName") } into g
                            select new
                            {
                                Code = (from value in g
                                        select value.Field<string>("CodeValue")).FirstOrDefault(),
                                Description = (from value in g
                                               select value.Field<string>("CodeName")).FirstOrDefault(),
                                Records = (from value in g.ToList()
                                           select value.Field<int>("RecordCount")).Sum(),
                                Copies = (from value in g.ToList()
                                          select value.Field<int>("Copies")).Sum()
                            };

                reqSum = ConvertToDataTable(query);
                DataRow dr = reqSum.NewRow();
                dr[0] = "Total";
                int total = 0;
                reqSum.Rows.Cast<DataRow>().ToList().ForEach(c => total += (int)c["Copies"]);
                dr["Copies"] = total;
                total = 0;
                reqSum.Rows.Cast<DataRow>().ToList().ForEach(c => total += (int)c["Records"]);
                dr["Records"] = total;
                reqSum.Rows.Add(dr);
            };
            bw.RunWorkerCompleted += (o, ea) =>
            {
                rgvSplitsSummary.ItemsSource = splitsList;
                rgvReqSummary.ItemsSource = reqSum;
                rgvSplitsSummary.Visibility = Visibility.Visible;
                rgvReqSummary.Visibility = Visibility.Visible;
                busy.IsBusy = false;
            };
            bw.RunWorkerAsync();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(rcbExtensions.SelectedItem == null || rcbExtensions.SelectedItem.ToString() == "")
            {
                Core_AMS.Utilities.WPF.Message("Please select a format before saving.");
                return;
            }
            string ext = rcbExtensions.SelectedItem as string;
            SaveFileDialog dialog = new SaveFileDialog()
            {
                DefaultExt = ext.ToLower(),
                Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", ext.ToLower(), ext),
                FilterIndex = 1,
            };
            if (dialog.ShowDialog() == true)
            {
                if (ext == "PDF")
                {
                    List<string> files = new List<string>();

                    if (rgvSplitsSummary.ItemsSource != null)
                    {
                        List<IssueSplitReport> splitReports = new List<IssueSplitReport>();
                        foreach (IssueSplitContainer isc in splitsList)
                            splitReports.Add(isc.GetReportEntity());
                        DataTable src = Core_AMS.Utilities.DataTableFunctions.ToDataTable(splitReports);
                        DataRow dr = src.NewRow();
                        dr[0] = "Total";
                        int total = 0;
                        src.Rows.Cast<DataRow>().ToList().ForEach(c => total += (int)c["Copies"]);
                        dr["Copies"] = total;
                        total = 0;
                        src.Rows.Cast<DataRow>().ToList().ForEach(c => total += (int)c["Records"]);
                        dr["Records"] = total;
                        src.Rows.Add(dr);
                        files.Add(ExportToPdf(src, "IssueSplitSummary.pdf", "Issue Split Summary", true, false));
                    }
                    if (rgvReqSummary.ItemsSource != null)
                    {
                        DataTable src = rgvReqSummary.ItemsSource as DataTable;
                        files.Add(ExportToPdf(src, "ReqFlagsummary.pdf", "", false, true));
                    }
                    if (files.Count > 1)
                    {
                        CombinePDFs(files, dialog.SafeFileName, dialog.FileName);
                    }
                }
                else
                {
                    using (Stream stream = dialog.OpenFile())
                    {
                        rgvSplitsSummary.Export(stream,
                            new GridViewExportOptions()
                            {
                                Format = ExportFormat.Csv,
                                ShowColumnHeaders = true,
                                ShowColumnFooters = true,
                                ShowGroupFooters = false
                            });
                        rgvReqSummary.Export(stream,
                        new GridViewExportOptions()
                        {
                            Format = ExportFormat.Csv,
                            ShowColumnHeaders = true,
                            ShowColumnFooters = true,
                            ShowGroupFooters = false
                        });
                    }
                }
                Core_AMS.Utilities.WPF.Message("Save complete.");
            }
        }
        public string ExportToPdf(DataTable dt, string fileName, string reportName, bool includeHeader, bool includeHeaderReq)
        {
            string filePath = "";
            try
            {
                string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string pathDownload = System.IO.Path.Combine(pathUser, "Downloads");
                Directory.CreateDirectory(pathDownload);

                //filePath = pathDownload + "\\" + fileName;
                filePath = fileName;

                Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
                document.Open();
                iTextSharp.text.Font font5 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 5, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font header = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font font7 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 7);
                Paragraph para = new Paragraph();
                Paragraph para2 = new Paragraph();
                if (myIssue != null && includeHeader == true)
                {
                    Chunk chunk2 = new Chunk("Issue Name:   " + myIssue.IssueName + "\n" + "Issue Code:    " + myIssue.IssueCode + "\n" +
                        "Report Date:    " + DateTime.Now.ToShortDateString() + "\n" + "User Name:    " + FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserName + "\n" + " " +
                        "\n" + " ", font7);
                    Chunk splitHeader = new Chunk("SPLIT SUMMARY" + "\n" + " ", header);
                    Phrase p = new Phrase();
                    p.AddRange(new List<Chunk>() { chunk2, splitHeader });
                    para.AddRange(p);
                    para.SetLeading(10, 0);
                    para.Alignment = Element.ALIGN_LEFT;
                }
                if (includeHeaderReq == true)
                {
                    Chunk reqHeader = new Chunk("REQ_FLAG SUMMARY" + "\n" + " ", header);
                    Phrase p2 = new Phrase();
                    p2.AddRange(new List<Chunk>() { reqHeader });
                    para2.AddRange(p2);
                    para2.SetLeading(10, 0);
                    para2.Alignment = Element.ALIGN_LEFT;
                }

                PdfPTable table = new PdfPTable(dt.Columns.Count);
                table.WidthPercentage = 100;

                foreach (DataColumn c in dt.Columns)
                {
                    table.AddCell(new Phrase(c.ColumnName, font5));
                }

                foreach (DataRow r in dt.Rows)
                {
                    for (int i = 0; i < r.ItemArray.Count(); i++)
                    {
                        table.AddCell(new Phrase(r[i].ToString(), font5));
                    }
                }

                if (includeHeader == true)
                    document.Add(para);
                if(includeHeaderReq)
                    document.Add(para2);
                document.Add(table);

                document.Close();
            }
            catch
            {
                Core_AMS.Utilities.WPF.Message("An error occurred while exporting your file. Please contact support if error keeps recurring.");
            }

            return filePath;
        }
        private void CombinePDFs(List<string> files, string outputName, string filepath)
        {
            PdfReader reader = null;
            Document sourceDocument = null;
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage;
            string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string pathDownload = System.IO.Path.Combine(pathUser, "Downloads");
            pathDownload += "\\" + outputName;

            sourceDocument = new Document();
            pdfCopyProvider = new PdfCopy(sourceDocument, new System.IO.FileStream(filepath, System.IO.FileMode.Create));

            //Open the output file
            sourceDocument.Open();

            try
            {
                //Loop through the files list
                for (int f = 0; f < files.Count; f++)
                {
                    int pages = Get_Page_Count(files[f]);

                    reader = new PdfReader(files[f]);
                    //Add pages of current file
                    for (int i = 1; i <= pages; i++)
                    {
                        importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                        pdfCopyProvider.AddPage(importedPage);
                    }

                    reader.Close();
                }
                //At the end save the output file
                sourceDocument.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            for (int i = 0; i < files.Count; i++)
            {
                File.Delete(files[i]);
            }
        }
        private int Get_Page_Count(string file)
        {
            using (StreamReader sr = new StreamReader(File.OpenRead(file)))
            {
                Regex regex = new Regex(@"/Type\s*/Page[^s]");
                MatchCollection matches = regex.Matches(sr.ReadToEnd());

                return matches.Count;
            }
        }
        DataTable ConvertToDataTable<TSource>(IEnumerable<TSource> source)
        {
            var props = typeof(TSource).GetProperties();

            var dt = new DataTable();
            dt.Columns.AddRange(
              props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray()
            );

            source.ToList().ForEach(
              i => dt.Rows.Add(props.Select(p => p.GetValue(i, null)).ToArray())
            );

            return dt;
        }
        #endregion
    }
}
