using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;

namespace UAD_WS.Interface
{
    [ServiceContract]
    public interface IEmailStatus
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.EmailStatus>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client);
    }
}
