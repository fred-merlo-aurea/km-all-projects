using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;
using System.Data;

namespace UAD_WS.Interface
{
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface IIssueCompDetail
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.IssueCompDetail>> Select(Guid accessKey, int issueCompID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<List<int>> GetByFilter(Guid accessKey, string xml, string adHocXml, int issueCompID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<bool> Clear(Guid accessKey, int issueCompID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.IssueCompDetail x, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<DataTable> SelectForExport(Guid accessKey, int issueID, string cols, List<int> subs, KMPlatform.Object.ClientConnections client);
    }
}
