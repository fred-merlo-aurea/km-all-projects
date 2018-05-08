using System;
using System.Collections.Generic;
using Core_AMS.Utilities;
using UAS_WS.Interface;
using FrameworkUAS.Service;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAS.BusinessLogic.AdHocDimensionGroup;
using EntityAdHocDimensionGroup = FrameworkUAS.Entity.AdHocDimensionGroup;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class AdHocDimensionGroup : FrameworkServiceBase, IAdHocDimensionGroup
    {
        private const string EntityName = "AdHocDimensionGroup";
        private const string MethodSaveBulkSqlInsert = "SaveBulkSqlInsert";
        private const string MethodSave = "Save";
        private const string MethodSelectByAdHocDimensionGroupId = "SelectByAdHocDimensionGroupId";
        private const string MethodSelect = "Select";

        /// <summary>
        /// Selects a list of AdHocDimensionGroup objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="includeCustomProperties">boolean to include the available custom properties to the AdHocDimensionGroup object</param>
        /// <returns>response.result will contain a list of AdHocDimensionGroup objects</returns>
        public Response<List<EntityAdHocDimensionGroup>> Select(Guid accessKey, bool includeCustomProperties = false)
        {
            var param = $"includeCustomProperties:{includeCustomProperties}";
            var model = new ServiceRequestModel<List<EntityAdHocDimensionGroup>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(includeCustomProperties)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of AdHocDimensionGroup objects based on the client ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <param name="includeCustomProperties">boolean to include the available custom properties to the AdHocDimensionGroup object</param>
        /// <returns>repsonse.result will contain a list of AdHocDimensionGroup objects</returns>
        public Response<List<EntityAdHocDimensionGroup>> Select(Guid accessKey, int clientID, bool includeCustomProperties = false)
        {
            var param = $"ClientID:{clientID} includeCustomProperties:{includeCustomProperties}";
            var model = new ServiceRequestModel<List<EntityAdHocDimensionGroup>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(clientID, includeCustomProperties)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of AdHocDimensionGroup objects based on the client ID and the adHoc dimension group name
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <param name="adHocDimensionGroupName">the adHoc dimension group name</param>
        /// <param name="includeCustomProperties">boolean to include the available custom properties to the AdHocDimensionGroup object</param>
        /// <returns>response.result will contain a list of AdHocDimensionGroup objects</returns>
        public Response<List<EntityAdHocDimensionGroup>> Select(Guid accessKey, int clientID, string adHocDimensionGroupName, bool includeCustomProperties = false)
        {
            var param = $"ClientID:{clientID} adHocDimensionGroupName:{adHocDimensionGroupName} includeCustomProperties:{includeCustomProperties}";
            var model = new ServiceRequestModel<List<EntityAdHocDimensionGroup>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(clientID, adHocDimensionGroupName, includeCustomProperties)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects an AdHocDimensionGroup object based on the client ID, the source file ID, and the adHoc dimension group name
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="adHocDimensionGroupName">the adHoc dimension group name</param>
        /// <param name="includeCustomProperties">boolean to include the available custom properties to the AdHocDimensionGroup object</param>
        /// <returns>response.result will contain an AdHocDimensionGroup object</returns>
        public Response<EntityAdHocDimensionGroup> Select(Guid accessKey, int clientID, int sourceFileID, string adHocDimensionGroupName, bool includeCustomProperties = false)
        {
            var param = $"ClientID:{clientID} sourceFileID:{sourceFileID} adHocDimensionGroupName:{adHocDimensionGroupName} includeCustomProperties:{includeCustomProperties}";
            var model = new ServiceRequestModel<EntityAdHocDimensionGroup>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(clientID, sourceFileID, adHocDimensionGroupName, includeCustomProperties)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of AdHocDimensionGroup objects based on the client ID and the source file ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="includeCustomProperties">boolean to include the available custom properties to the AdHocDimensionGroup object</param>
        /// <returns>response.result will contain a list of AdHocDimensionGroup objects</returns>
        public Response<List<EntityAdHocDimensionGroup>> Select(Guid accessKey, int clientID, int sourceFileID, bool includeCustomProperties = false)
        {
            var param = $"ClientID:{clientID} sourceFileID:{sourceFileID} includeCustomProperties:{includeCustomProperties}";
            var model = new ServiceRequestModel<List<EntityAdHocDimensionGroup>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(clientID, sourceFileID, includeCustomProperties)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects an AdHocDimensionGroup object based on the adHoc dimension group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="adHocDimensionGroupId">the adHoc dimension group ID</param>
        /// <param name="includeCustomProperties">boolean to include the available custom properties to the AdHocDimensionGroup object</param>
        /// <returns>response.result will contain an AdHocDimensionGroup object</returns>
        public Response<EntityAdHocDimensionGroup> SelectByAdHocDimensionGroupId(Guid accessKey, int adHocDimensionGroupId, bool includeCustomProperties = false)
        {
            var param = $"adHocDimensionGroupId:{adHocDimensionGroupId} includeCustomProperties:{includeCustomProperties}";
            var model = new ServiceRequestModel<EntityAdHocDimensionGroup>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectByAdHocDimensionGroupId,
                WorkerFunc = _ => new BusinessLogicWorker().SelectByAdHocDimensionGroupId(adHocDimensionGroupId, includeCustomProperties)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves an AdHocDimensionGroup object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntityAdHocDimensionGroup"/> object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Save(Guid accessKey, EntityAdHocDimensionGroup entity)
        {
            var param = new JsonFunctions().ToJson(entity);
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                AuthenticateFunc = auth => auth.IsAuthenticated && auth.IsKM,
                WorkerFunc = _ => new BusinessLogicWorker().Save(entity)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves AdHocDimensionGroup objects in a bulk of 10,000 inserted into a list
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="list">the AdHocDimensionGroup object list</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> SaveBulkSqlInsert(Guid accessKey, List<FrameworkUAS.Entity.AdHocDimensionGroup> list)
        {
            var param = string.Empty;
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSaveBulkSqlInsert,
                AuthenticateFunc = auth => auth.IsAuthenticated && auth.IsKM,
                WorkerFunc = _ => new BusinessLogicWorker().SaveBulkSqlInsert(list)
            };

            return GetResponse(model);
        }
    }
}
