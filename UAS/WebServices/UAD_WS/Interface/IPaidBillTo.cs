using System;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;

namespace UAD_WS.Interface
{
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface IPaidBillTo
    {
        [OperationContract]
        Response<FrameworkUAD.Entity.PaidBillTo> SelectSubscription(Guid accessKey, int subscriptionID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<FrameworkUAD.Entity.PaidBillTo> Select(Guid accessKey, int subscriptionPaidID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.PaidBillTo x, KMPlatform.Object.ClientConnections client);
    }
}
