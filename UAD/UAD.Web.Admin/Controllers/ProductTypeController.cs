using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UAD.Web.Admin.Infrastructure;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using UAD.Web.Admin.Controllers.Common;
using FrameworkUAD.Object;
using System.Web.Script.Serialization;
using System.Dynamic;
using System.Text;

namespace UAD.Web.Admin.Controllers
{
    public class ProductTypeController : BaseController
    {
        public ActionResult Index()
        {
            if (!KM.Platform.User.IsSystemAdministrator(CurrentUser))
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }

            return View();
        }

        public ActionResult GetProductTypeData([DataSourceRequest]DataSourceRequest request)
        {
            List<FrameworkUAD.Entity.ProductTypes> ptList = new FrameworkUAD.BusinessLogic.ProductTypes().Select(ClientConnections).OrderBy(e => e.PubTypeDisplayName).ToList();
            IQueryable<FrameworkUAD.Entity.ProductTypes> dr = ptList.AsQueryable();
            DataSourceResult result = dr.ToDataSourceResult(request);
            result.Total = ptList.Count;
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult AddEdit(int id = 0)
        {
            if (!KM.Platform.User.IsSystemAdministrator(CurrentUser))
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }

            Models.ProductTypes model = new Models.ProductTypes();

            if (id > 0)
            {
                ViewBag.Title = "Edit Product Type";
                FrameworkUAD.Entity.ProductTypes oProductTypes = new FrameworkUAD.BusinessLogic.ProductTypes().SelectByID(id, ClientConnections);
                model.PubTypeID = oProductTypes.PubTypeID;
                model.PubTypeDisplayName = oProductTypes.PubTypeDisplayName;
                model.ColumnReference = oProductTypes.ColumnReference;
                model.IsActive = oProductTypes.IsActive;
                model.SortOrder = oProductTypes.SortOrder;
                model.DateCreated = oProductTypes.DateCreated;
                model.DateUpdated = oProductTypes.DateUpdated;
                model.CreatedByUserID = oProductTypes.CreatedByUserID;
                model.UpdatedByUserID = oProductTypes.UpdatedByUserID;
            }
            else
            {
                ViewBag.Title = "Add Product Type";
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult AddEdit(Models.ProductTypes model)
        {
            dynamic messageToView = new ExpandoObject();
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new JavaScriptConverter[] { new ExpandoJSONConverter() });

            FrameworkUAD.Entity.ProductTypes pt = new FrameworkUAD.Entity.ProductTypes();
            pt.PubTypeID = model.PubTypeID;
            pt.PubTypeDisplayName = model.PubTypeDisplayName;
            pt.ColumnReference = model.PubTypeDisplayName;
            pt.IsActive = model.IsActive;
            pt.SortOrder = model.SortOrder;

            if (model.PubTypeID > 0)
            {
                pt.DateUpdated = DateTime.Now;
                pt.UpdatedByUserID = CurrentUser.UserID;
                pt.DateCreated = model.DateCreated == null ? DateTime.Now : model.DateCreated.Value;
                pt.CreatedByUserID = model.CreatedByUserID.Value;
            }
            else
            {
                pt.DateCreated = DateTime.Now;
                pt.CreatedByUserID = CurrentUser.UserID;
            }

            try
            {
                int id = new FrameworkUAD.BusinessLogic.ProductTypes().Save(pt, ClientConnections);

                if (id > 0)
                {
                    messageToView.Text = "Product Type has been saved successfully.";
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
                "Product Type",
                () => new FrameworkUAD.BusinessLogic.ProductTypes().Delete(ClientConnections, id));
        }

        public JsonResult GetProductTypeSortOrder()
        {
            List<SelectListItem> dataTypeSelectList = new List<SelectListItem>();
            dataTypeSelectList.Add(new SelectListItem() { Text = "Select Sort Order", Value = "" });
            List<FrameworkUAD.Entity.ProductTypes> pubTypes = new FrameworkUAD.BusinessLogic.ProductTypes().Select(ClientConnections);

            for (int i = 1; i <= pubTypes.Count + 1; i++)
            {
                dataTypeSelectList.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            return Json(dataTypeSelectList, JsonRequestBehavior.AllowGet);
        }
    }
}