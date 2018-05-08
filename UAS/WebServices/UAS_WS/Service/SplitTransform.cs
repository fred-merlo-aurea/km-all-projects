using System;
using System.Collections.Generic;
using System.Linq;
using UAS_WS.Interface;
using FrameworkUAS.Service;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAS.BusinessLogic.TransformSplitTrans;
using UtilityJsonFunctions = Core_AMS.Utilities.JsonFunctions;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class SplitTransform : FrameworkServiceBase, ISplitTransform
    {
        private const string EntityName = "SplitTransform";
        private const string MethodSave = "Save";

        /// <summary>
        /// Selects a list of TransformSplitTrans objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of TransformSplitTrans objects</returns>
        public Response<List<FrameworkUAS.Entity.TransformSplitTrans>> Select(Guid accessKey)
        {

            Response<List<FrameworkUAS.Entity.TransformSplitTrans>> response = new Response<List<FrameworkUAS.Entity.TransformSplitTrans>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "SplitTransform", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.TransformSplitTrans worker = new FrameworkUAS.BusinessLogic.TransformSplitTrans();
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
                LogError(accessKey, ex, "SplitTransform", "Select");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a list of TransformSplitTrans objects based on the source file ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <returns>response.result will contain a list of TransformSplitTrans objects</returns>
        public Response<List<FrameworkUAS.Entity.TransformSplitTrans>> SelectForSourceFile(Guid accessKey, int sourceFileID)
        {
            Response<List<FrameworkUAS.Entity.TransformSplitTrans>> response = new Response<List<FrameworkUAS.Entity.TransformSplitTrans>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "SourceFileID:" + sourceFileID.ToString(), "SplitTransform", "SelectForSourceFile");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.TransformSplitTrans worker = new FrameworkUAS.BusinessLogic.TransformSplitTrans();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectSourceFileID(sourceFileID);
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
                LogError(accessKey, ex, "SplitTransform", "SelectForSourceFile");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a list of TransformSplitTrans objects based on the transformation ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="transformationID">the transformation ID</param>
        /// <returns>response.result will contain a list of TransformSplitTrans objects</returns>
        public Response<List<FrameworkUAS.Entity.TransformSplitTrans>> SelectForTransformation(Guid accessKey, int transformationID)
        {
            Response<List<FrameworkUAS.Entity.TransformSplitTrans>> response = new Response<List<FrameworkUAS.Entity.TransformSplitTrans>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "TransformationID:" + transformationID.ToString(), "SplitTransform", "SelectForTransformation");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.TransformSplitTrans worker = new FrameworkUAS.BusinessLogic.TransformSplitTrans();
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
                LogError(accessKey, ex, "SplitTransform", "SelectForTransformation");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Saves the TransformSplitTrans object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the TransformSplitTrans object</param>
        /// <returns>the response will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAS.Entity.TransformSplitTrans x)
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
    }
}
