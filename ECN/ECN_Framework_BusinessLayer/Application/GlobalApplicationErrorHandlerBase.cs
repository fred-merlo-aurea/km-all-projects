using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.UI;
using ECN_Framework_Common.Objects;
using KM.Common.Entity;
using static ECN_Framework_Common.Objects.Enums;

namespace ECN_Framework_BusinessLayer.Application
{
    public class GlobalApplicationErrorHandlerBase
    {
        private const string GlobalApplicationErrorMethodName = "Global.Application_Error";
        private const string HttpPostKey = "HTTP_HOST";
        protected const string AspxErrorPage = "error.aspx";
        protected const string ApplicationErrorKey = "err";

        protected HttpServerUtility Server { get; private set; }
        protected HttpRequest Request { get; private set; }
        protected HttpResponse Response { get; private set; }
        protected HttpApplicationState Application { get; private set; }
        protected string PageUrl { get; private set; }

        private string ApplicationId { get; set; }

        public GlobalApplicationErrorHandlerBase(
            HttpServerUtility server,
            HttpRequest request,
            HttpResponse response,
            HttpApplicationState application,
            string applicationId,
            string pageUrl)
        {
            Server = server;
            Request = request;
            Response = response;
            Application = application;
            ApplicationId = applicationId;
            PageUrl = pageUrl;
        }

        // This method can be overridden by child classes.
        // Each child class could implement its own beharior to handle application error
        public virtual void HandleApplicationError()
        {
            var lastException = Server.GetLastError();
            var error = ErrorMessage.HardError;
            if (lastException.InnerException is SecurityException)
            {
                HandleSecurityException(lastException);
            }
            else if (lastException is ECNException)
            {
                var ecnExceptionInformation = GetECNExceptionInformation(lastException);
                LogErrorAndRedirectToErrorPage(
                    lastException,
                    ErrorMessage.ValidationError,
                    PageUrl,
                    AspxErrorPage,
                    ecnExceptionInformation);
            }
            else if (lastException.InnerException is ArgumentException)
            {
                LogErrorAndRedirectToErrorPage(
                    lastException,
                    ErrorMessage.HardError,
                    PageUrl);
            }
            else if (lastException.InnerException is ViewStateException)
            {
                LogErrorAndRedirectToErrorPage(
                    lastException,
                    ErrorMessage.Timeout,
                    PageUrl);
            }
            else if (lastException is TransactionException)
            {
                LogErrorAndRedirectToErrorPage(
                    lastException,
                    ErrorMessage.Timeout,
                    PageUrl);
            }
            else if (lastException.InnerException is SqlException)
            {
                LogErrorAndRedirectToErrorPage(
                    lastException,
                    ErrorMessage.Timeout,
                    PageUrl);
            }
            else if (lastException is HttpException)
            {
                HandleHttpException(lastException, error);
            }
            else
            {
                HandleGeneralException(lastException, error);
            }
        }

        // This method can be overridden by child classes.
        // Each child class could implement its own beharior to handle general exception
        protected virtual ErrorMessage HandleGeneralException(Exception lastException, ErrorMessage error)
        {
            if (lastException == null)
            {
                throw new ArgumentNullException(nameof(lastException));
            }

            if (lastException.Message.Contains("does not exist"))
            {
                RedirectToErrorPage(
                    lastException,
                    ErrorMessage.HardError,
                    PageUrl);
            }
            else if (lastException.Message.Contains("ASP.NET session has expired or could not be found"))
            {
                RedirectToErrorPage(
                    lastException,
                    ErrorMessage.HardError,
                    PageUrl);
            }
            else
            {
                LogCriticalErrorWithUserInfoAndRedirectToErrorPage(
                    lastException,
                    ErrorMessage.HardError,
                    PageUrl);
            }

            return ErrorMessage.HardError;
        }

        protected virtual string AppendCurrentUserInfoIfExists()
        {
            return GetCurrentUserInfoFromSession();
        }

