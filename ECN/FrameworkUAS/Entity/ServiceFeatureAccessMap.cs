using System;
using System.Linq;
using System.Runtime.Serialization;

namespace KMPlatform.Entity
{
    [DataContract]
    [Serializable]
    public class ServiceFeatureAccessMap
    {
        public ServiceFeatureAccessMap() { }
        #region Properties
        [DataMember]
        public int ServiceFeatureAccessMapID { set; get; }
        [DataMember]
        public string ServiceFeatureID { get; set; }
        [DataMember]
        public int AccessID { get; set; }
        [DataMember]
        public bool IsEnabled { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        #endregion
    }
}
