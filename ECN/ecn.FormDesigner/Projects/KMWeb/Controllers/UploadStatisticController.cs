using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KMManagers;
using KMManagers.APITypes;
using KMSite;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Configuration;

namespace KMWeb.Controllers
{
    public class UploadStatisticController : Controller
    {
        //
        // GET: /UploadStatistic/

        FormManager manager = new FormManager();
        [HttpPost]
        public ActionResult ReportsLog([DataSourceRequest] DataSourceRequest request, string formStatisticID)
        {

            FormStatisticLoader fsl = new FormStatisticLoader();
            return Json(fsl.GetStatisticLog(formStatisticID).ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult Reports([DataSourceRequest] DataSourceRequest request, string formUID)
        {   
            FormStatisticLoader fsl = new FormStatisticLoader();
            string sorting ;
            bool order;
            if (request.Sorts == null || request.Sorts.Count == 0)
            {
                order = false;
                sorting = "Start";
            }
            else
            {
                order = request.Sorts.First().SortDirection == System.ComponentModel.ListSortDirection.Ascending;
                sorting = request.Sorts.First().Member;
            }
            var dataResult = fsl.GetStatistic(formUID, sorting, order, request.Page, request.PageSize);                        
            int totalResult = fsl.GetStatisticCount(formUID);
            return Json(new DataSourceResult { Data = dataResult.ToList(), Total = totalResult });                 
        }


        //public ActionResult GetStatistic(string formUID) 
        //{
        //    FormStatisticLoader fsl = new FormStatisticLoader();
        //    return Json(fsl.GetStatistic(formUID),JsonRequestBehavior.AllowGet);
        //}

        [AllowAnonymous]
        public ActionResult GetFormStats(string formUID) 
        {
            FormStatisticLoader fsl = new FormStatisticLoader();
            string formName = fsl.GetFormNameByUid(formUID);
            
            int knownVisitors = fsl.GetKnowVisitor(formUID);
            int Total = fsl.GetStatisticCount(formUID);
            int unknownVisitors = Total - knownVisitors;
            KMWeb.Models.Forms.Modals.FormStatsModel model = new KMWeb.Models.Forms.Modals.FormStatsModel { FormName = formName, Total = Total, KnownVisitors = knownVisitors, UnknownVisitors = unknownVisitors };
            return PartialView("Partials/_FormStatistic", model);
            
        }

        public ActionResult GetStatisticLog(string id)
        {
            FormStatisticLoader fsl = new FormStatisticLoader();
            return Json(fsl.GetStatisticLog(id), JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult CreateStatistic(string formUID, int? totalPages, string email)
        {
            try {
                if (string.IsNullOrEmpty(formUID) || totalPages == null)
                    throw new Exception("Empty parameters for formUID or totalPages");

                FormStatisticLoader fsl = new FormStatisticLoader();
                if (!APIRunnerBase.CheckEmail(ref email))
                {
                    email = null;
                }            
                var FormStatistic_ID = fsl.CreateStatistic(formUID, totalPages.Value, email); 
                return Json(new { FormStatistic_ID = FormStatistic_ID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string note = "formUID: " + formUID + ", totalPages:" + totalPages + ", email:" + email + ", User Agent: " + Request.UserAgent.ToString();
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(e, "UploadStatistic.CreateStatistic", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), note);
                return null;
            }
        }

        [AllowAnonymous]
        public ActionResult UploadFinish(long? FormStatistic_ID, int? numberPage)
        {
            try
            {
                if (FormStatistic_ID == null || numberPage == null)
                    throw new Exception("Empty parameters for FormStatistic_ID or numberPage");

                FormStatisticLoader fsl = new FormStatisticLoader();
                fsl.LogFinish(FormStatistic_ID.Value, numberPage.Value);
                return Json(new { FormStatistic_ID = FormStatistic_ID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string note = "FormStatistic_ID: " + FormStatistic_ID + ", numberPage:" + numberPage + ", User Agent: " + Request.UserAgent.ToString();
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(e, "UploadStatistic.UploadFinish", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), note);
                return null;
            }            
        }

        [AllowAnonymous]
        public ActionResult UploadNewer(long? FormStatistic_ID, int? numberPage)
        {
            try
            {
                if (FormStatistic_ID == null || numberPage == null)
                    throw new Exception("Empty parameters for FormStatistic_ID or numberPage");

                FormStatisticLoader fsl = new FormStatisticLoader();
                fsl.LogNewer(FormStatistic_ID.Value, numberPage.Value);
                return Json(new { FormStatistic_ID = FormStatistic_ID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string note = "FormStatistic_ID: " + FormStatistic_ID + ", numberPage:" + numberPage + ", User Agent: " + Request.UserAgent.ToString();
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(e, "UploadStatistic.UploadNewer", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), note);
                return null;
            }            
        }

        [AllowAnonymous]
        public ActionResult UpdateTotalPages(long? FormStatistic_ID, int? totalPages)
        {
            try
            {
                if (FormStatistic_ID == null || totalPages == null)
                    throw new Exception("Empty parameters for FormStatistic_ID or totalPages");

                FormStatisticLoader fsl = new FormStatisticLoader();
                fsl.UpdateTotalPages(FormStatistic_ID.Value, totalPages.Value);
                return Json(new { FormStatistic_ID = FormStatistic_ID.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string note = "FormStatistic_ID: " + FormStatistic_ID + ", totalPages:" + totalPages + ", User Agent: " + Request.UserAgent.ToString();
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(e, "UploadStatistic.UpdateTotalPages", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), note);
                return null;
            }            
        }

        [AllowAnonymous]
        public ActionResult UpdateEmail(long? FormStatistic_ID, string email)
        {
            try
            {
                if (FormStatistic_ID == null)
                    throw new Exception("Empty parameters for FormStatistic_ID");

                FormStatisticLoader fsl = new FormStatisticLoader();
                if (APIRunnerBase.CheckEmail(ref email))
                {
                    var FormStatistic = fsl.UpdateEmail(FormStatistic_ID.Value, email);
                    return Json(new { FormStatistic_ID = FormStatistic }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { FormStatistic_ID = FormStatistic_ID.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string note = "FormStatistic_ID: " + FormStatistic_ID + ", email:" + email + ", User Agent: " + Request.UserAgent.ToString();
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(e, "UploadStatistic.UpdateEmail", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), note);
                return null;
            }
            
            
        }     

        [AllowAnonymous]
        public ActionResult UploadSubmit(long? FormStatistic_ID, int? numberPage, string email)
        {
            try
            {
                if (FormStatistic_ID == null || numberPage == null)
                    throw new Exception("Empty parameters for FormStatistic_ID or numberPage");

                FormStatisticLoader fsl = new FormStatisticLoader();
                if (!APIRunnerBase.CheckEmail(ref email))
                    email = null;
                var FormStatistic = fsl.SubmitStatistic(FormStatistic_ID.Value, numberPage.Value, email);
                return Json(new { FormStatistic_ID = FormStatistic }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string note = "FormStatistic_ID: " + FormStatistic_ID + ", numberPage:" + numberPage + ", email:" + email + ", User Agent: " + Request.UserAgent.ToString();
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(e, "UploadStatistic.UploadSubmit", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), note);
                return null;
            }            
        }

        [AllowAnonymous]
        public ActionResult UnloadForm(long? FormStatistic_ID, int? numberPage)
        {
            try
            {
                if (FormStatistic_ID == null || numberPage == null)
                    throw new Exception("Empty parameters for FormStatistic_ID or numberPage");

                FormStatisticLoader fsl = new FormStatisticLoader();
                var FormStatistic = fsl.UnloadFormStatistic(FormStatistic_ID.Value, numberPage.Value);
                return Json(new { FormStatistic_ID = FormStatistic }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                string note = "FormStatistic_ID: " + FormStatistic_ID + ", numberPage:" + numberPage + ", User Agent: " + Request.UserAgent.ToString();
                KM.Common.Entity.ApplicationLog.LogNonCriticalError(e, "UploadStatistic.UnloadForm", Convert.ToInt32(ConfigurationManager.AppSettings["KMCommon_Application"]), note);
                return null;
            }
        }
    }
}
