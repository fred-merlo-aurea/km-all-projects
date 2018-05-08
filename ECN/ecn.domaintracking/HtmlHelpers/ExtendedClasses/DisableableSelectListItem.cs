using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ecn.domaintracking.HtmlHelpers.ExtendedClasses
{
    public class DisableableSelectListItem : SelectListItem
    {
        public bool Disabled { get; set; }
    }
}