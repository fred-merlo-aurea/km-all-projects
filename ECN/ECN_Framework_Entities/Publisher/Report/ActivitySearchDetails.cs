using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Publisher.Report
{
    [Serializable]
    [DataContract]
    public class ActivitySearchDetails
    {
        public ActivitySearchDetails() 
        { 
        }

        public string EmailAddress { get; set; }
        public DateTime ActionDate { get; set; }
        public string SearchText { get; set; }
    }
}
