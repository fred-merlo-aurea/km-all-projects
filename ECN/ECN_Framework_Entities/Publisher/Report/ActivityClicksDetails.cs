using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Publisher.Report
{
    [Serializable]
    [DataContract]
    public class ActivityClicksDetails
    {
        public ActivityClicksDetails() 
        { 
        }
        public DateTime ActionDate { get; set; }
        public int PageNumber { get; set; }
        public string Link { get; set; }
        public string EmailAddress { get; set; }
    }
}
