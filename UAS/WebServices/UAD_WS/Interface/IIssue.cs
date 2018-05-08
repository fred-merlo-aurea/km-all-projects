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
    public interface IIssue
    {
        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.Issue x, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<List<FrameworkUAD.Entity.Issue>> SelectForPublication(Guid accesssKey, int publicationID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<List<FrameworkUAD.Entity.Issue>> SelectForPublisher(Guid accessKey, int publisherID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<List<FrameworkUAD.Entity.Issue>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<bool> ArchiveAll(Guid accessKey, int productID, int issueID, Dictionary<int, string> imb, Dictionary<int, string> compImb, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<bool> BulkInsertSubGenIDs(Guid accessKey, List<FrameworkUAD.Entity.IssueCloseSubGenMap> ids, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<bool> ValidateArchive(Guid accessKey, int pubId, int issueId, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<bool> RollBackIssue(Guid accessKey, int pubId, int issueId, int origIMB, KMPlatform.Object.ClientConnections client);

    }
}
