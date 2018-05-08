using System;
using System.Collections.Generic;
using System.Linq;
using UAS_WS.Interface;
using FrameworkUAS.Service;
using WebServiceFramework;
using UtilityJsonFunctions = Core_AMS.Utilities.JsonFunctions;
using EntityUser = KMPlatform.Entity.User;
using UserBusinessLogic = KMPlatform.BusinessLogic.User;
using EmailDirectBusinessLogic = ECN_Framework_BusinessLayer.Communicator.EmailDirect;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class User : FrameworkServiceBase, IUser
    {
        private const string EntityNameUser = "User";
        private const string MethodSelect = "Select";
        private const string MethodSetUserObjects = "SetUserObjects";
        private const string MethodLogIn = "LogIn";
        private const string MethodSearchUserName = "SearchUserName";
        private const string MethodSetAuthorizedUserObjects = "SetAuthorizedUserObjects";
        private const string MethodEmailExist = "EmailExist";
        private const string MethodSearch = "Search";
        private const string MethodSelectUser = "SelectUser";
        private const string MethodSave = "Save";
        private const string MethodIsKmUser = "IsKmUser";

        /// <summary>
        /// Selects a list of User objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="includeObjects">boolean to include objects after decryption</param>
        /// <returns>response.result will contain a list of User objects</returns>
        public Response<List<EntityUser>> Select(Guid accessKey, bool includeObjects = false)
        {
            var model = new ServiceRequestModel<List<EntityUser>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityNameUser,
                AuthenticateMethod = MethodSelect,
                WorkerFunc = _ =>
                {
                    var worker = new UserBusinessLogic();
                    return worker.Select(includeObjects);
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of user objects based on client group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientGroupID">the client group ID</param>
        /// <param name="includeObjects">boolean to include objects after decryption</param>
        /// <returns>response.result will contain a list of User objects</returns>
        public Response<List<EntityUser>> Select(Guid accessKey, int clientGroupID, bool includeObjects = false)
        {
            var param = $"ClientGroupID: {clientGroupID}";
            var model = new ServiceRequestModel<List<EntityUser>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityNameUser,
                AuthenticateMethod = MethodSelect
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of user objects based on client ID and security group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <param name="securityGroupID">the security group ID</param>
        /// <param name="includeObjects">boolean to include objects after decryption</param>
        /// <returns>response.result will contain a list of User objects</returns>
        public Response<List<EntityUser>> Select(Guid accessKey, int clientID, int securityGroupID, bool includeObjects = false)
        {
            var param = $"ClientID: {clientID} SecurityGroupID:{securityGroupID}";
            var model = new ServiceRequestModel<List<EntityUser>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityNameUser,
                AuthenticateMethod = MethodSelect
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of user objects based on client ID and security group name
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientID">the client ID</param>
        /// <param name="securityGroupName">the security group name</param>
        /// <param name="includeObjects">boolean to include objects after decryption</param>
        /// <returns>response.result will contain a list of User objects</returns>
        public Response<List<EntityUser>> Select(Guid accessKey, int clientID, string securityGroupName, bool includeObjects = false)
        {
            var param = $"ClientID: {clientID} SecurityGroupName:{securityGroupName}";
            var model = new ServiceRequestModel<List<EntityUser>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityNameUser,
                AuthenticateMethod = MethodSelect
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Handles user log ins
        /// </summary>
        /// <param name="userName">the username</param>
        /// <param name="password">the password</param>
        /// <param name="includeObjects">boolean to include objects after decryption</param>
        /// <returns>response.result will contain a User object</returns>
        public Response<KMPlatform.Entity.User> LogIn(string userName, string password, bool includeObjects = false)
        {
            Response<KMPlatform.Entity.User> response = new Response<KMPlatform.Entity.User>();
            try
            {
                KMPlatform.BusinessLogic.User pluWorker = new KMPlatform.BusinessLogic.User();
                response.Message = "AccessKey Validated";
                response.Result = pluWorker.LogIn(userName, password, includeObjects);
                KMPlatform.BusinessLogic.UserLog ulWorker = new KMPlatform.BusinessLogic.UserLog();
                int userID = -1;
                if (response.Result != null)
                {
                    userID = response.Result.UserID;
                }

                ulWorker.LogIn(userName, password, userID);
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
                Guid accessKey = Guid.NewGuid();
                response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, this.GetType().Name.ToString());
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Handles user log ins without the username or password
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="includeObjects">boolean to include objects after decryption</param>
        /// <returns>response.result will contain a User object</returns>
        public Response<EntityUser> LogIn(Guid accessKey, bool includeObjects = false)
        {
            var param = $"AccessKey:{accessKey} IncludeMenu:{includeObjects}";
            var model = new ServiceRequestModel<EntityUser>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityNameUser,
                AuthenticateMethod = MethodLogIn,
                WorkerFunc = _ =>
                {
                    var worker = new UserBusinessLogic();
                    return worker.LogIn(accessKey, includeObjects);
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Sets the given user object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="user">the user object</param>
        /// <returns>response.result will contain a User object</returns>
        public Response<EntityUser> SetUserObjects(Guid accessKey, EntityUser user)
        {
            var jsonFunction = new UtilityJsonFunctions();
            var param = jsonFunction.ToJson(user);
            var model = new ServiceRequestModel<EntityUser>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityNameUser,
                AuthenticateMethod = MethodSetUserObjects,
                WorkerFunc = _ =>
                {
                    var worker = new UserBusinessLogic();
                    return worker.SetUserObjects(user);
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Searches the given username
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="userName">the username</param>
        /// <returns>response.result will contain a User object</returns>
        public Response<EntityUser> SearchUserName(Guid accessKey, string userName)
        {
            var param = $"UserName:{userName}";
            var model = new ServiceRequestModel<EntityUser>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityNameUser,
                AuthenticateMethod = MethodSearchUserName,
                WorkerFunc = _ =>
                {
                    var worker = new UserBusinessLogic();
                    return worker.SearchUserName(userName);
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Finds forgotten passwords using the given email address
        /// </summary>
        /// <param name="emailAddress">the email address</param>
        /// <returns>response.result will contain a User object</returns>
        public Response<KMPlatform.Entity.User> FindForgotPassword(string emailAddress)
        {
            Response<KMPlatform.Entity.User> response = new Response<KMPlatform.Entity.User>();
            try
            {
                response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.No_Access_Key_Required;
                KMPlatform.BusinessLogic.User worker = new KMPlatform.BusinessLogic.User();
                response.Message = "No Access Key Required";
                response.Result = worker.SearchEmail(emailAddress);
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
                Guid accessKey = Guid.NewGuid();
                response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, this.GetType().Name.ToString());
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Sets authorized user objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="user">the user object</param>
        /// <returns>response.result will contain a User object</returns>
        public Response<EntityUser> SetAuthorizedUserObjects(Guid accessKey, EntityUser user)
        {
            var jsonFunction = new UtilityJsonFunctions();
            var param = jsonFunction.ToJson(user);
            var model = new ServiceRequestModel<EntityUser>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityNameUser,
                AuthenticateMethod = MethodSetAuthorizedUserObjects
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Gets an email address based on the given username
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="userName">the username</param>
        /// <returns>response.result will contain a String</returns>
        public Response<string> GetEmailAddress(Guid accessKey, string userName)
        {
            var param = $"UserName:{userName}";
            var model = new ServiceRequestModel<string>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityNameUser,
                AuthenticateMethod = MethodLogIn,
                WorkerFunc = _ =>
                {
                    var worker = new UserBusinessLogic();
                    return worker.GetEmailAddress(userName);
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Checks to see if the given email exists
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="email">the email</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> EmailExist(Guid accessKey, string email)
        {
            var param = $"Email:{email}";
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityNameUser,
                AuthenticateMethod = MethodEmailExist,
                WorkerFunc = request =>
                {
                    var worker = new UserBusinessLogic();
                    return worker.EmailExist(email);
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Searches for a user based on search value in the given search list of user objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="searchValue">the search value</param>
        /// <param name="searchList">the search list of user objects</param>
        /// <returns>response.result will contain a List of User objects</returns>
        public Response<List<EntityUser>> Search(Guid accessKey, string searchValue, List<EntityUser> searchList)
        {
            var param = $"SearchValue:{searchValue}";
            var model = new ServiceRequestModel<List<EntityUser>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityNameUser,
                AuthenticateMethod = MethodSearch,
                WorkerFunc = _ =>
                {
                    var worker = new UserBusinessLogic();
                    return worker.Search(searchValue, searchList);
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a user based on user ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="userID">the user ID</param>
        /// <param name="includeObjects">boolean to include objects after decryption</param>
        /// <returns>response.result will contain a User object</returns>
        public Response<EntityUser> SelectUser(Guid accessKey, int userID, bool includeObjects = false)
        {
            var param = $"UserID:{userID} IncludeObjects:{includeObjects}";
            var model = new ServiceRequestModel<EntityUser>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityNameUser,
                AuthenticateMethod = MethodSelectUser,
                WorkerFunc = _ =>
                {
                    var worker = new UserBusinessLogic();
                    return worker.SelectUser(userID, includeObjects);
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves the user object
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the user object</param>
        /// <returns>response.result will contai an integer</returns>
        public Response<int> Save(Guid accessKey, EntityUser x, ECN_Framework_Entities.Communicator.EmailDirect ed = null)
        {
            var jsonFunction = new UtilityJsonFunctions();
            var param = jsonFunction.ToJson(x);
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityNameUser,
                AuthenticateMethod = MethodSave,
                WorkerFunc = request =>
                {
                    var worker = new UserBusinessLogic();
                    var result = worker.Save(x);
                    request.Succeeded = result >= 0;

                    if (request.Succeeded.Value && ed != null)
                    {
                        EmailDirectBusinessLogic.Save(ed);
                    }

                    return result;
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Checks to see if the user is a KM user
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the user object</param>
        /// <returns>response.result will contain a boolean</returns>
        public Response<bool> IsKmUser(Guid accessKey, EntityUser x)
        {
            var jsonFunction = new UtilityJsonFunctions();
            var param = jsonFunction.ToJson(x);
            var model = new ServiceRequestModel<bool>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityNameUser,
                AuthenticateMethod = MethodIsKmUser
            };

            return GetResponse(model);
        }

        public Response<string> GenerateTempPassword()
        {
            Response<string> response = new Response<string>();
            try
            {
                response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.No_Access_Key_Required;
                KMPlatform.BusinessLogic.User worker = new KMPlatform.BusinessLogic.User();
                response.Message = "No Access Key Required";
                response.Result = worker.GenerateTempPassword();
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
                Guid accessKey = Guid.NewGuid();
                response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, this.GetType().Name.ToString());
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
