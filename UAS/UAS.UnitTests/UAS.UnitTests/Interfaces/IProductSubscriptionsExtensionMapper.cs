using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using KMPlatform.Object;

using FrameworkUADEntity = FrameworkUAD.Entity;

namespace UAS.UnitTests.Interfaces
{
    public interface IProductSubscriptionsExtensionMapper
    {
        void Delete(int subscriptionsExtensionMapperID, int pubID, ClientConnections client);
        bool ExistsByCustomField(string customField, ClientConnections client);
        bool ExistsByIDCustomField(int pubSubscriptionsExtensionMapperID, int pubID, string customField, ClientConnections client);
        int Save(FrameworkUADEntity.ProductSubscriptionsExtensionMapper x, ClientConnections client);
        List<FrameworkUADEntity.ProductSubscriptionsExtensionMapper> SelectAll(ClientConnections client);
        FrameworkUADEntity.ProductSubscriptionsExtensionMapper SelectByID(int pubSubscriptionsExtensionMapperID, ClientConnections client);
        DataSet SelectBySearch(int pubID, string name, string searchCriteria, int currentPage, int pageSize, string sortDirection, string sortColumn, ClientConnections client);
        NameValueCollection ValidationForDeleteorInActive(int pubSubscriptionsExtensionMapperID, ClientConnections client);
    }
}