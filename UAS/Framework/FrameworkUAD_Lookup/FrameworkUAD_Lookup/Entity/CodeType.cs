using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FrameworkUAD_Lookup.Entity
{
    [Serializable]
    [DataContract]
    public class CodeType
    {
        public CodeType()
        {
            CodeTypeId = 0;
            CodeTypeName = string.Empty;
            CodeTypeDescription = string.Empty;
            IsActive = false;
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
            Codes = new List<Code>();
        }
        #region Properties
        [DataMember]
        public int CodeTypeId { get; set; }
        [DataMember]
        public string CodeTypeName { get; set; }
        [DataMember]
        public string CodeTypeDescription { get; set; }
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
        List<Code> Codes { get; set; }
    }
}
