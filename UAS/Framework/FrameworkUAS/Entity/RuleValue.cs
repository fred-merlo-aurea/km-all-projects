using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class RuleValue
    {
        public RuleValue()
        {
            RuleValueId = 0;
            DisplayValue = string.Empty;
            Value = string.Empty;
            ValueDescription = string.Empty;
            RuleValueTypeId = 0;
            IsDefault = false;
            IsActive = true;
            DateCreated = DateTime.Now;
            CreatedByUserId = 1;
            DateUpdated = null;
            UpdatedByUserId = null;
            IsSystem = false;
            ClientId = 0;
        }
        #region Properties
        [DataMember]
        public int RuleValueId { get; set; }
        [DataMember]
        public string DisplayValue { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public string ValueDescription { get; set; }
        [DataMember]
        public int RuleValueTypeId { get; set; }
        [DataMember]
        public bool IsDefault { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserId { get; set; }
        [DataMember]
        public int? UpdatedByUserId { get; set; }
        [DataMember]
        public bool IsSystem { get; set; }
        [DataMember]
        public int ClientId { get; set; }

        private string _ruleValueTypeEnum { get; set; }
        private FrameworkUAD_Lookup.Enums.RuleValueType _rvtEnum;

        [DataMember]
        public FrameworkUAD_Lookup.Enums.RuleValueType RuleValueTypeEnum
        {
            get
            {
                if (!string.IsNullOrEmpty(_ruleValueTypeEnum))
                    _rvtEnum = FrameworkUAD_Lookup.Enums.GetRuleValueType(_ruleValueTypeEnum);
                return _rvtEnum;
            }
            //set
            //{
            //    try
            //    {
            //        RuleValueTypeEnum = value;
            //    }
            //    catch(Exception ex)
            //    {
            //        string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            //    }
            //}
        }
        #endregion

        #region HashSet support overrides
        //item.PubCode, item.FName, item.LName, item.Company, item.Title, item.Address, item.Email, item.Phone
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int mult = 486187739;
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * mult + RuleValueId.GetHashCode();
                hash = hash * mult + DisplayValue.GetHashCode();
                hash = hash * mult + Value.GetHashCode();
                hash = hash * mult + ValueDescription.GetHashCode();
                hash = hash * mult + RuleValueTypeId.GetHashCode();
                hash = hash * mult + IsDefault.GetHashCode();
                hash = hash * mult + IsActive.GetHashCode();
                hash = hash * mult + DateCreated.GetHashCode();
                hash = hash * mult + CreatedByUserId.GetHashCode();
                if(DateUpdated != null) hash = hash * mult + DateUpdated.GetHashCode();
                if(UpdatedByUserId != null) hash = hash * mult + UpdatedByUserId.GetHashCode();
                return hash;
            }
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as RuleValue);
        }
        public bool Equals(RuleValue other)
        {
            // Check for null
            if (ReferenceEquals(other, null))
                return false;

            // Check for same reference
            if (ReferenceEquals(this, other))
                return true;

            // Check for same Values
            return (RuleValueId == other.RuleValueId &&
                   DisplayValue == other.DisplayValue &&
                   Value == other.Value &&
                   ValueDescription == other.ValueDescription &&
                   RuleValueTypeId == other.RuleValueTypeId &&
                   IsDefault == other.IsDefault &&
                   IsActive == other.IsActive &&
                   DateCreated == other.DateCreated &&
                   CreatedByUserId == other.CreatedByUserId &&
                   DateUpdated == other.DateUpdated &&
                   UpdatedByUserId == other.UpdatedByUserId);

        }
        #endregion
    }
}
