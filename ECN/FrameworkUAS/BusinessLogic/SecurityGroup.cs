using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace KMPlatform.BusinessLogic
{
    public class SecurityGroup
    {
        #region AMS
        public List<Entity.SecurityGroup> AMS_Select(bool includeServices = true)
        {
            List<Entity.SecurityGroup> x = null;
            x = DataAccess.SecurityGroup.Select().ToList();

            if (includeServices == true)
            {
                foreach (Entity.SecurityGroup sg in x)
                    sg.Services = AMS_SetAuthorizedSecurityGroup(sg.SecurityGroupID);
            }
            return x;
        }
        public List<Entity.SecurityGroup> AMS_SelectForClientGroup(int clientGroupID, bool includeServices = true)
        {
            List<Entity.SecurityGroup> x = null;
            x = DataAccess.SecurityGroup.SelectForClientGroup(clientGroupID).ToList();

            if (includeServices == true)
            {
                foreach (Entity.SecurityGroup sg in x)
                    sg.Services = AMS_SetAuthorizedClientGroup(sg.SecurityGroupID, clientGroupID);
            }
            return x;
        }
        public List<Entity.SecurityGroup> AMS_SelectActiveForClientGroup(int clientGroupID, bool includeServices = true)
        {
            List<Entity.SecurityGroup> x = null;
            x = DataAccess.SecurityGroup.SelectActiveForClientGroup(clientGroupID).ToList();

            if (includeServices == true)
            {
                foreach (Entity.SecurityGroup sg in x)
                    sg.Services = AMS_SetAuthorizedClientGroup(sg.SecurityGroupID, clientGroupID);
            }
            return x;
        }
        public Entity.SecurityGroup AMS_Select(int userID, int clientID, bool includeServices = true)
        {
            Entity.SecurityGroup x = null;
            x = DataAccess.SecurityGroup.Select(userID, clientID);

            if (includeServices == true)
            {
                x.Services = AMS_SetAuthorizedUser(x.SecurityGroupID, userID);
            }
            return x;
        }
        private List<Entity.Service> AMS_SetAuthorizedUser(int securityGroupID, int userID)
        {
            Service sWorker = new Service();
            return sWorker.AMS_SelectForSecurityGroupAndUserID(securityGroupID, userID, true);
        }
        private List<Entity.Service> AMS_SetAuthorizedSecurityGroup(int securityGroupID)
        {
            Service sWorker = new Service();
            return sWorker.AMS_SelectForSecurityGroup(securityGroupID, true);
        }
        private List<Entity.Service> AMS_SetAuthorizedClientGroup(int securityGroupID, int clientGroupID)
        {
            Service sWorker = new Service();
            return sWorker.AMS_SelectForSecurityGroupAndClientGroupID(securityGroupID, clientGroupID, true);
        }
        #endregion

        public List<Entity.SecurityGroup> SelectForClientGroup(int clientGroupID, bool includeServices = true)
        {
            List<Entity.SecurityGroup> x = null;
            x = DataAccess.SecurityGroup.SelectForClientGroup(clientGroupID).ToList();

            if (includeServices == true)
            {
                FetchServicesForClientGroup(x, clientGroupID);
            }
            return x;
        }

        public List<Entity.SecurityGroup> SelectForClientGroup(int clientGroupID, KMPlatform.Enums.SecurityGroupAdministrativeLevel administrativeLevel, bool includeServices = true)
        {
            List<Entity.SecurityGroup> x = null;
            x = DataAccess.SecurityGroup.SelectForClientGroup(clientGroupID, administrativeLevel).ToList();
            if (x != null && includeServices == true)
            {
                FetchServicesForClientGroup(x, clientGroupID);
            }
            return x;
        }

        public List<Entity.SecurityGroup> SelectForClient(int clientID, bool includeServices = true)
        {
            List<Entity.SecurityGroup> x = null;
            x = DataAccess.SecurityGroup.SelectForClient(clientID).ToList();
            if (x != null && includeServices == true)
            {
                FetchServicesForClient(x, clientID);
            }
            return x;
        }

        public List<Entity.SecurityGroup> SelectForClient(int clientID, KMPlatform.Enums.SecurityGroupAdministrativeLevel administrativeLevel, bool includeServices = true)
        {
            List<Entity.SecurityGroup> x = null;
            x = DataAccess.SecurityGroup.SelectForClient(clientID, administrativeLevel).ToList();
            if (x != null && includeServices == true)
            {
                FetchServicesForClient(x, clientID);
            }
            return x;
        }

        void FetchServicesForClientGroup(List<Entity.SecurityGroup> x, int clientGroupID)
        {
            foreach (Entity.SecurityGroup sg in x)
            {
                sg.Services = (new Service()).SelectForClientGroupID(clientGroupID);

                //Dictionary<Entity.SecurityGroup.PermissionKey, List<string>> Permissions = new Dictionary<Entity.SecurityGroup.PermissionKey, List<string>>();

                //foreach (var p in SecurityGroupPermission.GetPermissions(sg.SecurityGroupID))
                //{
                //    Entity.SecurityGroup.PermissionKey k = new Entity.SecurityGroup.PermissionKey { ServiceCode = p.ServiceCode, ServiceFeatureCode = p.SFCode };
                //    if (false == Permissions.ContainsKey(k))
                //    {
                //        Permissions.Add(k, new List<string>());
                //    }
                //    Permissions[k].Add(p.AccessCode);
                //}
                //sg.Permissions = Permissions;

                Dictionary<Tuple<KMPlatform.Enums.Services, KMPlatform.Enums.ServiceFeatures>, List<KMPlatform.Enums.Access>> Permissions = new Dictionary<Tuple<KMPlatform.Enums.Services, KMPlatform.Enums.ServiceFeatures>, List<KMPlatform.Enums.Access>>();

                foreach (var p in SecurityGroupPermission.GetPermissions(sg.SecurityGroupID))
                {
                    try
                    {
                        Tuple<KMPlatform.Enums.Services, KMPlatform.Enums.ServiceFeatures> k = Tuple.Create((KMPlatform.Enums.Services)Enum.Parse(typeof(KMPlatform.Enums.Services), p.ServiceCode), (KMPlatform.Enums.ServiceFeatures)Enum.Parse(typeof(KMPlatform.Enums.ServiceFeatures), p.SFCode));
                        if (false == Permissions.ContainsKey(k))
                        {
                            Permissions.Add(k, new List<KMPlatform.Enums.Access>());
                        }
                        Permissions[k].Add((KMPlatform.Enums.Access)Enum.Parse(typeof(KMPlatform.Enums.Access), p.AccessCode));
                    }
                    catch { }
                }
                sg.Permissions = Permissions;
            }
        }

        void FetchServicesForClient(List<Entity.SecurityGroup> x, int clientID)
        {
            foreach (Entity.SecurityGroup sg in x)
            {
                sg.Services = (new Service()).SelectForClientID(clientID);

                Dictionary<Tuple<KMPlatform.Enums.Services, KMPlatform.Enums.ServiceFeatures>, List<KMPlatform.Enums.Access>> Permissions = new Dictionary<Tuple<KMPlatform.Enums.Services, KMPlatform.Enums.ServiceFeatures>, List<KMPlatform.Enums.Access>>();

                foreach (var p in SecurityGroupPermission.GetPermissions(sg.SecurityGroupID))
                {
                    try
                    {
                        Tuple<KMPlatform.Enums.Services, KMPlatform.Enums.ServiceFeatures> k = Tuple.Create((KMPlatform.Enums.Services)Enum.Parse(typeof(KMPlatform.Enums.Services), p.ServiceCode), (KMPlatform.Enums.ServiceFeatures)Enum.Parse(typeof(KMPlatform.Enums.ServiceFeatures), p.SFCode));
                        if (false == Permissions.ContainsKey(k))
                        {
                            Permissions.Add(k, new List<KMPlatform.Enums.Access>());
                        }
                        Permissions[k].Add((KMPlatform.Enums.Access)Enum.Parse(typeof(KMPlatform.Enums.Access), p.AccessCode));
                    }
                    catch { }
                }
                sg.Permissions = Permissions;
            }
        }

        public Entity.SecurityGroup Select(int userID, int clientID, bool isKMUser, bool includeServices = true)
        {
            Entity.SecurityGroup x = null;
            x = DataAccess.SecurityGroup.Select(userID, clientID);

            if (x == null)
                return x;

            if (includeServices == true)
            {
                PopulateServices(x, isKMUser);
            }
            return x;
        }

        public Entity.SecurityGroup Select(int securityGroupID, bool isKMUser, bool includeServices = true)
        {
            Entity.SecurityGroup x = DataAccess.SecurityGroup.Select(securityGroupID);
            if (x != null && includeServices) PopulateServices(x, isKMUser);
            return x;
        }

        public List<Entity.SecurityGroup> SelectActiveForClientGroup(int clientGroupID, bool includeServices = true)
        {
            List<Entity.SecurityGroup> x = null;
            x = DataAccess.SecurityGroup.SelectActiveForClientGroup(clientGroupID).ToList();

            if (includeServices == true)
            {
                foreach (Entity.SecurityGroup sg in x)
                    sg.Services = SetAuthorizedClientGroup(sg.SecurityGroupID, clientGroupID);
            }
            return x;
        }

        //void AMS_PopulateServices(Entity.SecurityGroup x)
        //{
        //    BusinessLogic.Menu menuW = new Menu();
        //    x.Services = (new Service()).SelectForSecurityGroupID(x.SecurityGroupID, false);
        //    foreach (Entity.Service s in x.Services)
        //    {
        //        s.Applications = DataAccess.Application.SelectForService(s.ServiceID);                
        //        foreach (Entity.Application a in s.Applications)
        //        {
        //            //a.Menus = DataAccess.Menu.SelectForApplication(a.ApplicationID, true);
        //            a.Menus = menuW.SelectForApplication(a.ApplicationID, true, true);
        //        }
        //    }
        //}

        void PopulateServices(Entity.SecurityGroup x, bool isKMUser)
        {
            x.Services = (new Service()).SelectForSecurityGroupID(x.SecurityGroupID, false);
            foreach (Entity.Service s in x.Services)
            {
                //s.Applications = DataAccess.Application.SelectForService(s.ServiceID);
                s.Applications = (new BusinessLogic.Application()).SelectOnlyEnabledForServiceWithSecurityGroup(s.ServiceID, x.SecurityGroupID, isKMUser);
            }
            Dictionary<Tuple<KMPlatform.Enums.Services, KMPlatform.Enums.ServiceFeatures>, List<KMPlatform.Enums.Access>> Permissions = new Dictionary<Tuple<KMPlatform.Enums.Services, KMPlatform.Enums.ServiceFeatures>, List<KMPlatform.Enums.Access>>();

            foreach (var p in SecurityGroupPermission.GetPermissions(x.SecurityGroupID))
            {
                try
                {
                    Tuple<KMPlatform.Enums.Services, KMPlatform.Enums.ServiceFeatures> k = Tuple.Create((KMPlatform.Enums.Services)Enum.Parse(typeof(KMPlatform.Enums.Services), p.ServiceCode), (KMPlatform.Enums.ServiceFeatures)Enum.Parse(typeof(KMPlatform.Enums.ServiceFeatures), p.SFCode));
                    if (false == Permissions.ContainsKey(k))
                    {
                        Permissions.Add(k, new List<KMPlatform.Enums.Access>());
                    }
                    Permissions[k].Add((KMPlatform.Enums.Access)Enum.Parse(typeof(KMPlatform.Enums.Access), p.AccessCode));
                }
                catch { }
            }
            x.Permissions = Permissions;
        }

        private List<Entity.Service> SetAuthorizedClientGroup(int securityGroupID, int clientGroupID)
        {
            Service sWorker = new Service();
            return sWorker.SelectForSecurityGroupAndClientGroupID(securityGroupID, clientGroupID, true);
        }

        private List<Entity.Service> SetAuthorizedUser(int securityGroupID, int userID)
        {
            Service sWorker = new Service();
            return sWorker.SelectForSecurityGroupAndUserID(securityGroupID, userID, true);
        }
        private List<Entity.Service> SetAuthorizedSecurityGroup(int securityGroupID)
        {
            Service sWorker = new Service();
            return sWorker.SelectForSecurityGroup(securityGroupID, true);
        }

        public bool SecurityGroupNameExists(string name, int clientGroupID)
        {
            return SelectForClientGroup(clientGroupID).Exists(x => x.SecurityGroupName.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        public int CreateFromTemplateForClientGroup(string securityGroupTemplateName, int clientGroupID, string administrativeLevel, Entity.User user)
        {
            int rv = -1;
            using (TransactionScope scope = new TransactionScope())
            {
                rv = DataAccess.SecurityGroup.CreateFromTemplateForClientGroup(securityGroupTemplateName, clientGroupID, administrativeLevel, user);
                scope.Complete();
            }
            return rv;
        }

        public int CreateFromTemplateForClient(string securityGroupTemplateName, int clientGroupID, int clientID, string administrativeLevel, Entity.User user)
        {
            int rv = -1;
            using (TransactionScope scope = new TransactionScope())
            {
                rv = DataAccess.SecurityGroup.CreateFromTemplateForClient(securityGroupTemplateName, clientGroupID, clientID, administrativeLevel, user);
                scope.Complete();
            }
            return rv;
        }

        public void UpdateAdministrators(int ClientID, int ClientGroupID, int UserID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                DataAccess.SecurityGroup.UpdateAdministrators(ClientID, ClientGroupID, UserID);
                scope.Complete();
            }
        }

        /// <summary>
        /// For ECN
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public int Save(Entity.SecurityGroup x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.SecurityGroupID = DataAccess.SecurityGroup.Save(x);
                scope.Complete();
            }

            return x.SecurityGroupID;
        }



        public bool ExistsForClient_ClientGroup(string name, int clientgroupID, int clientID, int securityGroupID)
        {
            bool exists = false;
            using(TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = DataAccess.SecurityGroup.ExistsByClient_ClientGroup(name, clientgroupID, clientID,securityGroupID);
                scope.Complete();
            }
            return exists;
        }
    }
}
