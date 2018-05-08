using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using KM.Common.Entity;

namespace ecn.webservice
{
    public class Global : System.Web.HttpApplication
    {
        private const string KMCommonAppKey = "KMCommon_Application";
        private const string GlobalApplicationError = "Global.Application_Error";
        private const string GlobalAsax = "Global.asax";
        private const string Err = "err";
        private const string ErrorAspx = "error.aspx";
        private const string DoesNotExists = "does not exist";
        private const string DangerousPath = "A potentially dangerous Request.Path";

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var err = Server.GetLastError();
            int kmCommonAppKey;
            int.TryParse(ConfigurationManager.AppSettings[KMCommonAppKey], out kmCommonAppKey);
            HttpException lastErrorWrapper = Server.GetLastError() as HttpException;

            if (null != lastErrorWrapper && (lastErrorWrapper.GetBaseException().GetType() == typeof(HttpRequestValidationException) || lastErrorWrapper.GetHttpCode() == 400))
            {
                ApplicationLog.LogNonCriticalError(lastErrorWrapper.ToString(), GlobalApplicationError, kmCommonAppKey);
            }
            else if (err.InnerException is ArgumentException)
            {
                ApplicationLog.LogNonCriticalError(err, GlobalApplicationError, kmCommonAppKey);
            }
            else if (err.InnerException is ViewStateException)
            {
                ApplicationLog.LogNonCriticalError(err, GlobalApplicationError, kmCommonAppKey);
            }
            else if (err.InnerException is SqlException)
            {
                ApplicationLog.LogNonCriticalError(err, GlobalApplicationError, kmCommonAppKey);
            }
            else if (err is TransactionException)
            {
                ApplicationLog.LogNonCriticalError(err, GlobalApplicationError, kmCommonAppKey);
            }
            else if (err.InnerException is HttpException)
            {
                try
                {
                    HttpException httpError = err.InnerException as HttpException;
                    if (httpError != null)
                    {
                        if (httpError.GetBaseException().GetType() == typeof(ViewStateException))
                        {
                            ApplicationLog.LogNonCriticalError(httpError, GlobalAsax, kmCommonAppKey);
                        }
                        else if (httpError.GetBaseException().GetType() == typeof(ArgumentException))
                        {
                            ApplicationLog.LogNonCriticalError(httpError, GlobalAsax, kmCommonAppKey);
                        }
                        else
                        {
                            ApplicationLog.LogCriticalError(httpError, GlobalApplicationError, kmCommonAppKey);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error occurred : {ex}");
                }
            }
            else if (err.InnerException is Win32Exception)
            {
                ApplicationLog.LogNonCriticalError(err, GlobalApplicationError, kmCommonAppKey);
            }
            else
            {
                if (err.Message.Contains(DangerousPath))
                {
                    ApplicationLog.LogNonCriticalError(err, GlobalApplicationError, kmCommonAppKey);
                }
                else if (err.Message.Contains(DoesNotExists))
                {
                    Application[Err] = err;
                }
                else
                {
                    ApplicationLog.LogCriticalError(err, GlobalApplicationError, kmCommonAppKey);
                }
            }

            Application[Err] = err;
            Server.Transfer(ErrorAspx);
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}