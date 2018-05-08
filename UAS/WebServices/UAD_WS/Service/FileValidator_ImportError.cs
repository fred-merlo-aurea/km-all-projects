using System;
using System.Collections.Generic;
using System.Linq;
using UAD_WS.Interface;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    public class FileValidator_ImportError : ServiceBase, IFileValidator_ImportError
    {
        public Response<List<FrameworkUAD.Entity.FileValidator_ImportError>> Select(Guid accessKey, string processCode, int sourceFileID, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.FileValidator_ImportError>> response = new Response<List<FrameworkUAD.Entity.FileValidator_ImportError>>();
            try
            {
                string param = " processCode:" + processCode + " sourceFileID:" + sourceFileID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ImportError", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.FileValidator_ImportError worker = new FrameworkUAD.BusinessLogic.FileValidator_ImportError();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(processCode, sourceFileID, client).ToList();
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
        /// Saves a list of ImportError objects for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="list">the list of ImportError objects</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> SaveBulkSqlInsert(Guid accessKey, List<FrameworkUAD.Entity.ImportError> list, KMPlatform.Object.ClientConnections client)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "FileValidator_ImportError", "SaveBulkSqlInsert");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.FileValidator_ImportError worker = new FrameworkUAD.BusinessLogic.FileValidator_ImportError();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SaveBulkSqlInsert(list, client);
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
        /// Saves a batch of 1000 ImportErrors at once for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the ImportError object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> SaveBulkSqlInsert(Guid accessKey, FrameworkUAD.Entity.ImportError x, KMPlatform.Object.ClientConnections client)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD.Entity.ImportError>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "FileValidator_ImportError", "SaveBulkSqlInsert");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.FileValidator_ImportError worker = new FrameworkUAD.BusinessLogic.FileValidator_ImportError();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SaveBulkSqlInsert(x, client);
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
    }
}
