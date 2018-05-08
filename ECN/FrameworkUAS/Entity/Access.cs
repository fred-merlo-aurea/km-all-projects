using System;
using System.Runtime.Serialization;
using System.Linq;

namespace KMPlatform.Entity
{
    [Serializable]
    [DataContract]
    public class Access
    {
        public Access() { }

        #region Properties
        [DataMember]
        public int AccessId { get; set; }
        [DataMember]
        public string AccessName { get; set; }
        [DataMember]
        public string AccessCode { get; set; }
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
