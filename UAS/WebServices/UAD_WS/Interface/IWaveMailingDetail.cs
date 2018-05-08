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
    public interface IWaveMailingDetail
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.WaveMailingDetail>> SelectIssue(Guid accessKey, int issueID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<bool> UpdateOriginalSubInfo(Guid accessKey, int productID, int userID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.WaveMailingDetail x, KMPlatform.Object.ClientConnections client);
    }
}
