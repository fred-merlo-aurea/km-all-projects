using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Text;
using System.Configuration;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_Common.Objects;

namespace ecn.collector
{
    public class Global : System.Web.HttpApplication
    {
        private const string SourceMethod = "Global.Application_Error";

        private const string VirtualPathAppSettingsKey = "Collector_VirtualPath";
        private const string AccountsVirtualPathAppSettingsKey = "Accounts_VirtualPath";
        private const string CommunicatorVirtualPathAppSettingsKey = "Communicator_VirtualPath";

        private const string ApplicationIdAppSettingsKey = "KMCommon_Application";
        private const string ApplicationStateExceptionKey = "err";
        private const string RedirectDestination = "/error.aspx?E=";


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
            var exception = Server.GetLastError();
            var applicationId = Convert.ToInt32(ConfigurationManager.AppSettings[ApplicationIdAppSettingsKey]);
            var globalExceptionLogging = new GlobalExceptionLogging(SourceMethod, applicationId);

            if (exception.InnerException is SecurityException)
            {
                Response.Redirect(ConfigurationManager.AppSettings[AccountsVirtualPathAppSettingsKey] + "/main/securityAccessError.aspx", true);
            }
            else if (exception is ECNException)
            {
                globalExceptionLogging.LogNonCriticalError(Request, exception);
                Application[ApplicationStateExceptionKey] = exception;
                Response.Redirect(BuildRedirectPath(VirtualPathAppSettingsKey, Enums.ErrorMessage.ValidationError), true);
            }
            else if (exception.InnerException is System.ArgumentException)
            {
                globalExceptionLogging.LogNonCriticalError(Request, exception);
                Application[ApplicationStateExceptionKey] = exception;
                Response.Redirect(BuildRedirectPath(VirtualPathAppSettingsKey, Enums.ErrorMessage.HardError), true);
            }
            else if (exception.InnerException is System.Web.UI.ViewStateException
                || exception.InnerException is System.Data.SqlClient.SqlException)
            {
                globalExceptionLogging.LogNonCriticalError(Request, exception);
                Application[ApplicationStateExceptionKey] = exception;
                Response.Redirect(BuildRedirectPath(VirtualPathAppSettingsKey, Enums.ErrorMessage.Timeout), true);
            }
            else if (exception is System.Transactions.TransactionException
                || (exception.InnerException is System.Transactions.TransactionAbortedException))
            {
                globalExceptionLogging.LogNonCriticalError(Request, exception);
                Application[ApplicationStateExceptionKey] = exception;
                Response.Redirect(BuildRedirectPath(AccountsVirtualPathAppSettingsKey, Enums.ErrorMessage.Timeout), true);
            }
            else if (exception.InnerException is HttpException) //We should check exception here not InnerExeption
            {
                HandleHttpExeption(exception.InnerException, globalExceptionLogging);
            }
            else
            {
                HandleGeneralException(exception, globalExceptionLogging);
                Application[ApplicationStateExceptionKey] = exception;
            }
        }

        private void HandleGeneralException(Exception exception, GlobalExceptionLogging globalExceptionLogging)
        {
            Enums.ErrorMessage error;
            if (exception.Message.Contains("does not exist")
                    || exception.Message.Contains("ASP.NET session has expired or could not be found"))
            {
                // catching file does not exist
                error = Enums.ErrorMessage.HardError;
                Response.Redirect(BuildRedirectPath(CommunicatorVirtualPathAppSettingsKey, error), true);
            }
            else if (exception.Message.Contains("A potentially dangerous Request"))
            {
                // dangerous Request.Path
                error = Enums.ErrorMessage.InvalidLink;
                globalExceptionLogging.LogNonCriticalError(Request, exception);
                Response.Redirect(BuildRedirectPath(CommunicatorVirtualPathAppSettingsKey, error), true);
            }
            else
            {
                // Any other error
                error = Enums.ErrorMessage.HardError;
                globalExceptionLogging.LogNonCriticalErrorWithExtendedHandling(Request, exception);
                Response.Redirect(BuildRedirectPath(CommunicatorVirtualPathAppSettingsKey, error), true);
            }
        }

        private void HandleHttpExeption(Exception exception, GlobalExceptionLogging globalExceptionLogging)
        {
            try
            {
                var httpError = exception as HttpException;
                if (httpError != null)
                {
                    var error = globalExceptionLogging.ClassifyAndLogHttpException(httpError, Request);
                    Application[ApplicationStateExceptionKey] = httpError;
                    Response.Redirect(BuildRedirectPath(VirtualPathAppSettingsKey, error), true);
                }
            }
            catch (Exception) // to stay consistent with legacy behavior
            { }
        }

        private static string BuildRedirectPath(string virtualPathAppSettingsKey, Enums.ErrorMessage error)
        {
            return string.Format("{0}{1}{2}",
                ConfigurationManager.AppSettings[virtualPathAppSettingsKey],
                RedirectDestination,
                error);
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}