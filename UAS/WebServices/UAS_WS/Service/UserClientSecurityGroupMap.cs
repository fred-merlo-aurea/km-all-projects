using System;
using System.Collections.Generic;
using FrameworkUAS.Service;
using UAS_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = KMPlatform.BusinessLogic.UserClientSecurityGroupMap;
using EntityUserClientSecurityGroupMap = KMPlatform.Entity.UserClientSecurityGroupMap;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class UserClientSecurityGroupMap : FrameworkServiceBase, IUserClientSecurityGroupMap
    {
        private const string EntityName = "UserClientSecurityGroupMap";
        private const string MethodSelectForUser = "SelectForUser";
        private const string MethodSelectForClient = "SelectForClient";

        /// <summary>
        /// Selects a list of UserClientSecurityGroupMap objects
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of UserClientSecurityGroupMap objects</returns>
        public Response<List<KMPlatform.Entity.UserClientSecurityGroupMap>> Select(Guid accessKey)
        {
            Response<List<KMPlatform.Entity.UserClientSecurityGroupMap>> response = new Response<List<KMPlatform.Entity.UserClientSecurityGroupMap>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "", "UserClientSecurityGroupMap", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.UserClientSecurityGroupMap worker = new KMPlatform.BusinessLogic.UserClientSecurityGroupMap();
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
                LogError(accessKey, ex, "UserClientSecurityGroupMap", "Select");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a list of UserClientSecurityGroupMap objects based on user ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="userId">the user ID</param>
        /// <returns>response.result will contain a list of UserClientSecurityGroupMap objects</returns>
        public Response<List<EntityUserClientSecurityGroupMap>> SelectForUser(Guid accessKey, int userId)
        {
            var param = $"UserID: {userId}";
            var model = new ServiceRequestModel<List<EntityUserClientSecurityGroupMap>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForUser,
                WorkerFunc = request => new BusinessLogicWorker().SelectForUser(userId)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of UserClientSecurityGroupMap objects based on user authorization
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="userID">the user ID</param>
        /// <returns>response.result will contain a list of UserClientSecurityGroupMap objects</returns>
        public Response<List<KMPlatform.Entity.UserClientSecurityGroupMap>> SelectForUserAuthorization(Guid accessKey, int userID)
        {
            Response<List<KMPlatform.Entity.UserClientSecurityGroupMap>> response = new Response<List<KMPlatform.Entity.UserClientSecurityGroupMap>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "UserID: " + userID.ToString(), "UserClientSecurityGroupMap", "SelectForUserAuthorization");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.UserClientSecurityGroupMap worker = new KMPlatform.BusinessLogic.UserClientSecurityGroupMap();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectForUserAuthorization(userID);
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
                LogError(accessKey, ex, "UserClientSecurityGroupMap", "SelectForUserAuthorization");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a list of UserClientSecurityGroupMap objects based on the client ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="clientId">the client ID</param>
        /// <returns>response.result will contain a list of UserClientSecurityGroupMap objects</returns>
        public Response<List<EntityUserClientSecurityGroupMap>> SelectForClient(Guid accessKey, int clientId)
        {
            var param = $"ClientID: {clientId}";
            var model = new ServiceRequestModel<List<EntityUserClientSecurityGroupMap>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSelectForClient,
                WorkerFunc = request => new BusinessLogicWorker().SelectForClient(clientId)
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Selects a list of UserClientSecurityGroupMap objects based on the security group ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="securityGroupID">the security group ID</param>
        /// <returns>response.result will contain a list of UserClientSecurityGroupMap objects</returns>
        public Response<List<KMPlatform.Entity.UserClientSecurityGroupMap>> SelectForSecurityGroup(Guid accessKey, int securityGroupID)
        {
            Response<List<KMPlatform.Entity.UserClientSecurityGroupMap>> response = new Response<List<KMPlatform.Entity.UserClientSecurityGroupMap>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "SecurityGroupID: " + securityGroupID.ToString(), "UserClientSecurityGroupMap", "SelectForSecurityGroup");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.UserClientSecurityGroupMap worker = new KMPlatform.BusinessLogic.UserClientSecurityGroupMap();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectForSecurityGroup(securityGroupID);
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
                LogError(accessKey, ex, "UserClientSecurityGroupMap", "SelectForSecurityGroup");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Saves the UserClientSecurityGroupMap
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="x">the UserClientSecurityGroupMap object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, KMPlatform.Entity.UserClientSecurityGroupMap x)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<KMPlatform.Entity.UserClientSecurityGroupMap>(x);
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "UserClientSecurityGroupMap", "Save");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true && auth.IsKM == true)
                {
                    KMPlatform.BusinessLogic.UserClientSecurityGroupMap worker = new KMPlatform.BusinessLogic.UserClientSecurityGroupMap();
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
                LogError(accessKey, ex, "UserClientSecurityGroupMap", "Save");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
    }
}
