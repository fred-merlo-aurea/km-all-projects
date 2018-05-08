using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;
using System.Xml.Serialization;


namespace ECN_Framework_Entities.Activity.Report
{
    [Serializable]
    [DataContract]
    public class EmailPerformanceByDomain
    {
        public EmailPerformanceByDomain() 
        { 
        }
        
        public string Domain { get; set; }
        public int SendTotal { get; set; }
        public decimal SendTotalPercentage { get; set; }
        public int Opens { get; set; }
        public decimal OpensPercentage { get; set; }
        public int Clicks { get; set; }
        public decimal ClicksPercentage { get; set; }
        public int Bounce { get; set; }
        public decimal BouncePercentage { get; set; }
        public int Unsubscribe { get; set; }
        public decimal UnsubscribePercentage { get; set; }
        [XmlIgnoreAttribute]
        public int Forward { get; set; }
        public int Delivered { get; set; }
        [XmlIgnoreAttribute]
        public decimal DeliveredPercentage { get; set; }
    }

}
