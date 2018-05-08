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
    public class ServiceFeature : ServiceBase, IServiceFeature
    {
        /// <summary>
        /// Selects a list of ServiceFeature objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of ServiceFeature objects</returns>
        public Response<List<KMPlatform.Entity.ServiceFeature>> Select(Guid accessKey)
        {
            Response<List<KMPlatform.Entity.ServiceFeature>> response = new Response<List<KMPlatform.Entity.ServiceFeature>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "ServiceFeature", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.ServiceFeature worker = new KMPlatform.BusinessLogic.ServiceFeature();
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
                LogError(accessKey, ex, "ServiceFeature", "Select");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a list of ServiceFeature objects based on the service ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="serviceID">the service ID</param>
        /// <returns>response.result will contain a list of ServiceFeature objects</returns>
        public Response<List<KMPlatform.Entity.ServiceFeature>> Select(Guid accessKey, int serviceID)
        {
            Response<List<KMPlatform.Entity.ServiceFeature>> response = new Response<List<KMPlatform.Entity.ServiceFeature>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "ServiceID:" + serviceID.ToString(), "ServiceFeature", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.ServiceFeature worker = new KMPlatform.BusinessLogic.ServiceFeature();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(serviceID);
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
                LogError(accessKey, ex, "ServiceFeature", "Select");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a list of ServiceFeature objects that are only enabled and based on the service ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="serviceID">the service ID</param>
        /// <returns>response.result will contain a list of ServiceFeature objects</returns>
        public Response<List<KMPlatform.Entity.ServiceFeature>> SelectOnlyEnabled(Guid accessKey, int serviceID)
        {
            Response<List<KMPlatform.Entity.ServiceFeature>> response = new Response<List<KMPlatform.Entity.ServiceFeature>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "ServiceID:" + serviceID.ToString(), "ServiceFeature", "SelectOnlyEnabled");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.ServiceFeature worker = new KMPlatform.BusinessLogic.ServiceFeature();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectOnlyEnabled(serviceID);
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
                LogError(accessKey, ex, "ServiceFeature", "SelectOnlyEnabled");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a ServiceFeature object based on the service feature ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="serviceFeatureID">the service feature ID</param>
        /// <returns>response.result will contain a ServiceFeature object</returns>
        public Response<KMPlatform.Entity.ServiceFeature> SelectServiceFeature(Guid accessKey, int serviceFeatureID)
        {
            Response<KMPlatform.Entity.ServiceFeature> response = new Response<KMPlatform.Entity.ServiceFeature>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "serviceFeatureID:" + serviceFeatureID.ToString(), "ServiceFeature", "SelectServiceFeature");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.ServiceFeature worker = new KMPlatform.BusinessLogic.ServiceFeature();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectServiceFeature(serviceFeatureID);
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
                LogError(accessKey, ex, "ServiceFeature", "SelectServiceFeature");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Saves a ServiceFeature object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the ServiceFeature object</param>
        /// <returns>reponse.resutl will contain a boolean</returns>
        public Response<bool> Save(Guid accessKey, KMPlatform.Entity.ServiceFeature x)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<KMPlatform.Entity.ServiceFeature>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ServiceFeature", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.ServiceFeature worker = new KMPlatform.BusinessLogic.ServiceFeature();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(x);
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
                LogError(accessKey, ex, "ServiceFeature", "Save");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Saves the service feature ID on the ServiceFeature object as the return ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the ServiceFeature object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> SaveReturnId(Guid accessKey, KMPlatform.Entity.ServiceFeature x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<KMPlatform.Entity.ServiceFeature>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ServiceFeature", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.ServiceFeature worker = new KMPlatform.BusinessLogic.ServiceFeature();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SaveReturnId(x);
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
                LogError(accessKey, ex, "ServiceFeature", "SaveReturnId");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
