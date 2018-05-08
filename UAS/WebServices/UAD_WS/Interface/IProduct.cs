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
    public interface IProduct
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.Product>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client);

        [OperationContract(Name = "SelectForProduct")]
        Response<FrameworkUAD.Entity.Product> Select(Guid accessKey, int pubID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.Product x, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> Copy(Guid accessKey, int fromID, int toID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> UpdateLock(Guid accessKey, KMPlatform.Object.ClientConnections client, int userID);
    }
}
