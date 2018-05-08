using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Activity.Report
{
    [Serializable]
    [DataContract]
    public class EmailsDeliveredByPercentage
    {
        public EmailsDeliveredByPercentage() 
        { 
        }

        public string Range { get; set; }
        public int TotalCount { get; set; }
        public decimal Percentage { get; set; }
    }
}
