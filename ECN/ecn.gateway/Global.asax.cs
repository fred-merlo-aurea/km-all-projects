using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ECN_Framework_BusinessLayer.Application;

namespace ecn.gateway
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private const string KMCommonApplicationKey = "KMCommon_Application";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }

        protected void Application_Error()
        {
            var applicationId = ConfigurationManager.AppSettings[KMCommonApplicationKey];

            var errorHandler = new GatewayApplicationErrorHandler(
                Server,
                Request,
                Response,
                Application,
                applicationId,
                string.Empty);

            errorHandler.HandleApplicationError();
        }
    }
}