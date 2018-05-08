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
    public interface IProspect
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.Prospect>> Select(Guid accessKey, int subscriberID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> Save(Guid accessKey, FrameworkUAD.Entity.Prospect x, KMPlatform.Object.ClientConnections client);
    }
}
