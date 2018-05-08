using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class RuleCondition
    {
        public List<Entity.RuleCondition> Select(int ruleId)
        {
            List<Entity.RuleCondition> retList = new List<Entity.RuleCondition>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = DataAccess.RuleCondition.Select(ruleId).ToList();
                scope.Complete();
            }
            return retList;
        }
        public bool Save(Entity.RuleCondition rc)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.RuleCondition.Save(rc);
                scope.Complete();
                done = true;
            }
            return done;
        }
        public bool Delete(int ruleId, int lineNumber)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.RuleCondition.Delete(ruleId, lineNumber);
                scope.Complete();
                done = true;
            }
            return done;
        }
    }
}
