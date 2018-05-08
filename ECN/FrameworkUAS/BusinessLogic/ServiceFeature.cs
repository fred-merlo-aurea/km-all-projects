using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace KMPlatform.BusinessLogic
{
    public class ServiceFeature
    {
        public List<Entity.ServiceFeature> Select()
        {
            List<Entity.ServiceFeature> x = null;
            x = DataAccess.ServiceFeature.Select();

            return x;
        }
        public List<Entity.ServiceFeature> Select(int serviceID)
        {
            List<Entity.ServiceFeature> x = null;
            x = DataAccess.ServiceFeature.Select(serviceID);

            return x;
        }
        public List<Entity.ServiceFeature> SelectOnlyEnabled(int serviceID)
        {
            List<Entity.ServiceFeature> x = null;
            x = DataAccess.ServiceFeature.SelectOnlyEnabled(serviceID);

            return x;
        }
        public List<Entity.ServiceFeature> SelectOnlyEnabledClientGroupID(int serviceID, int clientGroupID)
        {
            List<Entity.ServiceFeature> x = null;
            x = DataAccess.ServiceFeature.SelectOnlyEnabledClientGroupID(serviceID, clientGroupID);

            return x;
        }
        public List<Entity.ServiceFeature> SelectOnlyEnabledClientID(int serviceID, int clientID)
        {
            List<Entity.ServiceFeature> x = null;
            x = DataAccess.ServiceFeature.SelectOnlyEnabledClientID(serviceID, clientID);

            return x;
        }

        public List<Entity.ServiceFeature.ClientGroupTreeListRow> GetClientGroupTreeList(int? clientGroupID, bool isAdditionalCost)
        {
            return DataAccess.ServiceFeature.GetClientGroupTreeList(clientGroupID??0, isAdditionalCost);
        }

        public List<Entity.ServiceFeature.SecurityGroupTreeListRow> GetEmptySecurityGroupTreeList(int clientGroupID, int clientID)
        {
            return DataAccess.ServiceFeature.GetEmptySecurityGroupTreeList(clientGroupID, clientID);
        }

        public List<Entity.ServiceFeature.SecurityGroupTreeListRow> GetSecurityGroupTreeList(int securityGroupID, int clientGroupID, int clientID)
        {
            return DataAccess.ServiceFeature.GetSecurityGroupTreeList(securityGroupID, clientGroupID, clientID);
        }

        public List<Entity.ServiceFeature.ClientGroupTreeListRow> GetClientTreeList(int clientGroupID, int? clientID, bool isAdditionalCost)
        {
            return DataAccess.ServiceFeature.GetClientTreeList(clientGroupID, clientID ?? -1, isAdditionalCost);
        }

        public Entity.ServiceFeature SelectServiceFeature(int serviceFeatureID)
        {
            Entity.ServiceFeature x = null;
            x = DataAccess.ServiceFeature.SelectServiceFeature(serviceFeatureID);

            return x;
        }

        public static bool HasAccess(int userID, int customerID, KMPlatform.Enums.Services serviceCode, KMPlatform.Enums.ServiceFeatures sfCode, KMPlatform.Enums.Access accessCode)
        {
            return DataAccess.ServiceFeature.HasAccess(userID, customerID, serviceCode, sfCode, accessCode);
        }  
     
        public bool Save(Entity.ServiceFeature x)
        {
            bool done = false;

            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            using (TransactionScope scope = new TransactionScope())
            {
                done = DataAccess.ServiceFeature.Save(x);
                scope.Complete();
            }

            return done;
        }
        public int SaveReturnId(Entity.ServiceFeature x)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            using (TransactionScope scope = new TransactionScope())
            {
                x.ServiceFeatureID = DataAccess.ServiceFeature.SaveReturnId(x);
                scope.Complete();
            }

            return x.ServiceFeatureID;
        }
    }
}
