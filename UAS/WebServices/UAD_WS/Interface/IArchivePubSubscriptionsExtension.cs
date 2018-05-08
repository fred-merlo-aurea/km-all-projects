using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;

namespace UAD_WS.Interface
{
    [ServiceContract]
    public interface IArchivePubSubscriptionsExtension
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.ArchivePubSubscriptionsExtension>> SelectForUpdate(Guid accessKey, int productID, int issueid, List<int> subs, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<List<FrameworkUAD.Object.PubSubscriptionAdHoc>> GetArchiveAdhocs(Guid accessKey, int productID, int issueid, int PubSubID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> Save(Guid accessKey, List<FrameworkUAD.Object.PubSubscriptionAdHoc> x, int issueArchiveSubscriptionID, int pubID, KMPlatform.Object.ClientConnections client);
    }
}
