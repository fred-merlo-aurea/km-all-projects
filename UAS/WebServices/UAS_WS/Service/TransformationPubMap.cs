using System;
using System.Collections.Generic;
using FrameworkUAS.Service;
using UAS_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAS.BusinessLogic.TransformationPubMap;
using EntityTransformationPubMap = FrameworkUAS.Entity.TransformationPubMap;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class TransformationPubMap : FrameworkServiceBase, ITransformationPubMap
    {
        private const string EntityName = "TransformationPubMap";
        private const string MethodDelete = "Delete";
        private const string MethodSelect = "Select";

        /// <summary>
        /// Selects a list of TransformationPubMap objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of TransformationPubMap objects</returns>
        public Response<List<EntityTransformationPubMap>> Select(Guid accessKey)
        {
            var model = new ServiceRequestModel<List<EntityTransformationPubMap>>
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
        /// Selects a list of TransformationPubMap objects based on the transformation ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="transformationId">the transformation ID</param>
        /// <returns>response.result will contain a list of TransformationPubMap objects</returns>
        public Response<List<EntityTransformationPubMap>> Select(Guid accessKey, int transformationId)
        {
            var param = $"TransformationID:{transformationId}";
            var model = new ServiceRequestModel<List<EntityTransformationPubMap>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = request => new BusinessLogicWorker().Select(transformationId)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves the TransformationPubMap
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the transformation pub map object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAS.Entity.TransformationPubMap x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAS.Entity.TransformationPubMap>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransformationPubMap", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.TransformationPubMap worker = new FrameworkUAS.BusinessLogic.TransformationPubMap();
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
                LogError(accessKey, ex, "TransformationPubMap", "Save");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Deletes a TransformationPubMap based on the transformation ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="transformationID">the transformation ID</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Delete(Guid accessKey, int transformationID)
        {
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = $"TransformationID:{transformationID}",
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

        /// <summary>
        /// Deletes a TransformationPubMap based on the transformation ID and the pub ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="transformationID">the transformation ID</param>
        /// <param name="pubID">the pub ID</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Delete(Guid accessKey, int transformationID, int pubID)
        {
            var param = $"TransformationID:{transformationID} PubID:{pubID}";
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodDelete,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().Delete(transformationID, pubID);
                    request.Succeeded = result >= 0;
                    return result;
                }
            };

            return GetResponse(model);
        }
    }
}
