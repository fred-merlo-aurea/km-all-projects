using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Activity.Report
{
    [Serializable]
    [DataContract]
    public class MasterSuppressionSourceReportDetails
    {
        public string SuppressionCode { get; set; }
        public string EmailAddress { get; set; }
        public DateTime SuppressedDateTime { get; set; }
        public string Reason { get; set; }
    }
}   
