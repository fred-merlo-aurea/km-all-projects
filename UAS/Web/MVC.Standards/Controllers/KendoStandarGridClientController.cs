using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MVC.Standards.Common;
using MVC.Standards.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;

namespace MVC.Standards.Controllers
{
    public class KendoStandarGridClientController : BaseController
    {
        // GET: KendoStandarGridClient
        public ActionResult Index()
        {
           return View();
        }

        public ActionResult Read_Data([DataSourceRequest]DataSourceRequest request, string Title = "")
        {

            List<ContentViewModel> listRange = new List<ContentViewModel>();

         
            DataSet ds = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentTitle(Title, null, 0, CurrentUser.UserID, null, null, CurrentUser, 1, 100, "ASC", "ContentTitle");
            int totalCount = 0;
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.AsEnumerable())
            {
                ContentViewModel content = new ContentViewModel();
                if (totalCount == 0)
                    totalCount = Convert.ToInt32(dr["TotalCount"].ToString());
                content.TotalRecordCounts = dr["TotalCount"].ToString();
                content.ContentTitle = dr["ContentTitle"].ToString();
                content.CreatedDate = (DateTime)dr["CreatedDate"];
                content.ContentURL = dr["ContentURL"].ToString();
                content.ContentType = dr["ContentTypeCode"].ToString();
                listRange.Add(content);
            }
            IQueryable<ContentViewModel> gs = listRange.AsQueryable();
            DataSourceResult result = gs.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTitleFilter([DataSourceRequest]DataSourceRequest request, string Title = "")
        {
            List<string> listTitle = new List<string>();
            
            DataSet ds = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentTitle(Title, null, 0, CurrentUser.UserID, null, null, CurrentUser, 1, 100, "ASC", "ContentTitle");
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.AsEnumerable())
            {
                listTitle.Add(dr["ContentTitle"].ToString());
            }
            var stringlist = listTitle.Select(x => new SelectListItem{Value = x,Text = x}).ToList();
            return Json(stringlist.Distinct(), JsonRequestBehavior.AllowGet);
        }

    }
}