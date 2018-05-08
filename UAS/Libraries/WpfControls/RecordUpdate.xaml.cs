using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Core_AMS.Utilities;
using KM.Common.Functions;
using Microsoft.Win32;
using Telerik.Windows.Controls;
using WpfControls.Helpers;

namespace WpfControls
{
    /// <summary>
    /// Interaction logic for RecordUpdate.xaml
    /// </summary>
    public partial class RecordUpdate : UserControl
    {
        #region Service Calls
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IReports> reportWorker = FrameworkServices.ServiceClient.UAD_ReportsClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProductSubscription> subscriptionWorker = FrameworkServices.ServiceClient.UAD_ProductSubscriptionClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IIssueArchiveProductSubscription> issueProductSubscriptionWorker = FrameworkServices.ServiceClient.UAD_IssueArchiveProductSubscriptionClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IIssueArchiveProductSubscriptionDetail> issueProductSubscriptionDetailWorker = FrameworkServices.ServiceClient.UAD_IssueArchiveProductSubscriptionDetailClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IArchivePubSubscriptionsExtension> archPubSubExtensionWorker = FrameworkServices.ServiceClient.UAD_ArchivePubSubscriptionsExtensionClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.ICodeSheet> codeSheetWorker = FrameworkServices.ServiceClient.UAD_CodeSheetClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IResponseGroup> responseGroupWorker = FrameworkServices.ServiceClient.UAD_ResponseGroupClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IEmailStatus> emailStatusWorker = FrameworkServices.ServiceClient.UAD_EmailStatusClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProductSubscription> productSubscriptionWorker = FrameworkServices.ServiceClient.UAD_ProductSubscriptionClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IPubSubscriptionDetail> pubSubscriptionDetailWorker = FrameworkServices.ServiceClient.UAD_PubSubscriptionDetailClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProductSubscriptionsExtension> productSubscriptionExtensionWorker = FrameworkServices.ServiceClient.UAD_ProductSubscriptionsExtensionClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICodeType> codeTypeWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICategoryCode> catCodeWorker = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ITransactionCode> tranCodeWorker = FrameworkServices.ServiceClient.UAD_Lookup_TransactionCodeClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IUserLog> userLogWorker = FrameworkServices.ServiceClient.UAS_UserLogClient();
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IApplication> appWorker = FrameworkServices.ServiceClient.UAS_ApplicationClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IIssue> issueWorker = FrameworkServices.ServiceClient.UAD_IssueClient();
        #endregion

        #region Service Response
        private FrameworkUAS.Service.Response<List<int>> subCountResponse = new FrameworkUAS.Service.Response<List<int>>();
        private FrameworkUAS.Service.Response<int> pubSubscriptionIDResponse = new FrameworkUAS.Service.Response<int>();
        private FrameworkUAS.Service.Response<int> issueArchiveSubscriptionIdResponse = new FrameworkUAS.Service.Response<int>();
        private FrameworkUAS.Service.Response<FrameworkUAD.Entity.ProductSubscription> pubSubscriptionResponse = new FrameworkUAS.Service.Response<FrameworkUAD.Entity.ProductSubscription>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ActionProductSubscription>> actionSubscriptionResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ActionProductSubscription>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.CodeSheet>> codeSheetResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.CodeSheet>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>> responseGroupResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.EmailStatus>> emailStatusResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.EmailStatus>>();
        private FrameworkUAS.Service.Response<List<string>> adhocResponse = new FrameworkUAS.Service.Response<List<string>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> codeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CodeType>> codeTypeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CodeType>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>> catCodeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.CategoryCode>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>> tranCodeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.TransactionCode>>();
        private FrameworkUAS.Service.Response<List<KMPlatform.Entity.UserLog>> userLogResponse = new FrameworkUAS.Service.Response<List<KMPlatform.Entity.UserLog>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Issue>> issueResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.Issue>>();
        #endregion

        private ObservableCollection<ProductCollection> productCollectionMain = new ObservableCollection<ProductCollection>();
        private ObservableCollection<ProductCollection> productCollectionOrig = new ObservableCollection<ProductCollection>();
        private ObservableCollection<BulkRecordUpdateDetail> lstBulkRecordUpdateDetail = new ObservableCollection<BulkRecordUpdateDetail>();        
        public ObservableCollection<BulkRecordUpdateDetail> BulkRecordUpdates { get { return lstBulkRecordUpdateDetail; } }
        private ObservableCollection<ChangesAppliedDetail> lstChangesApplied = new ObservableCollection<ChangesAppliedDetail>();

        //FrameworkUAD.BusinessLogic.ProductSubscription psWorker;

        private List<FrameworkUAD.Entity.ActionProductSubscription> subscriptionList = new List<FrameworkUAD.Entity.ActionProductSubscription>();
        private List<FrameworkUAD.Entity.ActionProductSubscription> activeSubsList = new List<FrameworkUAD.Entity.ActionProductSubscription>();        
        private List<FrameworkUAD.Entity.ProductSubscription> psList = new List<FrameworkUAD.Entity.ProductSubscription>();
        private List<FrameworkUAD.Entity.CodeSheet> codeSheetList = new List<FrameworkUAD.Entity.CodeSheet>();
        private List<FrameworkUAD.Entity.ResponseGroup> responseGroupList = new List<FrameworkUAD.Entity.ResponseGroup>();
        private List<FrameworkUAD.Entity.EmailStatus> emailStatusList = new List<FrameworkUAD.Entity.EmailStatus>();
        public List<FrameworkUAD.Entity.Issue> issueList = new List<FrameworkUAD.Entity.Issue>();
        private List<string> adhocList = new List<string>();

