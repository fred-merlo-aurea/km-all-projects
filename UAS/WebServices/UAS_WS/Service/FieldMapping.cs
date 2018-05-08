using System;
using System.Collections.Generic;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using UAS_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAS.BusinessLogic.FieldMapping;
using EntityFieldMapping = FrameworkUAS.Entity.FieldMapping;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class FieldMapping : FrameworkServiceBase, IFieldMapping
    {
        private const string EntityName = "FieldMapping";
        private const string MethodSelect = "Select";
        private const string MethodSave = "Save";
        private const string MethodColumnReorder = "ColumnReorder";
        private const string MethodDelete = "Delete";
        private const string MethodDeleteMapping = "DeleteMapping";
        private const string MethodSelectForFieldMapping = "SelectForFieldMapping";

        /// <summary>
        /// Selects a list of FieldMapping objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of FieldMapping objects</returns>
        public Response<List<EntityFieldMapping>> Select(Guid accessKey)
        {
            var model = new ServiceRequestModel<List<EntityFieldMapping>>
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
        /// <param name="sourceFileID"></param>
        /// <param name="includeCustomProperties"></param>
        /// <returns></returns>
        public Response<List<EntityFieldMapping>> Select(Guid accessKey, int sourceFileID, bool includeCustomProperties = false)
        {
            var param = $"SourceFileID:{sourceFileID}";
            var model = new ServiceRequestModel<List<EntityFieldMapping>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(sourceFileID, includeCustomProperties)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a FieldMapping object based on the field mapping ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="fieldMappingId">the field mapping ID</param>
        /// <returns>response.result will contain a FieldMapping object</returns>
        public Response<EntityFieldMapping> SelectForFieldMapping(Guid accessKey, int fieldMappingId)
        {
            var param = $"FieldMappingID:{fieldMappingId}";
            var model = new ServiceRequestModel<EntityFieldMapping>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForFieldMapping,
                WorkerFunc = _ => new BusinessLogicWorker().SelectFieldMappingID(fieldMappingId)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of FieldMapping objects based on the client name
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientName">the client name</param>
        /// <returns>response.result will contain a list of FieldMapping objects</returns>
        public Response<List<EntityFieldMapping>> Select(Guid accessKey, string clientName)
        {
            var model = new ServiceRequestModel<List<EntityFieldMapping>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = $"clientName:{clientName}",
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(clientName)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of FieldMapping objects based on the client ID and the file name
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <param name="fileName">the file name</param>
        /// <returns>response.result will contain a list of FieldMapping objects</returns>
        public Response<List<EntityFieldMapping>> Select(Guid accessKey, int clientID, string fileName)
        {
            var model = new ServiceRequestModel<List<EntityFieldMapping>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = $"ClientID:{clientID} FileName:{fileName}",
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(clientID, fileName)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Savesa a FieldMapping object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntityFieldMapping"/> object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, EntityFieldMapping entity)
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
        /// Reorders the FieldMappingID column that have source file ID's equal to the given source file ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="SourceFileID">the source file ID</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> ColumnReorder(Guid accessKey, int SourceFileID)
        {
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = $"SourceFileID: {SourceFileID}",
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodColumnReorder,
                WorkerFunc = _ => new BusinessLogicWorker().ColumnReorder(SourceFileID)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Deletes a FieldMapping by the source file ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="SourceFileID">the source file ID</param>
        /// <returns>response.result will containa an integer</returns>
        public Response<int> Delete(Guid accessKey, int SourceFileID)
        {
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = $"SourceFileID:{SourceFileID}",
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodDelete,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().Delete(SourceFileID);
                    request.Succeeded = result >= 0;
                    return result;
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Deletes a FieldMapping based on the field mapping ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="FieldMappingID">the field mapping ID</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> DeleteMapping(Guid accessKey, int FieldMappingID)
        {
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = $"FieldMappingID:{FieldMappingID}",
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodDeleteMapping,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().DeleteMapping(FieldMappingID);
                    request.Succeeded = result >= 0;
                    return result;
                }
            };

            return GetResponse(model);
        }
    }
}
