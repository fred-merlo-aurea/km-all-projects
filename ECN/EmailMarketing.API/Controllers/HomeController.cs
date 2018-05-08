using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmailMarketing.API.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StatusCodes()
        {
            return View();
        }

        public ActionResult SearchOptions()
        {
            return View();
        }
    }
}
