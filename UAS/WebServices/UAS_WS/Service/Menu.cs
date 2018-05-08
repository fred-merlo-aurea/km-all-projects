using System;
using System.Collections.Generic;
using FrameworkUAS.Service;
using UAS_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = KMPlatform.BusinessLogic.Menu;
using EntityMenu = KMPlatform.Entity.Menu;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class Menu : FrameworkServiceBase, IMenu
    {
        private const string EntityName = "Menu";
        private const string MethodSelect = "Select";
        private const string MethodSelectForApplication = "SelectForApplication";

        /// <summary>
        /// Selects a list of Menu objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="includeFeatures">boolean to include available features to the Menu object</param>
        /// <returns>response.result will contain a list of Menu objects</returns>
        public Response<List<KMPlatform.Entity.Menu>> Select(Guid accessKey, bool includeFeatures = false)
        {
            Response<List<KMPlatform.Entity.Menu>> response = new Response<List<KMPlatform.Entity.Menu>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "IncludeFeatures:" + includeFeatures.ToString(), "Menu", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.Menu worker = new KMPlatform.BusinessLogic.Menu();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(includeFeatures);
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
        /// Selects a list of Menu objects joined through MenuSecurityGroupMap based on the security group ID and whether the Menu has access or not
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="securityGroupID">the security group ID</param>
        /// <param name="hasAccess">boolean if the Menu object has access</param>
        /// <param name="includeFeatures">boolean to include available features to the Menu object</param>
        /// <returns>response.result will contain a list of Menu objects</returns>
        public Response<List<EntityMenu>> Select(Guid accessKey, int securityGroupID, bool hasAccess, bool includeFeatures = false)
        {
            var model = new ServiceRequestModel<List<EntityMenu>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = $"SecurityGroupID:{securityGroupID} HasAccess:{hasAccess} IncludeFeatures:{includeFeatures}",
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ => new BusinessLogicWorker().Select(securityGroupID, hasAccess, includeFeatures)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of Menu objects joined through MenuSecurityGroupMap based on the security group ID, application ID and whether the Menu has access or not
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="securityGroupID">the security group ID</param>
        /// <param name="hasAccess">boolean if the Menu object has access</param>
        /// <param name="applicationID">the application ID</param>
        /// <param name="includeFeatures">boolean to include available features to the Menu object</param>
        /// <returns>response.result will contain a list of Menu objects</returns>
        public Response<List<KMPlatform.Entity.Menu>> Select(Guid accessKey, int securityGroupID, bool hasAccess, int applicationID, bool includeFeatures = false)
        {
            Response<List<KMPlatform.Entity.Menu>> response = new Response<List<KMPlatform.Entity.Menu>>();
            try
            {
                string param = "SecurityGroupID:" + securityGroupID.ToString() + " HasAccess:" + hasAccess.ToString() + " ApplicationID:" + applicationID.ToString() + " IncludeFeatures:" + includeFeatures.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Menu", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.Menu worker = new KMPlatform.BusinessLogic.Menu();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(securityGroupID, applicationID, includeFeatures);
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
        /// Selects a list of Menu objects based on the application ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="applicationID">the application ID</param>
        /// <param name="isActive">boolean if the Menu object is active</param>
        /// <param name="includeFeatures">boolean to include available features to the Menu object</param>
        /// <returns>response.result will contain a list of Menu objects</returns>
        public Response<List<EntityMenu>> SelectForApplication(Guid accessKey, int applicationID, bool isActive, bool includeFeatures = false)
        {
            var model = new ServiceRequestModel<List<EntityMenu>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = $"applicationID:{applicationID} isActive:{isActive} IncludeFeatures:{includeFeatures}",
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForApplication,
                WorkerFunc = _ => new BusinessLogicWorker().SelectForApplication(applicationID, isActive, includeFeatures)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of Menu objects based on the application ID and userID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="applicationID">the application ID</param>
        /// <param name="userID">the user ID</param>
        /// <param name="isActive">boolean if the Menu object is active</param>
        /// <param name="includeFeatures">boolean to include available features to the Menu object</param>
        /// <returns>response.result will contain a list of Menu objects</returns>
        public Response<List<KMPlatform.Entity.Menu>> SelectForApplicationUserID(Guid accessKey, int applicationID, int userID, bool isActive, bool includeFeatures = false)
        {
            Response<List<KMPlatform.Entity.Menu>> response = new Response<List<KMPlatform.Entity.Menu>>();
            try
            {
                string param = "applicationID:" + applicationID.ToString() + " userID:" + userID.ToString() + " isActive:" + isActive.ToString() + " IncludeFeatures:" + includeFeatures.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Menu", "SelectForApplication");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.Menu worker = new KMPlatform.BusinessLogic.Menu();
                    response.Message = "AccessKey Validated";
                    //response.Result = worker.SelectForApplication(applicationID, userID, isActive, includeFeatures);
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
        /// Selects a list of Menu objects joined through MenuSecurityGroupMap and UserClientSecurityGroupMap based on the user ID and whether the Menu is active and if it has access or not
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="userID">the user ID</param>
        /// <param name="securityGroupID">the security group ID</param>
        /// <param name="isActive">boolean if the Menu object is active</param>
        /// <param name="hasAccess">boolean if the Menu object has access</param>
        /// <param name="includeFeatures">boolean to include available features to the Menu object</param>
        /// <returns>response.result will contain a list of Menu objects</returns>
        public Response<List<KMPlatform.Entity.Menu>> SelectForUser(Guid accessKey, int userID, int securityGroupID, bool isActive, bool hasAccess, bool includeFeatures = false)
        {
            Response<List<KMPlatform.Entity.Menu>> response = new Response<List<KMPlatform.Entity.Menu>>();
            try
            {
                string param = "userID:" + userID.ToString() + " securityGroupID: " + securityGroupID.ToString() + " isActive:" + isActive.ToString() + " HasAccess:" + hasAccess.ToString() + " IncludeFeatures:" + includeFeatures.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Menu", "SelectForUser");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.Menu worker = new KMPlatform.BusinessLogic.Menu();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectForUser(userID, securityGroupID, isActive, hasAccess, includeFeatures);
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
        /// Saves a Menu object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the Menu object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, KMPlatform.Entity.Menu x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<KMPlatform.Entity.Menu>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Menu", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    KMPlatform.BusinessLogic.Menu worker = new KMPlatform.BusinessLogic.Menu();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(x);
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
