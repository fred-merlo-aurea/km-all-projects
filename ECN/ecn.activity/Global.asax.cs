using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.UI;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_Common.Objects;

namespace ecn.activity
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private const string SourceMethod = "Global.Application_Error";
        private const string ApplicationIdAppSettingsKey = "KMCommon_Application";
        private const string ApplicationStateExceptionKey = "err";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            var applicationId = Convert.ToInt32(ConfigurationManager.AppSettings[ApplicationIdAppSettingsKey]);
            var globalExceptionLogging = new GlobalExceptionLogging(SourceMethod, applicationId);

            if (exception.InnerException is SecurityException)
            {
                RedirectToErrorController(Enums.ErrorMessage.HardError);
            }
            else if (exception is ECNException)
            {
                globalExceptionLogging.LogNonCriticalError(Request, exception);
                Application[ApplicationStateExceptionKey] = exception;
                RedirectToErrorController(Enums.ErrorMessage.ValidationError);
            }
            else if (exception.InnerException is ArgumentException)
            {
                globalExceptionLogging.LogNonCriticalError(Request, exception);
                Application[ApplicationStateExceptionKey] = exception;
                RedirectToErrorController(Enums.ErrorMessage.HardError);
            }
            else if (exception.InnerException is ViewStateException
             || exception.InnerException is SqlException
             || exception is TransactionException)
            {
                globalExceptionLogging.LogNonCriticalError(Request, exception);
                Application[ApplicationStateExceptionKey] = exception;
                RedirectToErrorController(Enums.ErrorMessage.Timeout);
            }
            else if (exception.InnerException is HttpException)
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

            if (exception.Message.Contains("A potentially dangerous Request"))
            {
                error = Enums.ErrorMessage.InvalidLink;
                globalExceptionLogging.LogNonCriticalError(Request, exception);
                RedirectToErrorController(error);
            }
            else if (exception.Message.Contains("does not exist"))
            {
                error = Enums.ErrorMessage.InvalidLink;
                RedirectToErrorController(error);
            }
            else if (exception.Message.Contains("ASP.NET session has expired or could not be found"))
            {
                error = Enums.ErrorMessage.HardError;
                RedirectToErrorController(error);
            }
            else if (exception.Message.Contains("This is an invalid"))
            {
                // catching invalid webresource request
                Application[ApplicationStateExceptionKey] = exception;
                Server.Transfer($"Error.aspx?E={Enums.ErrorMessage.PageNotFound}", false);
            }
            else
            {
                // Any other error
                error = Enums.ErrorMessage.Timeout;
                globalExceptionLogging.LogNonCriticalError(Request, exception);
                RedirectToErrorController(error);
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
                    RedirectToErrorController(error);
                }
            }
            catch (Exception ex) // to stay consistent with legacy behavior
            {
                Trace.TraceError($"Exception while processing HttpException. Exception: {ex.ToString()}");
            }
        }

        private void RedirectToErrorController(Enums.ErrorMessage error)
        {
            Response.RedirectToRoute(new { controller = "Error", action = "Error", error = error.ToString() });
        }
    }
}