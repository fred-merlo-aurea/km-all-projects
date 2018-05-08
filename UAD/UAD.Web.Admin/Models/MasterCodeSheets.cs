using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UAD.Web.Admin.Models
{
    public class MasterCodeSheets
    {
        public int MasterID { get; set; }
        [Display(Name = "Master Group :")]
        public int MasterGroupID { get; set; }
        [Display(Name = "Value :")]
        public string MasterValue { get; set; }
        [Display(Name = "Description :")]
        public string MasterDesc { get; set; }
        [Display(Name = "Description 1 :")]
        public string MasterDesc1 { get; set; }
        [Display(Name = "Enable Searching :")]
        public bool EnableSearching { get; set; }
        public int SortOrder { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? CreatedByUserID { get; set; }
        public int? UpdatedByUserID { get; set; }
        public int TotalRecordCounts { get; set; }

        public MasterCodeSheets()
        {
            MasterID = 0;
            MasterValue = string.Empty;
            MasterDesc = string.Empty;
            MasterGroupID = 0;
            MasterDesc1 = string.Empty;
            EnableSearching = false;
            SortOrder = 0;
            DateCreated = DateTime.Now;
            DateUpdated = null;
            CreatedByUserID = 1;
            UpdatedByUserID = null;
        }
    }
}