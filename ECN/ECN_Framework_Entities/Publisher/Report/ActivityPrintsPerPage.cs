using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Publisher.Report
{
    [Serializable]
    [DataContract]
    public class ActivityPrintsPerPage
    {
        public ActivityPrintsPerPage() 
        { 
        }

        public int PageNumber { get; set; }
        public int Unique { get; set; }
        public int Total { get; set; }
    }
}
