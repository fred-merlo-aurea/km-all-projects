using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace KMPlatform.Entity
{
    [Serializable]
    [DataContract]
    public class ClientGroupServiceMap
    {
        public ClientGroupServiceMap() { }

        public ClientGroupServiceMap(int clientGroupID, int mapID, int serviceID, bool enabled, int createdByUserID)
        {
            ClientGroupServiceMapID = mapID;
            ClientGroupID = clientGroupID;
            ServiceID = serviceID;
            IsEnabled = enabled;
            DateCreated = DateTime.Now;
            CreatedByUserID = createdByUserID;
        }
        #region Properties
        [DataMember]
        public int ClientGroupServiceMapID { get; set; }
        [DataMember]
        public int ClientGroupID { get; set; }
         [DataMember]
        public int ServiceID { get; set; }
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

        [DataMember]
        public List<ClientGroupServiceFeatureMap> Features { get; set; }
    }
}
