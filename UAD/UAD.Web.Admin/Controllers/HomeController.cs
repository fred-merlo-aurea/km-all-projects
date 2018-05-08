using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Web.Security;
using UAD.Web.Admin.Controllers.Common;
using UAD.Web.Admin.Models;

namespace UAD.Web.Admin.Controllers
{
    public class HomeController : BaseController
    {
        private readonly KMSite.IKMAuthenticationManager _kmAuthenticationManager = new KMSite.KMAuthenticationManager();

        public ActionResult Index()
        {
            if (KM.Platform.User.HasService(CurrentUser, KMPlatform.Enums.Services.UAD))
            {
                return View();
            }
            else
            {
                return Redirect("/ecn.accounts/main/");
            }
        }

        [HttpPost]
        public ActionResult ClientChange(ECN_Framework_BusinessLayer.MVCModels.PostModels.Menu.ClientDropDownViewModel ddmodel)
        {
            var clients = new KMPlatform.BusinessLogic.Client().SelectActiveForClientGroupLite(ddmodel.SelectedClientGroupID)
                    .Where(x => x.IsAMS && x.IsActive)
                    .OrderBy(x => x.ClientName)
                    .ToList();

            if (ddmodel.SelectedClientGroupID != ddmodel.CurrentClientGroupID)
            {
                if (clients != null && clients.Count > 0)
                {
                    ddmodel.SelectedClientID = clients.First().ClientID;
                }

                _kmAuthenticationManager.AddFormsAuthenticationCookie(
                    ddmodel.SelectedClientID,
                    ddmodel.SelectedClientGroupID,
                    Response.Cookies);

                return Json(ddmodel.AccountChange);
            }
            else if (ddmodel.SelectedClientID != ddmodel.CurrentClientID)
            {
                var currentClient = clients.FirstOrDefault(x => x.ClientID == ddmodel.SelectedClientID);
                if (currentClient.Products == null || currentClient.Products.Count == 0)
                {
                    var productItems = new KMPlatform.BusinessLogic.Client().SelectProducts(currentClient).Where(x => x.IsCirc == true).ToList();
                    ddmodel.SelectedProductID = Convert.ToInt16(productItems.FirstOrDefault().ProductID);
                    ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ProductID = ddmodel.SelectedProductID;
                }
                else
                {
                    var productItems = currentClient.Products.Where(x => x.IsCirc == true).ToList();
                    ddmodel.SelectedProductID = Convert.ToInt16(productItems.FirstOrDefault().ProductID);
                    ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ProductID = ddmodel.SelectedProductID;
                }

                _kmAuthenticationManager.AddFormsAuthenticationCookie(
                    ddmodel.SelectedClientID,
                    ddmodel.SelectedClientGroupID,
                    Response.Cookies);

                return Json(ddmodel.AccountChange);
            }
            else if (ddmodel.SelectedProductID != ddmodel.CurrentProductID)
            {
                _kmAuthenticationManager.AddFormsAuthenticationCookie(
                    ddmodel.SelectedClientID,
                    ddmodel.SelectedClientGroupID,
                    Response.Cookies);

                return Json(ddmodel.AccountChange);
            }
            else
            {
                return Json(ddmodel.AccountChange);
            }
        }

        public bool HasAuthorized(int userID, int clientID)
        {
            return _kmAuthenticationManager.HasAuthorized(clientID);
        }
    }
}