using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Principal;
using System.Web.Security;
using System.Text;

namespace KMPS.MD
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            //Fires upon attempting to authenticate the use
            if (!(HttpContext.Current.User == null))
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity.GetType() == typeof(FormsIdentity))
                    {
                        FormsIdentity fi = (FormsIdentity)HttpContext.Current.User.Identity;
                        FormsAuthenticationTicket fat = fi.Ticket;

                        String[] astrRoles = fat.UserData.Split('|');
                        HttpContext.Current.User = new GenericPrincipal(fi, astrRoles);
                    }
                }
            }
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

                        if (!Request.RawUrl.ToUpper().Contains("DASHBOARD.ASPX"))  //SUPPRESS ALL ERRORS IN DASHBOARD.ASPX
                        {
                            HttpContext.Current.Server.ClearError();
                            HttpContext.Current.Response.Redirect("~/error.aspx");
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}