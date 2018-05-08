using FrameworkUAS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace UAD_WS.Interface
{
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface ISubscriberAddKill
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.SubscriberAddKill>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.SubscriberAddKill subAddKill, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<int> UpdateSubscription(Guid accessKey, int addKillID, int productID, string subscriptionIDs, bool deleteAddRemoveID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<bool> BulkInsertDetails(Guid accessKey, List<FrameworkUAD.Entity.SubscriberAddKillDetail> subs, int addRemoveID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<bool> ClearDetails(Guid accessKey, int productID, KMPlatform.Object.ClientConnections client);
    }
}
