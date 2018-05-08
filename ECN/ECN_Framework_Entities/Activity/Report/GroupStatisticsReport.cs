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
    public class GroupStatisticsReport
    {
        public GroupStatisticsReport() 
        { 
        }

        [XmlIgnore]
        public DateTime SendTime { get; set; }

        [ExportAttribute(FieldOrder = 1)]
        [XmlElement(Order = 1)]
        public string Date
        {
            get { return SendTime.ToString("MM/dd/yyyy"); }
            set { SendTime = DateTime.Parse(value); }
        }

        [ExportAttribute(FieldOrder = 2)]
        [XmlElement(Order = 2)]
        public int BlastID { get; set; }

        [ExportAttribute(FieldOrder = 3)]
        [XmlElement(Order = 3)]
        public string EmailSubject { get; set; }

        [ExportAttribute(FieldOrder = 4)]
        [XmlElement(Order = 4)]
        public string CampaignName { get; set; }

        [ExportAttribute(FieldOrder = 5)]
        [XmlElement(Order = 5)]
        public int TSend { get; set; }

        [ExportAttribute(FieldOrder = 6)]
        [XmlElement(Order = 6)]
        public int UBounce { get; set; }

        [ExportAttribute(FieldOrder = 7)]
        [XmlElement(Order = 7)]
        public int Delivered { get; set; }

        [ExportAttribute(FieldOrder = 9)]
        [XmlElement(Order = 9)]
        public int TOpen { get; set; }

        [ExportAttribute(FieldOrder = 11)]
        [XmlElement(Order = 11)]
        public int UOpen { get; set; }

        [ExportAttribute(FieldOrder = 13, Header = "Total Clicks By URL")]
        [XmlElement(Order = 13)]
        public int TClick { get; set; }

        [ExportAttribute(FieldOrder = 15, Header = "Unique Clicks By URL")]
        [XmlElement(Order = 15)]
        public int UClick { get; set; }

        [ExportAttribute(FieldOrder = 16, Header = "Click Through Ratio")]
        [XmlElement(Order = 16)]
        public int ClickThrough { get; set; }

        [ExportAttribute(FieldOrder = 17, Header = "Click Through Ratio%")]
        [XmlElement(Order = 17)]
        public Decimal ClickThroughPercentage { get; set; }

        [ExportAttribute(FieldOrder = 19)]
        [XmlElement(Order = 19)]
        public int UUnsubscribe { get; set; }

        [ExportAttribute(FieldOrder = 8)]
        [XmlElement(Order = 8)]
        public Decimal SuccessPercentage { get; set; }

        [ExportAttribute(FieldOrder = 10)]
        [XmlElement(Order = 10)]
        public Decimal DeliveredOpensPercentage { get; set; }

        [ExportAttribute(FieldOrder = 12)]
        [XmlElement(Order = 12)]
        public Decimal UniqueOpensPercentage { get; set; }

        [ExportAttribute(FieldOrder = 14, Header = "Delivered Clicks By URL")]
        [XmlElement(Order = 14)]
        public Decimal DeliveredClicksPercentage { get; set; }

        [ExportAttribute(FieldOrder = 18, Header = "Unique Clicks By URL%" )]
        [XmlElement(Order = 18)]
        public Decimal UniqueClicksPercentage { get; set; }

        [ExportAttribute(FieldOrder = 20)]
        [XmlElement(Order = 20)]
        public Decimal OpenClicksPercentage { get; set; }

        [ExportAttribute(FieldOrder = 21)]
        [XmlElement(Order = 21)]
        public Decimal UniqueOpenClicksPercentage { get; set; }

        [ExportAttribute(FieldOrder = 22)]
        [XmlElement(Order = 22)]
        public List<ECN_Framework_Entities.Activity.Report.PlatformDetail> BrowserStats { get; set; }

        [ExportAttribute(FieldOrder = 23)]
        [XmlElement(Order = 23)]
        public String Filter { get; set; }

        [ExportAttribute(FieldOrder = 25)]
        [XmlElement(Order = 25)]
        [XmlIgnore]
        public int Suppressed { get; set; }
    }
}
