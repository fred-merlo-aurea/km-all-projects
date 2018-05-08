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
    public interface IBatch
    {
        [OperationContract]
        Response<FrameworkUAD.Entity.Batch> StartNewBatch(Guid accessKey, int userID, int publicationID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<List<FrameworkUAD.Entity.Batch>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client);

        [OperationContract(Name = "SelectForUser")]
        Response<List<FrameworkUAD.Entity.Batch>> Select(Guid accessKey, int userID, bool isActive, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> BatchCheck(Guid accessKey, FrameworkUAD.Entity.Batch batch);

        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.Batch x, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> CloseBatches(Guid accessKey, int userID, KMPlatform.Object.ClientConnections client);
    }
}
