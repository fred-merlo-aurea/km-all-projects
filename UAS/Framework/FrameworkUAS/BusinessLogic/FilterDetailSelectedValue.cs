using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class FilterDetailSelectedValue
    {
        public List<Entity.FilterDetailSelectedValue> Select(int filterDetailID)
        {
            List<Entity.FilterDetailSelectedValue> retItem = null;
            retItem = DataAccess.FilterDetailSelectedValue.Select(filterDetailID);

            return retItem;
        }

        public int Save(Entity.FilterDetailSelectedValue x)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            using (TransactionScope scope = new TransactionScope())
            {
                x.FilterDetailSelectedValueId = DataAccess.FilterDetailSelectedValue.Save(x);
                scope.Complete();
            }

            return x.FilterDetailSelectedValueId;
        }
    }
}
