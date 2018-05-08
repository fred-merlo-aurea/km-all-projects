using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class SubscriptionSearchResult
    {
        public SubscriptionSearchResult() { }
        #region Properties
        [DataMember]
        public int SubscriptionID { get; set; }
        [DataMember]
        public int SequenceID { get; set; }
        [DataMember]
        public string FullAddress { get; set; }
        [DataMember]
        public string ProductCode { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public int ClientID { get; set; }
        [DataMember]
        public int ProductSubscriptionID { get; set; }
        [DataMember]
        public string ClientName { get; set; }
        [DataMember]
        public string Company { get; set; }
        [DataMember]
        public bool IsSubscribed { get; set; }
        [DataMember]
        public string AccountNumber { get; set; }
        [DataMember]
        public int PhoneCode { get; set; }
        [DataMember]
        public int SubscriptionStatusID { get; set; }
        #endregion
    }
}
