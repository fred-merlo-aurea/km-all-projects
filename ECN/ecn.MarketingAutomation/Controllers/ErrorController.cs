using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ecn.MarketingAutomation.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index(ECN_Framework_Common.Objects.Enums.ErrorMessage type)
        {
            return View(type);
        }
    }
}