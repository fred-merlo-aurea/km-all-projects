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
    public interface IBatchHistoryDetail
    {
        [OperationContract(Name = "SelectForUser")]
        Response<List<FrameworkUAD.Object.BatchHistoryDetail>> Select(Guid accessKey, int userID, bool isActive, KMPlatform.Object.ClientConnections client, string clientName);

        [OperationContract]
        Response<List<FrameworkUAD.Object.BatchHistoryDetail>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client, string clientName);

        [OperationContract(Name = "SelectForSubscription")]
        Response<List<FrameworkUAD.Object.BatchHistoryDetail>> Select(Guid accessKey, int SubscriptionID, KMPlatform.Object.ClientConnections client, string clientName);

        [OperationContract]
        Response<List<FrameworkUAD.Object.BatchHistoryDetail>> SelectForSubscriber(Guid accessKey, int subscriberID, KMPlatform.Object.ClientConnections client, string clientName);

        [OperationContract(Name = "SelectForBatch")]
        Response<List<FrameworkUAD.Object.BatchHistoryDetail>> SelectBatch(Guid accessKey, int BatchID, string Name, int SequenceID, KMPlatform.Object.ClientConnections client, string clientName);

        [OperationContract(Name = "SelectForBatchDate")]
        Response<List<FrameworkUAD.Object.BatchHistoryDetail>> SelectBatch(Guid accessKey, int BatchID, string Name, int SequenceID, DateTime From, DateTime To, KMPlatform.Object.ClientConnections client, string clientName);


    }
}
