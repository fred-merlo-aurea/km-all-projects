using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UAD.Web.Admin.Infrastructure;
using FrameworkUAD.Object;
using UAD.Web.Admin.Controllers.Common;
using Kendo.Mvc.UI;
using UAD.Web.Admin.Models;
using System.Data;
using Kendo.Mvc.Extensions;
using System.Dynamic;
using System.Web.Script.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace UAD.Web.Admin.Controllers
{
    public class MasterGroupController : BaseController
    {
        public ActionResult Index()
        {
            if (!KM.Platform.User.IsSystemAdministrator(CurrentUser))
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }

            return View();
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult GetMasterGroupData([DataSourceRequest]DataSourceRequest request, string Name = "", string SearchCriteria = "", int PageNumber = 1)
        {
            KendoGridHelper<Models.MasterGroups> gh = new KendoGridHelper<Models.MasterGroups>();
            List<GridSort> gsList = gh.GetGridSort(request, "DisplayName");
            string sortColumn = gsList[0].SortColumnName;
            string sortdirection = gsList[0].SortDirection;

            DataSet ds = new FrameworkUAD.BusinessLogic.MasterGroup().SelectByMasterGroupsSearch(Name, SearchCriteria, PageNumber, request.PageSize, sortdirection, sortColumn, ClientConnections);
            DataTable dt = ds.Tables[0];
            List<Models.MasterGroups> mgList =  Core_AMS.Utilities.DataTableFunctions.ConvertToList<Models.MasterGroups>(dt);
            IQueryable<Models.MasterGroups> dr = mgList.AsQueryable();
            DataSourceResult result = dr.ToDataSourceResult(request);
            result.Total = mgList.Count;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddEdit(int id = 0)
        {
            if (!KM.Platform.User.IsSystemAdministrator(CurrentUser))
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }

            Models.MasterGroups model = new Models.MasterGroups();

            if (id > 0)
            {
                ViewBag.Title = "Edit Master Group";
                FrameworkUAD.Entity.MasterGroup oMasterGroup = new FrameworkUAD.BusinessLogic.MasterGroup().SelectByID(id, ClientConnections);
                model.MasterGroupID = oMasterGroup.MasterGroupID;
                model.DisplayName = oMasterGroup.DisplayName;
                model.Name = oMasterGroup.Name;
                model.IsActive = oMasterGroup.IsActive;
                model.EnableSubReporting = oMasterGroup.EnableSubReporting;
                model.EnableSearching = oMasterGroup.EnableSearching;
                model.EnableAdhocSearch = oMasterGroup.EnableAdhocSearch;
                model.SortOrder = oMasterGroup.SortOrder;
                model.DateCreated = DateTime.Now;
                model.CreatedByUserID = CurrentUser.UserID;
            }
            else
            {
                ViewBag.Title = "Add Master Group";
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult AddEdit(Models.MasterGroups model)
        {
            dynamic messageToView = new ExpandoObject();
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new JavaScriptConverter[] { new ExpandoJSONConverter() });

            FrameworkUAD.Entity.MasterGroup mg = new FrameworkUAD.Entity.MasterGroup();
            mg.MasterGroupID = model.MasterGroupID;
            mg.DisplayName = model.DisplayName;
            mg.Name = model.Name;
            mg.IsActive = model.IsActive;
            mg.EnableSubReporting = model.EnableSubReporting;
            mg.EnableSearching = model.EnableSearching;
            mg.EnableAdhocSearch = model.EnableAdhocSearch;
            mg.SortOrder = model.SortOrder;

            if (model.MasterGroupID > 0)
            {
                mg.DateUpdated = DateTime.Now;
                mg.UpdatedByUserID = CurrentUser.UserID;
                mg.DateCreated = model.DateCreated == null ? DateTime.Now : model.DateCreated.Value;
                mg.CreatedByUserID = model.CreatedByUserID.Value;
            }
            else
            {
                mg.DateCreated = DateTime.Now;
                mg.CreatedByUserID = CurrentUser.UserID;
            }

            try
            {
                int id = new FrameworkUAD.BusinessLogic.MasterGroup().Save(mg, ClientConnections);

                if (id > 0)
                {
                    messageToView.Text = "Master Group has been saved successfully..";
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

        [HttpGet]
        public JsonResult Delete(int id)
        {
            return DeleteInternal(
                "Master Group",
                () => new FrameworkUAD.BusinessLogic.MasterGroup().Delete(id, ClientConnections));
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
        public JsonResult UpdateSortOrder(string[] MasterGroupIDs)
        {
            dynamic messageToView = new ExpandoObject();
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new JavaScriptConverter[] { new ExpandoJSONConverter() });

            XDocument xmlDoc = new XDocument();
            XElement xmlNode = new XElement("MasterGroups");

            try
            {
                int i = 0;
                foreach (string s in MasterGroupIDs)
                {
                    i++;
                    XElement xmlID = (new XElement("MasterGroup",
                    new XElement("ID", s),
                    new XElement("SortOrder", i)));

                    xmlNode.Add(xmlID);
                }

                new FrameworkUAD.BusinessLogic.MasterGroup().UpdateSortOrder(xmlNode.ToString(), ClientConnections);

                messageToView.Text = "Master Groups sort order saved successfully.";
                messageToView.Success = true;

                var json = serializer.Serialize(messageToView);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (UADException ex)
            {
                return GetErrorJsonResult(ex);
            }
        }

        public JsonResult GetMasterGroups()
        {
            List<SelectListItem> mgSelectList = new List<SelectListItem>();
            List<FrameworkUAD.Entity.MasterGroup> masterGroupListUAD = new FrameworkUAD.BusinessLogic.MasterGroup().Select(ClientConnections);
            var mgList = masterGroupListUAD.OrderBy(x => x.SortOrder).ToList();
            mgList.ForEach(c => mgSelectList.Add(new SelectListItem() { Text = c.DisplayName, Value = c.MasterGroupID.ToString() }));
            return Json(mgSelectList, JsonRequestBehavior.AllowGet);
       }
    }
}