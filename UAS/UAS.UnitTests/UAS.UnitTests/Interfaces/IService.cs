using System.Collections.Generic;

namespace UAS.UnitTests.Interfaces
{
    public interface IService
    {
        List<KMPlatform.Entity.Service> AMS_SelectForSecurityGroup(int securityGroupID, bool includeObjects = false);
        List<KMPlatform.Entity.Service> AMS_SelectForSecurityGroupAndClientGroupID(int securityGroupID, int clientGroupID, bool includeObjects = false);
        List<KMPlatform.Entity.Service> AMS_SelectForSecurityGroupAndUserID(int securityGroupID, int userID, bool includeObjects = false);
        int Save(KMPlatform.Entity.Service x);
        List<KMPlatform.Entity.Service> Select(bool includeObjects = false);
        KMPlatform.Entity.Service Select(KMPlatform.Enums.Services service, bool includeObjects = false);
        KMPlatform.Entity.Service Select(int serviceID, bool includeObjects = false);
        List<KMPlatform.Entity.Service> SelectForClientGroupID(int clientGroupID, bool includeObjects = false);
        List<KMPlatform.Entity.Service> SelectForClientID(int clientID, bool includeObjects = false);
        List<KMPlatform.Entity.Service> SelectForSecurityGroup(int securityGroupID, bool includeObjects = false);
        List<KMPlatform.Entity.Service> SelectForSecurityGroupAndClientGroupID(int securityGroupID, int clientGroupID, bool isKMUser, bool includeObjects = false);
        List<KMPlatform.Entity.Service> SelectForSecurityGroupAndUserID(int securityGroupID, int userID, bool includeObjects = false);
        List<KMPlatform.Entity.Service> SelectForSecurityGroupID(int securityGroupID, bool includeObjects = false);
        List<KMPlatform.Entity.Service> SelectForUser(int userID, bool includeObjects = false);
        List<KMPlatform.Entity.Service> SelectForUserAuthorization(int userID);
        KMPlatform.Entity.Service SetObjects(KMPlatform.Entity.Service service);
        KMPlatform.Entity.Service SetObjectsForUserAuthorization(KMPlatform.Entity.Service service);
    }
}