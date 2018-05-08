using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;

namespace UAD_Lookup_WS.Interface
{
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface ISubscriptionStatusMatrix
    {
        [OperationContract]
        Response<List<FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix>> Select(Guid accessKey);

        [OperationContract(Name="SelectForStatusCategoryTransaction")]
        Response<FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix> Select(Guid accessKey, int subscriptionStatusID, int categoryCodeID, int transactionCodeID);

        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix x);
    }
}
