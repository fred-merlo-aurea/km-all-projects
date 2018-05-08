using UAD_Lookup_WS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;

namespace UAD_Lookup_WS.Service
{
    public class SubscriptionStatus : ServiceBase, ISubscriptionStatus
    {
        /// <summary>
        /// Selects a list of SubscriptionStatus objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of SubscriptionStatus objects</returns>
        public Response<List<FrameworkUAD_Lookup.Entity.SubscriptionStatus>> Select(Guid accessKey)
        {
            Response<List<FrameworkUAD_Lookup.Entity.SubscriptionStatus>> response = new Response<List<FrameworkUAD_Lookup.Entity.SubscriptionStatus>>();
            try
            {
                string param = string.Empty;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriptionStatus", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.SubscriptionStatus worker = new FrameworkUAD_Lookup.BusinessLogic.SubscriptionStatus();
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
                LogError(accessKey, ex, this.GetType().Name.ToString()); response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a SubscriptionStatus object based on the category code ID and the transaction code ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="categoryCodeID">the category code ID</param>
        /// <param name="transactionCodeID">the transaction code ID</param>
        /// <returns>resposne.result will contain a SubscriptionStatus object</returns>
        public Response<FrameworkUAD_Lookup.Entity.SubscriptionStatus> Select(Guid accessKey, int categoryCodeID, int transactionCodeID)
        {
            Response<FrameworkUAD_Lookup.Entity.SubscriptionStatus> response = new Response<FrameworkUAD_Lookup.Entity.SubscriptionStatus>();
            try
            {
                string param = "categoryCodeID:" + categoryCodeID.ToString() + " transactionCodeID:" + transactionCodeID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriptionStatus", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.SubscriptionStatus worker = new FrameworkUAD_Lookup.BusinessLogic.SubscriptionStatus();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(categoryCodeID, transactionCodeID);
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

        /// <summary>
        /// Selects a SubscriptionStatus object based on the subscription status ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="subscriptionStatusID">the subscription status ID</param>
        /// <returns>response.result will contain a SubscriptionStatus object</returns>
        public Response<FrameworkUAD_Lookup.Entity.SubscriptionStatus> Select(Guid accessKey, int subscriptionStatusID)
        {
            Response<FrameworkUAD_Lookup.Entity.SubscriptionStatus> response = new Response<FrameworkUAD_Lookup.Entity.SubscriptionStatus>();
            try
            {
                string param = "subscriptionStatusID:" + subscriptionStatusID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriptionStatus", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.SubscriptionStatus worker = new FrameworkUAD_Lookup.BusinessLogic.SubscriptionStatus();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(subscriptionStatusID);
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

        /// <summary>
        /// Saves the given SubscriptionStatus object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the SubscriptionStatus object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAD_Lookup.Entity.SubscriptionStatus x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD_Lookup.Entity.SubscriptionStatus>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriptionStatus", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.SubscriptionStatus worker = new FrameworkUAD_Lookup.BusinessLogic.SubscriptionStatus();
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
                LogError(accessKey, ex, this.GetType().Name.ToString()); response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
