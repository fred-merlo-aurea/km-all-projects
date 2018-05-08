using System;
using System.Collections.Generic;
using System.Linq;
using UAD_Lookup_WS.Interface;
using FrameworkUAS.Service;

namespace UAD_Lookup_WS.Service
{
    public class Region : ServiceBase, IRegion
    {
        /// <summary>
        /// Selects a list of Region objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of Region objects</returns>
        public Response<List<FrameworkUAD_Lookup.Entity.Region>> Select(Guid accessKey)
        {
            Response<List<FrameworkUAD_Lookup.Entity.Region>> response = new Response<List<FrameworkUAD_Lookup.Entity.Region>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "Region", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.Region worker = new FrameworkUAD_Lookup.BusinessLogic.Region();
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
