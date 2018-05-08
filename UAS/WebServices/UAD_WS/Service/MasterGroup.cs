using System;
using System.Collections.Generic;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using KMPlatform.Object;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.MasterGroup;
using EntityMasterGroup = FrameworkUAD.Entity.MasterGroup;

namespace UAD_WS.Service
{
    public class MasterGroup : FrameworkServiceBase, IMasterGroup
    {
        private const string EntityName = "MasterGroup";
        private const string MethodDelete = "Delete";
        private const string MethodSave = "Save";
        private const string MethodSelect = "Select";

        /// <summary>
        /// Selects a list of MasterGroup objects based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of MasterGroup objects</returns>
        public Response<List<EntityMasterGroup>> Select(Guid accessKey, ClientConnections client)
        {
            var model = new ServiceRequestModel<List<EntityMasterGroup>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = request => new BusinessLogicWorker().Select(client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves the MasterGroup object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the MasterGroup object</param>
        /// <param name="client">the <see cref="EntityMasterGroup"/> object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, EntityMasterGroup entity, ClientConnections client)
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
        /// Deletes a MasterGroup object for the client based on the master group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="masterGroupID">the master group ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Delete(Guid accessKey, int masterGroupID, ClientConnections client)
        {
            var param = $" masterGroupID:{masterGroupID}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodDelete,
                WorkerFunc = _ => new BusinessLogicWorker().Delete(masterGroupID, client)
            };

            return GetResponse(model);
        }
    }
}
