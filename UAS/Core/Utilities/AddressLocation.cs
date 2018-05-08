using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Core_AMS.Utilities
{
    [Serializable]
    [DataContract]
    public class AddressLocation
    {
        public AddressLocation() 
        {
            Street = string.Empty;
            MailStop = string.Empty;
            City = string.Empty;
            Region = string.Empty;
            PostalCode = string.Empty;
            PostalCodePlusFour = string.Empty;
            County = string.Empty;
            Country = string.Empty;
            Latitude = 0;
            Longitude = 0;
            IsValid = false;
            ValidationMessage = string.Empty;
            DateUpdated = DateTime.Now;
            ModifiedByID = 1;
            SORecordIdentifier = Guid.Empty;
            StreetNumber = string.Empty;
            StreetName = string.Empty;
            RecordIdentifier = 0;
            ErrorOccured = false;
        }

        public AddressLocation(string _Street = "", string _MailStop = "", string _City = "", string _Region = "", string _PostalCode = "", double _Latitude = 0, double _Longitude = 0, bool _IsValid = false,
                        string _ValidationMessage = "")
        {
            Street = _Street;
            MailStop = _MailStop;
            City = _City;
            Region = _Region;
            PostalCode = _PostalCode;
            Latitude = _Latitude;
            Longitude = _Longitude;
            IsValid = _IsValid;
            ValidationMessage = _ValidationMessage;
            DateUpdated = DateTime.Now;
            ModifiedByID = 1;
            SORecordIdentifier = Guid.Empty;
            ErrorOccured = false;
        }
        #region properties

        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string MailStop { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string Region { get; set; }
        [DataMember]
        public string PostalCode { get; set; }
        [DataMember]
        public string PostalCodePlusFour { get; set; }
        [DataMember]
        public string County { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public double Latitude { get; set; }
        [DataMember]
        public double Longitude { get; set; }
        [DataMember]
        public bool IsValid { get; set; }
        [DataMember]
        public string ValidationMessage { get; set; }
        [DataMember]
        public DateTime DateUpdated { get; set; }
        [DataMember]
        public int ModifiedByID { get; set; }
        [DataMember]
        public Guid SORecordIdentifier { get; set; }
        [DataMember]
        public string StreetNumber { get; set; }
        [DataMember]
        public string StreetName { get; set; }
        [DataMember]
        public int RecordIdentifier { get; set; }
        [DataMember]
        public bool ErrorOccured { get; set; }

        #endregion

    }
}
