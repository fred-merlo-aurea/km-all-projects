using System;
using System.Collections.Generic;

using PlatformUser = KMPlatform.Entity.User;
using PlatformClient = KMPlatform.Entity.Client;
using EcnNotification = ECN_Framework_Entities.Accounts.Notification;

namespace EmailMarketing.Site.Infrastructure.Abstract
{
    public interface IAccountProvider
    {
        PlatformUser User_Login(string username, string password);
        PlatformUser User_GetByUserID(int userID, bool getChildren);
        //List<EcnUser> User_GetByCustomerID(int customerID);

        //PlatformClient Client_GetByClientID(int client, bool getChildren);
        List<PlatformClient> Client_GetByUserID(int userID, bool getChildren);
        List<PlatformClient> Client_GetAllByUserID(int userID, bool getChildren);
        EcnNotification Notification_GetByCurrentDateTime();
    }
}
