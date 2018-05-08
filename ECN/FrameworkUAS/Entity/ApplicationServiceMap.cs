using System;
using System.Runtime.Serialization;

namespace KMPlatform.Entity
{
    [Serializable]
    [DataContract]
    public class ApplicationServiceMap
    {
        public ApplicationServiceMap() { }
        #region Properties
        [DataMember]
        public int ApplicationServiceMapID { get; set; }
        [DataMember]
        public int ApplicationID { get; set; }
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
        #endregion
    }
}
