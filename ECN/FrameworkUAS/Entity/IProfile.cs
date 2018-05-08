using System;

namespace KMPlatform.Entity
{
    public interface IProfile
    {
        int ProfileID { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Company { get; set; }
        string Title { get; set; }
        string Occupation { get; set; }
        int AddressTypeID { get; set; }
        string Address1 { get; set; }
        string Address2 { get; set; }
        string City { get; set; }
        string RegionCode { get; set; }
        int RegionID { get; set; }
        string ZipCode { get; set; }
        string Plus4 { get; set; }
        string CarrierRoute { get; set; }
        string County { get; set; }
        string Country { get; set; }
        int CountryID { get; set; }
        decimal Latitude { get; set; }
        decimal Longitude { get; set; }
        bool IsAddressValidated { get; set; }
        DateTime? AddressValidationDate { get; set; }
        string AddressValidationSource { get; set; }
        string AddressValidationMessage { get; set; }
        string Email { get; set; }
        string Phone { get; set; }
        string Fax { get; set; }
        string Mobile { get; set; }
        string Website { get; set; }
        int Age { get; set; }
        DateTime BirthDate { get; set; }
        string Income { get; set; }
        string Gender { get; set; }
        string DatabaseSource { get; set; }
        string DatabaseTable { get; set; }
        int TableID { get; set; }
        int ExternalKeyID { get; set; }
        DateTime DateCreated { get; set; }
        DateTime? DateUpdated { get; set; }
        int CreatedByUserID { get; set; }
        int? UpdatedByUserID { get; set; }
    }
}