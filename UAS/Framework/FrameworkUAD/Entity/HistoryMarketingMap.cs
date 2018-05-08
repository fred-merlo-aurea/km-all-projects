using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class HistoryMarketingMap
    {
        public HistoryMarketingMap() { }
        #region Properties
        [DataMember]
        public int HistoryMarketingMapID { get; set; }
        [DataMember]
        public int MarketingID { get; set; }
        [DataMember]
        public int PubSubscriptionID { get; set; }
        [DataMember]
        public int PublicationID { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        #endregion
    }
}
