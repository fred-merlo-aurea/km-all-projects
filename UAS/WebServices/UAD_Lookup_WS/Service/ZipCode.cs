using UAD_Lookup_WS.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkUAS.Service;

namespace UAD_Lookup_WS.Service
{
    public class ZipCode : ServiceBase, IZipCode
    {
        public Response<List<FrameworkUAD_Lookup.Entity.ZipCode>> Select(Guid accessKey)
        {
            Response<List<FrameworkUAD_Lookup.Entity.ZipCode>> response = new Response<List<FrameworkUAD_Lookup.Entity.ZipCode>>();
            try
            {
                string param = "";
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ZipCode", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD_Lookup.BusinessLogic.ZipCode worker = new FrameworkUAD_Lookup.BusinessLogic.ZipCode();
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
