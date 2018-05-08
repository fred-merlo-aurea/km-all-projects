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
    public interface IFilter
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.Filter>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<bool> Delete(Guid accessKey, int filterID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<int> Save(Guid accessKey, KMPlatform.Object.ClientConnections client, FrameworkUAD.Entity.Filter x);
    }
}
