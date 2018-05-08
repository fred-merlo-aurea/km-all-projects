using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ecn.MarketingAutomation
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Error",
                "Error/{type}",
                new { controller = "Error", action = "Index", type = UrlParameter.Optional },
                new string[] { "ecn.MarketingAutomation.Controllers" }

           );

            routes.MapRoute(
                 "Default", // Route name
                 "{controller}/{action}/{id}", // URL with parameters
                 new { controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                 new string[] { "ecn.MarketingAutomation.Controllers" }
            );
        }
    }
}
