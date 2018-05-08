using System;
using System.Web;
using static ECN_Framework_Common.Objects.Enums;

namespace ECN_Framework_BusinessLayer.Application
{
    public class DigitalEditionApplicationErrorHandler : GlobalApplicationErrorHandlerBase
    {
        private string CommunicatorPageUrl { get; set; }

        public DigitalEditionApplicationErrorHandler(
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

        protected override void HandleSecurityException(Exception securityException)
        {
            NavigateToErrorPage(ErrorMessage.SecurityError, PageUrl, AspxErrorPage);
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
            else
            {
                LogCriticalErrorWithUserInfoAndRedirectToErrorPage(
                    lastException,
                    ErrorMessage.HardError,
                    CommunicatorPageUrl);
            }

            return ErrorMessage.HardError;
        }

        protected override string AppendCurrentUserInfoIfExists()
        {
            return string.Empty;
        }
    }
}
