using System;
using System.Linq;
using System.Runtime.Serialization;

namespace KMPlatform.Entity
{
    public class SecurityGroupPermission
    {
        [Serializable]
        [DataContract]
        public class Permission : SecurityGroupPermission
        {
            [DataMember]
            public int ServiceID { get; set; }
            [DataMember]
            public string ServiceCode { get; set; }
            [DataMember]
            public int ServiceFeatureID { get; set; }
            [DataMember]
            public string SFCode { get; set; }
            [DataMember]
            public int AccessID { get; set; }
            [DataMember]
            public string AccessCode { get; set; }
        }

        [DataMember]
        public int SecurityGroupPermissionID { get; set; }
        [DataMember]
        public int SecurityGroupID { get; set; }
        [DataMember]
        public int ServiceFeatureAccessMapID { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
    }
}
