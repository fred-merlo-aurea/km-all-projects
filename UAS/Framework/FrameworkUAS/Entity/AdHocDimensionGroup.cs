using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class AdHocDimensionGroup
    {
        public AdHocDimensionGroup() 
        {
            AdHocDimensionGroupId = 0;
            AdHocDimensionGroupName = string.Empty;
            ClientID = 0;
            SourceFileID = 0;
            IsActive = true;
            OrderOfOperation = 1;
            StandardField = string.Empty;
            CreatedDimension = string.Empty;
            DefaultValue = string.Empty;
            IsPubcodeSpecific = false;
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
        }

        #region Properties
        [DataMember]
        public int AdHocDimensionGroupId { get; set; }
        [DataMember]
        public string AdHocDimensionGroupName { get; set; }
        [DataMember]
        public int ClientID { get; set; }
        [DataMember]
        public int SourceFileID { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public int OrderOfOperation { get; set; }
        [DataMember]
        public string StandardField { get; set; }
        [DataMember]
        public string CreatedDimension { get; set; }
        [DataMember]
        public string DefaultValue { get; set; }
        [DataMember]
        public bool IsPubcodeSpecific { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        #endregion
        [DataMember]
        public List<AdHocDimension> AdHocDimensions { get; set; }
        [DataMember]
        public List<AdHocDimensionGroupPubcodeMap> DimensionGroupPubcodeMappings { get; set; }
    }
}
