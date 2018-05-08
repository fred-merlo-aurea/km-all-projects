using System;
using System.Collections.Generic;
using System.Linq;
using UAD_WS.Interface;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    public class Databases : ServiceBase, IDatabases
    {
        /// <summary>
        /// Selects a list of Databases objects based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of Databases objects</returns>
        public Response<List<FrameworkUAD.Entity.Databases>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.Databases>> response = new Response<List<FrameworkUAD.Entity.Databases>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "", "Databases", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.Databases worker = new FrameworkUAD.BusinessLogic.Databases();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(client).ToList();
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
    }
}
