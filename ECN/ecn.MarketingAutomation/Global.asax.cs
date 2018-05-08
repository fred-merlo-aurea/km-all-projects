using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_Common.Objects;

namespace ecn.MarketingAutomation
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private const string SourceMethod = "Global.Application_Error";
        private const string VirtualPathAppSettingsKey = "marketingautomation_VirtualPath";
        private const string ApplicationIdAppSettingsKey = "KMCommon_Application";
        private const string ApplicationStateExceptionKey = "err";
        private const string RedirectDestination = "/Error/";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            var applicationId = Convert.ToInt32(ConfigurationManager.AppSettings[ApplicationIdAppSettingsKey]);
            var globalExceptionLogging = new GlobalExceptionLogging(SourceMethod, applicationId);

            if (exception.InnerException is SecurityException)
            {
                Response.Clear();
                Server.ClearError();
                Response.StatusCode = 500;
                Response.Redirect(BuildRedirectPath(Enums.ErrorMessage.HardError), true);
            }
            else if (exception is ECNException
                || exception.InnerException is ArgumentException
                || exception.InnerException is System.Data.SqlClient.SqlException)
            {
                globalExceptionLogging.LogNonCriticalError(Request, exception);
                Application[ApplicationStateExceptionKey] = exception;
                Response.Redirect(BuildRedirectPath(Enums.ErrorMessage.HardError), true);
            }
            else if (exception.InnerException is System.Web.UI.ViewStateException
                || exception is System.Transactions.TransactionException)
            {
                globalExceptionLogging.LogNonCriticalError(Request, exception);
                Application[ApplicationStateExceptionKey] = exception;
                Response.Redirect(BuildRedirectPath(Enums.ErrorMessage.Timeout), true);
            }
            else if (exception is HttpException)
            {
                HandleHttpException(exception, globalExceptionLogging);
            }
            else
            {
                HandleGeneralException(exception, globalExceptionLogging);
            }
        }

        private void HandleGeneralException(Exception exception, GlobalExceptionLogging globalExceptionLogging)
        {
            if (exception.Message.Contains("does not exist")
                 || exception.Message.Contains("ASP.NET session has expired or could not be found"))
            {
                Application[ApplicationStateExceptionKey] = exception;
                Response.Redirect(BuildRedirectPath(Enums.ErrorMessage.HardError), true);
            }
            else if (exception.Message.Contains("A potentially dangerous Request"))
            {
                // dangerous Request.Path
                globalExceptionLogging.LogNonCriticalError(Request, exception);
                Application[ApplicationStateExceptionKey] = exception;
                Response.Redirect(BuildRedirectPath(Enums.ErrorMessage.InvalidLink), true);
            }
            else
            {
                // Any other error
                globalExceptionLogging.LogCriticalErrorWithRequestBody(Request, exception);
                Application[ApplicationStateExceptionKey] = exception;
                Response.Redirect(BuildRedirectPath(Enums.ErrorMessage.HardError), true);
            }
        }

        private void HandleHttpException(Exception exception, GlobalExceptionLogging globalExceptionLogging)
        {
            try
            {
                var httpError = exception as HttpException;
                if (httpError != null)
                {
                    var error = globalExceptionLogging.ClassifyAndLogHttpException(httpError, Request);
                    Application[ApplicationStateExceptionKey] = httpError;
                    Response.Redirect(BuildRedirectPath(error), true);
                }
            }
            catch (Exception ex) // to stay consistent with legacy behavior
            {
                Trace.TraceError($"Exception while processing HttpException. Exception: {ex.ToString()}");
            }
        }

        private static string BuildRedirectPath(Enums.ErrorMessage error)
        {
            return string.Format("{0}{1}{2}",
                ConfigurationManager.AppSettings[VirtualPathAppSettingsKey],
                RedirectDestination,
                error);
        }
    }
}
