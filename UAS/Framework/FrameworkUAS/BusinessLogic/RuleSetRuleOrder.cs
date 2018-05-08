using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class RuleSetRuleOrder
    {
        public bool Save(Entity.RuleSetRuleOrder rsRo)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                done = DataAccess.RuleSetRuleOrder.Save(rsRo);
                scope.Complete();
            }
            return done;
        }
        public int GetRuleCount(int ruleSetId)
        {
            int c = DataAccess.RuleSetRuleOrder.GetRuleCount(ruleSetId);
            return c;
        }
        public bool UpdateExecutionOrder(int ruleSetId, int ruleId, int executionOrder)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                done = DataAccess.RuleSetRuleOrder.UpdateExecutionOrder(ruleSetId, ruleId, executionOrder);
                scope.Complete();
            }
            return done;
        }
        public bool DeleteRule(int ruleSetId, int ruleId, int userId)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                done = DataAccess.RuleSetRuleOrder.DeleteRule(ruleSetId, ruleId, userId);
                scope.Complete();
            }
            return done;
        }
        public int GetExecutionOrder(int ruleSetId)
        {
            int eo = 1;
            eo = DataAccess.RuleSetRuleOrder.GetExecutionOrder(ruleSetId);
            if (eo == 0) eo = 1;
            return eo;
        }
    }
}
