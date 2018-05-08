using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;
using UAD_WS.Interface;
using System.Data;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.IssueCompDetail;

namespace UAD_WS.Service
{
    public class IssueCompDetail : ServiceBase, IIssueCompDetail
    {
        private const string EntityName = "IssueCompDetail";
        private const string MethodClear = "Clear";

        /// <summary>
        /// Selects a list of IssueCompDetail objects based on the issueComp ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="issueCompID">the issueComp ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of IssueCompDetail objects</returns>
        public Response<List<FrameworkUAD.Entity.IssueCompDetail>> Select(Guid accessKey, int issueCompID, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.IssueCompDetail>> response = new Response<List<FrameworkUAD.Entity.IssueCompDetail>>();
            try
            {
                string param = "publicationID:" + issueCompID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "IssueCompDetail", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.IssueCompDetail worker = new FrameworkUAD.BusinessLogic.IssueCompDetail();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(issueCompID, client);
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
        /// Gets the information from the reporting object, then passes it through a filtering process, stores the information on the IssueCompDetail object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="xml"></param>
        /// <param name="adHocXml"></param>
        /// <param name="issueCompID">the issueComp ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of IssueCompDetail objects</returns>
        public Response<List<int>> GetByFilter(Guid accessKey, string xml, string adHocXml, int issueCompID, KMPlatform.Object.ClientConnections client)
        {
            Response<List<int>> response = new Response<List<int>>();
            try
            {
                string param = string.Empty;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "IssueCompDetail", "GetFromFilter");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.IssueCompDetail worker = new FrameworkUAD.BusinessLogic.IssueCompDetail();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.GetByFilter(xml, adHocXml, issueCompID, client);
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
                response.Message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            }
            return response;
        }

        /// <summary>
        /// Saves the IssueCompDetail object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the IssueCompDetail object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAD.Entity.IssueCompDetail x, KMPlatform.Object.ClientConnections client)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD.Entity.IssueCompDetail>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "IssueCompDetail", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.IssueCompDetail worker = new FrameworkUAD.BusinessLogic.IssueCompDetail();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(x, client);
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
        /// Clears the IssueCompDetail object selected by the issueComp ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="issueCompID">the issueComp ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Clear(Guid accessKey, int issueCompID, KMPlatform.Object.ClientConnections client)
        {
            var param = $"IssueCompID: {issueCompID}";
            var model = new RequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodClear,
                WorkerFunc = _ => new BusinessLogicWorker().Clear(issueCompID, client)
            };

            return GetResponse(model);
        }

        public Response<DataTable> SelectForExport(Guid accessKey, int issueID, string cols, List<int> subs, KMPlatform.Object.ClientConnections client)
        {
            Response<DataTable> response = new Response<DataTable>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "IssueCompDetail", "SelectForExport");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.IssueCompDetail worker = new FrameworkUAD.BusinessLogic.IssueCompDetail();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select_For_Export(issueID, cols, subs, client);
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
