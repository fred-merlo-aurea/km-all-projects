using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FrameworkUAS.Object
{
    [Serializable]
    [DataContract]
    public class CustomRuleGrid
    {
        public CustomRuleGrid() { }
        public CustomRuleGrid(int ruleSetId, int ruleId, string ruleTypeAction, string ruleName, int executionOrder, string ruleScript)
        {
            RuleSetId = ruleSetId;
            RuleId = ruleId;
            RuleTypeAction = ruleTypeAction;
            RuleName = ruleName;
            ExecutionOrder = executionOrder;
            RuleScript = ruleScript;
        }
        #region Properties
        [DataMember]
        public int RuleSetId { get; set; }
        [DataMember]
        public int RuleId { get; set; }
        [DataMember]
        public string RuleTypeAction { get; set; }
        [DataMember]
        public string RuleName { get; set; }
        [DataMember]
        public int ExecutionOrder { get; set; }
        [DataMember]
        public string RuleScript { get; set; }
        #endregion
    }
}
