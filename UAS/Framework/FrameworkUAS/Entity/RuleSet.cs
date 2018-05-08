using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace FrameworkUAS.Entity
{
    /// <summary>
    /// RuleSet is a system ruleSet if IsSystem=1 and ClientId=0 (this should always be our case)
    /// </summary>
    [Serializable]
    [DataContract]
    public class RuleSet
    {
        public RuleSet() {
            RuleSetId = 0;
            RuleSetName = string.Empty;
            DisplayName = string.Empty;
            RuleSetDescription = string.Empty;
            IsActive = false;
            IsSystem = false;
            IsGlobal = false;
            ClientId = 0;
            IsDateSpecific = true;
            StartMonth = null;
            EndMonth = null;
            StartDay = null;
            EndDay = null;
            StartYear = null;
            EndYear = null;
            DateCreated = DateTime.Now;
            CreatedByUserId = 1;
            DateUpdated = null;
            UpdatedByUserId = null;
            CustomImportRuleId = 0;
        }
        #region Properties
        [DataMember]
        public int RuleSetId { get; set; }
        [DataMember]
        public string RuleSetName { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public string RuleSetDescription { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public bool IsSystem { get; set; }
        [DataMember]
        public bool IsGlobal { get; set; }
        [DataMember]
        public int ClientId { get; set; }
        [DataMember]
        public bool IsDateSpecific { get; set; }
        [DataMember]
        public int? StartMonth { get; set; }
        [DataMember]
        public int? EndMonth { get; set; }
        [DataMember]
        public int? StartDay { get; set; }
        [DataMember]
        public int? EndDay { get; set; }
        [DataMember]
        public int? StartYear { get; set; }
        [DataMember]
        public int? EndYear { get; set; }
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
        #endregion
    }
}
