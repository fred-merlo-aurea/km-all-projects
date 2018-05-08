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
    public interface IProductAudit
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.ProductAudit>> Select(Guid accessKey, int productId, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> Save(Guid accessKey, FrameworkUAD.Entity.ProductAudit x, KMPlatform.Object.ClientConnections client);
    }
}
