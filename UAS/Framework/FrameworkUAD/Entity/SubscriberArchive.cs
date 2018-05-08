using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using KM.Common.Functions;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class SubscriberArchive : SubscriberArchiveBase
    {
        public SubscriberArchive()
        {
            SubscriberArchiveID = NoId;
            SFRecordIdentifier = Guid.Empty;
            Title = string.Empty;

            Initialize();
        }

        public SubscriberArchive(int sourceFileID, Guid sfRecordIdentifier, string processCode)
            : base(processCode)
        {
            SFRecordIdentifier = sfRecordIdentifier;
            SourceFileID = sourceFileID;
            PubCode = null;
            Sequence = ZeroInt;
            FName = null;
            LName = null;
            Title = null;
            Company = null;
            Address = null;
            MailStop = null;
            City = null;
            State = null;
            Zip = null;
            Plus4 = null;
            ForZip = null;
            County = null;
            Country = null;
            CountryID = 0;
            CategoryID = ZeroId;
            TransactionID = ZeroId;
            QSourceID = ZeroId;
            RegCode = null;
            Verified = null;
            SubSrc = null;
            OrigsSrc = null;
            Par3C = null;
            Source = null;
            Priority = null;
            IGrp_Cnt = ZeroInt;
            CGrp_Cnt = ZeroInt;
            Sic = null;
            SicCode = null;
            Gender = null;
            IGrp_Rank = null;
            CGrp_Rank = null;
            Address3 = null;
            Home_Work_Address = null;
            Demo7 = null;
            Latitude = ZeroInt;
            Longitude = ZeroInt;
            CreatedByUserID = ZeroId;
            UpdatedByUserID = null;
            ImportRowNumber = ZeroInt;

            Initialize();
            ClearPhoneMobileFax();
        }

        [DataMember]
        public int SubscriberArchiveID { get; set; }

        [DataMember]
        [StringLength(100)]
        public string Title { get; set; }

        [DataMember]
        public Guid SFRecordIdentifier { get; set; }

        [DataMember]
        public int? EmailStatusID { get; set; }

        [DataMember]
        public bool IsMailable { get; set; }

        [DataMember]
        public Guid SARecordIdentifier { get; set; }

        [DataMember]
        public List<SubscriberDemographicArchive> DemographicArchiveList { get; set; }

        public List<SubscriberArchive> DuplicateProfiles { get; set; }

        #region HashSet support overrides
        //item.PubCode, item.FName, item.LName, item.Company, item.Title, item.Address, item.Email, item.Phone
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int mult = 486187739;
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * mult + PubCode.GetHashCode();
                hash = hash * mult + FName.GetHashCode();
                hash = hash * mult + LName.GetHashCode();
                hash = hash * mult + Company.GetHashCode();
                hash = hash * mult + Title.GetHashCode();
                hash = hash * mult + Address.GetHashCode();
                hash = hash * mult + Email.GetHashCode();
                hash = hash * mult + Phone.GetHashCode();
                hash = hash * mult + Sequence.GetHashCode();
                hash = hash * mult + PubCode.GetHashCode();
                hash = hash * mult + AccountNumber.GetHashCode();
                hash = hash * mult + DemographicArchiveList.GetHashCode();
                return hash;
            }
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as SubscriberArchive);
        }
        public bool Equals(SubscriberArchive other)
        {
            // Check for null
            if (ReferenceEquals(other, null))
                return false;

            // Check for same reference
            if (ReferenceEquals(this, other))
                return true;

            // Check for same Values
            if (FName == other.FName &&
                   LName == other.LName &&
                   Company == other.Company &&
                   Title == other.Title &&
                   Address == other.Address &&
                   Email == other.Email &&
                   Phone == other.Phone &&
                   Sequence == other.Sequence &&
                   PubCode == other.PubCode &&
                   AccountNumber == other.AccountNumber &&
                   DemographicArchiveList == other.DemographicArchiveList)
            {
                DuplicateProfiles.Add(other);
                return true;
            }
            else
                return false;
        }
        #endregion

        private void Initialize()
        {
            IGrp_No = Guid.NewGuid();
            TransactionDate = DateTimeFunctions.GetMinDate();
            QDate = DateTime.Now;
            DemographicArchiveList = new List<SubscriberDemographicArchive>();
            EmailStatusID = StatusOne;
            IsMailable = true;
            SARecordIdentifier = Guid.NewGuid();
        }
    }
}
