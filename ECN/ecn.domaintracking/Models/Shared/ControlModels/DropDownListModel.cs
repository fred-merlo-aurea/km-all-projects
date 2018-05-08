using System.Collections.Generic;
using System.Web.Mvc;
using ecn.domaintracking.HtmlHelpers;

namespace ecn.domaintracking.Models.Shared.ControlModels
{
    public class DropDownListModel
    {
        public string Name { get; set; }
        public List<SelectListItem> Items { get; set; }
        public DropDownListOptions Options { get; set; }
        public string FirstItemText { get; set; }
        public string FirstItemValue { get; set; }
        public object HtmlAttributes { get; set; }
    }
}