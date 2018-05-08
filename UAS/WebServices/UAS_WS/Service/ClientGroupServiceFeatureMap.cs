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
    public class ClientGroupServiceFeatureMap : ServiceBase, IClientGroupServiceFeatureMap
    {
        /// <summary>
        /// Selects a list of ClientGroupServiceFeatureMap objects based on the client group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientGroupID">the client group ID</param>
        /// <returns>response.result will contain a list of ClientGroupServiceFeatureMap objects</returns>
        public Response<List<KMPlatform.Entity.ClientGroupServiceFeatureMap>> SelectForClientGroup(Guid accessKey, int clientGroupID)
        {
            Response<List<KMPlatform.Entity.ClientGroupServiceFeatureMap>> response = new Response<List<KMPlatform.Entity.ClientGroupServiceFeatureMap>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "clientGroupID:" + clientGroupID.ToString(), "ClientServiceFeature", "SelectForClientGroup");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    KMPlatform.BusinessLogic.ClientGroupServiceFeatureMap worker = new KMPlatform.BusinessLogic.ClientGroupServiceFeatureMap();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectForClientGroup(clientGroupID);
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
        /// Selects a list of ClientGroupServiceFeatureMap objects based on the client ID and the service feature ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <param name="serviceFeatureID">the service feature ID</param>
        /// <returns>response.result will contain a list of ClientGroupServiceFeatureMap objects</returns>
        public Response<List<KMPlatform.Entity.ClientGroupServiceFeatureMap>> SelectForServiceFeature(Guid accessKey, int clientID, int serviceFeatureID)
        {
            Response<List<KMPlatform.Entity.ClientGroupServiceFeatureMap>> response = new Response<List<KMPlatform.Entity.ClientGroupServiceFeatureMap>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "ClientID:" + clientID.ToString() + " serviceFeatureID:" + serviceFeatureID.ToString(), "ClientServiceFeature", "SelectForServiceFeature");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    KMPlatform.BusinessLogic.ClientGroupServiceFeatureMap worker = new KMPlatform.BusinessLogic.ClientGroupServiceFeatureMap();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectForServiceFeature(clientID, serviceFeatureID);
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
        /// Selects a list of ClientGroupServiceFeatureMap objects based on the client ID and the service ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <param name="serviceID">the service ID</param>
        /// <returns>response.result will contain a list of ClientGroupServiceFeatureMap objects</returns>
        public Response<List<KMPlatform.Entity.ClientGroupServiceFeatureMap>> SelectForService(Guid accessKey, int clientID, int serviceID)
        {
            Response<List<KMPlatform.Entity.ClientGroupServiceFeatureMap>> response = new Response<List<KMPlatform.Entity.ClientGroupServiceFeatureMap>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "ClientID:" + clientID.ToString() + " ServiceID:" + serviceID.ToString(), "ClientServiceFeature", "SelectForService");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    KMPlatform.BusinessLogic.ClientGroupServiceFeatureMap worker = new KMPlatform.BusinessLogic.ClientGroupServiceFeatureMap();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectForService(clientID, serviceID);
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
        /// Saves a ClientGroupServiceFeatureMap object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the ClientGroupServiceFeatureMap object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, KMPlatform.Entity.ClientGroupServiceFeatureMap x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<KMPlatform.Entity.ClientGroupServiceFeatureMap>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ClientServiceFeature", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    KMPlatform.BusinessLogic.ClientGroupServiceFeatureMap worker = new KMPlatform.BusinessLogic.ClientGroupServiceFeatureMap();
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
