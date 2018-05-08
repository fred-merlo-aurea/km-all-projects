using System;
using System.Collections.Generic;
using Core_AMS.Utilities;
using UAS_WS.Interface;
using FrameworkUAS.Service;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAS.BusinessLogic.TransformationFieldMap;
using EntityTransformationFieldMap = FrameworkUAS.Entity.TransformationFieldMap;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class TransformationFieldMap : FrameworkServiceBase, ITransformationFieldMap
    {
        private const string EntityName = "TransformationFieldMap";
        private const string MethodDeleteFieldMapping = "DeleteFieldMapping";
        private const string MethodDeleteSourceFile = "DeleteSourceFile";
        private const string MethodDelete = "Delete";
        private const string MethodDeleteTransformationFieldMapping = "DeleteTransformationFieldMapping";
        private const string MethodSave = "Save";
        private const string MethodSelect = "Select";

        /// <summary>
        /// Selects a list of TransformationFieldMap objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of TransformationFieldMap objects</returns>
        public Response<List<EntityTransformationFieldMap>> Select(Guid accessKey)
        {
            var model = new ServiceRequestModel<List<EntityTransformationFieldMap>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = request => new BusinessLogicWorker().Select()
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of TransformationFieldMap objects based on the transformation ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="transformationID">the transformation ID</param>
        /// <returns>response.result will contain a list of TransformationFieldMap objects</returns>
        public Response<List<EntityTransformationFieldMap>> Select(Guid accessKey, int transformationID)
        {
            var param = $"TransformationID:{transformationID}";
            var model = new ServiceRequestModel<List<EntityTransformationFieldMap>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = request => new BusinessLogicWorker().SelectTransformationID(transformationID)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves the TransformationFieldMap
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntityTransformationFieldMap"/> object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, EntityTransformationFieldMap entity)
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
                    var result = new BusinessLogicWorker().Save(entity);
                    request.Succeeded = result >= 0;
                    return result;
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Deletes a TransformationFieldMap based on the transformation name, client name and column name
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="transformationName">the transformation name</param>
        /// <param name="clientID">the client name</param>
        /// <param name="columnName">the column name</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Delete(Guid accessKey, string transformationName, int clientID, string columnName)
        {
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = $"TransformationName:{transformationName} ClientID:{clientID} ColumnName:{columnName}",
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodDelete,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().Delete(transformationName, clientID, columnName);
                    request.Succeeded = result >= 0;
                    return result;
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Deletes a TransformFieldMap based on the source file ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> DeleteSourceFile(Guid accessKey, int sourceFileID)
        {
            var param = $"SourceFileID:{sourceFileID}";
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodDeleteSourceFile,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().DeleteSourceFileID(sourceFileID);
                    request.Succeeded = result >= 0;
                    return result;
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Deletes a TransformationFieldMap based on the field mapping ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="fieldMappingID">the field mapping ID</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> DeleteFieldMapping(Guid accessKey, int fieldMappingID)
        {
            var param = $"FieldMappingID:{fieldMappingID}";
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodDeleteFieldMapping,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().DeleteFieldMappingID(fieldMappingID);
                    request.Succeeded = result >= 0;
                    return result;
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Deletes a TransformatinoFieldMap based on the transformation name, the client name, and the field mapping ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="transformationName">the transformation name</param>
        /// <param name="clientID">the integer id number of the client</param>
        /// <param name="fieldMappingID">the field mapping ID</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> DeleteTransformationFieldMapping(Guid accessKey, string transformationName, int clientID, int fieldMappingID)
        {
            var param = $"TransformationName:{transformationName} ClientID:{clientID} FiledMappingID:{fieldMappingID}";
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodDeleteTransformationFieldMapping,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().DeleteFieldMappingID(transformationName, clientID, fieldMappingID);
                    request.Succeeded = result >= 0;
                    return result;
                }
            };

            return GetResponse(model);
        }
    }
}
