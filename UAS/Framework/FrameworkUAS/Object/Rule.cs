using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FrameworkUAS.Object
{
    /// <summary>
    /// this is used in ADMS engine for processing files
    /// </summary>
    [Serializable]
    [DataContract]
    public class Rule
    {
        public Rule()
        {
            RuleId = 0;
            RuleName = string.Empty;
            DisplayName = string.Empty;
            RuleDescription = string.Empty;
            IsDeleteRule = false;
            IsSystem = false;
            ClientId = 0;
            IsActive = true;
            RuleSetId = 0;
            RecordTypeId = 0;
            RuleValueId = null;
            FreeTextValue = string.Empty;

            ConditionGroup = 0;
            ConditionChainId = 0;
            ConditionOrder = 0;
            ConditionBreakResult = true;
            RuleOrder = 0;
            RuleChainId = 0;
            RuleGroup = 0;
            RuleGroupChainId = 0;
            RuleGroupOrder = 0;
            RuleGroupBreakResult = false;

            RuleValues = new HashSet<Entity.RuleValue>();
        }
        #region properties
        [DataMember]
        public int RuleId { get; set; }
        [DataMember]
        public string RuleName { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public string RuleDescription { get; set; }
        [DataMember]
        public bool IsDeleteRule { get; set; }
        [DataMember]
        public bool IsSystem { get; set; }
        [DataMember]
        public int ClientId { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public int RuleSetId { get; set; }
        [DataMember]
        public int RecordTypeId { get; set; }//RuleSet_Rule_Map
        [DataMember]
        public int? RuleValueId { get; set; }
        [DataMember]
        public string FreeTextValue { get; set; }

        [DataMember]
        public int ConditionGroup { get; set; }
        [DataMember]
        public int ConditionChainId { get; set; }
        [DataMember]
        public int ConditionOrder { get; set; }
        [DataMember]
        public bool ConditionBreakResult { get; set; }
        [DataMember]
        public int RuleOrder { get; set; }
        [DataMember]
        public int RuleChainId { get; set; }
        [DataMember]
        public int RuleGroup { get; set; }
        [DataMember]
        public int RuleGroupChainId { get; set; }
        [DataMember]
        public int RuleGroupOrder { get; set; }
        [DataMember]
        public bool RuleGroupBreakResult { get; set; }



        public string _recordTypeEnum { get; set; }
        [DataMember]
        public FrameworkUAD_Lookup.Enums.RecordType RecordTypeEnum
        {
            get
            {
                return FrameworkUAD_Lookup.Enums.GetRecordType(_recordTypeEnum);
            }
            set
            {
                _recordTypeEnum = value.ToString();
            }
        }
        [DataMember]
        public HashSet<Entity.RuleValue> RuleValues { get; set; }
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
                hash = hash * mult + RuleId.GetHashCode();
                hash = hash * mult + RuleName.GetHashCode();
                hash = hash * mult + DisplayName.GetHashCode();
                hash = hash * mult + RuleDescription.GetHashCode();
                hash = hash * mult + IsDeleteRule.GetHashCode();
                hash = hash * mult + IsSystem.GetHashCode();
                hash = hash * mult + ClientId.GetHashCode();
                hash = hash * mult + IsActive.GetHashCode();
                hash = hash * mult + RuleSetId.GetHashCode();
                hash = hash * mult + RecordTypeId.GetHashCode();
                hash = hash * mult + RuleValueId.GetHashCode();
                hash = hash * mult + FreeTextValue.GetHashCode();

                hash = hash * mult + ConditionGroup.GetHashCode();
                hash = hash * mult + ConditionChainId.GetHashCode();
                hash = hash * mult + ConditionOrder.GetHashCode();
                hash = hash * mult + ConditionBreakResult.GetHashCode();
                hash = hash * mult + RuleOrder.GetHashCode();
                hash = hash * mult + RuleChainId.GetHashCode();
                hash = hash * mult + RuleGroup.GetHashCode();
                hash = hash * mult + RuleGroupChainId.GetHashCode();
                hash = hash * mult + RuleGroupOrder.GetHashCode();
                hash = hash * mult + RuleGroupBreakResult.GetHashCode();

                return hash;
            }
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Rule);
        }
        public bool Equals(Rule other)
        {
            // Check for null
            if (ReferenceEquals(other, null))
                return false;

            // Check for same reference
            if (ReferenceEquals(this, other))
                return true;

            // Check for same Values
            return (RuleId == other.RuleId &&
                   RuleName == other.RuleName &&
                   DisplayName == other.DisplayName &&
                   RuleDescription == other.RuleDescription &&
                   IsDeleteRule == other.IsDeleteRule &&
                   IsSystem == other.IsSystem &&
                   ClientId == other.ClientId &&
                   IsActive == other.IsActive &&
                   RuleSetId == other.RuleSetId &&
                   RecordTypeId == other.RecordTypeId &&
                   RuleValueId == other.RuleValueId &&
                   FreeTextValue == other.FreeTextValue &&

                   ConditionGroup == other.ConditionGroup &&
                    ConditionChainId == other.ConditionChainId &&
                    ConditionOrder == other.ConditionOrder &&
                    ConditionBreakResult == other.ConditionBreakResult &&
                    RuleOrder == other.RuleOrder &&
                    RuleChainId == other.RuleChainId &&
                    RuleGroup == other.RuleGroup &&
                    RuleGroupChainId == other.RuleGroupChainId &&
                    RuleGroupOrder == other.RuleGroupOrder &&
                    RuleGroupBreakResult == other.RuleGroupBreakResult &&

                   RuleGroup == RuleGroup &&
                   RuleValues == other.RuleValues);

        }
        #endregion
    }
}
