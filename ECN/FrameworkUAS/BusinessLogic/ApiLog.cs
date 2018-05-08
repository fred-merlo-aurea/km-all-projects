using System;
using System.Linq;
using System.Transactions;

namespace KMPlatform.BusinessLogic
{
    public class ApiLog
    {
        public int Save(Entity.ApiLog x)
        {
            if (x.RequestStartDate == null)
                x.RequestStartDate = DateTime.Now;
            if (x.RequestStartTime == null)
                x.RequestStartTime = DateTime.Now.TimeOfDay;

            using (TransactionScope scope = new TransactionScope())
            {
                x.ApiLogId = DataAccess.ApiLog.Save(x);
                scope.Complete();
            }

            return x.ApiLogId;
        }
    }
}
