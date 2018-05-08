using System;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using UAS_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = KMPlatform.BusinessLogic.UserLog;
using EntityUserLog = KMPlatform.Entity.UserLog;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class TransformationDetail : FrameworkServiceBase, ITransformationDetail
    {
        private const string EntityName = "UserLog";
        private const string MethodSave = "Save";

        /// <summary>
        /// Saves a user log
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntityUserLog"/> object</param>
        /// <returns>repsonse.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, EntityUserLog entity)
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
    }
}
