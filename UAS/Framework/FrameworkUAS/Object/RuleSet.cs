using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FrameworkUAS.Object
{
    /// <summary>
    /// Object.RuleSet will be the base List<object> for the file that is being processed
    /// for the System list will get objects where IsSytem=1 and SourceFileId=0 and FileTypeId>0 (this will get all the System defined rules with defaults to use)
    ///     when a file is being mapped if the user choses to "use defaults" then there will NOT be an entry in RuleSet_File_Map for the sourceFileId
    ///     The file will instead be processed with the defined default RuleSets and values based on FileTypeId
    /// </summary>
    [Serializable]
    [DataContract]
    public class RuleSet
    {
        public RuleSet()
        {
            RuleSetId = 0;
            RuleSetName = string.Empty;
            DisplayName = string.Empty;
            RuleSetDescription = string.Empty;
            IsActive = false;
            IsSystem = false;
            IsGlobal = false;
            ClientId = 0;
            IsDateSpecific = true;
            StartMonth = null;
            EndMonth = null;
            StartDay = null;
            EndDay = null;
            StartYear = null;
            EndYear = null;
            CustomImportRuleId = 0;
            SourceFileId = 0;
            FileTypeId = 0;
            ExecutionPointId = 0;
            ExecutionOrder = 0;
            WhereClause = string.Empty;
            Rules = new HashSet<Rule>();
        }
        #region properties
        [DataMember]
        public int RuleSetId { get; set; }
        [DataMember]
        public string RuleSetName { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public string RuleSetDescription { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public bool IsSystem { get; set; }
        [DataMember]
        public bool IsGlobal { get; set; }
        [DataMember]
        public int ClientId { get; set; }
        [DataMember]
        public bool IsDateSpecific { get; set; }
        [DataMember]
        public int? StartMonth { get; set; }
        [DataMember]
        public int? EndMonth { get; set; }
        [DataMember]
        public int? StartDay { get; set; }
        [DataMember]
        public int? EndDay { get; set; }
        [DataMember]
        public int? StartYear { get; set; }
        [DataMember]
        public int? EndYear { get; set; }
        [DataMember]
        public int CustomImportRuleId { get; set; }
        [DataMember]
        public int SourceFileId { get; set; }
        [DataMember]
        public int FileTypeId { get; set; }
        [DataMember]
        public int ExecutionPointId { get; set; }
        [DataMember]
        public int ExecutionOrder { get; set; }
        [DataMember]
        public string WhereClause { get; set; }

        public string _fileTypeEnum { get; set; }
        [DataMember]
        public FrameworkUAD_Lookup.Enums.FileTypes FileTypeEnum
        {
            get
            {
                return FrameworkUAD_Lookup.Enums.GetDatabaseFileType(_fileTypeEnum);
            }
            //set
            //{
            //    _fileTypeEnum = value.ToString();
            //}
        }
        public string _executionPoint { get; set; }
        [DataMember]
        public FrameworkUAD_Lookup.Enums.ExecutionPointType ExecutionPointEnum
        {
            get
            {
                return FrameworkUAD_Lookup.Enums.GetExecutionPointTypes(_executionPoint);
            }
            //set
            //{
            //    _executionPoint = value.ToString();
            //}
        }
        [DataMember]
        public HashSet<Object.Rule> Rules { get; set; }
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
                hash = hash * mult + RuleSetId.GetHashCode();
                hash = hash * mult + RuleSetName.GetHashCode();
                hash = hash * mult + DisplayName.GetHashCode();
                hash = hash * mult + RuleSetDescription.GetHashCode();
                hash = hash * mult + IsActive.GetHashCode();
                hash = hash * mult + IsSystem.GetHashCode();
                hash = hash * mult + IsGlobal.GetHashCode();
                hash = hash * mult + ClientId.GetHashCode();
                hash = hash * mult + IsDateSpecific.GetHashCode();
                if(StartMonth != null) hash = hash * mult + StartMonth.GetHashCode();
                if(EndMonth != null) hash = hash * mult + EndMonth.GetHashCode();
                if(StartDay != null) hash = hash * mult + StartDay.GetHashCode();
                if (EndDay != null) hash = hash * mult + EndDay.GetHashCode();
                if(StartYear != null) hash = hash * mult + StartYear.GetHashCode();
                if(EndYear != null) hash = hash * mult + EndYear.GetHashCode();
                hash = hash * mult + SourceFileId.GetHashCode();
                hash = hash * mult + FileTypeId.GetHashCode();
                hash = hash * mult + ExecutionPointId.GetHashCode();
                hash = hash * mult + ExecutionOrder.GetHashCode();
                hash = hash * mult + WhereClause.GetHashCode();
                hash = hash * mult + Rules.GetHashCode();
                return hash;
            }
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as RuleSet);
        }
        public bool Equals(RuleSet other)
        {
            // Check for null
            if (ReferenceEquals(other, null))
                return false;

            // Check for same reference
            if (ReferenceEquals(this, other))
                return true;

            // Check for same Values
            return (RuleSetId == other.RuleSetId &&
                   RuleSetName == other.RuleSetName &&
                   DisplayName == other.DisplayName &&
                   RuleSetDescription == other.RuleSetDescription &&
                   IsActive == other.IsActive &&
                   IsSystem == other.IsSystem &&
                   IsGlobal == other.IsGlobal &&
                   ClientId == other.ClientId &&
                   IsDateSpecific == other.IsDateSpecific &&
                   StartMonth == other.StartMonth &&
                   EndMonth == other.EndMonth &&
                   StartDay == other.StartDay &&
                   EndDay == other.EndDay &&
                   StartYear == other.StartYear &&
                   EndYear == other.EndYear &&
                   SourceFileId == other.SourceFileId &&
                   FileTypeId == other.FileTypeId &&
                   ExecutionPointId == other.ExecutionPointId &&
                   ExecutionOrder == other.ExecutionOrder &&
                   WhereClause == other.WhereClause &&
                   Rules == other.Rules);


        }
        #endregion
    }
}
