using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class History
    {       
        public History() 
        {
            UserLogList = new List<int>();
            HistoryResponseList = new List<int>();
            HistoryMarketingMapList = new List<int>();
        }
        public History(int batchID, int batchCountItem, int publicationID, int pubSubscriptionID, int subscriptionID, int historySubscriptionID, int historyPaidID, int userID, int historyPaidBillToID = -1)
        {
            BatchID = batchID;
            BatchCountItem = batchCountItem;
            PublicationID = publicationID;
            PubSubscriptionID = pubSubscriptionID;
            SubscriptionID = subscriptionID;
            HistorySubscriptionID = historySubscriptionID;
            HistoryPaidID = historyPaidID;
            HistoryPaidBillToID = historyPaidBillToID;
            DateCreated = DateTime.Now;
            CreatedByUserID = userID;
            UserLogList = new List<int>();
            HistoryResponseList = new List<int>();
            HistoryMarketingMapList = new List<int>();
        }
        #region Properties
        [DataMember]
        public int HistoryID { get; set; }
        [DataMember]
        public int BatchID { get; set; }
        [DataMember]
        public int BatchCountItem { get; set; }
        [DataMember]
        public int PublicationID { get; set; }
        [DataMember]
        public int PubSubscriptionID { get; set; }
        [DataMember]
        public int SubscriptionID { get; set; }
        [DataMember]
        public int HistorySubscriptionID { get; set; }
        [DataMember]
        public int HistoryPaidID { get; set; }
        [DataMember]
        public int HistoryPaidBillToID { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        #endregion

        [DataMember]
        public List<int> UserLogList { get; set; }
        [DataMember]
        public List<int> HistoryResponseList { get; set; }
        [DataMember]
        public List<int> HistoryMarketingMapList { get; set; }

    }
}
