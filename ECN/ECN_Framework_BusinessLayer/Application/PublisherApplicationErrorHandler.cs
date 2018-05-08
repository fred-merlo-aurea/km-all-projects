using System;
using System.Data.SqlClient;
using System.Transactions;
using System.Web;
using System.Web.UI;
using ECN_Framework_Common.Objects;
using static ECN_Framework_Common.Objects.Enums;

namespace ECN_Framework_BusinessLayer.Application
{
    public class PublisherApplicationErrorHandler : GlobalApplicationErrorHandlerBase
    {
        private string AccountPageUrl { get; set; }

        public PublisherApplicationErrorHandler(
            HttpServerUtility server, 
            HttpRequest request, 
            HttpResponse response, 
            HttpApplicationState application,
            string applicationId,
            string pageUrl,
            string accountPageUrl) : base(server, request, response, application, applicationId, pageUrl)
        {
            AccountPageUrl = accountPageUrl;
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
            else if (lastException.InnerException is SqlException)
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
                    AccountPageUrl);
            }
            else if (lastException.InnerException is HttpException)
            {
                HandleHttpException(lastException.InnerException, error);
            }
            else
            {
                HandleGeneralException(lastException, error);
            }
        }

        protected override void HandleSecurityException(Exception securityException)
        {
            Response.Redirect(
                        string.Format("{0}/main/securityAccessError.aspx", AccountPageUrl), 
                        true);
        }
    }
}
