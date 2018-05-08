using System;
using System.Collections.Generic;
using FrameworkUAS.Service;
using UAS_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAS.BusinessLogic.TransformationFieldMultiMap;
using EntityTransformationFieldMultiMap = FrameworkUAS.Entity.TransformationFieldMultiMap;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class TransformationFieldMultiMap : FrameworkServiceBase, ITransformationFieldMultiMap
    {
        private const string EntityName = "TransformationFieldMultiMap";
        private const string MethodDeleteByFieldMultiMapId = "DeleteByFieldMultiMapID";
        private const string MethodDeleteBySourceFileId = "DeleteBySourceFileID";
        private const string MethodDeleteBySourceFileIdAndFieldMultiMapId = "DeleteBySourceFileIDAndFieldMultiMapID";
        private const string MethodDeleteByFieldMappingId = "DeleteByFieldMappingID";
        private const string MethodSelect = "Select";
        private const string MethodSelectTransformationId = "SelectTransformationID";

        /// <summary>
        /// Selects a list of TransformationFieldMultiMap objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of TransformationMultiMap objects</returns>
        public Response<List<EntityTransformationFieldMultiMap>> Select(Guid accessKey)
        {
            var model = new ServiceRequestModel<List<EntityTransformationFieldMultiMap>>
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
        /// Selects a list of TransformationFieldMultiMap based on the transformation ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="transformationId">the transformation ID</param>
        /// <returns>response.result will contain a list of TransformationFieldMultiMap objects</returns>
        public Response<List<EntityTransformationFieldMultiMap>> SelectTransformationID(Guid accessKey, int transformationId)
        {
            var param = $"TransformationID:{transformationId}";
            var model = new ServiceRequestModel<List<EntityTransformationFieldMultiMap>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectTransformationId,
                WorkerFunc = request => new BusinessLogicWorker().SelectTransformationID(transformationId)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves the TransformationFieldMultiMap
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the transformation field multi map object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAS.Entity.TransformationFieldMultiMap x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAS.Entity.TransformationFieldMultiMap>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransformationFieldMultiMap", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.TransformationFieldMultiMap worker = new FrameworkUAS.BusinessLogic.TransformationFieldMultiMap();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(x);
                    if (response.Result >= 0)
                    {
                        response.Message = "Success";
                        response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success;
                    }
                    else
                    {
                        response.Message = "Error";
                        response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, "TransformationFieldMultiMap", "Save");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Deletes a TransformationFieldMultiMap based on the field multi map ID
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

        /// <summary>
        /// Deletes a TransformationFieldMultiMap based on the source file ID
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
        /// Deletes a TransformationFieldMultiMap based on the source file ID and the field multi map ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="SourceFileID">the source file ID</param>
        /// <param name="FieldMultiMapID">the field multi map ID</param>
        /// <returns>response.result will contain an intege</returns>
        public Response<int> DeleteBySourceFileIDAndFieldMultiMapID(Guid accessKey, int SourceFileID, int FieldMultiMapID)
        {
            var param = $"SourceFileID:{SourceFileID} FieldMultiMapID:{FieldMultiMapID}";
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodDeleteBySourceFileIdAndFieldMultiMapId,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().DeleteBySourceFileIDAndFieldMultiMapID(SourceFileID, FieldMultiMapID);
                    request.Succeeded = result >= 0;
                    return result;
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Deletes a TransformationFieldMultiMap based on the field mapping ID
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
    }
}
