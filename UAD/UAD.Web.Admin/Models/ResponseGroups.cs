using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UAD.Web.Admin.Models
{
    public class ResponseGroups
    {
        public int ResponseGroupID { get; set; }
        [Display(Name = "Product :")]
        [Required( ErrorMessage = "Please provide Product.")]
        [RegularExpression("([1-9][0-9]*)")]
        public int PubID { get; set; }
        [Display(Name = "Name :")]
        [Required(ErrorMessage = "Please provide Name.")]
        [MaxLength(100, ErrorMessage = "File name cannot exceed 100 characters.")]
        public string ResponseGroupName { get; set; }
        [Display(Name = "Display Name :")]
        [Required(ErrorMessage = "Please provide Display Name.")]
        [MaxLength(100, ErrorMessage = "File name cannot exceed 100 characters.")]
        public string DisplayName { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? CreatedByUserID { get; set; }
        public int? UpdatedByUserID { get; set; }
        public int? DisplayOrder { get; set; }
        [Display(Name = "Multiple Value :")]
        public bool? IsMultipleValue { get; set; }
        [Display(Name = "Required :")]
        public bool? IsRequired { get; set; }
        [Display(Name = "Active :")]
        public bool? IsActive { get; set; }
        public int? WQT_ResponseGroupID { get; set; }
        [Display(Name = "KM Product :")]
        [Range(1, int.MaxValue, ErrorMessage = "Please provide KM Product.")]
        [Required(ErrorMessage = "Please provide KM Product.")]
        public int ResponseGroupTypeId { get; set; }
        public string PubCode { get; set; }
        public int TotalRecordCounts { get; set; }

        public ResponseGroups()
        {
            ResponseGroupID = 0;
            PubID = 0;
            ResponseGroupName = string.Empty;
            DisplayName = string.Empty;
            DateCreated = DateTime.Now;
            DateUpdated = null;
            CreatedByUserID = 1;
            UpdatedByUserID = null;
            DisplayOrder = null;
            IsMultipleValue = false;
            IsRequired = false;
            IsActive = true;
            WQT_ResponseGroupID = null;
            ResponseGroupTypeId = 0;
            PubCode = string.Empty;
        }
    }
}