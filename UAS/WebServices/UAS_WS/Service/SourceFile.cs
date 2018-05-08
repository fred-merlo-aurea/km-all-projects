using System;
using System.Collections.Generic;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using UAS_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAS.BusinessLogic.SourceFile;
using EntitySourceFile = FrameworkUAS.Entity.SourceFile;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class SourceFile : FrameworkServiceBase, ISourceFile
    {
        private const string EntityName = "SourceFile";
        private const string MethodSelect = "Select";
        private const string MethodSelectSpecialFiles = "SelectSpecialFiles";
        private const string MethodDelete = "Delete";
        private const string MethodIsFileNameUnique = "IsFileNameUnique";
        private const string MethodSave = "Save";
        private const string MethodSelectForSourceFile = "SelectForSourceFile";

        /// <summary>
        /// Selects a list of SourceFile objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="includeCustomProperties">boolean to include the custom properties from the field mappings</param>
        /// <returns>response.result will contain a list of SourceFile objects</returns>
        public Response<List<EntitySourceFile>> Select(Guid accessKey, bool includeCustomProperties = false)
        {
            var model = new ServiceRequestModel<List<EntitySourceFile>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = request => new BusinessLogicWorker().Select(includeCustomProperties)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of SourceFile objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="includeCustomProperties">boolean to include the custom properties from the field mappings</param>
        /// <param name="isDeleted">boolean if file has been deleted</param>
        /// <returns>response.result will contain a list of SourceFile objects</returns>
        public Response<List<EntitySourceFile>> Select(Guid accessKey, bool includeCustomProperties = false, bool isDeleted = false)
        {
            var param = $"IncludeCustomProperties:{includeCustomProperties} IsDeleted:{isDeleted}";
            var model = new ServiceRequestModel<List<EntitySourceFile>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = request => new BusinessLogicWorker().Select(includeCustomProperties, isDeleted)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of special SourceFile objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="includeCustomProperties">boolean to include the custom properties from the field mappings </param>
        /// <returns>response.result will contain a list of SourceFile objects</returns>
        public Response<List<EntitySourceFile>> SelectSpecialFiles(Guid accessKey, bool includeCustomProperties = false)
        {
            var param = $"IncludeCustomProperties:{includeCustomProperties}";
            var model = new ServiceRequestModel<List<EntitySourceFile>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectSpecialFiles,
                WorkerFunc = request => new BusinessLogicWorker().SelectSpecialFiles(includeCustomProperties)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of special SourceFile objects based on the client ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <param name="includeCustomProperties">boolean to include the custom properties from the field mappings</param>
        /// <returns>response.result will contain a list of SourceFile objects</returns>
        public Response<List<EntitySourceFile>> SelectSpecialFiles(Guid accessKey, int clientID, bool includeCustomProperties = false)
        {
            var param = $"IncludeCustomProperties:{includeCustomProperties} clientID:{clientID}";
            var model = new ServiceRequestModel<List<EntitySourceFile>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectSpecialFiles,
                WorkerFunc = request => new BusinessLogicWorker().SelectSpecialFiles(clientID, includeCustomProperties)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of SourceFile objects based on the client ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <param name="includeCustomProperties">boolean to include the custom properties from the field mappings</param>
        /// <param name="isDeleted">boolean if file has been deleted</param>
        /// <returns>response.result will contain a list of SourceFile objects</returns>
        public Response<List<EntitySourceFile>> Select(Guid accessKey, int clientID, bool includeCustomProperties = false, bool isDeleted = false)
        {
            var param = $"ClientID:{clientID} IncludeCustomProperties:{includeCustomProperties} IsDeleted:{isDeleted}";
            var model = new ServiceRequestModel<List<EntitySourceFile>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = request => new BusinessLogicWorker().Select(clientID, includeCustomProperties, isDeleted)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of SourceFile objects based on the client ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <param name="includeCustomProperties">boolean to include custom properties from the field mappings</param>
        /// <returns>response.result will contain a list of SourceFile objects</returns>
        public Response<List<EntitySourceFile>> Select(Guid accessKey, int clientID, bool includeCustomProperties = false)
        {
            var param = $"ClientID:{clientID} IncludeCustomProperties:{includeCustomProperties}";
            var model = new ServiceRequestModel<List<EntitySourceFile>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = request => new BusinessLogicWorker().Select(clientID, includeCustomProperties)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a SourceFile object based on the client name, and the file name
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientName">the client name</param>
        /// <param name="fileName">the file name</param>
        /// <param name="includeCustomProperties">boolean to include the custom properties from the field mappings</param>
        /// <returns>response.result will contain a SourceFile object</returns>
        public Response<EntitySourceFile> Select(Guid accessKey, string clientName, string fileName, bool includeCustomProperties = false)
        {
            var param = $"ClientName:{clientName} FileName:{fileName} IncludeCustomProperties:{includeCustomProperties}";
            var auth = Authenticate(accessKey, false, param, EntityName, MethodSelect);
            var model = new ServiceRequestModel<EntitySourceFile>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = request => new BusinessLogicWorker().Select(auth.Client.ClientID, fileName, includeCustomProperties)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a SourceFile objcet based on the source file ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="includeCustomProperties">boolean to include the custom properties from the field mappings</param>
        /// <returns>response.result will contain a SourceFile object</returns>
        public Response<EntitySourceFile> SelectForSourceFile(Guid accessKey, int sourceFileID, bool includeCustomProperties = false)
        {
            var param = $"SourceFileID:{sourceFileID} IncludeCustomProperties:{includeCustomProperties}";
            var model = new ServiceRequestModel<EntitySourceFile>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForSourceFile,
                WorkerFunc = request => new BusinessLogicWorker().SelectSourceFileID(sourceFileID, includeCustomProperties)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves the SourceFile object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntitySourceFile"/> object</param>
        /// <param name="defaultRules">will default Rules be applied to SourceFile</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, EntitySourceFile entity, bool defaultRules = true)
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
                    var result = new BusinessLogicWorker().Save(entity, defaultRules);
                    request.Succeeded = result >= 0;
                    return result;
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Deletes a SourceFile object based on the source file ID, and the client ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="clientID">the client ID</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Delete(Guid accessKey, int sourceFileID, int clientID)
        {
            var param = $"SourceFileID:{sourceFileID} ClientID:{clientID}";
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodDelete,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().Delete(sourceFileID, clientID);
                    request.Succeeded = result >= 0;
                    return result;
                }
            };

            return GetResponse(model);
        }

        public Response<bool> IsFileNameUnique(Guid accessKey, int clientId, string fileName)
        {
            var param = $"ClientId:{clientId} FileName:{fileName}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodIsFileNameUnique,
                WorkerFunc = request => new BusinessLogicWorker().IsFileNameUnique(clientId, fileName)
            };

            return GetResponse(model);
        }
    }
}
