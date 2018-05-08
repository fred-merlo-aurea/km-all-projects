using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class RuleSetFileMap
    {
        public RuleSetFileMap() {
            RuleSetId = 0;
            SourceFileId = 0;
            FileTypeId = 0;
            IsSystem = false;
            IsGlobal = false;
            IsActive = true;
            ExecutionPointId = 0;
            ExecutionOrder = 0;
            WhereClause = string.Empty;
            DateCreated = DateTime.Now;
            CreatedByUserId = 1;
            DateUpdated = null;
            UpdatedByUserId = null;
        }
        #region Properties
        [DataMember]
        public int RuleSetId { get; set; }
        [DataMember]
        public int SourceFileId { get; set; }
        [DataMember]
        public int FileTypeId { get; set; }
        [DataMember]
        public bool IsSystem { get; set; }
        [DataMember]
        public bool IsGlobal { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public int ExecutionPointId { get; set; }
        [DataMember]
        public int ExecutionOrder { get; set; }
        [DataMember]
        public string WhereClause { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserId { get; set; }
        [DataMember]
        public int? UpdatedByUserId { get; set; }
        #endregion
    }
}
