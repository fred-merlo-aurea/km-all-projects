using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class SecurityGroupProductMap
    {
        public SecurityGroupProductMap() { }
        #region Properties
        [DataMember]
        public int SecurityGroupProductMapID { get; set; }
        [DataMember]
        public int SecurityGroupID { get; set; }
        [DataMember]
        public int ProductID { get; set; }
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
