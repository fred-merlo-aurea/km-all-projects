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
    public interface IIssueArchiveProductSubscriptionDetail
    {
        [OperationContract]
        Response<bool> SaveBulkSqlInsert(Guid accessKey, List<FrameworkUAD.Entity.IssueArchiveProductSubscriptionDetail> list, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<bool> Save(Guid accessKey, FrameworkUAD.Entity.IssueArchiveProductSubscriptionDetail x, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<List<FrameworkUAD.Entity.IssueArchiveProductSubscriptionDetail>> SelectForUpdate(Guid accessKey, int productID, int issueid, List<int> subs, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<List<FrameworkUAD.Entity.IssueArchiveProductSubscriptionDetail>> SaveBulkUpdate(Guid accessKey, KMPlatform.Object.ClientConnections client, List<FrameworkUAD.Entity.IssueArchiveProductSubscriptionDetail> list);
    }
}
