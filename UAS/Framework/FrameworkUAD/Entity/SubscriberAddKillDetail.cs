using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class SubscriberAddKillDetail
    {
        public SubscriberAddKillDetail()
        {
            AddKillID = 0;
            AddKillDetailID = 0;
            PubCategoryID = 0;
            PubTransactionID = 0;
            PubSubscriptionID = 0;
        }
        public SubscriberAddKillDetail(int addKillID, int cat, int trans, int subID)
        {
            AddKillID = addKillID;
            AddKillDetailID = 0;
            PubCategoryID = cat;
            PubTransactionID = trans;
            PubSubscriptionID = subID;
        }

        #region Properties
        [DataMember]
        public int AddKillDetailID { get; set; }
        [DataMember]
        public int PubSubscriptionID { get; set; }
        [DataMember]
        public int AddKillID { get; set; }
        [DataMember]
        public int PubCategoryID { get; set; }
        [DataMember]
        public int PubTransactionID { get; set; }
        #endregion
    }
}
