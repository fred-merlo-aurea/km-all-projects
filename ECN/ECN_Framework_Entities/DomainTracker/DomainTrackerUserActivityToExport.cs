using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;


namespace ECN_Framework_Entities.DomainTracker
{
    [Serializable]
    [DataContract]
    public class DomainTrackerUserActivityToExport
    {
        public DomainTrackerUserActivityToExport()
        {
            EmailAddress = string.Empty;
            PageURL = string.Empty;
            TimeStamp = null;
            IPAddress = null;
            OS = null;
            Browser = null;
        }
        [DataMember]
        public string EmailAddress { get; set; }
        [DataMember]
        public string PageURL { get; set; }
        [DataMember]
        public string TimeStamp { get; set; }
        [DataMember]
        public string IPAddress { get; set; }
        [DataMember]
        public string OS { get; set; }
        [DataMember]
        public string Browser { get; set; }
    }
}
