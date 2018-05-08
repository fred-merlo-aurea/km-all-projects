using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UAD.Web.Admin.Models
{
    public class CodeSheets
    {
        public int CodeSheetID { get; set; }
        [Display(Name = "Product :")]
        public int PubID { get; set; }
        public string ResponseGroup { get; set; }
        [Display(Name = "Value :")]
        public string ResponseValue { get; set; }
        [Display(Name = "Description :")]
        public string ResponseDesc { get; set; }
        [Display(Name = "Response Group :")]
        public int ResponseGroupID { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? CreatedByUserID { get; set; }
        public int? UpdatedByUserID { get; set; }
        public int? DisplayOrder { get; set; }
        [Display(Name = "Report Group :")]
        public int? ReportGroupID { get; set; }
        public string ReportGroupName { get; set; }
        [Display(Name = "Active :")]
        public bool? IsActive { get; set; }
        public int? WQT_ResponseID { get; set; }
        [Display(Name = "Other :")]
        public bool? IsOther { get; set; }
        public int TotalRecordCounts { get; set; }
        public List<MasterGroups> MasterGroups { get; set; }
        public List<CodeSheetMaster> CodeSheetMaster { get; set; }
        public CodeSheets()
        {
            CodeSheetID = 0;
            PubID = 0;
            ResponseGroup = string.Empty;
            ResponseValue = string.Empty;
            ResponseDesc = string.Empty;
            ResponseGroupID = 0;
            DateCreated = DateTime.Now;
            DateUpdated = null;
            CreatedByUserID = 1;
            UpdatedByUserID = null;
            DisplayOrder = 0;
            ReportGroupID = 0;
            IsActive = false;
            WQT_ResponseID = 0;
            IsOther = false;
            MasterGroups = null;
            CodeSheetMaster = null;
        }
    }
}