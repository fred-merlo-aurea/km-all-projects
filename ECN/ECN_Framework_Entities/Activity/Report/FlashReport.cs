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
    public class FlashReport
    {
        public FlashReport() 
        { 
        }

        [DataMember]
        public string PromoCode { get; set; }
        [DataMember]
        public int TotalEmails { get; set; }
        [DataMember]
        public int UniqueEmails { get; set; }
    }
}
