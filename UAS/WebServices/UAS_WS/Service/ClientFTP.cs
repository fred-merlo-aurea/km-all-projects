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
    public class ClientFTP : ServiceBase, IClientFTP
    {
        /// <summary>
        /// Select a list of ClientFTP objects based on the client ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <returns>response.result will contain a list of ClientFTP objects</returns>
        public Response<List<FrameworkUAS.Entity.ClientFTP>> Select(Guid accessKey, int clientID)
        {

            Response<List<FrameworkUAS.Entity.ClientFTP>> response = new Response<List<FrameworkUAS.Entity.ClientFTP>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "ClientID:" + clientID.ToString(), "ClientFTP", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.ClientFTP worker = new FrameworkUAS.BusinessLogic.ClientFTP();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectClient(clientID);
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
        /// Saves a ClientFTP object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the ClientFTP object</param>
        /// <returns>resposne.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAS.Entity.ClientFTP x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAS.Entity.ClientFTP>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ClientFTP", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    FrameworkUAS.BusinessLogic.ClientFTP worker = new FrameworkUAS.BusinessLogic.ClientFTP();
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
