using System;
using System.Configuration;
using System.Web;
using ECN_Framework_BusinessLayer.Application;

namespace ecn.publisher
{
    public class Global : HttpApplication
    {
        private const string KMCommonApplicationKey = "KMCommon_Application";
        private const string CommunicatorVirtualPathKey = "Communicator_VirtualPath";
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
            var communicatorPageUrl = ConfigurationManager.AppSettings[CommunicatorVirtualPathKey];
            var accountsPageUrl = ConfigurationManager.AppSettings[AccountsVirtualPathKey];

            var errorHandler = new PublisherApplicationErrorHandler(
                                    Server,
                                    Request,
                                    Response,
                                    Application,
                                    applicationId,
                                    communicatorPageUrl,
                                    accountsPageUrl);

            errorHandler.HandleApplicationError();
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }

    public class Enums
    {
        public enum ErrorMessage
        {
            HardError,
            InvalidLink,
            PageNotFound,
            Timeout,
            Unknown
        }
    }
}