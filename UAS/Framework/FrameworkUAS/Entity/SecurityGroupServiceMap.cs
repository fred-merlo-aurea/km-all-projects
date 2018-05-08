using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class SecurityGroupServiceMap
    {
        public SecurityGroupServiceMap()
        {
            SecurityGroupServiceMapID = 0;
            SecurityGroupID = 0;
            ServiceID = 0;
            IsEnabled = false;
            DateCreated = DateTime.Now;
            CreatedByUserID = 0;
        }
        [DataMember]
        public int SecurityGroupServiceMapID { get; set; }
        [DataMember]
        public int SecurityGroupID { get; set; }
        [DataMember]
        public int ServiceID { get; set; }
        [DataMember]
        public bool IsEnabled { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
    }
}
