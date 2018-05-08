using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    public class ProductAudit
    {
        public ProductAudit() { }
        #region Properties
        [DataMember]
        public int ProductAuditId { get; set; }
        [DataMember]
        public int ProductId { get; set; }
        [DataMember]
        public string AuditField { get; set; }
        [DataMember]
        public int FieldMappingTypeId { get; set; }
        [DataMember]
        public int ResponseGroupID { get; set; }
        [DataMember]
        public int SubscriptionsExtensionMapperID { get; set; }
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
