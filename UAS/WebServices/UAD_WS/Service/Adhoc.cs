using System;
using System.Collections.Generic;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using KMPlatform.Object;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.Adhoc;
using EntityAdhoc = FrameworkUAD.Entity.Adhoc;

namespace UAD_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class Adhoc : FrameworkServiceBase, IAdhoc
    {
        private const string EntityName = "Adhoc";
        private const string MethodDeleteAdhoc = "Delete_AdHoc";
        private const string MethodDelete = "Delete";
        private const string MethodSave = "Save";
        private const string MethodSelectCategoryId = "SelectCategoryID";
        private const string MethodSelectAll = "SelectAll";

        /// <summary>
        /// Selects all of the Adhoc objects for the specified Client object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the Client object</param>
        /// <returns>response.result will contain a list of Adhoc objects</returns>
        public Response<List<EntityAdhoc>> SelectAll(Guid accessKey, ClientConnections client)
        {
            var model = new ServiceRequestModel<List<EntityAdhoc>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectAll,
                WorkerFunc = _ => new BusinessLogicWorker().SelectAll(client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of Adhoc objects for the specified Client object and the category ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the Client object</param>
        /// <param name="categoryID">the category ID</param>
        /// <returns>response.result will contain a list of Adhoc objects</returns>
        public Response<List<EntityAdhoc>> SelectCategoryID(Guid accessKey, ClientConnections client, int categoryID)
        {
            var param = $" CategoryID: {categoryID}";
            var model = new ServiceRequestModel<List<EntityAdhoc>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectCategoryId,
                WorkerFunc = _ => new BusinessLogicWorker().SelectCategoryID(categoryID, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves the given Adhoc object for the Client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the Client object</param>
        /// <param name="entity">the <see cref="EntityAdhoc"/> object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, ClientConnections client, EntityAdhoc entity)
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
        /// Deletes the Adhoc object for the client based on the category ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the Client object</param>
        /// <param name="categoryID">the category ID</param>
        /// <returns>repsonse.result will contain a boolean</returns>
        public Response<bool> Delete(Guid accessKey, KMPlatform.Object.ClientConnections client, int categoryID)
        {
            var param = $" CategoryID: {categoryID}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodDelete,
                WorkerFunc = _ => new BusinessLogicWorker().Delete(categoryID, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Deletes the Adhoc object for the client based on the Adhoc ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the Client object</param>
        /// <param name="adhocID">the Adhoc ID</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Delete_AdHoc(Guid accessKey, KMPlatform.Object.ClientConnections client, int adhocID)
        {
            var param = $" AdHocID: {adhocID}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodDeleteAdhoc,
                WorkerFunc = _ => new BusinessLogicWorker().Delete_AdHoc(adhocID, client)
            };

            return GetResponse(model);
        }
    }
}
