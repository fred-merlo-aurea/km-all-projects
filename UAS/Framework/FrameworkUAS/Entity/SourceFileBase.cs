using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class SourceFileBase
    {
        private const int DefaultBatchSize = 2500;

        public SourceFileBase()
        {
            SourceFileID = 0;
            FileRecurrenceTypeId = 0;
            DatabaseFileTypeId = 0;
            FileName = string.Empty;
            ClientID = 0;
            PublicationID = 0;
            IsDeleted = false;
            IsIgnored = false;
            FileSnippetID = 0;
            Extension = string.Empty;
            IsDQMReady = true;
            Delimiter = string.Empty;
            IsTextQualifier = false;
            ServiceID = 0;
            ServiceFeatureID = 0;
            MasterGroupID = 0;
            UseRealTimeGeocoding = false;
            IsSpecialFile = false;
            ClientCustomProcedureID = 0;
            SpecialFileResultID = 0;
            DateCreated = DateTime.Now;
            DateUpdated = null;
            CreatedByUserID = 0;
            UpdatedByUserID = null;
            QDateFormat = string.Empty;
            BatchSize = DefaultBatchSize;
            IsPasswordProtected = false;
            ProtectionPassword = string.Empty;
 
            NotifyEmailList = string.Empty;
            IsBillable = true;
            IsFullFile = false;
            Notes = string.Empty;
        }

        [DataMember]
        public int SourceFileID { get; set; }
        [DataMember]
        public int FileRecurrenceTypeId { get; set; }
        [DataMember]
        public int DatabaseFileTypeId { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public int ClientID { get; set; }
        [DataMember]
        public int? PublicationID { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public bool IsIgnored { get; set; }
        [DataMember]
        public int FileSnippetID { get; set; }
        [DataMember]
        public string Extension { get; set; }
        [DataMember]
        public bool IsDQMReady { get; set; }
        [DataMember]
        public string Delimiter { get; set; }
        [DataMember]
        public bool IsTextQualifier { get; set; }
        [DataMember]
        public int ServiceID { get; set; }
        [DataMember]
        public int ServiceFeatureID { get; set; }
        [DataMember]
        public int MasterGroupID { get; set; }
        [DataMember]
        public bool UseRealTimeGeocoding { get; set; }
        [DataMember]
        public bool IsSpecialFile { get; set; }
        [DataMember]
        public int ClientCustomProcedureID { get; set; }
        [DataMember]
        public int SpecialFileResultID { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        [DataMember]
        public string QDateFormat { get; set; }
        [DataMember]
        public int BatchSize { get; set; }
        [DataMember]
        public bool IsPasswordProtected { get; set; }
        [DataMember]
        public string ProtectionPassword { get; set; }
        [DataMember]
        public string NotifyEmailList { get; set; }
        [DataMember]
        public bool IsBillable { get; set; }
        [DataMember]
        public bool IsFullFile { get; set; }
        [DataMember]
        public string Notes { get; set; }
    }
}
