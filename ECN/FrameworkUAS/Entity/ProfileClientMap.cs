﻿using System;
using System.Runtime.Serialization;

namespace KMPlatform.Entity
{
    [Serializable]
    [DataContract]
    public class ProfileClientMap
    {
        public ProfileClientMap() { }
        #region Properties
        [DataMember]
        public int ProfileID { get; set; }
        [DataMember]
        public int ClientID { get; set; }
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
