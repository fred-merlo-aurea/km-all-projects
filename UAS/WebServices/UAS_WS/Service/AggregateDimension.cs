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
    public class AggregateDimension : ServiceBase, IAggregateDimension
    {
        /// <summary>
        /// Selects a list of AggregateDimention objects based on the client ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <returns>reponse.result will contian a list of AggregateDimension objects</returns>
        public Response<List<FrameworkUAS.Entity.AggregateDimension>> Select(Guid accessKey, int clientID)
        {
            Response<List<FrameworkUAS.Entity.AggregateDimension>> response = new Response<List<FrameworkUAS.Entity.AggregateDimension>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, clientID.ToString(), "AggregateDimension", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.AggregateDimension worker = new FrameworkUAS.BusinessLogic.AggregateDimension();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(clientID);
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
