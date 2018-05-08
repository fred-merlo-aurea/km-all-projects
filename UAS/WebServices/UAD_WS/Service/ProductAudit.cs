using System;
using System.Collections.Generic;
using System.Linq;
using UAD_WS.Interface;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    public class ProductAudit : ServiceBase, IProductAudit
    {
        /// <summary>
        /// Selects a list of ProductAudit objects based on the product ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="productId">the product ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of ProductAudit objects</returns>
        public Response<List<FrameworkUAD.Entity.ProductAudit>> Select(Guid accessKey, int productId, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.ProductAudit>> response = new Response<List<FrameworkUAD.Entity.ProductAudit>>();
            try
            {
                string param = " ProductId:" + productId.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ProductAudit", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.ProductAudit worker = new FrameworkUAD.BusinessLogic.ProductAudit();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(productId,client).ToList();
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
        /// Saves the ProductAudit object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the ProductAudit object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Save(Guid accessKey, FrameworkUAD.Entity.ProductAudit x, KMPlatform.Object.ClientConnections client)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD.Entity.ProductAudit>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ProductAudit", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.ProductAudit worker = new FrameworkUAD.BusinessLogic.ProductAudit();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(x, client);
                    if (response.Result == true || response.Result == false)
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
