using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class HistoryResponseMap
    {
        public HistoryResponseMap() { }
        #region Properties
        [DataMember]
        public int HistoryResponseMapID { get; set; }
        [DataMember]
        public int PubSubscriptionDetailID { get; set; }
        [DataMember]
        public int PubSubscriptionID { get; set; }
        [DataMember]
        public int SubscriptionID { get; set; }
        [DataMember]
        public int CodeSheetID { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        [DataMember]
        public string ResponseOther { get; set; }
        [DataMember]
        public int HistorySubscriptionID { get; set; }
        #endregion
    }
}
