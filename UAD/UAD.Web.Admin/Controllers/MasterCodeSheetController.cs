using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using FrameworkUAD.Object;
using Kendo.Mvc.UI;
using UAD.Web.Admin.Models;
using System.Data;
using Kendo.Mvc.Extensions;
using UAD.Web.Admin.Controllers.Common;
using System.Dynamic;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Linq;

namespace UAD.Web.Admin.Controllers
{
    public class MasterCodeSheetController : BaseController
    {
        public ActionResult Index([DefaultValue(0)]int id)
        {
            if (!KM.Platform.User.IsSystemAdministrator(CurrentUser))
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }

            Models.MasterCodeSheets model = new Models.MasterCodeSheets();
            model.MasterGroupID = id;

            return View(model);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult GetMasterCodeSheetData([DataSourceRequest]DataSourceRequest request, int MasterGroupID, string Name = "", string SearchCriteria = "", int PageNumber = 1)
        {
            KendoGridHelper<Models.MasterCodeSheets> gh = new KendoGridHelper<Models.MasterCodeSheets>();
            List<GridSort> gsList = gh.GetGridSort(request, "MasterValue");
            string sortColumn = gsList[0].SortColumnName;
            string sortdirection = gsList[0].SortDirection;

            DataSet ds = new FrameworkUAD.BusinessLogic.MasterCodeSheet().SelectByMasterCodeSheetSearch(MasterGroupID, Name, SearchCriteria, PageNumber, request.PageSize, sortdirection, sortColumn, ClientConnections);
            DataTable dt = ds.Tables[0];
            List<Models.MasterCodeSheets> mgList = Core_AMS.Utilities.DataTableFunctions.ConvertToList<Models.MasterCodeSheets>(dt);
            IQueryable<Models.MasterCodeSheets> dr = mgList.AsQueryable();
            DataSourceResult result = dr.ToDataSourceResult(request);
            result.Total = mgList.Count;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddEdit(int id = 0, int masterGroupID = 0)
        {
            if (!KM.Platform.User.IsSystemAdministrator(CurrentUser))
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }

            Models.MasterCodeSheets model = new Models.MasterCodeSheets();
            model.MasterGroupID = masterGroupID;

            if (id > 0)
            {
                ViewBag.Title = "Edit Master Code Sheet";
                FrameworkUAD.Entity.MasterCodeSheet oMasterCodeSheet = new FrameworkUAD.BusinessLogic.MasterCodeSheet().SelectByID(id, ClientConnections);
                model.MasterID = oMasterCodeSheet.MasterID;
                model.MasterGroupID = oMasterCodeSheet.MasterGroupID;
                model.MasterValue = oMasterCodeSheet.MasterValue;
                model.MasterDesc = oMasterCodeSheet.MasterDesc;
                model.MasterDesc1 = oMasterCodeSheet.MasterDesc1;
                model.EnableSearching = oMasterCodeSheet.EnableSearching;
                model.DateCreated = oMasterCodeSheet.DateCreated;
                model.DateUpdated = oMasterCodeSheet.DateUpdated;
            }
            else
            {
                ViewBag.Title = "Add Master Code Sheet";
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult AddEdit(Models.MasterCodeSheets model)
        {
            dynamic messageToView = new ExpandoObject();
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new JavaScriptConverter[] { new ExpandoJSONConverter() });

            FrameworkUAD.Entity.MasterCodeSheet mcs = new FrameworkUAD.Entity.MasterCodeSheet();
            mcs.MasterID = model.MasterID;
            mcs.MasterGroupID = model.MasterGroupID;
            mcs.MasterValue = model.MasterValue;
            mcs.MasterDesc = model.MasterDesc;
            mcs.MasterDesc1 = model.MasterDesc1;
            mcs.EnableSearching = Convert.ToBoolean(model.EnableSearching);

            if (model.MasterID > 0)
            {
                mcs.DateUpdated = DateTime.Now;
                mcs.UpdatedByUserID = CurrentUser.UserID;
                mcs.DateCreated = model.DateCreated == null ? DateTime.Now : model.DateCreated.Value;
                mcs.CreatedByUserID = model.CreatedByUserID.Value;
            }
            else
            {
                mcs.DateCreated = DateTime.Now;
                mcs.CreatedByUserID = CurrentUser.UserID;
            }

            try
            {
                int id = new FrameworkUAD.BusinessLogic.MasterCodeSheet().Save(mcs, ClientConnections);

                if (id > 0)
                {
                    messageToView.Text = "Master Code Sheet has been saved successfully.";
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
        public JsonResult Delete(int id)
        {
            return DeleteInternal(
                "Master Code Sheet",
                () => new FrameworkUAD.BusinessLogic.MasterCodeSheet().DeleteMasterID(ClientConnections, id));
        }

        [HttpPost]
        public JsonResult Import(HttpPostedFileBase uploadFile)
        {
            dynamic messageToView = new ExpandoObject();
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new JavaScriptConverter[] { new ExpandoJSONConverter() });
            HttpPostedFileBase File = Request.Files["uploadFile"];
            try
            {
                if (HttpContext.Request.Files.AllKeys.Any())
                {
                    messageToView.Text = "Master Code Sheet has been uploaded successfully.";
                   
                }

                messageToView.Success = true;
                var json = serializer.Serialize(messageToView);
                return Json(json, "application/json", JsonRequestBehavior.AllowGet);
            }
            catch (UADException ex)
            {
                messageToView.Text = ex.ErrorList[0].ErrorMessage;
                messageToView.Success = false;
                var json = serializer.Serialize(messageToView);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Sort()
        {
            if (!KM.Platform.User.IsSystemAdministrator(CurrentUser))
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }

            return View();
        }

        [HttpPost]
        public JsonResult UpdateSortOrder(string[] masterCodeSheetIDs)
        {
            dynamic messageToView = new ExpandoObject();
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new JavaScriptConverter[] { new ExpandoJSONConverter() });

            XDocument xmlDoc = new XDocument();
            XElement xmlNode = new XElement("MasterCodeSheets");

            try
            {
                int i = 0;
                foreach (string s in masterCodeSheetIDs)
                {
                    i++;
                    XElement xmlID = (new XElement("MasterCodeSheet",
                    new XElement("ID", s),
                    new XElement("SortOrder", i)));

                    xmlNode.Add(xmlID);
                }

                new FrameworkUAD.BusinessLogic.MasterCodeSheet().UpdateSortOrder(xmlNode.ToString(), ClientConnections);

                messageToView.Text = "Master Code Sheets sort order saved successfully.";
                messageToView.Success = true;
                var json = serializer.Serialize(messageToView);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (UADException ex)
            {
                StringBuilder sbError = new StringBuilder();
                foreach (UADError er in ex.ErrorList)
                {
                    sbError.AppendLine(er.ErrorMessage + "<br/>");
                }
                messageToView.Text = sbError.ToString();
                messageToView.Success = false;
                var json = serializer.Serialize(messageToView);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }
    }
}