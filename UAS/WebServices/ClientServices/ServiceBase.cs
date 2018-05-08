using System;
using System.IO;
using WebServiceFramework;

namespace ClientServices
{
    public abstract class ServiceBase : FrameworkServiceBase
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

            return log;
        }

        public void SaveLog(KMPlatform.Entity.ApiLog logEntry)
        {
            KMPlatform.BusinessLogic.ApiLog alWorker = new KMPlatform.BusinessLogic.ApiLog();
            logEntry.RequestEndDate = DateTime.Now;
            logEntry.RequestEndTime = DateTime.Now.TimeOfDay;
            logEntry.ApiLogId = alWorker.Save(logEntry);
        }

        protected override FrameworkUAS.Service.Authentication Authenticate(Guid accessKey, bool includeMenu = false, string requestData = "", string entity = "", string method = "")
        {
            KMPlatform.Entity.User user = Authenticate(accessKey, includeMenu);
            KMPlatform.Entity.Client client = null;
            if (user != null)
            {
                KMPlatform.BusinessLogic.Client cWorker = new KMPlatform.BusinessLogic.Client();
                client = cWorker.Select(user.DefaultClientID, false);
            }
            FrameworkUAS.Service.Authentication auth = new FrameworkUAS.Service.Authentication();

            //Log
            if (user != null && client != null)
            {
                auth.LogEntry = CreateLog(client.ClientID, accessKey, requestData, entity, method, "Access_Validated");
                if (method.Equals("SavePaidSubscriber"))
                    SaveLog(auth.LogEntry);
                auth.Client = client;
                auth.User = user;//Authenticate(accessKey, includeMenu);
                auth.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Access_Validated;
                auth.IsAuthenticated = true;
                auth.IsKM = user.IsKMStaff;
            }
            else
            {
                auth.LogEntry = CreateLog(-1, accessKey, requestData, entity, method, "Access_Denied - invalid Access key");
                if (method.Equals("SavePaidSubscriber"))
                    SaveLog(auth.LogEntry);
                auth.Client = null;
                auth.User = null;
                auth.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Access_Denied;
                auth.IsAuthenticated = false;
                auth.IsKM = false;
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

        protected void FileLog(string msg, string processCode)
        {
            FrameworkUAS.BusinessLogic.FileLog fworker = new FrameworkUAS.BusinessLogic.FileLog();
            FrameworkUAS.Entity.FileLog fl = new FrameworkUAS.Entity.FileLog();
            fl.LogDate = DateTime.Now;
            fl.LogTime = DateTime.Now.TimeOfDay;
            fl.Message = msg;
            fl.ProcessCode = processCode;
            fworker.Save(fl);
        }
    }
}