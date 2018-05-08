using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UAD.Web.Admin.Models
{
    public class MasterGroups : BaseWrapper
    {
        public int MasterGroupID { get; set; }
        [Display(Name = "Name :")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "Display Name :")]
        public string DisplayName { get; set; }
        public string ColumnReference { get; set; }
        public int SortOrder { get; set; }
        [Display(Name = "Active :")]
        public bool IsActive { get; set; }
        [Display(Name = "Enable SubReporting :")]
        public bool EnableSubReporting { get; set; }
        [Display(Name = "Enable Searching :")]
        public bool EnableSearching { get; set; }
        [Display(Name = "Enable Adhoc Search :")]
        public bool EnableAdhocSearch { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? CreatedByUserID { get; set; }
        public int? UpdatedByUserID { get; set; }
        public int TotalRecordCounts { get; set; }

        public MasterGroups()
        {
            MasterGroupID = 0;
            Name = string.Empty;
            Description = string.Empty;
            DisplayName = string.Empty;
            ColumnReference = string.Empty;
            SortOrder = 0;
            IsActive = false;
            EnableSubReporting = false;
            EnableSearching = false;
            EnableAdhocSearch = false;
            DateCreated = DateTime.Now;
            DateUpdated = null;
            CreatedByUserID = 1;
            UpdatedByUserID = null;
        }
    }
}