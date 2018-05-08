using System;
using System.Collections.Generic;
using System.Linq;
using UAS_WS.Interface;
using FrameworkUAS.Service;

namespace UAS_WS.Service
{
    public class FileRuleMap : ServiceBase, IFileRuleMap
    {
        /// <summary>
        /// Selects a list of FileRuleMap objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result</returns>
        public Response<List<FrameworkUAS.Entity.FileRuleMap>> Select(Guid accessKey)
        {
            Response<List<FrameworkUAS.Entity.FileRuleMap>> response = new Response<List<FrameworkUAS.Entity.FileRuleMap>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "FileRuleMap", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.FileRuleMap worker = new FrameworkUAS.BusinessLogic.FileRuleMap();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select();
                    if (response.Result != null)
                    {
                        response.Message = "Success";
                        response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Success;
                    }
                    else
                    {
                        response.Message = "Error";
                        response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, "FileRuleMap", "Select");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Saves a FileRuleMap object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the FileRuleMap object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAS.Entity.FileRuleMap x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAS.Entity.FileRuleMap>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "FileRuleMap", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.FileRuleMap worker = new FrameworkUAS.BusinessLogic.FileRuleMap();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(x);
                    if (response.Result > 0)
                    {
                        response.Message = "Success";
                        response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Success;
                    }
                    else
                    {
                        response.Message = "Error";
                        response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, "FileRuleMap", "Save");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Deletes a FileRuleMap object based on the source file ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Delete(Guid accessKey, int sourceFileID)
        {
            string param = "SourceFileID: " + sourceFileID.ToString();
            Response<bool> response = new Response<bool>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "FileRuleMap", "Delete");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.FileRuleMap rmWorker = new FrameworkUAS.BusinessLogic.FileRuleMap();
                    response.Message = "AccessKey Validated";
                    response.Result = rmWorker.Delete(sourceFileID);
                    if (response.Result == true || response.Result == false)
                    {
                        response.Message = "Success";
                        response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Success;
                    }
                    else
                    {
                        response.Message = "Error";
                        response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, this.GetType().Name.ToString());
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
