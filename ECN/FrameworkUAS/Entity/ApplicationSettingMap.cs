using System;
using System.Runtime.Serialization;

namespace KMPlatform.Entity
{
    [Serializable]
    [DataContract]
    public class ApplicationSettingMap
    {
        public ApplicationSettingMap() { }
        #region Properties
        [DataMember]
        public int ApplicationSettingMapID { get; set; }
        [DataMember]
        public int ApplicationID { get; set; }
        [DataMember]
        public int ApplicationSettingID { get; set; }
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
