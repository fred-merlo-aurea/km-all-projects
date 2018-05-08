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
    public class ClientGroupClientMap : ServiceBase, IClientGroupClientMap
    {
        /// <summary>
        /// Selects a list of ClientGroupClientMap objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of ClientGroupClientMap objects</returns>
        public Response<List<KMPlatform.Entity.ClientGroupClientMap>> Select(Guid accessKey)
        {
            Response<List<KMPlatform.Entity.ClientGroupClientMap>> response = new Response<List<KMPlatform.Entity.ClientGroupClientMap>>();
            try { 
            string param = string.Empty;
            FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ClientGroupClientMap", "Select");
            response.Status = auth.Status;

            if (auth.IsAuthenticated == true)
            {
                KMPlatform.BusinessLogic.ClientGroupClientMap worker = new KMPlatform.BusinessLogic.ClientGroupClientMap();
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
        /// Selects a list of ClientGroupClientMap objects based on the client group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientGroupID">the client group ID</param>
        /// <returns>response.result will contain a list of ClientGroupClientMap objects</returns>
        public Response<List<KMPlatform.Entity.ClientGroupClientMap>> SelectForClientGroup(Guid accessKey, int clientGroupID)
        {
            Response<List<KMPlatform.Entity.ClientGroupClientMap>> response = new Response<List<KMPlatform.Entity.ClientGroupClientMap>>();
            try { 
            string param = "clientGroupID: " + clientGroupID.ToString();
            FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SelectForClientGroup", "Select");
            response.Status = auth.Status;

            if (auth.IsAuthenticated == true)
            {
                KMPlatform.BusinessLogic.ClientGroupClientMap worker = new KMPlatform.BusinessLogic.ClientGroupClientMap();
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
        /// Selects a list of ClientGroupClientMap objects based on the client ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <returns>response.result will contain a list of ClientGroupClientMap objects</returns>
         public Response<List<KMPlatform.Entity.ClientGroupClientMap>> SelectForClientID(Guid accessKey, int clientID)
        {
            Response<List<KMPlatform.Entity.ClientGroupClientMap>> response = new Response<List<KMPlatform.Entity.ClientGroupClientMap>>();
            try { 
            string param = "clientID: " + clientID.ToString();
            FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ClientGroupClientMap", "SelectForClientID");
            response.Status = auth.Status;

            if (auth.IsAuthenticated == true)
            {
                KMPlatform.BusinessLogic.ClientGroupClientMap worker = new KMPlatform.BusinessLogic.ClientGroupClientMap();
                response.Message = "AccessKey Validated";
                response.Result = worker.SelectForClientID(clientID);
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
         /// Saves a ClientGroupClientMap object
        /// </summary>
        /// <param name="accessKey">the access key</param>
         /// <param name="x">the ClientGroupClientMap object</param>
        /// <returns>response.result will contain an integer</returns>
         public Response<int> Save(Guid accessKey, KMPlatform.Entity.ClientGroupClientMap x)
         {
             Response<int> response = new Response<int>();
             try { 
             Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
             string param = jf.ToJson<KMPlatform.Entity.ClientGroupClientMap>(x);
             FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ClientGroupClientMap", "Save");
             response.Status = auth.Status;

             if (auth.IsAuthenticated == true)
             {
                 KMPlatform.BusinessLogic.ClientGroupClientMap worker = new KMPlatform.BusinessLogic.ClientGroupClientMap();
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
