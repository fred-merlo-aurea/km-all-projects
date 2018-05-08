using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FrameworkUAD.BusinessLogic;
using FrameworkUAD.Object;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using UAD.Web.Admin.Controllers.Common;
using UAD.Web.Admin.Models;

namespace UAD.Web.Admin.Controllers
{
    public class CodeSheetController : BaseController
    {
        public ActionResult Index([DefaultValue(0)]int pubID, [DefaultValue(0)]int responseGroupID)
        {
            if (!KM.Platform.User.IsSystemAdministrator(CurrentUser))
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }

            Models.CodeSheets model = new Models.CodeSheets();
            model.PubID = pubID;
            model.ResponseGroupID = responseGroupID;

            return View(model);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult GetCodeSheetData([DataSourceRequest]DataSourceRequest request, int ResponseGroupID, string Name = "", string SearchCriteria = "", int PageNumber = 1)
        {
            KendoGridHelper<Models.CodeSheets> gh = new KendoGridHelper<Models.CodeSheets>();
            List<GridSort> gsList = gh.GetGridSort(request, "ResponseDesc");
            string sortColumn = gsList[0].SortColumnName;
            string sortdirection = gsList[0].SortDirection;

            DataSet ds = new FrameworkUAD.BusinessLogic.CodeSheet().SelectBySearch(ResponseGroupID, Name, SearchCriteria, PageNumber, request.PageSize, sortdirection, sortColumn, ClientConnections);
            DataTable dt = ds.Tables[0];
            List<Models.CodeSheets> mgList = Core_AMS.Utilities.DataTableFunctions.ConvertToList<Models.CodeSheets>(dt);
            IQueryable<Models.CodeSheets> dr = mgList.AsQueryable();
            DataSourceResult result = dr.ToDataSourceResult(request);
            result.Total = mgList.Count;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddEdit(int id = 0, int pubID = 0, int responseGroupID = 0)
        {
            if (!KM.Platform.User.IsSystemAdministrator(CurrentUser))
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }

            Models.CodeSheets model = new Models.CodeSheets();
            model.PubID = pubID;
            model.ResponseGroupID = responseGroupID;

            if (id > 0)
            {
                ViewBag.Title = "Edit Code Sheet";
                FrameworkUAD.Entity.CodeSheet oCodeSheet = new FrameworkUAD.BusinessLogic.CodeSheet().SelectByID(id, ClientConnections);
                model.CodeSheetID = oCodeSheet.CodeSheetID;
                model.PubID = oCodeSheet.PubID;
                model.ResponseGroupID = oCodeSheet.ResponseGroupID;
                model.ResponseValue = oCodeSheet.ResponseValue;
                model.ResponseDesc = oCodeSheet.ResponseDesc;
                model.ReportGroupID = oCodeSheet.ReportGroupID;
                model.IsActive = oCodeSheet.IsActive;
                model.WQT_ResponseID = oCodeSheet.WQT_ResponseID;
                model.IsOther = oCodeSheet.IsOther;
                model.DateCreated = oCodeSheet.DateCreated;
                model.DateUpdated = oCodeSheet.DateUpdated;
                model.CreatedByUserID = oCodeSheet.CreatedByUserID;
                model.UpdatedByUserID = oCodeSheet.UpdatedByUserID;
            }
            else
            {
                ViewBag.Title = "Add Code Sheet";
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult AddEdit(Models.CodeSheets model, List<Models.CodeSheetMaster> MasterData)
        {
            dynamic messageToView = new ExpandoObject();
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new JavaScriptConverter[] { new ExpandoJSONConverter() });

            FrameworkUAD.Entity.CodeSheet cs = new FrameworkUAD.Entity.CodeSheet();
            cs.PubID = model.PubID;
            cs.CodeSheetID = model.CodeSheetID;
            cs.ResponseGroupID = model.ResponseGroupID;
            cs.ResponseValue = model.ResponseValue;
            cs.ResponseDesc = model.ResponseDesc;
            cs.ReportGroupID = model.ReportGroupID;
            cs.IsActive = model.IsActive;
            cs.IsOther = model.IsOther;
            cs.WQT_ResponseID = model.WQT_ResponseID;

            if (model.CodeSheetID > 0)
            {
                cs.DisplayOrder = model.DisplayOrder;
                cs.DateUpdated = DateTime.Now;
                cs.UpdatedByUserID = CurrentUser.UserID;
                cs.DateCreated = model.DateCreated == null ? DateTime.Now : model.DateCreated.Value;
                cs.CreatedByUserID = model.CreatedByUserID.Value;
            }
            else
            {
                List<FrameworkUAD.Entity.CodeSheet> csList = new FrameworkUAD.BusinessLogic.CodeSheet().SelectByResponseGroupID(model.ResponseGroupID, ClientConnections);

                if(csList.Count > 0)
                    cs.DisplayOrder = csList.Max(x => x.DisplayOrder.Value);
                else
                    cs.DisplayOrder = 1;

                cs.DateCreated = DateTime.Now;
                cs.CreatedByUserID = CurrentUser.UserID;
            }

            try
            {
                int id = new FrameworkUAD.BusinessLogic.CodeSheet().Save(cs, ClientConnections);

                if (model.CodeSheetID > 0)
                    new FrameworkUAD.BusinessLogic.CodeSheetMasterCodeSheetBridge().Delete(ClientConnections, id);

                foreach (CodeSheetMaster csm in MasterData)
                {
                    FrameworkUAD.Entity.CodeSheetMasterCodeSheetBridge csbridge = new FrameworkUAD.Entity.CodeSheetMasterCodeSheetBridge();
                    csbridge.MasterID = csm.MasterID;
                    csbridge.CodeSheetID = id;
                    new FrameworkUAD.BusinessLogic.CodeSheetMasterCodeSheetBridge().Save(csbridge, ClientConnections);
                }

                if (id > 0)
                {
                    messageToView.Text = "Code Sheet has been saved successfully.";
                    messageToView.Success = true;
                }
                else
                {
                    messageToView.Text = "Something went wrong. Please try later.";
                    messageToView.Success = false;
                }

                var json = serializer.Serialize(messageToView);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (UADException ex)
            {
                return GetErrorJsonResult(ex);
            }
        }

        [HttpGet]
        public JsonResult Delete(int id = 0)
        {
            return DeleteInternal(
                "Code Sheet",
                ()=> new CodeSheet().Delete(ClientConnections, id));
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult GetCodeSheetMasterData([DataSourceRequest] DataSourceRequest request, int CodeSheetID)
        {
            KendoGridHelper<Models.CodeSheetMaster> gh = new KendoGridHelper<Models.CodeSheetMaster>();
            DataSet ds = new FrameworkUAD.BusinessLogic.MasterCodeSheet().SelectByCodeSheetID(CodeSheetID, ClientConnections);
            DataTable dt = ds.Tables[0];
            List<Models.CodeSheetMaster> csmList = Core_AMS.Utilities.DataTableFunctions.ConvertToList<Models.CodeSheetMaster>(dt);
            IQueryable<Models.CodeSheetMaster> dr = csmList.AsQueryable();
            DataSourceResult result = dr.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult GetMasterData([DataSourceRequest] DataSourceRequest request, int codeSheetID = 0)
        {
            DataSet ds = new FrameworkUAD.BusinessLogic.MasterCodeSheet().SelectByCodeSheetID(codeSheetID, ClientConnections);
            DataTable dt = ds.Tables[0];
            List<Models.CodeSheetMaster> csmList = Core_AMS.Utilities.DataTableFunctions.ConvertToList<Models.CodeSheetMaster>(dt);
            csmList.ForEach(c => c.Title = c.DisplayName + " - " + c.MasterValue + " - " + c.MasterDesc);
            IQueryable<Models.CodeSheetMaster> dr = csmList.AsQueryable();
            DataSourceResult result = dr.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMasterCodeSheetAvailableData(string MasterData, int masterGroupID = 0)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List< Models.CodeSheetMaster> csmList = (List<Models.CodeSheetMaster>)ser.Deserialize(MasterData, typeof(List<Models.CodeSheetMaster>));

            List<SelectListItem> mcsAvailableList = new List<SelectListItem>();

            List<FrameworkUAD.Entity.MasterCodeSheet> mc = new FrameworkUAD.BusinessLogic.MasterCodeSheet().SelectMasterGroupID(ClientConnections, masterGroupID);
            FrameworkUAD.Entity.MasterGroup mg = new FrameworkUAD.BusinessLogic.MasterGroup().SelectByID(masterGroupID, ClientConnections);

            var query = (from c in mc
                         select new { MasterID = c.MasterID, Title = mg.DisplayName + " - " + c.MasterValue + " - " + c.MasterDesc });

            var available = query.ToList().Where(item => !csmList.Any(item2 => item2.MasterID == item.MasterID)).ToList();
            available.ToList().ForEach(c => mcsAvailableList.Add(new SelectListItem() { Text = c.Title, Value = c.MasterID.ToString()}));

            return Json(mcsAvailableList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMasterCodeSheetSelectedData(string MasterData, int masterGroupID = 0)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<Models.CodeSheetMaster> csmList = (List<Models.CodeSheetMaster>)ser.Deserialize(MasterData, typeof(List<Models.CodeSheetMaster>));

            List<SelectListItem> mcsSelecedtList = new List<SelectListItem>();

            List<FrameworkUAD.Entity.MasterCodeSheet> mc = new FrameworkUAD.BusinessLogic.MasterCodeSheet().SelectMasterGroupID(ClientConnections, masterGroupID);
            FrameworkUAD.Entity.MasterGroup mg = new FrameworkUAD.BusinessLogic.MasterGroup().SelectByID(masterGroupID, ClientConnections);

            var query = (from c in mc
                         select new { MasterID = c.MasterID, Title = mg.DisplayName + " - " + c.MasterValue + " - " + c.MasterDesc });

            var selected = query.ToList().Where(item => csmList.Any(item2 => item2.MasterID == item.MasterID)).ToList();
            selected.ToList().ForEach(c => mcsSelecedtList.Add(new SelectListItem() { Text = c.Title, Value = c.MasterID.ToString() }));

            return Json(mcsSelecedtList, JsonRequestBehavior.AllowGet);
        }
    }
}