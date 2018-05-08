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
    public interface ISubscriberDemographicFinal
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.SubscriberDemographicFinal>> SelectPublication(Guid accessKey, int pubID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<List<FrameworkUAD.Entity.SubscriberDemographicFinal>> SelectSubscriberOriginal(Guid accessKey, Guid SORecordIdentifier, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<List<FrameworkUAD.Entity.SubscriberDemographicFinal>> SelectForFileAudit(Guid accessKey, string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.SubscriberDemographicFinal x, KMPlatform.Object.ClientConnections client);
    }
}
