using System;
using System.Runtime.Serialization;

namespace KMPlatform.Entity
{
    [Serializable]
    [DataContract]
    public class ClientConfigurationMap
    {
        public ClientConfigurationMap()
        {
            ClientConfigurationMapId = 0;
            ClientID = 0;
            CodeTypeId = 0;
            CodeId = 0;
            ClientValue = string.Empty;
            IsActive = false;
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
            DateUpdated = null;
            UpdatedByUserID = 0;
        }
        #region Properties
        [DataMember]
        public int ClientConfigurationMapId { get; set; }
        [DataMember]
        public int ClientID { get; set; }
        [DataMember]
        public int CodeTypeId { get; set; }
        [DataMember]
        public int CodeId { get; set; }
        [DataMember]
        public string ClientValue { get; set; }
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
