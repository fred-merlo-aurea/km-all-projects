using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel;
using UAD.Web.Admin.Controllers.Common;
using Kendo.Mvc.UI;
using UAD.Web.Admin.Models;
using System.Data;
using Kendo.Mvc.Extensions;
using System.Dynamic;
using System.Web.Script.Serialization;
using FrameworkUAD.Object;
using System.Text;
using FrameworkUAD.BusinessLogic;

namespace UAD.Web.Admin.Controllers
{
    public class PubCustomFieldController : BaseController
    {
        public ActionResult Index([DefaultValue(0)]int pubID)
        {
            if (!KM.Platform.User.IsSystemAdministrator(CurrentUser))
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }

            Models.ProductCustomFields model = new Models.ProductCustomFields();
            model.PubID = pubID;

            return View(model);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult GetProductCustomFieldData([DataSourceRequest]DataSourceRequest request, int PubID, string Name = "", string SearchCriteria = "", int PageNumber = 1)
        {
            KendoGridHelper<Models.ProductCustomFields> gh = new KendoGridHelper<Models.ProductCustomFields>();
            List<GridSort> gsList = gh.GetGridSort(request, "CustomField");
            string sortColumn = gsList[0].SortColumnName;
            string sortdirection = gsList[0].SortDirection;

            DataSet ds = new FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper().SelectBySearch(PubID, Name, SearchCriteria, PageNumber, request.PageSize, sortdirection, sortColumn, ClientConnections);
            DataTable dt = ds.Tables[0];
            List<Models.ProductCustomFields> mgList = Core_AMS.Utilities.DataTableFunctions.ConvertToList<Models.ProductCustomFields>(dt);
            IQueryable<Models.ProductCustomFields> dr = mgList.AsQueryable();
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

            Models.ProductCustomFields model = new Models.ProductCustomFields();
            model.PubID = pubID;

            if (id > 0)
            {
                ViewBag.Title = "Edit Product Custom Field";
                FrameworkUAD.Entity.ProductSubscriptionsExtensionMapper oProductCustomField = new FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper().SelectByID(id, ClientConnections);
                model.PubSubscriptionsExtensionMapperID = oProductCustomField.PubSubscriptionsExtensionMapperID;
                model.PubID = oProductCustomField.PubID;
                model.StandardField = oProductCustomField.StandardField;
                model.CustomField = oProductCustomField.CustomField;
                model.CustomFieldDataType = oProductCustomField.CustomFieldDataType;
                model.Active = oProductCustomField.Active;
                model.DateCreated = oProductCustomField.DateCreated;
                model.DateUpdated = oProductCustomField.DateUpdated;
                model.CreatedByUserID = oProductCustomField.CreatedByUserID;
                model.UpdatedByUserID = oProductCustomField.UpdatedByUserID;
            }
            else
            {
                ViewBag.Title = "Add Product Custom Field";
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult AddEdit(Models.ProductCustomFields model)
        {
            dynamic messageToView = new ExpandoObject();
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new JavaScriptConverter[] { new ExpandoJSONConverter() });

            FrameworkUAD.Entity.ProductSubscriptionsExtensionMapper psem = new FrameworkUAD.Entity.ProductSubscriptionsExtensionMapper();
            psem.PubSubscriptionsExtensionMapperID = model.PubSubscriptionsExtensionMapperID;
            psem.PubID = model.PubID;

            if (model.PubSubscriptionsExtensionMapperID > 0)
            {
                psem.StandardField = model.StandardField;
            }
            else
            {
                List<FrameworkUAD.Entity.ProductSubscriptionsExtensionMapper> lpsem = new FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper().SelectAll(ClientConnections).FindAll(x => x.PubID == model.PubID);

                int max = 0;
                if (lpsem != null && lpsem.Count > 0)
                {
                    max = (from s in lpsem
                           where !String.IsNullOrEmpty(s.StandardField.Substring(5))
                           select Convert.ToInt32(s.StandardField.Substring(5))).Max();
                }

                psem.StandardField = "Field" + (max + 1);
            }

            psem.CustomField = model.CustomField;
            psem.CustomFieldDataType = model.CustomFieldDataType;
            psem.Active = model.Active;

            if (model.PubSubscriptionsExtensionMapperID > 0)
            {
                psem.DateUpdated = DateTime.Now;
                psem.UpdatedByUserID = CurrentUser.UserID;
                psem.DateCreated = model.DateCreated == null ? DateTime.Now : model.DateCreated.Value;
                psem.CreatedByUserID = model.CreatedByUserID.Value;
            }
            else
            {
                psem.DateCreated = DateTime.Now;
                psem.CreatedByUserID = CurrentUser.UserID;
            }

            try
            {
                int id = new FrameworkUAD.BusinessLogic.ProductSubscriptionsExtensionMapper().Save(psem, ClientConnections);

                if (id > 0)
                {
                    messageToView.Text = "Product Custom Field has been saved successfully.";
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
        public JsonResult Delete(int id = 0, int pubID = 0)
        {
            return DeleteInternal(
                "Product Custom Field",
                () => new ProductSubscriptionsExtensionMapper().Delete(id, pubID, ClientConnections));
        }
    }
}