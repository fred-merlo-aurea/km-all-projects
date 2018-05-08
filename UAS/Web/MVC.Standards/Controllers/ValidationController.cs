using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Standards.Controllers
{
    public class ValidationController : Controller
    {
        // GET: Validation
        public ActionResult Index()
        {
            ValidationDemo.ViewModel.ValidationProto model = new ValidationDemo.ViewModel.ValidationProto();
            ValidationDemo.Data.Folder.LoadData().ForEach(x => model.FolderList.Add(new SelectListItem() { Text = x.FolderName, Value = x.FolderID.ToString() }));
            ValidationDemo.Data.Content.LoadData().ForEach(x => model.ContentList.Add(new SelectListItem() { Text = x.ContentName, Value = x.ContentID.ToString() }));
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(ValidationDemo.ViewModel.ValidationProto model)
        {
            ValidationDemo.Model.Layout layout = new ValidationDemo.Model.Layout();

            ValidationDemo.Data.Folder.LoadData().ForEach(x => model.FolderList.Add(new SelectListItem() { Text = x.FolderName, Value = x.FolderID.ToString() }));
            ValidationDemo.Data.Content.LoadData().ForEach(x => model.ContentList.Add(new SelectListItem() { Text = x.ContentName, Value = x.ContentID.ToString() }));

            if (ModelState.IsValid)
            {
                layout.ContentID = model.ContentID;
                layout.FolderID = model.FolderID;
                layout.LayoutName = model.LayoutName;
                layout.TemplateID = model.TemplateID;

                if (Request.IsAjaxRequest())
                {
                    try
                    {
                        ValidationDemo.Business.Layout.Save(layout);
                    }
                    catch (ValidationDemo.Model.ECNException ex)
                    {
                        if (ex.ErrorList.Count > 0)
                        {
                            model.ErrorList = ex.ErrorList;
                            return Json(model, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    try
                    {
                        ValidationDemo.Business.Layout.Save(layout);
                    }
                    catch (ValidationDemo.Model.ECNException ex)
                    {
                        if (ex.ErrorList.Count > 0)
                        {
                            ex.ErrorList.ForEach(x => ModelState.AddModelError("", x.ErrorMessage));
                            return View(model);
                        }
                    }
                }

                
            }
            return View(model);
        }
    }

   
}