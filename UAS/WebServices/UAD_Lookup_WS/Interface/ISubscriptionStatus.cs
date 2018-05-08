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
    public interface ISubscriptionStatus
    {
        [OperationContract]
        Response<List<FrameworkUAD_Lookup.Entity.SubscriptionStatus>> Select(Guid accessKey);

        [OperationContract(Name="SelectForCategoryCodeTransactionCode")]
        Response<FrameworkUAD_Lookup.Entity.SubscriptionStatus> Select(Guid accessKey, int categoryCodeID, int transactionCodeID);

        [OperationContract(Name = "SelectForSubscriptionStatus")]
        Response<FrameworkUAD_Lookup.Entity.SubscriptionStatus> Select(Guid accessKey, int subscriptionStatusID);

        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD_Lookup.Entity.SubscriptionStatus x);
    }
}
