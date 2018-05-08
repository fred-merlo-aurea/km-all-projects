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
    public interface IImportSummary
    {
        [OperationContract]
        Response<List<FrameworkUAD.Object.ImportErrorSummary>> Select(Guid accessKey, int sourceFileID, string processCode, KMPlatform.Object.ClientConnections client);
    }
}
