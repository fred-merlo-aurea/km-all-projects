using System;
using System.Collections.Generic;
using FrameworkUAS.Service;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.BatchHistoryDetail;
using EntityBatchHistoryDetail = FrameworkUAD.Object.BatchHistoryDetail;

namespace UAD_WS.Service
{
    public class BatchHistoryDetail : FrameworkServiceBase, IBatchHistoryDetail
    {
        private const string EntityName = "BatchHistoryDetail";
        private const string MethodSelect = "Select";
        private const string MethodSelectForSubscriber = "SelectForSubscriber";
        private const string MethodSelectBatch = "SelectBatch";

        /// <summary>
        /// Selects a list of BatchHistoryDetail objects based on the user ID, the client and if it is active or not
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="userID">the user ID</param>
        /// <param name="isActive">boolean if the batch history is active or not</param>
        /// <param name="client">the Client object</param>
        /// <param name="clientName">the Client object</param>
        /// <returns>response.result will contain a list of BatchHistoryDetail objects</returns>
        public Response<List<EntityBatchHistoryDetail>> Select(
            Guid accessKey,
            int userID,
            bool isActive,
            KMPlatform.Object.ClientConnections client,
            string clientName)
        {
            var model = new ServiceRequestModel<List<EntityBatchHistoryDetail>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = $"UserID:{userID} IsActive:{isActive}",
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = request => new BusinessLogicWorker().Select(userID, isActive, client, clientName)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of BatchHistoryDetail objects based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the Client object</param>
        /// /// <param name="clientName">the Client object</param>
        /// <returns>response.result will contain a list of BatchHistoryDetail objects</returns>
        public Response<List<EntityBatchHistoryDetail>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client, string clientName)
        {
            var model = new ServiceRequestModel<List<EntityBatchHistoryDetail>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = request => new BusinessLogicWorker().Select(client, clientName)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of BatchHistoryDetail objects  based on the subscription ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="subscriptionID">the subscription ID</param>
        /// <param name="client">the Client object</param>
        /// /// <param name="clientName">the Client object</param>
        /// <returns>response.result will contain a list of BatchHistoryDetail objects</returns>
        public Response<List<EntityBatchHistoryDetail>> Select(
            Guid accessKey,
            int subscriptionID,
            KMPlatform.Object.ClientConnections client,
            string clientName)
        {
            var model = new ServiceRequestModel<List<EntityBatchHistoryDetail>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = $"SubscriptionID:{subscriptionID}",
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = request => new BusinessLogicWorker().Select(subscriptionID, client, clientName)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of BatchHistoryDetail objects based on the subscriber ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="subscriberID">the subscriber ID</param>
        /// <param name="client">the client object</param>
        /// /// <param name="clientName">the Client object</param>
        /// <returns>response.result will contain a list of BatchHistoryDetail objects</returns>
        public Response<List<EntityBatchHistoryDetail>> SelectForSubscriber(
            Guid accessKey,
            int subscriberID,
            KMPlatform.Object.ClientConnections client,
            string clientName)
        {
            var model = new ServiceRequestModel<List<EntityBatchHistoryDetail>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = $"SubscriberID:{subscriberID}",
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForSubscriber,
                WorkerFunc = request => new BusinessLogicWorker().SelectSubscriber(subscriberID, client, clientName)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of BatchHistoryDetail objects based on the batch ID, batch name, the client and sequence ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="BatchID">the batch ID</param>
        /// <param name="Name">the name of the batch</param>
        /// <param name="SequenceID">the sequence ID</param>
        /// <param name="client">the client object</param>
        /// /// <param name="clientName">the Client object</param>
        /// <returns>response.result will contain a list of BatchHistoryDetail objects</returns>
        public Response<List<EntityBatchHistoryDetail>> SelectBatch(
            Guid accessKey,
            int BatchID,
            string Name,
            int SequenceID,
            KMPlatform.Object.ClientConnections client,
            string clientName)
        {
            var model = new ServiceRequestModel<List<EntityBatchHistoryDetail>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = $"BatchID:{BatchID} Name:{Name} SequenceID:{SequenceID}",
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectBatch,
                WorkerFunc = request => new BusinessLogicWorker().SelectBatch(BatchID, Name, SequenceID, client, clientName)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of BatchHistoryDetail objects based on the batch ID, batch name, the client sequence ID and the dates
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="BatchID">the batch ID</param>
        /// <param name="Name">the batch name</param>
        /// <param name="SequenceID">the sequence ID</param>
        /// <param name="From">date for when the batch history started</param>
        /// <param name="To">date for when the batch history ends</param>
        /// <param name="client">the client object</param>
        /// /// <param name="clientName">the Client object</param>
        /// <returns>response.result will contain a list of BatchHistoryDetail objects</returns>
        public Response<List<EntityBatchHistoryDetail>> SelectBatch(
            Guid accessKey,
            int BatchID,
            string Name,
            int SequenceID,
            DateTime From,
            DateTime To,
            KMPlatform.Object.ClientConnections client, string clientName)
        {
            var param = $"BatchID:{BatchID} Name:{Name} SequenceID:{SequenceID} From:{From.ToShortDateString()} To:{To.ToShortDateString()}";
            var model = new ServiceRequestModel<List<EntityBatchHistoryDetail>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectBatch,
                WorkerFunc = request => new BusinessLogicWorker().SelectBatch(BatchID, Name, SequenceID, From, To, client, clientName)
            };

            return GetResponse(model);
        }
    }
}
