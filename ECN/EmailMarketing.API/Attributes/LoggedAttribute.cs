#define DEBUG_ATTRIBUTES

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Http.Filters;
using System.Web.Http;
using System.Web.Http.Controllers;

using ECN_Framework_Entities.Accounts;

using EmailMarketing.API.Controllers;

using System.Diagnostics;

namespace EmailMarketing.API.Attributes
{
    /// <summary>
    /// Implements API logging as pre and post execution actions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class LoggedAttribute : OrderedActionFilterAttribute
    {
        public LoggedAttribute() : base(order:1) { }
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            #if DEBUG_ATTRIBUTES
            Debug.WriteLine("OnActionExecuting -> starting logging filter handler");
            #endif

            AuthenticatedUserControllerBase c = actionContext.ControllerContext.Controller as AuthenticatedUserControllerBase;
            c.LogRequest();
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            #if DEBUG_ATTRIBUTES
            Debug.WriteLine("OnActionExecuted -> reached the post request logging filter handler");
            #endif

            AuthenticatedUserControllerBase c = actionExecutedContext.ActionContext.ControllerContext.Controller as AuthenticatedUserControllerBase;            
            c.UpdateLog();
        }
    }
}