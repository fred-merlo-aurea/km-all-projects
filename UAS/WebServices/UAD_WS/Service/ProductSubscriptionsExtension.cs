using System;
using System.Collections.Generic;
using System.Linq;
using UAD_WS.Interface;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    public class ProductSubscriptionsExtension : ServiceBase, IProductSubscriptionsExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="x"></param>
        /// <param name="pubSubscriptionID"></param>
        /// <param name="pubID"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public Response<bool> Save(Guid accessKey, List<FrameworkUAD.Object.PubSubscriptionAdHoc> x, int pubSubscriptionID, int pubID, KMPlatform.Object.ClientConnections client)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = "pubSubscriptionID: " + pubSubscriptionID.ToString() + " , pubID: " + pubID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ProductSubscriptionsExtension", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.ProductSubscriptionsExtension worker = new FrameworkUAD.BusinessLogic.ProductSubscriptionsExtension();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(x, pubSubscriptionID, pubID, client);
                    if (response.Result)
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

        public Response<List<FrameworkUAD.Entity.ProductSubscriptionsExtensionMapper>> SelectAll(Guid accessKey, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.ProductSubscriptionsExtensionMapper>> response = new Response<List<FrameworkUAD.Entity.ProductSubscriptionsExtensionMapper>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "ProductSubscriptionsExtension", "SelectAll");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper worker = new FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper();
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
    }
}
