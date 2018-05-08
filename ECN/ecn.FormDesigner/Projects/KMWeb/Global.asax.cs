using System;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_Common.Objects;

namespace KMWeb
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private const string SourceMethod = "Global.Application_Error";
        private const string VirtualPathAppSettingsKey = "Forms_VirtualPath";
        private const string AccountsVirtualPathAppSettingsKey = "Accounts_VirtualPath";
        private const string ApplicationIdAppSettingsKey = "KMCommon_Application";
        private const string ApplicationStateExceptionKey = "err";
        private const string RedirectDestination = "/Error/";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalConfiguration.Configure(WebApiConfig.Register); //Web API 2
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            var applicationId = Convert.ToInt32(ConfigurationManager.AppSettings[ApplicationIdAppSettingsKey]);
            var globalExceptionLogging = new GlobalExceptionLogging(SourceMethod, applicationId);

            if (exception.InnerException is SecurityException)
            {
                var securityException = (SecurityException)exception.InnerException;
                if (securityException.SecurityType == Enums.SecurityExceptionType.RoleAccess)
                {
                    Response.Redirect(ConfigurationManager.AppSettings[AccountsVirtualPathAppSettingsKey] + "/main/securityAccessError.aspx", true);
                }
                else if (securityException.SecurityType == Enums.SecurityExceptionType.FeatureNotEnabled)
                {
                    Response.Redirect(ConfigurationManager.AppSettings[AccountsVirtualPathAppSettingsKey] + "/main/featureAccessError.aspx", true);
                }
            }
            else if (exception is ECNException
                || exception.InnerException is ArgumentException
                || exception.InnerException is SqlException)
            {
                globalExceptionLogging.LogNonCriticalError(Request, exception);
                Application[ApplicationStateExceptionKey] = exception;
                Response.Redirect(BuildRedirectPath(Enums.ErrorMessage.HardError), true);
            }
            else if (exception.InnerException is ViewStateException
                 || exception is TransactionException)
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
            else if (exception.Message.Contains("Validation failed for one or more entities"))
            {
                var userInfo = GetValidationErrorUserInfo(exception);
                globalExceptionLogging.LogCriticalErrorWithCustomInfo(exception, userInfo.ToString());
                Application[ApplicationStateExceptionKey] = exception;
                Response.Redirect(BuildRedirectPath(Enums.ErrorMessage.HardError), true);
            }
            else
            {
                // Any other error
                globalExceptionLogging.LogCriticalErrorWithRequestBody(Request, exception);
                Application[ApplicationStateExceptionKey] = exception;
                Response.Redirect(BuildRedirectPath(Enums.ErrorMessage.HardError), true);
            }
        }

        private string GetValidationErrorUserInfo(Exception exception)
        {
            var userInfo = new StringBuilder();
            var entityValidationException = (DbEntityValidationException)exception;
            try
            {
                userInfo.Append(GlobalExceptionLogging.FormatLogMessage(Request, exception));

                // Logging error details
                var errorDetails = new StringBuilder();
                foreach (var failure in entityValidationException.EntityValidationErrors)
                {
                    errorDetails.AppendFormat("Type \"{0}\" in state \"{1}\" has the following validation errors:\n",
                        failure.Entry.
                        Entity.GetType().Name,
                        failure.Entry.State);
                    foreach (var fail in failure.ValidationErrors)
                    {
                        errorDetails.AppendFormat("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                            fail.PropertyName,
                            failure.Entry.CurrentValues.GetValue<object>(fail.PropertyName),
                            fail.ErrorMessage);
                        errorDetails.AppendLine();
                    }
                }
                userInfo.AppendLine("<BR>Entity Validation Error: " + errorDetails.ToString());
            }
            catch (Exception ex) // to stay consistent with legacy behavior
            {
                Trace.TraceError($"Exception while formatting log message. Exception: {ex.ToString()}");
            }

            return userInfo.ToString();
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
            return string.Format ("{0}{1}{2}",
                ConfigurationManager.AppSettings[VirtualPathAppSettingsKey],
                RedirectDestination,
                error);
        }
    }
}