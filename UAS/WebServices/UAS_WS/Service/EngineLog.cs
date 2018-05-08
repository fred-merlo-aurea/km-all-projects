using System;
using System.Collections.Generic;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using UAS_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAS.BusinessLogic.EngineLog;
using EngineEnums = FrameworkUAS.BusinessLogic.Enums.Engine;
using EntityEngineLog = FrameworkUAS.Entity.EngineLog;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class EngineLog : FrameworkServiceBase, IEngineLog
    {
        private const string EntityName = "EngineLog";
        private const string MethodUpdateIsRunning = "UpdateIsRunning";
        private const string MethodUpdateRefreshClientIdEngine = "UpdateRefreshClientIdEngine";
        private const string MethodUpdateIsRunningClientIdEngineEnum = "UpdateIsRunningClientIdEngineEnum";
        private const string MethodSelectAll = "SelectAll";
        private const string MethodSelectClientId = "SelectClientId";
        private const string MethodSelectIsRunning = "SelectIsRunning";
        private const string MethodSelect = "Select";
        private const string MethodSave = "Save";
        private const string MethodUpdateRefresh = "UpdateRefresh";
        private const string MethodUpdateRefreshClientIdEngineEnum = "UpdateRefreshClientIdEngineEnum";
        private const string MethodUpdateIsRunningClientIdEngine = "UpdateIsRunningClientIdEngine";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        public Response<List<EntityEngineLog>> SelectAll(Guid accessKey)
        {
            var model = new ServiceRequestModel<List<EntityEngineLog>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectAll,
                WorkerFunc = _ => new BusinessLogicWorker().Select()
            };

            return GetResponse(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public Response<List<EntityEngineLog>> SelectClientId(Guid accessKey, int clientId)
        {
            var param = $"ClientId:{clientId}";
            var model = new ServiceRequestModel<List<EntityEngineLog>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectClientId,
                WorkerFunc = _ => new BusinessLogicWorker().Select(clientId)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="isRunning"></param>
        /// <returns></returns>
        public Response<List<EntityEngineLog>> SelectIsRunning(Guid accessKey, bool isRunning)
        {
            var param = $"IsRunning:{isRunning}";
            var model = new ServiceRequestModel<List<EntityEngineLog>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectIsRunning,
                WorkerFunc = _ => new BusinessLogicWorker().Select(isRunning)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientId"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public Response<EntityEngineLog> Select(Guid accessKey, int clientId, string engine)
        {
            var param = $"ClientId:{clientId} Engine:{engine}";
            var model = new ServiceRequestModel<EntityEngineLog>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(clientId, engine)
            };

            return GetResponse(model);
        }

        public Response<bool> Save(Guid accessKey, EntityEngineLog entity)
        {
            var param = new JsonFunctions().ToJson(entity);
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().Save(entity);
                    request.Succeeded = result;
                    return result;
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="engineLogId"></param>
        /// <returns></returns>
        public Response<bool> UpdateRefresh(Guid accessKey, int engineLogId)
        {
            var param = $"EngineLogId:{engineLogId}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodUpdateRefresh,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().UpdateRefresh(engineLogId);
                    request.Succeeded = result;
                    return result;
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientId"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public Response<bool> UpdateRefreshClientIdEngine(Guid accessKey, int clientId, string engine)
        {
            var param = $"ClientId:{clientId} Engine:{engine}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodUpdateRefreshClientIdEngine,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().UpdateRefresh(clientId,engine);
                    request.Succeeded = result;
                    return result;
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientId"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public Response<bool> UpdateRefreshClientIdEngineEnum(Guid accessKey, int clientId, EngineEnums engine)
        {
            var param = $"ClientId:{clientId} Engine:{engine}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodUpdateRefreshClientIdEngineEnum,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().UpdateRefresh(clientId, engine);
                    request.Succeeded = result;
                    return result;
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="engineLogId"></param>
        /// <param name="isRunning"></param>
        /// <returns></returns>
        public Response<bool> UpdateIsRunning(Guid accessKey, int engineLogId, bool isRunning)
        {
            var param = $"EngineLogId:{engineLogId} IsRunning:{isRunning}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodUpdateIsRunning,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().UpdateIsRunning(engineLogId, isRunning);
                    request.Succeeded = result;
                    return result;
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientId"></param>
        /// <param name="engine"></param>
        /// <param name="isRunning"></param>
        /// <returns></returns>
        public Response<bool> UpdateIsRunningClientIdEngine(Guid accessKey, int clientId, string engine, bool isRunning)
        {
            var param = $"ClientId:{clientId} Engine:{engine} IsRunning:{isRunning}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodUpdateIsRunningClientIdEngine,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().UpdateIsRunning(clientId, engine, isRunning);
                    request.Succeeded = result;
                    return result;
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientId"></param>
        /// <param name="engine"></param>
        /// <param name="isRunning"></param>
        /// <returns></returns>
        public Response<bool> UpdateIsRunningClientIdEngineEnum(Guid accessKey, int clientId, EngineEnums engine, bool isRunning)
        {
            var param = $"ClientId:{clientId} Engine:{engine} IsRunning:{isRunning}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodUpdateIsRunningClientIdEngineEnum,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().UpdateIsRunning(clientId, engine, isRunning);
                    request.Succeeded = result;
                    return result;
                }
            };

            return GetResponse(model);
        }
    }
}
