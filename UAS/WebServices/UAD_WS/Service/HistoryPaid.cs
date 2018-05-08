using System;
using System.Collections.Generic;
using FrameworkUAS.Service;
using KMPlatform.Object;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.HistoryPaid;
using EntityHistoryPaid = FrameworkUAD.Entity.HistoryPaid;
using EntitySubscriptionPaid = FrameworkUAD.Entity.SubscriptionPaid;
using UtilityJsonFunctions = Core_AMS.Utilities.JsonFunctions;

namespace UAD_WS.Service
{
    public class HistoryPaid : FrameworkServiceBase, IHistoryPaid
    {
        private const string EntityName = "HistoryPaid";
        private const string MethodSelect = "Select";
        private const string MethodSave = "Save";

        /// <summary>
        /// Selects a list of HistoryPaid objects based on the subscription ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="subscriptionID">the subscription ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of HistoryPaid objects</returns>
        public Response<List<EntityHistoryPaid>> Select(Guid accessKey, int subscriptionID, ClientConnections client)
        {
            var param = $"subscriptionID:{subscriptionID}";
            var model = new ServiceRequestModel<List<EntityHistoryPaid>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(subscriptionID, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves the SubscriptionPaid object and the user ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="subscriptionPaid">the <see cref="EntitySubscriptionPaid"/> object</param>
        /// <param name="userID">the user ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, EntitySubscriptionPaid subscriptionPaid, int userID, ClientConnections client)
        {
            var param = new UtilityJsonFunctions().ToJson(subscriptionPaid);
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = _ => new BusinessLogicWorker().Save(subscriptionPaid, userID, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves the HistoryPaid object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntityHistoryPaid"/> object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, EntityHistoryPaid entity, ClientConnections client)
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
    }
}
