using System;
using System.Collections.Generic;
using System.Linq;
using UAD_Lookup_WS.Interface;
using FrameworkUAS.Service;

namespace UAD_Lookup_WS.Service
{
    public class CountryMap : ServiceBase, ICountryMap
    {
        /// <summary>
        /// Selects a list of CountryMap objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of CountryMap objects</returns>
        public Response<List<FrameworkUAD_Lookup.Entity.CountryMap>> Select(Guid accessKey)
        {
            Response<List<FrameworkUAD_Lookup.Entity.CountryMap>> response = new Response<List<FrameworkUAD_Lookup.Entity.CountryMap>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "CountryMap", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.CountryMap worker = new FrameworkUAD_Lookup.BusinessLogic.CountryMap();
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
