using FrameworkUAS.Service;
using System;
using System.Linq;
using System.ServiceModel;

namespace UAD_WS.Interface
{
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface IActionBackUp
    {
        [OperationContract]
        Response<bool> Restore(Guid accessKey, int productID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<bool> Bulk_Insert(Guid accessKey, int productID, KMPlatform.Object.ClientConnections client);
    }
}
