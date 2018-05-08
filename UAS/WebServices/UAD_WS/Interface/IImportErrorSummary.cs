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
    public interface IImportErrorSummary
    {
        [OperationContract]
        Response<List<FrameworkUAD.Object.ImportErrorSummary>> Select(Guid accessKey, int sourceFileID, string processCode, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<List<FrameworkUAD.Object.ImportErrorSummary>> FileValidatorSelect(Guid accessKey, int sourceFileID, string processCode, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<string> CreateDimensionErrorsSummaryReport(Guid accessKey, int sourceFileID, string processCode, string clientName, KMPlatform.Object.ClientConnections client, string clientArchiveDir);
    }
}
