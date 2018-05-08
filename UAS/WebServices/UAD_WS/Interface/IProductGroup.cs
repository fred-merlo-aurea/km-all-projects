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
    public interface IProductGroup
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.ProductGroup>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.ProductGroup x, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> Delete(Guid accessKey, int pubID, KMPlatform.Object.ClientConnections client);
    }
}
