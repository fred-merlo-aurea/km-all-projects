using UAD_WS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;

namespace UAD_WS.Service
{
    public class HistoryResponseMap : ServiceBase, IHistoryResponseMap
    {
        /// <summary>
        /// Selects a list of HistoryResponseMap objects based on the subscription ID and the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="subscriptionID">the subscription ID</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of HistoryResponseMap object</returns>
        public Response<List<FrameworkUAD.Entity.HistoryResponseMap>> Select(Guid accessKey, int subscriptionID, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.HistoryResponseMap>> response = new Response<List<FrameworkUAD.Entity.HistoryResponseMap>>();
            try
            {
                string param = "subscriptionID:" + subscriptionID.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "HistoryResponseMap", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.HistoryResponseMap worker = new FrameworkUAD.BusinessLogic.HistoryResponseMap();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(subscriptionID, client);
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
        /// Saves the HistoryResponseMap object for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the HistoryResponseMap object</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, FrameworkUAD.Entity.HistoryResponseMap x, KMPlatform.Object.ClientConnections client)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD.Entity.HistoryResponseMap>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "HistoryResponseMap", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.HistoryResponseMap worker = new FrameworkUAD.BusinessLogic.HistoryResponseMap();
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
        /// Saves a list of HistoryResponseMap objects for the client
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="list">the list of HistoryResponseMap objects to be saved</param>
        /// <param name="client">the client object</param>
        /// <returns>response.result will contain a list of HistoryResponseMap objects</returns>
        public Response<List<FrameworkUAD.Entity.HistoryResponseMap>> SaveBulkUpdate(Guid accessKey, List<FrameworkUAD.Entity.HistoryResponseMap> list, KMPlatform.Object.ClientConnections client)
        {
            Response<List<FrameworkUAD.Entity.HistoryResponseMap>> response = new Response<List<FrameworkUAD.Entity.HistoryResponseMap>>();
            try
            {
                string param = "subscriptionID:" + list.Select(s => s.SubscriptionID).Distinct();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "HistoryResponseMap", "SaveBulkUpdate");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.HistoryResponseMap worker = new FrameworkUAD.BusinessLogic.HistoryResponseMap();
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
                LogError(accessKey, ex, this.GetType().Name.ToString());response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
