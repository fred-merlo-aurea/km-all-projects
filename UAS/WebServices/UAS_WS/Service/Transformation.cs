using System;
using System.Collections.Generic;
using UAS_WS.Interface;
using FrameworkUAS.Service;
using WebServiceFramework;
using UtilityJsonFunctions = Core_AMS.Utilities.JsonFunctions;
using BusinessLogicWorker = FrameworkUAS.BusinessLogic.Transformation;
using EntityTransformation = FrameworkUAS.Entity.Transformation;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class Transformation : FrameworkServiceBase, ITransformation
    {
        private const string EntityName = "Transformation";
        private const string MethodSelect = "Select";
        private const string MethodSelectAssigned = "SelectAssigned";
        private const string MethodDelete = "Delete";
        private const string MethodSave = "Save";

        /// <summary>
        /// Selects a list of Transformation objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>repsonse.result will contain a list of Transformation objects</returns>
        public Response<List<EntityTransformation>> Select(Guid accessKey)
        {
            var param = string.Empty;
            var model = new ServiceRequestModel<List<EntityTransformation>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select()
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of Transformation objects based on the transformation ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="transformationID">the transformation ID</param>
        /// <returns>response.result will contain a list of Transformation objects</returns>
        public Response<List<EntityTransformation>> Select(Guid accessKey, int transformationID)
        {
            var param = $"TransformationID:{transformationID}";
            var model = new ServiceRequestModel<List<EntityTransformation>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = request => new BusinessLogicWorker().Select(transformationID)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of Transformation objects based on the client ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <param name="includeCustomProperties">boolean to include the custom properties from the client for the transformation</param>
        /// <returns>response.result will contain a list of Transformation objects</returns>
        public Response<List<EntityTransformation>> Select(Guid accessKey, int clientID, bool includeCustomProperties = false)
        {
            var param = $"ClientID:{clientID} IncludeCustomProperties:{includeCustomProperties}";
            var model = new ServiceRequestModel<List<EntityTransformation>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = request => new BusinessLogicWorker().SelectClient(clientID, includeCustomProperties)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of Transformation objects based on the client ID, and the source file ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="includeCustomProperties">boolean to include custom properties from the client for the transformation</param>
        /// <returns>response.result will contain a list of Transformation objects</returns>
        public Response<List<EntityTransformation>> Select(Guid accessKey, int clientID, int sourceFileID, bool includeCustomProperties = false)
        {
            var param = $"ClientID:{clientID}SourceFileID:{sourceFileID} IncludeCustomProperties:{includeCustomProperties}";
            var model = new ServiceRequestModel<List<EntityTransformation>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = request => new BusinessLogicWorker().Select(clientID, sourceFileID, includeCustomProperties)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of Transformation objects based on the assigned mappings in a file
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="fileName">the file name</param>
        /// <returns>response.result will contain a list of Transformation objects</returns>
        public Response<List<EntityTransformation>> SelectAssigned(Guid accessKey, string fileName)
        {
            var param = $"FileName:{fileName}";
            var model = new ServiceRequestModel<List<EntityTransformation>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectAssigned,
                WorkerFunc = request => new BusinessLogicWorker().SelectAssigned(fileName)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of Transformation objects based on the assigned mappings by the field mapping ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="fieldMappingID">the field mapping ID</param>
        /// <returns>resposne.result will contain a list of Transformation objects</returns>
        public Response<List<FrameworkUAS.Entity.Transformation>> SelectAssigned(Guid accessKey, int fieldMappingID)
        {
            var param = $"FieldMappingID:{fieldMappingID}";
            var model = new ServiceRequestModel<List<EntityTransformation>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectAssigned,
                WorkerFunc = request => new BusinessLogicWorker().SelectAssigned(fieldMappingID)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of Transformation objects based on the transformation name and the name of the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="transformationName">the transformation name</param>
        /// <param name="clientName">the client name</param>
        /// <returns>response.result will contain a list of Transformation objects</returns>
        public Response<List<EntityTransformation>> Select(Guid accessKey, string transformationName, string clientName)
        {
            var param = $"TransformationName:{transformationName} ClientName:{clientName}";
            var model = new ServiceRequestModel<List<EntityTransformation>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = request => new BusinessLogicWorker().SelectTransformationID(transformationName, clientName)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves the Transformation
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the Transformation object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAS.Entity.Transformation x)
        {
            var param = new UtilityJsonFunctions().ToJson(x);
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().Save(x);
                    request.Succeeded = result >= 0;
                    return result;
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Deletes a Transformation based on the tranformation ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="transformationID">the transformation ID</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Delete(Guid accessKey, int transformationID)
        {
            var param = $"TransformationID:{transformationID}";
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodDelete,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().Delete(transformationID);
                    request.Succeeded = result >= 0;
                    return result;
                }
            };

            return GetResponse(model);
        }
    }
}
