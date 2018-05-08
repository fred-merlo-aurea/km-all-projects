using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ECN_Framework_BusinessLayer.Application;

namespace ecn.communicator.mvc
{
    public class MvcApplication : HttpApplication
    {
        private const string KMCommonApplicationKey = "KMCommon_Application";
        private const string CommunicatorVirtualPathKey = "Communicator_VirtualPath";
        private const string AccountsVirtualPathKey = "Accounts_VirtualPath";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error()
        {
            var applicationId = ConfigurationManager.AppSettings[KMCommonApplicationKey];
            var pageUrl = ConfigurationManager.AppSettings[CommunicatorVirtualPathKey];
            var accountsPageUrl = ConfigurationManager.AppSettings[AccountsVirtualPathKey];

            var errorHandler = new CommunicatorMvcApplicationErrorHandler(
                Server,
                Request,
                Response,
                Application,
                applicationId,
                pageUrl,
                accountsPageUrl);

            errorHandler.HandleApplicationError();
        }
    }
}
