using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class FileRule
    {
        public List<Entity.FileRule> Select()
        {
            List<Entity.FileRule> x = null;
            x = DataAccess.FileRule.Select().ToList();
           
            return x;
        }
        public int Save(Entity.FileRule x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.FileRuleID = DataAccess.FileRule.Save(x);
                scope.Complete();
            }

            return x.FileRuleID;
        }
    }
}
