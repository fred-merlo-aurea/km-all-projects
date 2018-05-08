using System;
using System.Collections.Generic;
using Core_AMS.Utilities;
using FrameworkUAS.Service;
using UAS_WS.Interface;
using WebServiceFramework;
using BusinessLogicWorker = KMPlatform.BusinessLogic.UserLog;
using EntityUserLog = KMPlatform.Entity.UserLog;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class UserLog : FrameworkServiceBase, IUserLog
    {
        private const string EntityName = "UserLog";
        private const string MethodSave = "Save";

        /// <summary>
        /// Creates a user log
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="applicationID">the application ID</param>
        /// <param name="userLogType">the user log type (Add, Change_Active_Flag, Delete, Edit, Log_In, Log_Out)</param>
        /// <param name="userID">the user ID</param>
        /// <param name="objectName">the object name</param>
        /// <param name="originalObjectJson">the original object Json</param>
        /// <param name="newObjectJson">the new object Json</param>
        /// <returns>response.result will contain a UserLog object</returns>
        public Response<KMPlatform.Entity.UserLog> CreateLog(Guid accessKey, int applicationID, KMPlatform.BusinessLogic.Enums.UserLogTypes userLogType,
                                                          int userID, string objectName, string originalObjectJson, string newObjectJson)
        {
            Response<KMPlatform.Entity.UserLog> response = new Response<KMPlatform.Entity.UserLog>();
            try
            {
                string param = "ApplicationID:" + applicationID.ToString() + " UserLogType:" + userLogType.ToString() + " UserID:" + userID.ToString() + " ObjectName:" + objectName + " OriginalObjectJson:" + originalObjectJson + " NewObjectJson:" + newObjectJson;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "UserLog", "CreateLog");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.UserLog worker = new KMPlatform.BusinessLogic.UserLog();
                    response.Message = "AccessKey Validated";
                    FrameworkUAD_Lookup.BusinessLogic.Code cWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
                    int userLogTypeID = cWorker.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.User_Log, userLogType.ToString()).CodeId;
                    response.Result = worker.CreateLog(applicationID, userLogType, userID, objectName, originalObjectJson, newObjectJson, userLogTypeID);
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
                LogError(accessKey, ex, "UserLog", "CreateLog");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a user log
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <returns>response.result will contain a list of UserLog objects</returns>
        public Response<List<KMPlatform.Entity.UserLog>> Select(Guid accessKey)
        {
            Response<List<KMPlatform.Entity.UserLog>> response = new Response<List<KMPlatform.Entity.UserLog>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "UserLog", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.UserLog worker = new KMPlatform.BusinessLogic.UserLog();
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
                LogError(accessKey, ex, "UserLog", "Select");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Selects a user log based on the user's log ID
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="userLogID">the user log ID</param>
        /// <returns>response.result will contain a UserLog object</returns>
        public Response<KMPlatform.Entity.UserLog> Select(Guid accessKey, int userLogID)
        {
            Response<KMPlatform.Entity.UserLog> response = new Response<KMPlatform.Entity.UserLog>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "UserLogID:" + userLogID.ToString(), "UserLog", "Select");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.UserLog worker = new KMPlatform.BusinessLogic.UserLog();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(userLogID);
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
                LogError(accessKey, ex, "UserLog", "Select");
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// Saves the user log
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="entity">the <see cref="EntityUserLog"/> object</param>
        /// <returns>response.result will contain an integer</returns>
        public Response<int> Save(Guid accessKey, EntityUserLog entity)
        {
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = new JsonFunctions().ToJson(entity),
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().Save(entity);
                    request.Succeeded = result >= 0;
                    return result;
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// Saves the inserted user logs in a bulk of 500 at a time
        /// </summary>
        /// <param name="accessKey">the access key</param>
        /// <param name="list">the user log list</param>
        /// <param name="client">the client</param>
        /// <returns>response.result will contain a list of UserLog objects</returns>
        public Response<List<KMPlatform.Entity.UserLog>> SaveBulkInsert(Guid accessKey, List<KMPlatform.Entity.UserLog> list, KMPlatform.Entity.Client client)
        {
            Response<List<KMPlatform.Entity.UserLog>> response = new Response<List<KMPlatform.Entity.UserLog>>();
            try
            {
                string param = "Client: " + client.ClientName;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "UserLog", "SaveBulkInsert");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.UserLog worker = new KMPlatform.BusinessLogic.UserLog();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SaveBulkInsert(list, client);
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
