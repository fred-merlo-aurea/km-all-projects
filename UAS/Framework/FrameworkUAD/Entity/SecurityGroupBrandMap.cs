using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class SecurityGroupBrandMap
    {
        public SecurityGroupBrandMap() { }
        #region Properties
        [DataMember]
        public int SecurityGroupBrandMapID { get; set; }
        [DataMember]
        public int SecurityGroupID { get; set; }
        [DataMember]
        public int BrandID { get; set; }
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
