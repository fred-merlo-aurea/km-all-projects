using System;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class FilterGroup
    {
        public int Save(Entity.FilterGroup x)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            using (TransactionScope scope = new TransactionScope())
            {
                x.FilterGroupId = DataAccess.FilterGroup.Save(x);
                scope.Complete();
            }

            return x.FilterGroupId;
        }
    }
}
