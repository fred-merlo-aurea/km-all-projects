using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Web.Security;

using KMSite;

namespace KM_MVC_Template.Controllers
{
    public class HomeController : BaseController
    {

        private KMPlatform.Entity.User CurrentUser
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser; }
        }

        private int CurrentClientGroupID
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClientGroupID; }
        }

        private int CurrentClientID
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClientID; }
        }

        public ActionResult Index()
        {
            //return Redirect("Dashboard");
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/EmailMarketing.Site/Login/Logout");
        }

        public ActionResult _ClientDropDown(KM_MVC_Template.Models.ClientDropDown cdd)
        {
            if (ModelState.IsValid)
            {
                if (cdd != null && cdd.CurrentClientGroupID > 0)
                {
                    List<ECN_Framework_Entities.Accounts.BaseChannel> bcList = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetAll();
                    cdd.ClientGroups = CurrentUser.ClientGroups.Where(x => bcList.Exists(y => y.PlatformClientGroupID == x.ClientGroupID) && x.IsActive == true).OrderBy(x => x.ClientGroupName).ToList();
                    cdd.ClientGroupItems = new SelectList(cdd.ClientGroups, "ClientGroupID", "ClientGroupName", cdd.SelectedClientGroupID);
                    List<KMPlatform.Entity.Client> lstClient = new List<KMPlatform.Entity.Client>();
                    if (cdd.SelectedClientGroupID != cdd.CurrentClientGroupID)
                    {
                        //Client Group change
                        if (KMPlatform.BusinessLogic.User.IsChannelAdministrator(CurrentUser))
                        {
                            lstClient = new KMPlatform.BusinessLogic.Client().SelectActiveForClientGroupLite(cdd.SelectedClientGroupID).OrderBy(x => x.ClientName).ToList();

                        }
                        cdd.SelectedClientID = lstClient.First().ClientID;
                        cdd.Clients = lstClient;
                        cdd.ClientItems = new SelectList(cdd.Clients, "ClientID", "ClientName", cdd.SelectedClientID);
                        cdd.CurrentClientGroupID = cdd.SelectedClientGroupID;
                        cdd.CurrentClientID = cdd.SelectedClientID;
                        DoFormsAuth(cdd.SelectedClientID, cdd.SelectedClientGroupID);
                        return Redirect(ConfigurationManager.AppSettings["UAS_VirtualPath"].ToString() + "/Home");
                    }
                    else if (cdd.SelectedClientID != cdd.CurrentClientID)
                    {
                        //Client change
                        cdd = RepopulateDropDowns(cdd);
                        DoFormsAuth(cdd.SelectedClientID, cdd.SelectedClientGroupID);
                        return Redirect(ConfigurationManager.AppSettings["UAS_VirtualPath"].ToString() + "/Home");
                    }
                    else//Post from different view???
                    {
                        cdd = RepopulateDropDowns(cdd);
                    }
                }
                else
                {
                    cdd = RepopulateDropDowns(cdd);
                }

                return PartialView("~/Views/Shared/Partials/_ClientDropDown.cshtml", cdd);
            }
            else
            {
                cdd = RepopulateDropDowns(cdd);
                return PartialView("~/Views/Shared/Partials/_ClientDropDown.cshtml", cdd);
            }
        }


        private KM_MVC_Template.Models.ClientDropDown RepopulateDropDowns(KM_MVC_Template.Models.ClientDropDown cdd)
        {

            List<ECN_Framework_Entities.Accounts.BaseChannel> bcList = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetAll();
            cdd.ClientGroups = CurrentUser.ClientGroups.Where(x => bcList.Exists(y => y.PlatformClientGroupID == x.ClientGroupID) && x.IsActive == true).OrderBy(x => x.ClientGroupName).ToList();
            cdd.ClientGroupItems = new SelectList(cdd.ClientGroups, "ClientGroupID", "ClientGroupName", cdd.SelectedClientGroupID);
            List<KMPlatform.Entity.Client> lstClient = new List<KMPlatform.Entity.Client>();
            if (KMPlatform.BusinessLogic.User.IsChannelAdministrator(CurrentUser))
            {
                lstClient = new KMPlatform.BusinessLogic.Client().SelectActiveForClientGroupLite(CurrentUser.ClientGroups.First(x => x.ClientGroupID == CurrentClientGroupID).ClientGroupID).OrderBy(x => x.ClientName).ToList();
            }
            else
            {
                lstClient = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUserClientGroupClients;
            }
            cdd.Clients = lstClient;
            cdd.ClientItems = new SelectList(cdd.Clients, "ClientID", "ClientName", CurrentClientID);
            cdd.SelectedClientGroupID = CurrentClientGroupID;
            cdd.SelectedClientID = CurrentClientID;
            cdd.CurrentClientGroupID = cdd.SelectedClientGroupID;
            cdd.CurrentClientID = cdd.SelectedClientID;

            return cdd;
        }

        private void DoFormsAuth(int clientID, int ClientGroupID)
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
                    CreateAuthenticationTicketUserData(CurrentUser, ClientGroupID, clientID), // User-data, in this case the roles
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

        public bool HasAuthorized(int userID, int clientID)
        {
            if (CurrentUser.UserClientSecurityGroupMaps.Find(x => x.ClientID == clientID) != null)
                return true;

            return false;
        }

        internal static string CreateAuthenticationTicketUserData(KMPlatform.Entity.User u, int clientgroupID, int clientID)
        {
            ECN_Framework_Entities.Accounts.Customer ecnCustomer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(clientID, false);

            return String.Join(",",
                ecnCustomer.CustomerID,
                ecnCustomer.BaseChannelID,
                clientgroupID,
                clientID,
                u.AccessKey
                );
        }
    }
}