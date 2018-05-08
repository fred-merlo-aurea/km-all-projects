using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Standards
{
    public class JavaScriptRedirectResult : ActionResult
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public object RouteValues { get; set; }

        public JavaScriptRedirectResult(string actionName)
        {
            ActionName = actionName;
        }

        public JavaScriptRedirectResult(string actionName, object routeValues)
        {
            ActionName = actionName;
            RouteValues = routeValues;
        }

        public JavaScriptRedirectResult(string actionName, string controllerName)
        {
            ActionName = actionName;
            ControllerName = controllerName;
        }

        public JavaScriptRedirectResult(string actionName, string controllerName, object routeValues)
        {
            ActionName = actionName;
            ControllerName = controllerName;
            RouteValues = routeValues;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var url = new UrlHelper(context.RequestContext);
            var result = new JavaScriptResult
            {
                Script = string.Format("location.href = '{0}'", url.Action(ActionName, ControllerName, RouteValues))
            };
            result.ExecuteResult(context);
        }
    }
}
