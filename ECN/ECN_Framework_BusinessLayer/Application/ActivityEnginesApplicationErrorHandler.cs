using System;
using System.Data.SqlClient;
using System.Transactions;
using System.Web;
using System.Web.UI;
using ECN_Framework_Common.Objects;
using static ECN_Framework_Common.Objects.Enums;

namespace ECN_Framework_BusinessLayer.Application
{
    public class ActivityEnginesApplicationErrorHandler : GlobalApplicationErrorHandlerBase
    {
        public ActivityEnginesApplicationErrorHandler(
            HttpServerUtility server, 
            HttpRequest request, 
            HttpResponse response, 
            HttpApplicationState application, 
            string applicationId, 
            string pageUrl) : base(server, request, response, application, applicationId, pageUrl)
        {
        }

        public override void HandleApplicationError()
        {
            var lastException = Server.GetLastError();
            var error = ErrorMessage.HardError;
            if (lastException.InnerException is SecurityException)
            {
                HandleSecurityException(lastException);
            }
            else if (lastException is ECNException)
            {
                error = ErrorMessage.ValidationError;
                var ecnExceptionInformation = GetECNExceptionInformation(lastException);
                LogErrorAndRedirectToErrorPage(
                    lastException,
                    error,
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
            else if (lastException.InnerException is ViewStateException
                || lastException.Message.IndexOf("Invalid viewstate", StringComparison.OrdinalIgnoreCase) != -1)
            {
                error = ErrorMessage.Timeout;
                LogErrorAndRedirectToErrorPage(
                    lastException,
                    error,
                    PageUrl);
            }
            else if (lastException.InnerException is SqlException
                || lastException.Message.IndexOf("was deadlocked on lock resource", StringComparison.OrdinalIgnoreCase) != -1)
            {
                error = ErrorMessage.Timeout;
                LogErrorAndRedirectToErrorPage(
                    lastException,
                    error,
                    PageUrl);
            }
            else if (lastException is TransactionException
                || lastException.Message.IndexOf("The transaction has aborted", StringComparison.OrdinalIgnoreCase) != -1)
            {
                error = ErrorMessage.Timeout;
                LogErrorAndRedirectToErrorPage(
                    lastException,
                    error,
                    PageUrl);
            }
            else if (lastException.InnerException is HttpException)
            {
                error = HandleHttpException(lastException.InnerException, error);
            }
            else if (lastException.InnerException is TimeoutException)
            {
                error = ErrorMessage.Timeout;
                LogErrorAndRedirectToErrorPage(
                    lastException,
                    error,
                    PageUrl);
            }
            else
            {
                error = HandleGeneralException(lastException, error);
            }

            Server.ClearError();
            NavigateToErrorPage(error, string.Empty, string.Empty);
        }

        protected override void HandleSecurityException(Exception lastException)
        {
            NavigateToErrorPage(ErrorMessage.HardError, string.Empty, string.Empty);
        }

        protected override void NavigateToErrorPage(ErrorMessage error, string pagePath, string errorPage)
        {
            Server.Transfer(
                    string.Format("Error.aspx?E={0}", error.ToString()),
                    false);
        }

        protected override ErrorMessage HandleGeneralException(Exception lastException, ErrorMessage error)
        {
            if (lastException == null)
            {
                throw new ArgumentNullException(nameof(lastException));
            }

            if (lastException.Message.Contains("does not exist"))
            {
                error = ErrorMessage.HardError;
                RedirectToErrorPage(
                    lastException,
                    error,
                    PageUrl);
            }
            else if (lastException.Message.Contains("ASP.NET session has expired or could not be found"))
            {
                RedirectToErrorPage(
                    lastException,
                    ErrorMessage.HardError,
                    PageUrl);
            }
            else if (lastException.Message.Contains("A potentially dangerous Request"))
            {
                error = ErrorMessage.InvalidLink;
                RedirectToErrorPage(
                    lastException,
                    error,
                    PageUrl);
            }
            else if (lastException.Message.Contains("This is an invalid"))
            {
                error = ErrorMessage.PageNotFound;
                RedirectToErrorPage(
                    lastException,
                    error,
                    PageUrl);
            }
            else
            {
                error = ErrorMessage.HardError;
                LogCriticalErrorWithUserInfoAndRedirectToErrorPage(
                    lastException,
                    error,
                    PageUrl);
            }

            return error;
        }
    }
}
