using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UAD.Web.Admin.Controllers
{
    public class ProductSortController : Controller
    {
        // GET: ProductSort
        public ActionResult Index()
        {
            ViewBag.Attendees = new List<string>
            {
            };

            return View();
        }
    }
}