using UAD_WS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    public class HistoryPaidBillTo : ServiceBase, IHistoryPaidBillTo
    {
        /// <summary>
        /// Selects a list of HistoryPaidBillTo objects based on the subscription ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="subscriptionID">the subscirption ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of HistoryPaidBillTo objects</returns>
        public Response<List<FrameworkUAD.Entity.HistoryPaidBillTo>> SelectSubscription(Guid accessKey, int subscriptionID, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.HistoryPaidBillTo>> response = new Response<List<FrameworkUAD.Entity.HistoryPaidBillTo>>();
            try
            {
            string param = "subscriptionID:" + subscriptionID.ToString();
            FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "HistoryPaid", "Select");
            response.Status = auth.Status;

            if (auth.IsAuthenticated == true)
            {
                FrameworkUAD.BusinessLogic.HistoryPaidBillTo worker = new FrameworkUAD.BusinessLogic.HistoryPaidBillTo();
                response.Message = "AccessKey Validated";
                response.Result = worker.SelectSubscription(subscriptionID, client);
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
        /// Selects a HistoryPaidBillTo object based on the subscription paid Id and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="subscriptionPaidID">the subscription paid ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a HistoryPaidBillTo object</returns>
        public Response<FrameworkUAD.Entity.HistoryPaidBillTo> Select(Guid accessKey, int subscriptionPaidID, KMPlatform.Object.ClientConnections client)
        {
            Response<FrameworkUAD.Entity.HistoryPaidBillTo> response = new Response<FrameworkUAD.Entity.HistoryPaidBillTo>();
            try
            {
            string param = "subscriptionPaidID:" + subscriptionPaidID.ToString();
            FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "HistoryPaidBillTo", "Select");
            response.Status = auth.Status;

            if (auth.IsAuthenticated == true)
            {
                FrameworkUAD.BusinessLogic.HistoryPaidBillTo worker = new FrameworkUAD.BusinessLogic.HistoryPaidBillTo();
                response.Message = "AccessKey Validated";
                response.Result = worker.Select(subscriptionPaidID, client);
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
        /// Saves the PaidBillTo object based on the user ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="myPaidBillTo">the PaidBillTo object</param>
        /// <param name="userID">the user ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAD.Entity.PaidBillTo myPaidBillTo, int userID, KMPlatform.Object.ClientConnections client)
        {
            Response<int> response = new Response<int>();
            try
            {
            Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
            string param = jf.ToJson<FrameworkUAD.Entity.PaidBillTo>(myPaidBillTo);
            FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "HistoryPaidBillTo", "Save");
            response.Status = auth.Status;

            if (auth.IsAuthenticated == true)
            {
                FrameworkUAD.BusinessLogic.HistoryPaidBillTo worker = new FrameworkUAD.BusinessLogic.HistoryPaidBillTo();
                response.Message = "AccessKey Validated";
                response.Result = worker.Save(myPaidBillTo, userID, client);
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

        /// <summary>
        /// Saves a HistoryPaidBillTo object based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the HistoryPaidBillTo object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAD.Entity.HistoryPaidBillTo x, KMPlatform.Object.ClientConnections client)
        {
            Response<int> response = new Response<int>();
            try
            {
            Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
            string param = jf.ToJson<FrameworkUAD.Entity.HistoryPaidBillTo>(x);
            FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "HistoryPaidBillTo", "Save");
            response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
            {
                FrameworkUAD.BusinessLogic.HistoryPaidBillTo worker = new FrameworkUAD.BusinessLogic.HistoryPaidBillTo();
                response.Message = "AccessKey Validated";
                response.Result = worker.Save(x, client);
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
