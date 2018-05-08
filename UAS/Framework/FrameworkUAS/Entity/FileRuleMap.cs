using System;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class FileRuleMap
    {
        public FileRuleMap() { }

        #region Properties
        [DataMember]
        public int RulesAssignedID { get; set; }
	    [DataMember]
        public int RulesID { get; set; }
        [DataMember]
        public int SourceFileID { get; set; }
        [DataMember]
        public int RuleOrder { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
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
