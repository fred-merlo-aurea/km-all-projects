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
    public interface IFinalizeBatch
    {
        [OperationContract]
        Response<List<FrameworkUAD.Object.FinalizeBatch>> Select(Guid accessKey, int userID, KMPlatform.Object.ClientConnections client, int clientId, string clientName);

        [OperationContract]
        Response<List<FrameworkUAD.Object.FinalizeBatch>> SelectBatch(Guid accessKey, int userID, KMPlatform.Object.ClientConnections client, int clientId, string clientName);

        [OperationContract]
        Response<List<FrameworkUAD.Object.FinalizeBatch>> SelectBatchUserName(Guid accessKey, int userID, KMPlatform.Object.ClientConnections client, int clientId, string clientName);
    }
}
