using System;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;

namespace UAD_WS.Interface
{
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface IAcsShippingDetail
    {
        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.AcsShippingDetail acsShippingDetail, KMPlatform.Object.ClientConnections client);
    }
}
