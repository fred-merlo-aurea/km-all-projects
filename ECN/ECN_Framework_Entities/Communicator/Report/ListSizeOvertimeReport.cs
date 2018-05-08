using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Communicator.Report
{
    [Serializable]
    [DataContract]
    public class ListSizeOvertimeReport
    {
        public ListSizeOvertimeReport() 
        { 
        }

        public DateTime RangeStart { get; set; }
        public DateTime RangeEnd { get; set; }
        public int Added { get; set; }
        public int Bounced { get; set; }
        public int UnSubscribed { get; set; }
        public int Active { get; set; }
    }
}
