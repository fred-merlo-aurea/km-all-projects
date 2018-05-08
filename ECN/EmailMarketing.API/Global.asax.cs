using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace EmailMarketing.API
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

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

            // custom XML formatter
            /*GlobalConfiguration.Configuration.Formatters.Add(
                new Formatters.CustomXmlFormatter()); // in App_Start folder
            GlobalConfiguration.Configuration.Formatters
                .Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
            */
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception err = Server.GetLastError();
            ECN_Framework_Common.Objects.Enums.ErrorMessage error = ECN_Framework_Common.Objects.Enums.ErrorMessage.HardError;

            if (err is System.Web.HttpException)
            {
                #region HttpException
                StringBuilder userInfo = new StringBuilder();
                try
                {
                    HttpException httpError = err as HttpException;
                    if (httpError != null)
                    {
                        if (httpError.GetHttpCode() == 404)
                        {
                            error = ECN_Framework_Common.Objects.Enums.ErrorMessage.PageNotFound;
                        }

                    }
                #endregion HttpException
                   
                    //Application["err"] = httpError;
                    //Server.Transfer("Error.aspx?E=" + error.ToString(), false);
                }
                catch (Exception)
                { }

            }
        }
    }
}