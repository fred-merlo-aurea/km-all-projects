using UAD_WS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;
using HistoryBusinessLogic = FrameworkUAD.BusinessLogic.History;
using HistoryEntity = FrameworkUAD.Entity.History;
using UtilityJsonFunctions = Core_AMS.Utilities.JsonFunctions;

namespace UAD_WS.Service
{
    public class History : ServiceBase, IHistory
    {
        private const string EntityHistory = "History";
        private const string MethodSelect = "Select";
        private const string MethodUserLogList = "UserLogList";
        private const string MethodHistoryResponseList = "HistoryResponseList";
        private const string MethodHistoryMarketingMapList = "HistoryMarketingMapList";
        private const string MethodAddHistoryEntry = "AddHistoryEntry";
        private const string MethodInsertHistoryToUserLog = "Insert_History_To_UserLog";
        private const string MethodInsertHistoryToHistoryMarketingMapList = "Insert_History_To_HistoryMarketingMap_List";
        private const string MethodInsertHistoryToHistoryMarketingMap = "Insert_History_To_HistoryMarketingMap";

        /// <summary>
        /// Selects a list of History objects based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of History objects</returns>
        public Response<List<HistoryEntity>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client)
        {
            var model = new RequestModel<List<HistoryEntity>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityHistory,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ =>
                {
                    var worker = new HistoryBusinessLogic();
                    return worker.Select(client);
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of History objects based on the batch ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="batchID">the batch ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of History objects</returns>
        public Response<List<HistoryEntity>> Select(Guid accessKey, int batchID, KMPlatform.Object.ClientConnections client)
        {
            var param = $"batchID:{batchID}";
            var model = new RequestModel<List<HistoryEntity>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityHistory,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ =>
                {
                    var worker = new HistoryBusinessLogic();
                    return worker.Select(batchID, client);
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of History objects based on the specified start and end dates and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="startDate">the start date</param>
        /// <param name="endDate">the end date</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of History objects</returns>
        public Response<List<HistoryEntity>> Select(Guid accessKey, DateTime startDate, DateTime endDate, KMPlatform.Object.ClientConnections client)
        {
            var param = $"startDate:{startDate} endDate:{endDate}";
            var model = new RequestModel<List<HistoryEntity>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityHistory,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ =>
                {
                    var worker = new HistoryBusinessLogic();
                    return worker.Select(startDate, endDate, client);
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a lis tof History objects based on the user ID, the publication ID, and the client 
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="userID">the user ID</param>
        /// <param name="publicationID">the publication ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of History objects</returns>
        public Response<List<HistoryEntity>> Select(Guid accessKey, int userID, int publicationID, KMPlatform.Object.ClientConnections client)
        {
            var param = $"userID:{userID} publicationID:{publicationID}";
            var model = new RequestModel<List<HistoryEntity>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityHistory,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ =>
                {
                    var worker = new HistoryBusinessLogic();
                    return worker.SelectBatch(userID, publicationID, client);
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Creates a user log list from the History objects based on the history ID and the client 
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="historyID">the history ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer list</returns>
        public Response<List<int>> UserLogList(Guid accessKey, int historyID, KMPlatform.Object.ClientConnections client)
        {
            var param = $"historyID:{historyID}";
            var model = new RequestModel<List<int>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityHistory,
                AuthenticateMethod = MethodUserLogList,
                WorkerFunc = _ =>
                {
                    var worker = new HistoryBusinessLogic();
                    return worker.UserLogList(historyID, client);
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Creates a list of History responses from the History objects based on the history ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="historyID">the history ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer list</returns>
        public Response<List<int>> HistoryResponseList(Guid accessKey, int historyID, KMPlatform.Object.ClientConnections client)
        {
            var param = $"historyID:{historyID}";
            var model = new RequestModel<List<int>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityHistory,
                AuthenticateMethod = MethodHistoryResponseList,
                WorkerFunc = _ =>
                {
                    var worker = new HistoryBusinessLogic();
                    return worker.HistoryResponseList(historyID, client);
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Creates a list of HistoryMarketingMap objects from the History object based on the history ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="historyID">the history ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer list</returns>
        public Response<List<int>> HistoryMarketingMapList(Guid accessKey, int historyID, KMPlatform.Object.ClientConnections client)
        {
            var param = $"historyID:{historyID}";
            var model = new RequestModel<List<int>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityHistory,
                AuthenticateMethod = MethodHistoryMarketingMapList,
                WorkerFunc = _ =>
                {
                    var worker = new HistoryBusinessLogic();
                    return worker.HistoryMarketingMapList(historyID, client);
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Adds a History entry to the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="batchID">the batch ID</param>
        /// <param name="batchCountItem">the batch count item</param>
        /// <param name="publicationID">the publication ID</param>
        /// <param name="subscriberID">the subscriber ID</param>
        /// <param name="subscriptionID">the subscription ID</param>
        /// <param name="historySubscriptionID">the history subscription ID</param>
        /// <param name="historyPaidID">the history paid ID</param>
        /// <param name="userID">the user ID</param>
        /// <param name="historyPaidBillToID">the history paid bill to ID</param>
        /// <param name="userLogIDs">the list of user log IDs</param>
        /// <param name="historyResponseIDs">the list of history response IDs</param>
        /// <param name="historyMarketingMapIDs">the list of history marketing map IDs</param>
        /// <returns>response.result will contain a History object</returns>
        public Response<HistoryEntity> AddHistoryEntry(Guid accessKey, KMPlatform.Object.ClientConnections client, int batchID, int batchCountItem, int publicationID, int subscriberID, int subscriptionID, int historySubscriptionID, int historyPaidID, int userID, int historyPaidBillToID = -1, List<int> userLogIDs = null, List<int> historyResponseIDs = null, List<int> historyMarketingMapIDs = null)
        {
            var param = $"batchID:{batchID} batchCountItem:{batchCountItem} publicationID:{publicationID} subscriberID:{subscriberID} subscriptionID:{subscriptionID} historySubscriptionID:{historySubscriptionID} historyPaidID:{historyPaidID} userID:{userID}";
            var model = new RequestModel<HistoryEntity>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityHistory,
                AuthenticateMethod = MethodAddHistoryEntry,
                WorkerFunc = _ =>
                {
                    var worker = new HistoryBusinessLogic();
                    return worker.AddHistoryEntry(
                        client,
                        batchID,
                        batchCountItem,
                        publicationID,
                        subscriberID,
                        subscriptionID,
                        historySubscriptionID,
                        historyPaidID,
                        userID,
                        historyPaidBillToID,
                        userLogIDs,
                        historyResponseIDs,
                        historyMarketingMapIDs);
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Inserts a History object to the User log based on the history ID, user log ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="historyID">the history ID</param>
        /// <param name="userLogID">the user log ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Insert_History_To_UserLog(Guid accessKey, int historyID, int userLogID, KMPlatform.Object.ClientConnections client)
        {
            var param = $"historyID:{historyID} userLogID:{userLogID}";
            var model = new RequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityHistory,
                AuthenticateMethod = MethodInsertHistoryToUserLog,
                WorkerFunc = _ =>
                {
                    var worker = new HistoryBusinessLogic();
                    return worker.Insert_History_To_UserLog(historyID, userLogID, client);
                }
            };

            return GetResponse(model);
        }
    
        /// <summary>
        /// Inserts a History object into a HistoryMarketingMap list based on the client and the history ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="historyID">the history ID</param>
        /// <param name="list">the HistoryMarketingMap list</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Insert_History_To_HistoryMarketingMap_List(Guid accessKey, int historyID, List<FrameworkUAD.Entity.HistoryMarketingMap> list, KMPlatform.Object.ClientConnections client)
        {
            var param =
                $"historyID:{historyID} historyMarketingMapID:{list.Select(h => h.HistoryMarketingMapID).Distinct()}";
            var model = new RequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityHistory,
                AuthenticateMethod = MethodInsertHistoryToHistoryMarketingMapList,
                WorkerFunc = _ =>
                {
                    var worker = new HistoryBusinessLogic();
                    return worker.Insert_History_To_HistoryMarketingMap_List(historyID, list, client);
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Inserts a history object into a HistoryMarketingMap object based on the history ID, the history marketing map ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="historyID">the history ID</param>
        /// <param name="historyMarketingMapID">the history marketing map ID </param>
        /// <param name="client">teh client object</param>
        /// <returns>resposne.result will contain a boolean</returns>
        public Response<bool> Insert_History_To_HistoryMarketingMap(Guid accessKey, int historyID, int historyMarketingMapID, KMPlatform.Object.ClientConnections client)
        {
            var param = $"historyID:{historyID} historyMarketingMapID:{historyMarketingMapID}";
            var model = new RequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityHistory,
                AuthenticateMethod = MethodInsertHistoryToHistoryMarketingMap,
                WorkerFunc = _ =>
                {
                    var worker = new HistoryBusinessLogic();
                    return worker.Insert_History_To_HistoryMarketingMap(historyID, historyMarketingMapID, client);
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves a history object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the History object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, HistoryEntity x, KMPlatform.Object.ClientConnections client)
        {
            var jsonFunction = new UtilityJsonFunctions();
            var param = jsonFunction.ToJson(x);
            var model = new RequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityHistory,
                AuthenticateMethod = MethodInsertHistoryToHistoryMarketingMap,
                WorkerFunc = _ =>
                {
                    var worker = new HistoryBusinessLogic();
                    return worker.Save(x, client);
                }
            };

            return GetResponse(model);
        }
    }
}