        private List<FrameworkUAD_Lookup.Entity.Code> codeList = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.CodeType> codeTypeList = new List<FrameworkUAD_Lookup.Entity.CodeType>();
        private List<FrameworkUAD_Lookup.Entity.CategoryCode> catCodeList = new List<FrameworkUAD_Lookup.Entity.CategoryCode>();
        private List<FrameworkUAD_Lookup.Entity.TransactionCode> tranCodeList = new List<FrameworkUAD_Lookup.Entity.TransactionCode>();

        private List<int> activeSubscriptionIDs = new List<int>();
        private List<int> subscriptionPool = new List<int>();

        private RadBusyIndicator busy = new RadBusyIndicator();
        KMPlatform.Entity.Client myClient;    
        private Guid accessKey;
        private int myPubID;
        private int currentIssueID;
        private List<WindowsAndDialogs.RecordUpdateChangeSelector.AppliedChanges> appliedChanges;
        PropertyInfo[] propertyInfos;

        public RecordUpdate(KMPlatform.Entity.Client client, int ProductID)
        {
            InitializeComponent();

            accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
            myClient = client;
            myPubID = ProductID;
            BulkRecordUpdates.CollectionChanged += OnCollectionChanged;       
            LoadIssues();
            LoadBulkData();
            SetBulkControls();                       
        }

        #region Bulk Methods
        private void LoadBulkRecordUpdates(object sender, RoutedEventArgs e)
        {
            UserControl uc = this.ParentOfType<UserControl>();
            busy = uc.FindChildByType<RadBusyIndicator>();
        }
        public void LoadBulkData()
        {
            actionSubscriptionResponse = subscriptionWorker.Proxy.SelectActionSubscription(accessKey, myPubID, myClient.ClientConnections);
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

            propertyInfos = typeof(FrameworkUAD.Entity.ProductSubscription).GetType().GetProperties();
            codeSheetResponse = codeSheetWorker.Proxy.Select(accessKey, myPubID, myClient.ClientConnections);
            if (Helpers.Common.CheckResponse(codeSheetResponse.Result, codeSheetResponse.Status))
                codeSheetList = codeSheetResponse.Result;
            responseGroupResponse = responseGroupWorker.Proxy.Select(accessKey, myClient.ClientConnections, myPubID);
            if (Helpers.Common.CheckResponse(responseGroupResponse.Result, responseGroupResponse.Status))
                responseGroupList = responseGroupResponse.Result;
            codeResponse = codeWorker.Proxy.Select(accessKey);
            if (Helpers.Common.CheckResponse(codeResponse.Result, codeResponse.Status))
                codeList = codeResponse.Result;
            catCodeResponse = catCodeWorker.Proxy.Select(accessKey);
            if (Helpers.Common.CheckResponse(catCodeResponse.Result, catCodeResponse.Status))
                catCodeList = catCodeResponse.Result;
            tranCodeResponse = tranCodeWorker.Proxy.Select(accessKey);
            if (Helpers.Common.CheckResponse(tranCodeResponse.Result, tranCodeResponse.Status))
                tranCodeList = tranCodeResponse.Result;
            codeTypeResponse = codeTypeWorker.Proxy.Select(accessKey);
            if (Helpers.Common.CheckResponse(codeTypeResponse.Result, codeTypeResponse.Status))
                codeTypeList = codeTypeResponse.Result;
            emailStatusResponse = emailStatusWorker.Proxy.Select(accessKey, myClient.ClientConnections);
            if (Helpers.Common.CheckResponse(emailStatusResponse.Result, emailStatusResponse.Status))
                emailStatusList = emailStatusResponse.Result;
            adhocResponse = productSubscriptionWorker.Proxy.Get_AdHocs(accessKey, myPubID, myClient.ClientConnections);
            if (Helpers.Common.CheckResponse(adhocResponse.Result, adhocResponse.Status))
                adhocList = adhocResponse.Result;
        }
        public void SetBulkControls()
        {
            subscriptionPool = subscriptionList.Select(x => x.PubSubscriptionID).ToList();
            busy.IsBusy = false;
        }
        public void ReturnResults(List<WindowsAndDialogs.RecordUpdateChangeSelector.AppliedChanges> changesApplied, BulkRecordUpdateDetail brud)
        {
            appliedChanges = changesApplied;
            lstChangesApplied.Clear();
            foreach (WindowsAndDialogs.RecordUpdateChangeSelector.AppliedChanges ac in appliedChanges)
            {
                if (ac.AppliedChange.Type.Equals(WindowsAndDialogs.RecordUpdateChangeSelector.ColumnSelectionType.Demographic.ToString(), StringComparison.CurrentCultureIgnoreCase) == true)
                    this.lstChangesApplied.Add(new ChangesAppliedDetail(ac.AppliedChange.DisplayName, ac.display));// string.Join(",", ac.rlbOptions)));
                else
                    this.lstChangesApplied.Add(new ChangesAppliedDetail(ac.AppliedChange.DisplayName, ac.display));// ac.option));
            }            
        }
        public void LoadIssues()
        {
            issueResponse = issueWorker.Proxy.Select(accessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);
            if (Helpers.Common.CheckResponse(issueResponse.Result, issueResponse.Status))
            {
                issueList = issueResponse.Result.Where(x => x.PublicationId == myPubID).ToList();
                rcbIssues.ItemsSource = issueList;
                FrameworkUAD.Entity.Issue currIssue = issueList.Where(x => x.IsComplete == false).FirstOrDefault();
                if (currIssue != null)
                {
                    currIssue.IssueId = 0;
                    rcbIssues.SelectedIndex = issueList.IndexOf(currIssue);
                    currentIssueID = currIssue.IssueId;
                }
                else
                    Core_AMS.Utilities.WPF.MessageError("There is no issue created for this publication. Please go to Open Close and open an issue before running reports.");
            }
        }
        #endregion

