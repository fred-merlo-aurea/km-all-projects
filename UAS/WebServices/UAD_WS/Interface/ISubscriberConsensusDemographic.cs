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
    public interface ISubscriberConsensusDemographic
    {
        [OperationContract]
        Response<List<FrameworkUAD.Object.SubscriberConsensusDemographic>> Select(Guid accessKey, int subscriptionID, KMPlatform.Object.ClientConnections client);
    }
}
