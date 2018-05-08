using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using KM.Common.Functions;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class SubscriberInvalid: SubscriberInvalidTransformedBase
    {
        public SubscriberInvalid()
        {
            Title = string.Empty;
        }

        public SubscriberInvalid(int sourceFileID, Guid soRecordIdentifier, string processCode)
            : base(sourceFileID, soRecordIdentifier, processCode)
        {
            SourceFileID = sourceFileID;
            DateUpdated = DateTimeFunctions.GetMinDate();
            Title = string.Empty;
        }

        public SubscriberInvalid(SubscriberTransformed st)
        {
            SubscriberInvalidID = -1;
            SORecordIdentifier = st.SORecordIdentifier;
            SourceFileID = st.SourceFileID;
            PubCode = st.PubCode;
            Sequence = st.Sequence;
            FName = st.FName;
            LName = st.LName;
            Title = st.Title;
            Company = st.Company;
            Address = st.Address;
            MailStop = st.MailStop;
            City = st.City;
            State = st.State;
            Zip = st.Zip;
            Plus4 = st.Plus4;
            ForZip = st.ForZip;
            County = st.County;
            Country = st.Country;
            CountryID = st.CountryID;
            Phone = st.Phone;
            Fax = st.Fax;
            Email = st.Email;
            CategoryID = st.CategoryID;
            TransactionID = st.TransactionID;
            QSourceID = st.QSourceID;
            RegCode = st.RegCode;
            Verified = st.Verified;
            SubSrc = st.SubSrc;
            OrigsSrc = st.OrigsSrc;
            Par3C = st.Par3C;
            Source = st.Source;
            Priority = st.Priority;
            Sic = st.Sic;
            SicCode = st.SicCode;
            Gender = st.Gender;
            Address3 = st.Address3;
            Home_Work_Address = st.Home_Work_Address;
            Demo7 = st.Demo7;
            Mobile = st.Mobile;
            Latitude = st.Latitude;
            Longitude = st.Longitude;
            ImportRowNumber = st.ImportRowNumber;
            CreatedByUserID = st.CreatedByUserID;
            UpdatedByUserID = st.UpdatedByUserID;
            ProcessCode = st.ProcessCode;

            SIRecordIdentifier = Guid.NewGuid();
            TransactionDate = st.TransactionDate;
            QDate = st.QDate;
            DateUpdated = DateTimeFunctions.GetMinDate();
            MailPermission = st.MailPermission;
            FaxPermission = st.FaxPermission;
            PhonePermission = st.PhonePermission;
            OtherProductsPermission = st.OtherProductsPermission;
            ThirdPartyPermission = st.ThirdPartyPermission;
            EmailRenewPermission = st.EmailRenewPermission;
            TextPermission = st.TextPermission;
            EmailStatusID = st.EmailStatusID;
            IsActive = st.IsActive;
            ExternalKeyId = st.ExternalKeyId;
            AccountNumber = st.AccountNumber;
            EmailID = st.EmailID;
            Copies = st.Copies;
            GraceIssues = st.GraceIssues;
            IsComp = st.IsComp;
            IsPaid = st.IsPaid;
            IsSubscribed = st.IsSubscribed;
            Occupation = st.Occupation;
            SubscriptionStatusID = st.SubscriptionStatusID;
            SubsrcID = st.SubsrcID;
            Website = st.Website;

            DemographicInvalidList = new HashSet<SubscriberDemographicInvalid>();
            foreach(var sdt in st.DemographicTransformedList)
            {
                SubscriberDemographicInvalid sdi = new SubscriberDemographicInvalid();
                sdi.CreatedByUserID = sdt.CreatedByUserID;
                sdi.DateCreated = DateTime.Now;
                sdi.MAFField = sdt.MAFField;
                sdi.NotExists = sdt.NotExists;
                sdi.PubID = sdt.PubID;
                sdi.SORecordIdentifier = sdt.SORecordIdentifier;
                sdi.SIRecordIdentifier = SIRecordIdentifier;
                sdi.Value = sdt.Value;
                sdi.DemographicUpdateCodeId = sdt.DemographicUpdateCodeId;
                sdi.IsAdhoc = sdt.IsAdhoc;
                sdi.ResponseOther = sdt.ResponseOther;
                
                DemographicInvalidList.Add(sdi);
            }
            DuplicateProfiles = new HashSet<SubscriberInvalid>();

        }

        [DataMember]
        public int SubscriberInvalidID { get; set; } = NoId;

        [DataMember]
        [StringLength(100)]
        public string Title { get; set; }

        [DataMember]
        public DateTime? TransactionDate { get; set; } = DateTimeFunctions.GetMinDate();

        [DataMember]
        public bool StatList { get; set; }

        [DataMember]
        public Guid SIRecordIdentifier { get; set; } = Guid.NewGuid();

        [DataMember]
        public HashSet<SubscriberDemographicInvalid> DemographicInvalidList { get; set; } = new HashSet<SubscriberDemographicInvalid>();

        public HashSet<SubscriberInvalid> DuplicateProfiles { get; set; } = new HashSet<SubscriberInvalid>();
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
                hash = hash * mult + DemographicInvalidList.GetHashCode();
                return hash;
            }
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as SubscriberInvalid);
        }
        public bool Equals(SubscriberInvalid other)
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
                   DemographicInvalidList == other.DemographicInvalidList)
            {
                DuplicateProfiles.Add(other);
                return true;
            }
            else
                return false;
        }
        #endregion
    }
}
