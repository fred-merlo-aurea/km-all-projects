using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace UAD.Web.Admin.Infrastructure
{
    public class ActionType
    {
        public ActionType(string text)
        {
            Text = text;
            HtmlAttributes = null;
            Action = null;
        }
        public ActionType(string text, object attrs)
        {
            Text = text;
            HtmlAttributes = attrs;
            Action = null;
        }
        public ActionType(string text, object attrs, string actionName, string controllerName)
        {
            Text = text;
            HtmlAttributes = attrs;
            Action = new ActionInfo(actionName, controllerName);
        }
        public ActionType(string text, object attrs, string actionName, string controllerName, Dictionary<string, object> routevals)
        {
            Text = text;
            HtmlAttributes = attrs;
            Action = new ActionInfo(actionName, controllerName, routevals);
        }
        public string Text { get; set; }
        public object HtmlAttributes { get; set; }
        public ActionInfo Action { get; set; }
        public class ActionInfo
        {
            public ActionInfo(string action, string controller)
            {
                ActionName = action;
                ControllerName = controller;
                RouteValues = null;
            }
            public ActionInfo(string action, string controller, Dictionary<string, object> dict)
            {
                ActionName = action;
                ControllerName = controller;
                RouteValues = dict;
            }
            public string ActionName { get; set; }
            public string ControllerName { get; set; }
            public Dictionary<string, object> RouteValues { get; set; }
        }
    }
    public static class ActionMenu
    {
        public static Kendo.Mvc.UI.Fluent.MenuBuilder ActionMenuFactory(this HtmlHelper html, string idName, IEnumerable<ActionType> actions)
        {
            var template = html.Kendo()
            .Menu()
            .Name("ActionMenu_" + idName) // Must be put in grid as Client Template
            .HtmlAttributes(new { @class = "ActionMenu" })
            .Direction(MenuDirection.Right)
            .Orientation(MenuOrientation.Vertical)
            .Animation(false)
            .Items(items =>
            {
                items.Add().Text("Actions").HtmlAttributes(new { @class = "k-menu-actions" }).Items(
                innerItems =>
                {
                    foreach (ActionType action in actions)
                    {
                        if (action.Action == null)
                            innerItems.Add().Text(action.Text).HtmlAttributes(action.HtmlAttributes);
                        else
                        {
                            if (action.Action.RouteValues == null)
                                innerItems.Add().Text(action.Text).HtmlAttributes(action.HtmlAttributes).Action(action.Action.ActionName, action.Action.ControllerName);
                            else
                                innerItems.Add().Text(action.Text).HtmlAttributes(action.HtmlAttributes).Action(action.Action.ActionName, action.Action.ControllerName, new RouteValueDictionary(action.Action.RouteValues));
                        }
                    }
                });
            }
            );
            return template;
        }
        public static string ToKendoGridClientTemplate(Kendo.Mvc.UI.Fluent.MenuBuilder actionMenu)
        {
            return actionMenu.ToClientTemplate().ToString();
        }
    }
}