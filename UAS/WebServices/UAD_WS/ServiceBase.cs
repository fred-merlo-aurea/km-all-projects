using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using FrameworkUAS.Service;

namespace UAD_WS
{
    public class ServiceBase
    {
        private const string MessageAccessKeyValidated = "AccessKey Validated";
        private const string MessageSuccess = "Success";
        private const string MessageError = "Error";
        private const string ExceptionParameterNullOrWhiteSpace = "{0} cannot be null or white space.";

        StreamWriter logWriter;

        private KMPlatform.Entity.ApiLog CreateLog(int clientID, Guid accessKey, string requestData, string entity, string method, string msg)
        {
            KMPlatform.Entity.ApiLog log = new KMPlatform.Entity.ApiLog();

            log.ClientID = clientID;
            log.AccessKey = accessKey;
            log.RequestStartDate = DateTime.Now;
            log.RequestStartTime = DateTime.Now.TimeOfDay;
            log.RequestFromIP = GetCallingIp();
            log.Entity = entity;
            log.Method = method;
            log.RequestData = requestData;
            log.ErrorMessage = msg;

            return log;
        }

        public static string GetCallingIp()
        {
            return OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] != null
                   ? ((RemoteEndpointMessageProperty)OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name]).Address
                   : string.Empty;
        }

        protected FrameworkUAS.Service.Authentication Authenticate(Guid accessKey, bool includeMenu = false, string requestData = "", string entity = "", string method = "")
        {
            if (AuthenticationCache.AuthObjects == null)
                AuthenticationCache.AuthObjects = new System.Collections.Generic.Dictionary<FrameworkUAS.Service.Authentication, DateTime>();

            if (AuthenticationCache.AuthObjects.Any(x => x.Key.User.AccessKey == accessKey && x.Value < DateTime.Now.AddHours(-24)) == true)
            {
                FrameworkUAS.Service.Authentication remove = AuthenticationCache.AuthObjects.FirstOrDefault(x => x.Key.User.AccessKey == accessKey).Key;
                AuthenticationCache.AuthObjects.Remove(remove);
            }

            FrameworkUAS.Service.Authentication auth = new FrameworkUAS.Service.Authentication();
            if (AuthenticationCache.AuthObjects.Any(x => x.Key.User.AccessKey == accessKey && x.Value >= DateTime.Now.AddHours(-24)) == true)
            {
                auth = AuthenticationCache.AuthObjects.FirstOrDefault(x => x.Key.User.AccessKey == accessKey).Key;
            }
            else
            {
                KMPlatform.BusinessLogic.Client cWorker = new KMPlatform.BusinessLogic.Client();
                KMPlatform.Entity.Client client = cWorker.SelectDefault(accessKey);
                
                //Log
                if (client != null)
                {
                    //CreateLog(client.ClientID, accessKey, requestData, entity, method, "Access_Validated");
                    auth.Client = client;
                    auth.User = Authenticate(accessKey, includeMenu);
                    auth.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Access_Validated;
                    auth.IsAuthenticated = true;
                    if (client.ClientCode.Equals("KM") == true)
                        auth.IsKM = true;
                    else
                        auth.IsKM = false;

                    AuthenticationCache.AuthObjects.Add(auth, DateTime.Now);
                }
                else
                {
                    //CreateLog(-1, accessKey, requestData, entity, method, "Access_Denied - invalid Access key");
                    auth.Client = null;
                    auth.User = null;
                    auth.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Access_Denied;
                    auth.IsAuthenticated = false;
                    auth.IsKM = false;
                }
            }
            return auth;
        }
        private KMPlatform.Entity.User Authenticate(Guid accessKey, bool includeMenu = false)
        {
            KMPlatform.Entity.User user = null;
            KMPlatform.BusinessLogic.User uworker = new KMPlatform.BusinessLogic.User();
            user = uworker.LogIn(accessKey, includeMenu);

            return user;
        }

        protected void WriteToLog(Exception ex)
        {
            string logPath = ConfigurationManager.AppSettings["Log"].ToString();
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);
            logWriter = new StreamWriter(logPath + "UASWS_" + DateTime.Now.ToString("MMddyyyy") + ".txt", true);
            string error = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            WriteToFile(error, logWriter);
        }
        protected void WriteToLog(string msg)
        {
            string logPath = ConfigurationManager.AppSettings["Log"].ToString();
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);
            logWriter = new StreamWriter(logPath + "UASWS_" + DateTime.Now.ToString("MMddyyyy") + ".txt", true);
            WriteToFile(msg, logWriter);
        }
        void WriteToFile(string text, StreamWriter WriteFile)
        {
            try
            {
                WriteFile.AutoFlush = true;
                WriteFile.WriteLine(text);
                WriteFile.Flush();
                WriteFile.Close();
                System.GC.Collect();
            }
            catch { }
        }
        protected void LogError(Guid accessKey, Exception ex, string entity, [CallerMemberName] string method = "")
        {
            KMPlatform.BusinessLogic.Client cWorker = new KMPlatform.BusinessLogic.Client();
            KMPlatform.Entity.Client client = cWorker.SelectDefault(accessKey);

            KMPlatform.Entity.ApiLog log = new KMPlatform.Entity.ApiLog();
            int clientID = 0;
            if (client != null) clientID = client.ClientID;

            log.ClientID = clientID;
            log.AccessKey = accessKey;
            log.RequestFromIP = GetCallingIp();
            log.Entity = entity;
            log.Method = method;
            log.ErrorMessage = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            log.RequestStartDate = DateTime.Now;
            log.RequestStartTime = DateTime.Now.TimeOfDay;
            log.RequestEndDate = DateTime.Now;
            log.RequestEndTime = DateTime.Now.TimeOfDay;

            KMPlatform.BusinessLogic.ApiLog alWorker = new KMPlatform.BusinessLogic.ApiLog();
            alWorker.Save(log);
        }

        protected Response<TResult> GetResponse<TResult>(RequestModel<TResult> request)
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

                if (auth.IsAuthenticated)
                {
                    response.Message = MessageAccessKeyValidated;
                    response.Result = request.WorkerFunc(request);

                    if (!request.Succeeded.HasValue)
                    {
                        request.Succeeded = response.Result != null;
                    }

                    if (request.Succeeded.Value)
                    {
                        response.Message = MessageSuccess;
                        response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success;
                    }
                    else
                    {
                        response.Message = MessageError;
                        response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(request.AccessKey, ex, GetType().Name);
                response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }

            return response;
        }

        private void ValidateRequestModel<TResult>(RequestModel<TResult> request)
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

            if (request.WorkerFunc == null)
            {
                throw new ArgumentException(string.Format(ExceptionParameterNullOrWhiteSpace, "WorkerFunc"));
            }
        }
    }
}
