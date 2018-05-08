using System;
using System.Runtime.Serialization;
using System.Linq;

namespace KMPlatform.Entity
{

    [Serializable]
    [DataContract]
    public class SecurityGroupTemplatePermission
    {
        public SecurityGroupTemplatePermission() { }

        #region Properties
        [DataMember]
        public int SecurityGroupTemplatePermissionID { get; set; }
        [DataMember]
        public int SecurityGroupTemplateID { get; set; }
        [DataMember]
        public int ServiceFeatureAccessMapID { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        #endregion
    }
}
