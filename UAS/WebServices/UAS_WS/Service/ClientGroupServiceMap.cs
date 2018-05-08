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
    public class ClientGroupServiceMap : ServiceBase, IClientGroupServiceMap
    {
        /// <summary>
        /// Selects a list of ClientGroupServiceMap objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of ClientGroupServiceMap objects</returns>
        public Response<List<KMPlatform.Entity.ClientGroupServiceMap>> Select(Guid accessKey)
        {
            Response<List<KMPlatform.Entity.ClientGroupServiceMap>> response = new Response<List<KMPlatform.Entity.ClientGroupServiceMap>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "ClientGroupServiceMap", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    KMPlatform.BusinessLogic.ClientGroupServiceMap worker = new KMPlatform.BusinessLogic.ClientGroupServiceMap();
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
        /// Selects a list of ClientGroupServiceMap objects based on the client group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientGroupID">the client group ID</param>
        /// <returns>response.result will contain a list of ClientGroupServiceMap objects</returns>
        public Response<List<KMPlatform.Entity.ClientGroupServiceMap>> Select(Guid accessKey, int clientGroupID)
        {
            Response<List<KMPlatform.Entity.ClientGroupServiceMap>> response = new Response<List<KMPlatform.Entity.ClientGroupServiceMap>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "ClientGroupID:" + clientGroupID.ToString(), "ClientGroupServiceMap", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    KMPlatform.BusinessLogic.ClientGroupServiceMap worker = new KMPlatform.BusinessLogic.ClientGroupServiceMap();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(clientGroupID);
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
        /// Saves a ClientGroupServiceMap object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the ClientGroupServiceMap object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, KMPlatform.Entity.ClientGroupServiceMap x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<KMPlatform.Entity.ClientGroupServiceMap>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ClientGroupServiceMap", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    KMPlatform.BusinessLogic.ClientGroupServiceMap worker = new KMPlatform.BusinessLogic.ClientGroupServiceMap();
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
