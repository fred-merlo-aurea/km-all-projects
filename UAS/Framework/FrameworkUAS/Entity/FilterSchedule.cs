using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAS.Entity
{
    [Serializable]
    [DataContract]
    public class FilterSchedule
    {
        public FilterSchedule()
        {
            FilterScheduleId = 0;
            FilterId = 0;
            ExportTypeId = 0;
            IsRecurring = false;
            RecurrenceTypeId = 0;
            StartDate = null;
            StartTime = null;
            EndDate = null;
            RunSunday = false;
            RunMonday = false;
            RunTuesday = false;
            RunWednesday = false;
            RunThursday = false;
            RunFriday = false;
            RunSaturday = false;
            MonthScheduleDay = 0;
            MonthLastDay = false;
            EmailNotification = string.Empty;
            Server = string.Empty;
            UserName = string.Empty;
            Password = string.Empty;
            Folder = string.Empty;
            ExportFormatTypeId = 0;
            FileName = string.Empty;
            IsDeleted = false;
            ClientID = 0;
            FilterGroupID_Selected = 0;
            FilterGroupID_Suppressed = 0;
            FolderId = 0;
            GroupId = 0;
            OperatorsTypeId = 0;
            DateCreated = DateTime.Now;
            DateUpdated = null;
            CreatedByUserID = 0;
            UpdatedByUserID = null;
        }
        #region Properties
        [DataMember]
        public int FilterScheduleId { get; set; }
        [DataMember]
        public int FilterId { get; set; }
        [DataMember]
        public int ExportTypeId { get; set; }
        [DataMember]
        public bool IsRecurring { get; set; }
        [DataMember]
        public int RecurrenceTypeId { get; set; }
        [DataMember]
        public DateTime? StartDate { get; set; }
        [DataMember]
        public TimeSpan? StartTime { get; set; }
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
        public int MonthScheduleDay { get; set; }
        [DataMember]
        public bool MonthLastDay { get; set; }
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
        public int ExportFormatTypeId { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public int ClientID { get; set; }
        [DataMember]
        public int FilterGroupID_Selected { get; set; }
        [DataMember]
        public int FilterGroupID_Suppressed { get; set; }
        [DataMember]
        public int FolderId { get; set; }
        [DataMember]
        public int GroupId { get; set; }
        [DataMember]
        public int OperatorsTypeId { get; set; }
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
