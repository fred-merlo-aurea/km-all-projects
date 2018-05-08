using System;
using System.Collections.Generic;
using System.Linq;
using UAS_WS.Interface;
using FrameworkUAS.Service;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class DataCompareCostClient : ServiceBase, IDataCompareCostClient
    {
        /// <summary>
        /// Selects a list of DataCompareCostToClient objects based on the client ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientId">the client ID</param>
        /// <returns>response.result will contain a list of DataCompareCostToClient objects</returns>
        public Response<List<FrameworkUAS.Entity.DataCompareCostClient>> Select(Guid accessKey, int clientId)
        {
            Response<List<FrameworkUAS.Entity.DataCompareCostClient>> response = new Response<List<FrameworkUAS.Entity.DataCompareCostClient>>();
            try
            {
                string param = "ClientID:" + clientId.ToString();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, this.GetType().Name.ToString(), "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.DataCompareCostClient worker = new FrameworkUAS.BusinessLogic.DataCompareCostClient();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(clientId);
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
