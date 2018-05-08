using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace EmailMarketing.API.Models.Reports.Blast
{
    [Serializable]
    [DataContract]
    [XmlRoot("ReportRow")]
    public class ReportRow
    {
        [XmlElement]
        [DataMember]
        public ReportRowType RowType { get; set; }

        [XmlElement]
        [DataMember]
        public int DistinctCount { get; set; }

        [XmlElement]
        [DataMember]
        public int TotalCount { get; set; }

        [XmlElement]
        [DataMember]
        public float UniquePercent { get; set; }

        [XmlElement]
        [DataMember]
        public float TotalPercent { get; set; }
    }
}