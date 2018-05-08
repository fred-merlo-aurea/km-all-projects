using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace KMPlatform.BusinessLogic
{
    public class SubGenUserMap
    {
        public List<Entity.SubGenUserMap> Select(int userId)
        {
            List<Entity.SubGenUserMap> x = null;
            x = DataAccess.SubGenUserMap.Select(userId).ToList();
            return x;
        }
        public bool Save(Entity.SubGenUserMap x)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.SubGenUserMap.Save(x);
                scope.Complete();
            }

            return done;
        }
    }
}
