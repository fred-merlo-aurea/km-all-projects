using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
//using System.Data;

namespace KMPlatform.BusinessLogic
{
    public class Client
    {
        public List<Entity.Client> SelectAMS(bool isAms = true, bool isActive = true, bool includeObjects = false)
        {
            List<Entity.Client> x = null;
            x = DataAccess.Client.SelectAMS(isAms,isActive).ToList();
            foreach (var c in x)
            {
                c.ClientConnections.ClientLiveDBConnectionString = c.ClientLiveDBConnectionString;
                c.ClientConnections.ClientTestDBConnectionString = c.ClientTestDBConnectionString;
            }

            if (includeObjects)
            {
                foreach (var c in x)
                {
                    SetObjects(c);
                }
            }

            return x;
        }
        public List<Entity.Client> Select(bool includeObjects = false)
        {
            List<Entity.Client> x = null;
            x = DataAccess.Client.Select().ToList();
            foreach (var c in x)
            {
                c.ClientConnections.ClientLiveDBConnectionString = c.ClientLiveDBConnectionString;
                c.ClientConnections.ClientTestDBConnectionString = c.ClientTestDBConnectionString;
            }

            if (includeObjects)
            {
                foreach (var c in x)
                {
                    SetObjects(c);
                }
            }

            return x;
        }
        public List<Entity.Client> AMS_SelectPaid(bool includeObjects = false)
        {
            List<Entity.Client> x = null;
            x = DataAccess.Client.AMS_SelectPaid().ToList();
            foreach (var c in x)
            {
                c.ClientConnections.ClientLiveDBConnectionString = c.ClientLiveDBConnectionString;
                c.ClientConnections.ClientTestDBConnectionString = c.ClientTestDBConnectionString;
            }

            if (includeObjects)
            {
                foreach (var c in x)
                {
                    SetObjects(c);
                }
            }

            return x;
        }
        public Entity.Client Select(string clientName, bool includeObjects = false)
        {
            Entity.Client x = null;
            x = DataAccess.Client.Select(clientName);
            x.ClientConnections.ClientLiveDBConnectionString = x.ClientLiveDBConnectionString;
            x.ClientConnections.ClientTestDBConnectionString = x.ClientTestDBConnectionString;

            if (includeObjects && x != null)
            {
                SetObjects(x);
            }

            return x;
        }
        public Entity.Client SelectFtpFolder(string ftpFolder, bool includeObjects = false)
        {
            Entity.Client x = null;
            x = DataAccess.Client.SelectFtpFolder(ftpFolder);
            x.ClientConnections.ClientLiveDBConnectionString = x.ClientLiveDBConnectionString;
            x.ClientConnections.ClientTestDBConnectionString = x.ClientTestDBConnectionString;

            if (includeObjects && x != null)
            {
                SetObjects(x);
            }

            return x;
        }
        public Entity.Client Select(int clientID, bool includeObjects = false)
        {
            Entity.Client x = null;
            x = DataAccess.Client.Select(clientID);
            x.ClientConnections.ClientLiveDBConnectionString = x.ClientLiveDBConnectionString;
            x.ClientConnections.ClientTestDBConnectionString = x.ClientTestDBConnectionString;
            if (includeObjects && x != null)
            {
                 SetObjects(x);
            }

            return x;
        }

        public List<Entity.Client> SelectbyUserID(int userID, bool includeObjects = false)
        {
            List<Entity.Client> x = null;
            x = DataAccess.Client.SelectbyUserID(userID);
            foreach (Entity.Client c in x)
            {
                c.ClientConnections.ClientLiveDBConnectionString = c.ClientLiveDBConnectionString;
                c.ClientConnections.ClientTestDBConnectionString = c.ClientTestDBConnectionString;
            }

            if (includeObjects)
            {
                foreach (Entity.Client c in x)
                {
                    SetObjects(c);
                }
            }

            return x;
        }



