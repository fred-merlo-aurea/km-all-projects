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
    /// Implements enterprise exception logging  by intercepting uncaught exceptions, recording
    /// these to the enterprise exceptions repository, and associating them with the active API
    /// request log entry.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ExceptionsLoggedAttribute : OrderedExceptionFilterAttribute
    {
        static int? webApiNonServerErrorLevel = null;
        public static int WebApiNonServerErrorLevel 
        {
            get
            {
                if(false == webApiNonServerErrorLevel.HasValue)
                {
                    webApiNonServerErrorLevel = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["WebApiNonServerErrorLevel"]);
                }
                return webApiNonServerErrorLevel ?? 0;
            }
        }
        public static bool LogNonServerErrors
        {
            get
            {
                return 0 < WebApiNonServerErrorLevel;
            }
        }
        public ExceptionsLoggedAttribute() : base(order: 99) { }
        //TODO:  should only be sending to Common log for 500 errors!
        /// <summary>
        /// Overrides the base behavior of an <see cref="System.Web.Http.Filters.ExceptionFilterAttribute"/>
        /// to provide custom application wide logic for processing of exceptions, recording them in the
        /// Enterprise "Common" exceptions repository and creating an association from the record to the
        /// API request log entry related to the active request.
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            #if DEBUG_ATTRIBUTES
            Debug.WriteLine("ExceptionsLoggedAttribute -> starting exception handler");
            #endif

            AuthenticatedUserControllerBase c = actionExecutedContext.ActionContext.ControllerContext.Controller as AuthenticatedUserControllerBase;
            //c.LogError(0, actionExecutedContext.Exception, actionExecutedContext.Response.ReasonPhrase);
            string reason = null != actionExecutedContext.Response 
                          ? actionExecutedContext.Response.ReasonPhrase 
                          : actionExecutedContext.Exception.GetType().FullName;
            // suppress notifications for non-issues
            int severity = 1;
            switch((reason??"").ToLower())
            {
                case "not found":    case "emailmarketing.api.exceptions.apiresourcenotfoundexception":
                case "forbidden":    case "emailmarketing.api.exceptions.apiaccesskeyexception":
                case "unauthorized": case "ecn_framework_common.objects.securityexception":
                case "bad request":  case "ecn_framework_common.objects.ecnexception":
                                     case "system.web.http.httpresponseexception":
                                     case "emailmarketing.api.exceptions.imageexception":
                    if (false == LogNonServerErrors)
                    {
                        return; // web.config: set WebApiNonServerErrorLevel == 0 to disable logging for above exception types/http status messages
                    }
                    severity = WebApiNonServerErrorLevel;
                    break;
                case "system.operationcanceledexception": // Don't log critical exception (or any) on session abort.
                    return;
                    
            }            
            c.LogError(severity, actionExecutedContext.Exception, reason);
        }
    }
}