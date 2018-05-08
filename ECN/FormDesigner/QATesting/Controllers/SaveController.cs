using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ecn.qatools.Infrastructure.ExtentionMethods;

namespace ecn.qatools.Controllers
{
    public class SaveController : Controller
    {
        // GET: Save
        [HttpGet]
        [HttpPost]
        [HttpDelete]
        public ActionResult Index(dynamic postObject)
        {
            return View();
        }

        [HttpGet]
        [HttpPost]
        [HttpDelete]
        public ActionResult Index()
        {
            return View(new { });
        }
    }
}