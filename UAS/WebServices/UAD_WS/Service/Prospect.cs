using UAD_WS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    public class Prospect : ServiceBase, IProspect
    {
        /// <summary>
        /// Selects a list of Prospect objects based on the subscriber ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="subscriberID">the subscriber ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of Prospects objects</returns>
        public Response<List<FrameworkUAD.Entity.Prospect>> Select(Guid accessKey, int subscriberID, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.Prospect>> response = new Response<List<FrameworkUAD.Entity.Prospect>>();
            try
            {
                string param = "subscriberID:" + subscriberID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Prospect", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.Prospect worker = new FrameworkUAD.BusinessLogic.Prospect();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(subscriberID, client);
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
        /// Saves a Prospect object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the Prospect object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Save(Guid accessKey, FrameworkUAD.Entity.Prospect x, KMPlatform.Object.ClientConnections client)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD.Entity.Prospect>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Prospect", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.Prospect worker = new FrameworkUAD.BusinessLogic.Prospect();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(x, client);
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
    }
}
