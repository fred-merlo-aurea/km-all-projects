using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UAD.Web.Admin.Controllers.Common
{
    public class ErrorController : BaseController
    {
        // GET: Error
        public ActionResult Error(string errorType = "")
        {
            List<ECN_Framework_Common.Objects.ECNError> errorList = new List<ECN_Framework_Common.Objects.ECNError>();
            if (errorType == "UnAuthorized")
                errorList.Add(new ECN_Framework_Common.Objects.ECNError() { ErrorMessage = "User is not authorized to view this page for current customer " + CurrentClient.DisplayName + ".", Method = ECN_Framework_Common.Objects.Enums.Method.HasPermission, Entity = ECN_Framework_Common.Objects.Enums.Entity.Role });
            return View("Error", errorList);
        }
    }
}