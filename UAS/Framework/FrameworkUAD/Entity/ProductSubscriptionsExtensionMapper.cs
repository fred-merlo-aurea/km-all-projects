using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class ProductSubscriptionsExtensionMapper
    {
        public ProductSubscriptionsExtensionMapper() 
        {
            PubSubscriptionsExtensionMapperID = 0;
            PubID = 0;
            StandardField = string.Empty;
            CustomField = string.Empty;
            CustomFieldDataType = string.Empty;
            Active = false;
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
        }
        #region Properties
        [DataMember(Name = "ProductSubscriptionsExtensionMapperID")]
        public int PubSubscriptionsExtensionMapperID { get; set; }
        [DataMember(Name = "ProductID")]
        public int PubID { get; set; }
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
