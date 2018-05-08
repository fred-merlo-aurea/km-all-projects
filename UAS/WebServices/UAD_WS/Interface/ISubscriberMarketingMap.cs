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
    public interface ISubscriberMarketingMap
    {
        [OperationContract]
        Response<List<FrameworkUAD.Object.SubscriberMarketingMap>> Select(Guid accessKey, int subscriberID, KMPlatform.Object.ClientConnections client);
    }
}