        // This method should be implemented by child classes.
        // Each child class should implement its own beharior to handle security exception
        protected virtual void HandleSecurityException(Exception securityException)
        {
            throw new NotImplementedException();
        }

        protected virtual void NavigateToErrorPage(ErrorMessage error, string pagePath, string errorPage)
        {
            Response.Redirect(
                        string.Format("{0}/{1}?E={2}", pagePath, errorPage, error.ToString()),
                        true);
        }

        protected void RedirectToErrorPage(Exception exception, ErrorMessage error, string pagePath, string errorPage = AspxErrorPage)
        {
            Application[ApplicationErrorKey] = exception;
            NavigateToErrorPage(error, pagePath, errorPage);
        }

        protected void LogCriticalError(Exception exception, string errorMessage)
        {
            var applicationId = GetApplicationId();
            ApplicationLog.LogCriticalError(
                                        exception,
                                        GlobalApplicationErrorMethodName,
                                        applicationId,
                                        errorMessage);
        }

        protected string GetCurrentUserInfoFromSession()
        {
            var userInfo = new StringBuilder();

            var ecnSession = ECNSession.CurrentSession();
            if (ecnSession != null)
            {
                userInfo.AppendFormat("<BR><BR>CustomerID: {0}", ecnSession.CurrentUser.CustomerID.ToString());
                userInfo.AppendLine();
                userInfo.AppendFormat("<BR>UserName: {0}", ecnSession.CurrentUser.UserName);
                userInfo.AppendLine();
            }

            return userInfo.ToString();
        }

