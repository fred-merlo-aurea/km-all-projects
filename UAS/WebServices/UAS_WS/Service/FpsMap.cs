using System;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using UAS_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAS.BusinessLogic.FpsMap;
using EntityFpsMap = FrameworkUAS.Entity.FpsMap;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class FpsMap : FrameworkServiceBase, IFpsMap
    {
        private const string EntityName = "FpsMap";
        private const string MethodDelete = "Delete";
        private const string MethodSave = "Save";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="entity">The <see cref="EntityFpsMap"/> object.</param>
        /// <returns></returns>
        public Response<int> Save(Guid accessKey, EntityFpsMap entity)
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
