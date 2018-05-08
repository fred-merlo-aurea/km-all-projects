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
    public interface IResponseGroup
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.ResponseGroup>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client);

        [OperationContract(Name = "SelectForResponseGroup")]
        Response<List<FrameworkUAD.Entity.ResponseGroup>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client, int pubID);

        [OperationContract]
        Response<int> Save(Guid accessKey, KMPlatform.Object.ClientConnections client, FrameworkUAD.Entity.ResponseGroup x);

        [OperationContract]
        Response<bool> Copy(Guid accessKey, int responseGroupID, string destPubsXML, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> Delete(Guid accessKey, int responseGroupID, KMPlatform.Object.ClientConnections client);
    }
}
