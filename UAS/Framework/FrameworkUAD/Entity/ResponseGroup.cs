using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class ResponseGroup
    {
        public ResponseGroup()
        {
            ResponseGroupID = 0;
            PubID = 0;
            ResponseGroupName = string.Empty;
            DisplayName = string.Empty;
            DateCreated = DateTime.Now;
            CreatedByUserID = 1;
            DisplayOrder = null;
            IsMultipleValue = false;
            IsRequired = false;
            IsActive = true;
            WQT_ResponseGroupID = null;
            ResponseGroupTypeId = 0;
        }
        #region Properties
        [DataMember]
        public int ResponseGroupID { get; set; }
        [DataMember]
        public int PubID { get; set; }
        [DataMember]
        public string ResponseGroupName { get; set; }
        [DataMember]
        public string DisplayName { get; set; }
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
        public bool? IsMultipleValue { get; set; }
        [DataMember]
        public bool? IsRequired { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }
        [DataMember]
        public int? WQT_ResponseGroupID { get; set; }
        [DataMember]
        public int ResponseGroupTypeId { get; set; }
        [DataMember]
        public string PubCode { get; set; }
        #endregion
    }
}
