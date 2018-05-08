using System;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;

namespace UAD_WS.Interface
{
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface ISubscriberMasterValue
    {
        [OperationContract]
        Response<bool> DeleteMasterID(Guid accessKey, int masterID, KMPlatform.Object.ClientConnections client);
    }
}
