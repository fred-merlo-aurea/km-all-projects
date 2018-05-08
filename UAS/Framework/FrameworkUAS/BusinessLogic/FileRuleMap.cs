using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class FileRuleMap
    {
        public List<Entity.FileRuleMap> Select()
        {
            List<Entity.FileRuleMap> x = null;
            x = DataAccess.FileRuleMap.Select().ToList();

            return x;
        }
        public int Save(Entity.FileRuleMap x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.RulesAssignedID = DataAccess.FileRuleMap.Save(x);
                scope.Complete();
            }

            return x.RulesAssignedID;
        }
        public bool Delete(int SourceFileID)
        {
            bool complete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                DataAccess.FileRuleMap.Delete(SourceFileID);
                scope.Complete();
                complete = true;
            }

            return complete;
        }
    }
}
