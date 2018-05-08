using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class ClientGroupUserMap
    {
        public List<Entity.ClientGroupUserMap> Select()
        {
            List<Entity.ClientGroupUserMap> x = null;
            x = DataAccess.ClientGroupUserMap.Select();
            return x;
        }
        public List<Entity.ClientGroupUserMap> SelectForClientGroup(int clientGroupID)
        {
            List<Entity.ClientGroupUserMap> x = null;
            x = DataAccess.ClientGroupUserMap.SelectForClientGroup(clientGroupID);
            return x;
        }
        public List<Entity.ClientGroupUserMap> SelectForUser(int userID)
        {
            List<Entity.ClientGroupUserMap> x = null;
            x = DataAccess.ClientGroupUserMap.SelectForUser(userID);
            return x;
        }
        public int Save(Entity.ClientGroupUserMap x)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            using (TransactionScope scope = new TransactionScope())
            {
                x.ClientGroupUserMapID = DataAccess.ClientGroupUserMap.Save(x);
                scope.Complete();
            }

            return x.ClientGroupUserMapID;
        }
    }
}
