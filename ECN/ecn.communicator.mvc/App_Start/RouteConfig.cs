using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ecn.communicator.mvc
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            //routes.MapRoute(
            //     "Menu", // Route name
            //     "Menu/Index", // URL with parameters
            //     new { controller = "Index", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
            //     new string[] { "ecn.menu.Controllers" }
            //);
        }
    }
}
