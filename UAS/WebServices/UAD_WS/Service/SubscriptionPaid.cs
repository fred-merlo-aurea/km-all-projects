using UAD_WS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    public class SubscriptionPaid : ServiceBase, ISubscriptionPaid
    {
        /// <summary>
        /// Selects a list of SubscriptionPaid objects based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of SubscriptionPaid objects</returns>
        public Response<List<FrameworkUAD.Entity.SubscriptionPaid>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.SubscriptionPaid>> response = new Response<List<FrameworkUAD.Entity.SubscriptionPaid>>();
            try
            {
            string param = string.Empty;
            FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriptionPaid", "Select");
            response.Status = auth.Status;

            if (auth.IsAuthenticated == true)
            {
                FrameworkUAD.BusinessLogic.SubscriptionPaid worker = new FrameworkUAD.BusinessLogic.SubscriptionPaid();
                response.Message = "AccessKey Validated";
                response.Result = worker.Select(client);
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
        /// Selects a SubscriptionPaid object based on the subscription ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="subscriptionID">the subscription ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a SubscriptionPaid object</returns>
        public Response<FrameworkUAD.Entity.SubscriptionPaid> Select(Guid accessKey, int subscriptionID, KMPlatform.Object.ClientConnections client)
        {
            Response<FrameworkUAD.Entity.SubscriptionPaid> response = new Response<FrameworkUAD.Entity.SubscriptionPaid>();
            try
            {
            string param = "subscriptionID:" + subscriptionID.ToString();
            FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriptionPaid", "Select");
            response.Status = auth.Status;

            if (auth.IsAuthenticated == true)
            {
                FrameworkUAD.BusinessLogic.SubscriptionPaid worker = new FrameworkUAD.BusinessLogic.SubscriptionPaid();
                response.Message = "AccessKey Validated";
                response.Result = worker.Select(subscriptionID, client);
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
        /// Saves a SubscriptionPaid object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the SubscriptionPaid object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAD.Entity.SubscriptionPaid x, KMPlatform.Object.ClientConnections client)
        {
            Response<int> response = new Response<int>();
            try
            {
            Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
            string param = jf.ToJson<FrameworkUAD.Entity.SubscriptionPaid>(x);
            FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriptionPaid", "Save");
            response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
            {
                FrameworkUAD.BusinessLogic.SubscriptionPaid worker = new FrameworkUAD.BusinessLogic.SubscriptionPaid();
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
                LogError(accessKey, ex, this.GetType().Name.ToString());response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
