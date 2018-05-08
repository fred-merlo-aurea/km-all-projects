using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ECN_Framework_Entities.Communicator
{
    [Serializable]
    [DataContract]
    public class ReportSchedule
    {
        public ReportSchedule()
        {
            ReportScheduleID = -1;            
            CustomerID = null;
            ReportID = null;
            StartTime = string.Empty;
            StartDate = string.Empty;
            EndDate = string.Empty;
            ScheduleType = string.Empty;
            RunSunday = null;
            RunMonday = null;
            RunTuesday = null;
            RunWednesday = null;
            RunThursday = null;
            RunFriday = null;
            RunSaturday = null;
            MonthScheduleDay = null;
            MonthLastDay = null;
            FromEmail = string.Empty;
            FromName = string.Empty;
            EmailSubject = string.Empty;
            ToEmail = string.Empty;
            CreatedUserID = null;
            CreatedDate = null;
            UpdatedUserID = null;
            UpdatedDate = null;
            IsDeleted = null;
            RecurrenceType = string.Empty;
            Report = null;
            ReportParameters = string.Empty;
            BlastID = null;
            ExportFormat = string.Empty;
        }

       
        [DataMember]
        public int ReportScheduleID { get; set; }
        [DataMember]
        public int? CustomerID { get; set; }
        [DataMember]
        public int? ReportID { get; set; }
        [DataMember]
        public string StartTime { get; set; }
        [DataMember]
        public string StartDate { get; set; }
        [DataMember]
        public string EndDate { get; set; }
        [DataMember]
        public string ScheduleType { get; set; }
        [DataMember]
        public bool? RunSunday { get; set; }
        [DataMember]
        public bool? RunMonday { get; set; }
        [DataMember]
        public bool? RunTuesday { get; set; }
        [DataMember]
        public bool? RunWednesday { get; set; }
        [DataMember]
        public bool? RunThursday { get; set; }
        [DataMember]
        public bool? RunFriday { get; set; }
        [DataMember]
        public bool? RunSaturday { get; set; }
        [DataMember]
        public int? MonthScheduleDay { get; set; }
        [DataMember]
        public bool? MonthLastDay { get; set; }
        [DataMember]
        public string FromEmail { get; set; }
        [DataMember]
        public string FromName { get; set; }
        [DataMember]
        public string EmailSubject { get; set; }
        [DataMember]
        public string RecurrenceType { get; set; }
        [DataMember]
        public string ToEmail { get; set; }
        [DataMember]
        public int? CreatedUserID { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get;  set; }
        [DataMember]
        public int? UpdatedUserID { get; set; }
        [DataMember]
        public DateTime? UpdatedDate { get;  set; }
        [DataMember]
        public bool? IsDeleted { get; set; }
        [DataMember]
        public string ReportParameters { get; set; }
        public ECN_Framework_Entities.Communicator.Reports Report { get; set; }
        [DataMember]
        public int? BlastID { get; set; }
        [DataMember]
        public string ExportFormat { get; set; }
    }
}
