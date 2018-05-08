using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;

namespace UAD_WS.Interface
{
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface IMarketingMap
    {
        [OperationContract]
        Response<FrameworkUAD.Entity.MarketingMap> Select(Guid accessKey, int marketingID, int subscriberID, int productID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<List<FrameworkUAD.Entity.MarketingMap>> SelectPublication(Guid accessKey, int publicationID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<List<FrameworkUAD.Entity.MarketingMap>> SelectSubscriber(Guid accessKey, int pubSubscriptionID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> Save(Guid accessKey, FrameworkUAD.Entity.MarketingMap x, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<bool> SaveBulkUpdate(Guid accessKey, List<FrameworkUAD.Entity.MarketingMap> x, KMPlatform.Object.ClientConnections client);
    }
}
