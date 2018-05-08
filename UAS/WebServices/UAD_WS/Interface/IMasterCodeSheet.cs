using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;
using System.Xml.Linq;

namespace UAD_WS.Interface
{
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface IMasterCodeSheet
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.MasterCodeSheet>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<List<FrameworkUAD.Entity.MasterCodeSheet>> SelectMasterGroupID(Guid accessKey, KMPlatform.Object.ClientConnections client, int masterGroupID);

        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.MasterCodeSheet x, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<int> ImportSubscriber(Guid accessKey, int masterID, XDocument xDoc, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> DeleteMasterID(Guid accessKey, int masterID, KMPlatform.Object.ClientConnections client);
    }
}
