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
    public class ApplicationServiceMap : ServiceBase, IApplicationServiceMap
    {
        /// <summary>
        /// Selects a list of ApplicationServiceMap objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of ApplicationServiceMap objects</returns>
        public Response<List<KMPlatform.Entity.ApplicationServiceMap>> Select(Guid accessKey)
        {
            Response<List<KMPlatform.Entity.ApplicationServiceMap>> response = new Response<List<KMPlatform.Entity.ApplicationServiceMap>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "ApplicationServiceMap", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.ApplicationServiceMap worker = new KMPlatform.BusinessLogic.ApplicationServiceMap();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select();
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
        /// Selects a list of ApplicationServiceMap objects based on the application ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="applicationID">the application ID</param>
        /// <returns>response.result will contain a list of ApplicationServiceMap objects</returns>
        public Response<List<KMPlatform.Entity.ApplicationServiceMap>> SelectForApplication(Guid accessKey, int applicationID)
        {
            Response<List<KMPlatform.Entity.ApplicationServiceMap>> response = new Response<List<KMPlatform.Entity.ApplicationServiceMap>>();
            try
            {
                string param = "ApplicationID:" + applicationID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ApplicationServiceMap", "SelectForApplication");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.ApplicationServiceMap worker = new KMPlatform.BusinessLogic.ApplicationServiceMap();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectForApplication(applicationID);
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
        /// Selects a list of ApplicationServiceMap objects based on the service ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="serviceID">the service ID</param>
        /// <returns>response.result will contain a list of ApplicationServiceMap objects</returns>
        public Response<List<KMPlatform.Entity.ApplicationServiceMap>> SelectForService(Guid accessKey, int serviceID)
        {
            Response<List<KMPlatform.Entity.ApplicationServiceMap>> response = new Response<List<KMPlatform.Entity.ApplicationServiceMap>>();
            try
            {
                string param = "serviceID:" + serviceID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ApplicationServiceMap", "SelectForService");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.ApplicationServiceMap worker = new KMPlatform.BusinessLogic.ApplicationServiceMap();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectForService(serviceID);
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
        /// Saves a ApplicationServiceMap object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the ApplicationServiceMap object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, KMPlatform.Entity.ApplicationServiceMap x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<KMPlatform.Entity.ApplicationServiceMap>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ApplicationServiceMap", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.ApplicationServiceMap worker = new KMPlatform.BusinessLogic.ApplicationServiceMap();
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
