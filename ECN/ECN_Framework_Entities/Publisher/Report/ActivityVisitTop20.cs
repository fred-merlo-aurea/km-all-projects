using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ECN_Framework_Entities.Publisher.Report
{
    [Serializable]
    [DataContract]
    public class ActivityVisitTop20
    {
        public ActivityVisitTop20() 
        { 
        }

        public string EmailAddress { get; set; }
        public int Count { get; set; }
    }
}
