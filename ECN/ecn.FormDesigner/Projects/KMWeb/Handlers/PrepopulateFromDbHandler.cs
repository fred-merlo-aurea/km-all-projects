using System;
using System.Net;
using System.Web;
using KMManagers;

namespace KMWeb.Handlers
{
    public class PrepopulateFromDbHandler : IHttpHandler
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            Prepopulator prepopulator = new Prepopulator();
            context.Response.Write(prepopulator.Get(context.Request));
            context.Response.StatusCode = (int)HttpStatusCode.OK;
        }

        #endregion
    }
}