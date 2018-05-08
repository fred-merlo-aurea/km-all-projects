using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace KMPlatform.BusinessLogic
{
    public class Application
    {
        public List<Entity.Application> Select()
        {
            List<Entity.Application> x = null;
            x = DataAccess.Application.Select().ToList();
            foreach (Entity.Application a in x)
            {
                Menu mWorker = new Menu();
                a.Menus = mWorker.SelectForApplication(a.ApplicationID, true, true).ToList();
            }
            return x;
        }
        public List<Entity.Application> SelectForUser(int userID)
        {
            List<Entity.Application> x = null;
            x = DataAccess.Application.SelectForUser(userID).ToList();
            foreach (Entity.Application a in x)
            {
                Menu mWorker = new Menu();
                a.Menus = mWorker.SelectForApplication(a.ApplicationID, true, true).ToList();
            }
            return x;
        }
        public List<Entity.Application> SelectForSecurityGroup(int securityGroupID)
        {
            List<Entity.Application> x = null;
            x = DataAccess.Application.SelectForSecurityGroup(securityGroupID).ToList();
            foreach (Entity.Application a in x)
            {
                Menu mWorker = new Menu();
                a.Menus = mWorker.SelectForApplication(a.ApplicationID, true, true).ToList();
            }
            return x;
        }
        public List<Entity.Application> SelectForService(int serviceID)
        {
            List<Entity.Application> x = null;
            x = DataAccess.Application.SelectForService(serviceID).ToList();
            foreach (Entity.Application a in x)
            {
                Menu mWorker = new Menu();
                a.Menus = mWorker.SelectForApplication(a.ApplicationID, true, true).ToList();
            }
            return x;
        }
        public List<Entity.Application> SelectOnlyEnabledForService(int serviceID)
        {
            List<Entity.Application> x = null;
            x = DataAccess.Application.SelectOnlyEnabledForService(serviceID).ToList();
            foreach (Entity.Application a in x)
            {
                Menu mWorker = new Menu();
                a.Menus = mWorker.SelectOnlyEnabledForApplication(a.ApplicationID, true).ToList();
            }
            return x;
        }
        public List<Entity.Application> SelectOnlyEnabledForServiceWithSecurityGroup(int serviceID, int securityGroupID, bool isKMUser)
        {
            List<Entity.Application> x = null;
            x = DataAccess.Application.SelectOnlyEnabledForService(serviceID).ToList();
            foreach (Entity.Application a in x)
            {
                Menu mWorker = new Menu();
                if (isKMUser)
                    a.Menus = mWorker.SelectForApplication(a.ApplicationID, true, false);
                else
                    a.Menus = mWorker.Select(securityGroupID, a.ApplicationID, true).ToList();
            }
            return x;
        }
        public List<Entity.Application> SelectOnlyEnabledForService(int serviceID, int userID)
        {
            List<Entity.Application> x = null;
            x = DataAccess.Application.SelectOnlyEnabledForService(serviceID, userID).ToList();
            foreach (Entity.Application a in x)
            {
                Menu mWorker = new Menu();
                a.Menus = mWorker.SelectOnlyEnabledForApplication(a.ApplicationID, userID, true).ToList();
            }
            return x;
        }
        public Entity.Application SetObjects(Entity.Application app)
        {
            Menu mWorker = new Menu();

            app.Menus = mWorker.SelectForApplication(app.ApplicationID,true,true).ToList();

            return app;
        }
        public Entity.Application SetOnlyEnabledObjects(Entity.Application app)
        {
            Menu mWorker = new Menu();

            app.Menus = mWorker.SelectOnlyEnabledForApplication(app.ApplicationID, true).ToList();

            return app;
        }
        public List<KMPlatform.Entity.Application> Search(string searchValue, List<KMPlatform.Entity.Application> searchList, bool? isActive = null)
        {
            searchValue = searchValue.ToLower().Trim();
            List<KMPlatform.Entity.Application> matchList = new List<KMPlatform.Entity.Application>();

            matchList.AddRange(searchList.FindAll(x => x.ApplicationName.ToLower().Contains(searchValue)));

            if (isActive != null)
                matchList = matchList.Where(x => x.IsActive == isActive).ToList();

            return matchList.Distinct().ToList();
        }
        public int Save(Entity.Application x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                if (string.IsNullOrEmpty(x.FromEmailAddress))
                    x.FromEmailAddress = "info@TeamKM.com";
                if (string.IsNullOrEmpty(x.ErrorEmailAddress))
                    x.ErrorEmailAddress = "platform-services@TeamKM.com";

                x.ApplicationID = DataAccess.Application.Save(x);
                scope.Complete();
            }

            return x.ApplicationID;
        }
    }
}
