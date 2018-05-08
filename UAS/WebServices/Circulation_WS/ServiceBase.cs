using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Circulation_WS
{
    public class ServiceBase
    {
        StreamWriter logWriter;

        private FrameworkUAS.Entity.ApiLog CreateLog(int clientID, Guid accessKey, string requestData, string entity, string method)
        {
            FrameworkUAS.Entity.ApiLog log = new FrameworkUAS.Entity.ApiLog();

            log.ClientID = clientID;
            log.AccessKey = accessKey;
            log.RequestStartDate = DateTime.Now;
            log.RequestStartTime = DateTime.Now.TimeOfDay;
            log.RequestFromIP = GetCallingIp();
            log.Entity = entity;
            log.Method = method;
            log.RequestData = requestData;

            return log;
        }

        private static string GetCallingIp()
        {
            return OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] != null
                   ? ((RemoteEndpointMessageProperty)OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name]).Address
                   : string.Empty;
        }

        protected FrameworkUAS.Service.Authentication Authenticate(Guid accessKey, bool includeMenu = false, string requestData = "", string entity = "", string method = "")
        {
            FrameworkUAS.BusinessLogic.Client cWorker = new FrameworkUAS.BusinessLogic.Client();
            FrameworkUAS.Entity.Client client = cWorker.SelectDefault(accessKey);
            FrameworkUAS.Service.Authentication auth = new FrameworkUAS.Service.Authentication();
            //Log
            if (client != null)
            {
                CreateLog(client.ClientID, accessKey, requestData, entity, method);
                auth.Client = client;
                auth.User = Authenticate(accessKey, includeMenu);
                auth.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Access_Validated;
                auth.IsAuthenticated = true;
                if (client.ClientCode.Equals("KM") == true)
                    auth.IsKM = true;
                else
                    auth.IsKM = false;
            }
            else
            {
                CreateLog(-1, accessKey, requestData, entity, method);
                auth.Client = null;
                auth.User = null;
                auth.Status = FrameworkUAS.BusinessLogic.Enums.ServiceResponseStatusTypes.Access_Denied;
                auth.IsAuthenticated = false;
                auth.IsKM = false;
            }

            return auth;
        }
        private FrameworkUAS.Entity.User Authenticate(Guid accessKey, bool includeMenu = false)
        {
            FrameworkUAS.Entity.User user = null;
            FrameworkUAS.BusinessLogic.User uworker = new FrameworkUAS.BusinessLogic.User();
            user = uworker.LogIn(accessKey, includeMenu);

            return user;
        }

        protected void WriteToLog(Exception ex)
        {
            string logPath = ConfigurationManager.AppSettings["Log"].ToString();
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);
            logWriter = new StreamWriter(logPath + "CircWS_" + DateTime.Now.ToString("MMddyyyy") + ".txt", true);
            string error = Core.Utilities.StringFunctions.FormatException(ex);
            WriteToFile(error, logWriter);
        }
        protected void WriteToLog(string msg)
        {
            string logPath = ConfigurationManager.AppSettings["Log"].ToString();
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);
            logWriter = new StreamWriter(logPath + "CircWS_" + DateTime.Now.ToString("MMddyyyy") + ".txt", true);
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
            FrameworkUAS.BusinessLogic.Client cWorker = new FrameworkUAS.BusinessLogic.Client();
            FrameworkUAS.Entity.Client client = cWorker.SelectDefault(accessKey);

            FrameworkUAS.Entity.ApiLog log = new FrameworkUAS.Entity.ApiLog();
            int clientID = 0;
            if (client != null) clientID = client.ClientID;

            log.ClientID = clientID;
            log.AccessKey = accessKey;
            log.RequestFromIP = GetCallingIp();
            log.Entity = entity;
            log.Method = method;
            log.ErrorMessage = Core.Utilities.StringFunctions.FormatException(ex);
            log.RequestStartDate = DateTime.Now;
            log.RequestStartTime = DateTime.Now.TimeOfDay;
            log.RequestEndDate = DateTime.Now;
            log.RequestEndTime = DateTime.Now.TimeOfDay;

            FrameworkUAS.BusinessLogic.ApiLog alWorker = new FrameworkUAS.BusinessLogic.ApiLog();
            alWorker.Save(log);
        }
    }
}
