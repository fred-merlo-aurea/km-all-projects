using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MVC.Standards.Common;
using MVC.Standards.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace MVC.Standards.Models
{
    public class KendoStandardGridController : BaseController
    {
        // GET: KendoStandardGrid
        public ActionResult Index()
        {
            SearchFilter vm = new SearchFilter();
            return View();
        }

        

        public ActionResult Read_Data([DataSourceRequest]DataSourceRequest request,  string Title ="", int PageNumber=1)
        {
            //v_Content_Select_Title @ContentTitle='', @UserID=7667, @CurrentPage=1, @PageSize=20,@SortDirection='DESC', @SortColumn='ContentTitle',@CustomerID=1797
            KendoGridHelper<ContentViewModel> gh = new KendoGridHelper<ContentViewModel>();
            List<GridSort> lstgs = gh.GetGridSort(request, "ContentTitle");
            List<GridFilter> lstgf = gh.GetGridFilter(request);
            if (lstgf.Count > 0)
            {
                Title =  lstgf.Where(x => x.FilterColumnName == "ContentTitle").Select(x => x.FilterColumnValue).FirstOrDefault().ToString();
            }
            string sortColumn = lstgs[0].SortColumnName;
            string sortdirection = lstgs[0].SortDirection;
            //List of Actual Records
            List<ContentViewModel> listRange = new List<ContentViewModel>();

            KMPlatform.Entity.User user = new KMPlatform.BusinessLogic.User().SelectUser(7667, false);

            DataSet ds = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentTitle(Title, null,0, user.UserID, null, null, CurrentUser, PageNumber, request.PageSize, sortdirection, sortColumn);
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
            result.Total = totalCount;
            return Json(result, JsonRequestBehavior.AllowGet);

        }


       
       
    }
}