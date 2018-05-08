using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class FilterDetail
    {
        public FilterDetail()
        {
            FilterDetailId = 0;
            FilterId = 0;
            FilterTypeId = 0;
            FilterField = string.Empty;
            SearchCondition = string.Empty;
            FilterObjectType = string.Empty;
            AdHocFromField = string.Empty;
            AdHocToField = string.Empty;
            AdHocFieldValue = string.Empty;
            FilterGroupID = 0;
            IDTag = "0";
            DateCreated = DateTime.Now;
            DateUpdated = null;
            CreatedByUserID = 0;
            UpdatedByUserID = null;
        }
        #region Properties
        [DataMember]
        public int FilterDetailId { get; set; }
        [DataMember]
        public int FilterId { get; set; }
        [DataMember]
        public int FilterTypeId { get; set; }
        [DataMember]
        public string FilterField { get; set; }
        [DataMember]
        public string AdHocFromField { get; set; }
        [DataMember]
        public string AdHocToField { get; set; }
        [DataMember]
        public string AdHocFieldValue { get; set; }
        [DataMember]
        public string FilterObjectType { get; set; }
        [DataMember]
        public string SearchCondition { get; set; }
        [DataMember]
        public int FilterGroupID { get; set; }
        [DataMember]
        public DateTime DateCreated { get; set; }
        [DataMember]
        public DateTime? DateUpdated { get; set; }
        [DataMember]
        public int CreatedByUserID { get; set; }
        [DataMember]
        public int? UpdatedByUserID { get; set; }
        [DataMember]
        public string GroupType { get; set; }
        [DataMember]
        public string IDTag { get; set; }
        #endregion
    }
}
