using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class Report
    {
        public Report() { }
        #region Properties
        [DataMember]
        public int ReportID { get; set; }
        [DataMember]        
        public string ReportName { get; set; }
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
        [DataMember]
        public bool ProvideID { get; set; }
        [DataMember]
        public int? ProductID { get; set; }
	    [DataMember]
        public string URL { get; set; }
	    [DataMember]
        public bool? IsCrossTabReport { get; set; }
	    [DataMember]
        public string Row { get; set; }
	    [DataMember]
        public string Column { get; set; }
	    [DataMember]
        public bool? SuppressTotal { get; set; }
	    [DataMember]
        public bool? Status { get; set; }
        [DataMember]
        public int ReportTypeID { get; set; }
        [DataMember]
        public string ReportView { get; set; }
        #endregion
    }
}
