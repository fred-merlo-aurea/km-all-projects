using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;
using UAD_WS.Interface;

namespace UAD_WS.Service
{
    public class IssueCompError : ServiceBase, IIssueCompError
    {
        /// <summary>
        /// Selects a list of IssueCompError objects based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of IssueCompError objects</returns>
        public Response<List<FrameworkUAD.Entity.IssueCompError>> Select(Guid accessKey,KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.IssueCompError>> response = new Response<List<FrameworkUAD.Entity.IssueCompError>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "IssueCompError", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.IssueCompError worker = new FrameworkUAD.BusinessLogic.IssueCompError();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(client);
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
                LogError(accessKey, ex, this.GetType().Name.ToString()); response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a list of IssueCompError objects based on the process code and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="processCode">the process code</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of IssueCompError objects</returns>
        public Response<List<FrameworkUAD.Entity.IssueCompError>> SelectProcessCode(Guid accessKey, string processCode, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.IssueCompError>> response = new Response<List<FrameworkUAD.Entity.IssueCompError>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, processCode, "IssueCompError", "SelectProcessCode");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.IssueCompError worker = new FrameworkUAD.BusinessLogic.IssueCompError();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectProcessCode(processCode, client);
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
                LogError(accessKey, ex, this.GetType().Name.ToString()); response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
