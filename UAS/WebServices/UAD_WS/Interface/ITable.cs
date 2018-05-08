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
    public interface ITable
    {
        //[OperationContract]
        //Response<List<FrameworkUAD.Object.Table>> Select(Guid accessKey, string dbName);

        [OperationContract]
        Response<List<FrameworkUAD.Object.Table>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client, string dbName);

        [OperationContract]
        Response<DataTable> SelectDataTable(Guid accessKey, KMPlatform.Object.ClientConnections client, string dbName, string table, string pubCode);  
    }
}
