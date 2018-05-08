using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ecn.communicator.mvc.Models;
using ecn.communicator.mvc.Infrastructure;
using ECN_Framework_Common.Objects;
using ECN_Framework_Common.Functions;
using System.Configuration;
using System.Data;
using System.Xml;
using System.IO;
using System.Collections;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using KMSite;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI;
using Microsoft.Reporting.WebForms;
using System.Reflection;
namespace ecn.communicator.mvc.Controllers
{
    public class ErrorController : BaseController
    {
        // GET: Error
        public ActionResult Index(string E = "")
        {
            string response = "";
            if (E == "InvalidLink")
            {
                response = ECN_Framework_Common.Objects.ECNException.GetErrorText(ECN_Framework_Common.Objects.Enums.ErrorMessage.InvalidLink);
            }
            else if (E == "PageNotFound")
            {
                response = ECN_Framework_Common.Objects.ECNException.GetErrorText(ECN_Framework_Common.Objects.Enums.ErrorMessage.PageNotFound);
            }
            else if (E == "Timeout")
            {
                response = ECN_Framework_Common.Objects.ECNException.GetErrorText(ECN_Framework_Common.Objects.Enums.ErrorMessage.Timeout);
            }
            else
            {
                response = ECN_Framework_Common.Objects.ECNException.GetErrorText(ECN_Framework_Common.Objects.Enums.ErrorMessage.HardError);
            }
            return View(model: response);
        }

        public ActionResult Error(string E = "")
        {
            string response = "";
            if (E == "InvalidLink")
            {
                response = ECN_Framework_Common.Objects.ECNException.GetErrorText(ECN_Framework_Common.Objects.Enums.ErrorMessage.InvalidLink);
            }
            else if (E == "PageNotFound")
            {
                response = ECN_Framework_Common.Objects.ECNException.GetErrorText(ECN_Framework_Common.Objects.Enums.ErrorMessage.PageNotFound);
            }
            else if (E == "Timeout")
            {
                response = ECN_Framework_Common.Objects.ECNException.GetErrorText(ECN_Framework_Common.Objects.Enums.ErrorMessage.Timeout);
            }
            else
            {
                response = ECN_Framework_Common.Objects.ECNException.GetErrorText(ECN_Framework_Common.Objects.Enums.ErrorMessage.HardError);
            }
            return View(viewName:"Index", model:response);
        }


        public ActionResult SecurityAccess()
        {
            return View();
        }

        public ActionResult FeatureAccess()
        {
            return View();
        }
    }
}