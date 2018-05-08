using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    /// <summary>
    /// Model.Field and Model.Field value methods are in FrameworkUAD.BuisnessLogic.Objects
    /// </summary>
    public class Model
    {
        public List<FrameworkUAS.Model.Rule> RulesGetRuleSet(int ruleSetId, int sourceFileId)
        {
            List<FrameworkUAS.Model.Rule> retList = new List<FrameworkUAS.Model.Rule>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = DataAccess.Model.RulesGetRuleSet(ruleSetId, sourceFileId).ToList();
                scope.Complete();
            }
            retList.ForEach(x =>
            {
                x.conditions = ConditionsGetRuleId(x.ruleId);
                x.updates = UpdatesGetRuleId(x.ruleId);
            });
            return retList;
        }
        public List<FrameworkUAS.Model.Rule> RulesGetClientTemplates(int clientId, int sourceFileId)
        {
            List<FrameworkUAS.Model.Rule> retList = new List<FrameworkUAS.Model.Rule>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = DataAccess.Model.RulesGetClientTemplates(clientId, sourceFileId).ToList();
                scope.Complete();
            }
            retList.ForEach(x =>
            {
                x.conditions = ConditionsGetRuleId(x.ruleId);
                x.updates = UpdatesGetRuleId(x.ruleId);
            });
            return retList;
        }
        public List<FrameworkUAS.Model.Condition> ConditionsGetRuleId(int ruleId)
        {
            List<FrameworkUAS.Model.Condition> retList = new List<FrameworkUAS.Model.Condition>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = DataAccess.Model.ConditionsGetRuleId(ruleId).ToList();
                scope.Complete();
            }
            return retList;
        }
        public List<FrameworkUAS.Model.Update> UpdatesGetRuleId(int ruleId)//RuleResult
        {
            List<FrameworkUAS.Model.Update> retList = new List<FrameworkUAS.Model.Update>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = DataAccess.Model.UpdatesGetRuleId(ruleId).ToList();
                scope.Complete();
            }
            return retList;
        }

        
    }
}
