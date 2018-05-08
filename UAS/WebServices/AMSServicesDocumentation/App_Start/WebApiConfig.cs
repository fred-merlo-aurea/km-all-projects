using System;
using System.Linq;
using System.Web.Http;

namespace AMSServicesDocumentation
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // CORS
            config.EnableCors();

            // serialization customization: render enumerations as strings (rather then numeric codes)
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(
                new Newtonsoft.Json.Converters.StringEnumConverter());
            
            config.Formatters.XmlFormatter.UseXmlSerializer = true;
            var xml = GlobalConfiguration.Configuration.Formatters.XmlFormatter;

            // special routing for "Search"
            // this enables us to implement search
            // in a special GET handler within each
            // controller vs. having a controller called
            // "SearchController" with expert knowledge
            // of each and every search-able subject area.
            /* 
             * NOTE:  this caused issues with routing and auto-generated docs.
             *        Essentially, it was exposing Search as a separate GET 
             *        route for path /api/{controller}, which would always 500
             *        
             * config.Routes.MapHttpRoute(
                name: "SearchApi",
                routeTemplate: "api/search/{controller}",
                defaults: new { action = "Search" },
            );
             */

            //config.Routes.IgnoreRoute("SearchIgnoreRoute", "api/search/{*pathInfo}");

            /*config.Routes.MapHttpRoute(
                name: "SearchApi",
                routeTemplate: "api/search/{subject}",
                defaults: new { subject= RouteParameter.Optional, controler="search", action="{subject}"}
                );
             */

            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name:          "SearchApi",
            //    routeTemplate: "api/{controller}/{action}",
            //    defaults:      null,
            //    constraints:   new { controller = "search" }
            //);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            /*config.Filters.Add(new Attributes.UserAuthTokenRequiredAttribute()
            {
                Order = 0
            });
            config.Filters.Add(new Attributes.CustomerIdRequiredAttribute()
            {
                Order = 1
            });
            config.Filters.Add(new Attributes.LoggedAttribute()
            {
                Order = 2
            });
            config.Filters.Add(new Attributes.ExceptionsLoggedAttribute()
            {
                Order = 3
            });*/
            /*config.Filters.Add(new Attributes.FriendlyExceptionsAttribute()
            {
                Order = 4,
                ExceptionHandlers = new List<Exceptions.ExceptionConfiguration>(AMSServicesDocumentation.Exceptions.ExceptionConfigurationLibrary.GetExceptionHandlers())
            });*/

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();
        }
    }
}
