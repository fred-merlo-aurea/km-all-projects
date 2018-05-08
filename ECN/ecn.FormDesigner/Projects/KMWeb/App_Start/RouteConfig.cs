using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;

namespace KMWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("GetForm/{*pathInfo}");
            routes.IgnoreRoute("SubmitForm/{*pathInfo}");
            routes.IgnoreRoute("Confirm/{*pathInfo}");
            routes.IgnoreRoute("PrepopulateFromDb/{*pathInfo}");
            routes.IgnoreRoute("AutoSubmitForm/{*pathInfo}");

            routes.MapRoute(
                name: "Error",
                url: "Error/{type}",
                defaults: new { controller = "Error", action = "Index", type = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Forms", action = "Index", id = UrlParameter.Optional }
            );            
        }
    }
}