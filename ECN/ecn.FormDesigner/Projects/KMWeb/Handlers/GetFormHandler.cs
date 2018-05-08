using System;
using System.Net;
using System.Web;
using KMManagers;

namespace KMWeb.Handlers
{
    public class GetFormHandler : IHttpHandler
    {
        private const string TokenUID = "tokenuid";
        private const string lang = "lang";
        private const string child = "child";

        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request[TokenUID] == null)
            {
                context.Response.Write("null");
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            else
            {
                HTMLGenerator gen = new HTMLGenerator(context.Request[lang]);
                context.Response.Write(gen.GenerateHTML(context.Request[TokenUID], context.Request[child], context.Request.IsSecureConnection));
                context.Response.StatusCode = (int)HttpStatusCode.OK;
            }
        }

        #endregion
    }
}