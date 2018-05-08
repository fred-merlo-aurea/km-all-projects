using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using KMSite;

namespace ecn.menu.Controllers
{
    [Authorize]
    public class IndexController : Controller
    {
        private const string UasWeb = "uas.web";
        private readonly IKMAuthenticationManager _kmAuthenticationManager = new KMAuthenticationManager();

        [HttpPost]
        public ActionResult ClientChange(ECN_Framework_BusinessLayer.MVCModels.PostModels.Menu.ClientDropDownViewModel ddmodel)
        {
            var clients = new List<KMPlatform.Entity.Client>();
            if (ddmodel.AccountChange.ToLower().Contains(UasWeb))
            {
                clients = new KMPlatform.BusinessLogic.Client().SelectActiveForClientGroupLite(ddmodel.SelectedClientGroupID)
                        .Where(x => x.IsAMS == true && x.IsActive == true)
                        .OrderBy(x => x.ClientName)
                        .ToList();
            }
            else
            {
                clients = new KMPlatform.BusinessLogic.Client().SelectActiveForClientGroupLite(ddmodel.SelectedClientGroupID)
                        .Where(x => x.IsActive == true)
                        .OrderBy(x => x.ClientName)
                        .ToList();
            }

            if (ddmodel.SelectedClientGroupID != ddmodel.CurrentClientGroupID)
            {
                if (clients != null && clients.Count > 0)
                {
                    ddmodel.SelectedClientID = clients.First().ClientID;
                }
                _kmAuthenticationManager.AddFormsAuthenticationCookie(
                    ddmodel.SelectedClientID,
                    ddmodel.SelectedClientGroupID,
                    ddmodel.SelectedProductID,
                    Response.Cookies);
                return Json(ddmodel.AccountChange);
            }
            else if (ddmodel.SelectedClientID != ddmodel.CurrentClientID)
            {

                var currentClient = clients.FirstOrDefault(x => x.ClientID == ddmodel.SelectedClientID);
                if (ddmodel.AccountChange.ToLower().Contains(UasWeb))
                {
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
                }
                _kmAuthenticationManager.AddFormsAuthenticationCookie(
                    ddmodel.SelectedClientID,
                    ddmodel.SelectedClientGroupID,
                    ddmodel.SelectedProductID,
                    Response.Cookies);
                return Json(ddmodel.AccountChange);
            }
            else if (ddmodel.SelectedProductID != ddmodel.CurrentProductID)
            {
                _kmAuthenticationManager.AddFormsAuthenticationCookie(
                    ddmodel.SelectedClientID,
                    ddmodel.SelectedClientGroupID,
                    ddmodel.SelectedProductID,
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
