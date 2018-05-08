using System;
using System.Collections.Generic;
using System.Linq;
using UAD_WS.Interface;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    public class SubscriberProductDemographic : ServiceBase, ISubscriberProductDemographic
    {
        /// <summary>
        /// Selects a list of SubscriberProductDemographic objects based on the subscription ID, pub code, and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="subscriptionID">the subscription ID</param>
        /// <param name="pubCode">the publication code</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberProductDemographic objects</returns>
        public Response<List<FrameworkUAD.Object.SubscriberProductDemographic>> Select(Guid accessKey, int subscriptionID, string pubCode, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Object.SubscriberProductDemographic>> response = new Response<List<FrameworkUAD.Object.SubscriberProductDemographic>>();
            try
            {
                string param = " subscriptionID:" + subscriptionID.ToString() + " pubCode:" + pubCode;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriberProductDemographic", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.SubscriberProductDemographic worker = new FrameworkUAD.BusinessLogic.SubscriberProductDemographic();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(subscriptionID, pubCode, client).ToList();
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
