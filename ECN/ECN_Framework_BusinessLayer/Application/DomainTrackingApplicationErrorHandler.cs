using System;
using System.Web;

namespace ECN_Framework_BusinessLayer.Application
{
    public class DomainTrackingApplicationErrorHandler : GlobalApplicationErrorHandlerBase
    {
        public DomainTrackingApplicationErrorHandler(
            HttpServerUtility server, 
            HttpRequest request, 
            HttpResponse response, 
            HttpApplicationState application, 
            string applicationId, 
            string pageUrl) : base(server, request, response, application, applicationId, pageUrl)
        {
        }

        protected override void HandleSecurityException(Exception securityException)
        {
            Response.Redirect(
                        string.Format("{0}/main/securityAccessError.aspx", PageUrl),
                        true);
        }
    }
}
