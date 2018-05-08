using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ecn.domaintracking.HtmlHelpers.ExtendedClasses;

namespace ecn.domaintracking.HtmlHelpers
{
    //public class DropDownListEnhanced
    //{
    //}


    public static class DropDownListHelper
    {
        public static MvcHtmlString DropDownListEnhanced(this HtmlHelper html, IEnumerable<SelectListItem> list, string name,
            object htmlAttributes, DropDownListOptions options = DropDownListOptions.None, string firstItemText = null, string firstItemValue = "-1")
        {
            if (list == null)
                return null;

            string fullName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

            TagBuilder tagBuilder = new TagBuilder("select");

            tagBuilder.GenerateId(name);
            tagBuilder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), true);
            tagBuilder.MergeAttribute("name", fullName, true);

            ModelState modelState;
            if (html.ViewData.ModelState.TryGetValue(fullName, out modelState) && modelState.Errors.Count > 0)
            {
                tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
            }

            StringBuilder sb = new StringBuilder();

            switch (options)
            {
                case DropDownListOptions.None:
                    break;
                case DropDownListOptions.FirstEntryBlank:
                    sb.Append("<option value=\"\"></option>");
                    break;
                case DropDownListOptions.FirstEntryAll:
                    if (firstItemText == null)
                        sb.Append("<option value=\"" + firstItemValue + "\">-All-</option>");
                    else
                        sb.Append("<option value=\"" + firstItemValue + "\">" + firstItemText + "</option>");
                    break;
                case DropDownListOptions.FirstEntrySelect:
                    if (firstItemText == null)
                        sb.Append("<option value=\"" + firstItemValue + "\">Select...</option>");
                    else
                        sb.Append("<option value=\"" + firstItemValue + "\">" + firstItemText + "</option>");
                    break;
            }

            foreach (SelectListItem item in list)
            {
                var listItem = item as DisableableSelectListItem;
                if (listItem != null)
                {
                    sb.AppendFormat("<option value=\"{0}\"{1} {2}>{3}</option>",
                        listItem.Value,
                        (listItem.Selected ? " selected" : string.Empty),
                        !listItem.Disabled ? "disabled='disabled'" : string.Empty,
                        listItem.Text);
                }
                else
                {
                    sb.AppendFormat("<option value=\"{0}\"{1}>{2}</option>", item.Value,
                        (item.Selected ? " selected" : string.Empty), item.Text);
                }
            }

            tagBuilder.InnerHtml = sb.ToString();

            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }
    }
}