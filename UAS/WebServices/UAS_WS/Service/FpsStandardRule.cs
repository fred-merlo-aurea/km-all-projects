using System;
using System.Collections.Generic;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using UAS_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAS.BusinessLogic.FpsStandardRule;
using EntityFpsStandardRule = FrameworkUAS.Entity.FpsStandardRule;

namespace UAS_WS.Service
{
    /// <summary>
    /// File Processing Schema standard rules defined and controlled by KM
    /// </summary>
    public class FpsStandardRule : FrameworkServiceBase, IFpsStandardRule
    {
        private const string EntityName = "FpsStandardRule";
        private const string MethodDelete = "Delete";
        private const string MethodSelect = "Select";
        private const string MethodSave = "Save";

        /// <summary>
        /// return list of KM Standard File Procssing Schema Rules
        /// </summary>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        public Response<List<EntityFpsStandardRule>> Select(Guid accessKey)
        {
            var model = new ServiceRequestModel<List<EntityFpsStandardRule>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select()
            };

            return GetResponse(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="entity">The <see cref="EntityFpsStandardRule"/> object.</param>
        /// <returns></returns>
        public Response<int> Save(Guid accessKey, EntityFpsStandardRule entity)
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
