using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FrameworkUAS.Service;
using System.Data;

namespace UAD_WS.Interface
{
    [ServiceContract]
    [ServiceKnownType(typeof(bool?))]
    [ServiceKnownType(typeof(int?))]
    public interface ISubscription
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.Subscription>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client, bool includeCustomProperties = false);

        [OperationContract(Name = "SelectForEmail")]
        Response<List<FrameworkUAD.Entity.Subscription>> Select(Guid accessKey, string email, KMPlatform.Object.ClientConnections client, bool includeCustomProperties = false);

        [OperationContract(Name = "SelectForSubscription")]
        Response<FrameworkUAD.Entity.Subscription> Select(Guid accessKey, int subscriptionID, KMPlatform.Object.ClientConnections client, bool includeCustomProperties = false);

        [OperationContract]
        Response<List<int>> SelectIDs(Guid accessKey, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<List<FrameworkUAD.Entity.Subscription>> SelectForFileAudit(Guid accessKey, string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.Subscription x, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<List<FrameworkUAD.Entity.Subscription>> GetSubscriber(Guid accessKey, string emailAddress);
        [OperationContract]
        Response<List<FrameworkUAD.Entity.Subscription>> SearchAddressZip(Guid accessKey, string address1, string zipCode, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<DataTable> FindMatches(Guid accessKey, int productID, string fname, string lname, string company, string address, string state, string zip, string phone, string email, string title, KMPlatform.Object.ClientConnections client);
    }
}
