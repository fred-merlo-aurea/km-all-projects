using System;
using System.Data.SqlClient;
using System.Transactions;
using System.Web;
using System.Web.UI;
using ECN_Framework_Common.Objects;
using static ECN_Framework_Common.Objects.Enums;

namespace ECN_Framework_BusinessLayer.Application
{
    public class CommunicatorMvcApplicationErrorHandler : GlobalApplicationErrorHandlerBase
    {
        private const string MvcErrorPage = "Error";
        private string AccountPageUrl { get; set; }

        public CommunicatorMvcApplicationErrorHandler(
            HttpServerUtility server, 
            HttpRequest request, 
            HttpResponse response, 
            HttpApplicationState application, 
            string applicationId,
            string pageUrl,
            string accountPageUrl) : base(server, request, response, application, applicationId, pageUrl)
        {
            AccountPageUrl = AccountPageUrl;
        }

        public override void HandleApplicationError()
        {
            var lastException = Server.GetLastError();
            var error = ErrorMessage.HardError;
            if (lastException.InnerException is SecurityException
                || lastException is SecurityException)
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
                    MvcErrorPage,
                    ecnExceptionInformation);
            }
            else if (lastException.InnerException is ArgumentException
                || lastException is ArgumentException)
            {
                LogErrorAndRedirectToErrorPage(
                    lastException,
                    ErrorMessage.HardError,
                    PageUrl,
                    MvcErrorPage);
            }
            else if (lastException.InnerException is ViewStateException)
            {
                LogErrorAndRedirectToErrorPage(
                    lastException,
                    ErrorMessage.Timeout,
                    PageUrl,
                    MvcErrorPage);
            }
            else if (lastException.InnerException is SqlException)
            {
                LogErrorAndRedirectToErrorPage(
                    lastException,
                    ErrorMessage.Timeout,
                    PageUrl,
                    MvcErrorPage);
            }
            else if (lastException is TransactionException)
            {
                LogErrorAndRedirectToErrorPage(
                    lastException,
                    ErrorMessage.Timeout,
                    AccountPageUrl);
            }
            else if (lastException.InnerException is HttpException)
            {
                HandleHttpException(lastException.InnerException, error, MvcErrorPage);
            }
            else
            {
                HandleGeneralException(lastException, error);
            }
        }

        protected override void HandleSecurityException(Exception lastException)
        {
            if (lastException.InnerException is SecurityException 
                || lastException is SecurityException)
            {
                var securityException = (lastException.InnerException != null 
                                            ? lastException.InnerException 
                                            : lastException) as SecurityException;

                if (securityException == null)
                {
                    throw new InvalidOperationException("securityException");
                }

                if (securityException.SecurityType == SecurityExceptionType.RoleAccess)
                {
                    Response.Redirect(string.Format("{0}/Error/FeatureAccess", PageUrl), true);
                }
                else if (securityException.SecurityType == SecurityExceptionType.FeatureNotEnabled)
                {
                    Response.Redirect(string.Format("{0}/Error/FeatureAccess", PageUrl), true);
                }
                else if (securityException.SecurityType == SecurityExceptionType.Security)
                {
                    Response.Redirect(string.Format("{0}/Error/SecurityAccess", PageUrl), true);
                }
            }
        }

        protected override ErrorMessage HandleGeneralException(Exception lastException, ErrorMessage error)
        {
            if (lastException == null)
            {
                throw new ArgumentNullException(nameof(lastException));
            }

            if (lastException.Message.Contains("A potentially dangerous Request"))
            {
                HandleDangerousRequest(lastException, ErrorMessage.InvalidLink, MvcErrorPage);
            }
            else if (lastException.Message.Contains("does not exist"))
            {
                RedirectToErrorPage(
                    lastException,
                    ErrorMessage.InvalidLink,
                    PageUrl,
                    MvcErrorPage);
            }
            else if (lastException.Message.Contains("ASP.NET session has expired or could not be found"))
            {
                RedirectToErrorPage(
                    lastException,
                    ErrorMessage.HardError,
                    PageUrl,
                    MvcErrorPage);
            }
            else if (lastException.Message.Contains("was not found on controller"))
            {
                RedirectToErrorPage(
                    lastException,
                    ErrorMessage.PageNotFound,
                    PageUrl,
                    MvcErrorPage);
            }
            else
            {
                LogNonCriticalErrorWithUserInfoAndRedirectToErrorPage(
                    lastException,
                    ErrorMessage.Unknown,
                    PageUrl,
                    MvcErrorPage);
            }

            return error;
        }
    }
}
