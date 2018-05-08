using System;
using System.Collections.Generic;
using Core_AMS.Utilities;
using UAS_WS.Interface;
using FrameworkUAS.Service;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAS.BusinessLogic.AdHocDimensionGroupPubcodeMap;
using EntityAdHocDimensionGroupPubCodeMap = FrameworkUAS.Entity.AdHocDimensionGroupPubcodeMap;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class AdHocDimensionGroupPubcodeMap : FrameworkServiceBase, IAdHocDimensionGroupPubcodeMap
    {
        private const string EntityName = "AdHocDimensionGroupPubcodeMap";
        private const string MethodSaveBulkSqlInsert = "SaveBulkSqlInsert";
        private const string MethodSave = "Save";
        private const string MethodSelect = "Select";

        /// <summary>
        /// Selects a list of AdHocDimensionGroupPubcodeMap objects based on the adHoc dimension group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="adHocDimensionGroupId">the adHoc dimension group ID</param>
        /// <returns>response.result will contain a list of AdHocDimensionGroupPubcodeMap objects</returns>
        public Response<List<EntityAdHocDimensionGroupPubCodeMap>> Select(Guid accessKey, int adHocDimensionGroupId)
        {
            var param = $"adHocDimensionGroupId:{adHocDimensionGroupId}";
            var model = new ServiceRequestModel<List<EntityAdHocDimensionGroupPubCodeMap>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(adHocDimensionGroupId)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves a AdHocDimensionGroupPubcodeMap object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntityAdHocDimensionGroupPubCodeMap"/> object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Save(Guid accessKey, EntityAdHocDimensionGroupPubCodeMap entity)
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
        /// Saves AdHocDimensionGroupPubcodeMap objects in a bulk of 10,000 inserted into a list
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="list">the AdHocDimensionGroupPubcodeMap object list</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> SaveBulkSqlInsert(Guid accessKey, List<FrameworkUAS.Entity.AdHocDimensionGroupPubcodeMap> list)
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
