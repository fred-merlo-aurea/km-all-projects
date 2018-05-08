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
    public interface IIssueComp
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.IssueComp>> SelectIssue(Guid accessKey, int issueID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.IssueComp x, KMPlatform.Object.ClientConnections client);
    }
}
