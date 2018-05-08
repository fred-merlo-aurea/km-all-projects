using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using FrameworkUAS.Service;
using EntityUser = KMPlatform.Entity.User;
using BusinessLogicUser = KMPlatform.BusinessLogic.User;
using BusinessLogicClient = KMPlatform.BusinessLogic.Client;
using static FrameworkUAD_Lookup.Enums;

namespace WebServiceFramework
{
    /// <summary>
    /// Represents the base class for web services.
    /// </summary>
    public abstract class FrameworkServiceBase
    {
        private const string MessageSuccess = "Success";
        private const string MessageError = "Error";
        private const string ExceptionParameterNullOrWhiteSpace = "{0} cannot be null or white space.";
        private const string SettingLog = "Log";
        private const string ClientCodeKm = "KM";
        private const int HoursInDay = 24;

        private StreamWriter _logWriter;

        public static string GetCallingIp()
        {
            var endpoint = OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name];
            return endpoint != null
                   ? ((RemoteEndpointMessageProperty)endpoint).Address
                   : string.Empty;
        }

        protected virtual Authentication Authenticate(Guid accessKey, bool includeMenu = false, string requestData = "", string entity = "", string method = "")
        {
            if (AuthenticationCache.AuthObjects == null)
            {
                AuthenticationCache.AuthObjects = new Dictionary<Authentication, DateTime>();
            }

            if (AuthenticationCache.AuthObjects.Any(x => x.Key.User.AccessKey == accessKey && x.Value < DateTime.Now.AddHours(-HoursInDay)))
            {
                var remove = AuthenticationCache.AuthObjects.FirstOrDefault(x => x.Key.User.AccessKey == accessKey).Key;
                AuthenticationCache.AuthObjects.Remove(remove);
            }

            var auth = new Authentication();
            if (AuthenticationCache.AuthObjects.Any(x => x.Key.User.AccessKey == accessKey && x.Value >= DateTime.Now.AddHours(-HoursInDay)))
            {
                auth = AuthenticationCache.AuthObjects.FirstOrDefault(x => x.Key.User.AccessKey == accessKey).Key;
            }
            else
            {
                var clientWorker = new BusinessLogicClient();
                var client = clientWorker.SelectDefault(accessKey);

                if (client != null)
                {
                    auth.Client = client;
                    auth.User = Authenticate(accessKey, includeMenu);
                    auth.Status = ServiceResponseStatusTypes.Access_Validated;
                    auth.IsAuthenticated = true;
                    auth.IsKM = client.ClientCode.Equals(ClientCodeKm);

                    AuthenticationCache.AuthObjects.Add(auth, DateTime.Now);
                }
                else
                {
                    auth.Client = null;
                    auth.User = null;
                    auth.Status = ServiceResponseStatusTypes.Access_Denied;
                    auth.IsAuthenticated = false;
                    auth.IsKM = false;
                }
            }

            return auth;
        }

        protected void WriteToLog(Exception ex)
        {
            var logPath = ConfigurationManager.AppSettings[SettingLog];

            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            _logWriter = new StreamWriter($"{logPath}UASWS_{DateTime.Now:MMddyyyy}.txt", true);
            var error = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            WriteToFile(error, _logWriter);
        }

        protected void WriteToLog(string msg)
        {
            var logPath = ConfigurationManager.AppSettings[SettingLog];
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            _logWriter = new StreamWriter($"{logPath}UASWS_{DateTime.Now:MMddyyyy}.txt", true);
            WriteToFile(msg, _logWriter);
        }

        protected void LogError(Guid accessKey, Exception ex, string entity, [CallerMemberName] string method = "")
        {
            var clientWorker = new BusinessLogicClient();
            var client = clientWorker.SelectDefault(accessKey);

            var log = new KMPlatform.Entity.ApiLog();
            var clientId = 0;
            if (client != null)
            {
                clientId = client.ClientID;
            }

            log.ClientID = clientId;
            log.AccessKey = accessKey;
            log.RequestFromIP = GetCallingIp();
            log.Entity = entity;
            log.Method = method;
            log.ErrorMessage = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            log.RequestStartDate = DateTime.Now;
            log.RequestStartTime = DateTime.Now.TimeOfDay;
            log.RequestEndDate = DateTime.Now;
            log.RequestEndTime = DateTime.Now.TimeOfDay;

            var alWorker = new KMPlatform.BusinessLogic.ApiLog();
            alWorker.Save(log);
        }

        protected Response<TResult> GetResponse<TResult>(ServiceRequestModel<TResult> request)
        {
            var response = new Response<TResult>();

            try
            {
                ValidateRequestModel(request);

                var auth = Authenticate(
                    request.AccessKey,
                    false,
                    request.AuthenticateRequestData,
                    request.AuthenticateEntity,
                    request.AuthenticateMethod);
                response.Status = auth.Status;
                request.ClientConnection = auth.Client?.ClientConnections;

                if (request.AuthenticateFunc?.Invoke(auth) ?? auth.IsAuthenticated)
                {
                    if (request.WorkerFunc != null)
                    {
                        response.Result = request.WorkerFunc(request);
                    }

                    if (!request.Succeeded.HasValue)
                    {
                        request.Succeeded = response.Result != null;
                    }

                    if (request.Succeeded.Value)
                    {
                        response.Message = MessageSuccess;
                        response.Status = ServiceResponseStatusTypes.Success;
                    }
                    else
                    {
                        response.Message = MessageError;
                        response.Status = ServiceResponseStatusTypes.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(request.AccessKey, ex, GetType().Name);
                response.Status = ServiceResponseStatusTypes.Error;
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }

            return response;
        }

        private void ValidateRequestModel<TResult>(ServiceRequestModel<TResult> request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.AuthenticateEntity))
            {
                throw new ArgumentException(string.Format(ExceptionParameterNullOrWhiteSpace, "AuthenticateEntity"));
            }

            if (string.IsNullOrWhiteSpace(request.AuthenticateMethod))
            {
                throw new ArgumentException(string.Format(ExceptionParameterNullOrWhiteSpace, "AuthenticateMethod"));
            }
        }

        private static EntityUser Authenticate(Guid accessKey, bool includeMenu = false)
        {
            var worker = new BusinessLogicUser();
            var user = worker.LogIn(accessKey, includeMenu);

            return user;
        }

        private static void WriteToFile(string text, StreamWriter fileWriter)
        {
            try
            {
                fileWriter.AutoFlush = true;
                fileWriter.WriteLine(text);
                fileWriter.Flush();
                fileWriter.Close();
                GC.Collect();
            }
            catch
            {
                // This method is from legacy code. It does nothing here.
            }
        }
    }
}
