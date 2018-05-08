using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class HistoryPaidBillTo
    {
        public HistoryPaidBillTo() { }

        #region Properties
        [DataMember]
        public int HistoryPaidBillToID { get; set; }
        [DataMember]
        public int PaidBillToID { get; set; }
        [DataMember]
        public int SubscriptionPaidID { get; set; }
        [DataMember]
        public int PubSubscriptionID { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Company { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public int AddressTypeID { get; set; }
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
        public int RegionID { get; set; }
        [DataMember]
        public string ZipCode { get; set; }
        [DataMember]
        public string Plus4 { get; set; }
        [DataMember]
        public string CarrierRoute { get; set; }
        [DataMember]
        public string County { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public int CountryID { get; set; }
        [DataMember]
        public decimal Latitude { get; set; }
        [DataMember]
        public decimal Longitude { get; set; }
        [DataMember]
        public bool IsAddressValidated { get; set; }
        [DataMember]
        public DateTime? AddressValidationDate { get; set; }
        [DataMember]
        public string AddressValidationSource { get; set; }
        [DataMember]
        public string AddressValidationMessage { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public string Mobile { get; set; }
        [DataMember]
        public string Website { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        #endregion
    }
}
