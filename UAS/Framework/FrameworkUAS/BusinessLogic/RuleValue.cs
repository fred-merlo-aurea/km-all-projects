using System;
using System.Linq;
using System.Collections.Generic;

namespace FrameworkUAS.BusinessLogic
{
    public class RuleValue
    {
        public HashSet<Entity.RuleValue> GetRuleValuesForRule(int ruleId, bool isActive = true)
        {
            HashSet<Entity.RuleValue> retList = new HashSet<Entity.RuleValue>(DataAccess.RuleValue.GetRuleValuesForRule(ruleId, isActive).ToList());
            return retList;
        }
        public HashSet<Entity.RuleValue> GetRuleValuesForSourceFile(int sourceFileId, bool isActive = true)
        {
            HashSet<Entity.RuleValue> retList = new HashSet<Entity.RuleValue>(DataAccess.RuleValue.GetRuleValuesForSourceFile(sourceFileId, isActive).ToList());
            return retList;
        }
        public HashSet<Entity.RuleValue> GetRuleValuesForRuleValue(int ruleValueId, bool isActive = true)
        {
            HashSet<Entity.RuleValue> retList = new HashSet<Entity.RuleValue>(DataAccess.RuleValue.GetRuleValuesForRuleValue(ruleValueId, isActive).ToList());
            return retList;
        }
        public HashSet<Entity.RuleValue> SelectIsActive(bool isActive = true)
        {
            HashSet<Entity.RuleValue> retList = new HashSet<Entity.RuleValue>(DataAccess.RuleValue.SelectIsActive(isActive).ToList());
            return retList;
        }
    }
}
