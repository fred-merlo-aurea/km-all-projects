using System;
using System.Linq;
using System.Runtime.Serialization;

namespace KMPlatform.Entity
{
    [Serializable]
    [DataContract]
    public class ApplicationSecurityGroupMap
    {
        public ApplicationSecurityGroupMap() { }
        #region Properties
        [DataMember]
        public int ApplicationSecurityGroupMapID { get; set; }
        [DataMember]
        public int SecurityGroupID { get; set; }
        [DataMember]
        public int ApplicationID { get; set; }
        [DataMember]
        public bool HasAccess { get; set; }
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
