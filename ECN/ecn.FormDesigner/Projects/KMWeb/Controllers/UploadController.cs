using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using KMDbManagers;

namespace KMWeb.Controllers
{
    public class UploadController : Controller
    {
        public string folder =
            WebConfigUtils.KMDesignerRoot() +
            ConfigurationManager.AppSettings.Get("CssDir");

        public ActionResult Save(HttpPostedFileBase file)
        {
            var fileUID = Guid.NewGuid();
            var fileName = string.Format("{0}.css", fileUID);

            var physicalPath = Path.Combine(folder, fileName);

            file.SaveAs(physicalPath);

            return Json(new { success = true, fileUID = fileUID });
        }
    }
}
