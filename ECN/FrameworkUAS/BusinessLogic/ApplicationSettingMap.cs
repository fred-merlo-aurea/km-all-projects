using System;
using System.Linq;
using System.Transactions;

namespace KMPlatform.BusinessLogic
{
    public class ApplicationSettingMap
    {
       
        public bool Save(Entity.ApplicationSettingMap x)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.ApplicationSettingMap.Save(x);
                scope.Complete();
            }

            return done;
        }
    }
}
