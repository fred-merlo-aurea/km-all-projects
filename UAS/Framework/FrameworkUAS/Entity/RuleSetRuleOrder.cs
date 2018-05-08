using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class RuleSetRuleOrder
    {
        public RuleSetRuleOrder() { }
        #region Properties
        [DataMember]
        public int RuleSetId { get; set; }
        [DataMember]
        public int RuleId { get; set; }
        [DataMember]
        public int ExecutionOrder { get; set; }
        [DataMember]
        public string RuleScript { get; set; }
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
