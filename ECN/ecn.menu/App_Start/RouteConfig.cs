using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ecn.menu
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "ClientChange",
                url: "{controller}/ClientChange",
                defaults: new { controller = "Index", action = "ClientChange" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{*query}",
                defaults: new { controller = "Index", action = "Index", query = UrlParameter.Optional }
            );

            routes.MapRoute(
                 "Client", // Route name
                 "{controller}/{action}/{id}", // URL with parameters
                 new { controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                 new string[] { "ecn.menu.Controllers" }
            );

            routes.MapRoute(
                name: "Index",
                url: "Index/Index/query",
                defaults: new { controller = "Index", action = "Index", query = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Dummy",
                url: "Dummy",
                defaults: new { controller = "Dummy", action = "PartialRender", query = UrlParameter.Optional }
                );
        }
    }
}