using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace KMPlatform.BusinessLogic
{
    public class ClientUADUsersMap
    {
        public List<Entity.ClientUADUsersMap> Select()
        {
            List<Entity.ClientUADUsersMap> x = null;
            x = DataAccess.ClientUADUsersMap.Select().ToList();

            return x;
        }
        public List<Entity.ClientUADUsersMap> SelectClient(int clientID)
        {
            List<Entity.ClientUADUsersMap> x = null;
            x = DataAccess.ClientUADUsersMap.SelectClient(clientID);

            return x;
        }
        public List<Entity.ClientUADUsersMap> SelectUser(int userID)
        {
            List<Entity.ClientUADUsersMap> x = null;
            x = DataAccess.ClientUADUsersMap.SelectUser(userID);

            return x;
        }
        public  Entity.ClientUADUsersMap Select(int clientID, int userID)
        {
            Entity.ClientUADUsersMap x = null;
            x = DataAccess.ClientUADUsersMap.Select(clientID,userID);

            return x;
        }
       
        public  bool Save(Entity.ClientUADUsersMap x)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                DataAccess.ClientUADUsersMap.Save(x);
                scope.Complete();
                done = true;
            }

            return done;
        }
        public  bool Save(int clientID,int userID)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                DataAccess.ClientUADUsersMap.Save(clientID,userID);
                scope.Complete();
                done = true;
            }

            return done;
        }
    }
}
