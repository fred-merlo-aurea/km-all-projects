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
    public interface ISubscriptionPaid
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.SubscriptionPaid>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client);

        [OperationContract(Name="SelectForSubscription")]
        Response<FrameworkUAD.Entity.SubscriptionPaid> Select(Guid accessKey, int subscriptionID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.SubscriptionPaid x, KMPlatform.Object.ClientConnections client);
    }
}
