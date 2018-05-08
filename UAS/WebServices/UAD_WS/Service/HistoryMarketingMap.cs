using UAD_WS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    public class HistoryMarketingMap : ServiceBase, IHistoryMarketingMap
    {

        /// <summary>
        /// Selects a list of HistoryMarketingMap objects based on the subscriber ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="subscriberID">the subscriber ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of HistoryMarketingMap objects</returns>
        public Response<List<FrameworkUAD.Entity.HistoryMarketingMap>> Select(Guid accessKey, int subscriberID, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.HistoryMarketingMap>> response = new Response<List<FrameworkUAD.Entity.HistoryMarketingMap>>();
            try
            {
            string param = "subscriberID:" + subscriberID.ToString();
            FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "HistoryMarketingMap", "Select");
            response.Status = auth.Status;

            if (auth.IsAuthenticated == true)
            {
                FrameworkUAD.BusinessLogic.HistoryMarketingMap worker = new FrameworkUAD.BusinessLogic.HistoryMarketingMap();
                response.Message = "AccessKey Validated";
                response.Result = worker.Select(subscriberID, client);
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
        /// Saves the HistoryMarketingMap object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the HistoryMarketingMap object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAD.Entity.HistoryMarketingMap x, KMPlatform.Object.ClientConnections client)
        {
            Response<int> response = new Response<int>();
            try
            {
            Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
            string param = jf.ToJson<FrameworkUAD.Entity.HistoryMarketingMap>(x);
            FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "HistoryMarketingMap", "Save");
            response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
            {
                FrameworkUAD.BusinessLogic.HistoryMarketingMap worker = new FrameworkUAD.BusinessLogic.HistoryMarketingMap();
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

        /// <summary>
        /// Save a list of HistoryMarketingMap objects for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="list">the list of HistoryMarketingMap objects</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of HistoryMarketingMap objects</returns>
        public Response<List<FrameworkUAD.Entity.HistoryMarketingMap>> SaveBulkUpdate(Guid accessKey, List<FrameworkUAD.Entity.HistoryMarketingMap> list,KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.HistoryMarketingMap>> response = new Response<List<FrameworkUAD.Entity.HistoryMarketingMap>>();
            try
            {
                string param = "subscrberID:" + list.Select(s => s.PubSubscriptionID).Distinct();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "HistoryResponseMap", "SaveBulkUpdate");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.HistoryMarketingMap worker = new FrameworkUAD.BusinessLogic.HistoryMarketingMap();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SaveBulkUpdate(list, client);
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
    }
}
