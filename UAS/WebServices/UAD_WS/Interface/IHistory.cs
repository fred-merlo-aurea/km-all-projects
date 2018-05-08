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
    public interface IHistory
    {
        [OperationContract]
        Response<List<FrameworkUAD.Entity.History>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client);

        [OperationContract(Name="SelectForBatch")]
        Response<List<FrameworkUAD.Entity.History>> Select(Guid accessKey, int batchID, KMPlatform.Object.ClientConnections client);

        [OperationContract(Name="SelectForDates")]
        Response<List<FrameworkUAD.Entity.History>> Select(Guid accessKey, DateTime startDate, DateTime endDate, KMPlatform.Object.ClientConnections client);

        [OperationContract(Name="SelectForUser")]
        Response<List<FrameworkUAD.Entity.History>> Select(Guid accessKey, int UserID, int PublicationID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<List<int>> UserLogList(Guid accessKey, int historyID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<List<int>> HistoryResponseList(Guid accessKey, int historyID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<List<int>> HistoryMarketingMapList(Guid accessKey, int historyID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<FrameworkUAD.Entity.History> AddHistoryEntry(Guid accessKey, KMPlatform.Object.ClientConnections client, int batchID, int batchCountItem, int publicationID, int subscriberID, int subscriptionID, int historySubscriptionID, int historyPaidID, int userID, int historyPaidBillToID = -1, List<int> userLogIDs = null, List<int> historyResponseIDs = null, List<int> historyMarketingMapIDs = null);

        [OperationContract]
        Response<bool> Insert_History_To_UserLog(Guid accessKey, int historyID, int userLogID, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<bool> Insert_History_To_HistoryMarketingMap(Guid accessKey, int historyID, int historyMarketingMapID, KMPlatform.Object.ClientConnections client);
        [OperationContract]
        Response<bool> Insert_History_To_HistoryMarketingMap_List(Guid accessKey, int historyID, List<FrameworkUAD.Entity.HistoryMarketingMap> list, KMPlatform.Object.ClientConnections client);

        [OperationContract]
        Response<int> Save(Guid accessKey, FrameworkUAD.Entity.History x, KMPlatform.Object.ClientConnections client);

    }
}
