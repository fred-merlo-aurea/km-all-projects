using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Web.Security;
using UAD.DataCompare.Web.Models;
using UAD.DataCompare.Web.Controllers.Common;

namespace UAD.DataCompare.Web.Controllers
{
    public class HomeController : BaseController
    {
        private const string HomeRedirectDestination = "/Home";
        private const string ClientGroupIdDataValueField = "ClientGroupID";
        private const string ClientGroupNameDataTextField = "ClientGroupName";
        private const string ClientIdDataValueField = "ClientID";
        private const string ClientNameDataTextField = "ClientName";
        private const string VirtualPathConfigKey = "UAS_VirtualPath";
        private readonly KMSite.IKMAuthenticationManager _kmAuthenticationManager = new KMSite.KMAuthenticationManager();

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
            return Redirect("Datacompare");
            //return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/EmailMarketing.Site/Login/Logout");
        }

        public ActionResult _ClientDropDown(ClientDropDown cdd)
        {
            if (ModelState.IsValid)
            {
                if (cdd != null && cdd.CurrentClientGroupID > 0)
                {
                    var baseChannels = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetAll();
                    var clientGroup = new KMPlatform.BusinessLogic.ClientGroup();
                    var amsClientGroupList = clientGroup.SelectForAMS(false);
                    cdd.ClientGroups = CurrentUser.ClientGroups
                        .Where(x => baseChannels.Exists(y => y.PlatformClientGroupID == x.ClientGroupID)
                                    && amsClientGroupList.Exists(z => z.ClientGroupID == x.ClientGroupID)
                                    && x.IsActive)
                        .OrderBy(x => x.ClientGroupName)
                        .ToList();

                    cdd.ClientGroupItems = new SelectList(
                        cdd.ClientGroups,
                        ClientGroupIdDataValueField,
                        ClientGroupNameDataTextField,
                        cdd.SelectedClientGroupID);

                    var clients = new List<KMPlatform.Entity.Client>();
                    if (cdd.SelectedClientGroupID != cdd.CurrentClientGroupID)
                    {
                        //Client Group change
                        if (KMPlatform.BusinessLogic.User.IsChannelAdministrator(CurrentUser))
                        {
                            clients = new KMPlatform.BusinessLogic.Client().SelectActiveForClientGroupLite(cdd.SelectedClientGroupID)
                                .Where(x => x.IsAMS)
                                .OrderBy(x => x.ClientName)
                                .ToList();
                        }
                        cdd.SelectedClientID = clients.First().ClientID;
                        cdd.Clients = clients;
                        cdd.ClientItems = new SelectList(cdd.Clients, ClientIdDataValueField, ClientNameDataTextField, cdd.SelectedClientID);
                        cdd.CurrentClientGroupID = cdd.SelectedClientGroupID;
                        cdd.CurrentClientID = cdd.SelectedClientID;
                        _kmAuthenticationManager.AddFormsAuthenticationCookie(cdd.SelectedClientID, cdd.SelectedClientGroupID, Response.Cookies);
                        return Redirect(ConfigurationManager.AppSettings[VirtualPathConfigKey].ToString() + HomeRedirectDestination);
                    }
                    else if (cdd.SelectedClientID != cdd.CurrentClientID)
                    {
                        //Client change
                        cdd = RepopulateDropDowns(cdd);
                        _kmAuthenticationManager.AddFormsAuthenticationCookie(cdd.SelectedClientID, cdd.SelectedClientGroupID, Response.Cookies);
                        return Redirect(ConfigurationManager.AppSettings[VirtualPathConfigKey].ToString() + HomeRedirectDestination);
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

        private ClientDropDown RepopulateDropDowns(ClientDropDown cdd)
        {

            List<ECN_Framework_Entities.Accounts.BaseChannel> bcList = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetAll();
            KMPlatform.BusinessLogic.ClientGroup cgWorker = new KMPlatform.BusinessLogic.ClientGroup();
            List<KMPlatform.Entity.ClientGroup> amsClientGroupList = cgWorker.SelectForAMS(false);
            cdd.ClientGroups = CurrentUser.ClientGroups.Where(x => bcList.Exists(y => y.PlatformClientGroupID == x.ClientGroupID) && amsClientGroupList.Exists(z => z.ClientGroupID == x.ClientGroupID) && x.IsActive == true).OrderBy(x => x.ClientGroupName).ToList();
            cdd.ClientGroupItems = new SelectList(cdd.ClientGroups, ClientGroupIdDataValueField, ClientGroupNameDataTextField, cdd.SelectedClientGroupID);
            List<KMPlatform.Entity.Client> lstClient = new List<KMPlatform.Entity.Client>();
            if (KMPlatform.BusinessLogic.User.IsChannelAdministrator(CurrentUser))
            {
                lstClient = new KMPlatform.BusinessLogic.Client().SelectActiveForClientGroupLite(CurrentUser.ClientGroups.First(x => x.ClientGroupID == CurrentClientGroupID).ClientGroupID).Where(x => x.IsAMS == true).OrderBy(x => x.ClientName).ToList();
            }
            else
            {
                lstClient = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUserClientGroupClients.Where(x => x.IsAMS == true).ToList();
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