using UAD_WS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    public class SubscriptionSearchResult : ServiceBase, ISubscriptionSearchResult
    {
        /// <summary>
        /// Selects a list of SubscriptionSearchResult objects based on the subscriber ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="subscriberID">the subscriber ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriptionSearchResult objects</returns>
        public Response<List<FrameworkUAD.Object.SubscriptionSearchResult>> Select(Guid accessKey, int subscriberID, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Object.SubscriptionSearchResult>> response = new Response<List<FrameworkUAD.Object.SubscriptionSearchResult>>();
            try
            {
                string param = "subscriberID:" + subscriberID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriptionSearchResult", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.SubscriptionSearchResult worker = new FrameworkUAD.BusinessLogic.SubscriptionSearchResult();
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

        /// <summary>
        /// Selects a list of SubscriptionSearchResult objects based on multiple subscriber IDs and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="subscriberIDs">the list of subscriber IDs</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriptionSearchResult objects</returns>
        public Response<List<FrameworkUAD.Object.SubscriptionSearchResult>> SelectMultiple(Guid accessKey, List<int> subscriberIDs, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Object.SubscriptionSearchResult>> response = new Response<List<FrameworkUAD.Object.SubscriptionSearchResult>>();
            try
            {
                string param = "subscriberID:" + subscriberIDs;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriptionSearchResult", "SelectMultiple");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.SubscriptionSearchResult worker = new FrameworkUAD.BusinessLogic.SubscriptionSearchResult();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectMultiple(subscriberIDs, client);
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
                LogError(accessKey, ex, this.GetType().Name.ToString()); response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
