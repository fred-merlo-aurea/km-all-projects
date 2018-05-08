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
    public class AdvertiserClickReport
    {
        public AdvertiserClickReport() 
        { 
        }

        
        public int BlastID { get; set; }
        public string EmailSubject { get; set; }

        [XmlIgnore]
        public DateTime SendTime { get; set; }

        public string Date
        {
            get { return SendTime.ToString("MM/dd/yyyy"); }
            set { SendTime = DateTime.Parse(value); }
        }

        [ExportAttribute(Header="LinkAlias")]
        [XmlElement(ElementName="LinkAlias")]
        public string Alias { get; set; }
        public string LinkURL { get; set; }
        public string LinkOwner { get; set; }
        public string LinkType { get; set; }
        public int UniqueCount { get; set; }
        public int TotalCount { get; set; }
    }
}
