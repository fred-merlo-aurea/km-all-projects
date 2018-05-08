using UAS_WS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;

namespace UAS_WS.Service
{
    public class PriceCode : ServiceBase, IPriceCode
    {
        /// <summary>
        /// Selects a list of PriceCode objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of PriceCode objects</returns>
        public Response<List<FrameworkUAS.Entity.PriceCode>> Select(Guid accessKey)
        {
            Response<List<FrameworkUAS.Entity.PriceCode>> response = new Response<List<FrameworkUAS.Entity.PriceCode>>();
            try
            {
            string param = string.Empty;
            FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "PriceCode", "Select");
            response.Status = auth.Status;

            if (auth.IsAuthenticated == true)
            {
                FrameworkUAS.BusinessLogic.PriceCode worker = new FrameworkUAS.BusinessLogic.PriceCode();
                response.Message = "AccessKey Validated";
                response.Result = worker.Select();
                if (response.Result != null)
                {
                    response.Message = "Success";
                    response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Success;
                }
                else
                {
                    response.Message = "Error";
                    response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Error;
                }
                }
            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, this.GetType().Name.ToString());response.Message = Core.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a PriceCode object based on the price code and the publication ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="priceCode">the price code</param>
        /// <param name="publicationID">the publication ID</param>
        /// <returns>response.result will contain a PriceCode object</returns>
        public Response<FrameworkUAS.Entity.PriceCode> Select(Guid accessKey, string priceCode, int publicationID)
        {
            Response<FrameworkUAS.Entity.PriceCode> response = new Response<FrameworkUAS.Entity.PriceCode>();
            try
            {
            string param = "priceCode:" + priceCode + " publicationID:" + publicationID.ToString();
            FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "PriceCode", "Select");
            response.Status = auth.Status;

            if (auth.IsAuthenticated == true)
            {
                FrameworkUAS.BusinessLogic.PriceCode worker = new FrameworkUAS.BusinessLogic.PriceCode();
                response.Message = "AccessKey Validated";
                response.Result = worker.Select(priceCode, publicationID);
                if (response.Result != null)
                {
                    response.Message = "Success";
                    response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Success;
                }
                else
                {
                    response.Message = "Error";
                    response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Error;
                }
                }
            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, this.GetType().Name.ToString());response.Message = Core.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Saves the given PriceCode object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the PriceCode object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAS.Entity.PriceCode x)
        {
            Response<int> response = new Response<int>();
            try
            {
            Core.Utilities.JsonFunctions jf = new Core.Utilities.JsonFunctions();
            string param = jf.ToJson<FrameworkUAS.Entity.PriceCode>(x);
            FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "PriceCode", "Save");
            response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
            {
                FrameworkUAS.BusinessLogic.PriceCode worker = new FrameworkUAS.BusinessLogic.PriceCode();
                response.Message = "AccessKey Validated";
                response.Result = worker.Save(x);
                if (response.Result != null)
                {
                    response.Message = "Success";
                    response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Success;
                }
                else
                {
                    response.Message = "Error";
                    response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Error;
                }
                }
            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, this.GetType().Name.ToString());response.Message = Core.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
