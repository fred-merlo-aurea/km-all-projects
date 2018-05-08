using System;
using System.Collections.Generic;
using System.Linq;
using UAS_WS.Interface;
using FrameworkUAS.Service;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class Service : ServiceBase, IService
    {
        /// <summary>
        /// Selects a list of Service objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="includeObjects">boolean to include objects that are enabled to the service</param>
        /// <returns>response.result will contain a list of Service objects</returns>
        public Response<List<KMPlatform.Entity.Service>> Select(Guid accessKey, bool includeObjects = false)
        {
            Response<List<KMPlatform.Entity.Service>> response = new Response<List<KMPlatform.Entity.Service>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "includeObjects:" + includeObjects.ToString(), "Service", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.Service worker = new KMPlatform.BusinessLogic.Service();
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
                LogError(accessKey, ex, "Service", "Select");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a list of Service objects for the user based on the user ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="userID">the user ID</param>
        /// <param name="includeObjects">boolean to include objects that are enabled to the service</param>
        /// <returns>response.result will contain a list of Service Objects</returns>
        public Response<List<KMPlatform.Entity.Service>> SelectForUser(Guid accessKey, int userID, bool includeObjects = false)
        {
            Response<List<KMPlatform.Entity.Service>> response = new Response<List<KMPlatform.Entity.Service>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "UserID:" + userID.ToString() + " includeObjects:" + includeObjects.ToString(), "Service", "SelectForUser");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.Service worker = new KMPlatform.BusinessLogic.Service();
                    response.Message = "AccessKey Validated";
                    //response.Result = worker.SelectForUser(userID, includeObjects);
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
                LogError(accessKey, ex, "Service", "SelectForUser");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a Service object based on the service ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="serviceID">the service ID</param>
        /// <param name="includeObjects">boolean to include objects that are enabled to the service</param>
        /// <returns>response.result will contain a Service object</returns>
        public Response<KMPlatform.Entity.Service> Select(Guid accessKey, int serviceID, bool includeObjects = false)
        {
            Response<KMPlatform.Entity.Service> response = new Response<KMPlatform.Entity.Service>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "serviceID:" + serviceID.ToString() + " includeObjects:" + includeObjects.ToString(), "Service", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.Service worker = new KMPlatform.BusinessLogic.Service();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(serviceID, includeObjects);
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
                LogError(accessKey, ex, "Service", "Select");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a Service object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="service">the service enum (Fulfillment, Unified_Audience_Database, Email_Marketing, Forms)</param>
        /// <param name="includeObjects">boolean to include objects that are enabled to the service</param>
        /// <returns>response.result will contain a Service object</returns>
        public Response<KMPlatform.Entity.Service> Select(Guid accessKey, KMPlatform.Enums.Services service, bool includeObjects = false)
        {
            Response<KMPlatform.Entity.Service> response = new Response<KMPlatform.Entity.Service>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "service:" + service.ToString() + " includeObjects:" + includeObjects.ToString(), "Service", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.Service worker = new KMPlatform.BusinessLogic.Service();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(service, includeObjects);
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
                LogError(accessKey, ex, "Service", "Select");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a list of Service objects available to the user's authorization by the user ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="userID">the user ID</param>
        /// <returns>response.result will contain a list of Service objects</returns>
        public Response<List<KMPlatform.Entity.Service>> SelectForUserAuthorization(Guid accessKey, int userID)
        {
            Response<List<KMPlatform.Entity.Service>> response = new Response<List<KMPlatform.Entity.Service>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "UserID:" + userID.ToString(), "Service", "SelectForUserAuthorization");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.Service worker = new KMPlatform.BusinessLogic.Service();
                    response.Message = "AccessKey Validated";
                    //response.Result = worker.SelectForUserAuthorization(userID);
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
                LogError(accessKey, ex, "Service", "SelectForUserAuthorization");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Sets Service objects availabe to the user's authorization
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the Service object</param>
        /// <returns>response.result will contain a Service object</returns>
        public Response<KMPlatform.Entity.Service> SetObjectsForUserAuthorization(Guid accessKey, KMPlatform.Entity.Service x)
        {
            Response<KMPlatform.Entity.Service> response = new Response<KMPlatform.Entity.Service>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<KMPlatform.Entity.Service>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Service", "SetObjectsForUserAuthorization");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    KMPlatform.BusinessLogic.Service worker = new KMPlatform.BusinessLogic.Service();
                    response.Message = "AccessKey Validated";
                   // response.Result = worker.SetObjectsForUserAuthorization(x);
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
                LogError(accessKey, ex, "Service", "SetObjectsForUserAuthorization");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Sets Service objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the Service object</param>
        /// <returns>response.result will contain a Service object</returns>
        public Response<KMPlatform.Entity.Service> SetObjects(Guid accessKey, KMPlatform.Entity.Service x)
        {
            Response<KMPlatform.Entity.Service> response = new Response<KMPlatform.Entity.Service>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<KMPlatform.Entity.Service>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Service", "SetObjects");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    KMPlatform.BusinessLogic.Service worker = new KMPlatform.BusinessLogic.Service();
                    response.Message = "AccessKey Validated";
                    //response.Result = worker.SetObjectsForUserAuthorization(x);
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
                LogError(accessKey, ex, "Service", "SetObjects");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Saves a Service object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the Service object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, KMPlatform.Entity.Service x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<KMPlatform.Entity.Service>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Service", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    KMPlatform.BusinessLogic.Service worker = new KMPlatform.BusinessLogic.Service();
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
                LogError(accessKey, ex, "Service", "Save");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
