using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;
using UAD_WS.Interface;

namespace UAD_WS.Service
{
    public class IssueSplit : ServiceBase, IIssueSplit
    {
        /// <summary>
        /// Selects a list of IssueSplit objects based on the issue ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="issueID">the issue ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of IssueSplit objects</returns>
        public Response<List<FrameworkUAD.Entity.IssueSplit>> SelectForIssueID(Guid accessKey, int issueID, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.IssueSplit>> response = new Response<List<FrameworkUAD.Entity.IssueSplit>>();
            try
            {
                string param = "issueID:" + issueID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "IssueSplit", "SelectForIssueID");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.IssueSplit worker = new FrameworkUAD.BusinessLogic.IssueSplit();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectIssueID(issueID, client);
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
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <param name="issueID"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public Response<bool> Save(Guid accessKey, List<FrameworkUAD.Entity.IssueSplit> x, int issueID, KMPlatform.Object.ClientConnections client)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<List<FrameworkUAD.Entity.IssueSplit>>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "IssueSplit", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.IssueSplit worker = new FrameworkUAD.BusinessLogic.IssueSplit();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(x, issueID, client);
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
