using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ecn.activity
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Empty",
                url: "",
                defaults: new { controller = "Error", action = "Error", error = "InvalidLink" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{*id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Error",
                url: "Error/Error/{error}",
                defaults: new { controller = "Error", action = "Error", id = UrlParameter.Optional }
                );
        }
    }
}