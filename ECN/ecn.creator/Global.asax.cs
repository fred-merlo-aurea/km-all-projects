using System;
using System.Configuration;
using System.Web;
using ECN_Framework_BusinessLayer.Application;

namespace ecn.creator
{
    public class Global : HttpApplication
    {
        private const string KMCommonApplicationKey = "KMCommon_Application";
        private const string CreatorVirtualPathKey = "Creator_VirtualPath";
        private const string AccountsVirtualPathKey = "Accounts_VirtualPath";

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var applicationId = ConfigurationManager.AppSettings[KMCommonApplicationKey];
            var pageUrl = ConfigurationManager.AppSettings[CreatorVirtualPathKey];
            var accountPageUrl = ConfigurationManager.AppSettings[AccountsVirtualPathKey];

            var errorHandler = new CreatorApplicationErrorHandler(
                Server,
                Request,
                Response,
                Application,
                applicationId,
                pageUrl,
                accountPageUrl);

            errorHandler.HandleApplicationError();
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}