        #region Bulk Custom Functions        
        private static List<int> getNth(int TotalRecords, int RequestedRecords)
        {
            List<int> listNth = new List<int>();

            if (RequestedRecords == 0)
                RequestedRecords = TotalRecords;

            double inccounter = (double) TotalRecords / RequestedRecords;

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
        #endregion

        #region Bulk Button Clicks
        private void btnGenerateFilters_Click(object sender, RoutedEventArgs e)
        {
            if (BulkRecordUpdates.Count > 0)
                MessageBox.Show("Please delete the previously added filters. Only one set of filters can be updated at a time.");
            else
            {
                Window w = Window.GetWindow(this);

                FilterControls.FilterWrapper fw = w.FindChildByType<FilterControls.FilterWrapper>();
                FilterControls.Framework.FiltersViewModel vm = fw.MyViewModel;

                BackgroundWorker bw = new BackgroundWorker();
                busy.IsBusy = true;
                busy.IsIndeterminate = true;
                List<int> subIDs = new List<int>();
                ObservableCollection<FilterOperations.DisplayedFilterDetail> details = new ObservableCollection<FilterOperations.DisplayedFilterDetail>();
                int count = 0;
                bool useArchive = false;
                if (currentIssueID > 0)
                    useArchive = true;

                bw.DoWork += (ev, o) =>
                {
                    FrameworkUAD.Object.ReportingXML obj = new FrameworkUAD.Object.ReportingXML();
                    obj = vm.ActiveFiltersXML;

                    subCountResponse = reportWorker.Proxy.SelectSubscriberCount(accessKey, obj.Filters, obj.AdHocFilters, false, useArchive, currentIssueID, myClient.ClientConnections);
                };
                bw.RunWorkerCompleted += (ev, o) =>
                {
                    busy.IsBusy = false;
                    if (Helpers.Common.CheckResponse(subCountResponse.Result, subCountResponse.Status))
                    {
                        subIDs = subCountResponse.Result;
                    }
                    count = subscriptionList.Where(x => subIDs.Contains(x.PubSubscriptionID)).Select(x => x.Copies).Sum();

                    if (count > 0)
                    {
                        subscriptionPool = subscriptionPool.Except(subIDs).ToList();
                        ObservableCollection<FilterControls.Framework.FilterObject> currentFilterObjects = new ObservableCollection<FilterControls.Framework.FilterObject>();
                        currentFilterObjects = vm.ActiveFilters.DeepClone();                        
                        this.BulkRecordUpdates.Add(new BulkRecordUpdateDetail(details, subscriptionList.Where(x => subIDs.Contains(x.PubSubscriptionID)).ToList(), currentFilterObjects, lstChangesApplied));
                    }
                    else
                        MessageBox.Show("No records to display.");
                };
                bw.RunWorkerAsync();
            }
        }

        private void Delete_BulkRecordUpdates(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            BulkRecordUpdateDetail record = img.DataContext as BulkRecordUpdateDetail;
            this.BulkRecordUpdates.Remove(record);
            subscriptionPool.AddRange(record.SubscriptionList.Select(x => x.PubSubscriptionID));
            if (appliedChanges != null)
                appliedChanges.Clear();

            if (lstChangesApplied != null)
                lstChangesApplied.Clear();

        }

        private void Apply_BulkRecordUpdates(object sender, RoutedEventArgs e)
        {
            if (lstBulkRecordUpdateDetail.Count > 0 && appliedChanges != null && appliedChanges.Count > 0)
            {
                string changes = "";// string.Join(",", appliedChanges.Select(x => x.AppliedChange.DisplayName).ToString() + "=" + appliedChanges.Select(x => x.display).ToString());
                foreach (WindowsAndDialogs.RecordUpdateChangeSelector.AppliedChanges ac in appliedChanges)
                {
                    changes += "\n" + ac.AppliedChange.DisplayName + " = " + ac.display;
                }
                changes = changes.TrimStart(',');
                MessageBoxResult areYouSureDelete = MessageBox.Show("Are you sure you want to apply these changes to the following?\n\n" + changes + "", "Warning", MessageBoxButton.YesNo);

                if (areYouSureDelete == MessageBoxResult.Yes)
                {
                    int issueId = currentIssueID;
                    List<int> PubSubIDs = new List<int>();
                    List<int> batchPubSubIDs = new List<int>();
                    foreach (BulkRecordUpdateDetail ard in grdBulkDetail.Items)
                    {
                        PubSubIDs.AddRange(ard.PendingSubscriptionList);
                    }
                    HashSet<int> pubSubscriptionIds = new HashSet<int>(PubSubIDs);

                    string groupTransCode = Core_AMS.Utilities.StringFunctions.GenerateProcessCode();

                    FrameworkUAS.Service.Response<bool> response = new FrameworkUAS.Service.Response<bool>();

                    busy.IsBusy = true;
                    busy.IsIndeterminate = true;
                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += (o, ea) =>
                    {
                        try
                        {
                            bool isCurrent = true;
                            if (issueId > 0)
                                isCurrent = false;

                            #region Record Update XML

                            string recordUpdateXML = "<RecordUpdate>";

                            foreach (WindowsAndDialogs.RecordUpdateChangeSelector.AppliedChanges ac in appliedChanges)
                            {
                                string table = "";
                                string column = "";
                                string value = "";
                                string other = "";
                                string responseGroupID = "0";
                                string isMultiple = "false";
                                bool useQDateForDemoDate = ac.useQDateForDate;
                                int regionid = 0;
                                int countryid = 0;

                                #region Set New Values
                                if (ac.AppliedChange.Type == WindowsAndDialogs.RecordUpdateChangeSelector.ColumnSelectionType.Standard.ToString())
                                {
                                    #region Standard
                                    if (isCurrent)
                                        table = "PubSubscriptions";
                                    else
                                        table = "IssueArchiveProductSubscription";

                                    #region Standard Columns in PubSubscription
                                    int myIntValue = -1;
                                    bool myBoolValue = false;
                                    DateTime myDateValue = DateTime.Parse("1/1/1900");
                                    Guid myGuid = Guid.Empty;
                                    switch (ac.AppliedChange.DisplayName.ToLower())
                                    {
                                        case "demo7":
                                            column = "Demo7";
                                            value = ac.option;
                                            break;
                                        case "qualificationdate":
                                            DateTime.TryParse(ac.option, out myDateValue);
                                            column = "QualificationDate";
                                            value = myDateValue.ToString();
                                            break;
                                        case "pubqsourceid":
                                            int.TryParse(ac.option, out myIntValue);
                                            column = "PubQSourceID";
                                            value = myIntValue.ToString();
                                            break;
                                        case "pubcategoryid":
                                            int.TryParse(ac.option, out myIntValue);
                                            column = "PubCategoryID";
                                            value = myIntValue.ToString();
                                            break;
                                        case "pubtransactionid":
                                            int.TryParse(ac.option, out myIntValue);
                                            column = "PubTransactionID";
                                            value = myIntValue.ToString();
                                            break;
                                        case "email":
                                            column = "Email";
                                            value = ac.option;
                                            break;
                                        case "copies":
                                            int.TryParse(ac.option, out myIntValue);
                                            column = "Copies";
                                            value = myIntValue.ToString();
                                            break;
                                        case "graceissues":
                                            int.TryParse(ac.option, out myIntValue);
                                            column = "GraceIssues";
                                            value = myIntValue.ToString();
                                            break;
                                        case "onbehalfof":
                                            column = "OnBehalfOf";
                                            value = ac.option;
                                            break;
                                        case "subscribersourcecode":
                                            column = "SubscriberSourceCode";
                                            value = ac.option;
                                            break;
                                        case "origssrc":
                                            column = "OrigsSrc";
                                            value = ac.option;
                                            break;
                                        case "verify":
                                            column = "Verify";
                                            value = ac.option;
                                            break;
                                        case "occupation":
                                            column = "Occupation";
                                            value = ac.option;
                                            break;
                                        case "carrierroute":
                                            column = "CarrierRoute";
                                            value = ac.option;
                                            break;
                                        case "county":
                                            column = "County";
                                            value = ac.option;
                                            break;
                                        case "phone":
                                            column = "Phone";
                                            value = ac.option;
                                            break;
                                        case "fax":
                                            column = "Fax";
                                            value = ac.option;
                                            break;
                                        case "mobile":
                                            column = "Mobile";
                                            value = ac.option;
                                            break;
                                        case "website":
                                            column = "Website";
                                            value = ac.option;
                                            break;
                                        case "birthdate":
                                            DateTime.TryParse(ac.option, out myDateValue);
                                            column = "Birthdate";
                                            value = myDateValue.ToString();
                                            break;
                                        case "age":
                                            int.TryParse(ac.option, out myIntValue);
                                            column = "Age";
                                            value = myIntValue.ToString();
                                            break;
                                        case "income":
                                            column = "Income";
                                            value = ac.option;
                                            break;
                                        case "gender":
                                            column = "Gender";
                                            value = ac.option;
                                            break;
                                        case "phoneext":
                                            column = "PhoneExt";
                                            value = ac.option;
                                            break;
                                        case "reqflag":
                                            int.TryParse(ac.option, out myIntValue);
                                            column = "ReqFlag";
                                            value = myIntValue.ToString();
                                            break;
                                        case "mailpermission":
                                            if (ac.option != null && !string.IsNullOrEmpty(ac.option) && (ac.option.Equals("true", StringComparison.CurrentCultureIgnoreCase) || ac.option.Equals("false", StringComparison.CurrentCultureIgnoreCase)))
                                            {
                                                bool.TryParse(ac.option.ToString(), out myBoolValue);
                                                column = "MailPermission";
                                                value = myBoolValue.ToString();
                                            }
                                            else
                                                column = "MailPermission";
                                            value = "null";
                                            break;
                                        case "faxpermission":
                                            if (ac.option != null && !string.IsNullOrEmpty(ac.option) && (ac.option.Equals("true", StringComparison.CurrentCultureIgnoreCase) || ac.option.Equals("false", StringComparison.CurrentCultureIgnoreCase)))
                                            {
                                                bool.TryParse(ac.option.ToString(), out myBoolValue);
                                                column = "FaxPermission";
                                                value = myBoolValue.ToString();
                                            }
                                            else
                                                column = "FaxPermission";
                                            value = "null";
                                            break;
                                        case "phonepermission":
                                            if (ac.option != null && !string.IsNullOrEmpty(ac.option) && (ac.option.Equals("true", StringComparison.CurrentCultureIgnoreCase) || ac.option.Equals("false", StringComparison.CurrentCultureIgnoreCase)))
                                            {
                                                bool.TryParse(ac.option.ToString(), out myBoolValue);
                                                column = "PhonePermission";
                                                value = myBoolValue.ToString();
                                            }
                                            else
                                                column = "PhonePermission";
                                            value = "null";
                                            break;
                                        case "otherproductspermission":
                                            if (ac.option != null && !string.IsNullOrEmpty(ac.option) && (ac.option.Equals("true", StringComparison.CurrentCultureIgnoreCase) || ac.option.Equals("false", StringComparison.CurrentCultureIgnoreCase)))
                                            {
                                                bool.TryParse(ac.option.ToString(), out myBoolValue);
                                                column = "OtherProductsPermission";
                                                value = myBoolValue.ToString();
                                            }
                                            else
                                                column = "OtherProductsPermission";
                                            value = "null";
                                            break;
                                        case "thirdpartypermission":
                                            if (ac.option != null && !string.IsNullOrEmpty(ac.option) && (ac.option.Equals("true", StringComparison.CurrentCultureIgnoreCase) || ac.option.Equals("false", StringComparison.CurrentCultureIgnoreCase)))
                                            {
                                                bool.TryParse(ac.option.ToString(), out myBoolValue);
                                                column = "ThirdPartyPermission";
                                                value = myBoolValue.ToString();
                                            }
                                            else
                                                column = "ThirdPartyPermission";
                                            value = "null";
                                            break;
                                        case "emailrenewpermission":
                                            if (ac.option != null && !string.IsNullOrEmpty(ac.option) && (ac.option.Equals("true", StringComparison.CurrentCultureIgnoreCase) || ac.option.Equals("false", StringComparison.CurrentCultureIgnoreCase)))
                                            {
                                                bool.TryParse(ac.option.ToString(), out myBoolValue);
                                                column = "EmailRenewPermission";
                                                value = myBoolValue.ToString();
                                            }
                                            else
                                                column = "EmailRenewPermission";
                                            value = "null";
                                            break;
                                        case "textpermission":
                                            if (ac.option != null && !string.IsNullOrEmpty(ac.option) && (ac.option.Equals("true", StringComparison.CurrentCultureIgnoreCase) || ac.option.Equals("false", StringComparison.CurrentCultureIgnoreCase)))
                                            {
                                                bool.TryParse(ac.option.ToString(), out myBoolValue);
                                                column = "TextPermission";
                                                value = myBoolValue.ToString();
                                            }
                                            else
                                                column = "TextPermission";
                                            value = "null";
                                            break;
                                        case "emailstatusid":
                                            int.TryParse(ac.option, out myIntValue);
                                            column = "EmailStatusID";
                                            value = myIntValue.ToString();
                                            break;
                                        case "membergroup":
                                            column = "MemberGroup";
                                            value = ac.option;
                                            break;
                                        case "par3cid":
                                            int.TryParse(ac.option, out myIntValue);
                                            column = "Par3CID";
                                            value = myIntValue.ToString();
                                            break;
                                        case "subsrcid":
                                            int.TryParse(ac.option, out myIntValue);
                                            column = "SubSrcID";
                                            value = myIntValue.ToString();
                                            break;
                                    }
                                    #endregion

                                    recordUpdateXML += "<FieldName>";
                                    recordUpdateXML += "<Table>" + table + "</Table>";
                                    recordUpdateXML += "<Column>" + column + "</Column>";
                                    recordUpdateXML += "<Values>";
                                    recordUpdateXML += "<Detail>";
                                    recordUpdateXML += "<Value>" + value + "</Value>";
                                    recordUpdateXML += "<UseQDateForDemo>" + useQDateForDemoDate.ToString() + "</UseQDateForDemo>";
                                    recordUpdateXML += "</Detail>";
                                    recordUpdateXML += "</Values>";
                                    recordUpdateXML += "<OtherText>" + other + "</OtherText>";
                                    recordUpdateXML += "<ResponseGroupID>" + responseGroupID + "</ResponseGroupID>";
                                    recordUpdateXML += "</FieldName>";
                                    #endregion
                                }
                                else if (ac.AppliedChange.Type == WindowsAndDialogs.RecordUpdateChangeSelector.ColumnSelectionType.Demographic.ToString())
                                {
                                    #region Demographic                                    
                                    if (isCurrent)
                                        table = "PubSubscriptionDetail";
                                    else
                                        table = "IssueArchiveProductSubscriptionDetail";

                                    column = ac.AppliedChange.DisplayName;
                                    responseGroupID = ac.AppliedChange.ResponseGroupID.ToString();
                                    isMultiple = ac.AppliedChange.IsMultiple.ToString();

                                    recordUpdateXML += "<FieldName>";
                                    recordUpdateXML += "<Table>" + table + "</Table>";
                                    recordUpdateXML += "<Column>" + column + "</Column>";
                                    recordUpdateXML += "<Values>";
                                    foreach (var option in ac.rlbOptions)
                                    {
                                        value = option;
                                        other = ac.other;
                                        recordUpdateXML += "<Detail>";
                                        recordUpdateXML += "<Value>" + value + "</Value>";
                                        recordUpdateXML += "<UseQDateForDemo>" + useQDateForDemoDate.ToString() + "</UseQDateForDemo>";
                                        recordUpdateXML += "</Detail>";
                                    }
                                    recordUpdateXML += "</Values>";
                                    recordUpdateXML += "<OtherText>" + other + "</OtherText>";
                                    recordUpdateXML += "<ResponseGroupID>" + responseGroupID + "</ResponseGroupID>";
                                    recordUpdateXML += "</FieldName>";
                                    #endregion
                                }
                                else if (ac.AppliedChange.Type == WindowsAndDialogs.RecordUpdateChangeSelector.ColumnSelectionType.Adhoc.ToString())
                                {
                                    #region Adhoc                                    
                                    if (isCurrent)
                                        table = "PubSubscriptionsExtension";
                                    else
                                        table = "IssueArchivePubSubscriptionsExtension";

                                    column = ac.AppliedChange.DisplayName;
                                    value = ac.option;

                                    recordUpdateXML += "<FieldName>";
                                    recordUpdateXML += "<Table>" + table + "</Table>";
                                    recordUpdateXML += "<Column>" + column + "</Column>";
                                    recordUpdateXML += "<Values>";
                                    recordUpdateXML += "<Detail>";
                                    recordUpdateXML += "<Value>" + value + "</Value>";
                                    recordUpdateXML += "<UseQDateForDemo>" + useQDateForDemoDate.ToString() + "</UseQDateForDemo>";
                                    recordUpdateXML += "</Detail>";
                                    recordUpdateXML += "</Values>";
                                    recordUpdateXML += "<OtherText>" + other + "</OtherText>";
                                    recordUpdateXML += "<ResponseGroupID>" + responseGroupID + "</ResponseGroupID>";
                                    recordUpdateXML += "</FieldName>";
                                    #endregion
                                }
                                #endregion
                            }

                            recordUpdateXML += "</RecordUpdate>";                            

                            response = productSubscriptionWorker.Proxy.RecordUpdate(accessKey, pubSubscriptionIds, recordUpdateXML, issueId, myPubID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID, myClient.ClientConnections);                            
                            #endregion                            
                        }
                        catch (Exception ex)
                        {
                            FrameworkServices.ServiceClient<UAS_WS.Interface.IApplicationLog> alClient = FrameworkServices.ServiceClient.UAS_ApplicationLogClient();
                            Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
                            KMPlatform.BusinessLogic.Enums.Applications app = KMPlatform.BusinessLogic.Enums.GetApplication(FrameworkUAS.Object.AppData.myAppData.CurrentApp.ApplicationName);
                            int logClientId = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientID;
                            string formatException = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                            alClient.Proxy.LogCriticalError(accessKey, formatException, this.GetType().Name.ToString() + ".Apply_BulkRecordUpdates", app, string.Empty, logClientId);

                            MessageBox.Show("Error applying changes to data.");
                        }
                    };
                    bw.RunWorkerCompleted += (o, ea) =>
                    {
                        busy.IsBusy = false;                        
                        if (response.Result != null && response.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                            MessageBox.Show("Record Updates Applied.");
                        else
                            MessageBox.Show("Record Update failed.");
                                                       
                    };
                    bw.RunWorkerAsync();
                }
            }
        }

        private void btnDownloadDetails_Click(object sender, RoutedEventArgs e)
        {
            if (lstBulkRecordUpdateDetail.Count > 0)
            {
                string columns = "";
                List<int> subIDs = new List<int>();
                foreach (BulkRecordUpdateDetail ard in grdBulkDetail.Items)
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
                            if (currentIssueID > 0)                            
                                dt = subscriptionWorker.Proxy.SelectForExportStatic(accessKey, myPubID, currentIssueID, columns, temp, myClient.ClientConnections).Result;                            
                            else
                                dt = subscriptionWorker.Proxy.SelectForExportStatic(accessKey, myPubID, columns, temp, myClient.ClientConnections).Result;

                            rowProcessedCount += dt.Rows.Count;

                            dt.AcceptChanges();
                            master.Merge(dt);
                        }
                        #endregion
                    };
                    bw.RunWorkerCompleted += (o, ea) =>
                    {
                        if (master.Columns.Contains("PubSubscriptionID"))
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

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void AppliedChanges_Click(object sender, RoutedEventArgs e)
        {
            Telerik.Windows.Controls.RadButton button = (Telerik.Windows.Controls.RadButton) e.Source;
            WpfControls.RecordUpdate.BulkRecordUpdateDetail brud = (WpfControls.RecordUpdate.BulkRecordUpdateDetail) button.DataContext;            
            object a = new FrameworkUAD.Entity.ProductSubscription();
            PropertyInfo[] pi = a.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            WindowsAndDialogs.RecordUpdateChangeSelector rucs = new WindowsAndDialogs.RecordUpdateChangeSelector(this, brud, myPubID, appliedChanges, pi.Select(x => x.Name).ToList(), codeSheetList, responseGroupList, codeList, catCodeList, tranCodeList, adhocList, emailStatusList, codeTypeList);
            rucs.ShowDialog();
        }
        #endregion

        #region Bulk Items
        public class BulkRecordUpdateDetail : INotifyPropertyChanged
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
                    PendingSubscriptionList = GetNthSubscribers(SubscriptionList.Count, _desiredCount, SubscriptionList.Select(x => x.PubSubscriptionID).ToList());
                    if (null != this.PropertyChanged)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("DesiredCount"));
                    }
                }
            }
            public List<FrameworkUAD.Entity.ActionProductSubscription> SubscriptionList { get; set; }
            public List<int> PendingSubscriptionList { get; set; }
            public List<FilterControls.Framework.FilterObject> Filters { get; set; }
            public ObservableCollection<Helpers.FilterOperations.DisplayedFilterDetail> FilterDetails { get; set; }
            public ObservableCollection<ChangesAppliedDetail> ChangesAppliedDetails { get; set; }

