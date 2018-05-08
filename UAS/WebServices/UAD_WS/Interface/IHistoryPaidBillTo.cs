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
    public interface IHistoryPaidBillTo
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.HistoryPaidBillTo>> SelectSubscription(Guid accessKey, int subscriptionID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<FrameworkUAD.Entity.HistoryPaidBillTo> Select(Guid accessKey, int subscriptionPaidID, KMPlatform.Object.ClientConnections client);

        [OperationContract(Name = "SavePaidBillTo")]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.PaidBillTo myPaidBillTo, int userID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.HistoryPaidBillTo x, KMPlatform.Object.ClientConnections client);
    }
}
