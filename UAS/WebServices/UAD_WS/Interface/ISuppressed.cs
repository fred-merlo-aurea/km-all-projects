using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;
using System.IO;

namespace UAD_WS.Interface
{
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface ISuppressed
    {
        [OperationContract]
        Response<bool> SaveBulkSqlInsert(Guid accessKey, List<FrameworkUAD.Entity.Suppressed> list, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<int> PerformSuppression(Guid accessKey, List<FrameworkUAD.Entity.SubscriberFinal> list, KMPlatform.Object.ClientConnections client, int sourceFileId, string processCode, FileInfo suppFile);
    }
}
