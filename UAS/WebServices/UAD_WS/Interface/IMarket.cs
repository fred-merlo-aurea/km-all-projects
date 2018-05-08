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
    public interface IMarket
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.Market>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<int> Save(Guid accessKey, KMPlatform.Object.ClientConnections client, FrameworkUAD.Entity.Market x);
    }
}
