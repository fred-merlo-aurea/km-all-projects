using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AMSServicesDocumentation
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // Web Api v1
            //WebApiConfig.Register(GlobalConfiguration.Configuration);

            // Web Api v2
            GlobalConfiguration.Configure(WebApiConfig.Register);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            // remove the default filter provider
            var providers = GlobalConfiguration.Configuration.Services.GetFilterProviders();
            var defaultProvider = providers.First(i => i is ActionDescriptorFilterProvider);
            GlobalConfiguration.Configuration.Services.Remove(typeof(System.Web.Http.Filters.IFilterProvider), defaultProvider);

            // and install our own, which allows specification of the order in which they will be triggered
            GlobalConfiguration.Configuration.Services.Add(typeof(System.Web.Http.Filters.IFilterProvider), new Attributes.OrderedFilterProvider());


            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
