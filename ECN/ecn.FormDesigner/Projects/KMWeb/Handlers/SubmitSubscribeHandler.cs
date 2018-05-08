using System;
using System.Net;
using System.Web;
using KMManagers;

namespace KMWeb.Handlers
{
    public class SubmitSubscribeHandler : IHttpHandler
    {
        private const string HToken = "htoken";

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            FormSubmitter submitter = new FormSubmitter();
            string html = null;
            if (submitter.Confirm(Guid.Parse(context.Request[HToken]), out html))
            {
                context.Response.Write(html);
                context.Response.StatusCode = (int)HttpStatusCode.OK;
            }
            else
            {
                context.Response.Write(html);
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
        }
    }
}