using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class WaveMailingDetail
    {
        public WaveMailingDetail()
        {
            WaveMailingDetailID = 0;
            WaveMailingID = 0;
            PubSubscriptionID = 0;
            SubscriptionID = 0;
            Copies = null;
            Demo7 = string.Empty;
            PubCategoryID = 0;
            PubTransactionID = 0;
            IsSubscribed = null;
            IsPaid = null;
            SubscriptionStatusID = 0;
            FirstName = null;
            LastName = null;
            Company = null;
            Title = null;
            AddressTypeID = null;
            Address1 = null;
            Address2 = null;
            Address3 = null;
            City = null;
            RegionCode = null;
            RegionID = null;
            ZipCode = null;
            Plus4 = null;
            County = null;
            Country = null;
            CountryID = null;
            Email = null;
            Phone = null;
            PhoneExt = null;
            Fax = null;
            Mobile = null;
            DateCreated = DateTime.Now;
            DateUpdated = null;
            CreatedByUserID = 0;
            UpdatedByUserID = 0;
        }
        #region Properties
        [DataMember]
        public int WaveMailingDetailID { get; set; }
        [DataMember]
        public int WaveMailingID { get; set; }
        [DataMember]
        public int PubSubscriptionID { get; set; }
        [DataMember]
        public int SubscriptionID { get; set; }
        [DataMember]
        public string Demo7 { get; set; }
        [DataMember]
        public int PubCategoryID { get; set; }
        [DataMember]
        public int PubTransactionID { get; set; }
        [DataMember]
        public int SubscriptionStatusID { get; set; }
        [DataMember]
        public bool? IsSubscribed { get; set; }
        [DataMember]
        public bool? IsPaid { get; set; }
        [DataMember]
        public int? Copies { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Company { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public int? AddressTypeID { get; set; }
        [DataMember]
        public string Address1 { get; set; }
        [DataMember]
        public string Address2 { get; set; }
        [DataMember]
        public string Address3 { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string RegionCode { get; set; }
        [DataMember]
        public int? RegionID { get; set; }
        [DataMember]
        public string ZipCode { get; set; }
        [DataMember]
        public string Plus4 { get; set; }
        [DataMember]
        public string County { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public int? CountryID { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string PhoneExt { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public string Mobile { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        #endregion
    }
}
