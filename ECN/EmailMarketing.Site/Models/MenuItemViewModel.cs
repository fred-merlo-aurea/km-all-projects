using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailMarketing.Site.Models
{
    public class MenuItemViewModel
    {
        public string Label { get; set; }

        private string href;
        public string Href
        {
            get
            {
                return href.StartsWith("~") ? VirtualPathUtility.ToAbsolute(href) : href;
            }
            set
            {
                href = value;
            }
        }

        public List<MenuItemViewModel> SubMenu { get; set; }
    }
}