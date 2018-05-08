using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class ClientGroupSecurityGroupMap
    {
        public ClientGroupSecurityGroupMap() { }
        #region Properties
        [DataMember]
        public int ClientGroupSecurityGroupMapID { get; set; }
        [DataMember]
        public int ClientGroupID { get; set; }
        [DataMember]
        public int SecurityGroupID { get; set; }
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
