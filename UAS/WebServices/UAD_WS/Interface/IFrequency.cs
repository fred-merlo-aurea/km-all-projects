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
    public interface IFrequency
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.Frequency>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client);
    }
}
