using System;
using System.Web;
using ECN_Framework_Common.Objects;
using static ECN_Framework_Common.Objects.Enums;

namespace ECN_Framework_BusinessLayer.Application
{
    public class CreatorApplicationErrorHandler : GlobalApplicationErrorHandlerBase
    {
        private string AccountPageUrl { get; set; }

        public CreatorApplicationErrorHandler(
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

        protected override void HandleSecurityException(Exception securityException)
        {
            Response.Redirect(
                        string.Format("{0}/main/securityAccessError.aspx", AccountPageUrl),
                        true);
        }
    }
}
