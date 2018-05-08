using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class SubscriberMarketingMap
    {
        public SubscriberMarketingMap() { }
        #region Properties
        [DataMember]
        public int MarketingID { get; set; }
        [DataMember]
        public string MarketingName { get; set; }
        [DataMember]
        public string MarketingCode { get; set; }
        [DataMember]
        public bool MarketingIsActive { get; set; }
        [DataMember]
        public int SubscriberID { get; set; }
        [DataMember]
        public int PublicationID { get; set; }
        [DataMember]
        public bool MarketingMapIsActive { get; set; }
        [DataMember]
        public DateTime MarketingMapDateCreated { get; set; }
        [DataMember]
        public DateTime? MarketingMapDateUpdated { get; set; }
        [DataMember]
        public int MarketingMapCreatedByUserID { get; set; }
        [DataMember]
        public int MarketingMapUpdatedByUserID { get; set; }
        #endregion
    }
}
