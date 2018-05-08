using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace FrameworkUAS.Entity
{

    [Serializable]
    [DataContract]
    public class RuleSetRuleMap
    {
        public RuleSetRuleMap()
        {
            RuleSetId = 0;
            RuleId = 0;
            RecordTypeId = 0;
            ExecutionOrder = 0;
            RuleValueId = null;
            FreeTextValue = string.Empty;
            IsActive = true;
            RuleChainCodeId = null;
            DateCreated = DateTime.Now;
            CreatedByUserId = 1;
            DateUpdated = null;
            UpdatedByUserId = null;
        }
        #region Properties
        [DataMember]
        public int RuleSetId { get; set; }
        [DataMember]
        public int RuleId { get; set; }
        [DataMember]
        public int RecordTypeId { get; set; }
        [DataMember]
        public int ExecutionOrder { get; set; }
        [DataMember]
        public int? RuleValueId { get; set; }
        [DataMember]
        public string FreeTextValue { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public int? RuleChainCodeId { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserId { get; set; }
        [DataMember]
        public int? UpdatedByUserId { get; set; }
        #endregion
    }
}
