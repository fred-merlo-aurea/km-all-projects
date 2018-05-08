using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Activity.Report
{
    [Serializable]
    [DataContract]
    public class PerformanceByDayAndTimeReport
    {
        public PerformanceByDayAndTimeReport()
        {
        }
        [DataMember]
        public string DayGroup { get; set; }
        [DataMember]
        public string HourGroup { get; set; }
        [DataMember]
        public string Opens { get; set; }
        [DataMember]
        public string Clicks { get; set; }
    }
}
