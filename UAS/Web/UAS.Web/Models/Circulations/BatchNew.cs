using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace UAS.Web.Models.Circulations
{
    //It tracks History changes to PubSubscriptions, PubSubscriptionDetails, SubscriptionPaid, PaidBillTo, and PubSubscriptionsExtension
    public class BatchNew
    {
        #region Entity
        private List<FrameworkUAD.Entity.CodeSheet> answers;
        private KMPlatform.Entity.Client myClient;
        private FrameworkUAD.Entity.Product myProduct;
        private FrameworkUAD.Entity.ProductSubscription myProductSubscription;
        private List<FrameworkUAD.Entity.ResponseGroup> questions;
        private List<FrameworkUAD.Object.BatchHistoryDetail> listBHD;
        private List<FrameworkUAD.Object.BatchHistoryDetail> listBHDHistory;
        private FrameworkUAD_Lookup.Entity.CodeType codeTypeVar = new FrameworkUAD_Lookup.Entity.CodeType();
        private KMPlatform.Entity.User userName = new KMPlatform.Entity.User();
        private List<KMPlatform.Entity.User> users;
        private EntityLists entitylist;
        private List<HistoryContainer> history;
        #endregion

        #region Business Logic
        private KMPlatform.BusinessLogic.User uWorker;
        private FrameworkUAD.BusinessLogic.BatchHistoryDetail bhdWorker;
        #endregion

        public BatchNew(List<FrameworkUAD.Entity.ResponseGroup> questions, List<FrameworkUAD.Entity.CodeSheet> answers, KMPlatform.Entity.Client myClient,
            FrameworkUAD.Entity.Product myProduct, FrameworkUAD.Entity.ProductSubscription myProductSubscription, EntityLists entlst)
        {
            this.questions = questions;
            this.answers = answers;
            this.myClient = myClient;
            this.myProduct = myProduct;
            this.myProductSubscription = myProductSubscription;
            this.users = new List<KMPlatform.Entity.User>();
            this.entitylist = entlst;
            this.history = new List<HistoryContainer>();
        }

        internal List<HistoryContainer> GetHistoryList(FrameworkUAD.Entity.ProductSubscription subscription)
        {
            List<Helpers.Common.HistoryData> hist = new List<Helpers.Common.HistoryData>();
            uWorker = new KMPlatform.BusinessLogic.User();
            bhdWorker = new FrameworkUAD.BusinessLogic.BatchHistoryDetail();
            StringBuilder sb = new StringBuilder();
            myProductSubscription = subscription;

            if (myProductSubscription != null)
            {
                #region Get Data

                if (users == null || users.Count == 0)
                {
                    users = uWorker.SelectByClientID(myClient.ClientID);
                }

                FrameworkUAD_Lookup.BusinessLogic.CodeType ctWorker = new FrameworkUAD_Lookup.BusinessLogic.CodeType();
                codeTypeVar = ctWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Deliver);

                #endregion
                #region Get History
                if (myProductSubscription != null)
                {
                    //dgSubHistory.Visibility = Visibility.Visible;

                    listBHD = bhdWorker.SelectSubscriber(myProductSubscription.PubSubscriptionID, myClient.ClientConnections, myClient.DisplayName);
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
                                            hist = Helpers.Common.JsonComparer(s.Object, s.FromObjectValues, s.ToObjectValues, codeTypeVar, entitylist.actionList, entitylist.categoryCodeList, entitylist.catTypeList, entitylist.transCodeList,
                                                entitylist.sstList, entitylist.qSourceList, entitylist.parList, entitylist.codeList, entitylist.marketingList, entitylist.countryList, entitylist.regions, questions, answers);

                                            foreach (Helpers.Common.HistoryData hd in hist.OrderBy(x => x.SortIndex))
                                            {
                                                if (s.Object.Equals("ProductSubscription") || s.Object.Equals("PubSubscriptionAdHoc"))
                                                    prChanges.Add(hd.PropertyName + ": " + hd.DisplayText);
                                                else if (s.Object.Equals("ProductSubscriptionDetail") || s.Object.Equals("MarketingMap"))
                                                    dChanges.Add(hd.PropertyName + ": " + hd.DisplayText);
                                                else if (s.Object.Equals("SubscriptionPaid") || s.Object.Equals("PaidBillTo"))
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
            }

            return history.OrderByDescending(x => x.HistoryDateCreated).ToList();
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