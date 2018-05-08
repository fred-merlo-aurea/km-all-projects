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
    public class BlastResponseDetail
    {
        public BlastResponseDetail() 
        { 
        }
        
        public string Action { get; set; }
        public string ActionDate { get; set; }
        public int Total { get; set; }
    }
}
