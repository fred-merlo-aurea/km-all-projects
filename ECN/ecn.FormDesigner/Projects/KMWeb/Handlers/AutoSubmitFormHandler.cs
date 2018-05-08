using System;
using System.Net;
using System.Web;
using KMManagers;
using System.Collections.Generic;

namespace KMWeb.Handlers
{
    public class AutoSubmitFormHandler : IHttpHandler
    {
        private const string TokenUID = "tokenuid";
        private const string lang = "lang";
        private const string child = "child";

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
                FormSubmitter submitter = new FormSubmitter();
                Dictionary<string, string> autosub = submitter.AutoSubmit(context.Request);
                //context.Response.Write(autosub["message"]);
                context.Response.StatusCode = Convert.ToInt32(autosub["status"]);
            }
        }
    }
}