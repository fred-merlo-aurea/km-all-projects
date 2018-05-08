using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Reflection;
using System.Web.Mvc.Html;
using Kendo.Mvc.UI;
using System.Web.Routing;

using ActionType = System.Tuple<string, object, System.Tuple<string, string, System.Collections.Generic.Dictionary<string, object>>>;

namespace UAD.DataCompare.Web.Infrastructure
{
    public static class HtmlHelperMethods
    {
        public static MvcHtmlString ActionImage(this HtmlHelper html, string actionName, string controllerName, object routeValues, string imagePath, string altText, string imageStyle = "", string linkStyle = "", string imageClass = "", string linkClass = "")
        {
            UrlHelper url = new UrlHelper(html.ViewContext.RequestContext);
            TagBuilder img = new TagBuilder("img");
            img.MergeAttribute("src", url.Content(imagePath));
            img.MergeAttribute("alt", altText);
            img.MergeAttribute("style", imageStyle);
            img.MergeAttribute("class", imageClass);
            TagBuilder a = new TagBuilder("a");
            a.MergeAttribute("href", url.Action(actionName, controllerName, routeValues));
            a.MergeAttribute("style", linkStyle);
            a.MergeAttribute("class", linkClass);
            a.InnerHtml = img.ToString(TagRenderMode.SelfClosing);
            return new MvcHtmlString(a.ToString());
        }
        
        public static string ActionMenu_Content(this HtmlHelper html, KMPlatform.Entity.User user = null)
        {
            if (user == null)
                user = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;
            
            var Filelink = new Dictionary<string, object>();
            Filelink.Add("PubSubscriptionID", "#=PubSubscriptionID#");
            Filelink.Add("PubID", "#=PubID#");

            #region EditAction
            var EditProfile = new ActionType("Edit Profile", null, "Profile", "Search", Filelink);
            #endregion

            #region Edit Record
            var EditFullRecord = new ActionType("Edit Record", null, "Subscribe", "Search", Filelink);
            #endregion
           
            var Actions = new List<ActionType>();

            #region User Permissions
            Actions.Add(EditProfile);
            Actions.Add(EditFullRecord);
            //if (KM.Platform.User.HasAccess(user, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.Content, KMPlatform.Enums.Access.Edit))
            //{
            //    Actions.Add(EditProfile);
            //}

            //if (KM.Platform.User.HasAccess(user, KMPlatform.Enums.Services.UAD, KMPlatform.Enums.ServiceFeatures.Content, KMPlatform.Enums.Access.Edit))
            //{
            //    Actions.Add(EditFullRecord);
            //}
            #endregion
            var menu = ActionMenu.ToKendoGridClientTemplate(html.ActionMenuFactory("#=PubSubscriptionID#", Actions));
            return menu;

        }

        public static MvcHtmlString ECNErrors(this HtmlHelper html, IEnumerable<ECN_Framework_Common.Objects.ECNError> errors)
        {
            if (errors == null)
                return new MvcHtmlString("");
            return html.Partial("Partials/_ECNErrors", errors);
        }


        [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
        public class MultipleButtonAttribute : ActionNameSelectorAttribute
        {
            public string Name { get; set; }
            public string Argument { get; set; }

            public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
            {
                var isValidName = false;
                var keyValue = string.Format("{0}:{1}", Name, Argument);
                var value = controllerContext.Controller.ValueProvider.GetValue(keyValue);

                if (value != null)
                {
                    controllerContext.Controller.ControllerContext.RouteData.Values[Name] = Argument;
                    isValidName = true;
                }

                return isValidName;
            }
        }
    }
}