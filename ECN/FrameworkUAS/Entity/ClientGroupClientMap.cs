using System;
using System.Runtime.Serialization;

namespace KMPlatform.Entity
{
    [Serializable]
    [DataContract]
    public class ClientGroupClientMap
    {
        public ClientGroupClientMap() { }
        #region Properties
        [DataMember]
        public int ClientGroupClientMapID { get; set; }
        [DataMember]
        public int ClientGroupID { get; set; }
        [DataMember]
        public int ClientID { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
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
