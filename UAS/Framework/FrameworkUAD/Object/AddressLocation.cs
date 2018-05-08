using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class AddressLocation
    {
        public AddressLocation() { }
        public AddressLocation(string _Street = "", string _City = "", string _Region = "", string _PostalCode = "", decimal _Latitude = 0, decimal _Longitude = 0,
                               bool _IsAddressValidated = false,string _AddressValidationSource = "",string _AddressValidationMessage = "")
        {
            Street = _Street;
            City = _City;
            Region = _Region;
            PostalCode = _PostalCode;
            Latitude = _Latitude;
            Longitude = _Longitude;
            IsAddressValidated = _IsAddressValidated;
            AddressValidationSource = _AddressValidationSource;
            AddressValidationMessage = _AddressValidationMessage;
            AddressValidationDate = DateTime.Now;
            UpdatedByUserID = 1;
        }
        #region properties
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string Region { get; set; }
        [DataMember]
        public string PostalCode { get; set; }
        [DataMember]
        public string Country { get; set; }
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
        public int UpdatedByUserID { get; set; }

        #endregion

    }
}
