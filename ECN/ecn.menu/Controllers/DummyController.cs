using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ecn.menu.Controllers
{
    public class DummyController : Controller
    {
        //
        // GET: /Dummy/

        public ActionResult PartialRender()
        {
            return PartialView();
        }

    }
}
