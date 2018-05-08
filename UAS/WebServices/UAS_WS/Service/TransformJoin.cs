using System;
using System.Collections.Generic;
using System.Linq;
using UAS_WS.Interface;
using FrameworkUAS.Service;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class TransformJoin : ServiceBase, ITransformJoin
    {
        /// <summary>
        /// Selects a list of TransformJoin objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of TransformJoin objects</returns>
        public Response<List<FrameworkUAS.Entity.TransformJoin>> Select(Guid accessKey)
        {
            Response<List<FrameworkUAS.Entity.TransformJoin>> response = new Response<List<FrameworkUAS.Entity.TransformJoin>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "TransformJoin", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.TransformJoin worker = new FrameworkUAS.BusinessLogic.TransformJoin();
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
                LogError(accessKey, ex, "TransformJoin", "Select");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a list of TransformJoin objects based on the transformation ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="transformationID">the transformation ID</param>
        /// <returns>response.result will contain a list of TransformJoin objects</returns>
        public Response<List<FrameworkUAS.Entity.TransformJoin>> Select(Guid accessKey, int transformationID)
        {
            Response<List<FrameworkUAS.Entity.TransformJoin>> response = new Response<List<FrameworkUAS.Entity.TransformJoin>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "TransformationID:" + transformationID.ToString(), "TransformJoin", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.TransformJoin worker = new FrameworkUAS.BusinessLogic.TransformJoin();
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
                LogError(accessKey, ex, "TransformJoin", "Select");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a list of TransformJoin objects based on the source file ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <returns>response.result will contain a list of TransformJoin objects</returns>
        public Response<List<FrameworkUAS.Entity.TransformJoin>> SelectForSourceFile(Guid accessKey, int sourceFileID)
        {
            Response<List<FrameworkUAS.Entity.TransformJoin>> response = new Response<List<FrameworkUAS.Entity.TransformJoin>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "SourceFileID:" + sourceFileID.ToString(), "TransformJoin", "SelectForSourceFile");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.TransformJoin worker = new FrameworkUAS.BusinessLogic.TransformJoin();
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
                LogError(accessKey, ex, "TransformJoin", "Select");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Saves the TransformJoin object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the TransformJoin object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAS.Entity.TransformJoin x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAS.Entity.TransformJoin>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "TransformJoin", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.TransformJoin worker = new FrameworkUAS.BusinessLogic.TransformJoin();
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
                LogError(accessKey, ex, "TransformJoin", "Select");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
