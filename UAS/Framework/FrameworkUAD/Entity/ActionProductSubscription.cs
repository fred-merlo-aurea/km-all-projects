using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class ActionProductSubscription
    {
        public ActionProductSubscription()
        {
            PubSubscriptionID = 0;
            SubscriptionID = 0;
            PubCategoryID = 0;
            PubTransactionID = 0;
            CategoryCodeValue = 0;
            CategoryType = "";
            TransactionType = "";
        }

        #region Properties

        [DataMember]
        public int PubSubscriptionID { get; set; }

        [DataMember]
        public int SubscriptionID { get; set; }

        [DataMember(Name = "CategoryID")]
        public int PubCategoryID { get; set; }

        [DataMember(Name = "TransactionID")]
        public int PubTransactionID { get; set; }

        [DataMember]
        public int Copies { get; set; }
        [DataMember]
        public string CategoryType { get; set; }
        [DataMember]
        public string TransactionType { get; set; }
        [DataMember]
        public int CategoryCodeValue { get; set; }

        #endregion
    }
}
