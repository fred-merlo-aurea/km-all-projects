using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Web.Security;
using MVC.Standards.Common;

namespace MVC.Standards.Controllers
{
    public class HomeController : BaseController
    {
        private const string ClientGroupIdDataValueField = "ClientGroupID";
        private const string ClientGroupNameDataTextField = "ClientGroupName";
        private const string ClientIdDataValueField = "ClientID";
        private const string ClientNameDataTextField = "ClientName";
        private const string UASVirtualPathConfigKey = "UAS_VirtualPath";
        private const string RedirectDestination = "/Home";
        private const string PartialViewPath = "~/Views/Shared/Partials/_ClientDropDown.cshtml";
        private readonly KMSite.IKMAuthenticationManager _kmAuthenticationManager = new KMSite.KMAuthenticationManager();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/EmailMarketing.Site/Login/Logout");
        }

        public ActionResult _ClientDropDown(MVC.Standards.Models.ClientDropDown cdd)
        {
            if (ModelState.IsValid)
            {
                if (cdd != null && cdd.CurrentClientGroupID > 0)
                {
                    var baseChannels = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetAll();
                    cdd.ClientGroups = CurrentUser.ClientGroups
                        .Where(x => baseChannels.Exists(y => y.PlatformClientGroupID == x.ClientGroupID) && x.IsActive)
                        .OrderBy(x => x.ClientGroupName)
                        .ToList();

                    cdd.ClientGroupItems = new SelectList(cdd.ClientGroups, ClientGroupIdDataValueField, ClientGroupNameDataTextField, cdd.SelectedClientGroupID);
                    var clients = new List<KMPlatform.Entity.Client>();
                    if (cdd.SelectedClientGroupID != cdd.CurrentClientGroupID)
                    {
                        //Client Group change
                        if (KMPlatform.BusinessLogic.User.IsChannelAdministrator(CurrentUser))
                        {
                            clients = new KMPlatform.BusinessLogic.Client().SelectActiveForClientGroupLite(cdd.SelectedClientGroupID).OrderBy(x => x.ClientName).ToList();
                        }
                        cdd.SelectedClientID = clients.First().ClientID;
                        cdd.Clients = clients;
                        cdd.ClientItems = new SelectList(cdd.Clients, ClientIdDataValueField, ClientNameDataTextField, cdd.SelectedClientID);
                        cdd.CurrentClientGroupID = cdd.SelectedClientGroupID;
                        cdd.CurrentClientID = cdd.SelectedClientID;
                        _kmAuthenticationManager.AddFormsAuthenticationCookie(cdd.SelectedClientID, cdd.SelectedClientGroupID, Response.Cookies);
                        return Redirect(ConfigurationManager.AppSettings[UASVirtualPathConfigKey].ToString() + RedirectDestination);
                    }
                    else if (cdd.SelectedClientID != cdd.CurrentClientID)
                    {
                        //Client change
                        cdd = RepopulateDropDowns(cdd);
                        _kmAuthenticationManager.AddFormsAuthenticationCookie(cdd.SelectedClientID, cdd.SelectedClientGroupID, Response.Cookies);
                        return Redirect(ConfigurationManager.AppSettings[UASVirtualPathConfigKey].ToString() + RedirectDestination);
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

                return PartialView(PartialViewPath, cdd);
            }
            else
            {
                cdd = RepopulateDropDowns(cdd);
                return PartialView(PartialViewPath, cdd);
            }
        }


        private MVC.Standards.Models.ClientDropDown RepopulateDropDowns(MVC.Standards.Models.ClientDropDown cdd)
        {

            List<ECN_Framework_Entities.Accounts.BaseChannel> bcList = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetAll();
            cdd.ClientGroups = CurrentUser.ClientGroups.Where(x => bcList.Exists(y => y.PlatformClientGroupID == x.ClientGroupID) && x.IsActive == true).OrderBy(x => x.ClientGroupName).ToList();
            cdd.ClientGroupItems = new SelectList(cdd.ClientGroups, ClientGroupIdDataValueField, ClientGroupNameDataTextField, cdd.SelectedClientGroupID);
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
            cdd.ClientItems = new SelectList(cdd.Clients, ClientIdDataValueField, ClientNameDataTextField, CurrentClientID);
            cdd.SelectedClientGroupID = CurrentClientGroupID;
            cdd.SelectedClientID = CurrentClientID;
            cdd.CurrentClientGroupID = cdd.SelectedClientGroupID;
            cdd.CurrentClientID = cdd.SelectedClientID;

            return cdd;
        }

        public bool HasAuthorized(int userID, int clientID)
        {
            return _kmAuthenticationManager.HasAuthorized(clientID);
        }
    }
}