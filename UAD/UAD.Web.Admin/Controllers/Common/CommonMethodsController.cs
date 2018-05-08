using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace UAD.Web.Admin.Controllers.Common
{
    public class CommonMethodsController : BaseController
    {
        public CommonMethodsController()
        {
        }

        [HttpGet]
        public JsonResult GetProducts()
        {
            List<SelectListItem> productSelectList = new List<SelectListItem>();
            productSelectList.Add(new SelectListItem() { Text = "Select Product", Value = "" });
            List<FrameworkUAD.Entity.Product> productListUAD = new FrameworkUAD.BusinessLogic.Product().Select(ClientConnections);
            var prodList = productListUAD.Where(x => x.IsActive == true).OrderBy(x => x.PubName).ToList();
            prodList.ForEach(c => productSelectList.Add(new SelectListItem() { Text = c.PubName, Value = c.PubID.ToString() }));
            return Json(productSelectList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMasterGroups()
        {
            List<SelectListItem> mgSelectList = new List<SelectListItem>();
            mgSelectList.Add(new SelectListItem() { Text = "Select Master Group", Value = "" });
            List<FrameworkUAD.Entity.MasterGroup> mgListUAD = new FrameworkUAD.BusinessLogic.MasterGroup().Select(ClientConnections);
            var mgList = mgListUAD.Where(x => x.IsActive == true).OrderBy(x => x.DisplayName).ToList();
            mgList.ForEach(c => mgSelectList.Add(new SelectListItem() { Text = c.DisplayName, Value = c.MasterGroupID.ToString() }));
            return Json(mgSelectList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMasterGroupsBySortOrder()
        {
            List<SelectListItem> mgSelectList = new List<SelectListItem>();
            List<FrameworkUAD.Entity.MasterGroup> masterGroupListUAD = new FrameworkUAD.BusinessLogic.MasterGroup().Select(ClientConnections);
            var mgList = masterGroupListUAD.OrderBy(x => x.SortOrder).ToList();
            mgList.ForEach(c => mgSelectList.Add(new SelectListItem() { Text = c.DisplayName, Value = c.MasterGroupID.ToString() }));
            return Json(mgSelectList, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetResponseGroups(int pubID = 0)
        {
            List<SelectListItem> csSelectList = new List<SelectListItem>();
            csSelectList.Add(new SelectListItem() { Text = "Select Response Group", Value = "" });
            List<FrameworkUAD.Entity.ResponseGroup> csListUAD = new FrameworkUAD.BusinessLogic.ResponseGroup().Select(pubID, ClientConnections);
            var csList = csListUAD.Where(x => x.IsActive == true).OrderBy(x => x.DisplayName).ToList();
            csList.ForEach(c => csSelectList.Add(new SelectListItem() { Text = c.DisplayName, Value = c.ResponseGroupID.ToString() }));
            return Json(csSelectList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetReportGroups(int responseGroupID = 0)
        {
            List<SelectListItem> rgSelectList = new List<SelectListItem>();
            rgSelectList.Add(new SelectListItem() { Text = "Select Report Group", Value = "" });
            List<FrameworkUAD.Entity.ReportGroups> rgListUAD = new FrameworkUAD.BusinessLogic.ReportGroups().Select(ClientConnections);
            var rgList = rgListUAD.Where(x => x.ResponseGroupID == responseGroupID).OrderBy(x => x.DisplayName).ToList();
            rgList.ForEach(c => rgSelectList.Add(new SelectListItem() { Text = c.DisplayName, Value = c.ReportGroupID.ToString() }));
            return Json(rgSelectList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMasterCodeSheets(int masterGroupID = 0)
        {
            List<SelectListItem> csSelectList = new List<SelectListItem>();
            List<FrameworkUAD.Entity.MasterCodeSheet> csListUAD = new FrameworkUAD.BusinessLogic.MasterCodeSheet().SelectMasterGroupID(ClientConnections, masterGroupID);
            var csList = csListUAD.OrderBy(x=>x.MasterDesc).ToList();
            csList.ForEach(c => csSelectList.Add(new SelectListItem() { Text = c.MasterDesc, Value = c.MasterID.ToString()}));
            return Json(csSelectList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMasterCodeSheetsBySortOrder(int masterGroupID = 0)
        {
            List<SelectListItem> csSelectList = new List<SelectListItem>();
            List<FrameworkUAD.Entity.MasterCodeSheet> csListUAD = new FrameworkUAD.BusinessLogic.MasterCodeSheet().SelectMasterGroupID(ClientConnections, masterGroupID);
            var csList = csListUAD.OrderBy(x => x.SortOrder).ToList();
            csList.ForEach(c => csSelectList.Add(new SelectListItem() { Text = c.MasterDesc, Value = c.MasterID.ToString() }));
            return Json(csSelectList, JsonRequestBehavior.AllowGet);
        }
    }
}