using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Circulation.Modules
{
    /// <summary>
    /// Interaction logic for BatchNew.xaml
    /// </summary>
    public partial class BatchNew : UserControl
    {
        //Batch exists in a tab within SubscriptionContainer. It tracks History changes to PubSubscriptions, PubSubscriptionDetails, SubscriptionPaid, PaidBillTo, and PubSubscriptionsExtension
        #region Service Response
        private FrameworkUAD.Entity.ProductSubscription myProductSubscription;
        private List<FrameworkUAD.Entity.ResponseGroup> Questions;
        private List<FrameworkUAD.Entity.CodeSheet> Answers;
        private List<FrameworkUAD.Object.BatchHistoryDetail> listBHD;
        private List<FrameworkUAD.Object.BatchHistoryDetail> listBHDHistory;
        private List<KMPlatform.Entity.User> users = new List<KMPlatform.Entity.User>();
        private KMPlatform.Entity.User userName = new KMPlatform.Entity.User();
        private FrameworkUAD_Lookup.Entity.CodeType codeTypeVar = new FrameworkUAD_Lookup.Entity.CodeType();
        #endregion
        #region List/Entity
        private List<FrameworkUAD_Lookup.Entity.Action> actionList;
        private List<FrameworkUAD_Lookup.Entity.CategoryCode> catCodeList;
        private List<FrameworkUAD_Lookup.Entity.CategoryCodeType> catTypeList;
        private List<FrameworkUAD_Lookup.Entity.TransactionCode> transList;
        private List<FrameworkUAD_Lookup.Entity.SubscriptionStatus> sStatusList;
        private List<FrameworkUAD_Lookup.Entity.Code> qSourceList;
        private List<FrameworkUAD_Lookup.Entity.Code> parList;
        private List<FrameworkUAD_Lookup.Entity.Code> codeList;
        private List<FrameworkUAD_Lookup.Entity.Code> marketingList;
        private List<FrameworkUAD_Lookup.Entity.Country> countryList;
        private List<FrameworkUAD_Lookup.Entity.Region> regionList;
        private KMPlatform.Entity.Client myClient;
        private FrameworkUAD.Entity.Product myProduct;
        private List<HistoryContainer> history = new List<HistoryContainer>();
        #endregion
        #region Worker
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.IAction> aWorker;
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICategoryCode> ccWorker;
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICategoryCodeType> ccTypeW = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeTypeClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ITransactionCode> tWorker;
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ISubscriptionStatus> stWorker;
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> qsWorker;
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> parWorker;
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> sstworker;
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> mWorker;
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IBatchHistoryDetail> bhdWorker;
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IUser> uWorker;
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICountry> countryWorker;
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.IRegion> regionWorker;
        private FrameworkServices.ServiceClient<UAS_WS.Interface.IClient> clientWorker;
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IProduct> productWorker;
        #endregion

        private Guid accessKey = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey;
        private List<int> respId = new List<int>();
        private List<int> uniqueBatchId = new List<int>();

        public BatchNew(
               List<FrameworkUAD.Entity.ResponseGroup> rtType,
               List<FrameworkUAD.Entity.CodeSheet> response,
               KMPlatform.Entity.Client client,
               FrameworkUAD.Entity.Product product,
               FrameworkUAD.Entity.ProductSubscription pubSubscription)
        {
            InitializeComponent();

            actionList = Home.Actions;
            catCodeList = Home.CategoryCodes;
            transList = Home.TransactionCodes;
            sStatusList = Home.SubscriptionStatuses;
            qSourceList = Home.QSourceCodes;
            parList = Home.Par3CCodes;
            codeList = Home.Codes;
            marketingList = Home.MarketingCodes;
            Questions = rtType;
            Answers = response;
            countryList = Home.Countries;
            regionList = Home.Regions;
            catTypeList = Home.CategoryCodeTypes;
            myClient = client;
            myProduct = product;

            aWorker = FrameworkServices.ServiceClient.UAD_Lookup_ActionClient();
            ccWorker = FrameworkServices.ServiceClient.UAD_Lookup_CategoryCodeClient();
            tWorker = FrameworkServices.ServiceClient.UAD_Lookup_TransactionCodeClient();
            stWorker = FrameworkServices.ServiceClient.UAD_Lookup_SubscriptionStatusClient();
            qsWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
            parWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
            sstworker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
            mWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
            bhdWorker = FrameworkServices.ServiceClient.UAD_BatchHistoryDetailClient();
            uWorker = FrameworkServices.ServiceClient.UAS_UserClient();
            countryWorker = FrameworkServices.ServiceClient.UAD_Lookup_CountryClient();
            regionWorker = FrameworkServices.ServiceClient.UAD_Lookup_RegionClient();
            productWorker = FrameworkServices.ServiceClient.UAD_ProductClient();

            BindGrid(pubSubscription);
        }

        private void BindGrid(FrameworkUAD.Entity.ProductSubscription subscription)
        {
            List<Helpers.Common.HistoryData> hist = new List<Helpers.Common.HistoryData>();
            StringBuilder sb = new StringBuilder();
            myProductSubscription = subscription;

            if (myProductSubscription != null)
            {
                #region Get Data

                if (users == null || users.Count == 0)
                {
                    users = uWorker.Proxy.Select(accessKey).Result;
                }

                FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICodeType> ctWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeTypeClient();
                FrameworkUAS.Service.Response<FrameworkUAD_Lookup.Entity.CodeType> ctResponse = new FrameworkUAS.Service.Response<FrameworkUAD_Lookup.Entity.CodeType>();
                ctResponse = ctWorker.Proxy.Select(accessKey, FrameworkUAD_Lookup.Enums.CodeType.Deliver);
                if (ctResponse.Result != null && ctResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
                {
                    codeTypeVar = ctResponse.Result;
                }

                #endregion
                #region Get History
                if (myProductSubscription != null)
                {
                    dgSubHistory.Visibility = Visibility.Visible;

                    listBHD = bhdWorker.Proxy.SelectForSubscriber(accessKey, myProductSubscription.PubSubscriptionID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.DisplayName).Result;
                    listBHDHistory = new List<FrameworkUAD.Object.BatchHistoryDetail>();

                    string fullName = string.Empty;
                    string userLogType = string.Empty;
                    int batchNumber = 0;
                    DateTime batchDateCreated = DateTime.Now;
                    DateTime historyDateCreated = DateTime.Now;
                    var batchNum = listBHD.GroupBy(b => b.BatchID).Select(x => x.Key).ToList();
                    List<string> prChanges = new List<string>();
                    List<string> dChanges = new List<string>();
                    List<string> paChanges = new List<string>();

                    foreach (var b in batchNum)
                    {
                        var historySubID = listBHD.Where(x => x.BatchID == b).GroupBy(k => k.HistorySubscriptionID).Select(m => m.Key).ToList();
                        foreach (var hs in historySubID)
                        {
                            prChanges = new List<string>();
                            dChanges = new List<string>();
                            paChanges = new List<string>();
                            userName = new KMPlatform.Entity.User();
                            var historyObject = listBHD.Where(x => x.BatchID == b && x.HistorySubscriptionID == hs).GroupBy(k => k.Object).Select(m => m.Key).ToList();
                            foreach (var hObject in historyObject)
                            {
                                foreach (var s in listBHD.Where(h => h.BatchID == b && h.HistorySubscriptionID == hs && h.Object == hObject))
                                {
                                    s.PubCode = myProduct.PubCode;
                                    s.PublicationName = myProduct.PubName;
                                    s.PublisherName = myClient.DisplayName;
                                    batchNumber = s.BatchNumber;

                                    if (userName == null || userName.UserID == 0)
                                    {
                                        userName = users.Where(z => z.UserID == s.UserID).SingleOrDefault();
                                        if (userName != null)
                                        {
                                            fullName = userName.FirstName + " " + userName.LastName;
                                            batchDateCreated = s.BatchDateCreated;
                                            historyDateCreated = s.HistoryDateCreated;
                                            userLogType = s.UserLogTypeName;
                                        }
                                    }

                                    if (s.Object != null && (!string.IsNullOrEmpty(s.ToObjectValues) || s.Object.Equals("ProductSubscriptionDetail") || s.Object.Equals("MarketingMap")))
                                    {
                                        try
                                        {
                                            hist = Helpers.Common.JsonComparer(s.Object, s.FromObjectValues, s.ToObjectValues, codeTypeVar, actionList, catCodeList, catTypeList, transList, sStatusList, qSourceList, parList, codeList,
                                                marketingList, countryList, regionList, Questions, Answers);

                                            foreach (Helpers.Common.HistoryData hd in hist.OrderBy(x=> x.SortIndex))
                                            {
                                                if (s.Object.Equals("ProductSubscription") || s.Object.Equals("PubSubscriptionAdHoc"))
                                                    prChanges.Add(hd.PropertyName + ": " + hd.DisplayText);
                                                else if(s.Object.Equals("ProductSubscriptionDetail") || s.Object.Equals("MarketingMap"))
                                                    dChanges.Add(hd.PropertyName + ": " + hd.DisplayText);
                                                else if(s.Object.Equals("SubscriptionPaid") || s.Object.Equals("PaidBillTo"))
                                                    paChanges.Add(hd.PropertyName + ": " + hd.DisplayText);
                                            }
                                        }
                                        catch { }
                                    }
                                }
                            }
                            if (prChanges.Count > 0 || dChanges.Count > 0 || paChanges.Count > 0)
                            {
                                history.Add(new HistoryContainer(batchNumber, fullName, batchDateCreated, historyDateCreated, userLogType, prChanges, dChanges, paChanges));
                            }
                        }
                    }
                }
                #endregion
                else
                {
                    dgSubHistory.Visibility = Visibility.Visible;
                }
                if(history.Count > 0)
                {
                    dgSubHistory.ItemsSource = history.OrderByDescending(x => x.HistoryDateCreated);
                    dgSubHistory.Rebind();
                }
            }
        }

        public class HistoryContainer
        {
            public int BatchNumber { get; set; }
            public string FullName { get; set; }
            public DateTime BatchDateCreated { get; set; }
            public DateTime HistoryDateCreated { get; set; }
            public string UserLogType { get; set; }
            public List<string> ProfileChanges { get; set; }
            public List<string> DemoChanges { get; set; }
            public List<string> PaidChanges { get; set; }

            public HistoryContainer(int batchNum, string fullName, DateTime batchDate, DateTime historyDate, string logType, List<string> profileChanges, List<string> demoChanges, List<string> paidChanges)
            {
                this.BatchNumber = batchNum;
                this.FullName = fullName;
                this.BatchDateCreated = batchDate;
                this.HistoryDateCreated = historyDate;
                this.UserLogType = logType;
                this.ProfileChanges = profileChanges;
                this.DemoChanges = demoChanges;
                this.PaidChanges = paidChanges;
            }
        }
    }
}
