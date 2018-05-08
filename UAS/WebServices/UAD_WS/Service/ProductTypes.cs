using System;
using System.Collections.Generic;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using KMPlatform.Object;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.ProductTypes;
using EntityProductTypes = FrameworkUAD.Entity.ProductTypes;

namespace UAD_WS.Service
{
    public class ProductTypes : FrameworkServiceBase, IProductTypes
    {
        private const string EntityName = "ProductTypes";
        private const string MethodDelete = "Delete";
        private const string MethodSave = "Save";
        private const string MethodSelect = "Select";

        /// <summary>
        /// Selects a list of ProductTypes objects based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of ProductTypes objects</returns>
        public Response<List<EntityProductTypes>> Select(Guid accessKey, ClientConnections client)
        {
            var model = new ServiceRequestModel<List<EntityProductTypes>>
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
        /// Saves a ProductTypes object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntityProductTypes"/> object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, EntityProductTypes entity, KMPlatform.Object.ClientConnections client)
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
        /// Deletes a ProductTypes object by the pub type ID for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="pubTypeID">the pub type ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Delete(Guid accessKey, int pubTypeID, KMPlatform.Object.ClientConnections client)
        {
            var param = $" ProductTypeID:{pubTypeID}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodDelete,
                WorkerFunc = _ => new BusinessLogicWorker().Delete(client, pubTypeID)
            };

            return GetResponse(model);
        }
    }
}
