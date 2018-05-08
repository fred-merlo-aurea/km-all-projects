using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using KM.Common.Functions;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class SubscriberFinal : SubscriberOriginalBase
    {
        public SubscriberFinal()
        {
            Initialize();
        }

        public SubscriberFinal(string processCode)
            : base(processCode)
        {
            Initialize();
        }

        [DataMember]
        public int SubscriberFinalID { get; set; }

        [DataMember]
        public Guid STRecordIdentifier { get; set; }

        [DataMember]
        [StringLength(100)]
        public string Title { get; set; }

        [DataMember]
        public DateTime TransactionDate { get; set; }

        [DataMember]
        public DateTime QDate { get; set; }

        [DataMember]
        public Guid IGrp_No { get; set; }

        [DataMember]
        public int IGrp_Cnt { get; set; }

        [DataMember]
        public Guid CGrp_No { get; set; }

        [DataMember]
        public int CGrp_Cnt { get; set; }

        [DataMember]
        public bool StatList { get; set; }

        [DataMember]
        [StringLength(2)]
        public string IGrp_Rank { get; set; }

        [DataMember]
        [StringLength(2)]
        public string CGrp_Rank { get; set; }

        [DataMember]
        public int? EmailStatusID { get; set; }

        [DataMember]
        public bool IsMailable { get; set; }

        [DataMember]
        public bool Ignore { get; set; }

        [DataMember]
        public bool IsDQMProcessFinished { get; set; }

        [DataMember]
        public DateTime DQMProcessDate { get; set; }

        [DataMember]
        public bool IsUpdatedInLive { get; set; }

        [DataMember]
        public DateTime UpdateInLiveDate { get; set; }

        [DataMember]
        public Guid SFRecordIdentifier { get; set; }

        [DataMember]
        public int SubGenSubscriberID { get; set; }

        [DataMember]
        public int SubGenSubscriptionID { get; set; }

        [DataMember]
        public int SubGenPublicationID { get; set; }

        [DataMember]
        public int SubGenMailingAddressId { get; set; }

        [DataMember]
        public int SubGenBillingAddressId { get; set; }

        [DataMember]
        public int IssuesLeft { get; set; }

        [DataMember]
        public decimal UnearnedReveue { get; set; }

        [DataMember]
        public bool SubGenIsLead { get; set; }

        [DataMember]
        public string SubGenRenewalCode { get; set; }

        [DataMember]
        public DateTime? SubGenSubscriptionRenewDate { get; set; }

        [DataMember]
        public DateTime? SubGenSubscriptionExpireDate { get; set; }

        [DataMember]
        public DateTime? SubGenSubscriptionLastQualifiedDate { get; set; }

        [DataMember]
        public bool ConditionApplied { get; set; }

        [DataMember]
        public bool IsNewRecord { get; set; }

        [DataMember]
        public bool IsLatLonValid { get; set; }

        [DataMember]
        [StringLength(500)]
        public string LatLonMsg { get; set; } = string.Empty;

        [DataMember]
        public HashSet<SubscriberDemographicFinal> DemographicFinalList { get; set; }

        public HashSet<SubscriberFinal> DuplicateProfiles { get; set; }

        private void Initialize()
        {
            SubscriberFinalID = NoId;
            STRecordIdentifier = Guid.Empty;
            Title = string.Empty;
            IGrp_Cnt = NoInt;
            CGrp_No = Guid.NewGuid();
            CGrp_Cnt = NoInt;
            StatList = false;
            IGrp_Rank = string.Empty;
            CGrp_Rank = string.Empty;
            IsLatLonValid = false;
            LatLonMsg = string.Empty;
            IsMailable = false;
            Ignore = false;
            IsDQMProcessFinished = false;
            IsUpdatedInLive = false;
            SFRecordIdentifier = Guid.NewGuid();
            IGrp_No = Guid.NewGuid();
            TransactionDate = DateTimeFunctions.GetMinDate();
            QDate = DateTime.Now;
            DQMProcessDate = DateTimeFunctions.GetMinDate();
            UpdateInLiveDate = DateTimeFunctions.GetMinDate();
            DemographicFinalList = new HashSet<SubscriberDemographicFinal>();
            EmailStatusID = 1;
            SubGenIsLead = false;
            SubGenRenewalCode = string.Empty;
            SubGenSubscriptionRenewDate = null;
            SubGenSubscriptionExpireDate = null;
            SubGenSubscriptionLastQualifiedDate = null;
            ConditionApplied = false;
            IsNewRecord = false;
            DuplicateProfiles = new HashSet<SubscriberFinal>();
        }

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
                hash = hash * mult + DemographicFinalList.GetHashCode();
                return hash;
            }
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as SubscriberFinal);
        }
        public bool Equals(SubscriberFinal other)
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
                   DemographicFinalList == other.DemographicFinalList)
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
