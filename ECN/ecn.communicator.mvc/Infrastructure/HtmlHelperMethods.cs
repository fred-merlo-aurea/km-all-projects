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
using System.IO;
// Tuple<output text, HtmlAttributes, ActionLinkInfo>
// Tuple<output text, HtmlAttributes, Tuple<action name, controller name, route values>>

namespace ecn.communicator.mvc.Infrastructure
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
            #region LinkAlias
            var Contentlink = new Dictionary<string, object>();
            Contentlink.Add("contentID", "#=ContentID#");
            var LinkAliasAction = new ActionType("Link Alias", null, "LinkAlias", "Contents", Contentlink);
            #endregion
            #region PreviewHTMLAction
            ActionType PreviewHTMLAction = new ActionType("Preview HTML");
            #endregion
            #region PreviewHTMLAction
            ActionType PreviewTEXTAction = new ActionType("Preview TEXT");
            #endregion
            #region EditAction
            var EditAction = new ActionType("Edit", null, "Edit", "Contents", Contentlink);
            #endregion
           
            #region DeleteAction
            var DeleteAction = new ActionType("Delete", new { onclick = "contentID=#=ContentID#;$('\\#deleteModal').modal('show');" });
            #endregion
            #region ArchiveAction
            ActionType ArchiveAction = new ActionType("Archive");
            #endregion
            var Actions = new List<ActionType>();

            #region User Permissions
            if (KM.Platform.User.HasAccess(user, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Content, KMPlatform.Enums.Access.View))
            {
                Actions.Add(LinkAliasAction);
            }
            if (KM.Platform.User.HasAccess(user, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Content, KMPlatform.Enums.Access.View))
            {
                Actions.Add(PreviewHTMLAction);
            }
            if (KM.Platform.User.HasAccess(user, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Content, KMPlatform.Enums.Access.View))
            {
                Actions.Add(PreviewTEXTAction);
            }
            if (KM.Platform.User.HasAccess(user, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Content, KMPlatform.Enums.Access.Edit))
            {
                Actions.Add(EditAction);
            }
            
            if (KM.Platform.User.HasAccess(user, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Content, KMPlatform.Enums.Access.Delete))
            {
                Actions.Add(DeleteAction);
            }
            if (KM.Platform.User.HasAccess(user, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Content, KMPlatform.Enums.Access.Edit))
            {
                Actions.Add(ArchiveAction);
            }
            #endregion
            var menu = ActionMenu.ToKendoGridClientTemplate(html.ActionMenuFactory("#=ContentID#", Actions));
            return menu;

        }

        public static MvcHtmlString ECNErrors(this HtmlHelper html, IEnumerable<ECN_Framework_Common.Objects.ECNError> errors)
        {
            if(errors == null)
                return new MvcHtmlString("");
            return html.Partial("Partials/_ECNErrors", errors);
        }

        public static string RenderViewToString(ControllerContext context, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = context.RouteData.GetRequiredString("action");

            ViewDataDictionary viewData = new ViewDataDictionary(model);

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                ViewContext viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}