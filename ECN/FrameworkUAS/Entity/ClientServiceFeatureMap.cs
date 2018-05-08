using System;
using System.Linq;
using System.Runtime.Serialization;


namespace KMPlatform.Entity
{
    [Serializable]
    [DataContract]
    public class ClientServiceFeatureMap
    {
        [DataMember]
        public int ClientServiceFeatureMapID { get; set; }
        [DataMember]
        public int ClientID { get; set; }
        [DataMember]
        public int ServiceFeatureID { get; set; }
        [DataMember]
        public bool IsEnabled { get; set; }
        [DataMember]
        public decimal Rate { get; set; }
        [DataMember]
        public int RateDurationInMonths { get; set; }
        [DataMember]
        public DateTime? RateStartDate { get; set; }
        [DataMember]
        public DateTime? RateExpireDate { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
    }
}
