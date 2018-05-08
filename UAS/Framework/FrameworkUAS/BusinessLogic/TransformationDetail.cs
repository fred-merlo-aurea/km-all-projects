using System;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class TransformationDetail
    {
        public  int Save(KMPlatform.Entity.UserLog x)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            using (TransactionScope scope = new TransactionScope())
            {
                x.UserLogID = KMPlatform.DataAccess.UserLog.Save(x);
                scope.Complete();
            }

            return x.UserLogID;
        }
    }
}
