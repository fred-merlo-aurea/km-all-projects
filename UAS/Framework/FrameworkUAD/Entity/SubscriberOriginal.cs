using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using KM.Common.Functions;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class SubscriberOriginal : SubscriberOriginalBase, IOriginalSubscriber
    {
        public SubscriberOriginal() 
        {
            Initialize();
        }

        public SubscriberOriginal(int sourceFileID, int rowNumber, string processCode)
            : base(processCode)
        {
            SourceFileID = sourceFileID;
            ImportRowNumber = rowNumber;
            Initialize();
        }

        [DataMember]
        [StringLength(100)]
        public string Title { get; set; }

        [DataMember]
        public int SubscriberOriginalID { get; set; }

        [DataMember]
        public int Score { get; set; }

        [DataMember]
        public int EmailStatusID { get; set; }

        [DataMember]
        public Guid SORecordIdentifier { get; set; }

        [DataMember]
        public DateTime? TransactionDate { get; set; }

        [DataMember]
        public DateTime? QDate { get; set; } 

        [DataMember]
        public HashSet<SubscriberDemographicOriginal> DemographicOriginalList { get; set; }

        public HashSet<SubscriberOriginal> DuplicateProfiles { get; set; }

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
                hash = hash * mult + DemographicOriginalList.GetHashCode();
                return hash;
            }
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as SubscriberOriginal);
        }
        public bool Equals(SubscriberOriginal other)
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
                   DemographicOriginalList == other.DemographicOriginalList)
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
            SubscriberOriginalID = -1;
            Title = string.Empty;
            Verified = string.Empty;
            Score = -1;
            SORecordIdentifier = Guid.NewGuid();
            TransactionDate = DateTimeFunctions.GetMinDate();
            QDate = DateTime.Now;
            DemographicOriginalList = new HashSet<SubscriberDemographicOriginal>();
            EmailStatusID = 1;
            DuplicateProfiles = new HashSet<SubscriberOriginal>();
        }
    }
}
