using System;
using System.Net;
using System.Web;
using KMManagers;
using Newtonsoft.Json;

namespace KMWeb.Handlers
{
    public class SubmitFormHandler : IHttpHandler
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            FormSubmitter submitter = new FormSubmitter();
            string[] res = submitter.Submit(context.Request);
            context.Response.Write(res[0] == null ? JsonConvert.SerializeObject(res) : res[0]);
            context.Response.StatusCode = (int)(res[0] == null ? HttpStatusCode.OK : HttpStatusCode.NotFound);
        }
        #endregion
    }
}