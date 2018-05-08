using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ecn.activity.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult Error(string error)
        {
            string errorType = "HardError";
            try
            {
                errorType = error;
            }
            catch { }
            if (errorType == "InvalidLink")
            {
                ViewData["ErrorMessage"] = ECN_Framework_Common.Objects.ECNException.GetErrorText(ECN_Framework_Common.Objects.Enums.ErrorMessage.InvalidLink);
            }
            else if (errorType == "PageNotFound")
            {
                ViewData["ErrorMessage"] = ECN_Framework_Common.Objects.ECNException.GetErrorText(ECN_Framework_Common.Objects.Enums.ErrorMessage.PageNotFound);
            }
            else if (errorType == "Timeout")
            {
                ViewData["ErrorMessage"] = ECN_Framework_Common.Objects.ECNException.GetErrorText(ECN_Framework_Common.Objects.Enums.ErrorMessage.Timeout);
            }
            else
            {
                ViewData["ErrorMessage"] = ECN_Framework_Common.Objects.ECNException.GetErrorText(ECN_Framework_Common.Objects.Enums.ErrorMessage.HardError);
            }
            return View();
        }

    }
}
