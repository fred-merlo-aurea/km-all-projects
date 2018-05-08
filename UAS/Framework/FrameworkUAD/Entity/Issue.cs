using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class Issue
    {
        public Issue() 
        {
            IssueId = 0;
            PublicationId = 0;
            IssueName = string.Empty;
            IssueCode = string.Empty;
            DateOpened = null;
            OpenedByUserID = 0;
            IsClosed = false;
            DateClosed = null;
            ClosedByUserID = 0;
            IsComplete = false;
            DateComplete = null;
            CompleteByUserID = 0;
            DateCreated = DateTime.Now;
            DateUpdated = null;
            CreatedByUserID = 0;
            UpdatedByUserID = 0;
        }
        #region Properties
        [DataMember]
        public int IssueId { get; set; }
        [DataMember]
        public int PublicationId { get; set; }
        [DataMember]
        public string IssueName { get; set; }
        [DataMember]
        public string IssueCode { get; set; }
        [DataMember]
        public DateTime? DateOpened { get; set; }
        [DataMember]
        public int OpenedByUserID { get; set; }
        [DataMember]
        public bool IsClosed { get; set; }
        [DataMember]
        public DateTime? DateClosed { get; set; }
        [DataMember]
        public int ClosedByUserID { get; set; }
        [DataMember]
        public bool IsComplete { get; set; }
        [DataMember]
        public DateTime? DateComplete { get; set; }
        [DataMember]
        public int CompleteByUserID { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int UpdatedByUserID { get; set; }
        #endregion
    }
}
