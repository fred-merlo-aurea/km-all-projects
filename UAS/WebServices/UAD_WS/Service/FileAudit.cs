using System;
using System.Collections.Generic;
using System.Linq;
using UAD_WS.Interface;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    public class FileAudit : ServiceBase, IFileAudit
    {
        /// <summary>
        /// Selects a list of FileAudit objects based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of FileAudit objects</returns>
        public Response<List<FrameworkUAD.Object.FileAudit>> SelectDistinctProcessCodePerSourceFile(Guid accessKey, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Object.FileAudit>> response = new Response<List<FrameworkUAD.Object.FileAudit>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "FileAudit", "SelectDistinctProcessCodePerSourceFile");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.FileAudit worker = new FrameworkUAD.BusinessLogic.FileAudit();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectDistinctProcessCodePerSourceFile(client).ToList();
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
