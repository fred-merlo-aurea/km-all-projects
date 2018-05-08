using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ecn.qatools.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{id}",
                defaults: new
                {
                    controller = "DefaultApi",
                    id = RouteParameter.Optional
                }
            );
        }
    }
}