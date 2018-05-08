using System;
using System.Collections.Generic;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using KMPlatform.Object;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.Product;
using EntityProduct = FrameworkUAD.Entity.Product;

namespace UAD_WS.Service
{
    public class Product : FrameworkServiceBase, IProduct
    {
        private const string EntityName = "Product";
        private const string MethodUpdateLock = "UpdateLock";
        private const string MethodCopy = "Copy";
        private const string MethodSelect = "Select";
        private const string MethodSave = "Save";

        /// <summary>
        /// Selects a list of Product objects based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of Product objects</returns>
        public Response<List<EntityProduct>> Select(Guid accessKey, ClientConnections client)
        {
            var model = new ServiceRequestModel<List<EntityProduct>>
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
        /// Selects a Product object based on the pub ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="pubID">the pub ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a Product object</returns>
        public Response<EntityProduct> Select(Guid accessKey, int pubID, ClientConnections client)
        {
            var model = new ServiceRequestModel<EntityProduct>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(pubID, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves a Product object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntityProduct"/> object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, EntityProduct entity, ClientConnections client)
        {
            var param = new JsonFunctions().ToJson(entity);
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().Save(entity, client);
                    request.Succeeded = result > 0;
                    return result;
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Copies over Product info into the CodeSheet_Mastercodesheet_Bridge data table
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="fromID">the pub ID from where you are copying over</param>
        /// <param name="toID">the pub ID of where you are copying to</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Copy(Guid accessKey, int fromID, int toID, KMPlatform.Object.ClientConnections client)
        {
            var param = $" FromID:{fromID} ToID:{toID}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodCopy,
                WorkerFunc = _ => new BusinessLogicWorker().Copy(client, fromID, toID)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Updates the publications lock status to the Products and sets that it was updated by the user ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="userID">the user ID</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> UpdateLock(Guid accessKey, KMPlatform.Object.ClientConnections client, int userID)
        {
            var param = $" UserID: {userID}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodUpdateLock,
                WorkerFunc = _ => new BusinessLogicWorker().UpdateLock(client, userID)
            };

            return GetResponse(model);
        }
    }
}
