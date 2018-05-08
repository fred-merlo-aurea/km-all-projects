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
    public interface IAcsMailerInfo
    {
        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.AcsMailerInfo x, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<FrameworkUAD.Entity.AcsMailerInfo> SelectByID(Guid accessKey, int acsMailerInfoID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<List<FrameworkUAD.Entity.AcsMailerInfo>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client);
    }
}
