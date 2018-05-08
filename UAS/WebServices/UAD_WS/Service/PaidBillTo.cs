using UAD_WS.Interface;
using System;
using System.Linq;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    public class PaidBillTo : ServiceBase, IPaidBillTo
    {
        /// <summary>
        /// Selects a PaidBillTo object based on the subscription ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="subscriptionID">the subscription ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a PaidBillTo object</returns>
        public Response<FrameworkUAD.Entity.PaidBillTo> SelectSubscription(Guid accessKey, int subscriptionID, KMPlatform.Object.ClientConnections client)
        {
            Response<FrameworkUAD.Entity.PaidBillTo> response = new Response<FrameworkUAD.Entity.PaidBillTo>();
            try
            {
            string param = "subscriptionID:" + subscriptionID.ToString();
            FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "PaidBillTo", "SelectSubscription");
            response.Status = auth.Status;

            if (auth.IsAuthenticated == true)
            {
                FrameworkUAD.BusinessLogic.PaidBillTo worker = new FrameworkUAD.BusinessLogic.PaidBillTo();
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
        /// Selects a PaidBillTo object based on the subscription paid ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="subscriptionPaidID">the subscription paid ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a PaidBillTo object</returns>
        public Response<FrameworkUAD.Entity.PaidBillTo> Select(Guid accessKey, int subscriptionPaidID, KMPlatform.Object.ClientConnections client)
        {
            Response<FrameworkUAD.Entity.PaidBillTo> response = new Response<FrameworkUAD.Entity.PaidBillTo>();
            try
            {
            string param = "subscriptionPaidID:" + subscriptionPaidID.ToString();
            FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "PaidBillTo", "Select");
            response.Status = auth.Status;

            if (auth.IsAuthenticated == true)
            {
                FrameworkUAD.BusinessLogic.PaidBillTo worker = new FrameworkUAD.BusinessLogic.PaidBillTo();
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
        /// Saves a PaidBillTo object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the PaidBillTo object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAD.Entity.PaidBillTo x, KMPlatform.Object.ClientConnections client)
        {
            Response<int> response = new Response<int>();
            try
            {
            Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
            string param = jf.ToJson<FrameworkUAD.Entity.PaidBillTo>(x);
            FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "PaidBillTo", "Save");
            response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
            {
                FrameworkUAD.BusinessLogic.PaidBillTo worker = new FrameworkUAD.BusinessLogic.PaidBillTo();
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
