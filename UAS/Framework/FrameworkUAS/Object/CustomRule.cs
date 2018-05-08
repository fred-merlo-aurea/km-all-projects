using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FrameworkUAS.Object
{
    [Serializable]
    [DataContract]
    public class CustomRule
    {
        public CustomRule() {
            RuleSetId = 0;
            RuleId = 0;
            //RuleTypeAction = string.Empty;//this is CustomImportRule_Code_DisplayName - RuleAction_Code_DisplayName
            RuleName = string.Empty;
            CustomImportRuleDisplayName = string.Empty;
            RuleActionDisplayName = string.Empty;

            Connector = string.Empty;
            DatabaseField = string.Empty;
            Operator = string.Empty;
            Values = string.Empty;
            ExecutionOrder = 0;

            Updates = new List<CustomRuleInsertUpdateNew>();
        }
        public CustomRule(int _ruleSetId, int _ruleId, string _customImportRuleDisplayName, string _ruleActionDisplayName, string _ruleName, string _connector, string _databaseField, string _operator, string _values, int _executionOrder)
        {
            RuleSetId = _ruleSetId;
            RuleId = _ruleId;
            //RuleTypeAction = _ruleTypeAction;
            RuleName = _ruleName;
            CustomImportRuleDisplayName = _customImportRuleDisplayName;
            RuleActionDisplayName = _ruleActionDisplayName;

            Connector = _connector;
            DatabaseField = _databaseField;
            Operator = _operator;
            Values = _values;
            ExecutionOrder = _executionOrder;

            Updates = new List<CustomRuleInsertUpdateNew>();
        }

        #region Properties
        [DataMember]
        public int RuleSetId { get; set; }
        [DataMember]
        public int RuleId { get; set; }
        [DataMember]
        public string RuleName { get; set; }
        [DataMember]
        public string CustomImportRuleDisplayName { get; set; }
        [DataMember]
        public string RuleActionDisplayName { get; set; }
        [DataMember]
        public string Connector { get; set; }
        [DataMember]
        public string DatabaseField { get; set; }
        [DataMember]
        public string Operator { get; set; }
        [DataMember]
        public string Values { get; set; }
        [DataMember]
        public int ExecutionOrder { get; set; }

        [DataMember]
        public List<CustomRuleInsertUpdateNew> Updates { get; set; }
        #endregion
    }
}
