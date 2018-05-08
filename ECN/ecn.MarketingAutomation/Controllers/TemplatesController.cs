using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Web.Script.Serialization;
using ecn.MarketingAutomation.Models;
using KMSite;

namespace ecn.MarketingAutomation.Controllers
{
    public class TemplatesController : BaseController
    {
        private KMPlatform.Entity.User CurrentUser
        {
            get { return ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser; }
        }

        public ActionResult Index()
        {
            if (KMPlatform.BusinessLogic.User.HasAccess(CurrentUser, KMPlatform.Enums.Services.MARKETINGAUTOMATION, KMPlatform.Enums.ServiceFeatures.MarketingAutomationTemplates, KMPlatform.Enums.Access.View))
            {
                ViewBag.CurrentUser = CurrentUser;
                bool canEdit = false;
                if (KMPlatform.BusinessLogic.User.IsSystemAdministrator(CurrentUser))
                {
                    canEdit = true;
                    
                }

                ViewBag.CanEditTemplate = canEdit;
                ViewBag.CanDeleteTemplate = canEdit;
                return View();
            }
            else
            {                
                return RedirectToAction("Index","Home");

            }
        }
       
        public ActionResult GetTemplatesGrid([DataSourceRequest] DataSourceRequest request,string Name="")
        {
            string filePath = System.Web.HttpContext.Current.Server.MapPath("/ecn.MarketingAutomation/LocalData") + "/MockupTemplates.txt";
            string jsonDiagrams = System.IO.File.ReadAllText(filePath);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<TemplateViewModel> ActiveTemplates;
            if (!string.IsNullOrEmpty(jsonDiagrams))
                ActiveTemplates = serializer.Deserialize<List<TemplateViewModel>>(jsonDiagrams);
            else
                ActiveTemplates = new List<TemplateViewModel>();

            ActiveTemplates.Reverse();
            IQueryable<TemplateViewModel> at = ActiveTemplates.AsQueryable();
            DataSourceResult result = at.ToDataSourceResult(request);
            return Json(result);
        }
        public ActionResult GetNameFilter([DataSourceRequest]DataSourceRequest request, string Name = "")
        {
            List<string> listTemplate = new List<string>();

            string filePath = System.Web.HttpContext.Current.Server.MapPath("/ecn.MarketingAutomation/LocalData") + "/MockupTemplates.txt";
            string jsonDiagrams = System.IO.File.ReadAllText(filePath);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<TemplateViewModel> ActiveTemplates;
            if (!string.IsNullOrEmpty(jsonDiagrams))
                ActiveTemplates = serializer.Deserialize<List<TemplateViewModel>>(jsonDiagrams);
            else
                ActiveTemplates = new List<TemplateViewModel>();
            foreach (TemplateViewModel tv in ActiveTemplates)
            {
                listTemplate.Add(tv.Name);
            }
            var stringlist = listTemplate.Select(x => new SelectListItem { Value = x, Text = x }).ToList();
            return Json(stringlist.Distinct(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadTemplate(int? id)
        {
            TemplateViewModel model = new TemplateViewModel();

            if (id.HasValue)
            {
                model = model.getSingleDiagram(id.Value);
            }

            return PartialView("Partials/_NewTemplate", model);
        }

        public ActionResult Create(TemplateViewModel model)
        {
            return ProcessCreate(model, true);
        }

        public ActionResult Delete(int id)
        {
            if (KMPlatform.BusinessLogic.User.IsSystemAdministrator(CurrentUser))
            {
                TemplateViewModel dvm = new TemplateViewModel();
                List<TemplateViewModel> ActiveDiagrams = dvm.getFileDiagrams();

                ActiveDiagrams.RemoveAll(a => a.Id == id);

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string jsonDiag = serializer.Serialize(ActiveDiagrams);
                string filePath = System.Web.HttpContext.Current.Server.MapPath("/ecn.MarketingAutomation/LocalData") + "/MockupTemplates.txt";
                System.IO.File.WriteAllText(filePath, jsonDiag);

                return JavaScriptRedirectToAction("Index");
            }
            else
            {
                return JavaScriptRedirectToAction("Index");
            }
        }

        public ActionResult Copy(TemplateViewModel model)
        {
            return ProcessCreate(model, false);
        }

        private ActionResult ProcessCreate(TemplateViewModel model, bool isCreate)
        {
            int diagId = 0;
            model.Name = (model.Name ?? string.Empty).Trim();
            bool valid = true;
            if (ModelState.IsValid)
            {
                if (model.Id <= 0) isCreate = true; else isCreate = false;
                if (CheckNameIsUnique(model.Name,model.Id))
                    diagId = isCreate ? Save(model) : Update(model);
                else
                {
                    // 37079 Automation Template Name validation needed and it should show appropriate validation messages
                    ModelState.AddModelError("Error", "Automation Template Name already exists. Please enter a unique Automation Template Name.");
                    valid = false;
                }
            }
            else
                valid = false;

            if (valid)
            {
                if (!isCreate) // returns to Template home page
                    return JavaScriptRedirectToAction("Index");
                else
                   return JavaScriptRedirectToAction("Edit", "Templates", new { id = diagId });
            }
               

            return PartialView("Partials/_NewTemplate", model);
        }

        private bool CheckNameIsUnique(string name,int Id)
        {
            TemplateViewModel dvm = new TemplateViewModel();
            List<TemplateViewModel> ActiveDiagrams = dvm.getFileDiagrams();

            bool has = ActiveDiagrams.Any(d => d.Name.ToLower() == name.ToLower() && d.Id!=Id);

            return !has;
        }

        private int Save(TemplateViewModel model)
        {
            int diagId = 1;
            List<TemplateViewModel> ActiveDiagrams = model.getFileDiagrams();
            bool getId = true;
            while (getId)
            {
                bool has = ActiveDiagrams.Any(d => d.Id == diagId);
                if (!has)
                    getId = false;
                else
                    diagId++;
            }
            model.Id = diagId;
            model.Type = DiagramType.Simple;
            model.Diagram = "{\"shapes\":[],\"connections\":[]}";
            ActiveDiagrams.Add(model);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string jsonDiag = serializer.Serialize(ActiveDiagrams);
            string filePath = System.Web.HttpContext.Current.Server.MapPath("/ecn.MarketingAutomation/LocalData") + "/MockupTemplates.txt";
            System.IO.File.WriteAllText(filePath, jsonDiag);

            return diagId;
        }
        /// <summary>
        /// Edit Automation Template Name Action Menu options - 37213
        /// </summary>
        /// <param name="model"></param>
        /// <returns>int - Model Id</returns>
        private int Update(TemplateViewModel model)
        {
            List<TemplateViewModel> ActiveDiagrams = model.getFileDiagrams();
            //Update particular Template Name
            var UpdateTemplate = ActiveDiagrams.Where(d => d.Id == model.Id).FirstOrDefault();
            if (UpdateTemplate != null) { UpdateTemplate.Name = model.Name; }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string jsonDiag = serializer.Serialize(ActiveDiagrams);
            string filePath = System.Web.HttpContext.Current.Server.MapPath("/ecn.MarketingAutomation/LocalData") + "/MockupTemplates.txt";
            System.IO.File.WriteAllText(filePath, jsonDiag);

            return model.Id;
        }
        private int FullCopy(TemplateViewModel model)
        {
            int diagId = 1;
            List<TemplateViewModel> ActiveDiagrams = model.getFileDiagrams();
            bool getId = true;
            while (getId)
            {
                bool has = ActiveDiagrams.Any(d => d.Id == diagId);
                if (!has)
                    getId = false;
                else
                    diagId++;
            }
            model.Id = diagId;
            model.Type = DiagramType.Simple;
            model.Diagram = model.Diagram; // Automation Copy
            ActiveDiagrams.Add(model);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string jsonDiag = serializer.Serialize(ActiveDiagrams);
            string filePath = System.Web.HttpContext.Current.Server.MapPath("/ecn.MarketingAutomation/LocalData") + "/MockupTemplates.txt";
            System.IO.File.WriteAllText(filePath, jsonDiag);

            return diagId;
        }

        public ActionResult Edit(int id = 0)
        {
            if (KMPlatform.BusinessLogic.User.IsSystemAdministrator(CurrentUser))
            {
                TemplateViewModel tvm = new TemplateViewModel();
                if (id != 0)
                {
                    // Load Json Diagram
                    return View(tvm.getSingleDiagram(id));
                }
                else
                    return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult SaveTemplateDiagram(TemplateViewModel tvm)
        {
            if (tvm.Id != 0)
            {
                tvm.saveSingleDiagram(tvm);
                return Json("Diagram successfully saved!");
            }
            else
                return Json("Diagram model is empty");
        }

        public ActionResult ViewTemplate(int id = 0)
        {
            TemplateViewModel tvm = new TemplateViewModel();
            if (id != 0)
            {
                // Load Json Diagram
                return View(tvm.getSingleDiagram(id));
            }
            else
                return RedirectToAction("Index");
        }
    }
}