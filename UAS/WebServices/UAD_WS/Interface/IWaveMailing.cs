using FrameworkUAS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace UAD_WS.Interface
{
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface IWaveMailing
    {
        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.WaveMailing x, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<List<FrameworkUAD.Entity.WaveMailing>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client);
    }
}
