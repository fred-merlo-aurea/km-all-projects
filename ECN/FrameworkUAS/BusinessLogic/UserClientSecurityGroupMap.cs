using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace KMPlatform.BusinessLogic
{
    public class UserClientSecurityGroupMap
    {
        public List<Entity.UserClientSecurityGroupMap> Select()
        {
            List<Entity.UserClientSecurityGroupMap> x = null;
            x = DataAccess.UserClientSecurityGroupMap.Select().ToList();

            return x;
        }
        public List<Entity.UserClientSecurityGroupMap> SelectForUser(int userID)
        {
            List<Entity.UserClientSecurityGroupMap> x = null;
            x = DataAccess.UserClientSecurityGroupMap.SelectForUser(userID).ToList();

            return x;
        }
        public List<Entity.UserClientSecurityGroupMap> SelectForUserAuthorization(int userID)
        {
            List<Entity.UserClientSecurityGroupMap> x = null;
            x = DataAccess.UserClientSecurityGroupMap.SelectForUserAuthorization(userID).ToList();

            return x;
        }
        public List<Entity.UserClientSecurityGroupMap> SelectForClient(int clientID)
        {
            List<Entity.UserClientSecurityGroupMap> x = null;
            x = DataAccess.UserClientSecurityGroupMap.SelectForClient(clientID).ToList();

            return x;
        }
        public List<Entity.UserClientSecurityGroupMap> SelectForSecurityGroup(int securityGroupID)
        {
            List<Entity.UserClientSecurityGroupMap> x = null;
            x = DataAccess.UserClientSecurityGroupMap.SelectForSecurityGroup(securityGroupID).ToList();

            return x;
        }

        public int Save(Entity.UserClientSecurityGroupMap x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.UserClientSecurityGroupMapID = DataAccess.UserClientSecurityGroupMap.Save(x);
                scope.Complete();
            }

            return x.UserClientSecurityGroupMapID;
        }

        public void Delete(int UserClientSecurityGroupMapID)
        {
            using(TransactionScope scope = new TransactionScope())
            {
                DataAccess.UserClientSecurityGroupMap.Delete(UserClientSecurityGroupMapID);
                scope.Complete();
            }
        }
    }
}
