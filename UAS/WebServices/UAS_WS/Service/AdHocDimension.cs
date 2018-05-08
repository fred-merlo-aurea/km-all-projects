using System;
using System.Collections.Generic;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using UAS_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAS.BusinessLogic.AdHocDimension;
using EntityAdHocDimension = FrameworkUAS.Entity.AdHocDimension;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class AdHocDimension : FrameworkServiceBase, IAdHocDimension
    {
        private const string EntityName = "AdHocDimension";
        private const string MethodDelete = "Delete";
        private const string MethodSelect = "Select";
        private const string MethodSaveBulkSqlInsert = "SaveBulkSqlInsert";

        /// <summary>
        /// Selects a list of AdHocDimension objects based on the adHoc dimension group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="adHocDimensionGroupId">the adHoc dimension group ID</param>
        /// <returns>response.result will contain a list of AdHocDimension objects</returns>
        public Response<List<EntityAdHocDimension>> Select(Guid accessKey, int adHocDimensionGroupId)
        {
            var param = adHocDimensionGroupId.ToString();
            var model = new ServiceRequestModel<List<EntityAdHocDimension>>
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
        /// Deletes an AdHocDimension object based on the source file ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Delete(Guid accessKey, int sourceFileID)
        {
            var param = $"SourceFileID: {sourceFileID}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodDelete,
                WorkerFunc = _ => new BusinessLogicWorker().Delete(sourceFileID)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves AdHocDimension objects in a bulk of 10,000 inserted into a list
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="list">the AdHocDimension objet list</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> SaveBulkSqlInsert(Guid accessKey, List<EntityAdHocDimension> list)
        {
            var param = new JsonFunctions().ToJson(list);
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSaveBulkSqlInsert,
                WorkerFunc = _ => new BusinessLogicWorker().SaveBulkSqlInsert(list)
            };

            return GetResponse(model);
        }
    }
}
