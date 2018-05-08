using System;
using System.Linq;
using SubGen_WS.Interface;
using FrameworkUAS.Service;

namespace SubGen_WS.Service
{
    public class SubGenUtils : ServiceBase, ISubGenUtils
    {
        public Response<string> GetLoginToken(Guid accessKey, FrameworkSubGen.Entity.Enums.Client sgClient, KMPlatform.Entity.User user, bool isSgAdmin)
        {
            Response<string> response = new Response<string>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, " SubGenClient:" + sgClient.ToString() + " UserId:" + user.UserID.ToString(), "SubGenUtils", "GetLoginToken");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkSubGen.SubGenUtils worker = new FrameworkSubGen.SubGenUtils();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.GetLoginToken(sgClient, user, isSgAdmin);
                    if (!string.IsNullOrEmpty(response.Result))
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

        public Response<string> GetTestingLoginToken(Guid accessKey, KMPlatform.Entity.User user, bool isSgAdmin)
        {
            Response<string> response = new Response<string>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, " UserId:" + user.UserID.ToString(), "SubGenUtils", "GetTestingLoginToken");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkSubGen.SubGenUtils worker = new FrameworkSubGen.SubGenUtils();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.GetTestingLoginToken(user, isSgAdmin);
                    if (!string.IsNullOrEmpty(response.Result))
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
