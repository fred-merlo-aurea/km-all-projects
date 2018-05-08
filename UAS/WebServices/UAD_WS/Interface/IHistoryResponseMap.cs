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
    public interface IHistoryResponseMap
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.HistoryResponseMap>> Select(Guid accessKey, int pubSubscriptionID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.HistoryResponseMap x, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<List<FrameworkUAD.Entity.HistoryResponseMap>> SaveBulkUpdate(Guid accessKey, List<FrameworkUAD.Entity.HistoryResponseMap> list, KMPlatform.Object.ClientConnections client);
    }
}
