using System;

namespace UAD.Web.Admin.Models
{
    public class CustomFields : BaseCustomFields
    {
        public int SubscriptionsExtensionMapperID { get; set; }

        public CustomFields()
        {
            SubscriptionsExtensionMapperID = 0;
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
