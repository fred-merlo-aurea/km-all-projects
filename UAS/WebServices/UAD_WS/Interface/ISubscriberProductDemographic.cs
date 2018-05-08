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
    public interface ISubscriberProductDemographic
    {
        [OperationContract]
        Response<List<FrameworkUAD.Object.SubscriberProductDemographic>> Select(Guid accessKey, int subscriptionID, string pubCode, KMPlatform.Object.ClientConnections client);
    }
}
