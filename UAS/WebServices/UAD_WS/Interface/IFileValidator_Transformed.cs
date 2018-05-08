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
    public interface IFileValidator_Transformed
    {
        [OperationContract]
        Response<bool> SaveBulkSqlInsert(Guid accessKey, List<FrameworkUAD.Entity.FileValidator_Transformed> list, KMPlatform.Object.ClientConnections client);
    }
}
