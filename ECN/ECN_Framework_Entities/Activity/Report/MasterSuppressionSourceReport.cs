using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Activity.Report
{
    [Serializable]
    [DataContract]
    public class MasterSuppressionSourceReport
    {
        public int GroupID { get; set; }

        public int UnsubscribeCodeID { get; set; }
        public string UnsubscribeCode { get; set; }
        public int Count { get; set; }
    }
}   
