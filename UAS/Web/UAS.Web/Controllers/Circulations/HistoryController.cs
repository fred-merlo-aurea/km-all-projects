using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using FrameworkUAD.BusinessLogic;
using FrameworkUAD_Lookup.BusinessLogic;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using KMPlatform.Entity;
using UAS.Web.Models.Circulations;
using Action = FrameworkUAD_Lookup.BusinessLogic.Action;
using FinalizeBatch = FrameworkUAD.BusinessLogic.FinalizeBatch;
using History = UAS.Web.Models.Circulations.History;
using Product = KMPlatform.Object.Product;

namespace UAS.Web.Controllers.Circulations
{
    public class HistoryController : Common.BaseController
    {
        private string HistoryActiveTab
        {
            get
            {
                if (Session["HistoryActiveTab"] == null)
                {
                    Session["HistoryActiveTab"] = "Open";
                    return "Open";
                }
                else
                    return (string) Session["HistoryActiveTab"];
            }
            set
            {
                Session["HistoryActiveTab"] = value;
            }
        }
        private Dictionary<int, string> Users
        {
            get
            {
                if (Session["HistoryControllerUsers"] == null)
                {
                    //KMPlatform.BusinessLogic.User uWrk = new KMPlatform.BusinessLogic.User();
                    //var users = uWrk.SelectByClientID(CurrentClient.ClientID);
                    //Users = new Dictionary<int, string>();
                    //users.ForEach(x => {
                    //    if (!Users.ContainsKey(x.UserID))
                    //        Users.Add(x.UserID, x.UserName);
                    //});
                    //Session["FileHistoryPubCodes"] = Users;
                    return new Dictionary<int, string>();
                }
                else
                    return (Dictionary<int, string>) Session["HistoryControllerUsers"];
            }
            set
            {
                Session["HistoryControllerUsers"] = value;
            }
        }
        private Dictionary<int, string> Products
        {
            get
            {
                if (Session["HistoryControllerPubCodes"] == null)
                {
                    FrameworkUAD.BusinessLogic.Product productWrk = new FrameworkUAD.BusinessLogic.Product();
                    List<FrameworkUAD.Entity.Product> products = new List<FrameworkUAD.Entity.Product>();
                    KMPlatform.BusinessLogic.Client clientWrk = new KMPlatform.BusinessLogic.Client();
                    KMPlatform.Entity.Client client = clientWrk.Select(CurrentClientID, false);
                    products = productWrk.Select(client.ClientConnections, false).OrderBy(x => x.PubCode).ToList();
                    Dictionary<int, string> pubs = new Dictionary<int, string>();
                    products.ForEach(x =>
                    {
                        if (!pubs.ContainsKey(x.PubID))
                            pubs.Add(x.PubID, x.PubCode);
                    });
                    Session["HistoryControllerPubCodes"] = pubs;
                    return pubs;
                }
                else
                    return (Dictionary<int, string>) Session["HistoryControllerPubCodes"];
            }
            set
            {
                Session["HistoryControllerPubCodes"] = value;
            }
        }
        // GET: History
        [HttpGet]
        public ActionResult Index()
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST, KMPlatform.Enums.Access.FullAccess))
            {
                //if (KM.Platform.User.HasServiceFeature(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST))
                //{

                Users = new Dictionary<int, string>();

                #region BusinessLogic
                KMPlatform.BusinessLogic.Client clientWorker = new KMPlatform.BusinessLogic.Client();
                FrameworkUAD.BusinessLogic.Product productWorker = new FrameworkUAD.BusinessLogic.Product();
                #endregion
                #region Variables
                //KMPlatform.Entity.Client client = new KMPlatform.Entity.Client();
                //List<KMPlatform.Object.Product> products = new List<KMPlatform.Object.Product>(); //List<FrameworkUAD.Entity.Product>();
                #endregion
                #region GetData
                //client = CurrentClient;//clientWorker.Select(CurrentClientID, false);
                //products = CurrentClient.Products;//productWorker.Select(client.ClientConnections, false);
                #endregion
                #region Format Data
                History h = new History();
                h.ResultsOpen = new UAS.Web.Models.Circulations.BatchHistoryWithName();
                h.ResultsOpen.BatchHistoryName = "OPEN";
                h.ResultsOpen.BatchHistoryIEnum = GetBatchHistoryResults(true);
                h.ResultsFinalized = new UAS.Web.Models.Circulations.BatchHistoryWithName();
                h.ResultsFinalized.BatchHistoryName = "FINALIZED";
                List<BatchHistory> blist = new List<BatchHistory>();
                blist = GetBatchHistoryResults(false);
                h.ResultsFinalized.BatchHistoryIEnum = blist;

                //Add this line to preload Finalized Batches Grid
                //h.ResultsFinalized.BatchHistoryIEnum = GetBatchHistoryResults(false);
                h.Users = new Dictionary<int, string>();
                //Only display distinct users 
                blist.ForEach(x =>
                {
                    if (!h.Users.ContainsKey(x.User.Key))
                        h.Users.Add(x.User.Key, x.User.Value);
                    if (!Users.ContainsKey(x.User.Key))
                        Users.Add(x.User.Key, x.User.Value);
                });

                h.Products = new Dictionary<int, string>();
                if (CurrentClient.Products == null || CurrentClient.Products.Count() == 0)
                {
                    List<FrameworkUAD.Entity.Product> productList = new List<FrameworkUAD.Entity.Product>();
                    productList = productWorker.Select(CurrentClient.ClientConnections, false);
                    productList.ForEach(x =>
                    {
                        KMPlatform.Object.Product p = new KMPlatform.Object.Product(x.PubID, x.PubName, x.PubCode, x.ClientID, x.IsActive, x.AllowDataEntry, x.IsUAD, x.IsCirc, x.UseSubGen);
                        CurrentClient.Products.Add(p);
                    });
                }

                foreach (KMPlatform.Object.Product p in CurrentClient.Products.Where(x => x.IsCirc == true).ToList())
                {
                    if (!h.Products.ContainsKey(p.ProductID))
                        h.Products.Add(p.ProductID, p.ProductCode);
                    if (!Products.ContainsKey(p.ProductID))
                        Products.Add(p.ProductID, p.ProductCode);
                }

                h.Client = new KeyValuePair<int, string>(CurrentClient.ClientID, CurrentClient.DisplayName);
                h.User = new KeyValuePair<int, string>(CurrentUser.UserID, CurrentUser.UserName);
                h.Product = new KeyValuePair<int, string>();
                h.StartDate = DateTime.Now.AddDays(-14); //DateTime.Parse(DateTime.Now.Month.ToString() + "/1/" + DateTime.Now.Year.ToString());
                h.EndDate = DateTime.Now;
                #endregion
                return View(h);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }

        #region Finalize Batches Actions
        [HttpGet]
        public ActionResult SaveReport(int BatchID, string BatchNumber, string PubCode)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST, KMPlatform.Enums.Access.FullAccess))
            {
                //if (KM.Platform.User.HasServiceFeature(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST))
                //{

                KMPlatform.Entity.Client Client = new KMPlatform.Entity.Client();
                Client = CurrentClient;
                FrameworkUAD.Object.FinalizeBatch bhd = null;

                if (PubCode == null)
                {
                    FrameworkUAD.BusinessLogic.FinalizeBatch fbWorker = new FrameworkUAD.BusinessLogic.FinalizeBatch();
                    bhd = fbWorker.SelectBatchId(BatchID, Client.ClientConnections);
                    PubCode = bhd.PublicationCode;
                }

                string filename = string.Format("{0}.csv", "BatchReport" + "_" + PubCode + "_" + BatchNumber);

                string csv = CreateReportCSV(Client.ClientConnections, BatchID, bhd, false, false);
                return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", filename);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }

        [HttpGet]
        public ActionResult PrintReport(int BatchID, string BatchNumber, string PubCode)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST, KMPlatform.Enums.Access.FullAccess))
            {
                //if (KM.Platform.User.HasServiceFeature(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST))
                //{
                if (PubCode == null)
                {
                    //KMPlatform.BusinessLogic.Client clientWorker = new KMPlatform.BusinessLogic.Client();
                    KMPlatform.Entity.Client Client = new KMPlatform.Entity.Client();
                    Client = CurrentClient; //clientWorker.Select(CurrentClientID, false);
                    FrameworkUAD.BusinessLogic.FinalizeBatch fbWorker = new FrameworkUAD.BusinessLogic.FinalizeBatch();
                    FrameworkUAD.Object.FinalizeBatch bhd = new FrameworkUAD.Object.FinalizeBatch();
                    bhd = fbWorker.SelectBatchId(BatchID, Client.ClientConnections);
                    PubCode = bhd.PublicationCode;
                }

                string filename = string.Format("{0}.csv", "BatchReport" + "_" + PubCode + "_" + BatchNumber);

                return File(new System.Text.UTF8Encoding().GetBytes("word"), "text/csv", filename);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }

        [HttpGet]
        public ActionResult SaveDetail(int BatchID, string BatchNumber, string PubCode)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST, KMPlatform.Enums.Access.FullAccess))
            {
                //if (KM.Platform.User.HasServiceFeature(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST))
                //{
                string filename = string.Format("{0}.csv", "BatchDetail" + "_" + PubCode + "_" + BatchNumber);

                string csv = CreateDetailsCSV(BatchID, UserID, false, false);
                return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", filename);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }

        [HttpGet]
        public ActionResult PrintDetail(int BatchID, string BatchNumber, string PubCode)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST, KMPlatform.Enums.Access.FullAccess))
            {
                //if (KM.Platform.User.HasServiceFeature(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST))
                //{
                string filename = string.Format("{0}.csv", "BatchReport" + "_" + PubCode + "_" + BatchNumber);

                return File(new System.Text.UTF8Encoding().GetBytes("word"), "text/csv", filename);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }
        #endregion

        #region Open Batches Actions
        [HttpGet]
        public ActionResult FinalizeBatch(int BatchID, string BatchNumber, string PubCode)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST, KMPlatform.Enums.Access.FullAccess))
            {
                //if (KM.Platform.User.HasServiceFeature(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST))
                //{
                #region BusinessLogic
                KMPlatform.BusinessLogic.Client clientWorker = new KMPlatform.BusinessLogic.Client();
                FrameworkUAD.BusinessLogic.Batch batchWorker = new FrameworkUAD.BusinessLogic.Batch();
                FrameworkUAD.BusinessLogic.FinalizeBatch fbWorker = new FrameworkUAD.BusinessLogic.FinalizeBatch();
                #endregion
                #region Variables
                KMPlatform.Entity.Client Client = new KMPlatform.Entity.Client();
                FrameworkUAD.Object.FinalizeBatch bhd = new FrameworkUAD.Object.FinalizeBatch();
                #endregion
                #region Execute
                Client = clientWorker.Select(CurrentClientID, false);
                batchWorker.FinalizeBatchID(BatchID, Client.ClientConnections);
                bhd = fbWorker.SelectBatchId(BatchID, Client.ClientConnections);
                #endregion
                #region Setup Model For View
                UAS.Web.Models.Circulations.BatchHistory bh = new BatchHistory();
                bh.BatchID = bhd.BatchID;
                bh.BatchNumber = bhd.BatchNumber.ToString();
                bh.Product = new KeyValuePair<int, string>(bhd.ProductID, bhd.PublicationCode);
                #endregion
                return PartialView("_FinalizeBatchDialog", bh);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }
        #endregion

        #region Grid Refreshes
        [HttpPost]
        public ActionResult FinalBatchHistory(History h)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST, KMPlatform.Enums.Access.FullAccess))
            {
                //if (KM.Platform.User.HasServiceFeature(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST))
                //{
                ReturnArgs r = new ReturnArgs();
                BatchHistoryWithName bhwn = new BatchHistoryWithName();
                List<BatchHistory> bhl = new List<BatchHistory>();
                //List<BatchHistory> finalbhl = new List<BatchHistory>();
                int batchNumber = 0;
                int.TryParse(h.BatchNumber, out batchNumber);
                bhl = GetBatchHistoryResults(false, batchNumber, h.UserId, h.ProductCode, h.StartDate, h.EndDate);
                //Only display distinct users 
                if (h.Users == null)
                    h.Users = new Dictionary<int, string>();
                foreach (var x in bhl)
                {
                    if (!h.Users.ContainsKey(x.User.Key))
                        h.Users.Add(x.User.Key, x.User.Value);
                    if (!Users.ContainsKey(x.User.Key))
                        Users.Add(x.User.Key, x.User.Value);
                }
                bhwn.Users = Users.ToList();
                //if (h.BatchNumber != null && string.IsNullOrEmpty(h.BatchNumber) != false)
                //    bhl = bhl.FindAll(x => x.BatchNumber == h.BatchNumber);

                //if (h.Username != null && string.IsNullOrEmpty(h.Username) != false)
                //    bhl = bhl.FindAll(x => x.UserName == h.Username);

                //if (h.Product != null && string.IsNullOrEmpty(h.Product) != false)
                //    bhl = bhl.FindAll(x => x.Product == h.Product);

                //if (h.StartDate != null)
                //    bhl = bhl.FindAll(x => x.DateCreated.Date >= h.StartDate);

                //if (h.EndDate != null)
                //    bhl = bhl.FindAll(x => x.DateCreated.Date <= h.EndDate);

                bhwn.BatchHistoryIEnum = bhl;
                bhwn.BatchHistoryName = "FINALIZED";
                r.ViewString = this.RenderViewToString("~/Views/History/_FinalBatchResults.cshtml", bhwn);//~/Views/History/_FinalBatchResults.cshtml   ///_FinalBatchResults
                r.Users = bhwn.Users;
                return Json(r);
                //return PartialView("_FinalBatchResults", bhwn);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }
        protected string RenderViewToString(string viewName, object model)
        {

            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }

        }
        [HttpPost]
        public ActionResult UpdateUserList(History h)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST, KMPlatform.Enums.Access.FullAccess))
            {
                //if (KM.Platform.User.HasServiceFeature(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST))
                //{
                //Only display distinct users 
                //if (h.Users == null)
                //    h.Users = new Dictionary<int, string>();
                //foreach (var x in bhl)
                //{
                //    if (!h.Users.ContainsKey(x.User.Key))
                //        h.Users.Add(x.User.Key, x.User.Value);
                //    if (!Users.ContainsKey(x.User.Key))
                //        Users.Add(x.User.Key, x.User.Value);
                //}

                if (h.Products == null)
                    h.Products = new Dictionary<int, string>();
                foreach (KMPlatform.Object.Product p in CurrentClient.Products.Where(x => x.IsCirc == true).ToList())
                {
                    if (!h.Products.ContainsKey(p.ProductID))
                        h.Products.Add(p.ProductID, p.ProductCode);
                    if (!Products.ContainsKey(p.ProductID))
                        Products.Add(p.ProductID, p.ProductCode);
                }

                // UpdateModel(h);

                //History hx = (History) ViewData.Model;
                //hx.Users = h.Users;
                //ViewData.Model = h;
                return View(h);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }
        [HttpPost]
        public ActionResult OpenBatchHistory(List<BatchHistory> list)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST, KMPlatform.Enums.Access.FullAccess))
            {
                //if (KM.Platform.User.HasServiceFeature(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST))
                //{
                BatchHistoryWithName bhwn = new BatchHistoryWithName();
                List<BatchHistory> bhl = new List<BatchHistory>();
                bhl = GetBatchHistoryResults(true);
                bhwn.BatchHistoryIEnum = bhl;
                bhwn.BatchHistoryName = "OPEN";
                return PartialView("_OpenBatchResults", bhwn);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }
        #endregion

        #region Report Printing and Grid Setup For Report Printing
        [HttpGet]
        public ActionResult PrintWindow(int BatchID)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST, KMPlatform.Enums.Access.FullAccess))
            {
                //if (KM.Platform.User.HasServiceFeature(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST))
                //{
                ViewBag.BatchID = BatchID;
                DataTable dt = CreateReport(BatchID);
                return PartialView("_PrintWindow", dt);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }

        [HttpGet]
        public ActionResult PrintWindowDetail(int BatchID)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST, KMPlatform.Enums.Access.FullAccess))
            {
                //if (KM.Platform.User.HasServiceFeature(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST))
                //{
                ViewBag.BatchID = BatchID;
                DataTable dt = CreateDetail(BatchID);

                return PartialView("_PrintWindowDetail", dt);
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request, int BatchID)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST, KMPlatform.Enums.Access.FullAccess))
            {
                //if (KM.Platform.User.HasServiceFeature(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST))
                //{

                int batchID = BatchID;
                //if (BatchID != null)
                //    batchID = BatchID;

                DataTable dt = CreateReport(batchID);

                return Json(dt.ToDataSourceResult(request));
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }

        public ActionResult GetDetail([DataSourceRequest] DataSourceRequest request, int BatchID)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST, KMPlatform.Enums.Access.FullAccess))
            {
                //if (KM.Platform.User.HasServiceFeature(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST))
                //{
                int batchID = BatchID;
                //if (BatchID != null)
                //    batchID = BatchID;

                DataTable dt = CreateDetail(batchID);

                return Json(dt.ToDataSourceResult(request));
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }
        #endregion

        public List<BatchHistory> GetBatchHistoryResults(bool isOpenBatch, int batchNumber = 0, int userId = 0, string productCode = "", DateTime? startDate = null, DateTime? endDate = null)
        {

            //isOpenBatch = true (Open Batches tab) isOpenBatch = false (Finalized Batches tab)
            #region BusinessLogic
            //KMPlatform.BusinessLogic.Client clientWorker = new KMPlatform.BusinessLogic.Client();
            FrameworkUAD.BusinessLogic.FinalizeBatch worker = new FrameworkUAD.BusinessLogic.FinalizeBatch();
            #endregion
            #region Variables
            // KMPlatform.Entity.Client client = new KMPlatform.Entity.Client();
            List<FrameworkUAD.Object.FinalizeBatch> listBHD = new List<FrameworkUAD.Object.FinalizeBatch>();
            List<BatchHistory> bhl = new List<BatchHistory>();
            #endregion
            #region GetData
            //client = CurrentClient;//clientWorker.Select(CurrentClientID, false);
            //int userID = 0;
            //if (isOpenBatch)
            //    userID = UserID;

            //isOpenBatch if true then IsDateFinalized = false and UserId = current logged in user

            //isOpenBatch if false then IsDateFinalized = true and can pass any of the search criteria
            //h.StartDate = DateTime.Now.AddDays(-1);
            //h.EndDate = DateTime.Now;

            //open batches should just return
            //finalized Batches - need a date range
            if (batchNumber > 0)
            {
                listBHD.Add(worker.SelectBatchNumber(batchNumber, CurrentClient.ClientConnections));
            }
            else
            {
                if (isOpenBatch)
                    listBHD = worker.SelectUser(UserID, CurrentClient.ClientConnections, true);//SelectBatchUserName(userID, client.ClientConnections, client.ClientID, client.DisplayName, isOpenBatch, startDate, endDate, batchNumber, userName, productCode);//pass IsDateFinalized, StartDate, EndDate
                else
                {
                    if (startDate == null)
                        startDate = DateTime.Now.AddDays(-14);//DateTime.Parse(DateTime.Now.Month.ToString() + "/1/" + DateTime.Now.Year.ToString());
                    if (endDate == null)
                        endDate = DateTime.Now;
                    listBHD = worker.SelectDateRange(startDate.Value, endDate.Value, CurrentClient.ClientConnections);
                }
            }

            if (listBHD.Count > 0)
            {
                if (userId > 0) //if (!string.IsNullOrEmpty(userName))
                    listBHD = listBHD.Where(x => x.UserID == userId).ToList(); //x.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase)).ToList();

                if (!string.IsNullOrEmpty(productCode) && !productCode.Equals("-- Select --", StringComparison.CurrentCultureIgnoreCase))
                    listBHD = listBHD.Where(x => x.PublicationCode.Equals(productCode, StringComparison.CurrentCultureIgnoreCase)).ToList();

                if (isOpenBatch)
                    listBHD = listBHD.Where(x => x.DateFinalized.HasValue == false).ToList();
                else
                    listBHD = listBHD.Where(x => x.DateFinalized.HasValue == true).ToList();
            }
            #endregion
            #region Format Final Data
            //Get Final Batch History
            foreach (FrameworkUAD.Object.FinalizeBatch fb in listBHD)
            {
                BatchHistory bh = new BatchHistory();
                bh.BatchID = fb.BatchID;
                bh.User = new KeyValuePair<int, string>(fb.UserID, fb.UserName);
                bh.Product = new KeyValuePair<int, string>(fb.ProductID, fb.PublicationCode);
                bh.BatchNumber = fb.BatchNumber.ToString();
                if (fb.DateCreated != null)
                    bh.DateCreated = (DateTime) fb.DateCreated;
                else
                    bh.DateCreated = DateTime.Now;

                if (fb.DateFinalized.HasValue == true)
                    bh.DateFinalized = (DateTime) fb.DateFinalized;

                bh.BatchCount = fb.LastCount.ToString();

                bhl.Add(bh);
            }
            #endregion
            return bhl;
        }

        #region Reports/Downloads
        #region Report Save
        public string CreateReportCSV(KMPlatform.Object.ClientConnections clientConnections, int batchID, FrameworkUAD.Object.FinalizeBatch bhd, bool download, bool PassUserID = true)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST, KMPlatform.Enums.Access.FullAccess))
            {
                //if (KM.Platform.User.HasServiceFeature(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST))
                //{
                if (bhd == null)
                {
                    FrameworkUAD.BusinessLogic.FinalizeBatch fbWorker = new FrameworkUAD.BusinessLogic.FinalizeBatch();
                    bhd = fbWorker.SelectBatchId(batchID, clientConnections);
                }

                StringBuilder s = new StringBuilder();
                if (bhd != null)
                {
                    s.AppendLine(" ");
                    s.AppendLine("Batch #: " + bhd.BatchNumber.ToString());
                    s.AppendLine("User Name: " + bhd.UserName);
                    s.AppendLine("Client: " + bhd.ClientName);
                    s.AppendLine("Product: " + bhd.PublicationName);
                    s.AppendLine("Date Created: " + bhd.DateCreated.ToString());
                    s.AppendLine("Date Finalized: " + bhd.DateFinalized.ToString());
                    s.AppendLine("Batch Count: " + bhd.LastCount.ToString());
                }

                return s.ToString();
            }
            else
            {
                return "Unauthorized to perform the action";
            }
        }
        #endregion
        #region Report Print
        public DataTable CreateReport(int batchID)
        {

            int BatchID = batchID;
            DataTable me = new DataTable();
            string csv = "";

            FrameworkUAD.BusinessLogic.FinalizeBatch fbWorker = new FrameworkUAD.BusinessLogic.FinalizeBatch();
            FrameworkUAD.Object.FinalizeBatch bhd = new FrameworkUAD.Object.FinalizeBatch();

            KMPlatform.BusinessLogic.Client clientWorker = new KMPlatform.BusinessLogic.Client();
            KMPlatform.Entity.Client myClient = new KMPlatform.Entity.Client();
            myClient = clientWorker.Select(CurrentClientID, false);
            bhd = fbWorker.SelectBatchId(BatchID, myClient.ClientConnections);

            me.Columns.Add(new DataColumn("Report", typeof(String)));

            if (bhd != null)
            {
                me.Rows.Add("Batch #: " + bhd.BatchNumber.ToString());
                me.Rows.Add("User Name: " + bhd.UserName);
                me.Rows.Add("Client: " + bhd.ClientName);
                me.Rows.Add("Product: " + bhd.PublicationName);
                me.Rows.Add("Date Created: " + bhd.DateCreated.ToString());
                me.Rows.Add("Date Finalized: " + bhd.DateFinalized.ToString());
                me.Rows.Add("Batch Count: " + bhd.LastCount.ToString());
            }

            return me;
        }
        #endregion
        #region Detail Save
        public string CreateDetailsCSV(int batchId, int user, bool download, bool passUserId = true)
        {
            var codeWorker = new Code();

            var info = new CsvDetailsInfo
            {
                CodeType = new CodeType().Select(FrameworkUAD_Lookup.Enums.CodeType.Deliver),
                Actions = new Action().Select(),
                CategoryCodes = new CategoryCode().Select(),
                CategoryCodeTypes = new CategoryCodeType().Select(),
                TransactionCodes = new TransactionCode().Select(),
                SubscriptionStatuses = new SubscriptionStatus().Select(),
                Countries = new Country().Select(),
                Regions = new Region().Select(),
                QualificationSources = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Qualification_Source),
                ParList = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Par3c),
                DeliverList = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Deliver),
                SubscriberSources = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Subscriber_Source),
                Adresses = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Address),
                MarketingList = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Marketing)
            };

            var client = CurrentClient;
            var details = CreateDetaildDataTable(batchId, client);

            var batchList = new BatchHistoryDetail().SelectBatchID(false, client.ClientConnections, client.DisplayName, batchId);

            var isSameRow = false;
            var listValue = new List<string>();

            var uadProduct = client.Products;

            foreach (var batchGroupedById in batchList.GroupBy(d => d.BatchID))
            {
                foreach (var batchGroupedBySubscriptionId in batchGroupedById.GroupBy(k => k.HistorySubscriptionID))
                {
                    foreach (var batchGroupedByObject in batchGroupedBySubscriptionId.GroupBy(k => k.Object))
                    {
                        foreach (var historyDetail in batchGroupedByObject)
                        {
                            const string productSubscriptionName = "ProductSubscriptionDetail";
                            if (historyDetail.Object == null || string.IsNullOrWhiteSpace(historyDetail.ToObjectValues) && !historyDetail.Object.Equals(productSubscriptionName))
                            {
                                continue;
                            }

                            FillCsvDetailsListValue(uadProduct, historyDetail, client, info, ref isSameRow, listValue);
                        }
                    }

                    FillDetailsDataTable(listValue, details);
                    isSameRow = false;
                }
            }

            return ReturnDetailString(details);
        }

        private void FillDetailsDataTable(ICollection<string> listValue, DataTable details)
        {
            if (listValue.Any())
            {
                var i = 0;
                var values = new string[listValue.Count];
                foreach (var value in listValue)
                {
                    values[i] = value;
                    i++;
                    if (i > details.Columns.Count)
                    {
                        const int headerLinesCount = 4;
                        var columnNumber = i - headerLinesCount;
                        details.Columns.Add($"Edit {columnNumber}", typeof(string));
                    }
                }

                details.Rows.Add(values);
            }

            listValue.Clear();
        }

        private void FillCsvDetailsListValue(
            IReadOnlyCollection<Product> uadProduct, 
            FrameworkUAD.Object.BatchHistoryDetail historyDetail, 
            Client client, 
            CsvDetailsInfo info, 
            ref bool isSameRow, 
            ICollection<string> listValue)
        {
            var questions = new List<FrameworkUAD.Entity.ResponseGroup>();
            var answers = new List<FrameworkUAD.Entity.CodeSheet>();

            if (uadProduct != null && uadProduct.Any())
            {
                var prodId = uadProduct.Single(p => p.ProductCode == historyDetail.PubCode).ProductID;

                questions = new ResponseGroup()
                    .Select(prodId, client.ClientConnections)
                    .Where(q => q.IsActive == true)
                    .OrderBy(q => q.DisplayOrder)
                    .ThenBy(q => q.DisplayName)
                    .ToList();

                answers = new CodeSheet()
                    .Select(prodId, client.ClientConnections)
                    .Where(a => a.IsActive == true)
                    .ToList();
            }

            var history = JsonComparer(
                historyDetail.Object, 
                historyDetail.FromObjectValues, 
                historyDetail.ToObjectValues,
                info.CodeType,
                info.Actions,
                info.CategoryCodes,
                info.CategoryCodeTypes,
                info.TransactionCodes,
                info.SubscriptionStatuses,
                info.QualificationSources,
                info.ParList,
                info.DeliverList,
                info.SubscriberSources,
                info.Adresses,
                info.MarketingList,
                info.Countries,
                info.Regions, 
                questions, 
                answers);

            if (!isSameRow)
            {
                listValue.Add($"Subscriber: {historyDetail.FirstName} {historyDetail.LastName}");
                listValue.Add($"Sequence #: {historyDetail.SequenceID}");
                listValue.Add($"SubscriptionID: {historyDetail.SubscriptionID}");
                listValue.Add($"Date Updated: {historyDetail.UserLogDateCreated}");
                listValue.Add(string.Empty);

                isSameRow = true;
            }

            foreach (var historyItem in history.OrderBy(h => h.SortIndex))
            {
                listValue.Add($"{historyItem.PropertyName}: {historyItem.DisplayText}");
            }
        }

        private DataTable CreateDetaildDataTable(int batchId, Client client)
        {
            var finalizeBatchWorker = new FinalizeBatch();
            var finalizeBatch = finalizeBatchWorker.SelectBatchId(batchId, client.ClientConnections);

            var details = new DataTable();

            details.Columns.Add(new DataColumn(" ", typeof(string)));
            details.Columns.Add(new DataColumn("  "));
            details.Columns.Add(new DataColumn("   "));
            details.Columns.Add(new DataColumn("    "));

            details.Rows.Add($"Batch #: {finalizeBatch.BatchNumber}");
            details.Rows.Add($"User Name: {finalizeBatch.UserName}");
            details.Rows.Add($"Client: {finalizeBatch.ClientName}");
            details.Rows.Add($"Product: {finalizeBatch.PublicationName}");
            details.Rows.Add($"Date Created: {finalizeBatch.DateCreated}");
            details.Rows.Add($"Date Finalized: {finalizeBatch.DateFinalized}");
            details.Rows.Add($"Batch Count: {finalizeBatch.LastCount}");

            details.Rows.Add("");

            return details;
        }

        public string ReturnDetailString(DataTable dt)
        {
            #region Variables
            char delim = ',';
            StringBuilder sb = new StringBuilder();

            #endregion
            #region Add Headers
            if (dt != null)
            {
                foreach (DataColumn drO in dt.Columns)
                {
                    //create the headers
                    sb.Append(drO.ColumnName + delim);
                }
                sb.AppendLine();
            }
            #endregion
            #region Add Data
            foreach (DataColumn dc in dt.Columns)
            {
                if (Type.GetTypeCode(dc.DataType) == TypeCode.String && dc.ReadOnly == false)
                    dc.ColumnName = dc.ColumnName.ToString().Replace('\r', ' ').Replace('\n', ' ');
            }

            foreach (DataRow otl in dt.Rows)
            {
                //create the orginal valid file
                sb.Append(string.Join(delim.ToString(), otl.ItemArray.Select(p => p.ToString().Replace('\r', ' ').Replace('\n', ' ').Trim().TrimEnd('\r', '\n')).ToArray()));
                sb.AppendLine();
            }
            #endregion

            return sb.ToString();
        }
        #endregion
        #region Detail Print
        public DataTable CreateDetail(int batchID)
        {
            int BatchID = batchID;
            DataTable details = new DataTable();
            List<HistoryData> history = new List<HistoryData>();
            StringBuilder sb = new StringBuilder();

            FrameworkUAD.BusinessLogic.FinalizeBatch fbWorker = new FrameworkUAD.BusinessLogic.FinalizeBatch();
            FrameworkUAD.Object.FinalizeBatch bhd = new FrameworkUAD.Object.FinalizeBatch();

            KMPlatform.BusinessLogic.Client clientWorker = new KMPlatform.BusinessLogic.Client();
            KMPlatform.Entity.Client myClient = new KMPlatform.Entity.Client();
            myClient = clientWorker.Select(CurrentClientID, false);
            bhd = fbWorker.SelectBatchId(BatchID, myClient.ClientConnections);

            List<FrameworkUAD.Object.BatchHistoryDetail> listBHD = new List<FrameworkUAD.Object.BatchHistoryDetail>();
            FrameworkUAD.BusinessLogic.BatchHistoryDetail bhdWorker = new FrameworkUAD.BusinessLogic.BatchHistoryDetail();
            listBHD = bhdWorker.SelectBatchID(UserID, false, myClient.ClientConnections, myClient.DisplayName, BatchID);

            details.Columns.Add(new DataColumn("Detail1", typeof(String)));
            details.Columns.Add(new DataColumn("Detail2", typeof(String)));
            details.Columns.Add(new DataColumn("Detail3", typeof(String)));
            details.Columns.Add(new DataColumn("Detail4", typeof(String)));

            details.Rows.Add("Batch #: " + bhd.BatchNumber.ToString());
            details.Rows.Add("User Name: " + bhd.UserName);
            details.Rows.Add("Client: " + bhd.ClientName);
            details.Rows.Add("Product: " + bhd.PublicationName);
            details.Rows.Add("Date Created: " + bhd.DateCreated.ToString());
            details.Rows.Add("Date Finalized: " + bhd.DateFinalized.ToString());
            details.Rows.Add("Batch Count: " + bhd.LastCount.ToString());

            var batchNum = listBHD.GroupBy(b => b.BatchID).Select(x => x.Key).ToList();
            bool isSameRow = false;
            List<string> listValue = new List<string>();

            List<FrameworkUAD.Entity.Product> UADProduct = new List<FrameworkUAD.Entity.Product>();
            FrameworkUAD.BusinessLogic.Product productWorker = new FrameworkUAD.BusinessLogic.Product();
            UADProduct = productWorker.Select(myClient.ClientConnections);

            List<int> singleAnswer = new List<int>();
            List<int> respId = new List<int>();
            #region Get Data
            FrameworkUAD_Lookup.Entity.CodeType codeTypeVar = new FrameworkUAD_Lookup.Entity.CodeType();
            FrameworkUAD_Lookup.BusinessLogic.CodeType ctWorker = new FrameworkUAD_Lookup.BusinessLogic.CodeType();
            codeTypeVar = ctWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Deliver);

            List<FrameworkUAD_Lookup.Entity.Action> action = new List<FrameworkUAD_Lookup.Entity.Action>();
            FrameworkUAD_Lookup.BusinessLogic.Action aWorker = new FrameworkUAD_Lookup.BusinessLogic.Action();
            action = aWorker.Select();

            List<FrameworkUAD_Lookup.Entity.CategoryCode> cat = new List<FrameworkUAD_Lookup.Entity.CategoryCode>();
            FrameworkUAD_Lookup.BusinessLogic.CategoryCode ccWorker = new FrameworkUAD_Lookup.BusinessLogic.CategoryCode();
            cat = ccWorker.Select();

            List<FrameworkUAD_Lookup.Entity.CategoryCodeType> catType = new List<FrameworkUAD_Lookup.Entity.CategoryCodeType>();
            FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType cctWorker = new FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType();
            catType = cctWorker.Select();

            List<FrameworkUAD_Lookup.Entity.TransactionCode> trans = new List<FrameworkUAD_Lookup.Entity.TransactionCode>();
            FrameworkUAD_Lookup.BusinessLogic.TransactionCode tcWorker = new FrameworkUAD_Lookup.BusinessLogic.TransactionCode();
            trans = tcWorker.Select();

            List<FrameworkUAD_Lookup.Entity.SubscriptionStatus> sStatus = new List<FrameworkUAD_Lookup.Entity.SubscriptionStatus>();
            FrameworkUAD_Lookup.BusinessLogic.SubscriptionStatus ssWorker = new FrameworkUAD_Lookup.BusinessLogic.SubscriptionStatus();
            sStatus = ssWorker.Select();

            //List<FrameworkUAD_Lookup.Entity.Code> qSource = new List<FrameworkUAD_Lookup.Entity.Code>();
            //FrameworkUAD_Lookup.BusinessLogic.Code qsWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
            //qSource = qsWorker.Select();

            //List<FrameworkUAD_Lookup.Entity.Code> par = new List<FrameworkUAD_Lookup.Entity.Code>();
            //FrameworkUAD_Lookup.BusinessLogic.Code parWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
            //par = parWorker.Select();

            //List<FrameworkUAD_Lookup.Entity.Code> codeList = new List<FrameworkUAD_Lookup.Entity.Code>();
            //FrameworkUAD_Lookup.BusinessLogic.Code codeWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
            //codeList = codeWorker.Select();

            //List<FrameworkUAD_Lookup.Entity.Code> marketing = new List<FrameworkUAD_Lookup.Entity.Code>();
            //FrameworkUAD_Lookup.BusinessLogic.Code mWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
            //marketing = mWorker.Select();
            FrameworkUAD_Lookup.BusinessLogic.Code codeWorker = new FrameworkUAD_Lookup.BusinessLogic.Code();
            List<FrameworkUAD_Lookup.Entity.Code> qSource = new List<FrameworkUAD_Lookup.Entity.Code>();
            List<FrameworkUAD_Lookup.Entity.Code> par = new List<FrameworkUAD_Lookup.Entity.Code>();
            List<FrameworkUAD_Lookup.Entity.Code> deliver = new List<FrameworkUAD_Lookup.Entity.Code>();
            List<FrameworkUAD_Lookup.Entity.Code> subSource = new List<FrameworkUAD_Lookup.Entity.Code>();
            List<FrameworkUAD_Lookup.Entity.Code> address = new List<FrameworkUAD_Lookup.Entity.Code>();
            List<FrameworkUAD_Lookup.Entity.Code> marketing = new List<FrameworkUAD_Lookup.Entity.Code>();

            qSource = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Qualification_Source);
            par = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Par3c);
            deliver = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Deliver);
            subSource = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Subscriber_Source);
            address = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Address);
            marketing = codeWorker.Select(FrameworkUAD_Lookup.Enums.CodeType.Marketing);

            List<FrameworkUAD_Lookup.Entity.Country> country = new List<FrameworkUAD_Lookup.Entity.Country>();
            FrameworkUAD_Lookup.BusinessLogic.Country countryWorker = new FrameworkUAD_Lookup.BusinessLogic.Country();
            country = countryWorker.Select();

            List<FrameworkUAD_Lookup.Entity.Region> region = new List<FrameworkUAD_Lookup.Entity.Region>();
            FrameworkUAD_Lookup.BusinessLogic.Region regionWorker = new FrameworkUAD_Lookup.BusinessLogic.Region();
            region = regionWorker.Select();

            #endregion

            foreach (var b in batchNum)
            {
                var historySubID = listBHD.Where(x => x.BatchID == b).GroupBy(k => k.HistorySubscriptionID).Select(m => m.Key).ToList();
                foreach (var hs in historySubID)
                {
                    var historyObject = listBHD.Where(x => x.BatchID == b && x.HistorySubscriptionID == hs).GroupBy(k => k.Object).Select(m => m.Key).ToList();
                    foreach (var hObject in historyObject)
                    {
                        foreach (var s in listBHD.Where(h => h.BatchID == b && h.HistorySubscriptionID == hs && h.Object == hObject))
                        {
                            if (s.Object != null && (!string.IsNullOrEmpty(s.ToObjectValues) || s.Object.Equals("ProductSubscriptionDetail")))
                            {
                                List<FrameworkUAD.Entity.ResponseGroup> Questions = new List<FrameworkUAD.Entity.ResponseGroup>();
                                List<FrameworkUAD.Entity.CodeSheet> Answers = new List<FrameworkUAD.Entity.CodeSheet>();
                                if (UADProduct == null || UADProduct.Count == 0)
                                {
                                    int prodId = UADProduct.SingleOrDefault(p => p.PubCode == s.PubCode).PubID;

                                    FrameworkUAD.BusinessLogic.ResponseGroup rtWorker = new FrameworkUAD.BusinessLogic.ResponseGroup();
                                    Questions = rtWorker.Select(prodId, myClient.ClientConnections).Where(x => x.IsActive == true).OrderBy(y => y.DisplayOrder).ThenBy(z => z.DisplayName).ToList();

                                    FrameworkUAD.BusinessLogic.CodeSheet rWorker = new FrameworkUAD.BusinessLogic.CodeSheet();
                                    Answers = rWorker.Select(prodId, myClient.ClientConnections).Where(x => x.IsActive == true).ToList();
                                }

                                history = JsonComparer(s.Object, s.FromObjectValues, s.ToObjectValues, codeTypeVar, action, cat, catType, trans, sStatus, qSource, par, deliver, subSource, address,
                                            marketing, country, region, Questions, Answers);
                                if (isSameRow == false) //We only want to process or add this information at the beginning of each row.
                                {

                                    // Get answers that are the only answers
                                    singleAnswer = (from a in Answers
                                                    group a by a.ResponseGroupID into grp
                                                    where grp.Count() <= 1
                                                    select grp.Key).ToList();

                                    foreach (var s2 in singleAnswer)
                                    {
                                        var Id = (from a in Answers
                                                  where a.ResponseGroupID == s2
                                                  select a.CodeSheetID).SingleOrDefault();
                                        respId.Add(Id);
                                    }
                                    listValue.Add("Subscriber: " + s.FirstName + " " + s.LastName);
                                    listValue.Add("Sequence #: " + s.SequenceID);
                                    listValue.Add("SubscriptionID: " + s.SubscriptionID);
                                    listValue.Add("Date Updated: " + s.UserLogDateCreated);
                                    listValue.Add("");
                                    isSameRow = true;
                                }
                                foreach (var y in history.OrderBy(x => x.SortIndex))
                                {
                                    listValue.Add(y.PropertyName + ": " + y.DisplayText);
                                }
                            }
                        }
                    }
                    if (listValue.Count > 0)
                    {
                        int i = 0;
                        string[] values = new string[listValue.Count];
                        foreach (string s2 in listValue)
                        {
                            values[i] = s2;
                            i++;
                            if (i > details.Columns.Count)
                                details.Columns.Add("Edit" + (i - 4), typeof(String));
                        }
                        details.Rows.Add(values);
                    }
                    listValue.Clear();
                    isSameRow = false;
                }
            }

            foreach (DataRow row in details.Rows)
            {
                foreach (DataColumn col in details.Columns)
                {
                    if (row.IsNull(col) && col.DataType == typeof(string))
                        row.SetField(col, String.Empty);
                }
            }

            return details;
        }
        #endregion
        #endregion

        //#region ECN Methods
        //private int CurrentClientID
        //{
        //    get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().ClientID; }
        //}

        //private int UserID
        //{
        //    get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().UserID; }
        //}
        //#endregion

        #region From Circulation.Helpers
        public class HistoryData
        {
            public int SortIndex { get; set; }
            public string DisplayText { get; set; }
            public string PropertyName { get; set; }

            public HistoryData(int index, string text, string name)
            {
                this.SortIndex = index;
                this.DisplayText = text;
                this.PropertyName = name;
            }
        }
        //be nice to get this into database
        private static List<string> IncludeColumns()
        {
            List<string> filter = new List<string>();
            filter.Add("SequenceID");
            filter.Add("FirstName");
            filter.Add("LastName");
            filter.Add("Title");
            filter.Add("Company");
            filter.Add("AddressTypeCodeId");
            filter.Add("Address1");
            filter.Add("Address2");
            filter.Add("Address3");
            filter.Add("City");
            filter.Add("County");
            filter.Add("CountryID");
            filter.Add("RegionID");
            filter.Add("ZipCode");
            filter.Add("Plus4");
            filter.Add("Phone");
            filter.Add("PhoneExt");
            filter.Add("Mobile");
            filter.Add("Fax");
            filter.Add("Email");
            filter.Add("Website");
            filter.Add("PubCategoryID");
            filter.Add("PubTransactionID");
            filter.Add("MemberGroup");
            filter.Add("SubscriberSourceCode");
            filter.Add("OrigsSrc");
            filter.Add("SubSrcID");
            filter.Add("Demo7");
            filter.Add("Verify");
            filter.Add("PubQSourceID");
            filter.Add("Par3CID");
            filter.Add("Qualificationdate");
            filter.Add("Copies");
            filter.Add("MarketingID");
            filter.Add("MailPermission");
            filter.Add("FaxPermission");
            filter.Add("PhonePermission");
            filter.Add("OtherProductsPermission");
            filter.Add("EmailRenewPermission");
            filter.Add("ThirdPartyPermission");
            filter.Add("TextPermission");
            return filter;
        }
        //type should be an enum then use a switch statement instead of long if/else check
        public static List<HistoryData> JsonComparer(string type, string fromJSON, string toJSON,
               FrameworkUAD_Lookup.Entity.CodeType deliverCodeTypeID,
               List<FrameworkUAD_Lookup.Entity.Action> actionList,
               List<FrameworkUAD_Lookup.Entity.CategoryCode> cat,
               List<FrameworkUAD_Lookup.Entity.CategoryCodeType> catType,
               List<FrameworkUAD_Lookup.Entity.TransactionCode> trans,
               List<FrameworkUAD_Lookup.Entity.SubscriptionStatus> sStatusList,
               List<FrameworkUAD_Lookup.Entity.Code> qSourceList,
               List<FrameworkUAD_Lookup.Entity.Code> parList,
               List<FrameworkUAD_Lookup.Entity.Code> deliver,
               List<FrameworkUAD_Lookup.Entity.Code> subSource,
               List<FrameworkUAD_Lookup.Entity.Code> address,
               List<FrameworkUAD_Lookup.Entity.Code> marketingList,
               List<FrameworkUAD_Lookup.Entity.Country> countryList,
               List<FrameworkUAD_Lookup.Entity.Region> regionList,
               List<FrameworkUAD.Entity.ResponseGroup> questions,
               List<FrameworkUAD.Entity.CodeSheet> answers)
        {
            List<HistoryData> history = new List<HistoryData>();

            bool isWaveMailChange = false;
            string waveMail = "";
            Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
            object jsonOne = null;
            object jsonTwo = null;

            if (type == "ProductSubscription")
            {
                jsonOne = jf.FromJson<FrameworkUAD.Entity.ProductSubscription>(fromJSON);
                jsonTwo = jf.FromJson<FrameworkUAD.Entity.ProductSubscription>(toJSON);

            }
            else if (type == "Subscription")
            {
                jsonOne = jf.FromJson<FrameworkUAD.Entity.Subscription>(fromJSON);
                jsonTwo = jf.FromJson<FrameworkUAD.Entity.Subscription>(toJSON);

            }
            else if (type == "SubscriptionPaid")
            {
                jsonOne = jf.FromJson<FrameworkUAD.Entity.SubscriptionPaid>(fromJSON);
                jsonTwo = jf.FromJson<FrameworkUAD.Entity.SubscriptionPaid>(toJSON);

            }
            else if (type == "PaidBillTo")
            {
                jsonOne = jf.FromJson<FrameworkUAD.Entity.PaidBillTo>(fromJSON);
                jsonTwo = jf.FromJson<FrameworkUAD.Entity.PaidBillTo>(toJSON);

                if (jsonOne == null)
                    jsonOne = new FrameworkUAD.Entity.PaidBillTo();
                if (jsonTwo == null)
                    jsonTwo = new FrameworkUAD.Entity.PaidBillTo();
            }
            else if (type == "MarketingMap")
            {
                jsonOne = jf.FromJson<FrameworkUAD.Entity.MarketingMap>(fromJSON);
                jsonTwo = jf.FromJson<FrameworkUAD.Entity.MarketingMap>(toJSON);

            }
            else if (type == "SubscriptionResponseMap" || type == "ProductSubscriptionDetail") //SubscriptionResponseMap to support old code and logs.
            {
                jsonOne = jf.FromJson<FrameworkUAD.Entity.ProductSubscriptionDetail>(fromJSON);
                jsonTwo = jf.FromJson<FrameworkUAD.Entity.ProductSubscriptionDetail>(toJSON);
            }
            else if (type == "PubSubscriptionAdHoc")
            {
                jsonOne = jf.FromJson<FrameworkUAD.Object.PubSubscriptionAdHoc>(fromJSON);
                jsonTwo = jf.FromJson<FrameworkUAD.Object.PubSubscriptionAdHoc>(toJSON);
            }

            List<string> filter = IncludeColumns();
            #region FROM Obj - TO Obj
            if (jsonOne != null || jsonTwo != null)
            {
                string originalValue = "";
                string newValue = "";
                PropertyInfo[] propertyInfos = new PropertyInfo[120];
                PropertyInfo piWaveMail;
                if (jsonOne != null)
                {
                    propertyInfos = jsonOne.GetType().GetProperties();
                    piWaveMail = propertyInfos.Where(x => x.Name.Equals("IsInActiveWaveMailing")).FirstOrDefault();
                    if (piWaveMail != null)
                        isWaveMailChange = (bool) piWaveMail.GetValue(jsonOne);
                }
                else if (jsonTwo != null)
                {
                    propertyInfos = jsonTwo.GetType().GetProperties();
                    piWaveMail = propertyInfos.Where(x => x.Name.Equals("IsInActiveWaveMailing")).FirstOrDefault();
                    if (piWaveMail != null)
                        isWaveMailChange = (bool) piWaveMail.GetValue(jsonTwo);
                }

                if (type == "SubscriptionResponseMap" || type == "ProductSubscriptionDetail")
                {
                    #region Responses
                    FrameworkUAD.Entity.CodeSheet responseNameFrom = new FrameworkUAD.Entity.CodeSheet();
                    FrameworkUAD.Entity.CodeSheet responseNameTo = new FrameworkUAD.Entity.CodeSheet();
                    FrameworkUAD.Entity.ResponseGroup rgFrom = new FrameworkUAD.Entity.ResponseGroup();
                    FrameworkUAD.Entity.ResponseGroup rgTo = new FrameworkUAD.Entity.ResponseGroup();
                    string responseOtherFrom = "";
                    string responseOtherTo = "";
                    string responseFrom = "";
                    string responseTo = "";
                    PropertyInfo response;

                    if (jsonOne != null)
                    {
                        int responseFromID = Convert.ToInt32(jsonOne.GetType().GetProperty("CodeSheetID").GetValue(jsonOne));
                        propertyInfos = jsonOne.GetType().GetProperties();
                        response = propertyInfos.Where(x => x.Name.Equals("ResponseOther")).FirstOrDefault();
                        if (response != null)
                            responseOtherFrom = (string) response.GetValue(jsonOne);

                        responseNameFrom = (from a in answers
                                            join q in questions on a.ResponseGroupID equals q.ResponseGroupID
                                            where a.CodeSheetID == responseFromID
                                            select a).SingleOrDefault();

                        rgFrom = (from a in answers
                                  join q in questions on a.ResponseGroupID equals q.ResponseGroupID
                                  where a.CodeSheetID == responseFromID && a.IsActive == true && q.IsActive == true
                                  select q).SingleOrDefault();

                        responseFrom = responseNameFrom.ResponseDesc;
                        if (responseOtherFrom.Length > 0)
                            responseFrom += " - " + responseOtherFrom;
                    }
                    if (jsonTwo != null)
                    {
                        int responseToID = Convert.ToInt32(jsonTwo.GetType().GetProperty("CodeSheetID").GetValue(jsonTwo));
                        propertyInfos = jsonTwo.GetType().GetProperties();
                        response = propertyInfos.Where(x => x.Name.Equals("ResponseOther")).FirstOrDefault();
                        if (response != null)
                            responseOtherTo = (string) response.GetValue(jsonTwo);

                        responseNameTo = (from a in answers
                                          join q in questions on a.ResponseGroupID equals q.ResponseGroupID
                                          where a.CodeSheetID == responseToID
                                          select a).SingleOrDefault();

                        rgTo = (from a in answers
                                join q in questions on a.ResponseGroupID equals q.ResponseGroupID
                                where a.CodeSheetID == responseToID && a.IsActive == true && q.IsActive == true
                                select q).SingleOrDefault();

                        responseTo = responseNameTo.ResponseDesc;
                        if (responseOtherTo.Length > 0)
                            responseTo += " - " + responseOtherTo;
                    }

                    if (responseFrom != null || responseTo != null)
                    {
                        string rgName = "";
                        if (rgFrom.DisplayName == "")
                            rgName = rgTo.DisplayName;
                        else
                            rgName = rgFrom.DisplayName;
                        history.Add(new HistoryData(0, responseFrom + " TO " + responseTo, rgName));
                    }
                    #endregion
                }
                else if (type == "SubscriptionPaid")
                {
                    string[] exclude = new string[] { "DateCreated", "DateUpdated", "PubSubscriptionID", "SubscriptionPaidID", "UpdatedByUserID", "CreatedByUserID" };
                    foreach (PropertyInfo p in propertyInfos.Where(x => !exclude.Contains(x.Name)))
                    {
                        if (jsonOne != null)
                            originalValue = p.GetValue(jsonOne).ToString().Trim();
                        if (jsonTwo != null)
                            newValue = p.GetValue(jsonTwo).ToString().Trim();

                        if (!originalValue.Equals(newValue))
                            history.Add(new HistoryData(0, originalValue.Trim() + " TO " + newValue.Trim() + waveMail, p.Name));
                    }
                }
                else
                {
                    foreach (PropertyInfo p in propertyInfos)
                    {
                        int index = 0;
                        bool include = false;
                        string adHoc = "";
                        if (p.Name.Equals("IsActive") && type == "Subscriber")
                            include = false;
                        else
                        {
                            include = filter.Contains(p.Name, StringComparer.CurrentCultureIgnoreCase);
                            index = filter.IndexOf(p.Name);
                        }
                        if (include == true || type == "PubSubscriptionAdHoc")
                        {
                            if (jsonOne != null && jsonTwo != null)
                            {
                                originalValue = (p.GetValue(jsonOne) ?? "").ToString();
                                newValue = (jsonTwo.GetType().GetProperty(p.Name).GetValue(jsonTwo) ?? "").ToString().Trim();
                            }
                            else if (jsonOne != null)
                                originalValue = (p.GetValue(jsonOne) ?? "").ToString();
                            else if (jsonTwo != null)
                                newValue = (p.GetValue(jsonTwo) ?? "").ToString();

                            if (isWaveMailChange)
                                waveMail = " (Wave Mail Change)";

                            if (type == "PubSubscriptionAdHoc")
                                adHoc = " (AdHoc Change)";

                            #region Name specific
                            if (!originalValue.Trim().Equals(newValue))
                            {
                                #region Country
                                if (p.Name.Equals("CountryID"))
                                {
                                    if (originalValue == "")
                                        originalValue = "0";
                                    if (newValue == "")
                                        newValue = "0";
                                    string pcountryListName = countryList.Where(s => s.CountryID == Convert.ToInt32(originalValue)).Select(x => x.ShortName).SingleOrDefault();
                                    string ncountryListName = countryList.Where(s => s.CountryID == Convert.ToInt32(newValue)).Select(x => x.ShortName).SingleOrDefault();

                                    if (type.Equals("Subscriber") || type.Equals("ProductSubscription"))
                                    {
                                        history.Add(new HistoryData(index, pcountryListName + " TO " + ncountryListName + waveMail, "Country"));
                                    }
                                    else if (type.Equals("PaidBillTo"))
                                    {
                                        history.Add(new HistoryData(index, pcountryListName + " TO " + ncountryListName + waveMail, "Bill To - Country"));
                                    }
                                }
                                #endregion
                                #region State
                                else if (p.Name.Equals("RegionID"))
                                {
                                    if (originalValue == "")
                                        originalValue = "0";
                                    if (newValue == "")
                                        newValue = "0";
                                    string pRegionName = regionList.Where(s => s.RegionID == Convert.ToInt32(originalValue)).Select(x => x.RegionName).SingleOrDefault();
                                    string nRegionName = regionList.Where(s => s.RegionID == Convert.ToInt32(newValue)).Select(x => x.RegionName).SingleOrDefault();

                                    if (type.Equals("Subscriber") || type.Equals("ProductSubscription"))
                                    {
                                        history.Add(new HistoryData(index, pRegionName + " TO " + nRegionName + waveMail, "State"));
                                    }
                                    else if (type.Equals("PaidBillTo"))
                                    {
                                        history.Add(new HistoryData(index, pRegionName + " TO " + nRegionName + waveMail, "Bill To - State"));
                                    }
                                }
                                #endregion
                                #region SubscriptionStatusID
                                else if (p.Name.Equals("SubscriptionStatusID"))
                                {
                                    if (originalValue == "")
                                        originalValue = "0";
                                    if (newValue == "")
                                        newValue = "0";
                                    string pStatusName = sStatusList.Where(s => s.SubscriptionStatusID == Convert.ToInt32(originalValue)).Select(x => x.StatusName).SingleOrDefault();
                                    string nStatusName = sStatusList.Where(s => s.SubscriptionStatusID == Convert.ToInt32(newValue)).Select(x => x.StatusName).SingleOrDefault();

                                    history.Add(new HistoryData(index, pStatusName + " TO " + nStatusName + waveMail, "Subscription Status"));
                                }
                                #endregion
                                #region QSource
                                else if (p.Name.Equals("PubQSourceID"))
                                {
                                    if (originalValue == "")
                                        originalValue = "0";
                                    if (newValue == "")
                                        newValue = "0";
                                    string pDisplayName = qSourceList.Where(q => q.CodeId == Convert.ToInt32(originalValue)).Select(x => x.DisplayName).SingleOrDefault();
                                    string nDisplayName = qSourceList.Where(q => q.CodeId == Convert.ToInt32(newValue)).Select(x => x.DisplayName).SingleOrDefault();

                                    history.Add(new HistoryData(index, pDisplayName + " TO " + nDisplayName + waveMail, "Qualification Source"));
                                }
                                #endregion
                                #region QDate
                                else if (p.Name.Equals("QSourceDate"))
                                {
                                    string origDate = string.Empty;
                                    string newDate = string.Empty;
                                    if (!string.IsNullOrEmpty(originalValue))
                                    {
                                        DateTime d = Convert.ToDateTime(originalValue);
                                        origDate = d.Date.ToShortDateString();
                                    }
                                    if (!string.IsNullOrEmpty(newValue))
                                    {
                                        DateTime n = Convert.ToDateTime(newValue);
                                        newDate = n.Date.ToShortDateString();
                                    }
                                    history.Add(new HistoryData(index, origDate + " TO " + newDate + waveMail, "Qualification Date"));
                                }
                                #endregion
                                #region Demo7
                                else if (p.Name.Equals("DeliverabilityID") || p.Name.Equals("DEMO7", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    if (originalValue == "")
                                        originalValue = "0";
                                    if (newValue == "")
                                        newValue = "0";
                                    string pDeName = "";
                                    string nDeName = "";
                                    if (p.Name.Equals("DeliverabilityID"))
                                    {
                                        pDeName = deliver.Where(d => d.CodeId == Convert.ToInt32(originalValue)).Select(x => x.DisplayName).SingleOrDefault();
                                        nDeName = deliver.Where(d => d.CodeId == Convert.ToInt32(newValue)).Select(x => x.DisplayName).SingleOrDefault();
                                    }
                                    else if (deliverCodeTypeID.CodeTypeId > 0)
                                    {
                                        pDeName = deliver.Where(d => d.CodeValue == originalValue && d.CodeTypeId == deliverCodeTypeID.CodeTypeId).Select(x => x.DisplayName).SingleOrDefault();
                                        nDeName = deliver.Where(d => d.CodeValue == newValue && d.CodeTypeId == deliverCodeTypeID.CodeTypeId).Select(x => x.DisplayName).SingleOrDefault();
                                    }

                                    history.Add(new HistoryData(index, pDeName + " TO " + nDeName + waveMail, "Media Type"));
                                }
                                #endregion
                                #region Par3C
                                else if (p.Name.Equals("Par3CID"))
                                {
                                    if (originalValue == "")
                                        originalValue = "0";
                                    if (newValue == "")
                                        newValue = "0";
                                    string pParName = parList.Where(a => a.CodeId == Convert.ToInt32(originalValue)).Select(x => x.DisplayName).SingleOrDefault();
                                    string nParName = parList.Where(a => a.CodeId == Convert.ToInt32(newValue)).Select(x => x.DisplayName).SingleOrDefault();

                                    history.Add(new HistoryData(index, pParName + " TO " + nParName + waveMail, "Par3C"));
                                }
                                #endregion
                                #region SubSrc
                                else if (p.Name.Equals("SubSrcID"))
                                {
                                    if (originalValue == "")
                                        originalValue = "0";
                                    if (newValue == "")
                                        newValue = "0";
                                    string pssTypeName = subSource.Where(s => s.CodeId == Convert.ToInt32(originalValue)).Select(x => x.CodeName).SingleOrDefault();
                                    string nssTypeName = subSource.Where(s => s.CodeId == Convert.ToInt32(newValue)).Select(x => x.CodeName).SingleOrDefault();

                                    history.Add(new HistoryData(index, pssTypeName + " TO " + nssTypeName + waveMail, "Subscriber Type"));
                                }
                                else if (p.Name.Equals("SubscriberSourceCode"))
                                {
                                    history.Add(new HistoryData(index, originalValue.Trim() + " TO " + newValue.Trim() + waveMail, "Subscriber Source Code"));
                                }
                                else if (p.Name.Equals("OrigsSrc"))
                                {
                                    history.Add(new HistoryData(index, originalValue.Trim() + " TO " + newValue.Trim() + waveMail, "Original Subscriber Source Code"));
                                }
                                #endregion
                                #region Address Type
                                else if (p.Name.Equals("AddressTypeID") || p.Name.Equals("AddressTypeCodeId", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    if (originalValue == "")
                                        originalValue = "0";
                                    if (newValue == "")
                                        newValue = "0";
                                    string pssTypeName = string.Empty;
                                    string nssTypeName = string.Empty;
                                    if (Convert.ToInt32(originalValue) > 0)
                                        pssTypeName = address.Where(s => s.CodeId == Convert.ToInt32(originalValue)).Select(x => x.CodeName).SingleOrDefault();

                                    if (Convert.ToInt32(newValue) > 0)
                                        nssTypeName = address.Where(s => s.CodeId == Convert.ToInt32(newValue)).Select(x => x.CodeName).SingleOrDefault();

                                    if (!string.IsNullOrEmpty(pssTypeName) || !string.IsNullOrEmpty(nssTypeName))
                                        history.Add(new HistoryData(index, pssTypeName + " TO " + nssTypeName + waveMail, "Address Type"));
                                }
                                #endregion
                                #region Various Profile Fields
                                else if (p.Name == "FirstName")
                                    history.Add(new HistoryData(index, originalValue.Trim() + " TO " + newValue.Trim() + waveMail, "First Name"));
                                else if (p.Name == "LastName")
                                    history.Add(new HistoryData(index, originalValue.Trim() + " TO " + newValue.Trim() + waveMail, "Last Name"));
                                else if (p.Name == "Address1")
                                    history.Add(new HistoryData(index, originalValue.Trim() + " TO " + newValue.Trim() + waveMail, "Address 1"));
                                else if (p.Name == "Address2")
                                    history.Add(new HistoryData(index, originalValue.Trim() + " TO " + newValue.Trim() + waveMail, "Address 2"));
                                else if (p.Name == "Address3")
                                    history.Add(new HistoryData(index, originalValue.Trim() + " TO " + newValue.Trim() + waveMail, "Address 3"));
                                else if (p.Name == "PhoneExt")
                                    history.Add(new HistoryData(index, originalValue.Trim() + " TO " + newValue.Trim() + waveMail, "Phone Ext"));
                                else if (p.Name == "ZipCode")
                                    history.Add(new HistoryData(index, originalValue.Trim() + " TO " + newValue.Trim() + waveMail, "Zip"));
                                #endregion
                                #region Contact Method
                                else if (type == "MarketingMap")
                                {
                                    string marketingName;
                                    int marketingID = -1;
                                    bool marketFromIsActive = false;
                                    bool marketToIsActive = false;

                                    if (jsonOne != null)
                                    {
                                        //marketFromIsActive = Convert.ToBoolean(jsonOne.GetType().GetProperty("IsActive").GetValue(jsonOne));
                                        marketFromIsActive = true;
                                        marketingID = Convert.ToInt32(jsonOne.GetType().GetProperty("MarketingID").GetValue(jsonOne));
                                    }
                                    if (jsonTwo != null)
                                    {
                                        marketToIsActive = Convert.ToBoolean(jsonTwo.GetType().GetProperty("IsActive").GetValue(jsonTwo));
                                        marketingID = Convert.ToInt32(jsonTwo.GetType().GetProperty("MarketingID").GetValue(jsonTwo));
                                    }

                                    marketingName = marketingList.Where(m => m.CodeId == marketingID).Select(x => x.CodeName).SingleOrDefault();

                                    string key = string.Empty;

                                    if (!string.IsNullOrEmpty(marketingName))
                                        key = "Contact Method - " + marketingName.Trim();

                                    string value = marketFromIsActive.ToString() + " TO " + marketToIsActive.ToString();

                                    if (history.Select(x => x.PropertyName == key).Count() == 0 && history.Select(x => x.DisplayText == value).Count() == 0)
                                        history.Add(new HistoryData(index, value, key));
                                }
                                #endregion
                                #region SequenceID
                                else if (p.Name == "SequenceID")
                                    history.Add(new HistoryData(index, originalValue.Trim() + " TO " + newValue.Trim() + waveMail, "Sequence #"));
                                #endregion
                                #region MemberGroup
                                else if (p.Name == "MemberGroup")
                                    history.Add(new HistoryData(index, originalValue.Trim() + " TO " + newValue.Trim() + waveMail, "Member Group"));
                                #endregion
                                #region Plus4
                                else if (p.Name == "Plus4")
                                    history.Add(new HistoryData(index, originalValue.Trim() + " TO " + newValue.Trim() + waveMail, "Zip Plus 4"));
                                #endregion
                                #region Category & Transaction
                                else if (p.Name == "PubCategoryID")
                                {
                                    if (originalValue == "")
                                        originalValue = "0";
                                    if (newValue == "")
                                        newValue = "0";
                                    string cat1 = "";
                                    string cat2 = "";
                                    int catTypeID1 = 0;
                                    int catTypeID2 = 0;
                                    if (Convert.ToInt32(originalValue) > 0)
                                    {
                                        cat1 = cat.Where(x => x.CategoryCodeID == Convert.ToInt32(originalValue)).Select(x => x.CategoryCodeName).FirstOrDefault();
                                        catTypeID1 = cat.Where(x => x.CategoryCodeID == Convert.ToInt32(originalValue)).Select(x => x.CategoryCodeTypeID).FirstOrDefault();
                                    }
                                    if (Convert.ToInt32(newValue) > 0)
                                    {
                                        cat2 = cat.Where(x => x.CategoryCodeID == Convert.ToInt32(newValue)).Select(x => x.CategoryCodeName).FirstOrDefault();
                                        catTypeID2 = cat.Where(x => x.CategoryCodeID == Convert.ToInt32(newValue)).Select(x => x.CategoryCodeTypeID).FirstOrDefault();
                                    }

                                    if (catTypeID1 != catTypeID2)
                                    {
                                        string catType1 = "";
                                        string catType2 = "";

                                        catType1 = catType.Where(x => x.CategoryCodeTypeID == catTypeID1).Select(x => x.CategoryCodeTypeName).FirstOrDefault();
                                        catType2 = catType.Where(x => x.CategoryCodeTypeID == catTypeID2).Select(x => x.CategoryCodeTypeName).FirstOrDefault();

                                        if (catType1 != "" && catType2 != "")
                                            history.Add(new HistoryData(index, catType1 + " TO " + catType2 + waveMail, "Free/Paid"));
                                    }

                                    if (cat1 != "" || cat2 != "")
                                        history.Add(new HistoryData(index, cat1 + " TO " + cat2 + waveMail, "Category Type"));

                                }
                                else if (p.Name == "PubTransactionID")
                                {
                                    string tran1 = "";
                                    string tran2 = "";
                                    if (Convert.ToInt32(originalValue) > 0)
                                        tran1 = trans.Where(x => x.TransactionCodeID == Convert.ToInt32(originalValue)).Select(x => x.TransactionCodeName).FirstOrDefault();
                                    if (Convert.ToInt32(newValue) > 0)
                                        tran2 = trans.Where(x => x.TransactionCodeID == Convert.ToInt32(newValue)).Select(x => x.TransactionCodeName).FirstOrDefault();

                                    if (tran1 != "" || tran2 != null)
                                        history.Add(new HistoryData(index, tran1 + " TO " + tran2 + waveMail, "Transaction Type"));
                                }
                                #endregion
                                #region Responses - No Other
                                else if ((type == "SubscriptionResponseMap" || type == "ProductSubscriptionDetail") && !p.Name.Equals("ResponseOther"))
                                {
                                    FrameworkUAD.Entity.CodeSheet responseNameFrom = new FrameworkUAD.Entity.CodeSheet();
                                    FrameworkUAD.Entity.CodeSheet responseNameTo = new FrameworkUAD.Entity.CodeSheet();
                                    int responseFromID = Convert.ToInt32(jsonOne.GetType().GetProperty("CodeSheetID").GetValue(jsonOne));
                                    int responseToID = Convert.ToInt32(jsonTwo.GetType().GetProperty("CodeSheetID").GetValue(jsonTwo));

                                    responseNameFrom = (from a in answers
                                                        join q in questions on a.ResponseGroupID equals q.ResponseGroupID
                                                        where a.CodeSheetID == responseFromID
                                                        select a).SingleOrDefault();
                                    responseNameTo = (from a in answers
                                                      join q in questions on a.ResponseGroupID equals q.ResponseGroupID
                                                      where a.CodeSheetID == responseToID
                                                      select a).SingleOrDefault();

                                    FrameworkUAD.Entity.ResponseGroup rtType = new FrameworkUAD.Entity.ResponseGroup();
                                    rtType = (from a in answers
                                              join q in questions on a.ResponseGroupID equals q.ResponseGroupID
                                              where a.CodeSheetID == responseToID && a.IsActive == true && q.IsActive == true
                                              select q).SingleOrDefault();


                                    string key = rtType.DisplayName;

                                    if (rtType != null && responseNameTo.IsOther == false && history.Select(x => x.PropertyName == key).Count() == 0)
                                        history.Add(new HistoryData(index, responseNameFrom.ResponseDesc.ToString() + " TO " + responseNameTo.ResponseDesc.ToString() + waveMail, key));
                                }
                                #endregion
                                #region Responses - Other
                                else if ((type == "SubscriptionResponseMap" || type == "ProductSubscriptionDetail") && p.Name.Equals("ResponseOther"))
                                {
                                    if (!string.IsNullOrEmpty(newValue))
                                    {
                                        FrameworkUAD.Entity.CodeSheet responseNameFrom = new FrameworkUAD.Entity.CodeSheet();
                                        FrameworkUAD.Entity.CodeSheet responseNameTo = new FrameworkUAD.Entity.CodeSheet();
                                        int responseFromID = Convert.ToInt32(jsonOne.GetType().GetProperty("CodeSheetID").GetValue(jsonOne));
                                        int responseToID = Convert.ToInt32(jsonTwo.GetType().GetProperty("CodeSheetID").GetValue(jsonTwo));
                                        string responseFromOther = Convert.ToString(jsonOne.GetType().GetProperty("ResponseOther").GetValue(jsonOne));
                                        string responseToOther = Convert.ToString(jsonTwo.GetType().GetProperty("ResponseOther").GetValue(jsonTwo));

                                        responseNameFrom = (from a in answers
                                                            join q in questions on a.ResponseGroupID equals q.ResponseGroupID
                                                            where a.CodeSheetID == responseFromID
                                                            select a).SingleOrDefault();
                                        responseNameTo = (from a in answers
                                                          join q in questions on a.ResponseGroupID equals q.ResponseGroupID
                                                          where a.CodeSheetID == responseToID
                                                          select a).SingleOrDefault();

                                        FrameworkUAD.Entity.ResponseGroup rtType = new FrameworkUAD.Entity.ResponseGroup();
                                        rtType = (from a in answers
                                                  join q in questions on a.ResponseGroupID equals q.ResponseGroupID
                                                  where a.CodeSheetID == responseToID && a.IsActive == true && q.IsActive == true
                                                  select q).SingleOrDefault();


                                        string key = rtType.DisplayName;
                                        if (rtType != null && history.Select(x => x.PropertyName == key).Count() == 0)
                                        {
                                            string toValues = " TO ";
                                            string fromValues = "";
                                            if (responseNameTo.IsOther == false)
                                            {
                                                toValues += responseNameTo.ResponseDesc;
                                            }
                                            else
                                                toValues += responseToOther;
                                            if (responseNameFrom.IsOther == false)
                                                fromValues += responseNameFrom.ResponseDesc;
                                            else
                                                fromValues += responseFromOther;

                                            history.Add(new HistoryData(index, fromValues + toValues, key));
                                        }
                                    }
                                }
                                #endregion
                                else
                                {
                                    if (type == "PubSubscriptionAdHoc")
                                    {
                                        FrameworkUAD.Object.PubSubscriptionAdHoc adhoc = jsonOne as FrameworkUAD.Object.PubSubscriptionAdHoc;
                                        if (adhoc != null)
                                            history.Add(new HistoryData(index, originalValue.Trim() + " TO " + newValue.Trim() + waveMail + adHoc, adhoc.AdHocField));
                                    }
                                    else
                                        history.Add(new HistoryData(index, originalValue.Trim() + " TO " + newValue.Trim() + waveMail + adHoc, p.Name));
                                }
                            }
                            #endregion
                        }
                    }
                }
            }
            #endregion

            return history;
        }
        #endregion

        #region Filters
        public ActionResult FileHistory_Filter_PubCode(List<BatchHistory> bhList)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST, KMPlatform.Enums.Access.FullAccess))
            {
                if (bhList == null)
                    return Json(Products.ToList(), JsonRequestBehavior.AllowGet);
                else
                {
                    var prods = bhList.Select(e => e.Product).Distinct();
                    return Json(prods.ToList(), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }
        public ActionResult FileHistory_Filter_User(List<BatchHistory> bhList)
        {
            if (KM.Platform.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.FULFILLMENT, KMPlatform.Enums.ServiceFeatures.HIST, KMPlatform.Enums.Access.FullAccess))
            {
                if (bhList == null)
                    return Json(Users.ToList(), JsonRequestBehavior.AllowGet);
                else
                {
                    Dictionary<int, string> Users = new Dictionary<int, string>();
                    bhList.ForEach(x =>
                    {
                        if (!Users.ContainsKey(x.User.Key))
                            Users.Add(x.User.Key, x.User.Value);
                    });
                    //bhList.BatchHistoryIEnum.Select(e => e.User).Distinct()
                    return Json(Users.ToList(), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }
        }
        #endregion

        private class CsvDetailsInfo
        {
            public FrameworkUAD_Lookup.Entity.CodeType CodeType { get; set; }
            public List<FrameworkUAD_Lookup.Entity.Action> Actions { get; set; }
            public List<FrameworkUAD_Lookup.Entity.CategoryCode> CategoryCodes { get; set; }
            public List<FrameworkUAD_Lookup.Entity.CategoryCodeType> CategoryCodeTypes { get; set; }
            public List<FrameworkUAD_Lookup.Entity.TransactionCode> TransactionCodes { get; set; }
            public List<FrameworkUAD_Lookup.Entity.SubscriptionStatus> SubscriptionStatuses { get; set; }
            public List<FrameworkUAD_Lookup.Entity.Country> Countries { get; set; }
            public List<FrameworkUAD_Lookup.Entity.Region> Regions { get; set; }
            public List<FrameworkUAD_Lookup.Entity.Code> QualificationSources { get; set; }
            public List<FrameworkUAD_Lookup.Entity.Code> ParList { get; set; }
            public List<FrameworkUAD_Lookup.Entity.Code> DeliverList { get; set; }
            public List<FrameworkUAD_Lookup.Entity.Code> SubscriberSources { get; set; }
            public List<FrameworkUAD_Lookup.Entity.Code> Adresses { get; set; }
            public List<FrameworkUAD_Lookup.Entity.Code> MarketingList { get; set; }
        }
    }

    public class ReturnArgs
    {
        public ReturnArgs()
        {
        }
        public List<KeyValuePair<int, string>> Users { get; set; }
        public string ViewString { get; set; }
    }
}