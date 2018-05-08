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
    public interface IIssueArchiveProductSubscription
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.IssueArchiveProductSubscription>> SelectIssue(Guid accessKey, int issueID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<List<FrameworkUAD.Entity.IssueArchiveProductSubscription>> SelectPaging(Guid accessKey, int page, int pageSize, int issueID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<List<FrameworkUAD.Entity.IssueArchiveProductSubscription>> SelectForUpdate(Guid accessKey, int productID, int issueid, List<int> subs, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<int> SelectCount(Guid accessKey, int issueID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<bool> SaveBulkSqlInsert(Guid accessKey, List<FrameworkUAD.Entity.IssueArchiveProductSubscription> list, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.IssueArchiveProductSubscription x, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<int> SaveAll(Guid accessKey, FrameworkUAD.Entity.IssueArchiveProductSubscription x, KMPlatform.Object.ClientConnections client);
    }
}
