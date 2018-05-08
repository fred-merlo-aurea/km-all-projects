using System;
using System.Collections.Generic;
using System.Linq;
using UAD_WS.Interface;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    public class SubscriptionsExtensionMapper : ServiceBase, ISubscriptionsExtensionMapper
    {
        /// <summary>
        /// Selects all of SubscriptionExtensionMapper objects based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriptionExtensionMapper objects</returns>
        public Response<List<FrameworkUAD.Entity.SubscriptionsExtensionMapper>> SelectAll(Guid accessKey, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.SubscriptionsExtensionMapper>> response = new Response<List<FrameworkUAD.Entity.SubscriptionsExtensionMapper>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "SubscriptionsExtensionMapper", "SelectAll");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.SubscriptionsExtensionMapper worker = new FrameworkUAD.BusinessLogic.SubscriptionsExtensionMapper();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectAll(client).ToList();
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
        /// Saves a SubscriptionExtensionMapper object for the client object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <param name="x">the SubscriptionExtensionMapper object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, KMPlatform.Object.ClientConnections client, FrameworkUAD.Entity.SubscriptionsExtensionMapper x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD.Entity.SubscriptionsExtensionMapper>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Subscription", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.SubscriptionsExtensionMapper worker = new FrameworkUAD.BusinessLogic.SubscriptionsExtensionMapper();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(x, client);
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
