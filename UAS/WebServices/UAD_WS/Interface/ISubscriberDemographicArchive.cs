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
    public interface ISubscriberDemographicArchive
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.SubscriberDemographicArchive>> SelectPublication(Guid accessKey, int pubID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<List<FrameworkUAD.Entity.SubscriberDemographicArchive>> SelectSubscriberOriginal(Guid accessKey, Guid SFRecordIdentifier, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<List<FrameworkUAD.Entity.SubscriberDemographicArchive>> SelectForFileAudit(Guid accessKey, string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.SubscriberDemographicArchive x, KMPlatform.Object.ClientConnections client);
    }
}
