using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.Collections.Specialized;
using UAD.Web.Admin.Infrastructure;
using Kendo.Mvc.UI;
using UAD.Web.Admin.Models;
using System.Data;
using Kendo.Mvc.Extensions;
using System.Dynamic;
using System.Web.Script.Serialization;
using UAD.Web.Admin.Controllers.Common;
using FrameworkUAD.Object;
using System.Text;

namespace UAD.Web.Admin.Controllers
{
    public class ResponseGroupController : BaseController
    {
        public ActionResult Index([DefaultValue(0)]int id)
        {
            if (!KM.Platform.User.IsSystemAdministrator(CurrentUser))
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }

            Models.ResponseGroups model = new Models.ResponseGroups();
            model.PubID = id;

            return View(model);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult GetResponseGroupData([DataSourceRequest]DataSourceRequest request, int PubID, string Name = "", string SearchCriteria = "", int PageNumber = 1)
        {
            KendoGridHelper<Models.ResponseGroups> gh = new KendoGridHelper<Models.ResponseGroups>();
            List<GridSort> gsList = gh.GetGridSort(request, "DisplayName");
            string sortColumn = gsList[0].SortColumnName;
            string sortdirection = gsList[0].SortDirection;

            DataSet ds = new FrameworkUAD.BusinessLogic.ResponseGroup().SelectByResponseGroupsSearch(PubID, Name, SearchCriteria, PageNumber, request.PageSize, sortdirection, sortColumn, ClientConnections);
            DataTable dt = ds.Tables[0];
            List<Models.ResponseGroups> mgList = Core_AMS.Utilities.DataTableFunctions.ConvertToList<Models.ResponseGroups>(dt);
            IQueryable<Models.ResponseGroups> dr = mgList.AsQueryable();
            DataSourceResult result = dr.ToDataSourceResult(request);
            result.Total = mgList.Count;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddEdit(int id = 0, int pubID = 0)
        {
            if (!KM.Platform.User.IsSystemAdministrator(CurrentUser))
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }

            Models.ResponseGroups model = new Models.ResponseGroups();
            model.PubID = pubID;

            if (id > 0)
            {
                ViewBag.Title = "Edit Response Group";
                FrameworkUAD.Entity.ResponseGroup oResponseGroup = new FrameworkUAD.BusinessLogic.ResponseGroup().SelectByID(id, ClientConnections);
                model.ResponseGroupID = oResponseGroup.ResponseGroupID;
                model.PubID = oResponseGroup.PubID;
                model.DisplayName = oResponseGroup.DisplayName;
                model.ResponseGroupName = oResponseGroup.ResponseGroupName;
                model.IsActive = oResponseGroup.IsActive;
                model.IsMultipleValue = oResponseGroup.IsMultipleValue;
                model.IsRequired = oResponseGroup.IsRequired;
                model.ResponseGroupTypeId = oResponseGroup.ResponseGroupTypeId;
                model.DisplayOrder = oResponseGroup.DisplayOrder;
                model.WQT_ResponseGroupID = oResponseGroup.WQT_ResponseGroupID;
                model.DateCreated = DateTime.Now;
                model.CreatedByUserID = CurrentUser.UserID;
            }
            else
            {
                ViewBag.Title = "Add Response Group";
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult AddEdit(Models.ResponseGroups model)
        {
            dynamic messageToView = new ExpandoObject();
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new JavaScriptConverter[] { new ExpandoJSONConverter() });

            FrameworkUAD.Entity.ResponseGroup rg = new FrameworkUAD.Entity.ResponseGroup();
            rg.ResponseGroupID = model.ResponseGroupID;
            rg.PubID = model.PubID;
            rg.DisplayName = model.DisplayName;
            rg.ResponseGroupName = model.ResponseGroupName;
            rg.IsActive = Convert.ToBoolean(model.IsActive);
            rg.IsMultipleValue = Convert.ToBoolean(model.IsMultipleValue);
            rg.IsRequired = Convert.ToBoolean(model.IsRequired);
            rg.ResponseGroupTypeId = model.ResponseGroupTypeId;
            rg.DisplayOrder = model.DisplayOrder;
            rg.WQT_ResponseGroupID = model.WQT_ResponseGroupID;

            if (model.ResponseGroupID > 0)
            {
                rg.DateUpdated = DateTime.Now;
                rg.UpdatedByUserID = CurrentUser.UserID;
                rg.DateCreated = model.DateCreated == null ? DateTime.Now : model.DateCreated.Value;
                rg.CreatedByUserID = model.CreatedByUserID.Value;
            }
            else
            {
                rg.DateCreated = DateTime.Now;
                rg.CreatedByUserID = CurrentUser.UserID;
            }

            try
            {
                int id = new FrameworkUAD.BusinessLogic.ResponseGroup().Save(rg, ClientConnections); 

                if (id > 0)
                {
                    messageToView.Text = "Response Group has been saved successfully.";
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
        public JsonResult Delete(int id, int pubID)
        {
            return DeleteInternal(
                "Response Group",
                () => new FrameworkUAD.BusinessLogic.ResponseGroup().Delete(ClientConnections, id, pubID));
        }

        [HttpPost]
        public JsonResult GetCodeByCodeTypeName()
        {
            List<SelectListItem> codeSelectList = new List<SelectListItem>();
            codeSelectList.Add(new SelectListItem() { Text = "Select KM Product", Value = "" });
            List<FrameworkUAD_Lookup.Entity.Code> codeList = new FrameworkUAD_Lookup.BusinessLogic.Code().Select(FrameworkUAD_Lookup.Enums.CodeType.Response_Group);
            codeList.ForEach(c => codeSelectList.Add(new SelectListItem() { Text = c.CodeName, Value = c.CodeId.ToString() }));
            return Json(codeSelectList, JsonRequestBehavior.AllowGet);
        }
    }
}