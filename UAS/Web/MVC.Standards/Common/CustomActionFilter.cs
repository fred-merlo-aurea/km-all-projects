﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Standards { 
    public class CustomActionFilter: ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            // TODO: Add your action filter's tasks here

          

        this.OnActionExecuting(filterContext);
    }
}
}