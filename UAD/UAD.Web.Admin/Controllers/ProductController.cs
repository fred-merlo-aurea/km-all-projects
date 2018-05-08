using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using KMPS.MD.Objects;
using System.Collections.Specialized;
using FrameworkUAD.Object;
using UAD.Web.Admin.Infrastructure;

namespace UAD.Web.Admin.Controllers
{
    public class ProductController : Controller
    {
        #region Current Session Properties
        private KMPlatform.Object.ClientConnections _clientconnections = null;

        private KMPlatform.Entity.User CurrentUser
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser; }
        }

        private KMPlatform.Object.ClientConnections ClientConnections
        {
            get
            {
                if (_clientconnections == null)
                {
                    _clientconnections = new KMPlatform.Object.ClientConnections(CurrentUser.CurrentClient);
                    return _clientconnections;
                }
                else
                    return _clientconnections;
            }
        }
        #endregion

        public ActionResult Index()
        {
            if (!KM.Platform.User.IsSystemAdministrator(CurrentUser))
            {
                Response.Redirect("../SecurityAccessError.aspx");
            }

            List<FrameworkUAD.Entity.Product> pubslist = new FrameworkUAD.BusinessLogic.Product().Select(ClientConnections);
            Models.ProductWrapper Model = new Models.ProductWrapper(pubslist);

            var errors = this.GetTempData("UADErrors") as List<UADError>;
            if (errors != null)
            {
                Model.Errors = errors;
            }

            return View(Model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            if (!KM.Platform.User.IsSystemAdministrator(CurrentUser))
            {
                Response.Redirect("../SecurityAccessError.aspx");
            }

            Models.ProductWrapper Model = new Models.ProductWrapper();
            Model.clients = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUserClientGroupClients;
            Model.availableBrands = new FrameworkUAD.BusinessLogic.Brand().Select(ClientConnections);

            return View(Model);
        }

        [HttpPost]
        public ActionResult Add(Models.ProductWrapper pw)
        {
            pw.clients = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUserClientGroupClients;
            List<ECN_Framework_Entities.Communicator.Group> groups = LoadGroups(pw.clientID);

            if (pw.selectedGroupsList != null)
            {
                foreach (ECN_Framework_Entities.Communicator.Group g in groups)
                {
                    if (pw.selectedGroupsList.ToList().Exists(x => x == g.GroupID.ToString()))
                        pw.selectedGroups.Add(g);
                }

                foreach (var item in pw.selectedGroupsList)
                {
                    groups.RemoveAll(x => x.GroupID.ToString() == item);
                }

            }

            pw.availableGroups = groups;

            List<FrameworkUAD.Entity.Brand> brands = new FrameworkUAD.BusinessLogic.Brand().Select(ClientConnections);

            if (pw.selectedBrandsList != null)
            {
                foreach (FrameworkUAD.Entity.Brand b in brands)
                {

                    if (pw.selectedBrandsList.ToList().Exists(x => x == b.BrandID.ToString()))
                        pw.selectedBrands.Add(b);
                }

                foreach (var item in pw.selectedBrandsList)
                {
                    brands.RemoveAll(x => x.BrandID.ToString() == item);
                }
            }

            pw.availableBrands = brands;

            try
            {
                FrameworkUAD.Entity.Product p = pw.pub;
                p.DateCreated = DateTime.Now;
                p.CreatedByUserID = CurrentUser.UserID;
                var productID = new FrameworkUAD.BusinessLogic.Product().Save(p, ClientConnections);

                if (productID != 0)
                {
                    new FrameworkUAD.BusinessLogic.ProductGroup().Delete(ClientConnections, productID);
                    foreach (var item in pw.selectedGroupsList)
                    {
                        FrameworkUAD.Entity.ProductGroup pg = new FrameworkUAD.Entity.ProductGroup();
                        pg.PubID = productID;
                        pg.GroupID = Convert.ToInt32(item);
                        new FrameworkUAD.BusinessLogic.ProductGroup().Save(pg, ClientConnections);
                    }

                    SaveBrandDetails(pw, productID);
                }
            }
            catch (UADException ex)
            {
                pw.Errors = ex.ErrorList.ToList();
                return View(pw);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult GetGroups(int key)
        {
            List<ECN_Framework_Entities.Communicator.Group> groups = LoadGroups(key);
            return Json(groups, JsonRequestBehavior.AllowGet);
        }

        private List<ECN_Framework_Entities.Communicator.Group> LoadGroups(int clientID)
        {
            int CustomerID = ECN_Framework_BusinessLayer.Accounts.Customer.GetByClientID(clientID, false).CustomerID;
            return ECN_Framework_BusinessLayer.Communicator.Group.GetByCustomerID(CustomerID, CurrentUser, "");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!KM.Platform.User.IsSystemAdministrator(CurrentUser))
            {
                Response.Redirect("../SecurityAccessError.aspx");
            }

            if (id > 0)
            {
                Models.ProductWrapper Model = new Models.ProductWrapper();
                List<KMPlatform.Entity.Client> clients = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUserClientGroupClients;
                FrameworkUAD.Entity.Product pub = new FrameworkUAD.BusinessLogic.Product().Select(id, ClientConnections);
                Model.pub = pub;
                Model.clients = clients;
                Model.clientID = pub.GroupID;
                List<ECN_Framework_Entities.Communicator.Group> groups = new List<ECN_Framework_Entities.Communicator.Group>();
                //Todo - Latha
                List<PubGroups> pg =  PubGroups.Get(ClientConnections, pub.PubID);
                List<string> sIDs = new List<string>();

                if (pg != null && pg.Count > 0)
                {
                    ECN_Framework_Entities.Communicator.Group grp = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(pg.First().GroupID, CurrentUser);
                    groups = ECN_Framework_BusinessLayer.Communicator.Group.GetByCustomerID(grp.CustomerID, CurrentUser, ""); 
                    Model.clientID = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(grp.CustomerID, false).PlatformClientID;

                    foreach (ECN_Framework_Entities.Communicator.Group g in groups)
                    {
                        if (pg.Exists(x => x.GroupID == g.GroupID))
                            Model.selectedGroups.Add(g);
                        sIDs.Add(g.GroupID.ToString());
                    }

                    foreach (var item in sIDs)
                    {
                        groups.RemoveAll(x => x.GroupID.ToString() == item);
                    }
                }

                Model.selectedGroupsList = sIDs;
                Model.availableGroups = groups;

                List<FrameworkUAD.Entity.Brand> brandList = new FrameworkUAD.BusinessLogic.Brand().Select(ClientConnections);
                List<FrameworkUAD.Entity.Brand> availableBrands = brandList;
                List<FrameworkUAD.Entity.Brand> brands = brandList;
                //Todo - Latha
                List<Brand> bd = Brand.GetByPubID(ClientConnections, pub.PubID);
                List<string> bIDs = new List<string>();

                if (bd != null || bd.Count > 0)
                {
                    foreach (FrameworkUAD.Entity.Brand b in brands)
                    {
                        if (bd.Exists(x => x.BrandID == b.BrandID))
                            Model.selectedBrands.Add(b);
                        bIDs.Add(b.BrandID.ToString());
                    }

                    foreach (var item in bIDs)
                    {
                        brands.RemoveAll(x => x.BrandID.ToString() == item);
                    }
                }

                Model.selectedBrandsList = bIDs;
                Model.availableBrands = brands;

                return View(Model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Edit(Models.ProductWrapper pw)
        {
            try
            {
                if (!pw.pub.IsActive)
                {
                    NameValueCollection nvc = Pubs.ValidationForDeleteorInActive(ClientConnections, pw.pub.PubID);

                    if (nvc.Count > 0)
                    {
                        string errorMsg = "The selected pub is being used in the following filters, Brand, DownloadTemplates or CrossTab Reports and cannot be updated as inactive  until all prior uses are previously deleted.</ br ></ br > ";

                        var items = nvc.AllKeys.SelectMany(nvc.GetValues, (k, v) => new { key = k, value = v });
                        foreach (var item in items)
                        {
                            errorMsg += item.key + " " + item.value + "</br>";
                        }

                        TempData["error"] = errorMsg;
                        return RedirectToAction("Edit", "Product");
                    }
                }

                FrameworkUAD.Entity.Product p = pw.pub;
                p.DateCreated = DateTime.Now;
                p.CreatedByUserID = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser.UserID;

                int pubID = new FrameworkUAD.BusinessLogic.Product().Save(p, ClientConnections);

                if (pubID != 0)
                {
                    new FrameworkUAD.BusinessLogic.ProductGroup().Delete(ClientConnections, pubID);
                    foreach (var item in pw.selectedGroupsList)
                    {
                        FrameworkUAD.Entity.ProductGroup pg = new FrameworkUAD.Entity.ProductGroup();
                        pg.PubID = pubID;
                        pg.GroupID = Convert.ToInt32(item);
                        new FrameworkUAD.BusinessLogic.ProductGroup().Save(pg, ClientConnections);
                    }

                    SaveBrandDetails(pw, pubID);
                }
            }
            catch (UADException ex)
            {
                pw.Errors = ex.ErrorList.ToList();
                return View(pw);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (!KM.Platform.User.IsSystemAdministrator(CurrentUser))
            {
                Response.Redirect("../SecurityAccessError.aspx");
            }

            try
            {
                NameValueCollection nvc = Pubs.ValidationForDeleteorInActive(ClientConnections, id);

                if (nvc.Count > 0)
                {
                    string errorMsg = "The selected pub is being used in the following filters, Brand, DownloadTemplates or CrossTab Reports and cannot be successfully deleted until all prior uses are previously deleted.</br></br>";

                    var items = nvc.AllKeys.SelectMany(nvc.GetValues, (k, v) => new { key = k, value = v });
                    foreach (var item in items)
                    {
                        errorMsg += item.key + " " + item.value + "</br>";
                    }

                    TempData["error"] = errorMsg;
                    return RedirectToAction("Index");
                }

                KMPS.MD.Objects.Pubs.Delete(ClientConnections, id);
            }
            catch (UADException ex)
            {
                this.SetTempData("UADErrors", ex.ErrorList);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Cancel()
        {
            return RedirectToAction("Index");
        }

        private void SaveBrandDetails(Models.ProductWrapper productWrapper, int pubID)
        {
            BrandDetails.DeleteByPubID(ClientConnections, pubID);
            foreach (var item in productWrapper.selectedBrandsList)
            {
                var brandDetails = new BrandDetails
                {
                    BrandID = int.Parse(item),
                    PubID = pubID
                };
                BrandDetails.Save(ClientConnections, brandDetails);
            }
        }
    }
}