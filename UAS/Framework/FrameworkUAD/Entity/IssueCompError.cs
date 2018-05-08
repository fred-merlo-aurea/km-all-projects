using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class IssueCompError
    {
        public IssueCompError() { }
        #region Properties
        [DataMember]
        public int IssueCompErrorID { get; set; }
        [DataMember]
        public string CompName { get; set; }
	    [DataMember]
        public Guid SFRecordIdentifier { get; set; }
	    [DataMember]
        public string ProcessCode { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        #endregion
    }
}
