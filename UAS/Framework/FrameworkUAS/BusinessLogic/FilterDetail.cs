using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class FilterDetail
    {
        public List<Entity.FilterDetail> Select(int filterID)
        {
            List<Entity.FilterDetail> retItem = null;
            retItem = DataAccess.FilterDetail.Select(filterID);

            return retItem;
        }

        public int Save(Entity.FilterDetail x)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            using (TransactionScope scope = new TransactionScope())
            {
                x.FilterDetailId = DataAccess.FilterDetail.Save(x);
                scope.Complete();
            }

            return x.FilterDetailId;
        }
    }
}
