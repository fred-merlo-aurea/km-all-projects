using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UAS.Web.Models.Common
{
    public class DownloadViewModel
    {

        public bool DownloadVisible { get; set; }
        public bool SaveToCampaignVisible { get; set; }
        public bool ExportToGroupVisible { get; set; }
        public bool PromoCodeVisible { get; set; }
        public bool QueryDetailsCheckboxVisible { get; set; }
        public bool DownloadCountVisible { get; set; }
        public bool IsQueryDetailsIncluded { get; set; }
        public bool IsNewGroupChecked { get; set; }
        public bool IsExistingGroupChecked { get; set; }
        public bool IsFilterBased { get; set; }
        public bool IsArchived { get; set; }
        public int IssueID { get; set; }
        public int PubID { get; set; }
        public string HeaderText { get; set; }
        public string DownloadFor { get; set; }
        public string MasterOptionSelected { get; set; }
        public string PromoCode { get; set; }
        public int DownloadTemplateID { get; set; }
        public int CustomerClientID { get; set; }
        public int CustomerClientGroupID { get; set; }
        public int FolderID { get; set; }
        public string GroupName { get; set; }
        public int GroupID { get; set; }
        public int DownloadCount { get; set; }
        public int TotalCount { get; set; }
        public int CampaignID { get; set; }
        public string CampaignName { get; set; }
        public string JobCode { get; set; }
        public bool IsNewCampaign { get; set; }
        public bool IsExistingCampaign { get; set; }
        public string FilterName { get; set; }
        public string Notes { get; set; }
        public List<FrameworkUAD.Object.FilterMVC> FilterList { get; set; }
        public List<int> SubscriberIDs { get; set; }
        public List<SelectListItem> AvailableProfileFields { get; set; }
        public List<SelectListItem> AvailableDemoFields { get; set; }
        public List<SelectListItem> AvailableAdhocFields { get; set; }
        public List<SelectListItem> SelectedItems { get; set; }
        public List<SelectListItem> DownLoadTemplates { get; set; }
        public List<SelectListItem> Customers { get; set; }
        public bool DisplayDownLoad { get; set; }
        public  FrameworkUAD.BusinessLogic.Enums.ViewType ViewType  { get; set; }
        public DownloadViewModel()
        {
            DownloadFor = "Report";
            DisplayDownLoad = true;
            Customers = new List<SelectListItem>();
            DownLoadTemplates = new List<SelectListItem>();
            AvailableProfileFields = new List<SelectListItem>();
            AvailableDemoFields = new List<SelectListItem>();
            AvailableAdhocFields = new List<SelectListItem>();
            SelectedItems = new List<SelectListItem>();
        }
    }
}