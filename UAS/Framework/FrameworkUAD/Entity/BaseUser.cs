using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    public class BaseUser : CoreUserProperty
    {
        public BaseUser(bool defaultValue = true) : base(defaultValue)
        {
            if (defaultValue)
            {
                AddressTypeID = 0;
                RegionID = 0;
                CountryID = 0;
                IsAddressValidated = false;
                AddressValidationSource = string.Empty;
                AddressValidationMessage = string.Empty;
                IsLocked = false;
                LockedByUserID = 0;
                IsInActiveWaveMailing = false;
                WaveMailingID = 0;
                AddressTypeCodeId = 0;
                AddressUpdatedSourceTypeCodeId = 0;
                IsNewSubscription = false;
            }
        }

        [DataMember]
        public bool IsNewSubscription { get; set; }

        [DataMember]
        public int AddressTypeID { get; set; }

        [DataMember]
        public int RegionID { get; set; }

        [DataMember]
        public int CountryID { get; set; }

        [DataMember]
        public bool IsAddressValidated { get; set; }

        [DataMember]
        public DateTime? AddressValidationDate { get; set; }

        [DataMember]
        public string AddressValidationSource { get; set; }

        [DataMember]
        public string AddressValidationMessage { get; set; }

        [DataMember]
        public bool IsLocked { get; set; }

        [DataMember]
        public int LockedByUserID { get; set; }

        [DataMember]
        public bool IsInActiveWaveMailing { get; set; }

        [DataMember]
        public int WaveMailingID { get; set; }

        [DataMember]
        public int AddressTypeCodeId { get; set; }

        [DataMember]
        public int AddressUpdatedSourceTypeCodeId { get; set; }

        [DataMember]
        public int CreatedByUserID { get; set; }

        [DataMember]
        public int? UpdatedByUserID { get; set; }
    }
}