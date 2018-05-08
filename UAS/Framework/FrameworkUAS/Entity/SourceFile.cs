using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class SourceFile : SourceFileBase
    {
        public SourceFile()
        {
            RuleSets = new HashSet<Object.RuleSet>();
            FieldMappings = new HashSet<FieldMapping>();
        }

        [DataMember]
        public string _fileTypeEnum { get; set; }
        [DataMember]
        public FrameworkUAD_Lookup.Enums.FileTypes FileTypeEnum
        {
            get
            {
                return FrameworkUAD_Lookup.Enums.GetDatabaseFileType(_fileTypeEnum);
            }
            set
            {
                _fileTypeEnum = value.ToString();
                //FileTypeEnum = value;
            }
        }

        [DataMember]
        public HashSet<FieldMapping> FieldMappings;
        [IgnoreDataMember]
        public HashSet<Object.RuleSet> RuleSets { get; set; }
        //[DataMember]
        //public HashSet<Object.RuleSet> RuleSetsDefault { get; set; }

        #region HashSet support overrides
        //item.PubCode, item.FName, item.LName, item.Company, item.Title, item.Address, item.Email, item.Phone
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int mult = 486187739;
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * mult + SourceFileID.GetHashCode();
                hash = hash * mult + FileRecurrenceTypeId.GetHashCode();
                hash = hash * mult + DatabaseFileTypeId.GetHashCode();
                hash = hash * mult + FileName.GetHashCode();
                hash = hash * mult + ClientID.GetHashCode();
                hash = hash * mult + PublicationID.GetHashCode();
                hash = hash * mult + IsDeleted.GetHashCode();
                hash = hash * mult + IsIgnored.GetHashCode();
                hash = hash * mult + FileSnippetID.GetHashCode();
                hash = hash * mult + Extension.GetHashCode();
                hash = hash * mult + IsDQMReady.GetHashCode();
                if (Delimiter != null)
                    hash = hash * mult + Delimiter.GetHashCode();
                hash = hash * mult + IsTextQualifier.GetHashCode();
                hash = hash * mult + ServiceID.GetHashCode();
                hash = hash * mult + ServiceFeatureID.GetHashCode();
                hash = hash * mult + MasterGroupID.GetHashCode();
                hash = hash * mult + UseRealTimeGeocoding.GetHashCode();
                hash = hash * mult + IsSpecialFile.GetHashCode();
                hash = hash * mult + ClientCustomProcedureID.GetHashCode();
                hash = hash * mult + SpecialFileResultID.GetHashCode();
                hash = hash * mult + DateCreated.GetHashCode();
                if (DateUpdated != null)
                    hash = hash * mult + DateUpdated.GetHashCode();
                hash = hash * mult + CreatedByUserID.GetHashCode();
                if (UpdatedByUserID != null)
                    hash = hash * mult + UpdatedByUserID.GetHashCode();
                hash = hash * mult + QDateFormat.GetHashCode();
                hash = hash * mult + BatchSize.GetHashCode();
                hash = hash * mult + IsPasswordProtected.GetHashCode();
                hash = hash * mult + ProtectionPassword.GetHashCode();
                return hash;
            }
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as SourceFile);
        }
        public bool Equals(SourceFile other)
        {
            // Check for null
            if (ReferenceEquals(other, null))
                return false;

            // Check for same reference
            if (ReferenceEquals(this, other))
                return true;

            // Check for same Values
            return (SourceFileID == other.SourceFileID &&
                   FileRecurrenceTypeId == other.FileRecurrenceTypeId &&
                   DatabaseFileTypeId == other.DatabaseFileTypeId &&
                   FileName == other.FileName &&
                   ClientID == other.ClientID &&
                   PublicationID == other.PublicationID &&
                   IsDeleted == other.IsDeleted &&
                   IsIgnored == other.IsIgnored &&
                   FileSnippetID == other.FileSnippetID &&
                   Extension == other.Extension &&
                   IsDQMReady == other.IsDQMReady &&
                   Delimiter == other.Delimiter &&
                   IsTextQualifier == other.IsTextQualifier &&
                   ServiceID == other.ServiceID &&
                   ServiceFeatureID == other.ServiceFeatureID &&
                   MasterGroupID == other.MasterGroupID &&
                   UseRealTimeGeocoding == other.UseRealTimeGeocoding &&
                   IsSpecialFile == other.IsSpecialFile &&
                   ClientCustomProcedureID == other.ClientCustomProcedureID &&
                   SpecialFileResultID == other.SpecialFileResultID &&
                   DateCreated == other.DateCreated &&
                   DateUpdated == other.DateUpdated &&
                   CreatedByUserID == other.CreatedByUserID &&
                   UpdatedByUserID == other.UpdatedByUserID &&
                   QDateFormat == other.QDateFormat &&
                   BatchSize == other.BatchSize &&
                   IsPasswordProtected == other.IsPasswordProtected &&
                   ProtectionPassword == other.ProtectionPassword);


        }
        #endregion
    }
}
