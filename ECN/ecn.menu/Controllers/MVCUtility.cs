using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using System.Text;
using System.Web.WebPages;
using System.Web.WebPages.Html;

namespace ecn.menu.Controllers
{
    public class MVCUtility
    {
        private static void RenderPartial(string partialViewName, object model)
        {
            HttpContextBase httpContextBase = new HttpContextWrapper(HttpContext.Current);
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Index");
            
            ControllerContext controllerContext = new ControllerContext(new RequestContext(httpContextBase, routeData), new ecn.menu.Controllers.DummyController());
            IView view = FindPartialView(controllerContext, partialViewName);
            ViewContext viewContext = new ViewContext(controllerContext, view, new ViewDataDictionary { Model = model }, new TempDataDictionary(), httpContextBase.Response.Output);
            view.Render(viewContext, httpContextBase.Response.Output);
        }

        private static IView FindPartialView(ControllerContext controllerContext, string partialViewName)
        {
            ViewEngineResult result = ViewEngines.Engines.FindPartialView(controllerContext, partialViewName);
            if (result.View != null)
            {
                return result.View;
            }
            StringBuilder locationsText = new StringBuilder();
            foreach (string location in result.SearchedLocations)
            {
                locationsText.AppendLine();
                locationsText.Append(location);
            }
            throw new InvalidOperationException(String.Format("Partial view {0} not found. Locations Searched: {1}", partialViewName, locationsText));
        }
        //Here the method that will be called from MasterPage or Aspx
        public static void RenderAction(string controllerName, string actionName, object routeValues)
        {
            RenderPartial("~/Views/Shared/PartialRender.cshtml", new RenderActionViewModel() { ControllerName = controllerName, ActionName = actionName, RouteValues = routeValues });
        }
    }

    public class RenderActionViewModel
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public object RouteValues { get; set; }
    }
}