using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class FilterSchedule
    {
        #region Properties
        [DataMember]
        public int FilterScheduleID { get; set; }
        [DataMember]
        public int FilterID { get; set; }
        [DataMember]
        public FrameworkUAD.BusinessLogic.Enums.ExportType ExportTypeID { get; set; }
        [DataMember]
        public bool IsRecurring { get; set; }
        [DataMember]
        public int? RecurrenceTypeID { get; set; }
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public string StartTime { get; set; }
        [DataMember]
        public DateTime? EndDate { get; set; }
        [DataMember]
        public bool RunSunday { get; set; }
        [DataMember]
        public bool RunMonday { get; set; }
        [DataMember]
        public bool RunTuesday { get; set; }
        [DataMember]
        public bool RunWednesday { get; set; }
        [DataMember]
        public bool RunThursday { get; set; }
        [DataMember]
        public bool RunFriday { get; set; }
        [DataMember]
        public bool RunSaturday { get; set; }
        [DataMember]
        public int? MonthScheduleDay { get; set; }
        [DataMember]
        public bool MonthLastDay { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public DateTime UpdatedDate { get; set; }
        [DataMember]
        public int UpdatedBy { get; set; }
        [DataMember]
        public string EmailNotification { get; set; }
        [DataMember]
        public string Server { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string Folder { get; set; }
        [DataMember]
        public string ExportFormat { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public string RecurrenceType { get; set; }
        [DataMember]
        public string FilterName { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public int? GroupID { get; set; }
        [DataMember]
        public int? FolderID { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public int? BrandID { get; set; }
        [DataMember]
        public string BrandName { get; set; }
        [DataMember]
        public string Operation { get; set; }
        [DataMember]
        public List<int> FilterGroupID_Selected { get; set; }
        [DataMember]
        public List<int> FilterGroupID_Suppressed { get; set; }
        [DataMember]
        public bool ShowHeader { get; set; }
        [DataMember]
        public bool AppendDateTimeToFileName { get; set; }
        [DataMember]
        public string ExportName { get; set; }
        [DataMember]
        public string ExportNotes { get; set; }
        [DataMember]
        public int? FilterSegmentationID { get; set; }
        [DataMember]
        public string FilterSegmentationName { get; set; }
        [DataMember]
        public string SelectedOperation { get; set; }
        [DataMember]
        public string SuppressedOperation { get; set; }
        #endregion
    }
}
