using System;
using System.Collections.Generic;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using KMPlatform.Object;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.AdhocCategory;
using EntityAdhocCategory = FrameworkUAD.Entity.AdhocCategory;

namespace UAD_WS.Service
{   
    public class AdhocCategory : FrameworkServiceBase, IAdhocCategory
    {
        private const string EntityName = "AdhocCategory";
        private const string MethodSelectAll = "SelectAll";
        private const string MethodSave = "Save";

        /// <summary>
        /// Selects all of the AdhocCategory objects for the specified Client object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the Client object</param>
        /// <returns>response.result will contain a list of AdhocCategory objects</returns>
        public Response<List<EntityAdhocCategory>> SelectAll(Guid accessKey, ClientConnections client)
        {
            var model = new ServiceRequestModel<List<EntityAdhocCategory>>
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
        /// Saves the given AdhocCategory object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the Client object</param>
        /// <param name="entity">the <see cref="EntityAdhocCategory"/> object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, ClientConnections client, EntityAdhocCategory entity)
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
    }
}
