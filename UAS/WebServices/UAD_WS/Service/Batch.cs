using System;
using System.Collections.Generic;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.Batch;
using EntityBatch = FrameworkUAD.Entity.Batch;

namespace UAD_WS.Service
{
    public class Batch : FrameworkServiceBase, IBatch
    {
        private const string EntityName = "Batch";
        private const string MethodCloseBatches = "CloseBatches";
        private const string MethodSave = "Save";
        private const string MethodStartNewBatch = "StartNewBatch";
        private const string MethodSelect = "Select";
        private const string MethodBatchCheck = "BatchCheck";

        /// <summary>
        /// Starts up a new batch based on the client for user and publication IDs
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="userID">the user ID</param>
        /// <param name="publicationID">the publication ID</param>
        /// <param name="client">the Client object</param>
        /// <returns>response.result will contain a Batch object</returns>
        public Response<EntityBatch> StartNewBatch(Guid accessKey, int userID, int publicationID, KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<EntityBatch>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = $"UserID:{userID} PublicationID:{publicationID}",
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodStartNewBatch,
                WorkerFunc = _ => new BusinessLogicWorker().StartNewBatch(userID, publicationID, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of Batch objects based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the Client object</param>
        /// <returns>response.result will contain a list of Batch objects</returns>
        public Response<List<EntityBatch>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<List<EntityBatch>>
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
        /// Selects a list of Batch objects based on the user ID, the client and if it is active or not
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="userID">the user ID</param>
        /// <param name="isActive">boolean if the batch object is active or not</param>
        /// <param name="client">the Client object</param>
        /// <returns>response.result will contain a list of Batch objects</returns>
        public Response<List<EntityBatch>> Select(Guid accessKey, int userID, bool isActive, KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<List<EntityBatch>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = $"UserID:{userID} IsActive:{isActive}",
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(userID, isActive, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Checks the given Batch to see if it has completed 100 transactions
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="batch">the Batch object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> BatchCheck(Guid accessKey, EntityBatch batch)
        {
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = new JsonFunctions().ToJson(batch),
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodBatchCheck,
                WorkerFunc = _ => new BusinessLogicWorker().BatchCheck(batch)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves the given Batch object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntityBatch"/> object</param>
        /// <param name="client">the Client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, EntityBatch entity, KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = new JsonFunctions().ToJson(entity),
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = request => new BusinessLogicWorker().Save(entity, client)
            };

            return GetResponse(model);
        }

        public Response<bool> CloseBatches(Guid accessKey, int userID, KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = $"UserID:{userID}",
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodCloseBatches,
                WorkerFunc = _ => new BusinessLogicWorker().CloseBatches(userID, client)
            };

            return GetResponse(model);
        }
    }
}
