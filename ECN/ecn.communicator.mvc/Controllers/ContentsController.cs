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


namespace ecn.communicator.mvc.Controllers
{
    public class ContentsController : Controller
    {
        // GET: Content
        [HttpGet]
        public ActionResult Index()
        {
            var user = ConvenienceMethods.GetCurrentUser();

            if (!KM.Platform.User.HasAccess(user, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.Content, KMPlatform.Enums.Access.View))
            {
                throw new SecurityException() { SecurityType = Enums.SecurityExceptionType.FeatureNotEnabled };
            }

            List<ECN_Framework_Entities.Communicator.Folder> folderList =
                ECN_Framework_BusinessLayer.Communicator.Folder.GetByType(user.CustomerID, ECN_Framework_Common.Objects.Communicator.Enums.FolderTypes.CNT.ToString(), user);

            var sortedFolderList = (from src in folderList
                                    orderby src.FolderName
                                    select src).ToList();

            IEnumerable<Kendo.Mvc.UI.TreeViewItemModel> folderTree = sortedFolderList.ToKendoTree();

            FolderExplorer Model = new FolderExplorer(folderTree);

            var Errors = this.GetTempData("ECNErrors") as List<ECNError>;
            if (Errors != null)
            {
                Model.Errors = Errors;
            }
            return View(Model);
        }

        [HttpPost]
        public ActionResult Search(string contentName, string archiveFilter, bool allFolders, int folderID, int pageSize, int currentPage, string sortcolumn, string sortdirection, int userID = 0)
        {
            KMPlatform.Entity.User user = ConvenienceMethods.GetCurrentUser();

            List<ECN_Framework_Entities.Communicator.Content> contentlist= ECN_Framework_BusinessLayer.Communicator.Content.GetListByContentTitle(contentName.Trim().ToUpper(), (allFolders? 0: folderID), userID, null, null, ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser, currentPage, pageSize, sortcolumn, sortdirection, archiveFilter);

            return PartialView("Partials/_ContentGrid", contentlist);
        }
    }
}