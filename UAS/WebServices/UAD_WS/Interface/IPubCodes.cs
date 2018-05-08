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
    public interface IPubCodes
    {
        [OperationContract]
        Response<List<FrameworkUAD.Object.PubCode>> Select(Guid accessKey, string dbName);

        [OperationContract]
        Response<List<FrameworkUAD.Object.PubCode>> SelectAllPubs(Guid accessKey, KMPlatform.Object.ClientConnections client, string dbName);
    }
}
