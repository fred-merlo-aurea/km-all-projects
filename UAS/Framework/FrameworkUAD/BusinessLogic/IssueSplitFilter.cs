using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class IssueSplitFilter
    {
        public Entity.IssueSplitFilter SelectFilterID(int FilterID, KMPlatform.Object.ClientConnections client)
        {
            Entity.IssueSplitFilter x = DataAccess.IssueSplitFilter.SelectFilterID(FilterID, client);
            return x;
        }
        public int Save(Entity.IssueSplitFilter filter, DataTable filterdetails, KMPlatform.Object.ClientConnections client)
        {
            int IssueSplitFilterId = 0;

            using (TransactionScope scope = new TransactionScope())
            {
                filter.FilterID = DataAccess.IssueSplitFilter.Save(filter, filterdetails, client);
                scope.Complete();
                IssueSplitFilterId = filter.FilterID;
            }
            return IssueSplitFilterId;
        }
    }
}
