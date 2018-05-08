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
    public interface IHistoryMarketingMap
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.HistoryMarketingMap>> Select(Guid accessKey, int subscriberID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.HistoryMarketingMap x, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<List<FrameworkUAD.Entity.HistoryMarketingMap>> SaveBulkUpdate(Guid accessKey, List<FrameworkUAD.Entity.HistoryMarketingMap> list, KMPlatform.Object.ClientConnections client);
    }
}
