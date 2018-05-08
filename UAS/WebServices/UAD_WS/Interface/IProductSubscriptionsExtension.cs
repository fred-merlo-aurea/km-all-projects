using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;

namespace UAD_WS.Interface
{
    [ServiceContract]
    public interface IProductSubscriptionsExtension
    {
        [OperationContract]
        Response<bool> Save(Guid accessKey, List<FrameworkUAD.Object.PubSubscriptionAdHoc> x, int pubSubscriptionID, int pubID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<List<FrameworkUAD.Entity.ProductSubscriptionsExtensionMapper>> SelectAll(Guid accessKey, KMPlatform.Object.ClientConnections client);
    }
}
