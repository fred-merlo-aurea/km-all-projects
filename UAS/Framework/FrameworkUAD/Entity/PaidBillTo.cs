using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class PaidBillTo
    {
        public PaidBillTo()
        {
            PaidBillToID = 0;
            SubscriptionPaidID = 0;
            PubSubscriptionID = 0;
            FirstName = "";
            LastName = "";
            Company = "";
            Title = "";
            AddressTypeId = 0;
            Address1 = "";
            Address2 = "";
            Address3 = "";
            City = "";
            RegionCode = "";
            RegionID = 0;
            ZipCode = "";
            Plus4 = "";
            CarrierRoute = "";
            County = "";
            Country = "";
            CountryID = 0;
            Latitude = 0;
            Longitude = 0;
            IsAddressValidated = false;
            AddressValidationDate = DateTime.Now;
            AddressValidationSource = "";
            AddressValidationMessage = "";
            Email = "";
            Phone = "";
            PhoneExt = "";
            Fax = "";
            Mobile = "";
            Website = "";
            DateCreated = DateTime.Now;
        }
        public PaidBillTo(PaidBillTo p)
        {
            PaidBillToID = p.PaidBillToID;
            SubscriptionPaidID = p.SubscriptionPaidID; 
            PubSubscriptionID = p.PubSubscriptionID;    
            FirstName = p.FirstName ?? "";
            LastName = p.LastName ?? "";
            Company = p.Company ?? "";
            Title = p.Title ?? "";   
            AddressTypeId = p.AddressTypeId;
            Address1 = p.Address1 ?? "";
            Address2 = p.Address2 ?? "";
            Address3 = p.Address3 ?? "";
            City = p.City ?? "";
            RegionCode = p.RegionCode ?? "";      
            RegionID = p.RegionID;
            ZipCode = p.ZipCode ?? "";
            Plus4 = p.Plus4 ?? "";
            CarrierRoute = p.CarrierRoute ?? "";
            County = p.County ?? "";
            Country = p.Country ?? "";
            CountryID = p.CountryID;
            Latitude = p.Latitude;
            Longitude = p.Longitude;
            IsAddressValidated = p.IsAddressValidated;   
            AddressValidationDate = p.AddressValidationDate;
            AddressValidationSource = p.AddressValidationSource ?? "";
            AddressValidationMessage = p.AddressValidationMessage ?? "";
            Email = p.Email ?? "";
            Phone = p.Phone ?? "";
            PhoneExt = p.PhoneExt ?? "";
            Fax = p.Fax ?? "";
            Mobile = p.Mobile ?? "";
            Website = p.Website ?? "";     
            DateCreated = p.DateCreated;
            DateUpdated = p.DateUpdated;  
            CreatedByUserID = p.CreatedByUserID;
            UpdatedByUserID = p.UpdatedByUserID;
        } 

        #region Properties
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
        public int AddressTypeId { get; set; }
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
        public string PhoneExt { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public string Mobile { get; set; }
        [DataMember]
        public string Website { get; set; }
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