            public BulkRecordUpdateDetail(ObservableCollection<FilterOperations.DisplayedFilterDetail> filterDetails, List<FrameworkUAD.Entity.ActionProductSubscription> subs,
                ObservableCollection<FilterControls.Framework.FilterObject> filters, ObservableCollection<ChangesAppliedDetail> changes)
            {
                this.ActualCount = subs.Count;
                this.FilterDetails = filterDetails;
                this.SubscriptionList = subs;
                this.Filters = filters.ToList();
                this.DesiredCount = subs.Count;
                this.ChangesAppliedDetails = changes;
            }
            public BulkRecordUpdateDetail() { }

            public event PropertyChangedEventHandler PropertyChanged;
        }
        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (BulkRecordUpdateDetail newItem in e.NewItems)
                {
                    //Add listener for each item on PropertyChanged event
                    newItem.PropertyChanged += this.OnItemPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (BulkRecordUpdateDetail oldItem in e.OldItems)
                {
                    oldItem.PropertyChanged -= this.OnItemPropertyChanged;
                }
            }
            UpdateGridTotals();
        }
        void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateGridTotals();
        }
        public void UpdateGridTotals()
        {
            grdBulkDetail.CalculateAggregates();
        }
        #endregion

        #region ChangeName
        public class ChangesAppliedDetail : INotifyPropertyChanged
        {
            private string _column;
            private string _answer;

            public string Column
            {
                get { return _column; }
                set
                {
                    if (_column != value)
                    {
                        _column = value;
                        OnPropertyChanged("Column");
                    }
                }
            }
            public string Answer
            {
                get { return _answer; }
                set
                {
                    if (_answer != value)
                    {
                        _answer = value;
                        OnPropertyChanged("Answer");
                    }
                }
            }

            public ChangesAppliedDetail(string column, string answer)
            {
                this.Column = column;
                this.Answer = answer;
            }
            public ChangesAppliedDetail() { }

            private void OnPropertyChanged(string propertyName)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }
        #endregion

        #region Solo Custom Functions
        public List<Variance> DC(FrameworkUAD.Entity.ProductSubscription main, FrameworkUAD.Entity.ProductSubscription orig)
        {
            List<Variance> variances = new List<Variance>();
            foreach (var oProperty in main.GetType().GetProperties())
            {
                var oOldValue = oProperty.GetValue(orig, null);
                var oNewValue = oProperty.GetValue(main, null);
                // this will handle the scenario where either value is null
                if (!object.Equals(oOldValue, oNewValue))
                {
                    // Handle the display values when the underlying value is null
                    var sOldValue = oOldValue == null ? "null" : oOldValue.ToString();
                    var sNewValue = oNewValue == null ? "null" : oNewValue.ToString();

                    variances.Add(new Variance { PubSubscriptionID = main.PubSubscriptionID, Prop = oProperty.Name, MainVal = sNewValue, OrigVal = sOldValue });
                    //System.Diagnostics.Debug.WriteLine("Property " + oProperty.Name + " was: " + sOldValue + "; is: " + sNewValue);
                }
            }
            return variances;
        }
        #endregion

        #region Solo Grid Methods
        private void grdPubSubscriptions_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            //var newData = e.NewData;
            //var oldData = e.OldData;
            //if (newData != oldData)
            //{
            //    Telerik.Windows.Controls.RadGridView grid = e.Source as Telerik.Windows.Controls.RadGridView;
            //    var converter = new System.Windows.Media.BrushConverter();
            //    var brush = (System.Windows.Media.Brush) converter.ConvertFromString("#FFCCCC");
            //    grid.CurrentCell.Background = brush;
            //}
        }

        private void grdPubSubscriptions_AutoGeneratingColumn(object sender, Telerik.Windows.Controls.GridViewAutoGeneratingColumnEventArgs e)
        {
            //if ((string) e.Column.Header == "productSubscription")
            //{
            //    e.Cancel = true;
            //}
        }
        #endregion        

        #region Solo Button Clicks
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //grdPubSubscriptions.CommitEdit();
            //foreach (ProductCollection pcMain in productCollectionMain)
            //{
            //    if (productCollectionOrig.FirstOrDefault(x => x.productSubscription.PubSubscriptionID == pcMain.productSubscription.PubSubscriptionID) != null)
            //    {
            //        FrameworkUAD.Entity.ProductSubscription pcOrig = productCollectionOrig.FirstOrDefault(x => x.productSubscription.PubSubscriptionID == pcMain.productSubscription.PubSubscriptionID).productSubscription;

            //        //DetailedCompare<FrameworkUAD.Entity.ProductSubscription> dc = new DetailedCompare<FrameworkUAD.Entity.ProductSubscription>();                    
            //        List<Variance> variances = DC(pcMain.productSubscription, pcOrig);
            //    }
            //}
        }
        #endregion

        private void rcbIssues_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int newIssueID = 0;
            if (rcbIssues.SelectedValue != null)
                int.TryParse(rcbIssues.SelectedValue.ToString(), out newIssueID);

            if (lstBulkRecordUpdateDetail.Count > 0 && (rcbIssues.SelectedValue != null && currentIssueID != newIssueID))
            {
                Core_AMS.Utilities.WPF.MessageError("Please delete the selected filters before selecting a different issue.");
                FrameworkUAD.Entity.Issue previousIssue = issueList.Where(x => x.IssueId == currentIssueID).FirstOrDefault();
                if (previousIssue != null)
                    rcbIssues.SelectedIndex = issueList.IndexOf(previousIssue);

            }
            else
            { 
                if (rcbIssues.SelectedValue != null)
                {
                    FrameworkUAD.Entity.Issue selectedIssue = rcbIssues.SelectedItem as FrameworkUAD.Entity.Issue;
                    int.TryParse(rcbIssues.SelectedValue.ToString(), out currentIssueID);
                    if (currentIssueID > 0)
                    {
                        actionSubscriptionResponse = subscriptionWorker.Proxy.SelectArchiveActionSubscription(accessKey, myPubID, currentIssueID, myClient.ClientConnections);
                        if (Helpers.Common.CheckResponse(actionSubscriptionResponse.Result, actionSubscriptionResponse.Status))
                        {
                            subscriptionList = actionSubscriptionResponse.Result;
                            activeSubsList = subscriptionList.Where(x => x.CategoryCodeValue != 70 && x.CategoryCodeValue != 71 &&
                                x.CategoryType != FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Paid.ToString().Replace("_", " ") &&
                                x.CategoryType != FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Free.ToString().Replace("_", " ") &&
                                x.TransactionType != FrameworkUAD_Lookup.Enums.TransactionCodeType.Free_Inactive.ToString().Replace("_", " ") &&
                                x.TransactionType != FrameworkUAD_Lookup.Enums.TransactionCodeType.Paid_Inactive.ToString().Replace("_", " ")).ToList();
                            activeSubscriptionIDs = activeSubsList.Select(x => x.PubSubscriptionID).ToList();
                            subscriptionPool = subscriptionList.Select(x => x.PubSubscriptionID).ToList();
                        }
                    }
                    else
                    {
                        actionSubscriptionResponse = subscriptionWorker.Proxy.SelectActionSubscription(accessKey, myPubID, myClient.ClientConnections);
                        if (Helpers.Common.CheckResponse(actionSubscriptionResponse.Result, actionSubscriptionResponse.Status))
                        {
                            subscriptionList = actionSubscriptionResponse.Result;
                            activeSubsList = subscriptionList.Where(x => x.CategoryCodeValue != 70 && x.CategoryCodeValue != 71 &&
                                x.CategoryType != FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Paid.ToString().Replace("_", " ") &&
                                x.CategoryType != FrameworkUAD_Lookup.Enums.CategoryCodeType.NonQualified_Free.ToString().Replace("_", " ") &&
                                x.TransactionType != FrameworkUAD_Lookup.Enums.TransactionCodeType.Free_Inactive.ToString().Replace("_", " ") &&
                                x.TransactionType != FrameworkUAD_Lookup.Enums.TransactionCodeType.Paid_Inactive.ToString().Replace("_", " ")).ToList();
                            activeSubscriptionIDs = activeSubsList.Select(x => x.PubSubscriptionID).ToList();
                            subscriptionPool = subscriptionList.Select(x => x.PubSubscriptionID).ToList();
                        }
                    }
                }
                else
                {
                    Core_AMS.Utilities.WPF.MessageError("Error selecting issue.");
                }
            }
        }
    }

    #region Custom Class
    public class Variance
    {
        public int PubSubscriptionID { get; set; }
        public string Prop { get; set; }
        public object MainVal { get; set; }
        public object OrigVal { get; set; }
    }
    public class ArchivePubSubscriptionAdHoc
    {
        public ArchivePubSubscriptionAdHoc()
        {

        }
        public ArchivePubSubscriptionAdHoc(int id, string field, string val)
        {
            this.PubSubscriptionID = id;
            this.AdHocField = field;
            this.Value = val;
        }
        public int PubSubscriptionID
        {
            get;
            set;
        }
        public string AdHocField
        {
            get;
            set;
        }
        public string Value
        {
            get;
            set;
        }
    }
    #endregion
    #region Observable Class    
    [Serializable]
    public class ProductCollection : INotifyPropertyChanged, ICloneable
    {
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public ProductCollection Clone()
        {
            return (ProductCollection) this.MemberwiseClone();
        }

        public ProductCollection(FrameworkUAD.Entity.ProductSubscription ps)
        {
            productSubscription = new FrameworkUAD.Entity.ProductSubscription(ps);
        }

        #region Properties
        private FrameworkUAD.Entity.ProductSubscription _ProductSubscription;
        #endregion

        #region Getters and Setters
        public FrameworkUAD.Entity.ProductSubscription productSubscription
        {
            get { return this._ProductSubscription; }
            set
            {
                if (this._ProductSubscription != value)
                    this._ProductSubscription = value;
            }
        }
        public string FirstName
        {
            get { return this._ProductSubscription.FirstName; }
            set
            {
                if (this._ProductSubscription.FirstName != value)
                    this._ProductSubscription.FirstName = value;
            }
        }
        #endregion

        #region EventHandlers
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Events
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        #endregion        
    }
    #endregion
}
