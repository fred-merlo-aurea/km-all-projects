using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Text;
using System.Transactions;
using System.Configuration;
using System.Diagnostics;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_Common.Objects;

namespace ecn.menu
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private const string SourceMethod = "Global.Application_Error";

        private const string VirtualPathAppSettingsKey = "Communicator_VirtualPath";
        private const string AccountsVirtualPathAppSettingsKey = "Accounts_VirtualPath";

        private const string ApplicationIdAppSettingsKey = "KMCommon_Application";
        private const string ApplicationStateExceptionKey = "err";
        private const string RedirectDestination = "/Error?E=";


        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            var applicationId = Convert.ToInt32(ConfigurationManager.AppSettings[ApplicationIdAppSettingsKey]);
            var globalExceptionLogging = new GlobalExceptionLogging(SourceMethod, applicationId);

            if (exception.InnerException is SecurityException)
            {
                var securityException = exception.InnerException as SecurityException;
                if (securityException.SecurityType == Enums.SecurityExceptionType.RoleAccess)
                {
                    Response.Redirect(ConfigurationManager.AppSettings[AccountsVirtualPathAppSettingsKey] + "/main/securityAccessError.aspx", true);
                }
                else if (securityException.SecurityType == Enums.SecurityExceptionType.FeatureNotEnabled)
                {
                    Response.Redirect(ConfigurationManager.AppSettings[AccountsVirtualPathAppSettingsKey] + "/main/featureAccessError.aspx", true);
                }
            }
            else if (exception is ECNException)
            {
                globalExceptionLogging.LogNonCriticalError(Request, exception);
                Application[ApplicationStateExceptionKey] = exception;
                Response.Redirect(BuildRedirectPath(VirtualPathAppSettingsKey, Enums.ErrorMessage.ValidationError), true);
            }
            else if (exception.InnerException is ArgumentException)
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
            else if (exception is TransactionException)
            {
                globalExceptionLogging.LogNonCriticalError(Request, exception);
                Application[ApplicationStateExceptionKey] = exception;
                // For review. Most likely this redirect path is incorrect
                Response.Redirect(
                    $"{ConfigurationManager.AppSettings[AccountsVirtualPathAppSettingsKey]}/error.aspx?E={Enums.ErrorMessage.Timeout}", true);
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

            if (exception.Message.Contains("A potentially dangerous Request"))
            {
                error = Enums.ErrorMessage.InvalidLink;
                globalExceptionLogging.LogNonCriticalError(Request, exception);
                Response.Redirect(BuildRedirectPath(VirtualPathAppSettingsKey, error), true);
            }
            else if (exception.Message.Contains("does not exist"))
            {
                error = Enums.ErrorMessage.InvalidLink;
                Response.Redirect(BuildRedirectPath(VirtualPathAppSettingsKey, error), true);
            }
            else if (exception.Message.Contains("ASP.NET session has expired or could not be found"))
            {
                error = Enums.ErrorMessage.HardError;
                Response.Redirect(BuildRedirectPath(VirtualPathAppSettingsKey, error), true);
            }
            else
            {
                // Any other error
                error = Enums.ErrorMessage.Timeout;
                globalExceptionLogging.LogNonCriticalErrorWithExtendedHandling(Request, exception);
                Response.Redirect(BuildRedirectPath(VirtualPathAppSettingsKey, error), true);
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
            catch (Exception ex) // to stay consistent with legacy behavior
            {
                Trace.TraceError($"Exception while processing HttpException. Exception: {ex.ToString()}");
            }
        }

        private static string BuildRedirectPath(string virtualPathAppSettingsKey, Enums.ErrorMessage error)
        {
            return string.Format("{0}{1}{2}",
              ConfigurationManager.AppSettings[virtualPathAppSettingsKey],
              RedirectDestination,
              error);
        }
    }
}