using System;
using System.Collections.Generic;
using FrameworkUAS.Service;
using UAS_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = KMPlatform.BusinessLogic.ClientGroup;
using EntityClientGroup = KMPlatform.Entity.ClientGroup;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class ClientGroup : FrameworkServiceBase, IClientGroup
    {
        private const string EntityName = "ClientGroup";
        private const string MethodSelectClient = "SelectClient";
        private const string MethodSelectForUserAuthorization = "SelectForUserAuthorization";

        /// <summary>
        /// Selects a list of ClientGroup objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="includeObjects">boolean to include available objects to the client group</param>
        /// <returns>response.result will contain a list of ClientGroup objects</returns>
        public Response<List<KMPlatform.Entity.ClientGroup>> Select(Guid accessKey, bool includeObjects = false)
        {
            Response<List<KMPlatform.Entity.ClientGroup>> response = new Response<List<KMPlatform.Entity.ClientGroup>>();
            try
            {
                string param = string.Empty;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ClientGroup", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.ClientGroup worker = new KMPlatform.BusinessLogic.ClientGroup();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(includeObjects);
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
        /// Selects a list of ClientGroup objects based on the client ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientId">the client ID</param>
        /// <param name="includeObjects">boolean to include available objects to the client group</param>
        /// <returns>reponse.result will contain a list of ClientGroup objects</returns>
        public Response<List<EntityClientGroup>> SelectClient(Guid accessKey, int clientId, bool includeObjects = false)
        {
            var param = $"clientID:{clientId}";
            var model = new ServiceRequestModel<List<EntityClientGroup>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectClient,
                WorkerFunc = _ => new BusinessLogicWorker().SelectClient(clientId, includeObjects)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of ClientGroup objects based on the user ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="userID">the user ID</param>
        /// <param name="includeObjects">boolean to include available objects to the client group</param>
        /// <returns>response.result will contain a list of ClientGroup objects</returns>
        public Response<List<KMPlatform.Entity.ClientGroup>> SelectForUser(Guid accessKey, int userID, bool includeObjects = false)
        {
            Response<List<KMPlatform.Entity.ClientGroup>> response = new Response<List<KMPlatform.Entity.ClientGroup>>();
            try
            {
                string param = "userID:" + userID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ClientGroup", "SelectForUser");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.ClientGroup worker = new KMPlatform.BusinessLogic.ClientGroup();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectForUser(userID, includeObjects);
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
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="userId"></param>
        /// <param name="includeObjects"></param>
        /// <returns></returns>
        public Response<List<EntityClientGroup>> SelectForUserAuthorization(Guid accessKey, int userId, bool includeObjects = false)
        {
            var param = $"userID:{userId}";
            var model = new ServiceRequestModel<List<EntityClientGroup>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForUserAuthorization,
                WorkerFunc = _ => new BusinessLogicWorker().SelectForUserAuthorization(userId, includeObjects)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a ClientGroup object based on the client group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientGroupID">the client group ID</param>
        /// <param name="includeObjects">boolean to include available objects to the client group</param>
        /// <returns>response.result will contain a ClientGroup object</returns>
        public Response<KMPlatform.Entity.ClientGroup> Select(Guid accessKey, int clientGroupID, bool includeObjects = false)
        {
            Response<KMPlatform.Entity.ClientGroup> response = new Response<KMPlatform.Entity.ClientGroup>();
            try
            {
                string param = "clientGroupID:" + clientGroupID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ClientGroup", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.ClientGroup worker = new KMPlatform.BusinessLogic.ClientGroup();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(clientGroupID, includeObjects);
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
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        public Response<List<KMPlatform.Entity.ClientGroup>> SelectLite(Guid accessKey)
        {
            Response<List<KMPlatform.Entity.ClientGroup>> response = new Response<List<KMPlatform.Entity.ClientGroup>>();
            try
            {
                string param = string.Empty;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ClientGroup", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.ClientGroup worker = new KMPlatform.BusinessLogic.ClientGroup();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectLite();
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
        /// Saves a ClientGroup object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the ClientGroup object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, KMPlatform.Entity.ClientGroup x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<KMPlatform.Entity.ClientGroup>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ClientGroup", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.ClientGroup worker = new KMPlatform.BusinessLogic.ClientGroup();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(x);
                    if (response.Result >= 0)
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
