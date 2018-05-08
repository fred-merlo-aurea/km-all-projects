using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;
using UAD_WS.Interface;

namespace UAD_WS.Service
{
    public class IssueComp : ServiceBase, IIssueComp
    {
        /// <summary>
        /// Selects a list of IssueComp objects based on the issue ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="issueID">the issue ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of IssueComp objects</returns>
        public Response<List<FrameworkUAD.Entity.IssueComp>> SelectIssue(Guid accessKey, int issueID, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.IssueComp>> response = new Response<List<FrameworkUAD.Entity.IssueComp>>();
            try
            {
                string param = "publicationID:" + issueID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "IssueComp", "SelectIssue");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.IssueComp worker = new FrameworkUAD.BusinessLogic.IssueComp();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectIssue(issueID, client);
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
        /// Saves the IssueComp object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the IssueComp object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAD.Entity.IssueComp x, KMPlatform.Object.ClientConnections client)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD.Entity.IssueComp>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "IssueComp", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.IssueComp worker = new FrameworkUAD.BusinessLogic.IssueComp();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(x, client);
                    if (response.Result > 0)
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
