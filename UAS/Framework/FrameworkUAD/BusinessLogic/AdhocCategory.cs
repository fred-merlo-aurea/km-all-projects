using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class AdhocCategory
    {
        public List<Entity.AdhocCategory> SelectAll(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.AdhocCategory> retList = null;
            retList = DataAccess.AdhocCategory.SelectAll(client);
            return retList;
        }
        public int Save(Entity.AdhocCategory x, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.CategoryID = DataAccess.AdhocCategory.Save(x, client);
                scope.Complete();
            }

            return x.CategoryID;
        }
    }
}