        public List<Entity.Client> SelectbyUserIDclientgroupID(int userID, int clientgroupID, bool includeObjects = false)
        {
            List<Entity.Client> x = null;
            x = DataAccess.Client.SelectbyUserIDclientgroupID(userID, clientgroupID);
            foreach (var c in x)
            {
                c.ClientConnections.ClientLiveDBConnectionString = c.ClientLiveDBConnectionString;
                c.ClientConnections.ClientTestDBConnectionString = c.ClientTestDBConnectionString;
            }

            if (includeObjects)
            {
                foreach (var c in x)
                {
                    SetObjects(c);
                    }
                }

            return x;
        }

        public Entity.Client SelectDefault(Guid accessKey, bool includeObjects = false)
        {
            Entity.Client x = null;
            x = DataAccess.Client.SelectDefault(accessKey);
            if (x != null)
            {
                x.ClientConnections.ClientLiveDBConnectionString = x.ClientLiveDBConnectionString;
                x.ClientConnections.ClientTestDBConnectionString = x.ClientTestDBConnectionString;
            }
            if (includeObjects && x != null)
            {
                SetObjects(x);
            }

            return x;
        }
        public List<Entity.Client> SelectForAccessKey(Guid accessKey, bool includeObjects = false)
        {
            List<Entity.Client> x = null;
            x = DataAccess.Client.SelectForAccessKey(accessKey);
            foreach (var c in x)
            {
                c.ClientConnections.ClientLiveDBConnectionString = c.ClientLiveDBConnectionString;
                c.ClientConnections.ClientTestDBConnectionString = c.ClientTestDBConnectionString;
            }
            if (includeObjects)
            {
                foreach (var c in x)
                {
                    SetObjects(c);
                }
            }

            return x;
        }
        public List<Entity.Client> SelectForClientGroup(int clientGroupID, bool includeObjects = false)
        {
            List<Entity.Client> x = null;
            x = DataAccess.Client.SelectForClientGroup(clientGroupID);
            foreach (var c in x)
            {
                c.ClientConnections.ClientLiveDBConnectionString = c.ClientLiveDBConnectionString;
                c.ClientConnections.ClientTestDBConnectionString = c.ClientTestDBConnectionString;
            }
            if (includeObjects)
            {
                foreach (var c in x)
                {
                    SetObjects(c);
                }
            }

            return x;
        }
        public List<Entity.Client> SelectForClientGroupLite(int clientGroupID, bool includeObjects = false)
        {
            List<Entity.Client> x = null;
            x = DataAccess.Client.SelectForClientGroup(clientGroupID);
            foreach (var c in x)
            {
                c.ClientConnections.ClientLiveDBConnectionString = c.ClientLiveDBConnectionString;
                c.ClientConnections.ClientTestDBConnectionString = c.ClientTestDBConnectionString;
            }

            return x;
        }
        public List<Entity.Client> SelectActiveForClientGroup(int clientGroupID)
        {
            List<Entity.Client> x = null;
            x = DataAccess.Client.SelectActiveForClientGroup(clientGroupID);
            foreach (var c in x)
            {
                c.ClientConnections.ClientLiveDBConnectionString = c.ClientLiveDBConnectionString;
                c.ClientConnections.ClientTestDBConnectionString = c.ClientTestDBConnectionString;
            }
            foreach (var c in x)
            {
                SetObjects(c);
            }
            return x;
        }
        public List<Entity.Client> SelectActiveForClientGroupLite(int clientGroupID)
        {
            List<Entity.Client> x = null;
            x = DataAccess.Client.SelectActiveForClientGroup(clientGroupID);
            foreach (var c in x)
            {
                c.ClientConnections.ClientLiveDBConnectionString = c.ClientLiveDBConnectionString;
                c.ClientConnections.ClientTestDBConnectionString = c.ClientTestDBConnectionString;
                c.Services = (new BusinessLogic.Service()).SelectForClientID(c.ClientID, false);
                //c.Products = DataAccess.Client.SelectProduct(c).ToList(); 
            }
            
            return x;
        }
        public int Save(Entity.Client x, bool UpdateUsersForSecurityGroups = false, int? clientGroupID = null)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            using (TransactionScope scope = new TransactionScope())
            {
                int origClientID = x.ClientID;
                x.ClientID = DataAccess.Client.Save(x);
                if(UpdateUsersForSecurityGroups && origClientID <= 0 && clientGroupID.HasValue)//only need to do for customer creation
                    DataAccess.Client.UpdateUsersForSecurityGroups(x.ClientID, clientGroupID.Value);

                scope.Complete();
            }

