using UAD_WS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    public class SubscriberMarketingMap : ServiceBase, ISubscriberMarketingMap
    {
        /// <summary>
        /// Selects a list of SubscriberMarketingMap objects based on the subscriber ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="subscriberID">the subscriber ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberMarketingMap objects</returns>
        public Response<List<FrameworkUAD.Object.SubscriberMarketingMap>> Select(Guid accessKey, int subscriberID, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Object.SubscriberMarketingMap>> response = new Response<List<FrameworkUAD.Object.SubscriberMarketingMap>>();
            try
            {
                string param = "subscriberID: " + subscriberID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Subscriber", "UpdateLock");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.SubscriberMarketingMap worker = new FrameworkUAD.BusinessLogic.SubscriberMarketingMap();
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
                LogError(accessKey, ex, this.GetType().Name.ToString());response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
