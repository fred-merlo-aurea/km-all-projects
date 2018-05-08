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
    public interface IMasterGroup
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.MasterGroup>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.MasterGroup x, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> Delete(Guid accessKey, int masterGroupID, KMPlatform.Object.ClientConnections client);
    }
}
