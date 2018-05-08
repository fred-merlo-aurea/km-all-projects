using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Publisher.Report
{
    [Serializable]
    [DataContract]
    public class ActivityTopClicks
    {
        public ActivityTopClicks() 
        { 
        }

        public int PageNumber { get; set; }
        public int LinkID { get; set; }
        public string link { get; set; }
        public int ClickCount { get; set; }
        public int DistinctClickcount { get; set; }
    }
}
