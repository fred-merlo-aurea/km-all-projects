using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace UAD.Web.Admin.Infrastructure
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

        public static string ActionMenu_Adhoc(this HtmlHelper html)
        {
            var idValue = "#=SubscriptionsExtensionMapperID#";
            var controllerName = "Adhoc";
            var onClickValue = "Id=#=SubscriptionsExtensionMapperID#;$('\\#deleteModal').modal('show');";

            return CreateActionMenu(html, idValue, controllerName, onClickValue);
        }

        public static string ActionMenu_PubCustomField(this HtmlHelper html)
        {
            var idValue = "#=PubSubscriptionsExtensionMapperId#";
            var controllerName = "PubCustomField";
            var onClickValue = "Id=#=PubSubscriptionsExtensionMapperId#,PubID=#=PubID#;$('\\#deleteModal').modal('show');";

            return CreateActionMenu(html, idValue, controllerName, onClickValue);
        }

        public static string ActionMenu_ResponseGroup(this HtmlHelper html)
        {
            var idValue = "#=ResponseGroupID#";
            var controllerName = "ResponseGroup";
            var onClickValue = "Id=#=ResponseGroupID#,PubID=#=PubID#;$('\\#deleteModal').modal('show');";

            return CreateActionMenu(html, idValue, controllerName, onClickValue);
        }

        public static string ActionMenu_ProductType(this HtmlHelper html)
        {
            var idValue = "#=PubTypeID#";
            var controllerName = "ProductType";
            var onClickValue = "Id=#=PubTypeID#;$('\\#deleteModal').modal('show');";

            return CreateActionMenu(html, idValue, controllerName, onClickValue);
        }

        public static string ActionMenu_MasterGroup(this HtmlHelper html)
        {
            var idValue = "#=MasterGroupID#";
            var controllerName = "MasterGroup";
            var onClickValue = "Id=#=MasterGroupID#;$('\\#deleteModal').modal('show');";

            return CreateActionMenu(html, idValue, controllerName, onClickValue);
        }

        public static string ActionMenu_Product(this HtmlHelper html)
        {
            var idValue = "#=PubID#";
            var controllerName = "Product";
            var onClickValue = "Id=#=PubID#;$('\\#deleteModal').modal('show');";

            return CreateActionMenu(html, idValue, controllerName, onClickValue);
        }

        public static MvcHtmlString UADErrors(this HtmlHelper html, IEnumerable<FrameworkUAD.Object.UADError> errors)
        {
            if (errors == null)
                return new MvcHtmlString("");
            return html.Partial("Partials/_UADErrors", errors);
        }

        private static string CreateActionMenu(HtmlHelper html, string idValue, string controllerName, string onClickValue)
        {
            var editdict = new Dictionary<string, object>();
            editdict.Add("Id", idValue);

            var editAction = new ActionType("Edit", null, "Edit", controllerName, editdict);
            var deleteAction = new ActionType("Delete", new { onclick = onClickValue });

            var actions = new List<ActionType>();
            actions.Add(editAction);
            actions.Add(deleteAction);

            return ActionMenu.ToKendoGridClientTemplate(
                html.ActionMenuFactory(idValue,
                actions));
        }
    }
}