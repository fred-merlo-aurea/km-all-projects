using System;
using System.Linq;
using UAS_WS.Interface;
using FrameworkUAS.Service;
using System.ServiceModel.Activation;
using System.ServiceModel;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class UserAuthorization : ServiceBase, IUserAuthorization
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="saltValue"></param>
        /// <param name="authSource"></param>
        /// <param name="ipAddress"></param>
        /// <param name="serverVariables"></param>
        /// <param name="appVersion"></param>
        /// <param name="useLite"></param>
        /// <returns></returns>
        public Response<KMPlatform.Object.UserAuthorization> Login(string userName, string password, string saltValue, string authSource, string ipAddress = "", KMPlatform.Object.ServerVariable serverVariables = null, string appVersion = "", bool useLite = true)
        {
            Response<KMPlatform.Object.UserAuthorization> response = new Response<KMPlatform.Object.UserAuthorization>();
            try
            {
                KMPlatform.BusinessLogic.UserAuthorization worker = new KMPlatform.BusinessLogic.UserAuthorization();
                response.Result = worker.Login(userName, password, saltValue, "API", ipAddress, serverVariables, appVersion, useLite);
                response.Message = "AccessKey Validated";
                //response.Result = new FrameworkUAS.Object.UserAuthorization(ua);
                if (response.Result != null)
                {
                    response.Message = "Success";
                    response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success;
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType() != typeof(KMPlatform.Object.UserLoginException))
                {
                    response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                    Guid accessKey = Guid.NewGuid();
                    LogError(accessKey, ex, "UserAuthorization", "Login");
                    response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
                }
                else
                {
                    KMPlatform.Object.UserLoginException ule = ex as KMPlatform.Object.UserLoginException;
                    if (ule.UserStatus == KMPlatform.Enums.UserLoginStatus.InvalidPassword)
                        response.Message = "Incorrect Login";
                    else if (ule.UserStatus == KMPlatform.Enums.UserLoginStatus.DisabledUser)
                        response.Message = "Disabled";
                    else if (ule.UserStatus == KMPlatform.Enums.UserLoginStatus.LockedUser)
                        response.Message = "Locked";
                    else if (ule.UserStatus == KMPlatform.Enums.UserLoginStatus.NoRoles)
                        response.Message = "No Roles";
                }
            }
            return response;
        }

        /// <summary>
        /// Handles user authorization logins without the username, password and salt value
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="authSource">the authorization source</param>
        /// <param name="ipAddress">the IP address</param>
        /// <param name="serverVariables">the server variables</param>
        /// <param name="appVersion">the application version</param>
        /// <returns>response.result will contain a UserAuthorization object</returns>
        public Response<KMPlatform.Object.UserAuthorization> Login(Guid accessKey, string authSource, string ipAddress = "", KMPlatform.Object.ServerVariable serverVariables = null, string appVersion = "")
        {
            Response<KMPlatform.Object.UserAuthorization> response = new Response<KMPlatform.Object.UserAuthorization>();
            try
            {
                KMPlatform.BusinessLogic.UserAuthorization worker = new KMPlatform.BusinessLogic.UserAuthorization();

                response.Result = worker.Login(accessKey, "API", ipAddress, serverVariables, appVersion);
                //response.Result = new FrameworkUAS.Object.UserAuthorization(ua);
                if (response.Result != null && response.Result.IsAuthenticated == true)
                {
                    response.Message = "AccessKey Validated";

                    response.Message = "Success";
                    response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success;
                }
            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, "UserAuthorization", "Login");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Gets the IP address of the user
        /// </summary>
        /// <returns>response.result will contain a string</returns>
        public Response<string> GetIpAddress()
        {
            Response<string> response = new Response<string>();
            try
            {
                response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.No_Access_Key_Required;

                KMPlatform.BusinessLogic.UserAuthorization worker = new KMPlatform.BusinessLogic.UserAuthorization();
                response.Message = "No AccessKey Required";
                response.Result = worker.GetIpAddress();
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
            catch (Exception ex)
            {
                response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                Guid accessKey = Guid.NewGuid();
                LogError(accessKey, ex, "UserAuthorization", "GetIpAddress");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
