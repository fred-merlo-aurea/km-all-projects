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
    public interface ISubscriptionSearchResult
    {
        [OperationContract]
        Response<List<FrameworkUAD.Object.SubscriptionSearchResult>> Select(Guid accessKey, int subscriberID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<List<FrameworkUAD.Object.SubscriptionSearchResult>> SelectMultiple(Guid accessKey, List<int> subscriberIDs, KMPlatform.Object.ClientConnections client);
    }
}
