using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace KMPlatform.BusinessLogic
{
    public class ClientGroupSecurityGroupMap
    {
        public List<Entity.ClientGroupSecurityGroupMap> Select()
        {
            List<Entity.ClientGroupSecurityGroupMap> retItem = null;
            retItem = DataAccess.ClientGroupSecurityGroupMap.Select().ToList();
            return retItem;
        }
        public List<Entity.ClientGroupSecurityGroupMap> SelectForClientGroup(int clientGroupID)
        {
            List<Entity.ClientGroupSecurityGroupMap> retItem = null;
            retItem = DataAccess.ClientGroupSecurityGroupMap.SelectForClientGroup(clientGroupID).ToList();
            return retItem;
        }
        public List<Entity.ClientGroupSecurityGroupMap> SelectForSecurityGroup(int securityGroupID)
        {
            List<Entity.ClientGroupSecurityGroupMap> retItem = null;
            retItem = DataAccess.ClientGroupSecurityGroupMap.SelectForSecurityGroup(securityGroupID).ToList();
            return retItem;
        }
        public int Save(Entity.ClientGroupSecurityGroupMap x)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;
            using (TransactionScope scope = new TransactionScope())
            {
                x.ClientGroupSecurityGroupMapID = DataAccess.ClientGroupSecurityGroupMap.Save(x);
                scope.Complete();
            }

            return x.ClientGroupSecurityGroupMapID;
        }
    }
}
