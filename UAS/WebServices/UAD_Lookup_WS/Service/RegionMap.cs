using System;
using System.Collections.Generic;
using System.Linq;
using UAD_Lookup_WS.Interface;
using FrameworkUAS.Service;

namespace UAD_Lookup_WS.Service
{
    public class RegionMap : ServiceBase, IRegionMap
    {
        /// <summary>
        /// Selects a list of RegionMap objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of RegionMap objects</returns>
        public Response<List<FrameworkUAD_Lookup.Entity.RegionMap>> Select(Guid accessKey)
        {
            Response<List<FrameworkUAD_Lookup.Entity.RegionMap>> response = new Response<List<FrameworkUAD_Lookup.Entity.RegionMap>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "RegionMap", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.RegionMap worker = new FrameworkUAD_Lookup.BusinessLogic.RegionMap();
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
