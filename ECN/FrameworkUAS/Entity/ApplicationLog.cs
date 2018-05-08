using System;
using System.Runtime.Serialization;

namespace KMPlatform.Entity
{
    [Serializable]
    [DataContract]
    public class ApplicationLog
    {
        public ApplicationLog()
        {
            ApplicationLogId = 0;
            ApplicationId = 0;
            SeverityCodeId = 0;
            SourceMethod = string.Empty;
            Exception = string.Empty;
            LogNote = string.Empty;
            IsBug = false;
            IsUserSubmitted = false;
            ClientId = 0;
            SubmittedBy = string.Empty;
            SubmittedByEmail = string.Empty;
            IsFixed = false;
            FixedDate = null;
            FixedTime = null;
            FixedBy = string.Empty;
            FixedNote = string.Empty;
            LogAddedDate = DateTime.Now;
            LogAddedTime = DateTime.Now.TimeOfDay;
            LogUpdatedDate = null;
            LogUpdatedTime = null;
            NotificationSent = false;
        }
        #region Properties
        [DataMember]
        public int ApplicationLogId { get; set; }
        [DataMember]
        public int ApplicationId { get; set; }
        [DataMember]
        public int SeverityCodeId { get; set; }
        [DataMember]
        public string SourceMethod { get; set; }
        [DataMember]
        public string Exception { get; set; }
        [DataMember]
        public string LogNote { get; set; }
        [DataMember]
        public bool IsBug { get; set; }
        [DataMember]
        public bool IsUserSubmitted { get; set; }
        [DataMember]
        public int ClientId { get; set; }
        [DataMember]
        public string SubmittedBy { get; set; }
        [DataMember]
        public string SubmittedByEmail { get; set; }
        [DataMember]
        public bool IsFixed { get; set; }
        [DataMember]
        public DateTime? FixedDate { get; set; }
        [DataMember]
        public TimeSpan? FixedTime { get; set; }
        [DataMember]
        public string FixedBy { get; set; }
        [DataMember]
        public string FixedNote { get; set; }
        [DataMember]
        public DateTime LogAddedDate { get; set; }
        [DataMember]
        public TimeSpan LogAddedTime { get; set; }
        [DataMember]
        public DateTime? LogUpdatedDate { get; set; }
        [DataMember]
        public TimeSpan? LogUpdatedTime { get; set; }
        [DataMember]
        public bool NotificationSent { get; set; }
        #endregion
    }
}
