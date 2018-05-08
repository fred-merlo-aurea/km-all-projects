using UAD_WS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    public class PriceCode : ServiceBase, IPriceCode
    {
        /// <summary>
        /// Selects a list of PriceCode objects based on the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of PriceCode objects</returns>
        public Response<List<FrameworkUAD.Entity.PriceCode>> Select(Guid accessKey, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.PriceCode>> response = new Response<List<FrameworkUAD.Entity.PriceCode>>();
            try
            {
                string param = string.Empty;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "PriceCode", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.PriceCode worker = new FrameworkUAD.BusinessLogic.PriceCode();
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
                LogError(accessKey, ex, this.GetType().Name.ToString()); response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a PriceCode object based on the price code, the publication ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="priceCode">the price code</param>
        /// <param name="publicationID">the publication ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a PriceCode object</returns>
        public Response<FrameworkUAD.Entity.PriceCode> Select(Guid accessKey, string priceCode, int publicationID, KMPlatform.Object.ClientConnections client)
        {
            Response<FrameworkUAD.Entity.PriceCode> response = new Response<FrameworkUAD.Entity.PriceCode>();
            try
            {
                string param = "priceCode:" + priceCode + " publicationID:" + publicationID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "PriceCode", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.PriceCode worker = new FrameworkUAD.BusinessLogic.PriceCode();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(priceCode, publicationID, client);
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
        /// Saves a PriceCode object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the PriceCode object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAD.Entity.PriceCode x, KMPlatform.Object.ClientConnections client)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD.Entity.PriceCode>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "PriceCode", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.PriceCode worker = new FrameworkUAD.BusinessLogic.PriceCode();
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
                LogError(accessKey, ex, this.GetType().Name.ToString()); response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
