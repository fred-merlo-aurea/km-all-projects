using System;
using System.ComponentModel.DataAnnotations;

namespace UAD.Web.Admin.Models
{
    public class ProductCustomFields : BaseCustomFields
    {
        [Display(Name = "Product :")]
        public int PubID { get; set; }
        public int PubSubscriptionsExtensionMapperID { get; set; }
        public int TotalRecordCounts { get; set; }

        public ProductCustomFields()
        {
            PubSubscriptionsExtensionMapperID = 0;
            PubID = 0;
            StandardField = string.Empty;
            CustomField = string.Empty;
            CustomFieldDataType = string.Empty;
            Active = false;
            DateCreated = DateTime.Now;
            DateUpdated = null;
            CreatedByUserID = 1;
            UpdatedByUserID = null;
        }
    }
}