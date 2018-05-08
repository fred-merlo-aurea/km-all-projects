using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace KMPlatform.BusinessLogic
{
    public class ClientGroup
    {
        public List<Entity.ClientGroup> Select(bool includeObjects = false)
        {
            List<Entity.ClientGroup> x = null;
            x = DataAccess.ClientGroup.Select().ToList();
            if (includeObjects == true)
            {
                foreach (Entity.ClientGroup cg in x)
                {
                    SecurityGroup sgWorker = new SecurityGroup();
                    Client cWorker = new Client();

                    cg.SecurityGroups = sgWorker.SelectForClientGroup(cg.ClientGroupID).ToList();
                    cg.Clients = cWorker.SelectForClientGroup(cg.ClientGroupID, true);
                }
            }
            return x;
        }
        public List<Entity.ClientGroup> SelectForAMS(bool includeObjects = false)
        {
            List<Entity.ClientGroup> x = null;
            x = DataAccess.ClientGroup.SelectForAMS().ToList();
            if (includeObjects == true)
            {
                foreach (Entity.ClientGroup cg in x)
                {
                    SecurityGroup sgWorker = new SecurityGroup();
                    Client cWorker = new Client();

                    cg.SecurityGroups = sgWorker.SelectForClientGroup(cg.ClientGroupID).ToList();
                    cg.Clients = cWorker.SelectForClientGroup(cg.ClientGroupID, true);
                }
            }
            return x;
        }
        public List<Entity.ClientGroup> SelectForAMSWithClientList(bool includeClientObjects = false)
        {
            List<Entity.ClientGroup> x = null;
            x = DataAccess.ClientGroup.SelectForAMS().ToList();
            foreach (Entity.ClientGroup cg in x)
            {
                Client cWorker = new Client();
                cg.Clients = cWorker.SelectForClientGroup(cg.ClientGroupID, includeClientObjects);
            }
            return x;
        }
        public List<Entity.ClientGroup> SelectDefaultForAuthorizedUser(bool includeObjects = false)
        {
            List<Entity.ClientGroup> x = null;
            x = DataAccess.ClientGroup.Select().ToList();
            if (includeObjects == true)
            {
                foreach (Entity.ClientGroup cg in x)
                {
                    SecurityGroup sgWorker = new SecurityGroup();
                    Client cWorker = new Client();

                    cg.SecurityGroups = sgWorker.SelectActiveForClientGroup(cg.ClientGroupID).ToList();
                    cg.Clients = cWorker.SelectActiveForClientGroup(cg.ClientGroupID);
                }
            }
            return x;
        }
        public List<Entity.ClientGroup> SelectClient(int clientID, bool includeObjects = false)
        {
            List<Entity.ClientGroup> x = null;
            x = DataAccess.ClientGroup.SelectClient(clientID).ToList();
            if (includeObjects == true)
            {
                foreach (Entity.ClientGroup cg in x)
                {
                    SecurityGroup sgWorker = new SecurityGroup();
                    Client cWorker = new Client();

                    cg.SecurityGroups = sgWorker.SelectForClientGroup(cg.ClientGroupID).ToList();
                    cg.Clients = cWorker.SelectForClientGroup(cg.ClientGroupID, true);
                }
            }
            return x;
        }
        
        public List<Entity.ClientGroup> SelectForUser(int userID, bool includeObjects = false)
        {
            List<Entity.ClientGroup> x = null;
            x = DataAccess.ClientGroup.SelectForUser(userID).ToList();
            if (includeObjects == true)
            {
                foreach (Entity.ClientGroup cg in x)
                {
                    SecurityGroup sgWorker = new SecurityGroup();
                    Client cWorker = new Client();

                    cg.SecurityGroups = sgWorker.SelectForClientGroup(cg.ClientGroupID).ToList();
                    cg.Clients = cWorker.SelectForClientGroup(cg.ClientGroupID, true);
                }
            }
            return x;
        }
        public List<Entity.ClientGroup> SelectForUserAuthorization(int userID, bool includeObjects = true)
        {
            List<Entity.ClientGroup> x = null;
            x = DataAccess.ClientGroup.SelectForUserAuthorization(userID).ToList();
            if (includeObjects == true)
            {
                foreach (Entity.ClientGroup cg in x)
                {
                    SecurityGroup sgWorker = new SecurityGroup();
                    Client cWorker = new Client();

                    cg.SecurityGroups = sgWorker.SelectActiveForClientGroup(cg.ClientGroupID).ToList();
                    cg.Clients = cWorker.SelectActiveForClientGroup(cg.ClientGroupID);
                }
            }
            return x;
        }
        public List<Entity.ClientGroup> SelectForUserAuthorizationLite(int userID, bool includeObjects = true)
        {
            List<Entity.ClientGroup> x = null;
            x = DataAccess.ClientGroup.SelectForUserAuthorization(userID).ToList();
            if (includeObjects == true)
            {
                foreach (Entity.ClientGroup cg in x)
                {
                    Client cWorker = new Client();
                    cg.Clients = cWorker.SelectActiveForClientGroupLite(cg.ClientGroupID);
                }
            }

            return x;
        }
        public List<Entity.ClientGroup> SelectLite()
        {
            List<Entity.ClientGroup> x = null;
            x = DataAccess.ClientGroup.Select();
            foreach (Entity.ClientGroup cg in x)
            {
                Client cWorker = new Client();
                cg.Clients = cWorker.SelectActiveForClientGroupLite(cg.ClientGroupID);
            }

            return x;
        }
        public Entity.ClientGroup Select(int clientGroupID, bool includeObjects = false)
        {
            Entity.ClientGroup x = null;
            x = DataAccess.ClientGroup.Select(clientGroupID);
            if (includeObjects == true && x != null)
                x = SetObjects(x);

            return x;
        }
        public Entity.ClientGroup SetObjects(Entity.ClientGroup cg)
        {
            SecurityGroup sgWorker = new SecurityGroup();
            Client cWorker = new Client();

            cg.Clients = cWorker.SelectForClientGroupLite(cg.ClientGroupID, true);
            cg.SecurityGroups = sgWorker.SelectForClientGroup(cg.ClientGroupID).ToList();

            return cg;
        }
        public Entity.ClientGroup SetObjectsForUserAuthorization(Entity.ClientGroup cg)
        {
            SecurityGroup sgWorker = new SecurityGroup();
            Client cWorker = new Client();

            cg.SecurityGroups = sgWorker.SelectActiveForClientGroup(cg.ClientGroupID).ToList();
            cg.Clients = cWorker.SelectActiveForClientGroup(cg.ClientGroupID);

            return cg;
        }
        public int Save(Entity.ClientGroup x)
        {
            
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            using (TransactionScope scope = new TransactionScope())
            {
                x.ClientGroupID = DataAccess.ClientGroup.Save(x);
                scope.Complete();
            }

            return x.ClientGroupID;
        }

        public static bool HasServiceFeature(int clientgroupID, KMPlatform.Enums.Services serviceCode, KMPlatform.Enums.ServiceFeatures servicefeatureCode)
        {
            try
            {
                List<KMPlatform.Entity.Service> services = (new Service()).SelectForClientGroupID(clientgroupID, true);

                if (!(services.First(a => a.ServiceCode.Equals(serviceCode.ToString(), StringComparison.InvariantCultureIgnoreCase))).ServiceFeatures.Any(x => x.SFCode.Equals(servicefeatureCode.ToString(), StringComparison.InvariantCultureIgnoreCase)))
                {
                    return false;
                }
                return true;

            }
            catch
            {
                return false;
            }

        }

        public List<KMPlatform.Entity.ClientGroup> SelectForAtLeastCustomerAdmin(int userID)
        {
            List<KMPlatform.Entity.ClientGroup> retList = new List<Entity.ClientGroup>();
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = DataAccess.ClientGroup.SelectForAtLeastCustomerAdmin(userID);
                scope.Complete();
            }
            return retList;

        }
    }
}