            return x.ClientID;
        }
        public List<KMPlatform.Entity.Client> Search(string searchValue, List<KMPlatform.Entity.Client> searchList, bool? isActive = null)
        {
            searchValue = searchValue.ToLower().Trim();
            List<KMPlatform.Entity.Client> matchList = new List<KMPlatform.Entity.Client>();

            matchList.AddRange(searchList.FindAll(x => x.ClientName.ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.ClientLiveDBConnectionString.ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.ClientTestDBConnectionString.ToLower().Contains(searchValue)));
            matchList.AddRange(searchList.FindAll(x => x.AccountManagerEmails.ToLower().Contains(searchValue)));

            if (isActive != null)
                matchList = matchList.Where(x => x.IsActive == isActive).ToList();

            return matchList.Distinct().ToList();
        }
        private Entity.Client SetObjects(KMPlatform.Entity.Client client)
        {
            client.ClientConfigurations = DataAccess.ClientConfigurationMap.Select(client.ClientID);

            if (client.ClientConnections == null || string.IsNullOrEmpty(client.ClientConnections.ClientLiveDBConnectionString) || string.IsNullOrEmpty(client.ClientConnections.ClientTestDBConnectionString))
            {
                client.ClientConnections = new Object.ClientConnections();
                client.ClientConnections.ClientLiveDBConnectionString = client.ClientLiveDBConnectionString;
                client.ClientConnections.ClientTestDBConnectionString = client.ClientTestDBConnectionString;
            }

            client.Services = new KMPlatform.BusinessLogic.Service().SelectForClientID(client.ClientID,true);
            client.Products = DataAccess.Client.SelectProduct(client).ToList(); 

            return client;
        }
        public List<Object.Product> SelectProducts(KMPlatform.Entity.Client client)
        {
            return DataAccess.Client.SelectProduct(client).ToList();
        }
        public bool UseUADSuppressionFeature(int clientId)
        {
            return true;
        }
        public List<Entity.Client> SelectForAtLeastCustAdmin(int clientGroupID, int userID)
        {
            List<Entity.Client> retList = new List<Entity.Client>();
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = DataAccess.Client.SelectForAtLeastCustAdmin(clientGroupID, userID);
                scope.Complete();
            }
            return retList;
        }

        #region Services and Features
       
        public static bool HasService(int clientID, KMPlatform.Enums.Services serviceCode)
        {
            try
            {
                List<KMPlatform.Entity.Service> services = (new Service()).SelectForClientID(clientID, false);

                return services.Any(x => x.ServiceCode.Equals(serviceCode.ToString(), StringComparison.InvariantCultureIgnoreCase));
            }
            catch
            {
                return false;
            }
        }

        public static bool HasServiceFeature(int clientID, KMPlatform.Enums.Services serviceCode, KMPlatform.Enums.ServiceFeatures servicefeatureCode)
        {
            try
            {
                List<KMPlatform.Entity.Service> services = (new Service()).SelectForClientID(clientID, true);

                if (!(services.First(a => a.ServiceCode.Equals(serviceCode.ToString(), StringComparison.InvariantCultureIgnoreCase)).ServiceFeatures.Any(x => x.SFCode.Equals(servicefeatureCode.ToString(), StringComparison.InvariantCultureIgnoreCase))))
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

        public bool HasService(int clientID, KMPlatform.Enums.Services service, int clientGroupID = 1)
        {
            bool hasService = false;
            string serviceName = service.ToString().Replace("_", " ");
            hasService = DataAccess.Client.HasService(clientID, clientGroupID, serviceName);

            return hasService;
        }
        public bool HasService(Entity.Client client, KMPlatform.Enums.Services service, int clientGroupID = 1)
        {
            return HasService(client.ClientID, service, clientGroupID);
        }
        //public bool HasFeature(int clientID, KMPlatform.Enums.Services service, string featureName, int clientGroupID = 1)
        //{
        //    bool hasFeature = false;
        //    string serviceName = service.ToString().Replace("_", " ");
        //    featureName = featureName.Replace("_", " ");
        //    hasFeature = DataAccess.Client.HasFeature(clientID, clientGroupID, serviceName, featureName);

        //    return hasFeature;
        //}
        //public bool HasFeature(Entity.Client client, KMPlatform.Enums.Services service, string featureName, int clientGroupID = 1)
        //{
        //    return HasFeature(client.ClientID, service, featureName, clientGroupID);
        //}
        public bool HasFulfillmentService(int clientID, int clientGroupID = 1)
        {
            return HasService(clientID, KMPlatform.Enums.Services.FULFILLMENT, clientGroupID);
        }
        public bool HasFulfillmentService(Entity.Client client, int clientGroupID = 1)
        {
            return HasService(client, KMPlatform.Enums.Services.FULFILLMENT, clientGroupID);
        }
        //public bool UseUADSuppressionFeature(int clientID, int clientGroupID = 1)
        //{
        //    return HasFeature(clientID, KMPlatform.Enums.Services.UAD, Enums.UADFeatures.Suppression.ToString(), clientGroupID);
        //}
        //public bool UseUADSuppressionFeature(Entity.Client client, int clientGroupID = 1)
        //{
        //    return HasFeature(client, KMPlatform.Enums.Services.UAD, Enums.UADFeatures.Suppression.ToString(), clientGroupID);
        //}

        #endregion

        public bool Exists(string clientName)
        {
            bool exists = false;
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = DataAccess.Client.Exists(clientName);
                scope.Complete();
            }
            return exists;
        }
        /// <summary>
        /// ONLY CALL THIS IF SAVING ECN CUSTOMER FAILS!!
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public void Delete(int clientID)
        {
            using(TransactionScope scope = new TransactionScope())
            {
                DataAccess.Client.Delete(clientID);
                scope.Complete();
            }
        }
        #region ECN Methods

        /// <summary>
        /// Will get all clients that a user can access, with respect to security groups
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="includeObjects"></param>
        /// <param name="isFileDeleted"></param>
        /// <returns></returns>
        public List<Entity.Client> ECN_SelectAllByUserID(int userID, bool includeObjects = false)
        {
            List<Entity.Client> x = null;
            x = DataAccess.Client.SelectAllByUserID(userID);
            foreach (Entity.Client c in x)
            {
                c.ClientConnections.ClientLiveDBConnectionString = c.ClientLiveDBConnectionString;
                c.ClientConnections.ClientTestDBConnectionString = c.ClientTestDBConnectionString;
            }

            if (includeObjects)
            {
                foreach (Entity.Client c in x)
                {
                    ECN_SetObjects(c);
                }
            }

            return x;
        }

        /// <summary>
        /// Only call to build Client Object in ECN
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="includeObjects"></param>
        /// <returns></returns>
        public Entity.Client ECN_Select(int clientID, bool includeObjects = false)
        {
            Entity.Client x = null;
            x = DataAccess.Client.Select(clientID);
            x.ClientConnections.ClientLiveDBConnectionString = x.ClientLiveDBConnectionString;
            x.ClientConnections.ClientTestDBConnectionString = x.ClientTestDBConnectionString;
            if (includeObjects && x != null)
            {
                ECN_SetObjects(x);
            }

            return x;
        }

        private Entity.Client ECN_SetObjects(KMPlatform.Entity.Client client)
        {
            client.ClientConfigurations = DataAccess.ClientConfigurationMap.Select(client.ClientID);

            if (client.ClientConnections == null || string.IsNullOrEmpty(client.ClientConnections.ClientLiveDBConnectionString) || string.IsNullOrEmpty(client.ClientConnections.ClientTestDBConnectionString))
            {
                client.ClientConnections = new Object.ClientConnections();
                client.ClientConnections.ClientLiveDBConnectionString = client.ClientLiveDBConnectionString;
                client.ClientConnections.ClientTestDBConnectionString = client.ClientTestDBConnectionString;
            }

            client.Services = new BusinessLogic.Service().SelectForClientID(client.ClientID, true);
            client.Products = DataAccess.Client.SelectProduct(client).ToList();

            return client;
        }




        #endregion
    }
}
