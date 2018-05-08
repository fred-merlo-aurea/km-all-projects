using System;
using System.Runtime.Serialization;
namespace KMPlatform.Entity
{
    [Serializable]
    [DataContract]
    public class ClientGroupServiceFeatureMap
    {
        public ClientGroupServiceFeatureMap() { }

        public ClientGroupServiceFeatureMap(int clientGroupID, int serviceID, int featureID, bool enabled, int createdByUserID)
            : this(clientGroupID, default(int), serviceID, featureID, enabled, createdByUserID)
        { }

        public ClientGroupServiceFeatureMap(int clientGroupID, int mapID, int serviceID, int featureID, bool enabled, int createdByUserID)
        {
            ClientGroupServiceFeatureMapID = mapID;
            ClientGroupID = clientGroupID;
            ServiceID = serviceID;
            ServiceFeatureID = featureID;
            IsEnabled = enabled;
            DateCreated = DateTime.Now;
            CreatedByUserID = createdByUserID;
        }
        
        #region Properties
        [DataMember]
        public int ClientGroupServiceFeatureMapID { get; set; }
        [DataMember]
        public int ClientGroupID { get; set; }
        [DataMember]
        public int ServiceID { get; set; }
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
        #endregion
    }
}
