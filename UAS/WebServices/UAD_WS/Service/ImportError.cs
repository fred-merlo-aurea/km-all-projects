using System;
using System.Collections.Generic;
using UAD_WS.Interface;
using FrameworkUAS.Service;
using KMPlatform.Object;
using WebServiceFramework;
using UtilityJsonFunctions = Core_AMS.Utilities.JsonFunctions;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.ImportError;
using EntityImportError = FrameworkUAD.Entity.ImportError;

namespace UAD_WS.Service
{
    public class ImportError : FrameworkServiceBase, IImportError
    {
        private const string EntityName = "ImportError";
        private const string MethodSaveBulkSqlInsert = "SaveBulkSqlInsert";
        private const string MethodSelect = "Select";
        private const string MethodSave = "Save";

        /// <summary>
        /// Selects a list of ImportError objects based on the process code, the source file ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="processCode">the process code</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of ImportError objects</returns>
        public Response<List<EntityImportError>> Select(Guid accessKey, string processCode, int sourceFileID, ClientConnections client)
        {
            var param = $" processCode:{processCode} sourceFileID:{sourceFileID}";
            var model = new ServiceRequestModel<List<EntityImportError>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(processCode, sourceFileID, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves the ImportError object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntityImportError"/> object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, EntityImportError entity, ClientConnections client)
        {
            var param = new UtilityJsonFunctions().ToJson(entity);
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
        /// Saves a list of ImportError objects for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="list">the list of ImportError objects</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> SaveBulkSqlInsert(Guid accessKey, List<FrameworkUAD.Entity.ImportError> list, KMPlatform.Object.ClientConnections client)
        {
            var param = string.Empty;
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSaveBulkSqlInsert,
                WorkerFunc = _ => new BusinessLogicWorker().SaveBulkSqlInsert(list, client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves a batch of 1000 ImportErrors at once for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the ImportError object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> SaveBulkSqlInsert(Guid accessKey, FrameworkUAD.Entity.ImportError x, KMPlatform.Object.ClientConnections client)
        {
            var param = new UtilityJsonFunctions().ToJson(x);
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSaveBulkSqlInsert,
                WorkerFunc = _ => new BusinessLogicWorker().SaveBulkSqlInsert(x, client)
            };

            return GetResponse(model);
        }
    }
}
