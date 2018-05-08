using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UAD.Web.Admin.Models
{
    public class ProductTypes : BaseWrapper
    {
        public int PubTypeID { get; set; }

        [Display(Name = "Display Name :")]
        [Required(ErrorMessage = "Please provide Display Name.")]
        [MaxLength(50, ErrorMessage = "File name cannot exceed 50 characters.")]
        public string PubTypeDisplayName { get; set; }
        public string ColumnReference { get; set; }

        [Display(Name = "Active :")]
        public bool IsActive { get; set; }

        [Display(Name = "Sort Order :")]
        public int SortOrder { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public int? CreatedByUserID { get; set; }
        public int? UpdatedByUserID { get; set; }

        public ProductTypes()
        {
            PubTypeID = 0;
            PubTypeDisplayName = string.Empty;
            ColumnReference = string.Empty;
            IsActive = false;
            SortOrder = 0;
            DateCreated = DateTime.Now;
            DateUpdated = null;
            CreatedByUserID = 1;
            UpdatedByUserID = null;
        }
    }
}