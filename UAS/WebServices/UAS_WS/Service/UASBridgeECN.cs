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
    public class UASBridgeECN : ServiceBase, IUASBridgeECN
    {
        /// <summary>
        /// Selects a list of UASBridgeECN objects based on the user ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="userID">the user ID</param>
        /// <returns>response.result will contain a list of UASBridgeECN objects</returns>
        public Response<List<FrameworkUAS.Entity.UASBridgeECN>> Select(Guid accessKey, int userID)
        {
            Response<List<FrameworkUAS.Entity.UASBridgeECN>> response = new Response<List<FrameworkUAS.Entity.UASBridgeECN>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "UASBridgeECN", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.UASBridgeECN worker = new FrameworkUAS.BusinessLogic.UASBridgeECN();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(userID);
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
                LogError(accessKey, ex, "UASBridgeECN", "Select");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
