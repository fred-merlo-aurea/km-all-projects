using System;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class FilterSchedule
    {
        public int Save(Entity.FilterSchedule x)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            using (TransactionScope scope = new TransactionScope())
            {
                x.FilterScheduleId = DataAccess.FilterSchedule.Save(x);
                scope.Complete();
            }

            return x.FilterScheduleId;
        }
    }
}
