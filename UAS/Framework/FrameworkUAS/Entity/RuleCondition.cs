using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class RuleCondition
    {
        public RuleCondition() { }
        #region Properties
        [DataMember]
        public int RuleId { get; set; }
        [DataMember]
        public int Line { get; set; }
        [DataMember]
        public bool IsGrouped { get; set; }
        [DataMember]
        public int GroupNumber { get; set; }
        [DataMember]
        public int ChainId { get; set; }
        [DataMember]
        public string CompareField { get; set; }
        [DataMember]
        public string CompareFieldPrefix { get; set; }
        [DataMember]
        public bool IsClientField { get; set; }
        [DataMember]
        public int OperatorId { get; set; }
        [DataMember]
        public string CompareValue { get; set; }
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
