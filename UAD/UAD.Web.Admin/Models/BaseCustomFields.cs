using System;
using System.ComponentModel.DataAnnotations;

namespace UAD.Web.Admin.Models
{
    public abstract class BaseCustomFields : BaseWrapper
    {
        public string StandardField { get; set; }

        [Display(Name = "Custom Field :")]
        [Required(ErrorMessage = "Please provide Custom Field.")]
        [MaxLength(255, ErrorMessage = "File name cannot exceed 255 characters.")]
        public string CustomField { get; set; }

        [Display(Name = "Data Type :")]
        [Required(ErrorMessage = "Please select Data Type.")]
        public string CustomFieldDataType { get; set; }

        [Display(Name = "Active :")]
        public bool Active { get; set; }

        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? CreatedByUserID { get; set; }
        public int? UpdatedByUserID { get; set; }
    }
}