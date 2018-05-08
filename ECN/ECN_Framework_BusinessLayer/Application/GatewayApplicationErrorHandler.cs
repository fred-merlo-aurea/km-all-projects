using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.UI;
using ECN_Framework_Common.Objects;
using static ECN_Framework_Common.Objects.Enums;

namespace ECN_Framework_BusinessLayer.Application
{
    public class GatewayApplicationErrorHandler : GlobalApplicationErrorHandlerBase
    {
        private const string ErrorController = "Error";
        private const string ErrorAction = "Error";

        public GatewayApplicationErrorHandler(
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
            if (lastException is ECNException)
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
                RedirectToErrorPage(lastException, ErrorMessage.ValidationError, string.Empty);
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

        protected override ErrorMessage HandleGeneralException(Exception lastException, ErrorMessage error)
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
            else
            {
                LogCriticalErrorWithUserInfoAndRedirectToErrorPage(
                    lastException,
                    ErrorMessage.HardError,
                    PageUrl);
            }

            return error;
        }

        protected override void NavigateToErrorPage(ErrorMessage error, string pagePath, string errorPage)
        {
            Response.RedirectToRoute(
                        new
                        {
                            controller = ErrorController,
                            action = ErrorAction,
                            message = error.ToString()
                        });
        }
    }
}
