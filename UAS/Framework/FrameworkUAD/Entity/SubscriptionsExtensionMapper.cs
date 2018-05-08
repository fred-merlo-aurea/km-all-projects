using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class SubscriptionsExtensionMapper
    {
        public SubscriptionsExtensionMapper()
        {
            SubscriptionsExtensionMapperID = 0;
            StandardField = string.Empty;
            CustomField = string.Empty;
            CustomFieldDataType = string.Empty;
            Active = false;
            DateCreated = DateTime.Now;
            DateUpdated = null;
            CreatedByUserID = 1;
            UpdatedByUserID = null;
        }
        #region Properties
        [DataMember]
        public int SubscriptionsExtensionMapperID { get; set; }
        [DataMember]
        public string StandardField { get; set; }
        [DataMember]
        public string CustomField { get; set; }
        [DataMember]
        public string CustomFieldDataType { get; set; }
        [DataMember]
        public bool Active { get; set; }
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
