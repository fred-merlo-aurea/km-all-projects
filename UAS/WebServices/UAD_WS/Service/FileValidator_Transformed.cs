using System;
using System.Collections.Generic;
using System.Linq;
using UAD_WS.Interface;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    public class FileValidator_Transformed : ServiceBase, IFileValidator_Transformed
    {
        /// <summary>
        /// Saves a list of FileValidator_Transformed objects based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="list">the list of FileValidator_Transformed objects</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> SaveBulkSqlInsert(Guid accessKey, List<FrameworkUAD.Entity.FileValidator_Transformed> list, KMPlatform.Object.ClientConnections client)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "FileValidator_Transformed", "SaveBulkSqlInsert");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.FileValidator_Transformed worker = new FrameworkUAD.BusinessLogic.FileValidator_Transformed();
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
    }
}
