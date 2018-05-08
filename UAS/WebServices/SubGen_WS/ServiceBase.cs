using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace SubGen_WS
{
    public class ServiceBase
    {
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

            KMPlatform.BusinessLogic.ApiLog alWorker = new KMPlatform.BusinessLogic.ApiLog();
            alWorker.Save(log);

            return log;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetCallingIp()
        {
            return OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] != null
                   ? ((RemoteEndpointMessageProperty)OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name]).Address
                   : string.Empty;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="includeMenu"></param>
        /// <param name="requestData"></param>
        /// <param name="entity"></param>
        /// <param name="method"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        protected void WriteToLog(Exception ex)
        {
            string logPath = ConfigurationManager.AppSettings["Log"].ToString();
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);
            logWriter = new StreamWriter(logPath + "UASWS_" + DateTime.Now.ToString("MMddyyyy") + ".txt", true);
            string error = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            WriteToFile(error, logWriter);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        protected void WriteToLog(string msg)
        {
            string logPath = ConfigurationManager.AppSettings["Log"].ToString();
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);
            logWriter = new StreamWriter(logPath + "UASWS_" + DateTime.Now.ToString("MMddyyyy") + ".txt", true);
            WriteToFile(msg, logWriter);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="WriteFile"></param>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="ex"></param>
        /// <param name="entity"></param>
        /// <param name="method"></param>
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
    }
}
