using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace KMPlatform.BusinessLogic
{
    public class Service
    {
        #region AMS
        public List<Entity.Service> AMS_SelectForSecurityGroupAndUserID(int securityGroupID, int userID, bool includeObjects = false)
        {
            List<Entity.Service> x = null;
            x = DataAccess.Service.AMS_SelectForSecurityGroupAndUserID(securityGroupID, userID);

            foreach (Entity.Service s in x)
            {
                ServiceFeature sfWorker = new ServiceFeature();
                Application appWorker = new Application();

                s.ServiceFeatures = sfWorker.SelectOnlyEnabled(s.ServiceID).ToList();
                s.Applications = appWorker.SelectOnlyEnabledForService(s.ServiceID, userID).ToList();
            }
            return x;
        }
        public List<Entity.Service> AMS_SelectForSecurityGroup(int securityGroupID, bool includeObjects = false)
        {
            List<Entity.Service> x = null;
            x = DataAccess.Service.AMS_SelectForSecurityGroup(securityGroupID);

            foreach (Entity.Service s in x)
            {
                ServiceFeature sfWorker = new ServiceFeature();
                Application appWorker = new Application();

                s.ServiceFeatures = sfWorker.SelectOnlyEnabled(s.ServiceID).ToList();
                s.Applications = appWorker.SelectOnlyEnabledForService(s.ServiceID).ToList();
            }
            return x;
        }
        public List<Entity.Service> AMS_SelectForSecurityGroupAndClientGroupID(int securityGroupID, int clientGroupID, bool includeObjects = false)
        {
            List<Entity.Service> x = null;
            x = DataAccess.Service.AMS_SelectForSecurityGroupAndClientGroupID(securityGroupID, clientGroupID);

            foreach (Entity.Service s in x)
            {
                ServiceFeature sfWorker = new ServiceFeature();
                Application appWorker = new Application();

                s.ServiceFeatures = sfWorker.SelectOnlyEnabled(s.ServiceID).ToList();
                s.Applications = appWorker.SelectOnlyEnabledForService(s.ServiceID).ToList();
            }
            return x;
        }
        #endregion
        public List<Entity.Service> Select(bool includeObjects = false)
        {
            List<Entity.Service> x = null;
            x = DataAccess.Service.Select();

            if (includeObjects)
            {
                foreach (Entity.Service s in x)
                {
                    ServiceFeature sfWorker = new ServiceFeature();
                    Application appWorker = new Application();

                    s.ServiceFeatures = sfWorker.SelectOnlyEnabled(s.ServiceID).ToList();
                    s.Applications = appWorker.SelectOnlyEnabledForService(s.ServiceID).ToList();
                }
            }
            return x;
        }
        public List<Entity.Service> SelectForUser(int userID, bool includeObjects = false)
        {
            List<Entity.Service> x = null;
            x = DataAccess.Service.SelectForUser(userID);

            foreach (Entity.Service s in x)
            {
                ServiceFeature sfWorker = new ServiceFeature();
                Application appWorker = new Application();

                s.ServiceFeatures = sfWorker.SelectOnlyEnabled(s.ServiceID).ToList();
                s.Applications = appWorker.SelectOnlyEnabledForService(s.ServiceID).ToList();
            }
            return x;
        }
        public Entity.Service Select(int serviceID, bool includeObjects = false)
        {
            Entity.Service x = null;

            x = DataAccess.Service.Select(serviceID);

            if (includeObjects)
                x = SetObjects(x);

            return x;
        }

        public Entity.Service Select(KMPlatform.Enums.Services service, bool includeObjects = false)
        {
            Entity.Service x = null;
            x = DataAccess.Service.Select(service);

            if (includeObjects)
                x = SetObjects(x);

            return x;
        }

        public List<Entity.Service> SelectForUserAuthorization(int userID)
        {
            List<Entity.Service> x = null;
            x = DataAccess.Service.SelectForUserAuthorization(userID);

            foreach (Entity.Service s in x)
            {
                ServiceFeature sfWorker = new ServiceFeature();
                Application appWorker = new Application();

                s.ServiceFeatures = sfWorker.SelectOnlyEnabled(s.ServiceID).ToList();
                s.Applications = appWorker.SelectForUser(userID).Where(z => z.IsActive == true).ToList();//SelectOnlyEnabledForService(s.ServiceID).ToList();
            }
            return x;
        }

        public List<Entity.Service> SelectForSecurityGroupID(int securityGroupID, bool includeObjects = false)
        {
            List<Entity.Service> x = null;
            x = DataAccess.Service.SelectForSecurityGroupID(securityGroupID);

            if (includeObjects)
            {
                foreach (Entity.Service s in x)
                {
                    ServiceFeature sfWorker = new ServiceFeature();
                    Application appWorker = new Application();

                    s.ServiceFeatures = sfWorker.SelectOnlyEnabled(s.ServiceID).ToList();
                    s.Applications = appWorker.SelectOnlyEnabledForService(s.ServiceID).ToList();
                }
            }
            return x;
        }

        public List<Entity.Service> SelectForClientGroupID( int clientGroupID, bool includeObjects = false)
        {
            List<Entity.Service> x = null;
            x = DataAccess.Service.SelectForClientGroupID(clientGroupID);

            if (includeObjects)
            {
                foreach (Entity.Service s in x)
                {
                    ServiceFeature sfWorker = new ServiceFeature();

                    s.ServiceFeatures = sfWorker.SelectOnlyEnabledClientGroupID(s.ServiceID, clientGroupID).ToList();
                }
            }
            return x;
        }

        public Entity.Service SetObjectsForUserAuthorization(Entity.Service service)
        {
            ServiceFeature sfWorker = new ServiceFeature();
            Application appWorker = new Application();

            service.ServiceFeatures = sfWorker.SelectOnlyEnabled(service.ServiceID).ToList();
            service.Applications = appWorker.SelectOnlyEnabledForService(service.ServiceID).ToList();
            return service;
        }

        public List<Entity.Service> SelectForSecurityGroupAndUserID(int securityGroupID, int userID, bool includeObjects = false)
        {
            List<Entity.Service> x = null;
            x = DataAccess.Service.SelectForSecurityGroupAndUserID(securityGroupID, userID);

            foreach (Entity.Service s in x)
            {
                ServiceFeature sfWorker = new ServiceFeature();
                Application appWorker = new Application();

                s.ServiceFeatures = sfWorker.SelectOnlyEnabled(s.ServiceID).ToList();
                s.Applications = appWorker.SelectOnlyEnabledForService(s.ServiceID, userID).ToList();
            }
            return x;
        }

        public List<Entity.Service> SelectForSecurityGroupAndClientGroupID(int securityGroupID, int clientGroupID, bool isKMUser, bool includeObjects = false)
        {
            List<Entity.Service> x = null;
            x = DataAccess.Service.SelectForSecurityGroupAndClientGroupID(securityGroupID, clientGroupID);

            foreach (Entity.Service s in x)
            {
                ServiceFeature sfWorker = new ServiceFeature();
                Application appWorker = new Application();

                s.ServiceFeatures = sfWorker.SelectOnlyEnabled(s.ServiceID).ToList();
                s.Applications = appWorker.SelectOnlyEnabledForServiceWithSecurityGroup(s.ServiceID, securityGroupID, isKMUser).ToList();
            }
            return x;
        }

        public List<Entity.Service> SelectForSecurityGroup(int securityGroupID, bool includeObjects = false)
        {
            List<Entity.Service> x = null;
            x = DataAccess.Service.SelectForSecurityGroup(securityGroupID);

            foreach (Entity.Service s in x)
            {
                ServiceFeature sfWorker = new ServiceFeature();
                Application appWorker = new Application();

                s.ServiceFeatures = sfWorker.SelectOnlyEnabled(s.ServiceID).ToList();
                s.Applications = appWorker.SelectOnlyEnabledForService(s.ServiceID).ToList();
            }
            return x;
        }

        public List<Entity.Service> SelectForClientID(int clientID, bool includeObjects = false)
        {
            List<Entity.Service> x = null;

            return DataAccess.Service.SelectForClientID(clientID, includeObjects);
        }

        public Entity.Service SetObjects(Entity.Service service)
        {
            ServiceFeature sfWorker = new ServiceFeature();
            Application appWorker = new Application();

            service.ServiceFeatures = sfWorker.SelectOnlyEnabled(service.ServiceID).ToList();
            service.Applications = appWorker.SelectOnlyEnabledForService(service.ServiceID).ToList();
            return service;
        }

        public int Save(Entity.Service x)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            using (TransactionScope scope = new TransactionScope())
            {
                x.ServiceID = DataAccess.Service.Save(x);
                scope.Complete();
            }

            return x.ServiceID;
        }
    }
}
