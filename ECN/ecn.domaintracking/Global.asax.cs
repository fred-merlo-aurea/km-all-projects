using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ECN_Framework_BusinessLayer.Application;

namespace ecn.domaintracking
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private const string KMCommonApplicationKey = "KMCommon_Application";
        private const string AccountsVirtualPathKey = "Accounts_VirtualPath";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Application_Error(object sender, EventArgs e)
        {
            var applicationId = ConfigurationManager.AppSettings[KMCommonApplicationKey];
            var pageUrl = ConfigurationManager.AppSettings[AccountsVirtualPathKey];

            var errorHandler = new DomainTrackingApplicationErrorHandler(
                Server,
                Request,
                Response,
                Application,
                applicationId,
                pageUrl);

            errorHandler.HandleApplicationError();
        }
    }
}
