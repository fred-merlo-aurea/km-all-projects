using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class ClientGroupUserMap
    {
        public ClientGroupUserMap() { }
        #region Properties
        [DataMember]
        public int ClientGroupUserMapID { get; set; }
        [DataMember]
        public int ClientGroupID { get; set; }
        [DataMember]
        public int UserID { get; set; }
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
