

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;

namespace UAD.DataCompare.Web.Controllers.Common
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var context = filterContext.HttpContext;

                var loginUrl = string.Format("{0}?ReturnUrl={1}", FormsAuthentication.LoginUrl, context.Request.UrlReferrer.AbsolutePath);

                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.AddHeader("REDIRECT", loginUrl);

                context.Response.End();
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}
