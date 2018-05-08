using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class IssueComp
    {
        public IssueComp() 
        {
            IssueCompId = 0;
            IssueId = 0;
            ImportedDate = DateTime.Now;
            IssueCompCount = 0;
            DateCreated = DateTime.Now;
            DateUpdated = null;
            CreatedByUserID = 0;
            UpdatedByUserID = 0;
            IsActive = false;
        }
        #region Properties
        [DataMember]
        public int IssueCompId { get; set; }
        [DataMember]
        public int IssueId { get; set; }
        [DataMember]
        public DateTime ImportedDate { get; set; }
        [DataMember]
        public int IssueCompCount { get; set; }
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
        #endregion
    }
}
