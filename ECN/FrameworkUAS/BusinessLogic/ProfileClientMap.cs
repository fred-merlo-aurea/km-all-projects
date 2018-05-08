using System;
using System.Linq;
using System.Transactions;

namespace KMPlatform.BusinessLogic
{
    public class ProfileClientMap
    {
        public  bool Save(Entity.ProfileClientMap x)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                 DataAccess.ProfileClientMap.Save(x);
                scope.Complete();
                done = true;
            }

            return done;
        }
    }
}
