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
    public interface IFileValidator_ImportError
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.FileValidator_ImportError>> Select(Guid accessKey, string processCode, int sourceFileID, KMPlatform.Object.ClientConnections client);

        [OperationContract(Name = "SaveBulkSqlInsertList")]
        Response<bool> SaveBulkSqlInsert(Guid accessKey, List<FrameworkUAD.Entity.ImportError> list, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> SaveBulkSqlInsert(Guid accessKey, FrameworkUAD.Entity.ImportError x, KMPlatform.Object.ClientConnections client);
    }
}
