using System;
using System.Collections.Generic;
using FrameworkUAS.Service;
using KMPlatform.Object;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.HistorySubscription;
using EntityHistorySubscription = FrameworkUAD.Entity.HistorySubscription;
using EntityProductSubscription = FrameworkUAD.Entity.ProductSubscription;
using UtilityJsonFunctions = Core_AMS.Utilities.JsonFunctions;

namespace UAD_WS.Service
{
    public class HistorySubscription : FrameworkServiceBase, IHistorySubscription
    {
        private const string EntityName = "HistorySubscription";
        private const string MethodSelect = "Select";
        private const string MethodSave = "Save";

        /// <summary>
        /// Selects a list of HistorySubscription objects based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of HistorySubscription objects</returns>
        public Response<List<EntityHistorySubscription>> Select(Guid accessKey, ClientConnections client)
        {
            var model = new ServiceRequestModel<List<EntityHistorySubscription>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves the HistorySubscription object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntityHistorySubscription"/> object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, EntityHistorySubscription entity, ClientConnections client)
        {
            var param = new UtilityJsonFunctions().ToJson(entity);
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = _ => new BusinessLogicWorker().Save(entity, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves the information from the ProductSubscription object to a HistorySubscription object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="productSubscription">the product subscription object</param>
        /// <param name="userID">the user ID</param>
        /// <param name="client"></param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> SaveForSubscriber(Guid accessKey, EntityProductSubscription productSubscription, int userID, ClientConnections client)
        {
            var param = new UtilityJsonFunctions().ToJson(productSubscription);
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = _ => new BusinessLogicWorker().Save(productSubscription, userID, client)
            };

            return GetResponse(model);
        }
    }
}
