using System;
using System.Collections.Generic;
using System.Linq;
using UAD_WS.Interface;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    class SubscriberConsensusDemographic : ServiceBase, ISubscriberConsensusDemographic
    {
        /// <summary>
        /// Selects a list of SubscriberConsensusDemographic objects based on the subscription ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="subscriptionID">the subscription ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriberConsensusDemographic objects</returns>
        public Response<List<FrameworkUAD.Object.SubscriberConsensusDemographic>> Select(Guid accessKey, int subscriptionID, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Object.SubscriberConsensusDemographic>> response = new Response<List<FrameworkUAD.Object.SubscriberConsensusDemographic>>();
            try
            {
                string param = " subscriptionID:" + subscriptionID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriberConsensusDemographic", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.SubscriberConsensusDemographic worker = new FrameworkUAD.BusinessLogic.SubscriberConsensusDemographic();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(subscriptionID, client).ToList();
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
