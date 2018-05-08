using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class CodeSheet
    {
        public CodeSheet()
        {
            CodeSheetID = 0;
            PubID = 0;
            ResponseGroup = string.Empty;
            ResponseValue = string.Empty;
            ResponseDesc = string.Empty;
            ResponseGroupID = 0;
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
            DateUpdated = DateTime.Now;
            UpdatedByUserID = 1;
            DisplayOrder = 0;
            ReportGroupID = 0;
            IsActive = true;
            WQT_ResponseID = 0;
            IsOther = false;
        }
        #region
        [DataMember]
        public int CodeSheetID { get; set; }
        [DataMember]
        public int PubID { get; set; }
        [DataMember]
        public string ResponseGroup { get; set; }
        [DataMember(Name = "Responsevalue")]
        public string ResponseValue { get; set; }
        [DataMember(Name = "Responsedesc")]
        public string ResponseDesc { get; set; }
        [DataMember]
        public int ResponseGroupID { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        [DataMember]
        public int? DisplayOrder { get; set; }
        [DataMember]
        public int? ReportGroupID { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }
        [DataMember]
        public int? WQT_ResponseID { get; set; }
        [DataMember]
        public bool? IsOther { get; set; }
        #endregion
    }
}
