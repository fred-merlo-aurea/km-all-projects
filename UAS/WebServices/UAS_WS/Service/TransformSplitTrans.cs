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
    public class TransformSplitTrans : FrameworkServiceBase, ITransformSplitTrans
    {
        private const string EntityName = "TransformSplitTrans";
        private const string MethodSave = "Save";

        /// <summary>
        /// Selects a list of TransformSplitTrans objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of TransformSplitTrans object</returns>
        public Response<List<FrameworkUAS.Entity.TransformSplitTrans>> Select(Guid accessKey)
        {
            Response<List<FrameworkUAS.Entity.TransformSplitTrans>> response = new Response<List<FrameworkUAS.Entity.TransformSplitTrans>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "TransformSplitTrans", "Select");
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
                LogError(accessKey, ex, "TransformSplitTrans", "Select");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a list of TransformSplitTrans objects by the source file ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <returns>response.result will contain a list of TransformSplitTrans objects</returns>
        public Response<List<FrameworkUAS.Entity.TransformSplitTrans>> SelectSourceFileID(Guid accessKey, int sourceFileID)
        {
            Response<List<FrameworkUAS.Entity.TransformSplitTrans>> response = new Response<List<FrameworkUAS.Entity.TransformSplitTrans>>();
            try
            {
                string param = "sourceFileID:" + sourceFileID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransformSplitTrans", "SelectSourceFileID");
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
                LogError(accessKey, ex, "TransformSplitTrans", "SelectSourceFileID");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a lits of TransformSplitTrans objects by the transformation ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="TransformationID">the transformation ID</param>
        /// <returns>response.result will contain a list of TransformSplitTrans objects</returns>
        public Response<List<FrameworkUAS.Entity.TransformSplitTrans>> Select(Guid accessKey, int TransformationID)
        {
            Response<List<FrameworkUAS.Entity.TransformSplitTrans>> response = new Response<List<FrameworkUAS.Entity.TransformSplitTrans>>();
            try
            {
                string param = "TransformationID:" + TransformationID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransformSplitTrans", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.TransformSplitTrans worker = new FrameworkUAS.BusinessLogic.TransformSplitTrans();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(TransformationID);
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
                LogError(accessKey, ex, "TransformSplitTrans", "Select");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Saves the TransformSplitTrans object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the TransformSplitTrans object</param>
        /// <returns>response.result will contain an integer</returns>
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
