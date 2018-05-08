using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class SecurityGroupBrandMap
    {
        public List<Entity.SecurityGroupBrandMap> SelectForBrand(KMPlatform.Object.ClientConnections client, int brandID)
        {
            List<Entity.SecurityGroupBrandMap> x = null;
            x = DataAccess.SecurityGroupBrandMap.SelectForBrand(client, brandID).ToList();

            return x;
        }
        public List<Entity.SecurityGroupBrandMap> SelectForSecurityGroup(KMPlatform.Object.ClientConnections client, int securityGroupID)
        {
            List<Entity.SecurityGroupBrandMap> x = null;
            x = DataAccess.SecurityGroupBrandMap.SelectForSecurityGroup(client, securityGroupID).ToList();

            return x;
        }
        public int Save(KMPlatform.Object.ClientConnections client, Entity.SecurityGroupBrandMap x)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;
            using (TransactionScope scope = new TransactionScope())
            {
                x.SecurityGroupBrandMapID = DataAccess.SecurityGroupBrandMap.Save(client, x);
                scope.Complete();
            }

            return x.SecurityGroupBrandMapID;
        }
    }
}
