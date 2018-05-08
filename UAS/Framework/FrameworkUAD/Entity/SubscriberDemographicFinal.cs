﻿using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class SubscriberDemographicFinal
    {
        public SubscriberDemographicFinal() 
        {
            SDFinalID = 0;
            PubID = 0;
            SFRecordIdentifier = Guid.Empty;
            MAFField = string.Empty;
            Value = string.Empty;
            NotExists = false;
            DateCreated = DateTime.Now;
            DateUpdated = null;
            CreatedByUserID = 1;
            UpdatedByUserID = null;
            DemographicUpdateCodeId = 1928;
            IsAdhoc = false;
            ResponseOther = string.Empty;
            IsDemoDate = false;
        }
        #region Properties
        [DataMember]
        public int SDFinalID { get; set; }
        [DataMember]
        public int PubID { get; set; }
        [DataMember]
        public Guid SFRecordIdentifier { get; set; }
        [DataMember]
        public string MAFField { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public bool NotExists { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        [DataMember]
        public int DemographicUpdateCodeId { get; set; }
        [DataMember]
        public bool IsAdhoc { get; set; }
        [DataMember]
        public string ResponseOther { get; set; }
        [DataMember]
        public bool IsDemoDate { get; set; }
        #endregion
        #region HashSet support overrides
        // join dd in deDupeMatch.DemographicOriginalList on excd.MAFField equals dd.MAFField
        // where excd.Value != dd.Value
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int mult = 486187739;
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * mult + PubID.GetHashCode();
                hash = hash * mult + MAFField.GetHashCode();
                hash = hash * mult + Value.GetHashCode();
                return hash;
            }
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as SubscriberDemographicFinal);
        }
        public bool Equals(SubscriberDemographicFinal other)
        {
            // Check for null
            if (ReferenceEquals(other, null))
                return false;

            // Check for same reference
            if (ReferenceEquals(this, other))
                return true;

            // Check for same Values
            return PubID == other.PubID &&
                   MAFField == other.MAFField &&
                   Value == other.Value;
        }
        #endregion
    }
}
