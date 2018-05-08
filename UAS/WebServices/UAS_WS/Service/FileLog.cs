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
    public class FileLog : ServiceBase, IFileLog
    {
        /// <summary>
        /// Selects a list of FileLog objects joined through SourceFile based on the client ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <returns>response.result will containa a list of FileLog objects</returns>
        public Response<List<FrameworkUAS.Entity.FileLog>> SelectClient(Guid accessKey, int clientID)
        {
            Response<List<FrameworkUAS.Entity.FileLog>> response = new Response<List<FrameworkUAS.Entity.FileLog>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "ClientID:" + clientID.ToString(), "FileLog", "SelectClient");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.FileLog worker = new FrameworkUAS.BusinessLogic.FileLog();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectClient(clientID);
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
                LogError(accessKey, ex, this.GetType().Name.ToString());
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a list of FileLog objects based on the processCode
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="processCode">the process code</param>
        /// <returns>response.result will contain a list of FileLog objects</returns>
        public Response<List<FrameworkUAS.Entity.FileLog>> SelectProcessCode(Guid accessKey, string processCode)
        {
            Response<List<FrameworkUAS.Entity.FileLog>> response = new Response<List<FrameworkUAS.Entity.FileLog>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, " ProcessCode:" + processCode, "FileLog", "SelectProcessCode");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.FileLog worker = new FrameworkUAS.BusinessLogic.FileLog();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectProcessCode(processCode);
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
                LogError(accessKey, ex, this.GetType().Name.ToString());
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a list of FileLog objects joined through SourceFile based on the sourceFileID or processCode
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="sourceFileID">the source file ID</param>
        /// <param name="processCode">the process code</param>
        /// <returns>response.result will contain a list of FileLog objects</returns>
        public Response<List<FrameworkUAS.Entity.FileLog>> SelectFileLog(Guid accessKey, int sourceFileID, string processCode)
        {
            Response<List<FrameworkUAS.Entity.FileLog>> response = new Response<List<FrameworkUAS.Entity.FileLog>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "SourceFileID:" + sourceFileID.ToString() + " ProcessCode:" + processCode, "FileLog", "SelectFileLog");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.FileLog worker = new FrameworkUAS.BusinessLogic.FileLog();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectFileLog(sourceFileID, processCode);
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
                LogError(accessKey, ex, this.GetType().Name.ToString());
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Saves a FileLog object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the FileLog object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Save(Guid accessKey, FrameworkUAS.Entity.FileLog x)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAS.Entity.FileLog>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "FileLog", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.FileLog worker = new FrameworkUAS.BusinessLogic.FileLog();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(x);
                    if (response.Result == true || response.Result == false)
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
                LogError(accessKey, ex, this.GetType().Name.ToString());
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    
        /// <summary>
        /// Selects a list of FileLog objects joined through SourceFile based on the client ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the ClientID</param>
        /// <returns>response.result will contain a list of FileLog objects</returns>
        public Response<List<FrameworkUAS.Object.FileLog>> SelectDistinctProcessCodePerSourceFile(Guid accessKey, int clientID)
        {
            Response<List<FrameworkUAS.Object.FileLog>> response = new Response<List<FrameworkUAS.Object.FileLog>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "ClientID:" + clientID.ToString(), "FileLog", "SelectDistinctProcessCodePerSourceFile");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.FileLog worker = new FrameworkUAS.BusinessLogic.FileLog();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectDistinctProcessCodePerSourceFile(clientID);
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
                LogError(accessKey, ex, this.GetType().Name.ToString());
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
