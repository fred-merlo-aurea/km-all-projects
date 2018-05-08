using System;
using System.Collections.Specialized;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using KMDbManagers;

namespace KMManagers
{
    public class Logger : APIRunnerBase
    {
        private static Logger logger = null;

        private const string APIAccessKey_EKey = "ApiKey_E";
        private const string CustID_EKey = "CustID_E";
        private const string ApplicationID_EKey = "ApplicationID_E";
        private const string LogFilePrefixKey = "LogFilePrefix";
        private const string StatusesKey = "DoNotLogStatuses";
        private const string ErrorListKey = "ErrorListFilePath";
        private readonly string LogFilePrefix = null;
        private const string DateFormat = "_ddMMyyyy";
        private const string TimeFormat = "HH:mm:ss";
        private string[] Statuses = new string[0];
        private string[] Errors = new string[0];
        private string LogFile = null;
        private const string txt = ".txt";

        private Logger()
        {
            LogFilePrefix =
                WebConfigUtils.KMDesignerRoot() +
                WebConfigurationManager.AppSettings[LogFilePrefixKey];
            string data = WebConfigurationManager.AppSettings[StatusesKey];
            if (!string.IsNullOrEmpty(data))
            {
                Statuses = data.Replace(',', ' ').Replace(';', ' ').Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            }
            string path =
                WebConfigUtils.KMDesignerRoot() +
                WebConfigurationManager.AppSettings[ErrorListKey];
            if (File.Exists(path))
            {
                Errors = File.ReadAllLines(path);
            }
        }

        public static void Log(string method, Exception ex)
        {
            if (logger == null)
            {
                lock (typeof(Logger))
                {
                    if (logger == null)
                    {
                        logger = new Logger();
                    }
                }
            }

            int severity = logger.GetSeverity(ex);
            int status = -1;
            try
            {
                status = logger.LogException(method, ex, severity);
            }
            catch
            { }
            if (logger.CheckStatus(status))
            {
                try
                {
                    logger.LogToFile(method, ex, severity);
                }
                catch
                { }
            }
        }

        public static void WriteLog(string message)
        {
            if (logger == null)
            {
                lock (typeof(Logger))
                {
                    if (logger == null)
                    {
                        logger = new Logger();
                    }
                }
            }

            try
            {
                logger.WriteToFile(message);
            }
            catch
            { }
        }

        #region Log errors
        public int LogException(string method, Exception ex, int severity)
        {
            NameValueCollection data = new NameValueCollection();
            data.Add(APIAccessKey, WebConfigurationManager.AppSettings[APIAccessKey_EKey]);
            data.Add(X_Customer_ID, WebConfigurationManager.AppSettings[CustID_EKey]);
            string resp = null;
            int appId = 81;
            try
            {
                appId = int.Parse(WebConfigurationManager.AppSettings[ApplicationID_EKey]);
            }
            catch
            { }

            return SendCommand(GetCURLWithItem("internal/error"), data,
                "{\"ApplicationID\":" + appId + ",\"SeverityID\":" + severity + ",\"SourceMethod\":\"" + method + 
                    "\",\"Exception\":\"" + ex.Message + "\",\"LogNote\":\"\"}", out resp);
        }

        private void LogToFile(string method, Exception ex, int severity)
        {
            lock (typeof(Logger))
            {
                try
                {
                    DateTime now = DateTime.Now;
                    LogFile = LogFilePrefix + now.ToString(DateFormat) + txt;
                    StreamWriter wr = new StreamWriter(LogFile, true);
                    wr.WriteLine(now.ToString(TimeFormat) + "   method is: " + method + " (severity " + severity + ')');
                    wr.WriteLine(now.ToString(TimeFormat) + "   stack: " + ex.StackTrace);
                    wr.WriteLine(now.ToString(TimeFormat) + "   exception is: " + ex.Message);
                    wr.Close();
                }
                catch
                { }
            }
        }

        private void WriteToFile(string message)
        {
            lock (typeof(Logger))
            {
                try
                {
                    DateTime now = DateTime.Now;
                    LogFile = LogFilePrefix + now.ToString(DateFormat) + txt;
                    StreamWriter wr = new StreamWriter(LogFile, true);
                    wr.WriteLine(now.ToString(TimeFormat) + "   " + message);
                    wr.Close();
                }
                catch
                { }
            }
        }
        
        //public void FormatException(string method, string message)
        //{
        //    NameValueCollection data = new NameValueCollection();
        //    data.Add(APIAccessKey, "23eb2cff-e71f-40a8-b288-1ee1bc382789");
        //    data.Add(X_Customer_ID, "1");
        //    string resp = null;
        //    SendCommand(GetCURLWithItem("internal/error/format"), data, "{\"ClassName\":\"System.Exception\",\"ExceptionMethod\":\"" + 
        //        method + "\",\"HResult\":-2146233088,\"HelpURL\":\"http://api.ecn5.com\",\"InnerException\":{\"ClassName\":\"System.Exception\",\"Data\":null,\"ExceptionMethod\":null,\"HResult\":-2146233088,\"HelpURL\":null,\"InnerException\":null,\"Message\":\"inner exception\",\"RemoteStackIndex\":0,\"RemoteStackTraceString\":null,\"Source\":null,\"StackTraceString\":null,\"WatsonBuckets\":null},\"Message\":\""
        //        + message + "\",\"RemoteStackIndex\":0,\"RemoteStackTraceString\":null,\"Source\":\"EmailMarketing.API\",\"StackTraceString\":\"   at EmailMarketing.API.Controllers.Internal.ErrorController.FormatException() in c:\\\\Projects\\\\ECN\\\\Dev\\\\2015_Q2\\\\EmailMarketing.API\\\\Controllers\\\\Internal\\\\ErrorController.cs:line 106\",\"WatsonBuckets\":null}", out resp);
        //}
        #endregion

        #region Private methods
        private bool CheckStatus(int status)
        {
            bool res = true;
            for (int i = 0; i < Statuses.Length; i++)
            {
                if (Statuses[i] == status.ToString())
                {
                    res = false;
                    break;
                }
            }
            
            return res;
        }

        private int GetSeverity(Exception ex)
        {
            int res = 1;
            for (int i = 0; i < Errors.Length; i++)
            {
                string error = Errors[i].Trim();
                if (error != string.Empty)
                {
                    if (error.StartsWith("^") && error.EndsWith("$"))
                    {
                        Regex rex = new Regex(error);
                        if (rex.IsMatch(ex.Message))
                        {
                            res = 2;
                            break;
                        }
                    }
                    else
                    {
                        if (ex.Message.Contains(error))
                        {
                            res = 2;
                            break;
                        }
                    }
                }
            }

            return res;
        }
        #endregion
    }
}