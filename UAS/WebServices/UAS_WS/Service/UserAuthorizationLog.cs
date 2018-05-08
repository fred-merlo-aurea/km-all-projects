using System;
using System.Linq;
using UAS_WS.Interface;
using FrameworkUAS.Service;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class UserAuthorizationLog : ServiceBase, IUserAuthorizationLog
    {
        /// <summary>
        /// Handles user authorization log objects logging out
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="userAuthLogId">the UserAuthorizationLog object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<bool> LogOut(Guid accessKey, int userAuthLogId)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, userAuthLogId.ToString(), "UserAuthorizationLog", "LogOut");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    KMPlatform.BusinessLogic.UserAuthorizationLog worker = new KMPlatform.BusinessLogic.UserAuthorizationLog();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.LogOut(userAuthLogId);
                    if (response.Result == true)
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
                LogError(accessKey, ex, "UserAuthorizationLog", "LogOut");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Sets the user authorization log object from UserAuthorization
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="uAuth">the UserAuthorization object</param>
        /// <param name="userID">the user ID</param>
        /// <returns>response.result will contain a UserAuthorizationLog object</returns>
        public Response<KMPlatform.Entity.UserAuthorizationLog> SetUserAuthLog(Guid accessKey, FrameworkUAS.Object.UserAuthorization uAuth, int userID)
        {
            Response<KMPlatform.Entity.UserAuthorizationLog> response = new Response<KMPlatform.Entity.UserAuthorizationLog>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<KMPlatform.Object.UserAuthorization>(uAuth.GetPlatformUserEntity());
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "UserID:" + userID.ToString() + " " + param, "UserAuthorizationLog", "SetUserAuthLog");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.UserAuthorizationLog worker = new KMPlatform.BusinessLogic.UserAuthorizationLog();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SetUserAuthLog(uAuth.GetPlatformUserEntity(), userID);
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
                LogError(accessKey, ex, "UserAuthorizationLog", "SetUserAuthLog");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Saves the user authorization log object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the user authorization log object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, KMPlatform.Entity.UserAuthorizationLog x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<KMPlatform.Entity.UserAuthorizationLog>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "UserAuthorizationLog", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    KMPlatform.BusinessLogic.UserAuthorizationLog worker = new KMPlatform.BusinessLogic.UserAuthorizationLog();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Save(x);
                    if (response.Result >= 0)
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
                LogError(accessKey, ex, "UserAuthorizationLog", "Save");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
