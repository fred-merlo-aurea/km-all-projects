using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UAD.DataCompare.Web.Controllers.Common
{
    public class CustomActionFilter: ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            // TODO: Add your acction filter's tasks here

          

        this.OnActionExecuting(filterContext);
    }
}
}