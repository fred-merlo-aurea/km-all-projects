using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECN_Framework_Common.Objects;
using ecn.domaintracking.Models;

namespace ecn.domaintracking.Controllers
{
    public class WebController : Controller
    {


        private KMPlatform.Entity.User CurrentUser
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser; }
        }


        public ActionResult GetCustomers()
        {
            //var customers = manager.GetCustomers(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.AccessKey.ToString());
            //return Json(customers, JsonRequestBehavior.AllowGet);
            List<KMPlatform.Entity.Client> clients = new List<KMPlatform.Entity.Client>();
            if (CurrentUser.IsPlatformAdministrator)
                clients = new KMPlatform.BusinessLogic.Client().SelectActiveForClientGroup(CurrentUser.CurrentClientGroup.ClientGroupID);
            else
                clients = new KMPlatform.BusinessLogic.Client().SelectbyUserIDclientgroupID(CurrentUser.UserID, CurrentUser.CurrentClientGroup.ClientGroupID, false);
            List<ECN_Framework_Entities.Accounts.Customer> custs = new List<ECN_Framework_Entities.Accounts.Customer>();

            foreach (KMPlatform.Entity.Client c in clients)
            {
                try
                {
                    if (CurrentUser.UserClientSecurityGroupMaps.Exists(x => x.ClientID == c.ClientID) || CurrentUser.IsPlatformAdministrator)
                        custs.Add(ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(c.ClientID, false));
                }
                catch (SecurityException se)
                { }
            }

            string AccessKey = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.AccessKey.ToString();

            IEnumerable<Customer> customers = from c in custs
                                              select new Customer { CustomerID = c.CustomerID.ToString(), CustomerName = c.CustomerName, AccessKey = AccessKey };

            return Json(customers, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFolders(int customerId, int? folderId)
        {
            IEnumerable<ECN_Framework_Entities.Communicator.Folder> folders;
            if (folderId.HasValue)
                folders = ECN_Framework_BusinessLayer.Communicator.Folder.GetByType(customerId, "GRP", CurrentUser).Where(x => x.ParentID == folderId.Value).ToList();// manager.GetFoldersByCustomerIDAndParentID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.AccessKey.ToString(), customerId, folderId.Value);
            else
                folders = ECN_Framework_BusinessLayer.Communicator.Folder.GetByType(customerId, "GRP", CurrentUser).Where(x => x.ParentID == 0).ToList();// manager.GetFoldersByCustomerID(ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentBaseChannel.AccessKey.ToString(), customerId);

            var iEnumFolders = from f in folders
                               select new Folder { CreatedDate = f.CreatedDate, CreatedUserID = f.CreatedUserID.HasValue ? f.CreatedUserID.Value : 0, CustomerID = f.CustomerID.Value, CustomerName = "", FolderDescription = f.FolderDescription, FolderID = f.FolderID, FolderName = f.FolderName, FolderType = f.FolderType, ParentID = f.ParentID, UpdatedDate = f.UpdatedDate, UpdatedUserID = f.UpdatedUserID.HasValue ? f.UpdatedUserID.Value : -1 };
            return Json(iEnumFolders, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetAllFolders(int customerID)
        {
            List<ECN_Framework_Entities.Communicator.Folder> folders;
            
                folders = ECN_Framework_BusinessLayer.Communicator.Folder.GetByCustomerID(customerID, CurrentUser).Where(x => x.FolderType == "GRP").OrderBy(x => x.FolderName).ToList();
            ECN_Framework_Entities.Communicator.Folder rootFolder = new ECN_Framework_Entities.Communicator.Folder();
            rootFolder.CustomerID = customerID;
            rootFolder.FolderID = 0;
            rootFolder.FolderName = "Root";
            rootFolder.FolderType = "GRP";
            folders.Insert(0, rootFolder);
            

            var iEnumFolders = from f in folders
                               select new Folder { CreatedDate = f.CreatedDate, CreatedUserID = f.CreatedUserID.HasValue ? f.CreatedUserID.Value : 0, CustomerID = f.CustomerID.Value, CustomerName = "", FolderDescription = f.FolderDescription, FolderID = f.FolderID, FolderName = f.FolderName, FolderType = f.FolderType, ParentID = f.ParentID, UpdatedDate = f.UpdatedDate, UpdatedUserID = f.UpdatedUserID.HasValue ? f.UpdatedUserID.Value : -1 };
            return Json(iEnumFolders, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult GetGroups(int customerId, int? folderId)
        {
            List<ECN_Framework_Entities.Communicator.Group> groups;

            if (folderId.HasValue)
            {
                groups = ECN_Framework_BusinessLayer.Communicator.Group.GetByFolderIDCustomerID_NoAccessCheck(folderId.Value, customerId, CurrentUser);
            }
            else
                groups = ECN_Framework_BusinessLayer.Communicator.Group.GetByFolderIDCustomerID_NoAccessCheck(0, customerId, CurrentUser); // manager.GetRootGroupsByCustomerID(ApiKey, customerId);

            IEnumerable<Group> GroupIEnum = from g in groups
                                            where g.Archived.Value == false
                                            select new Group { GroupID = g.GroupID, CustomerID = g.CustomerID, FolderID = g.FolderID ?? 0, GroupName = g.GroupName, GroupDescription = g.GroupDescription }
                                           ;

            return Json(GroupIEnum.OrderBy(g => g.GroupName), JsonRequestBehavior.AllowGet);
        }
    }
}