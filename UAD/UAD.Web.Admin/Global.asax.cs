using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Text;
using System.Configuration;

namespace UAD.Web.Admin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.Url.Host.ToLower() != "localhost")
            {
                Exception err = Server.GetLastError();

                StringBuilder sbEx = new StringBuilder();
                try
                {
                    sbEx.AppendLine("<BR><b>Page URL: </b>" + HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + Request.RawUrl.ToString() + "</br>");
                    sbEx.AppendLine("<b>Exception Message:</b>" + err.Message + "</br>");
                    sbEx.AppendLine("<b>Exception Source:</b>" + err.Source + "</br>");
                    sbEx.AppendLine("<b>Stack Trace:</b>" + err.StackTrace + "</br>");
                    sbEx.AppendLine("<b>Inner Exception:</b>" + err.InnerException + "</br>");

                    if (!err.Message.Contains("ASP.NET session has expired or could not be found"))
                    {
                        KM.Common.Entity.ApplicationLog.LogCriticalError(err, "UAD application error", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), sbEx.ToString());
                        Response.Redirect(String.Format("~/Error/{0}/?message={1}", "Error", err.Message));
                    }
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
