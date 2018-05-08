using UAD_WS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    public class MarketingMap : ServiceBase, IMarketingMap
    {
        /// <summary>
        /// Selects a MarketingMap object based on the marketing ID, the subscriber ID, the publication ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="marketingID">the marketing ID</param>
        /// <param name="subscriberID">the subscriber ID</param>
        /// <param name="publicationID">the publication ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a MarketingMap object</returns>
        public Response<FrameworkUAD.Entity.MarketingMap> Select(Guid accessKey, int marketingID, int subscriberID, int publicationID, KMPlatform.Object.ClientConnections client)
        {
            Response<FrameworkUAD.Entity.MarketingMap> response = new Response<FrameworkUAD.Entity.MarketingMap>();
            try
            {
            string param = "marketingID:" + marketingID.ToString() + " subscriberID:" + subscriberID.ToString() + " publicationID:" + publicationID.ToString();
            FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "MarketingMap", "Select");
            response.Status = auth.Status;

            if (auth.IsAuthenticated == true)
            {
                FrameworkUAD.BusinessLogic.MarketingMap worker = new FrameworkUAD.BusinessLogic.MarketingMap();
                response.Message = "AccessKey Validated";
                response.Result = worker.Select(marketingID, subscriberID, publicationID,client);
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
        /// Selects a list of MarketingMap objects based on the publication ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="publicationID">the publication ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of MarketingMap objects</returns>
        public Response<List<FrameworkUAD.Entity.MarketingMap>> SelectPublication(Guid accessKey, int publicationID, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.MarketingMap>> response = new Response<List<FrameworkUAD.Entity.MarketingMap>>();
            try
            {
            string param = "publicationID:" + publicationID.ToString();
            FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "MarketingMap", "SelectPublication");
            response.Status = auth.Status;

            if (auth.IsAuthenticated == true)
            {
                FrameworkUAD.BusinessLogic.MarketingMap worker = new FrameworkUAD.BusinessLogic.MarketingMap();
                response.Message = "AccessKey Validated";
                response.Result = worker.SelectPublication(publicationID,client);
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
        /// Selects a list of MarketingMap objects based on the subscriber Id and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="PubSubscriptionID">the subscriber ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of MarketingMap objects</returns>
        public Response<List<FrameworkUAD.Entity.MarketingMap>> SelectSubscriber(Guid accessKey, int PubSubscriptionID, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.MarketingMap>> response = new Response<List<FrameworkUAD.Entity.MarketingMap>>();
            try
            {
                string param = "subscriberID:" + PubSubscriptionID.ToString();
            FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "MarketingMap", "SelectSubscriber");
            response.Status = auth.Status;

            if (auth.IsAuthenticated == true)
            {
                FrameworkUAD.BusinessLogic.MarketingMap worker = new FrameworkUAD.BusinessLogic.MarketingMap();
                response.Message = "AccessKey Validated";
                response.Result = worker.SelectSubscriber(PubSubscriptionID, client);
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
        /// Saves the MarketingMap object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the MarketingMap object</param>
        /// <param name="client">the client</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Save(Guid accessKey, FrameworkUAD.Entity.MarketingMap x, KMPlatform.Object.ClientConnections client)
        {
            Response<bool> response = new Response<bool>();
            try
            {
            Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
            string param = jf.ToJson<FrameworkUAD.Entity.MarketingMap>(x);
            FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "MarketingMap", "Save");
            response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
            {
                FrameworkUAD.BusinessLogic.MarketingMap worker = new FrameworkUAD.BusinessLogic.MarketingMap();
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
                LogError(accessKey, ex, this.GetType().Name.ToString());response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Saves a list of MarketingMap for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="list">the MarketingMap list to be saved</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> SaveBulkUpdate(Guid accessKey, List<FrameworkUAD.Entity.MarketingMap> list, KMPlatform.Object.ClientConnections client)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                string param = "subscriberID:" + list.Select(s => s.PubSubscriptionID).Distinct();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "MarketingMap", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.MarketingMap worker = new FrameworkUAD.BusinessLogic.MarketingMap();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SaveBulkUpdate(list, client);
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
                LogError(accessKey, ex, this.GetType().Name.ToString()); response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
