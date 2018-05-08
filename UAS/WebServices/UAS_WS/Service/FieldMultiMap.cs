using System;
using System.Collections.Generic;
using UAS_WS.Interface;
using FrameworkUAS.Service;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAS.BusinessLogic.FieldMultiMap;
using UtilityJsonFunctions = Core_AMS.Utilities.JsonFunctions;
using EntityFieldMultiMap = FrameworkUAS.Entity.FieldMultiMap;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class FieldMultiMap : FrameworkServiceBase, IFieldMultiMap
    {
        private const string EntityName = "FieldMultiMap";
        private const string MethodDeleteByFieldMultiMapId = "DeleteByFieldMultiMapID";
        private const string MethodDeleteBySourceFileId = "DeleteBySourceFileID";
        private const string MethodDeleteByFieldMappingId = "DeleteByFieldMappingID";
        private const string MethodSave = "Save";
        private const string MethodSelectFieldMultiMapId = "SelectFieldMultiMapID";
        private const string MethodSelectFieldMappingId = "SelectFieldMappingID";
        private const string MethodSelect = "Select";

        /// <summary>
        /// Selects a list of FieldMultiMap objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of FieldMultiMap objects</returns>
        public Response<List<EntityFieldMultiMap>> Select(Guid accessKey)
        {
            var param = string.Empty;
            var model = new ServiceRequestModel<List<EntityFieldMultiMap>>
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
        /// Selects a list of FieldMultiMap objects based on the field mapping ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="fieldMappingID">the field mapping ID</param>
        /// <returns>response.result will containa a list of FieldMultiMap objects</returns>
        public Response<List<EntityFieldMultiMap>> SelectFieldMappingID(Guid accessKey, int fieldMappingID)
        {
            var param = $"fieldMappingID:{fieldMappingID}";
            var model = new ServiceRequestModel<List<EntityFieldMultiMap>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectFieldMappingId,
                WorkerFunc = _ => new BusinessLogicWorker().SelectFieldMappingID(fieldMappingID)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a FieldMultiMap object based on the field multi map ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="fieldMultiMapID">the field multi map ID</param>
        /// <returns>response.result will contain a FieldMultiMap object</returns>
        public Response<EntityFieldMultiMap> SelectFieldMultiMapID(Guid accessKey, int fieldMultiMapID)
        {
            var param = $"fieldMultiMapID:{fieldMultiMapID}";
            var model = new ServiceRequestModel<EntityFieldMultiMap>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectFieldMultiMapId,
                WorkerFunc = _ => new BusinessLogicWorker().SelectFieldMultiMapID(fieldMultiMapID)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves the FieldMultiMap object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the FieldMultiMap object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, EntityFieldMultiMap x)
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
        /// Deletes a FieldMulitMap object based on the source file ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="SourceFileID">the source file ID</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> DeleteBySourceFileID(Guid accessKey, int SourceFileID)
        {
            var param = $"SourceFileID:{SourceFileID}";
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodDeleteBySourceFileId,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().DeleteBySourceFileID(SourceFileID);
                    request.Succeeded = result >= 0;
                    return result;
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Deletes a FieldMultiMap object based on the field mapping ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="FieldMappingID">the field mapping ID</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> DeleteByFieldMappingID(Guid accessKey, int FieldMappingID)
        {
            var param = $"FieldMappingID:{FieldMappingID}";
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodDeleteByFieldMappingId,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().DeleteByFieldMappingID(FieldMappingID);
                    request.Succeeded = result >= 0;
                    return result;
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Deletes a FieldMultiMap object based on the field multi map ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="FieldMultiMapID">the field multi map ID</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> DeleteByFieldMultiMapID(Guid accessKey, int FieldMultiMapID)
        {
            var param = $"FieldMultiMapID:{FieldMultiMapID}";
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodDeleteByFieldMultiMapId,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().DeleteByFieldMultiMapID(FieldMultiMapID);
                    request.Succeeded = result >= 0;
                    return result;
                }
            };

            return GetResponse(model);
        }
    }
}
