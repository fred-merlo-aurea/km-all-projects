using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;
using System.Data;

namespace UAD_WS.Interface
{
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface ICircImportExport
    {
        [OperationContract]
        Response<List<FrameworkUAD.Object.CircImportExport>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client);
        [OperationContract(Name="SelectForPublisher")]
        Response<List<FrameworkUAD.Object.CircImportExport>> Select(Guid accessKey, int PublisherID, int PublicationID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<DataTable> SelectDataTable(Guid accessKey, int publisherID, int publicationID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<bool> SaveBulkSqlUpdate(Guid accessKey, int UserID, List<FrameworkUAD.Object.CircImportExport> list, KMPlatform.Object.ClientConnections client);
    }
}
