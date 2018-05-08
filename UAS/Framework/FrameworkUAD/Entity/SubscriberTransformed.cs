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
    public class SubscriberTransformed : SubscriberInvalidTransformedBase, ITransformedSubscriber
    {
        public SubscriberTransformed()
        {
            STRecordIdentifier = Guid.NewGuid();
            Title = string.Empty;
        }

        public SubscriberTransformed(int sourceFileID, Guid soRecordIdentifier, string processCode)
            : base(sourceFileID, soRecordIdentifier, processCode)
        {
            Title = string.Empty;
        }

        [DataMember]
        public int SubscriberTransformedID { get; set; } = NoId;

        [DataMember]
        [StringLength(100)]
        public string Title { get; set; }

        [DataMember]
        [DisplayName("PubTransactionDate")]
        public DateTime? TransactionDate { get; set; } = DateTimeFunctions.GetMinDate();

        [DataMember]
        public Guid STRecordIdentifier { get; set; }

        public int OriginalImportRow { get; set; }

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
        public string SubGenRenewalCode { get; set; } = string.Empty;

        [DataMember]
        public DateTime? SubGenSubscriptionRenewDate { get; set; }

        [DataMember]
        public DateTime? SubGenSubscriptionExpireDate { get; set; }

        [DataMember]
        public DateTime? SubGenSubscriptionLastQualifiedDate { get; set; }

        [DataMember]
        public bool IsLatLonValid { get; set; }

        [DataMember]
        [StringLength(500)]
        public string LatLonMsg { get; set; } = string.Empty;

        [DataMember]
        public HashSet<SubscriberDemographicTransformed> DemographicTransformedList { get; set; } 
            = new HashSet<SubscriberDemographicTransformed>();

        public HashSet<SubscriberTransformed> DuplicateProfiles { get; set; }
            = new HashSet<SubscriberTransformed>();

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
                hash = hash * mult + DemographicTransformedList.GetHashCode();
                return hash;
            }
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as SubscriberTransformed);
        }
        public bool Equals(SubscriberTransformed other)
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
                   DemographicTransformedList == other.DemographicTransformedList)
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
