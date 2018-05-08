using UAD_WS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    public class FinalizeBatch : ServiceBase, IFinalizeBatch
    {
        /// <summary>
        /// Selects a list of FinalizeBatch objects object based on the user ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="userID">the user ID</param>
        /// <param name="client">the client object</param>
        /// <param name="clientId">the Client object</param>
        /// <param name="clientName">the Client object</param>
        /// <returns>response.result will contain a list of FinalizeBatch objects</returns>
        public Response<List<FrameworkUAD.Object.FinalizeBatch>> Select(Guid accessKey, int userID, KMPlatform.Object.ClientConnections client, int clientId, string clientName)
        {
            Response<List<FrameworkUAD.Object.FinalizeBatch>> response = new Response<List<FrameworkUAD.Object.FinalizeBatch>>();
            try
            {
                string param = "UserID:" + userID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "FinalizeBatch", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.FinalizeBatch worker = new FrameworkUAD.BusinessLogic.FinalizeBatch();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectAllUser(userID, client);
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
        /// Selects a list of FinalizeBatch objects based on the user ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="userID">the user ID</param>
        /// <param name="client">the client object</param>
        /// /// <param name="clientId">the Client object</param>
        /// <param name="clientName">the Client object</param>
        /// <returns>response.result will contain a list of FinalizeBatch objects</returns>
        public Response<List<FrameworkUAD.Object.FinalizeBatch>> SelectBatch(Guid accessKey, int userID, KMPlatform.Object.ClientConnections client, int clientId, string clientName)
        {
            Response<List<FrameworkUAD.Object.FinalizeBatch>> response = new Response<List<FrameworkUAD.Object.FinalizeBatch>>();
            try
            {
                string param = "UserID:" + userID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "FinalizeBatch", "SelectBatch");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.FinalizeBatch worker = new FrameworkUAD.BusinessLogic.FinalizeBatch();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectAllUser(userID, client);
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
        /// Selects a list of FinalizeBatch objects user names based on the user ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="userID">the user ID</param>
        /// <param name="client">the client object</param>
        /// /// <param name="clientId">the Client object</param>
        /// <param name="clientName">the Client object</param>
        /// <returns>response.result will contain a list of FinalizeBatch objects</returns>
        public Response<List<FrameworkUAD.Object.FinalizeBatch>> SelectBatchUserName(Guid accessKey, int userID, KMPlatform.Object.ClientConnections client, int clientId, string clientName)
        {
            Response<List<FrameworkUAD.Object.FinalizeBatch>> response = new Response<List<FrameworkUAD.Object.FinalizeBatch>>();
            try
            {
                string param = "UserID:" + userID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "FinalizeBatch", "SelectBatchUserName");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.FinalizeBatch worker = new FrameworkUAD.BusinessLogic.FinalizeBatch();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectAllUser(userID, client);
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
