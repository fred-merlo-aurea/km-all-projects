using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UAS.Web.Controllers.Common;

namespace UAS.Web.Controllers
{
    public class ErrorController : BaseController
    {
        // GET: Error
        public ActionResult Error(string errorType ="")
        {
            string errorMessage = "";
            List<ECN_Framework_Common.Objects.ECNError> errorList = new List<ECN_Framework_Common.Objects.ECNError>();
            switch (errorType)
            {
                 case "RecordInActiv":
                        errorMessage = "Current record is inactive. Please provide different Sequence#.";
                        break;
                case "ProductEmpty":
                        errorMessage = "Please navigate from valid url link.";
                        break;
                case "AddRemoveNotAllowed":
                    errorMessage = "Add Remove is not allowed for this product. Please close Data Entry Internal Import	and External Import	to open Add Remove.";
                    break;
                case "DataEntryNotAllowed":
                     errorMessage = "Data entry is not allowed for this product. Please unlock DataEntry to perform the action.";
                     break;
                case "IssueOpenClosedLocked":
                    errorMessage = "Issue is locked by someone else.";
                    break;
                case "CloseOther":
                    errorMessage = "Please close Data Entry,Internal Import, External Import and Add Remove  .";
                    break;
                case "FileNotFound":
                    errorMessage = "A file you are trying to download is not available. Please try again.";
                    break;
                default :
                        errorMessage = "User is not authorized to view this page for current customer " + CurrentClient.DisplayName + ".";
                        break;
            }
            errorList.Add(new ECN_Framework_Common.Objects.ECNError() { ErrorMessage = errorMessage, Method = ECN_Framework_Common.Objects.Enums.Method.HasPermission, Entity = ECN_Framework_Common.Objects.Enums.Entity.Role });
            return View("Error",errorList);
        }
    }
}