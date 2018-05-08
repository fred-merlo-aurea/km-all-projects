using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Web.Security;

//Custom KM Namespaces
using UAS.Web.Controllers.Common;
using UAS.Web.Models.Common;
using System.Xml.Linq;
using System.Dynamic;

namespace UAS.Web.Controllers.Dashboard
{
    
    public class HomeController : BaseController
    {
       
        public ActionResult Index()
        {
            if (KM.Platform.User.HasService(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT) || KM.Platform.User.HasService(CurrentUser, KMPlatform.Enums.Services.UAD))
            {
                List<KMPlatform.Entity.Menu> menuList = new List<KMPlatform.Entity.Menu>();
                KMPlatform.Entity.Application app = new KMPlatform.Entity.Application();

                KMPlatform.BusinessLogic.Menu menuWorker = new KMPlatform.BusinessLogic.Menu();
                KMPlatform.BusinessLogic.Application appWorker = new KMPlatform.BusinessLogic.Application();

                app = appWorker.Select().FirstOrDefault(x => x.ApplicationName == "AMSCircMVC");

                if (app != null)
                    menuList = menuWorker.SelectForApplication(app.ApplicationID, true);
                else
                    menuList = null;
               
                return View(menuList);
            }
            else
            {
                return Redirect("/ecn.accounts/main/");
            }
        }

        public ActionResult RedirectToMenu(int MenuID)
        {

            var services = new KMPlatform.BusinessLogic.Service().Select();
            List<KMPlatform.Entity.Menu> menus = new KMPlatform.BusinessLogic.Menu().Select();
            List<KeyValuePair<string, int>> menusHasAccess = new List<KeyValuePair<string, int>>();
            var currentMenu = menus.Where(x => x.ParentMenuID == MenuID).OrderBy(x => x.MenuOrder);
            List<KMPlatform.Entity.ServiceFeature> servicefeatures = new List<KMPlatform.Entity.ServiceFeature>();
            if (KM.Platform.User.HasService(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT)|| KM.Platform.User.HasService(CurrentUser, KMPlatform.Enums.Services.UAD))
            {

                var AmSservices = services.Where(x => (x.ServiceCode == KMPlatform.Enums.Services.FULFILLMENT.ToString()|| x.ServiceCode == KMPlatform.Enums.Services.UAD.ToString())).Select(x => x);

                if (AmSservices == null && AmSservices.Count()>0)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach(var service in AmSservices)
                    {
                        servicefeatures.AddRange(new KMPlatform.BusinessLogic.ServiceFeature().SelectOnlyEnabledClientID(service.ServiceID, CurrentClientID));

                    }
                    var serviceFeaturesForMenus = from sf in servicefeatures
                                                  join m in currentMenu on sf.ServiceFeatureID equals m.ServiceFeatureID
                                                  orderby m.MenuOrder
                                                  select new { SFCode = sf.SFCode, SFMMenuUrl = m.URL, SFMenuOrder = m.MenuOrder };

                    foreach (var sfMenuFeature in serviceFeaturesForMenus)
                    {
                        KMPlatform.Enums.ServiceFeatures sfMenu = (KMPlatform.Enums.ServiceFeatures) Enum.Parse(typeof(KMPlatform.Enums.ServiceFeatures), sfMenuFeature.SFCode);
                        if ((Enum.IsDefined(typeof(KMPlatform.Enums.ServiceFeatures), sfMenu) && (KM.Platform.User.HasServiceFeature(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, sfMenu) || KM.Platform.User.HasServiceFeature(CurrentUser, KMPlatform.Enums.Services.UAD, sfMenu))))
                        {
                            menusHasAccess.Add(new KeyValuePair<string, int>(sfMenuFeature.SFMMenuUrl, sfMenuFeature.SFMenuOrder));
                        }
                        
                    }
                    if (menusHasAccess.Count == 0)
                        return RedirectToAction("Index", "Home");
                    else
                    {
                        if (menusHasAccess.Any(x => x.Value == 1)) {
                            string firstmenu =menusHasAccess.Where(x => x.Value == 1).Select(x => x.Key).First();
                            if (!string.IsNullOrEmpty(firstmenu))
                                return Redirect(firstmenu);
                            else
                                return RedirectToAction("Index", "Home");

                        }
                        else
                        {
                            string nextmenu = menusHasAccess.Select(x => x.Key).First();
                            if(!string.IsNullOrEmpty(nextmenu))
                                return Redirect(nextmenu);
                            else
                                return RedirectToAction("Index", "Home");
                        }
                    }
                        
                }

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/EmailMarketing.Site/Login/Logout");
        }

