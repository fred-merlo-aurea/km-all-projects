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
    public interface ISubscriberFinal
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.SubscriberFinal>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client);

        [OperationContract(Name = "SelectForProcessCode")]
        Response<List<FrameworkUAD.Entity.SubscriberFinal>> Select(Guid accessKey, string processCode, KMPlatform.Object.ClientConnections client);

        [OperationContract(Name = "SelectByAddressValidationForProcessCode")]
        Response<List<FrameworkUAD.Entity.SubscriberFinal>> SelectByAddressValidation(Guid accessKey, KMPlatform.Object.ClientConnections client, string processCode, int sourceFileID, bool isLatLonValid);

        [OperationContract]
        Response<List<FrameworkUAD.Entity.SubscriberFinal>> SelectByAddressValidation(Guid accessKey, KMPlatform.Object.ClientConnections client, bool isLatLonValid);

        [OperationContract]
        Response<List<FrameworkUAD.Entity.SubscriberFinal>> SelectForFileAudit(Guid accessKey, string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> SaveBulkUpdate(Guid accessKey, List<FrameworkUAD.Entity.SubscriberFinal> list, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> SaveBulkInsert(Guid accessKey, List<FrameworkUAD.Entity.SubscriberFinal> list, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.SubscriberFinal x, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> SaveDQMClean(Guid accessKey, KMPlatform.Object.ClientConnections client, int sourceFileID, string processCode, string fileType);

        [OperationContract]
        Response<bool> SetMissingMaster(Guid accessKey, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> SetOneMaster(Guid accessKey, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> AddressSearch(Guid accessKey, string address, string mailstop, string city, string state, string zip, KMPlatform.Object.ClientConnections client);
    }
}
