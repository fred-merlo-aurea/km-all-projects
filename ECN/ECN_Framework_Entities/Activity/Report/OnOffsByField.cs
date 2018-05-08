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
    public class OnOffsByField
    {
        public OnOffsByField() 
        { 
        }
        
        public string Field { get; set; }
        public DateTime Months { get; set; }
        public string SubscribeTypeCode { get; set; }
        public int Counts { get; set; }
    }
}
