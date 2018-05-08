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
    public class AudienceEngagementReport
    {
        public AudienceEngagementReport() 
        { 
        }
        
        public int SortOrder { get; set; }
        public string SubscriberType { get; set; }
        public int Counts { get; set; }
        [XmlIgnore()]
        public decimal Percents { get; set; }
      
        public decimal Percentage
        {
            get { return Math.Round(Percents, 2); }
            set { Percents = value; }
        }

        public string Description { get; set; }
    }
}
