using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class IssueSplit
    {
        public IssueSplit() 
        {
            IssueSplitId = 0;
            IssueId = 0;
            IssueSplitCode = string.Empty;
            IssueSplitName = string.Empty;
            IssueSplitCount = 0;
            FilterId = 0;
            DateCreated = DateTime.Now;
            DateUpdated = null;
            CreatedByUserID = 0;
            UpdatedByUserID = 0;
            IsActive = true;
            KeyCode = string.Empty;
            IssueSplitRecords = 0;
            IssueSplitDescription = "";
        }
        #region Properties
        [DataMember]
        public int IssueSplitId { get; set; }
        [DataMember]
        public int IssueId { get; set; }
        [DataMember]
        public string IssueSplitCode { get; set; }
        [DataMember]
        public string IssueSplitName { get; set; }
        [DataMember]
        public int IssueSplitCount { get; set; }
        [DataMember]
        public int IssueSplitRecords { get; set; }
        [DataMember]
        public int FilterId { get; set; }
        [DataMember]
        public Entity.Filter Filter { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string KeyCode { get; set; }
        [DataMember]
        public string IssueSplitDescription { get; set; }
        [DataMember]
        public int? WaveMailingID { get; set; }
        
        #endregion
    }
}
