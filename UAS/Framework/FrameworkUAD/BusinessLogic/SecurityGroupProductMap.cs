using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class SecurityGroupProductMap
    {
        public List<Entity.SecurityGroupProductMap> SelectForProduct(KMPlatform.Object.ClientConnections client, int productID)
        {
            List<Entity.SecurityGroupProductMap> x = null;
            x = DataAccess.SecurityGroupProductMap.SelectForProduct(client, productID).ToList();

            return x;
        }
        public List<Entity.SecurityGroupProductMap> SelectForSecurityGroup(KMPlatform.Object.ClientConnections client, int securityGroupID)
        {
            List<Entity.SecurityGroupProductMap> x = null;
            x = DataAccess.SecurityGroupProductMap.SelectForSecurityGroup(client, securityGroupID).ToList();

            return x;
        }
        public int Save(KMPlatform.Object.ClientConnections client, Entity.SecurityGroupProductMap x)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;
            using (TransactionScope scope = new TransactionScope())
            {
                x.SecurityGroupProductMapID = DataAccess.SecurityGroupProductMap.Save(client, x);
                scope.Complete();
            }

            return x.SecurityGroupProductMapID;
        }
    }
}
