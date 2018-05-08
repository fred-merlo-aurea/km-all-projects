using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using EmailMarketing.Site.Infrastructure.Abstract;

using PlatformUser = KMPlatform.Entity.User;
using PlatformClient = KMPlatform.Entity.Client;
using PlatformUserManager = KMPlatform.BusinessLogic.User;
using PlatformClientManager = KMPlatform.BusinessLogic.Client;

using EcnNotification = ECN_Framework_Entities.Accounts.Notification;
using EcnNotificationManager = ECN_Framework_BusinessLayer.Accounts.Notification;


namespace EmailMarketing.Site.Infrastructure.Concrete
{
    /// <summary>
    /// Testability encapsulation for BusinessLayer Account access
    /// </summary>
    public class AccountFrameworkEntitiesProvider : IAccountProvider
    {
        public PlatformUser User_Login(string username, string password)
        {
            PlatformUser user = new PlatformUserManager().LogIn(username,password, true);
            if(user != null)
            {
                return user;
            }
            else
            {
                user = null;
            }
            return user;
        }
        public PlatformUser User_GetByUserID(int userID, bool getChildren)
        {
            return new PlatformUserManager().SelectUser(userID, getChildren);
        }

        //public List<PlatformUser> User_GetByClientID(int clientID)
        //{
        //    return new PlatformUserManager().Select(.GetByCustomerID(customerID);
        //}

        public List<PlatformClient> Client_GetByUserID(int userID, bool getChildren)
        {
            return new PlatformClientManager().SelectbyUserID(userID, getChildren);            
        }

        public List<PlatformClient> Client_GetAllByUserID(int userID, bool getChildren)
        {
            return new PlatformClientManager().ECN_SelectAllByUserID(userID, getChildren);
        }

        public EcnNotification Notification_GetByCurrentDateTime()
        {
            return EcnNotificationManager.GetByCurrentDateTime();
        }
    }
}