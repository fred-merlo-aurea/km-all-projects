using System;
using System.Web;
using ECN_Framework_Common.Objects;
using static ECN_Framework_Common.Objects.Enums;

namespace ECN_Framework_BusinessLayer.Application
{
    public class AccountApplicationErrorHandler : GlobalApplicationErrorHandlerBase
    {
        private string CommunicatorPageUrl { get; set; }

        public AccountApplicationErrorHandler(
            HttpServerUtility server,
            HttpRequest request,
            HttpResponse response,
            HttpApplicationState application,
            string applicationId,
            string pageUrl,
            string communicatorPageUrl) : base(server, request, response, application, applicationId, pageUrl)
        {
            CommunicatorPageUrl = communicatorPageUrl;
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
                    CommunicatorPageUrl);
            }
            else if (lastException.Message.Contains("ASP.NET session has expired or could not be found"))
            {
                RedirectToErrorPage(
                    lastException,
                    ErrorMessage.HardError,
                    CommunicatorPageUrl);
            }
            else if (lastException.Message.Contains("A potentially dangerous Request"))
            {
                HandleDangerousRequest(lastException, ErrorMessage.InvalidLink);
            }
            else
            {
                LogCriticalErrorWithUserInfoAndRedirectToErrorPage(
                    lastException,
                    ErrorMessage.HardError,
                    CommunicatorPageUrl);
            }

            return error;
        }

        protected override void HandleSecurityException(Exception lastException)
        {
            var securityException = lastException.InnerException as SecurityException;
            if (securityException == null)
            {
                throw new InvalidOperationException("securityException");
            }

            if (securityException.SecurityType == SecurityExceptionType.RoleAccess)
            {
                Response.Redirect(string.Format("{0}/main/securityAccessError.aspx", PageUrl), true);
            }
            else if (securityException.SecurityType == SecurityExceptionType.FeatureNotEnabled)
            {
                Response.Redirect(string.Format("{0}/main/featureAccessError.aspx", PageUrl), true);
            }
        }
    }
}
