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
    public interface ISubscriberOriginal
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.SubscriberOriginal>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client);

        [OperationContract(Name = "SelectForProcessCode")]
        Response<List<FrameworkUAD.Entity.SubscriberOriginal>> Select(Guid accessKey, string processCode, KMPlatform.Object.ClientConnections client);

        [OperationContract(Name = "SelectForSourceFile")]
        Response<List<FrameworkUAD.Entity.SubscriberOriginal>> Select(Guid accessKey, int sourceFileID, KMPlatform.Object.ClientConnections client);

        [OperationContract(Name = "SelectForProcessCodeAndSourceFile")]
        Response<List<FrameworkUAD.Entity.SubscriberOriginal>> Select(Guid accessKey, string processCode, int sourceFileID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<List<FrameworkUAD.Entity.SubscriberOriginal>> SelectForFileAudit(Guid accessKey, string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.SubscriberOriginal x, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> SaveBulkUpdate(Guid accessKey, List<FrameworkUAD.Entity.SubscriberOriginal> list, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> SaveBulkInsert(Guid accessKey, List<FrameworkUAD.Entity.SubscriberOriginal> list, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> SaveBulkSqlInsert(Guid accessKey, List<FrameworkUAD.Entity.SubscriberOriginal> list, KMPlatform.Object.ClientConnections client);
    }
}
