using System;
using System.Collections.Generic;
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

namespace UAD.Web.Admin.Controllers
{
    public class CustomFieldController : BaseController
    {
        // GET: CustomField
        public ActionResult Index()
        {
            if (!KM.Platform.User.IsSystemAdministrator(CurrentUser))
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }

            return View();
        }

        public ActionResult GetCustomFieldData([DataSourceRequest]DataSourceRequest request)
        {
            List<FrameworkUAD.Entity.SubscriptionsExtensionMapper> semList = new FrameworkUAD.BusinessLogic.SubscriptionsExtensionMapper().SelectAll(ClientConnections).OrderBy(e=>e.CustomField).ToList();
            IQueryable<FrameworkUAD.Entity.SubscriptionsExtensionMapper> dr = semList.AsQueryable();
            DataSourceResult result = dr.ToDataSourceResult(request);
            result.Total = semList.Count;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddEdit(int id = 0)
        {
            if (!KM.Platform.User.IsSystemAdministrator(CurrentUser))
            {
                return RedirectToAction("Error", "Error", new { errorType = "UnAuthorized" });
            }

            Models.CustomFields model = new Models.CustomFields();

            if (id > 0)
            {
                ViewBag.Title = "Edit Custom Field";
                FrameworkUAD.Entity.SubscriptionsExtensionMapper oCustomField = new FrameworkUAD.BusinessLogic.SubscriptionsExtensionMapper().SelectByID(id, ClientConnections);
                model.SubscriptionsExtensionMapperID = oCustomField.SubscriptionsExtensionMapperID;
                model.StandardField = oCustomField.StandardField;
                model.CustomField = oCustomField.CustomField;
                model.CustomFieldDataType = oCustomField.CustomFieldDataType;
                model.Active = oCustomField.Active;
                model.DateCreated = oCustomField.DateCreated;
                model.DateUpdated = oCustomField.DateUpdated;
                model.CreatedByUserID = oCustomField.CreatedByUserID;
                model.UpdatedByUserID = oCustomField.UpdatedByUserID;
            }
            else
            {
                ViewBag.Title = "Add Custom Field";
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult AddEdit(Models.CustomFields model)
        {
            dynamic messageToView = new ExpandoObject();
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new JavaScriptConverter[] { new ExpandoJSONConverter() });

            FrameworkUAD.Entity.SubscriptionsExtensionMapper sem = new FrameworkUAD.Entity.SubscriptionsExtensionMapper();
            sem.SubscriptionsExtensionMapperID = model.SubscriptionsExtensionMapperID;

            if (model.SubscriptionsExtensionMapperID > 0)
            {
                sem.StandardField = model.StandardField;
            }
            else
            {
                List<FrameworkUAD.Entity.SubscriptionsExtensionMapper> lsem = new FrameworkUAD.BusinessLogic.SubscriptionsExtensionMapper().SelectAll(ClientConnections);

                int max = 0;
                if (lsem != null && lsem.Count > 0)
                {
                    max = (from s in lsem
                           where !String.IsNullOrEmpty(s.StandardField.Substring(5))
                           select Convert.ToInt32(s.StandardField.Substring(5))).Max();
                }

                sem.StandardField = "Field" + (max + 1);
            }

            sem.CustomField = model.CustomField;
            sem.CustomFieldDataType = model.CustomFieldDataType;
            sem.Active = model.Active;

            if (model.SubscriptionsExtensionMapperID > 0)
            {
                sem.DateUpdated = DateTime.Now;
                sem.UpdatedByUserID = CurrentUser.UserID;
                sem.DateCreated = model.DateCreated == null ? DateTime.Now : model.DateCreated.Value;
                sem.CreatedByUserID = model.CreatedByUserID.Value;
            }
            else
            {
                sem.DateCreated = DateTime.Now;
                sem.CreatedByUserID = CurrentUser.UserID;
            }

            try
            {
                int id = new FrameworkUAD.BusinessLogic.SubscriptionsExtensionMapper().Save(sem, ClientConnections);

                if (id > 0)
                {
                    messageToView.Text = "Custom Field has been saved successfully.";
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
                "Custom Field",
                () => new SubscriptionsExtensionMapper().Delete(id, ClientConnections));
        }
    }
}