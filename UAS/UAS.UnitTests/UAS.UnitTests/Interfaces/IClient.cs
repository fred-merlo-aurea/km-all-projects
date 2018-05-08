using System;
using System.Collections.Generic;
using KMPlatform.Object;

namespace UAS.UnitTests.Interfaces
{
    public interface IClient
    {
        List<KMPlatform.Entity.Client> AMS_SelectPaid(bool includeObjects = false);
        void Delete(int clientID);
        KMPlatform.Entity.Client ECN_Select(int clientID, bool includeObjects = false);
        List<KMPlatform.Entity.Client> ECN_SelectAllByUserID(int userID, bool includeObjects = false);
        bool Exists(string clientName);
        bool HasFulfillmentService(int clientID, int clientGroupID = 1);
        bool HasFulfillmentService(KMPlatform.Entity.Client client, int clientGroupID = 1);
        bool HasService(int clientID, KMPlatform.Enums.Services service, int clientGroupID = 1);
        bool HasService(KMPlatform.Entity.Client client, KMPlatform.Enums.Services service, int clientGroupID = 1);
        int Save(KMPlatform.Entity.Client x, bool UpdateUsersForSecurityGroups = false, int? clientGroupID = default(int?));
        List<KMPlatform.Entity.Client> Search(string searchValue, List<KMPlatform.Entity.Client> searchList, bool? isActive = default(bool?));
        List<KMPlatform.Entity.Client> Select(bool includeObjects = false);
        KMPlatform.Entity.Client Select(string clientName, bool includeObjects = false);
        KMPlatform.Entity.Client Select(int clientID, bool includeObjects = false);
        List<KMPlatform.Entity.Client> SelectActiveForClientGroup(int clientGroupID);
        List<KMPlatform.Entity.Client> SelectActiveForClientGroupLite(int clientGroupID);
        List<KMPlatform.Entity.Client> SelectAMS(bool isAms = true, bool isActive = true, bool includeObjects = false);
        List<KMPlatform.Entity.Client> SelectbyUserID(int userID, bool includeObjects = false);
        List<KMPlatform.Entity.Client> SelectbyUserIDclientgroupID(int userID, int clientgroupID, bool includeObjects = false);
        KMPlatform.Entity.Client SelectDefault(Guid accessKey, bool includeObjects = false);
        List<KMPlatform.Entity.Client> SelectForAccessKey(Guid accessKey, bool includeObjects = false);
        List<KMPlatform.Entity.Client> SelectForAtLeastCustAdmin(int clientGroupID, int userID);
        List<KMPlatform.Entity.Client> SelectForClientGroup(int clientGroupID, bool includeObjects = false);
        List<KMPlatform.Entity.Client> SelectForClientGroupLite(int clientGroupID, bool includeObjects = false);
        KMPlatform.Entity.Client SelectFtpFolder(string ftpFolder, bool includeObjects = false);
        List<Product> SelectProducts(KMPlatform.Entity.Client client);
        bool UseUADSuppressionFeature(int clientId);
    }
}