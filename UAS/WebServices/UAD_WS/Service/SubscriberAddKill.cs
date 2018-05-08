using System;
using System.Collections.Generic;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.SubscriberAddKill;
using EntitySubscriberAddKill = FrameworkUAD.Entity.SubscriberAddKill;
using EntitySubscriberAddKillDetail = FrameworkUAD.Entity.SubscriberAddKillDetail;

namespace UAD_WS.Service
{
    public class SubscriberAddKill : FrameworkServiceBase, ISubscriberAddKill
    {
        private const string EntityName = "SubscriberAddKill";
        private const string MethodClearDetails = "ClearDetails";
        private const string MethodSelect = "Select";
        private const string MethodSave = "Save";
        private const string MethodBulkInsertDetails = "BulkInsertDetails";

        /// <summary>
        /// Selects a list of SubscriberAddKill objects based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberAddKill objects</returns>
        public Response<List<EntitySubscriberAddKill>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<List<EntitySubscriberAddKill>>
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
        /// Saves the SubscriberAddKill object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="subAddKill">the SubAddKill object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, EntitySubscriberAddKill subAddKill, KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData =  new JsonFunctions().ToJson(subAddKill),
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = _ => new BusinessLogicWorker().Save(subAddKill, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Updates the subscriptions database table of the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="addKillID">the add kill object ID</param>
        /// <param name="productID">the product ID</param>
        /// <param name="subscriptionIDs">the subscription IDs</param>
        /// <param name="deleteAddRemoveID">boolean to delete add or remove the subscription ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> UpdateSubscription(
            Guid accessKey,
            int addKillID,
            int productID,
            string subscriptionIDs,
            bool deleteAddRemoveID,
            KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData =  string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = _ => new BusinessLogicWorker().UpdateSubscription(
                    addKillID,
                    productID,
                    subscriptionIDs,
                    deleteAddRemoveID,
                    client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Bulk Insert AddKillDetail Records
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="subs">List of PubsubscriptionIDs</param>
        /// <param name="addRemoveID">Add Remove ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a bool</returns>
        public Response<bool> BulkInsertDetails(
            Guid accessKey,
            List<EntitySubscriberAddKillDetail> subs,
            int addRemoveID,
            KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = $"Add Remove ID: {addRemoveID}",
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodBulkInsertDetails,
                WorkerFunc = _ => new BusinessLogicWorker().BulkInsertDetail(subs, addRemoveID, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Bulk Clear AddKillDetail Records
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="productID"></param>
        /// <param name="client"></param>
        /// <returns>response.result will contain a bool</returns>
        public Response<bool> ClearDetails(Guid accessKey, int productID, KMPlatform.Object.ClientConnections client)
        {
            var param = $"ProductID: {productID}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodClearDetails,
                WorkerFunc = _ => new BusinessLogicWorker().ClearDetails(productID, client)
            };

            return GetResponse(model);
        }
    }
}
