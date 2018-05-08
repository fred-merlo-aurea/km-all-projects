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
    public interface IPubSubscriptionDetail
    {
        [OperationContract]
        Response<bool> DeleteCodeSheetID(Guid accessKey, int codeSheetID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<List<FrameworkUAD.Entity.ProductSubscriptionDetail>> Select(Guid accessKey, int PubSubscriptionID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<List<FrameworkUAD.Entity.ProductSubscriptionDetail>> SaveBulkUpdate(Guid accessKey, KMPlatform.Object.ClientConnections client, List<FrameworkUAD.Entity.ProductSubscriptionDetail> list);
        [OperationContract]
        Response<int> SelectCount(Guid accessKey, int productID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<List<FrameworkUAD.Entity.ProductSubscriptionDetail>> SelectPaging(Guid accessKey, int page, int pageSize, int productID, KMPlatform.Object.ClientConnections client);
    }
}