        protected void LogNonCriticalErrorWithUserInfoAndRedirectToErrorPage(Exception lastException, ErrorMessage error, string pagePath, string errorPage = AspxErrorPage)
        {
            var userInfo = new StringBuilder();
            try
            {
                userInfo.Append(GetCurrentUserInfoFromSession());
                AppendPageUrlAndUserAgent(userInfo);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            LogNonCriticalError(lastException, userInfo.ToString());
            RedirectToErrorPage(lastException, error, pagePath, errorPage);
        }

        protected void LogCriticalErrorWithUserInfoAndRedirectToErrorPage(Exception lastException, ErrorMessage error, string pagePath, string errorPage = AspxErrorPage)
        {
            var userInfo = new StringBuilder();
            try
            {
                userInfo.Append(GetCurrentUserInfoFromSession());
                AppendPageUrlAndUserAgent(userInfo);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            LogCriticalError(lastException, userInfo.ToString());
            RedirectToErrorPage(lastException, error, pagePath, errorPage);
        }

        protected void AppendPageUrlAndUserAgent(StringBuilder userInfo)
        {
            AppendPageUrl(userInfo);
            AppendUserAgent(userInfo);
        }

        protected void HandleDangerousRequest(Exception exception, ErrorMessage error, string errorPage = AspxErrorPage)
        {
            var userInfo = new StringBuilder();
            try
            {
                userInfo.Append(AppendCurrentUserInfoIfExists());
                AppendPageUrlAndUserAgent(userInfo);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }

            LogNonCriticalError(exception, userInfo.ToString());
            RedirectToErrorPage(exception, error, PageUrl, errorPage);
        }

        private void AppendUserAgent(StringBuilder userInfo)
        {
            userInfo.AppendFormat("<BR>User Agent: {0}", Request.UserAgent.ToString());
            userInfo.AppendLine();
        }

        private void AppendPageUrl(StringBuilder userInfo)
        {
            userInfo.AppendFormat("<BR>Page URL: {0}{1}", HttpContext.Current.Request.ServerVariables[HttpPostKey].ToString(), Request.RawUrl.ToString());
            userInfo.AppendLine();
        }

        protected ErrorMessage HandleHttpException(Exception lastException, ErrorMessage error, string errorPage = AspxErrorPage)
        {
            try
            {
                var httpError = lastException as HttpException;
                if (httpError != null)
                {
                    if (httpError.GetHttpCode() == 404)
                    {
                        error = ErrorMessage.PageNotFound;
                    }
                    else if (httpError.GetHttpCode() == 400 || httpError.GetBaseException().GetType() == typeof(HttpRequestValidationException))
                    {
                        error = ErrorMessage.InvalidLink;
                    }
                    else if (httpError.GetBaseException().GetType() == typeof(ViewStateException))
                    {
                        error = ErrorMessage.Timeout;
                        LogNonCriticalError(httpError);
                    }
                    else if (httpError.GetBaseException().GetType() == typeof(ArgumentException))
                    {
                        error = ErrorMessage.InvalidLink;
                        LogNonCriticalError(httpError);
                    }
                    else if (httpError.GetBaseException().GetType() == typeof(HttpException) 
                        || httpError.GetBaseException().GetType() == typeof(HttpRequestValidationException))
                    {
                        error = ErrorMessage.InvalidLink;
                    }
                    else
                    {
                        var adminEmailVariables = new StringBuilder();
                        try
                        {
                            AppendPageUrl(adminEmailVariables);
                            adminEmailVariables.AppendFormat("<BR>SPY Info:&nbsp;[{0}] / [{1}]", Request.UserHostAddress, Request.UserAgent);
                            adminEmailVariables.AppendLine();
                            if (Request.UrlReferrer != null)
                            {
                                adminEmailVariables.AppendFormat("<BR>Referring URL: {0}", Request.UrlReferrer.ToString());
                                adminEmailVariables.AppendLine();
                            }

                            adminEmailVariables.AppendLine("<BR>HEADERS");
                            foreach (var key in Request.Headers.AllKeys)
                            {
                                adminEmailVariables.AppendFormat("<BR>{0}:{1}", key, Request.Headers[key]);
                            }

                            adminEmailVariables.AppendLine();
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex);
                        }

                        LogCriticalError(httpError, adminEmailVariables.ToString());
                    }

                    RedirectToErrorPage(httpError, error, PageUrl, errorPage);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }

            return error;
        }

        protected void LogNonCriticalError(Exception exception, string errorMessage = "")
        {
            var applicationId = GetApplicationId();
            ApplicationLog.LogNonCriticalError(
                exception,
                GlobalApplicationErrorMethodName,
                applicationId,
                errorMessage);
        }

        private int GetApplicationId()
        {
            int applicationId;
            if (!int.TryParse(ApplicationId, out applicationId))
            {
                Trace.WriteLine("Unable to parse ApplicationId");
            }
            return applicationId;
        }

        protected void LogErrorAndRedirectToErrorPage(Exception lastException, ErrorMessage error, string pagePath, string errorPage = AspxErrorPage, string ecnExceptionInformation = "")
        {
            var userInfo = new StringBuilder();
            try
            {
                userInfo.Append(AppendCurrentUserInfoIfExists());
                AppendPageUrlAndUserAgent(userInfo);
                userInfo.Append(ecnExceptionInformation);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
            LogNonCriticalError(lastException, userInfo.ToString());
            RedirectToErrorPage(lastException, error, pagePath, errorPage);
        }

        protected string GetECNExceptionInformation(Exception lastException)
        {
            var exceptionInfo = new StringBuilder();

            var excException = lastException as ECNException;
            if (excException != null)
            {
                foreach (var ecnError in excException.ErrorList)
                {
                    exceptionInfo.AppendFormat("<BR>Entity: {0}{1}", ecnError.Entity, Environment.NewLine);
                    exceptionInfo.AppendFormat("<BR>Method: {0}{1}", ecnError.Method, Environment.NewLine);
                    exceptionInfo.AppendFormat("<BR>Message: {0}{1}", ecnError.ErrorMessage, Environment.NewLine);
                }
            }

            return exceptionInfo.ToString();
        }
    }
}
