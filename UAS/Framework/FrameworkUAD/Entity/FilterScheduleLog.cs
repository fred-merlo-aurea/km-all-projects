using System;
using System.Linq;
using System.Runtime.Serialization;


namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class FilterScheduleLog
    {
        public FilterScheduleLog() { }

        #region Properties
        [DataMember]
        public int FilterScheduleLogID { get; set; }
        [DataMember]
        public int FilterScheduleID { get; set; }
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public int StartTime { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public int DownloadCount { get; set; }
        #endregion 
    }
}
