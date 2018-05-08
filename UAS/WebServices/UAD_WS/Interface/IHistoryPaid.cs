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
    public interface IHistoryPaid
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.HistoryPaid>> Select(Guid accessKey, int subscriptionID, KMPlatform.Object.ClientConnections client);

        [OperationContract(Name = "SaveSubscriptionPaid")]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.SubscriptionPaid mySubscriptionPaid, int userID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.HistoryPaid x, KMPlatform.Object.ClientConnections client);
    }
}
