using System;
using System.Collections.Generic;
using System.Data;
using FrameworkUAS.Service;
using UAD_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = FrameworkUAD.BusinessLogic.CircImportExport;
using EntityCircImportExport = FrameworkUAD.Object.CircImportExport;

namespace UAD_WS.Service
{
    public class CircImportExport : FrameworkServiceBase, ICircImportExport
    {
        private const string EntityName = "CircImportExport";
        private const string MethodSelect = "Select";

        /// <summary>
        /// Selects a list of CircImportExport objects based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of CircImportExport objects</returns>
        public Response<List<EntityCircImportExport>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client)
        {
            var model = new ServiceRequestModel<List<EntityCircImportExport>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(client)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list ofCircImportExport objects  based on the publisher ID, the client and publication ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="publisherID">the publisher ID</param>
        /// <param name="publicationID">the publication ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of CircImportExport objects</returns>
        public Response<List<FrameworkUAD.Object.CircImportExport>> Select(Guid accessKey, int publisherID, int publicationID, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Object.CircImportExport>> response = new Response<List<FrameworkUAD.Object.CircImportExport>>();
            try
            {
                string param = "PublisherID:" + publisherID.ToString() + " PublicationID:" + publicationID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "CircImportExport", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.CircImportExport worker = new FrameworkUAD.BusinessLogic.CircImportExport();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(publisherID, publicationID, client);
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
        /// Selects a list ofCircImportExport objects  based on the publisher ID, the client and publication ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="publisherID">the publisher ID</param>
        /// <param name="publicationID">the publication ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a DataTable of CircImportExport objects</returns>
        public Response<DataTable> SelectDataTable(Guid accessKey, int publisherID, int publicationID, KMPlatform.Object.ClientConnections client)
        {
            Response<DataTable> response = new Response<DataTable>();
            try
            {
                string param = "PublisherID:" + publisherID.ToString() + " PublicationID:" + publicationID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "CircImportExport", "SelectDataTable");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.CircImportExport worker = new FrameworkUAD.BusinessLogic.CircImportExport();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectDataTable(publisherID, publicationID, client);
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
        /// Saves a list of CircImportExport objects based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="userID">the user ID</param>
        /// <param name="list">the list of CircImportExport objects</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> SaveBulkSqlUpdate(Guid accessKey, int userID, List<FrameworkUAD.Object.CircImportExport> list, KMPlatform.Object.ClientConnections client)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = "UserID:" + userID.ToString() + " - " + jf.ToJson<List<FrameworkUAD.Object.CircImportExport>>(list);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "CircImportExport", "SaveBulkSqlUpdate");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.CircImportExport worker = new FrameworkUAD.BusinessLogic.CircImportExport();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SaveBulkSqlUpdate(userID, list, client);
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
