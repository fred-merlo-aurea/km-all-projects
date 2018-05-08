using System;
using System.Collections.Generic;
using FrameworkUAS.Service;
using UAS_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAS.BusinessLogic.TransformDataMap;
using EntityTransformDataMap = FrameworkUAS.Entity.TransformDataMap;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class TransformDataMap : FrameworkServiceBase, ITransformDataMap
    {
        private const string EntityName = "TransformDataMap";
        private const string MethodSelectForSourceFile = "SelectForSourceFile";
        private const string MethodDelete = "Delete";

        /// <summary>
        /// Selects a list of TransformDataMap objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of TransformDataMap objects</returns>
        public Response<List<FrameworkUAS.Entity.TransformDataMap>> Select(Guid accessKey)
        {
            Response<List<FrameworkUAS.Entity.TransformDataMap>> response = new Response<List<FrameworkUAS.Entity.TransformDataMap>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "TransformDataMap", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.TransformDataMap worker = new FrameworkUAS.BusinessLogic.TransformDataMap();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select();
                    if (response.Result != null)
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
                LogError(accessKey, ex, "TransformDataMap", "Select");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a list of TransformDataMap objects based on the source file ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="sourceFileId">the source file ID</param>
        /// <returns>response.result will contain a list of TransformDataMap objects</returns>
        public Response<List<EntityTransformDataMap>> SelectForSourceFile(Guid accessKey, int sourceFileId)
        {
            var param = $"SourceFileID:{sourceFileId}";
            var model = new ServiceRequestModel<List<EntityTransformDataMap>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForSourceFile,
                WorkerFunc = _ => new BusinessLogicWorker().SelectSourceFileID(sourceFileId)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of TransformDataMap objects based on the transformation ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="transformationID">the transformation ID</param>
        /// <returns>response.result will contain a list of TransformDataMap objects</returns>
        public Response<List<FrameworkUAS.Entity.TransformDataMap>> SelectForTransformation(Guid accessKey, int transformationID)
        {
            Response<List<FrameworkUAS.Entity.TransformDataMap>> response = new Response<List<FrameworkUAS.Entity.TransformDataMap>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "transformationID:" + transformationID.ToString(), "TransformDataMap", "SelectForTransformation");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.TransformDataMap worker = new FrameworkUAS.BusinessLogic.TransformDataMap();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(transformationID);
                    if (response.Result != null)
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
                LogError(accessKey, ex, "TransformDataMap", "SelectForTransformation");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Deletes a TransformDataMap based on the transform data map ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="transformDataMapId">the transform data map ID</param>
        /// <returns>response.result will contain a list of TransformDataMap objects</returns>
        public Response<List<EntityTransformDataMap>> Delete(Guid accessKey, int transformDataMapId)
        {
            var param = $"TransformDataMapID:{transformDataMapId}";
            var model = new ServiceRequestModel<List<EntityTransformDataMap>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodDelete,
                WorkerFunc = _ => new BusinessLogicWorker().Delete(transformDataMapId)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves the TransformDataMap
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the transform data map object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAS.Entity.TransformDataMap x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAS.Entity.TransformDataMap>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransformDataMap", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.TransformDataMap worker = new FrameworkUAS.BusinessLogic.TransformDataMap();
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
                LogError(accessKey, ex, "TransformDataMap", "Save");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
