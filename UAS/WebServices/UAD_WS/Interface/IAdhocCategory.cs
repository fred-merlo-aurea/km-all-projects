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
    public interface IAdhocCategory
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.AdhocCategory>> SelectAll(Guid accessKey, KMPlatform.Object.ClientConnections client);
        
        [OperationContract]
        Response<int> Save(Guid accessKey, KMPlatform.Object.ClientConnections client, FrameworkUAD.Entity.AdhocCategory x);
    }
}
