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
    public interface IIssueCompError
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.IssueCompError>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<List<FrameworkUAD.Entity.IssueCompError>> SelectProcessCode(Guid accessKey, string processCode, KMPlatform.Object.ClientConnections client);        
    }
}
