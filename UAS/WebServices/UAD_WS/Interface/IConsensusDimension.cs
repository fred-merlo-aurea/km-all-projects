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
    public interface IConsensusDimension
    {
        [OperationContract]
        Response<bool> SaveXML(Guid accessKey, List<FrameworkUAD.Object.ConsensusDimension> list, int masterGroupID, KMPlatform.Object.ClientConnections client);
    }
}
