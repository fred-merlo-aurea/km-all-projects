using System;
using System.Collections.Generic;
using System.Linq;
using UAS_WS.Interface;
using System.ServiceModel.Activation;
using System.ServiceModel;
using FrameworkUAS.Service;
using WebServiceFramework;
using BusinessLogicWorker = KMPlatform.BusinessLogic.Client;
using ClientAdditionalPropertiesWorker = FrameworkUAS.BusinessLogic.ClientAdditionalProperties;
using EntityClient = KMPlatform.Entity.Client;
using EntityClientAdditionalProperties = FrameworkUAS.Object.ClientAdditionalProperties;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class Client : FrameworkServiceBase, IClient
    {
        private const string EntityName = "Client";
        private const string MethodSelectFtpFolder = "SelectFtpFolder";
        private const string MethodSelectDefault = "SelectDefault";
        private const string MethodHasService = "HasService";
        private const string MethodHasFeature = "HasFeature";
        private const string MethodGetClientAdditionalProperties = "GetClientAdditionalProperties";

        /// <summary>
        /// Selects a list of Client objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="includeCustomProperties">boolean to include the available custom properties to the client</param>
        /// <returns>response.result will contain a list of Client objects</returns>
        public Response<List<KMPlatform.Entity.Client>> Select(Guid accessKey, bool includeCustomProperties = false)
        {
            Response<List<KMPlatform.Entity.Client>> response = new Response<List<KMPlatform.Entity.Client>>();
            try
            {
                string param = "includeCustomProperties:" + includeCustomProperties.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Client", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.Client worker = new KMPlatform.BusinessLogic.Client();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(includeCustomProperties);
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
        /// Selects a Client object based on the client name
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientName">the client name</param>
        /// <param name="includeCustomProperties">boolean to include the available custom properties to the client</param>
        /// <returns>response.result will contain a Client object</returns>
        public Response<KMPlatform.Entity.Client> Select(Guid accessKey, string clientName, bool includeCustomProperties = false)
        {
            Response<KMPlatform.Entity.Client> response = new Response<KMPlatform.Entity.Client>();
            try
            {
                string param = "ClientName:" + clientName + " includeCustomProperties:" + includeCustomProperties.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Client", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.Client worker = new KMPlatform.BusinessLogic.Client();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(clientName, includeCustomProperties);
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
        /// <param name="ftpFolder"></param>
        /// <param name="includeCustomProperties"></param>
        /// <returns></returns>
        public Response<KMPlatform.Entity.Client> SelectFtpFolder(Guid accessKey, string ftpFolder, bool includeCustomProperties = false)
        {
            var param = $"FtpFolder:{ftpFolder} includeCustomProperties:{includeCustomProperties}";
            var model = new ServiceRequestModel<EntityClient>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectFtpFolder,
                WorkerFunc = _ => new BusinessLogicWorker().SelectFtpFolder(ftpFolder, includeCustomProperties)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a Client object based on the client ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <param name="includeCustomProperties">boolean to include the available custom properties to the client</param>
        /// <returns>response.result will contain a Client object</returns>
        public Response<KMPlatform.Entity.Client> Select(Guid accessKey, int clientID, bool includeCustomProperties = false)
        {
            Response<KMPlatform.Entity.Client> response = new Response<KMPlatform.Entity.Client>();
            try
            {
                string param = "clientID:" + clientID.ToString() + " includeCustomProperties:" + includeCustomProperties.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Client", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.Client worker = new KMPlatform.BusinessLogic.Client();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(clientID, includeCustomProperties);
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
        /// Selects a default Client object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="includeCustomProperties">boolean to include the available custom properties to the client</param>
        /// <returns>response.result will contain a Client object</returns>
        public Response<KMPlatform.Entity.Client> SelectDefault(Guid accessKey, bool includeCustomProperties = false)
        {
            var param = $"accessKey:{accessKey} includeCustomProperties:{includeCustomProperties}";
            var model = new ServiceRequestModel<EntityClient>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectDefault,
                WorkerFunc = _ => new BusinessLogicWorker().SelectDefault(accessKey, includeCustomProperties)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of client objects based on the access key
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="includeCustomProperties">boolean to include the available custom properties to the client</param>
        /// <returns>response.result will contain a list of Client objects</returns>
        public Response<List<KMPlatform.Entity.Client>> SelectForAccessKey(Guid accessKey, bool includeCustomProperties = false)
        {
            Response<List<KMPlatform.Entity.Client>> response = new Response<List<KMPlatform.Entity.Client>>();
            try
            {
                string param = "accessKey:" + accessKey.ToString() + " includeCustomProperties:" + includeCustomProperties.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Client", "SelectForAccessKey");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.Client worker = new KMPlatform.BusinessLogic.Client();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectForAccessKey(accessKey, includeCustomProperties);
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
        /// Selects a list of Client objects based on the client group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientGroupID">the client group ID</param>
        /// <param name="includeCustomProperties">boolean to include the available custom properties to the client</param>
        /// <returns>response.result will contain a list of Client objects</returns>
        public Response<List<KMPlatform.Entity.Client>> SelectForClientGroup(Guid accessKey, int clientGroupID, bool includeCustomProperties = false)
        {
            Response<List<KMPlatform.Entity.Client>> response = new Response<List<KMPlatform.Entity.Client>>();
            try
            {
                string param = "accessKey:" + accessKey.ToString() + " clientGroupID:" + clientGroupID.ToString() + " includeCustomProperties:" + includeCustomProperties.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Client", "SelectForClientGroup");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.Client worker = new KMPlatform.BusinessLogic.Client();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectForClientGroup(clientGroupID, includeCustomProperties);
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
        /// Saves a Client object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the Client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, KMPlatform.Entity.Client x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<KMPlatform.Entity.Client>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Client", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.Client worker = new KMPlatform.BusinessLogic.Client();
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

        /// <summary>
        /// Searches for a Client object of a certain search value in the specified list
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="searchValue">the search value</param>
        /// <param name="searchList">the specified search list</param>
        /// <param name="isActive">boolean if the client is active</param>
        /// <returns>response.result will contain a list of Client objects</returns>
        public Response<List<KMPlatform.Entity.Client>> Search(Guid accessKey, string searchValue, List<KMPlatform.Entity.Client> searchList, bool? isActive = null)
        {
            Response<List<KMPlatform.Entity.Client>> response = new Response<List<KMPlatform.Entity.Client>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "Client", "Search");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.Client worker = new KMPlatform.BusinessLogic.Client();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Search(searchValue, searchList, isActive);
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

        #region Services and Features
        /// <summary>
        /// Checks to see if a client group has a specific service by client ID and client group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <param name="service">the service enum (Fulfillment, Unified_Audience_Database, Email_Marketing, Forms)</param>
        /// <param name="clientGroupID">the client gorup ID</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> HasService(Guid accessKey, int clientID, KMPlatform.Enums.Services service, int clientGroupID = 1)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                string param = "ClientID:" + clientID.ToString() + " Service:" + service.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Client", "HasService");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.Client worker = new KMPlatform.BusinessLogic.Client();
                    response.Message = "AccessKey Validated";
                    //response.Result = worker.HasService(clientID, service, clientGroupID);
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
        /// Checks to see if a client has a specific service by the Client object and client group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the Client object</param>
        /// <param name="service">the service enum (Fulfillment, Unified_Audience_Database, Email_Marketing, Forms)</param>
        /// <param name="clientGroupID">the client group ID</param>
        /// <returns>response.result will contian a boolean</returns>
        public Response<bool> HasService(Guid accessKey, KMPlatform.Entity.Client client, KMPlatform.Enums.Services service, int clientGroupID = 1)
        {
            var param = string.Empty;
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodHasService,
                // The worker invocation was commented out in original code.
                /* WorkerFunc = _ => new BusinessLogicWorker().HasService(client.ClientID, service, clientGroupID) */
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Checks to see if a client has a specific service feature by the client ID and client group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <param name="service">the service enum (Fulfillment, Unified_Audience_Database, Email_Marketing, Forms)</param>
        /// <param name="featureName">the service feature name</param>
        /// <param name="clientGroupID">the client gorup ID</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> HasFeature(Guid accessKey, int clientID, KMPlatform.Enums.Services service, string featureName, int clientGroupID = 1)
        {
            var param = $"ClientID:{clientID} Service:{service} Feature:{featureName}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodHasFeature,
                // The worker invocation was commented out in original code.
                /* WorkerFunc = _ => new BusinessLogicWorker().HasFeature(clientID, service, featureName, clientGroupID) */
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Checks to see if a client has a specific service feature by the Client object and client group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the Client object</param>
        /// <param name="service">the service enum (Fulfillment, Unified_Audience_Database, Email_Marketing, Forms)</param>
        /// <param name="featureName">the service feature name</param>
        /// <param name="clientGroupID">the client group ID</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> HasFeature(Guid accessKey, KMPlatform.Entity.Client client, KMPlatform.Enums.Services service, string featureName, int clientGroupID = 1)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                string param = "";
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Client", "HasService");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.Client worker = new KMPlatform.BusinessLogic.Client();
                    response.Message = "AccessKey Validated";
                    //esponse.Result = worker.HasFeature(client.ClientID, service, featureName, clientGroupID);
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
        /// Checks to see if a client has the Fulfillment Service by the client ID and client group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <param name="clientGroupID">the client group ID</param>
        /// <returns>repsonse.resutl will contain a boolean</returns>
        public Response<bool> HasFulfillmentService(Guid accessKey, int clientID, int clientGroupID = 1)
        {
            var param = string.Empty;
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodHasService,
                // The worker invocation was commented out in original code.
                /* WorkerFunc = _ => new BusinessLogicWorker().HasFulfillmentService(clientID, clientGroupID) */
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Chekcs to see if a client has the Fulfillment Service by the Client object and client group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the Client object</param>
        /// <param name="clientGroupID">the client group ID</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> HasFulfillmentService(Guid accessKey, KMPlatform.Entity.Client client, int clientGroupID = 1)
        {
            var param = string.Empty;
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodHasService,
                // The worker invocation was commented out in original code.
                /* WorkerFunc = _ => new BusinessLogicWorker().HasFulfillmentService(client.ClientID, clientGroupID) */
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Checks to see if a client uses the UAD suppression feature by the client ID and client group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <param name="clientGroupID">the client group ID</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> UseUADSuppressionFeature(Guid accessKey, int clientID, int clientGroupID = 1)
        {
            var param = string.Empty;
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodHasService,
                // The worker invocation was commented out in original code.
                /* WorkerFunc = _ => new BusinessLogicWorker().UseUADSuppressionFeature(clientID, clientGroupID) */
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Chekcs to see if a client uses the UAD suppression feature by the Client object and client group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the Client object</param>
        /// <param name="clientGroupID">the client group ID</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> UseUADSuppressionFeature(Guid accessKey, KMPlatform.Entity.Client client, int clientGroupID = 1)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                string param = "";
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Client", "HasService");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.Client worker = new KMPlatform.BusinessLogic.Client();
                    response.Message = "AccessKey Validated";
                    //response.Result = worker.UseUADSuppressionFeature(client, clientGroupID);
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
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="clientId"></param>
        /// <param name="isFileDeleted"></param>
        /// <returns></returns>
        public Response<FrameworkUAS.Object.ClientAdditionalProperties> GetClientAdditionalProperties(Guid accessKey, int clientId, bool isFileDeleted)
        {
            var param = $"ClientId:{clientId}";
            var model = new ServiceRequestModel<EntityClientAdditionalProperties>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodGetClientAdditionalProperties,
                WorkerFunc = _ => new ClientAdditionalPropertiesWorker().SetObjects(clientId, isFileDeleted)
            };

            return GetResponse(model);
        }
        #endregion
    }
}
