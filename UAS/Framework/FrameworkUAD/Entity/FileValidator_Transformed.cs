using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using KM.Common.Functions;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class FileValidator_Transformed : SubscriberArchiveBase
    {
        private const string StatusUpdatedReasonDefaultValue = "Subscribed";

        public FileValidator_Transformed()
        {
            SourceFileID = NoInt;
            ProcessCode = string.Empty;
            SetDefaultValue();
        }

        public FileValidator_Transformed(int sourceFileId, string processCode)
        {
            SourceFileID = sourceFileId;
            ProcessCode = processCode;
            SetDefaultValue();
        }

        private void SetDefaultValue()
        {
            FV_TransformedID = NoInt;
            PubCode = string.Empty;
            Sequence = NoInt;
            FName = string.Empty;
            LName = string.Empty;
            Title = string.Empty;
            Company = string.Empty;
            Address = string.Empty;
            MailStop = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Zip = string.Empty;
            Plus4 = string.Empty;
            ForZip = string.Empty;
            County = string.Empty;
            Country = string.Empty;
            CountryID = NoInt;
            Phone = string.Empty;
            PhoneExists = false;
            Fax = string.Empty;
            FaxExists = false;
            Email = string.Empty;
            EmailExists = false;
            CategoryID = NoInt;
            TransactionID = NoInt;
            QSourceID = NoInt;
            RegCode = string.Empty;
            Verified = string.Empty;
            SubSrc = string.Empty;
            OrigsSrc = string.Empty;
            Par3C = string.Empty;
            Source = string.Empty;
            Priority = string.Empty;
            IGrp_Cnt = NoInt;
            CGrp_No = Guid.NewGuid();
            CGrp_Cnt = NoInt;
            StatList = false;
            Sic = string.Empty;
            SicCode = string.Empty;
            Gender = string.Empty;
            IGrp_Rank = string.Empty;
            CGrp_Rank = string.Empty;
            Address3 = string.Empty;
            Home_Work_Address = string.Empty;
            PubIDs = string.Empty;
            Demo7 = string.Empty;
            IsExcluded = false;
            Mobile = string.Empty;
            Latitude = NoInt;
            Longitude = NoInt;
            IsLatLonValid = false;
            LatLonMsg = string.Empty;
            Score = NoInt;
            Ignore = false;
            IsDQMProcessFinished = false;
            IsUpdatedInLive = false;
            ImportRowNumber = NoInt;
            CreatedByUserID = NoInt;
            UpdatedByUserID = NoInt;
            DateCreated = DateTime.Now;
            STRecordIdentifier = Guid.NewGuid();
            IGrp_No = Guid.Empty;
            TransactionDate = DateTimeFunctions.GetMinDate();
            QDate = DateTime.Now;
            SuppressedDate = DateTimeFunctions.GetMinDate();
            StatusUpdatedDate = DateTimeFunctions.GetMinDate();
            DQMProcessDate = DateTimeFunctions.GetMinDate();
            UpdateInLiveDate = DateTimeFunctions.GetMinDate();
            DateUpdated = DateTimeFunctions.GetMinDate();
            FV_DemographicTransformedList = new List<FileValidator_DemographicTransformed>();
            Demo31 = true;
            Demo32 = true;
            Demo33 = true;
            Demo34 = true;
            Demo35 = true;
            Demo36 = true;
            EmailStatusID = StatusOne;
            StatusUpdatedReason = StatusUpdatedReasonDefaultValue;
            IsMailable = true;
        }

        [DataMember]
        public int FV_TransformedID { get; set; }
        [DataMember]
        [StringLength(100)]
        public string Title { get; set; }
        [DataMember]
        public bool Demo31 { get; set; }
        [DataMember]
        public bool Demo32 { get; set; }
        [DataMember]
        public bool Demo33 { get; set; }
        [DataMember]
        public bool Demo34 { get; set; }
        [DataMember]
        public bool Demo35 { get; set; }
        [DataMember]
        public bool Demo36 { get; set; }
        [DataMember]
        [StringLength(2000)]
        public string PubIDs { get; set; }
        [DataMember]
        public bool IsExcluded { get; set; }
        [DataMember]
        public bool InSuppression { get; set; }
        [DataMember]
        public int Score { get; set; }
        [DataMember]
        public DateTime? SuppressedDate { get; set; }
        [DataMember]
        public string SuppressedEmail { get; set; }
        [DataMember]
        public int EmailStatusID { get; set; }
        [DataMember]
        public DateTime? StatusUpdatedDate { get; set; }
        [DataMember]
        [StringLength(200)]
        public string StatusUpdatedReason { get; set; }
        [DataMember]
        public bool IsMailable { get; set; }
        [DataMember]
        public bool Ignore { get; set; }
        [DataMember]
        public bool IsDQMProcessFinished { get; set; }
        [DataMember]
        public DateTime? DQMProcessDate { get; set; }
        [DataMember]
        public bool IsUpdatedInLive { get; set; }
        [DataMember]
        public DateTime? UpdateInLiveDate { get; set; }
        [DataMember]
        public Guid STRecordIdentifier { get; set; }
        [DataMember]
        public List<FileValidator_DemographicTransformed> FV_DemographicTransformedList { get; set; }
        [DataMember]
        public bool IsLatLonValid { get; set; }
        [DataMember]
        [StringLength(500)]
        public string LatLonMsg { get; set; } = string.Empty;
    }
}
