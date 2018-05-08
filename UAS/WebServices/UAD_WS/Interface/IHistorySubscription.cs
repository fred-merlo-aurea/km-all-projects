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
    public interface IHistorySubscription
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.HistorySubscription>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.HistorySubscription x, KMPlatform.Object.ClientConnections client);

        [OperationContract(Name="SaveForSubscriber")]
        Response<int> SaveForSubscriber(Guid accessKey, FrameworkUAD.Entity.ProductSubscription productSubscription, int userID, KMPlatform.Object.ClientConnections client);

    }
}
