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
    public class BlastDelivery
    {
        public BlastDelivery() 
        { 
        }


       [XmlIgnore]
        public DateTime SendTime { get; set; }

       [ExportAttribute(FieldOrder = 1, Format = 0)]
       [XmlElement(Order = 1)]
       public string CustomerName { get; set; }

       [ExportAttribute(FieldOrder = 2, Header = "Campaign Name")]
       [XmlElement(Order = 2)]
       public string CampaignName { get; set; }

       [ExportAttribute(FieldOrder = 3, Format = 0)]
       [XmlElement(Order = 3)]
       public string CampaignItemName { get; set; }

       [ExportAttribute(FieldOrder = 4, Format = 0, Header = "Date")]
        [XmlElement(Order = 4)]
        public string Date
        {
            get { return SendTime.ToString("MM/dd/yyyy"); }
            set { SendTime = DateTime.Parse(value); }
        }

        [ExportAttribute(FieldOrder = 5, Format = 0)]
        [XmlElement(Order = 5)]
        public string GroupName { get; set; }

        [ExportAttribute(FieldOrder = 6, Format = 0, Header="From Email")]
        [XmlElement(Order = 6)]
        public string FromEmail { get; set; }

        [ExportAttribute(FieldOrder = 7, Format = 0)]
        [XmlElement(Order = 7)]
        public int BlastID { get; set; }

        [ExportAttribute(FieldOrder = 8, Format = 0)]
        [XmlElement(Order = 8)]
        public string BlastCategory { get; set; }

        [ExportAttribute(FieldOrder = 9, Format = 0)]
        [XmlElement(Order = 9)]
        public string FilterName { get; set; }

        [ExportAttribute(FieldOrder = 10, Format = 0)]
        [XmlElement(Order = 10)]
        public string EmailSubject { get; set; }

        [ExportAttribute(FieldOrder = 11, Format = 0, Total = 1, Header="Sends")]
        [XmlElement(Order = 11)]
        public int SendTotal { get; set; }

        [ExportAttribute(FieldOrder = 12, Format = 0, Total = 1, Header="Delivery")]
        [XmlElement(Order = 12)]
        public int Delivered { get; set; }

        [ExportAttribute(FieldOrder = 13, Format = 0, Total = 1, Header="Soft")]
        [XmlElement(Order = 13)]
        public int SoftBounceTotal { get; set; }

        [ExportAttribute(FieldOrder = 14, Format = FormatType.Percent, Total = 1, Header="Soft/Sent")]
        [XmlElement(Order = 14)]
        public Decimal SoftSendPercentage { get; set; }

        [ExportAttribute(FieldOrder = 15, Format = 0, Total = 1, Header="Hard")]
        [XmlElement(Order = 15)]
        public int HardBounceTotal { get; set; }

        [ExportAttribute(FieldOrder = 16, Format = FormatType.Percent, Total = 1, Header="Hard/Sent")]
        [XmlElement(Order = 16)]
        public Decimal HardSendPercentage { get; set; }

        [ExportAttribute(FieldOrder = 17, Format = 0, Total = 1, Header="Bounce")]
        [XmlElement(Order = 17)]
        public int BounceTotal { get; set; }

        [ExportAttribute(FieldOrder = 18, Format = FormatType.Percent, Total = 1, Header="Boun %")]
        [XmlElement(Order = 18)]
        public Decimal BounceSendPercentage { get; set; }

        [ExportAttribute(FieldOrder = 19, Format = 0, Total = 1, Header="UOpens")]
        [XmlElement(Order = 19)]
        public int UniqueOpens { get; set; }

        [ExportAttribute(FieldOrder = 20, Format = FormatType.Percent, Total = 1, Header = "UOpens%")]
        [XmlElement(Order = 20)]
        public Decimal UOpenDeliPercentage { get; set; }

        [ExportAttribute(FieldOrder = 21, Format = 0, Total = 1, Header = "T Mobile Opens")]
        [XmlElement(Order = 21)]
        public int MobileOpens { get; set; }

        [ExportAttribute(FieldOrder = 22, Format = FormatType.Percent, Total = 1, Header = "U Mobile Opens %")]
        [XmlElement(Order = 22)]
        public Decimal UnMobileOpenPercentage { get; set; }

        [ExportAttribute(FieldOrder = 23, Format = 0, Total = 1, Header = "TOpens")]
        [XmlElement(Order = 23)]
        public int TotalOpens { get; set; }

        [ExportAttribute(FieldOrder = 24, Format = FormatType.Percent, Total = 1, Header = "TOpens/Deli")]
        [XmlElement(Order = 24)]
        public Decimal OpenDeliPercentage { get; set; }

        [ExportAttribute(FieldOrder = 25, Format = 0, Total = 1, Header="UClicks By URL")]
        [XmlElement(Order = 25)]
        public int UniqueClicks { get; set; }

        [ExportAttribute(FieldOrder = 26, Format = FormatType.Percent, Total = 1, Header = "UClicks By URL%")]
        [XmlElement(Order = 26)]
        public Decimal UClickDeliPercentage { get; set; }

        [ExportAttribute(FieldOrder = 27, Format = 0, Total = 1, Header = "TClicks By URL")]
        [XmlElement(Order = 27)]
        public int TotalClicks { get; set; }

        [ExportAttribute(FieldOrder = 28, Format = FormatType.Percent, Total = 1, Header = "TClicks By URL/Deli")]
        [XmlElement(Order = 28)]
        public Decimal ClickDeliPercentage { get; set; }

        [ExportAttribute(FieldOrder = 29, Format = FormatType.Percent, Total = 1, Header = "UClick By URL/UOpen")]
        [XmlElement(Order = 29)]
        public Decimal UClickOpenPercentage { get; set; }

        [ExportAttribute(FieldOrder = 30, Format = FormatType.Percent, Total = 1, Header = "TClick By URL/TOpen")]
        [XmlElement(Order = 30)]
        public Decimal ClickOpenPercentage { get; set; }

        [ExportAttribute(FieldOrder = 31, Format = 0, Total = 1, Header = "Click Through Ratio")]        
        [XmlElement(Order = 31)]
        public int ClickThrough { get; set; }

        [ExportAttribute(FieldOrder = 32, Format = FormatType.Percent, Total = 1, Header = "Click Through Ratio%")]
        [XmlElement(Order = 32)]
        public Decimal ClickThroughPercentage { get; set; }


        [ExportAttribute(FieldOrder = 33, Format = 0, Total = 1, Header = "UnSub")]
        [XmlElement(Order = 33)]
        public int UnsubscribeTotal { get; set; }

        [ExportAttribute(FieldOrder = 34, Format = FormatType.Percent, Total = 1, Header = "UnSub %")]
        [XmlElement(Order = 34)]
        public Decimal UnSubDeliPercentage { get; set; }

        [ExportAttribute(FieldOrder = 35, Format = 0, Header = "Supp.")]
        [XmlElement(Order = 35)]
        public int SuppressedTotal { get; set; }

        /* Removed, unused: 2015-04-30
        [XmlElement(Order = 31)]
        [XmlIgnore]
        public int OtherBounceTotal { get; set; } */

        [ExportAttribute(FieldOrder = 36, Format = 0)]
        [XmlElement(Order = 36)]
        public string Field1 { get; set; }

        [ExportAttribute(FieldOrder = 37, Format = 0)]
        [XmlElement(Order = 37)]
        public string Field2 { get; set; }

        [ExportAttribute(FieldOrder = 38, Format = 0)]
        [XmlElement(Order = 38)]
        public string Field3 { get; set; }

        [ExportAttribute(FieldOrder = 39, Format = 0)]
        [XmlElement(Order = 39)]
        public string Field4 { get; set; }

        [ExportAttribute(FieldOrder = 40, Format = 0)]
        [XmlElement(Order = 40)]
        public string Field5 { get; set; }

        [ExportAttribute(FieldOrder = 41, Format = 0, Total = 1, Header = "Spam")]
        [XmlElement(Order = 41)]
        public int Spam{ get; set; }
        
        [ExportAttribute(FieldOrder = 42, Format = 0, Total = 1, Header = "Spam %")]
        [XmlElement(Order = 42)]
        public string SpamPercent { get; set; }


        [ExportAttribute(FieldOrder = 43, Format = 0, Total = 1, Ignore = true)]
        [XmlElement(Order = 43)]
        public int Abuse { get; set; }

        [ExportAttribute(FieldOrder = 44, Format = 0, Total = 1, Ignore = true)]
        [XmlElement(Order = 44)]
        public int Feedback { get; set; }

        // Appears unused but ignoring rather than removing: 2015-04-30
        [ExportAttribute(FieldOrder = 45, Format = 0, Total = 1, Ignore = true)]
        [XmlElement(Order = 45)]
        public int SpamCount { get; set; }



    }
}
