using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace KMPlatform.BusinessLogic
{
    public class ClientGroupClientMap
    {
        public List<Entity.ClientGroupClientMap> Select()
        {
            List<Entity.ClientGroupClientMap> retItem = null;
            retItem = DataAccess.ClientGroupClientMap.Select().ToList();
            return retItem;
        }
        public List<Entity.ClientGroupClientMap> SelectForClientGroup(int clientGroupID)
        {
            List<Entity.ClientGroupClientMap> retItem = null;
            retItem = DataAccess.ClientGroupClientMap.SelectForClientGroup(clientGroupID).ToList();
            return retItem;
        }
        public List<Entity.ClientGroupClientMap> SelectForClientID(int clientID)
        {
            List<Entity.ClientGroupClientMap> retItem = null;
            retItem = DataAccess.ClientGroupClientMap.SelectForClientID(clientID).ToList();
            return retItem;
        }
        public int Save(Entity.ClientGroupClientMap x)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;
            using (TransactionScope scope = new TransactionScope())
            {
                x.ClientGroupClientMapID = DataAccess.ClientGroupClientMap.Save(x);
                scope.Complete();
            }

            return x.ClientGroupClientMapID;
        }
    }
}
