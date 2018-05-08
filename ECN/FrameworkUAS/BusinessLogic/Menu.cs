using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace KMPlatform.BusinessLogic
{
    public class Menu
    {
        public  List<Entity.Menu> Select(bool includeFeatures = false)
        {
            List<Entity.Menu> x = null;
            x = DataAccess.Menu.Select().ToList();

            if (includeFeatures == true)
            {
                x = SetMenuFeatures(x);
            }
            
            return x;
        }
        public  List<Entity.Menu> Select(int securityGroupID, bool hasAccess, bool includeFeatures = false)
        {
            List<Entity.Menu> x = null;
            x = DataAccess.Menu.Select(securityGroupID, hasAccess).ToList();

            if (includeFeatures == true)
            {
                x = SetMenuFeatures(x);
            }

            return x;
        }
        public  List<Entity.Menu> Select(int securityGroupID, int applicationID, bool includeFeatures = false)
        {
            List<Entity.Menu> x = null;
            x = DataAccess.Menu.SelectForApplication(applicationID, securityGroupID);

            if (includeFeatures == true)
            {
                x = SetMenuFeatures(x);
            }

            return x;
        }
        public List<Entity.Menu> SelectForApplication(int applicationID, bool isActive, bool includeFeatures = false)
        {
            List<Entity.Menu> x = null;
            x = DataAccess.Menu.SelectForApplication(applicationID, isActive).ToList();

            if (includeFeatures == true)
            {
                x = SetMenuFeatures(x);
            }

            return x;
        }
        public List<Entity.Menu> SelectForApplication(int applicationID, int userID, bool isActive, bool includeFeatures = false)
        {
            List<Entity.Menu> x = null;
            x = DataAccess.Menu.SelectForApplication(applicationID, userID, isActive).ToList();
            x = x.Where(y => y.IsActive == isActive).ToList();

            if (includeFeatures == true)
            {
                x = SetMenuFeatures(x);
            }

            return x;
        }
        public List<Entity.Menu> SelectOnlyEnabledForApplication(int applicationID, bool includeFeatures = false)
        {
            List<Entity.Menu> x = null;
            x = DataAccess.Menu.SelectForApplication(applicationID, true).ToList();

            if (includeFeatures == true)
            {
                x = SetOnlyEnabledMenuFeatures(x);
            }

            return x;
        }
        public List<Entity.Menu> SelectOnlyEnabledForApplication(int applicationID, int userID, bool includeFeatures = false)
        {
            List<Entity.Menu> x = null;
            x = DataAccess.Menu.SelectForApplication(applicationID, userID, true).ToList();

            if (includeFeatures == true)
            {
                x = SetOnlyEnabledMenuFeatures(x);
            }

            return x;
        }
        public List<Entity.Menu> SelectForApplicationAndUser(string applicationName, int userID,int clientID, int securityGroupID, bool isActive, bool hasAccess, bool includeFeatures = false)
        {
            List<Entity.Menu> x = null;
            x = DataAccess.Menu.SelectForApplicationAndUser(applicationName,userID,clientID, securityGroupID, isActive, hasAccess).ToList();

            if (includeFeatures == true)
            {
                x = SetMenuFeatures(x);
            }

            return x;
        }
        public List<Entity.Menu> SelectForUser(int userID, int securityGroupID, bool isActive, bool hasAccess, bool includeFeatures = false)
        {
            List<Entity.Menu> x = null;
            x = DataAccess.Menu.SelectForUser(userID, securityGroupID, isActive, hasAccess).ToList();

            if (includeFeatures == true)
            {
                x = SetMenuFeatures(x);
            }

            return x;
        }
        private  List<Entity.Menu> SetMenuFeatures(List<Entity.Menu> menu)
        {
            foreach (Entity.Menu m in menu)
            {
                //MenuFeature mf = new MenuFeature();
                //m.MenuFeatures = mf.Select(m.MenuID).ToList();
            }
            return menu;
        }
        private List<Entity.Menu> SetOnlyEnabledMenuFeatures(List<Entity.Menu> menu)
        {
            foreach (Entity.Menu m in menu)
            {
                //MenuFeature mf = new MenuFeature();
                //m.MenuFeatures = mf.SelectOnlyEnabled(m.MenuID).ToList();
            }
            return menu;
        }
        public  int Save(Entity.Menu x)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.MenuID = DataAccess.Menu.Save(x);
                scope.Complete();
            }

            return x.MenuID;
        }
    }
}
