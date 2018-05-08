using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ecn.gateway.Controllers
{
    [Authorize]
    public class ErrorController : Controller
    {
        //
        // GET: /Error/
        [AllowAnonymous]
        public ActionResult Error(string message)
        {
            Session["PageTitle"] = "";
            ViewData["TypeCode"] = "";
            ViewData["PubCode"] = "";

            Session["PubCode"] = "";
            Session["TypeCode"] = "";

            Session["UserName"] = "";
            Session["LinkParameter"] = "";
            string error = "HardError";
            try
            {
                error = message;
            }
            catch { }
            if (error == "InvalidLink")
            {
                ViewData["ErrorMessage"] = ECN_Framework_Common.Objects.ECNException.GetErrorText(ECN_Framework_Common.Objects.Enums.ErrorMessage.InvalidLink);
            }
            else if (error == "PageNotFound")
            {
                ViewData["ErrorMessage"] = ECN_Framework_Common.Objects.ECNException.GetErrorText(ECN_Framework_Common.Objects.Enums.ErrorMessage.PageNotFound);
            }
            else if (error == "Timeout")
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
