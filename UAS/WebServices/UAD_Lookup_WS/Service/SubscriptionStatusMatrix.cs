using UAD_Lookup_WS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;

namespace UAD_Lookup_WS.Service
{
    public class SubscriptionStatusMatrix : ServiceBase, ISubscriptionStatusMatrix
    {
        /// <summary>
        /// Selects a list of SubscriptionStatusMatix objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of SubscriptionStatusMatrix objects</returns>
        public Response<List<FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix>> Select(Guid accessKey)
        {
            Response<List<FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix>> response = new Response<List<FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix>>();
            try
            {
                string param = string.Empty;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriptionStatusMatrix", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.SubscriptionStatusMatrix worker = new FrameworkUAD_Lookup.BusinessLogic.SubscriptionStatusMatrix();
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
        /// Selects a SubscriptionStatusMatrix object based on the subscription status ID, category code ID and the transaction code ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="subscriptionStatusID">the subscription status ID</param>
        /// <param name="categoryCodeID">the category code ID</param>
        /// <param name="transactionCodeID">the transaction code ID</param>
        /// <returns>response.result will contain a SubscriptionStatusMatrix object</returns>
        public Response<FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix> Select(Guid accessKey, int subscriptionStatusID, int categoryCodeID, int transactionCodeID)
        {
            Response<FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix> response = new Response<FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix>();
            try
            {
                string param = "subscriptionStatusID:" + subscriptionStatusID.ToString() + " categoryCodeID:" + categoryCodeID.ToString() + " transactionCodeID:" + transactionCodeID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriptionStatusMatrix", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.SubscriptionStatusMatrix worker = new FrameworkUAD_Lookup.BusinessLogic.SubscriptionStatusMatrix();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(subscriptionStatusID, categoryCodeID, transactionCodeID);
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
        /// Saves the given SubscriptionStatusMatrix object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the SubscriptionStatusMatrix object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD_Lookup.Entity.SubscriptionStatusMatrix>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriptionStatusMatrix", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.SubscriptionStatusMatrix worker = new FrameworkUAD_Lookup.BusinessLogic.SubscriptionStatusMatrix();
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