        public ActionResult GetOpenCloseWidget(int Pubid = 0)
        {
            if (KM.Platform.User.HasService(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT))
            {
                OpenCloseStatusViewModel ocVm = new OpenCloseStatusViewModel();
                if (Pubid == 0)
                {
                    Pubid = CurrentProductID;
                }
                if (Pubid > 0)
                {
                    FrameworkUAD.Entity.Product product = new FrameworkUAD.BusinessLogic.Product().Select(Pubid, CurrentClient.ClientConnections,false, true);
                    if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.OC, KMPlatform.Enums.Access.FullAccess))
                    {
                        ocVm.HasFullAccess = true;
                    }
                    else
                    {
                        ocVm.HasFullAccess = false;
                    }
                    if (product != null)
                    {
                        ocVm.PubID = product.PubID;
                        ocVm.PubName = product.PubCode;
                        ocVm.AllowAddRemove = product.AddRemoveAllowed.HasValue ? (bool) product.AddRemoveAllowed : false;
                        ocVm.AllowClientImport = product.ClientImportAllowed.HasValue ? (bool) product.ClientImportAllowed : false;
                        ocVm.AllowDataEntry = product.AllowDataEntry;
                        ocVm.AllowKMmport = product.KMImportAllowed.HasValue ? (bool) product.KMImportAllowed : false;

                    }
                    else
                    {
                        ocVm.PubName = "Product is empty.";

                    }
                }
                else
                {
                    ocVm.PubName = "Please Retry using Product Selection.";
                }
                return PartialView("Partials/_openCloseWidget", ocVm);
            }

            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult UpdateOpenCloseWidget(bool locked = false, string target = "", int PubID = 0)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.OC, KMPlatform.Enums.Access.FullAccess))
            {
                OpenCloseStatusViewModel ocVm = new OpenCloseStatusViewModel();
                if (PubID > 0)
                {
                    FrameworkUAD.BusinessLogic.Product prodWorker = new FrameworkUAD.BusinessLogic.Product();
                    FrameworkUAD.BusinessLogic.Issue issueWorker = new FrameworkUAD.BusinessLogic.Issue();
                    List<FrameworkUAD.Entity.Issue> issueList = issueWorker.SelectPublication(PubID, CurrentClient.ClientConnections);
                    FrameworkUAD.Entity.Issue myIssue = issueList.Where(x => x.IsComplete == false).FirstOrDefault();
                    FrameworkUAD.Entity.Product myPub = prodWorker.Select(PubID, CurrentClient.ClientConnections,false,true);
                    if (myPub != null)
                    {

                        if (locked)
                        {
                            switch (target)
                            {
                                #region Data Entry
                                case "dataEntry":

                                    myPub.AllowDataEntry = false;
                                    myPub.ClientImportAllowed = true;
                                    myPub.KMImportAllowed = true;
                                    myPub.DateUpdated = DateTime.Now;
                                    myPub.UpdatedByUserID = CurrentUser.UserID;
                                    prodWorker.Save(myPub, CurrentClient.ClientConnections);

                                    break;
                                #endregion

                                #region External Imports
                                case "webSync":
                                    if (myPub.AllowDataEntry == false && myPub.AddRemoveAllowed == false)
                                    {
                                        myPub.ClientImportAllowed = false;
                                        if (myPub.KMImportAllowed == false)
                                        {
                                            myPub.AddRemoveAllowed = true;
                                            FrameworkUAD.BusinessLogic.ActionBackUp aBkpWorker = new FrameworkUAD.BusinessLogic.ActionBackUp();
                                            aBkpWorker.Bulk_Insert(myPub.PubID, CurrentClient.ClientConnections);
                                        }
                                        myPub.DateUpdated = DateTime.Now;
                                        myPub.UpdatedByUserID = CurrentUser.UserID;
                                        prodWorker.Save(myPub, CurrentClient.ClientConnections);
                                    }
                                    else
                                        ocVm.ErrorMessage = "Data entry needs to be closed before you can close External Import.";
                                    break;
                                #endregion

                                #region Internal Imports
                                case "fileImport":
                                    if (myPub.AllowDataEntry == false && myPub.AddRemoveAllowed == false)
                                    {
                                        myPub.KMImportAllowed = false;
                                        if (myPub.ClientImportAllowed == false)
                                        {
                                            myPub.AddRemoveAllowed = true;
                                            FrameworkUAD.BusinessLogic.ActionBackUp aBkpWorker = new FrameworkUAD.BusinessLogic.ActionBackUp();
                                            aBkpWorker.Bulk_Insert(myPub.PubID, CurrentClient.ClientConnections);
                                        }

                                        myPub.DateUpdated = DateTime.Now;
                                        myPub.UpdatedByUserID = CurrentUser.UserID;
                                        prodWorker.Save(myPub, CurrentClient.ClientConnections);
                                    }
                                    else
                                        ocVm.ErrorMessage = "Data entry needs to be closed before you can close Internal Import.";
                                    break;
                                #endregion

                                #region Add Remove
                                case "addRemove":
                                    if (myPub.KMImportAllowed == false && myPub.ClientImportAllowed == false && myPub.AllowDataEntry == false)
                                    {

                                        myPub.AddRemoveAllowed = false;
                                        myPub.DateUpdated = DateTime.Now;
                                        myPub.UpdatedByUserID = CurrentUser.UserID;
                                        //pubData.Proxy.Save(accessKey, myPub,c);
                                        prodWorker.Save(myPub, CurrentClient.ClientConnections);
                                        myIssue.IsClosed = true;
                                        myIssue.DateClosed = DateTime.Now;
                                        myIssue.ClosedByUserID = CurrentUser.UserID;
                                        issueWorker.Save(myIssue, CurrentClient.ClientConnections);

                                    }
                                    else
                                        ocVm.ErrorMessage = "Internal Import and External Import needs to be closed before you can close Add/Removes.";
                                    break;
                                    #endregion
                            }
                        }
                        else
                        {
                            switch (target)
                            {
                                #region Data Entry
                                case "dataEntry":

                                    myPub.AllowDataEntry = true;
                                    myPub.AddRemoveAllowed = false;
                                    myPub.ClientImportAllowed = false;
                                    myPub.KMImportAllowed = false;
                                    myPub.DateUpdated = DateTime.Now;
                                    myPub.UpdatedByUserID = CurrentUser.UserID;
                                    prodWorker.Save(myPub, CurrentClient.ClientConnections);
                                    myIssue.IsClosed = false;
                                    myIssue.DateOpened = DateTime.Now;
                                    myIssue.OpenedByUserID = CurrentUser.UserID;
                                    issueWorker.Save(myIssue, CurrentClient.ClientConnections);
                                    break;
                                #endregion

                                #region External Imports
                                case "webSync":
                                    if (myPub.AllowDataEntry == false && myPub.AddRemoveAllowed == false && myIssue.IsClosed == false)
                                    {
                                        myPub.ClientImportAllowed = true;
                                        myPub.DateUpdated = DateTime.Now;
                                        myPub.UpdatedByUserID = CurrentUser.UserID;
                                        prodWorker.Save(myPub, CurrentClient.ClientConnections);
                                    }
                                    else
                                        ocVm.ErrorMessage = "Data entry needs to be closed before you can begin External Import.";
                                    break;
                                #endregion

                                #region Internal Imports
                                case "fileImport":
                                    if (myPub.AllowDataEntry == false && myPub.AddRemoveAllowed == false && myIssue.IsClosed == false)
                                    {
                                        myPub.KMImportAllowed = true;
                                        myPub.DateUpdated = DateTime.Now;
                                        myPub.UpdatedByUserID = CurrentUser.UserID;
                                        prodWorker.Save(myPub, CurrentClient.ClientConnections);
                                    }
                                    else
                                        ocVm.ErrorMessage = "Data entry needs to be closed before you can begin Internal Import.";
                                    break;
                                #endregion

                                #region Add Remove
                                case "addRemove":
                                    if (myPub.KMImportAllowed == false && myPub.ClientImportAllowed == false && myPub.AllowDataEntry == false)
                                    {

                                        myPub.AddRemoveAllowed = true;
                                        myPub.DateUpdated = DateTime.Now;
                                        myPub.UpdatedByUserID = CurrentUser.UserID;
                                        prodWorker.Save(myPub, CurrentClient.ClientConnections);
                                    }
                                    else
                                        ocVm.ErrorMessage = "Data Entry, Internal Import and External Import needs to be closed before you can begin Add/Removes.";
                                    break;
                                    #endregion
                            }
                        }
                        if (KM.Platform.User.HasServiceFeature(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.OC))
                        {
                            ocVm.HasFullAccess = true;
                        }
                        else
                        {
                            ocVm.HasFullAccess = false;
                        }
                        ocVm.PubID = myPub.PubID;
                        ocVm.PubName = myPub.PubCode;
                        ocVm.AllowAddRemove = myPub.AddRemoveAllowed.HasValue ? (bool) myPub.AddRemoveAllowed : false;
                        ocVm.AllowClientImport = myPub.ClientImportAllowed.HasValue ? (bool) myPub.ClientImportAllowed : false;
                        ocVm.AllowDataEntry = myPub.AllowDataEntry;
                        ocVm.AllowKMmport = myPub.KMImportAllowed.HasValue ? (bool) myPub.KMImportAllowed : false;
                    }
                    else
                    {
                        ocVm.ErrorMessage = "Please select product to view current status.";

                    }
                }
                else
                {
                    ocVm.ErrorMessage = "Please Retry using Product Selection.";
                }
                return PartialView("Partials/_openCloseWidget", ocVm);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }


        }

        #region dropdown forms auth stuff

        
        [HttpPost]
        //public ActionResult ClientChange(ECN_Framework_BusinessLayer.MVCModels.PostModels.Menu.ClientDropDown model)
        public ActionResult ClientChange(ECN_Framework_BusinessLayer.MVCModels.PostModels.Menu.ClientDropDownViewModel ddmodel)
        {
            List<KMPlatform.Entity.Client> lstClient = new List<KMPlatform.Entity.Client>();
            lstClient = new KMPlatform.BusinessLogic.Client().SelectActiveForClientGroupLite(ddmodel.SelectedClientGroupID)
                    .Where(x => x.IsAMS == true && x.IsActive == true)
                    .OrderBy(x => x.ClientName)
                    .ToList();

            if (ddmodel.SelectedClientGroupID != ddmodel.CurrentClientGroupID)
            {
               
                if (lstClient != null && lstClient.Count > 0)
                {
                    ddmodel.SelectedClientID = lstClient.First().ClientID;
                }
                DoFormsAuth(ddmodel.SelectedClientID, ddmodel.SelectedClientGroupID, ddmodel.SelectedProductID);
                return Json(ddmodel.AccountChange);
            }
            else if (ddmodel.SelectedClientID != ddmodel.CurrentClientID)
            {
                
                var CurrentClient = lstClient.FirstOrDefault(x => x.ClientID == ddmodel.SelectedClientID);
                if (CurrentClient.Products == null || CurrentClient.Products.Count == 0)
                {
                    var ProductItems = new KMPlatform.BusinessLogic.Client().SelectProducts(CurrentClient).Where(x => x.IsCirc == true ).ToList();
                    ddmodel.SelectedProductID = Convert.ToInt16(ProductItems.FirstOrDefault().ProductID);
                    ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ProductID = ddmodel.SelectedProductID;
                }
                else
                {
                    var ProductItems = CurrentClient.Products.Where(x=>x.IsCirc == true ).ToList();
                    ddmodel.SelectedProductID = Convert.ToInt16(ProductItems.FirstOrDefault().ProductID);
                    ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ProductID = ddmodel.SelectedProductID;
                }
                DoFormsAuth(ddmodel.SelectedClientID, ddmodel.SelectedClientGroupID, ddmodel.SelectedProductID);
                return Json(ddmodel.AccountChange);
            }
            else if (ddmodel.SelectedProductID != ddmodel.CurrentProductID)
            {
                DoFormsAuth(ddmodel.SelectedClientID, ddmodel.SelectedClientGroupID, ddmodel.SelectedProductID);
                return Json(ddmodel.AccountChange);
            }
            else
            {
                return Json(ddmodel.AccountChange);
            }
        }
        

        private void DoFormsAuth(int clientID, int ClientGroupID, int ProductID)
        {
            int userID = CurrentUser.UserID;

            if (clientID > 0 && (KMPlatform.BusinessLogic.User.IsSystemAdministrator(CurrentUser) || HasAuthorized(CurrentUser.UserID, clientID)))
            {

                FormsAuthentication.SignOut();

                FormsAuthentication.SetAuthCookie(userID.ToString(), false);

                // Create a new ticket used for authentication
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1, // Ticket version
                    userID.ToString(), // UserID associated with ticket
                    DateTime.Now, // Date/time issued
                    DateTime.Now.AddDays(30), // Date/time to expire
                    true, // "true" for a persistent user cookie
                    CreateAuthenticationTicketUserData(CurrentUser, ClientGroupID, clientID, ProductID), // User-data, in this case the roles
                    FormsAuthentication.FormsCookiePath); // Path cookie valid for

                // Hash the cookie for transport
                string hash = FormsAuthentication.Encrypt(ticket);
                HttpCookie cookie = new HttpCookie(
                    FormsAuthentication.FormsCookieName, // Name of auth cookie
                    hash); // Hashed ticket

                // Add the cookie to the list for outgoing response
                Response.Cookies.Add(cookie);
                ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClearSession();
                //Wiping out local UserSession so it will get what we just updated



            }
        }

        private bool HasAuthorized(int userID, int clientID)
        {
            if (CurrentUser.UserClientSecurityGroupMaps.Find(x => x.ClientID == clientID) != null)
                return true;

            return false;
        }

        internal static string CreateAuthenticationTicketUserData(KMPlatform.Entity.User u, int clientgroupID, int clientID, int productID)
        {
            ECN_Framework_Entities.Accounts.Customer ecnCustomer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(clientID, false);

            return String.Join(",",
                ecnCustomer.CustomerID,
                ecnCustomer.BaseChannelID,
                clientgroupID,
                clientID,
                u.AccessKey,
                productID
                );
        }
        #endregion
    }

   
}