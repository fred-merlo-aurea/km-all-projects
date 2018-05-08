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
    public class RelationalPubCode : ServiceBase, IRelationalPubCode
    {
        /// <summary>
        /// Selects a list of RelationalPubCode objects based on the client ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <returns>response.result will contain a list of RelationalPubCode objects</returns>
        public Response<List<FrameworkUAS.Entity.RelationalPubCode>> Select(Guid accessKey, int clientID)
        {
            Response<List<FrameworkUAS.Entity.RelationalPubCode>> response = new Response<List<FrameworkUAS.Entity.RelationalPubCode>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "ClientID:" + clientID.ToString(), "RelationalPubCode", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.RelationalPubCode worker = new FrameworkUAS.BusinessLogic.RelationalPubCode();
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

        /// <summary>
        /// Selects a list of RelationalPubCode objects based on the client ID and special file name
        /// </summary>
        /// <param name="accessKey">the acces key</param>
        /// <param name="clientID">the client ID</param>
        /// <param name="specialFileName">the special file name</param>
        /// <returns>response.result will contain a list of RelationalPubCode objects</returns>
        public Response<List<FrameworkUAS.Entity.RelationalPubCode>> Select(Guid accessKey, int clientID, string specialFileName)
        {
            Response<List<FrameworkUAS.Entity.RelationalPubCode>> response = new Response<List<FrameworkUAS.Entity.RelationalPubCode>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "ClientID:" + clientID.ToString() + " specialFileName:" + specialFileName, "RelationalPubCode", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.RelationalPubCode worker = new FrameworkUAS.BusinessLogic.RelationalPubCode();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(clientID, specialFileName);
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
