using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Publisher.Report
{
    [Serializable]
    [DataContract]
    public class ActivitySubscribesDetails
    {
        public ActivitySubscribesDetails() 
        { 
        }

        public DateTime ActionDate { get; set; }
        public string EmailAddress { get; set; }
        public string ActionValue { get; set; }
    }
}
