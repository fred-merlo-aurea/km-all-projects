using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class RuleResult
    {
        public RuleResult() { }
        #region Properties
        [DataMember]
        public int RuleResultId { get; set; }
        [DataMember]
        public int RuleId { get; set; }
        [DataMember]
        public int RuleFieldId { get; set; }
        [DataMember]
        public string UpdateField { get; set; }
        [DataMember]
        public string UpdateFieldPrefix { get; set; }
        [DataMember]
        public string UpdateFieldValue { get; set; }
        [DataMember]
        public bool IsClientField { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public int CreatedByUserId { get; set; }
        [DataMember]
        public int UpdatedByUserId { get; set; }
        #endregion
    }
}
