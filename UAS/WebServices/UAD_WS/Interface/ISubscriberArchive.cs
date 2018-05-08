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
    public interface ISubscriberArchive
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.SubscriberArchive>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client);

        [OperationContract(Name = "SelectForProcessCode")]
        Response<List<FrameworkUAD.Entity.SubscriberArchive>> Select(Guid accessKey, string processCode, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<List<FrameworkUAD.Entity.SubscriberArchive>> SelectForFileAudit(Guid accessKey, string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.SubscriberArchive x, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> SaveBulkInsert(Guid accessKey, List<FrameworkUAD.Entity.SubscriberArchive> list, KMPlatform.Object.ClientConnections client);
    }
}
