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
    public class BlastClickReport
    {
        public BlastClickReport() 
        { 
        }
        
        public int ClickCount { get; set; }
        public int DistinctClickCount { get; set; }
        public string Link { get; set; }
    }
}
