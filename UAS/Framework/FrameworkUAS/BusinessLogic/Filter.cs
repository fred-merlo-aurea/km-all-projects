using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class Filter
    {
        public List<Entity.Filter> Select(int publicationID)
        {
            List<Entity.Filter> retItem = null;
            retItem = DataAccess.Filter.Select(publicationID);

            return retItem;
        }

        public List<Entity.Filter> SelectClient(int clientID)
        {
            List<Entity.Filter> retItem = null;
            retItem = DataAccess.Filter.SelectClient(clientID);

            return retItem;
        }

        public int Save(Entity.Filter x)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            using (TransactionScope scope = new TransactionScope())
            {
                x.FilterId = DataAccess.Filter.Save(x);
                scope.Complete();
            }

            return x.FilterId;
        }
    }
}
