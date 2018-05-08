using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FrameworkUAD_Lookup.Entity
{
    [Serializable]
    [DataContract]
    public class Code
    {
        public Code()
        {
            CodeId = 0;
            CodeTypeId = 0;
            CodeName = string.Empty;
            CodeValue = string.Empty;
            DisplayName = string.Empty;
            CodeDescription = string.Empty;
            DisplayOrder = 0;
            HasChildren = false;
            ParentCodeId = null;
            IsActive = false;
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
            Children = new List<Code>();
        }
        #region Properties
        [DataMember]
        public int CodeId { get; set; }
        [DataMember]
        public int CodeTypeId { get; set; }
        [DataMember]
        public string CodeName { get; set; }
        [DataMember]
        public string CodeValue { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
        [DataMember]
        public string CodeDescription { get; set; }
        [DataMember]
        public int DisplayOrder { get; set; }
        [DataMember]
        public bool HasChildren { get; set; }
        [DataMember]
        public int? ParentCodeId { get; set; }
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

        [DataMember]
        public List<Code> Children { get; set; }
    }
}
