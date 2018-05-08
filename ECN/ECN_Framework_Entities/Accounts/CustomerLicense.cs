using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Accounts
{
    public class CustomerLicense
    {
        public CustomerLicense()
        {
            CLID = -1;
            CustomerID = -1;
            QuoteItemID = -1;
            LicenseTypeCode = null;
            LicenseLevel = null;
            Quantity = null;
            Used = null;
            ExpirationDate = null;
            AddDate = null;
            IsActive = true;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
        }
        [DataMember]
        public int CLID { get; set; }
        [DataMember]
        public int CustomerID { get; set; }
        [DataMember]
        public int QuoteItemID { get; set; }
        [DataMember]
        public string LicenseTypeCode { get; set; }
        [DataMember]
        public string LicenseLevel { get; set; }
        [DataMember]
        public int? Quantity { get; set; }
        [DataMember]
        public int? Used { get; set; }
        [DataMember]
        public DateTime? ExpirationDate { get; set; }
        [DataMember]
        public DateTime? AddDate { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get; set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
    }
}
