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
    public interface ISubscriberInvalid
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.SubscriberInvalid>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client);

        [OperationContract(Name = "SelectForProcessCode")]
        Response<List<FrameworkUAD.Entity.SubscriberInvalid>> Select(Guid accessKey, string processCode, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<List<FrameworkUAD.Entity.SubscriberInvalid>> SelectForFileAudit(Guid accessKey, string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> SaveBulkSqlInsert(Guid accessKey, List<FrameworkUAD.Entity.SubscriberInvalid> list, KMPlatform.Object.ClientConnections client);
    }
}
