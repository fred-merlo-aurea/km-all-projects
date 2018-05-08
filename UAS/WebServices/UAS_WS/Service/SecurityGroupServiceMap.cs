using FrameworkUAS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using UAS_WS.Interface;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class SecurityGroupServiceMap : ServiceBase, ISecurityGroupServiceMap
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        public Response<List<FrameworkUAS.Entity.SecurityGroupServiceMap>> Select(Guid accessKey)
        {
            Response<List<FrameworkUAS.Entity.SecurityGroupServiceMap>> response = new Response<List<FrameworkUAS.Entity.SecurityGroupServiceMap>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "SecurityGroupServiceMap", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAS.BusinessLogic.SecurityGroupServiceMap worker = new FrameworkUAS.BusinessLogic.SecurityGroupServiceMap();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select();
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
