using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace KMPlatform.BusinessLogic
{
    public class ApplicationServiceMap
    {
        public List<Entity.ApplicationServiceMap> Select()
        {
            List<Entity.ApplicationServiceMap> x = null;
            x = DataAccess.ApplicationServiceMap.Select().ToList();

            return x;
        }
        public List<Entity.ApplicationServiceMap> SelectForApplication(int applicationID)
        {
            List<Entity.ApplicationServiceMap> x = null;
            x = DataAccess.ApplicationServiceMap.SelectForApplication(applicationID).ToList();

            return x;
        }
        public List<Entity.ApplicationServiceMap> SelectForService(int serviceID)
        {
            List<Entity.ApplicationServiceMap> x = null;
            x = DataAccess.ApplicationServiceMap.SelectForService(serviceID).ToList();

            return x;
        }
        public int Save(Entity.ApplicationServiceMap x)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;
            using (TransactionScope scope = new TransactionScope())
            {
                x.ApplicationServiceMapID = DataAccess.ApplicationServiceMap.Save(x);
                scope.Complete();
            }

            return x.ApplicationServiceMapID;
        }
    }
}
