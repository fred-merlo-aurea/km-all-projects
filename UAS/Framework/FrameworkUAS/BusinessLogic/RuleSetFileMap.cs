using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class RuleSetFileMap
    {
        public bool Save(Entity.RuleSetFileMap x)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.RuleSetFileMap.Save(x);
                scope.Complete();
                done = true;
            }
            return done;
        }
        public bool Save(int ruleSetId, int sourceFileId, int userId)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.RuleSetFileMap.Save(ruleSetId, sourceFileId, userId);
                scope.Complete();
                done = true;
            }
            return done;
        }
    }
}
