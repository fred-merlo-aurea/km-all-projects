using System;
using System.Collections.Generic;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using UAS_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAS.BusinessLogic.FpsCustomRule;
using EntityFpsCustomRule = FrameworkUAS.Entity.FpsCustomRule;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class FpsCustomRule : FrameworkServiceBase, IFpsCustomRule
    {
        private const string EntityName = "FpsCustomRule";
        private const string MethodDelete = "Delete";
        private const string MethodSelectSourceFileId = "SelectSourceFileId";
        private const string MethodSave = "Save";
        private const string MethodSelectClientId = "SelectClientId";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public Response<List<EntityFpsCustomRule>> SelectClientId(Guid accessKey, int clientId)
        {
            var param = $"ClientId:{clientId}";
            var model = new ServiceRequestModel<List<EntityFpsCustomRule>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectClientId,
                WorkerFunc = _ => new BusinessLogicWorker().SelectClientId(clientId)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="sourceFileId"></param>
        /// <returns></returns>
        public Response<List<EntityFpsCustomRule>> SelectSourceFileId(Guid accessKey, int sourceFileId)
        {
            var model = new ServiceRequestModel<List<EntityFpsCustomRule>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = $"sourceFileId:{sourceFileId}",
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectSourceFileId,
                WorkerFunc = _ => new BusinessLogicWorker().SelectSourceFileId(sourceFileId)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="entity">The <see cref="EntityFpsCustomRule"/> object.</param>
        /// <returns></returns>
        public Response<int> Save(Guid accessKey, EntityFpsCustomRule entity)
        {
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = new JsonFunctions().ToJson(entity),
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().Save(entity);
                    request.Succeeded = result >= 0;
                    return result;
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="sourceFileId"></param>
        /// <returns></returns>
        public Response<bool> Delete(Guid accessKey, int sourceFileId)
        {
            var param = $"SourceFileID: {sourceFileId}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodDelete,
                WorkerFunc = _ => new BusinessLogicWorker().Delete(sourceFileId)
            };

            return GetResponse(model);
        }
    }
}
