using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace UAS.Web.Controllers.Common
{
    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var httpContext = filterContext.HttpContext;
            if (httpContext.User.Identity.IsAuthenticated)
            {
              
                    filterContext.Result = new RedirectResult("/ecn.accounts/main/");
            }
            else
            {
                filterContext.Result = new RedirectResult("/EmailMarketing.Site/Login");
            }
        }
       
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                
                KMPlatform.Entity.User CurrentUser= ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;
                return true;
                //if (CurrentUser.IsKMStaff)
                //{
                //   return true;
                //}
                //else
                //{
                //    return false;
                //}

            }else
            {
                return false;
            }
        }
    }
}
