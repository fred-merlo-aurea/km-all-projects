using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class Rule
    {
        public Rule()
        {
            RuleId = 0;
            RuleName = string.Empty;
            DisplayName = string.Empty;
            RuleDescription = string.Empty;
            IsDeleteRule = false;
            IsSystem = false;
            IsGlobal = false;
            ClientId = 0;
            IsActive = true;
            DateCreated = DateTime.Now;
            CreatedByUserId = 1;
            DateUpdated = null;
            UpdatedByUserId = null;
            CustomImportRuleId = 0;
            RuleActionId = 0;
        }
        #region Properties
        [DataMember]
        public int RuleId { get; set; }
        [DataMember]
        public string RuleName { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public string RuleDescription { get; set; }
        [DataMember]
        public bool IsDeleteRule { get; set; }
        [DataMember]
        public bool IsSystem { get; set; }
        [DataMember]
        public bool IsGlobal { get; set; }
        [DataMember]
        public int ClientId { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserId { get; set; }
        [DataMember]
        public int? UpdatedByUserId { get; set; }
        [DataMember]
        public int CustomImportRuleId { get; set; }
        [DataMember]
        public int RuleActionId { get; set; }
        #endregion
    }
}
