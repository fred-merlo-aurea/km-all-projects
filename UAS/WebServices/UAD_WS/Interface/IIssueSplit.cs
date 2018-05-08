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
    public interface IIssueSplit
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.IssueSplit>> SelectForIssueID(Guid accessKey, int issueID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<bool> Save(Guid accessKey, List<FrameworkUAD.Entity.IssueSplit> x, int issueID, KMPlatform.Object.ClientConnections client);
    }
}
