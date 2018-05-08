using System;
using System.Linq;
using System.Web.Mvc;

namespace AMSServicesDocumentation.Controllers
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
