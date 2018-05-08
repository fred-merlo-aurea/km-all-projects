using System;
using System.Collections.Generic;
using FrameworkUAS.Service;
using UAS_WS.Interface;
using WebServiceFramework;
using ApplicationEnum = KMPlatform.BusinessLogic.Enums.Applications;
using BusinessLogicWorker = KMPlatform.BusinessLogic.ApplicationLog;
using EntityApplicationLog = KMPlatform.Entity.ApplicationLog;
using SeverityTypeEnum = KMPlatform.BusinessLogic.Enums.SeverityTypes;
using UtilityJsonFunctions = Core_AMS.Utilities.JsonFunctions;

namespace UAS_WS.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationLog : FrameworkServiceBase, IApplicationLog
    {
        private const string EntityName = "ApplicationLog";
        private const string MethodSave = "Save";
        private const string MethodLogCriticalError = "LogCriticalError";
        private const string MethodLogNonCriticalError = "LogNonCriticalError";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="applicationID"></param>
        /// <returns></returns>
        public Response<List<KMPlatform.Entity.ApplicationLog>> SelectApplication(Guid accessKey, int applicationID)
        {
            Response<List<KMPlatform.Entity.ApplicationLog>> response = new Response<List<KMPlatform.Entity.ApplicationLog>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "ApplicationId: " + applicationID.ToString(), "ApplicationLog", "SelectApplication");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.ApplicationLog worker = new KMPlatform.BusinessLogic.ApplicationLog();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectApplication(applicationID);
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
        /// <summary>
        /// /
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="applicationId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public Response<List<KMPlatform.Entity.ApplicationLog>> SelectApplicationWithDateRange(Guid accessKey, int applicationId, DateTime startDate, DateTime endDate)
        {
            Response<List<KMPlatform.Entity.ApplicationLog>> response = new Response<List<KMPlatform.Entity.ApplicationLog>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "ApplicationId: " + applicationId.ToString() + " StartDate: " + startDate.ToShortDateString() + " EndDate: " + endDate.ToShortDateString(), "ApplicationLog", "SelectApplicationWithDateRange");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.ApplicationLog worker = new KMPlatform.BusinessLogic.ApplicationLog();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectApplicationWithDateRange(applicationId,startDate,endDate);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public Response<List<KMPlatform.Entity.ApplicationLog>> SelectWithDateRange(Guid accessKey, DateTime startDate, DateTime endDate)
        {
            Response<List<KMPlatform.Entity.ApplicationLog>> response = new Response<List<KMPlatform.Entity.ApplicationLog>>();
            try
            {
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, "StartDate: " + startDate.ToShortDateString() + " EndDate: " + endDate.ToShortDateString(), "ApplicationLog", "SelectWithDateRange");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.ApplicationLog worker = new KMPlatform.BusinessLogic.ApplicationLog();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.SelectWithDateRange(startDate, endDate);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="entity"> The <see cref="EntityApplicationLog"/> object.</param>
        /// <param name="app"></param>
        /// <param name="severity"></param>
        /// <returns></returns>
        public Response<int> Save(Guid accessKey, EntityApplicationLog entity, ApplicationEnum app, SeverityTypeEnum severity)
        {
            var param = new UtilityJsonFunctions().ToJson(entity);
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodSave,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().Save(entity, app, severity);
                    request.Succeeded = result >= 0;
                    return result;
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="applicationLogId"></param>
        /// <returns></returns>
        public Response<bool> UpdateNotified(Guid accessKey, int applicationLogId)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "ApplicationLog", "UpdateNotified");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.ApplicationLog worker = new KMPlatform.BusinessLogic.ApplicationLog();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.UpdateNotified(applicationLogId);
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
            catch (Exception except)
            {
                response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, except, this.GetType().Name.ToString());
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="ex"></param>
        /// <param name="sourceMethod"></param>
        /// <param name="application"></param>
        /// <param name="severity"></param>
        /// <param name="note"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public Response<int> LogError(Guid accessKey, Exception ex, string sourceMethod, KMPlatform.BusinessLogic.Enums.Applications application, KMPlatform.BusinessLogic.Enums.SeverityTypes severity, string note = "", int clientId = -1)
        {
            Response<int> response = new Response<int>();
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, string.Empty, "ApplicationLog", "LogError");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    KMPlatform.BusinessLogic.ApplicationLog worker = new KMPlatform.BusinessLogic.ApplicationLog();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.LogError(ex,sourceMethod,application,severity,note,clientId);
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
            catch (Exception except)
            {
                response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, except, this.GetType().Name.ToString());
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <param name="sourceMethod"></param>
        /// <param name="application"></param>
        /// <param name="note"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public Response<int> LogCriticalError(
            Guid accessKey,
            string exceptionMessage,
            string sourceMethod,
            ApplicationEnum application,
            string note = "",
            int clientId = -1)
        {
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodLogCriticalError,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().LogCriticalError(exceptionMessage,sourceMethod,application,note,clientId);
                    request.Succeeded = result >= 0;
                    return result;
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="exception"></param>
        /// <param name="sourceMethod"></param>
        /// <param name="application"></param>
        /// <param name="note"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public Response<int> LogNonCriticalError(
            Guid accessKey,
            Exception exception,
            string sourceMethod,
            ApplicationEnum application,
            string note = "",
            int clientId = -1)
        {
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodLogNonCriticalError,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().LogNonCriticalError(exception,sourceMethod,application,note,clientId);
                    request.Succeeded = result >= 0;
                    return result;
                }
            };

            return GetResponse(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="note"></param>
        /// <param name="sourceMethod"></param>
        /// <param name="application"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public Response<int> LogNonCriticalErrorNote(
            Guid accessKey,
            string note,
            string sourceMethod,
            ApplicationEnum application,
            int clientId = -1)
        {
            var model = new ServiceRequestModel<int>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = EntityName,
                AuthenticateMethod = MethodLogNonCriticalError,
                WorkerFunc = request =>
                {
                    var result = new BusinessLogicWorker().LogNonCriticalErrorNote(note, sourceMethod, application, clientId);
                    request.Succeeded = result >= 0;
                    return result;
                }
            };

            return GetResponse(model);
        }
    }
}
