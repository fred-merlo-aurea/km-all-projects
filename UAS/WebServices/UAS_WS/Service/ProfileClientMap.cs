using System;
using System.Linq;
using FrameworkUAS.Service;
using UAS_WS.Interface;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class ProfileClientMap : ServiceBase, IProfileClientMap
    {
        /// <summary>
        /// Saves a ProfileClientMap object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the ProfileClientMap object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> Save(Guid accessKey, KMPlatform.Entity.ProfileClientMap x)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<KMPlatform.Entity.ProfileClientMap>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "ProfileClientMap", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.ProfileClientMap worker = new KMPlatform.BusinessLogic.ProfileClientMap();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(x);
                    if (response.Result == true || response.Result == false)
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
