using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_Entities.Activity.Report
{
    [Serializable]
    [DataContract]
    public class CampaignStatisticsReport
    {
        public CampaignStatisticsReport() 
        { 
        }
        
        [ExportAttribute(FieldOrder = 1, Format = 0, Header="Send Time")]
        public DateTime SendTime { get; set; }
        [ExportAttribute(FieldOrder = 2, Header = "Campaign Item Name")]
        public string CampaignItemName { get; set; }

        [ExportAttribute(FieldOrder = 3, Header = "Message Name")]
        public string MessageName { get; set; }

        [ExportAttribute(FieldOrder = 4, Format = 0, Header="BlastID")]
        public int BlastID { get; set; }

        [ExportAttribute(FieldOrder = 5, Format = 0, Header="Subject")]
        public string EmailSubject { get; set; }

        [ExportAttribute(FieldOrder = 6, Format = 0, Header="Group")]
        public string GroupName { get; set; }

        [ExportAttribute(FieldOrder = 7, Format = 0, Header="Filter")]
        public string FilterName { get; set; }

        [ExportAttribute(FieldOrder = 8, Format = 0, Total = 1, Header="Sent")]
        public int SendTotal { get; set; }

        [ExportAttribute(FieldOrder = 9, Format = 0, Total = 1, Header="Bounce")]
        public int BounceTotal { get; set; }

        [ExportAttribute(FieldOrder = 10, Format = 0, Total = 1,Header="Delivery")]
        public int Delivered { get; set; }

        [ExportAttribute(FieldOrder = 11, Format = FormatType.Percent, Total = 1, Header="Success%")]
        public Decimal SuccessPercentage { get; set; }

        [ExportAttribute(FieldOrder = 12, Format = 0, Total = 1, Header="Total Opens")]
        public int TotalOpens { get; set; }

        [ExportAttribute(FieldOrder = 13, Format = FormatType.Percent, Total = 1, Header="Opens%")]
        public Decimal OpenDeliPercentage { get; set; }

        [ExportAttribute(FieldOrder = 14, Format = 0, Total = 1, Header="Unique Opens")]
        public int UniqueOpens { get; set; }

        [ExportAttribute(FieldOrder = 15, Format = FormatType.Percent, Total = 1, Header="Unique Opens%")]
        public Decimal UOpenDeliPercentage { get; set; }

        [ExportAttribute(FieldOrder = 16, Format = 0, Total = 1, Header="Unopened")]
        public int Unopened { get; set; }

        [ExportAttribute(FieldOrder = 17, Format = FormatType.Percent, Total = 1, Header="Unique Unopened%")]
        public Decimal UUnopenedDeliPercentage { get; set; }

        [ExportAttribute(FieldOrder = 18, Format = 0, Total = 1, Header="Total Clicks By URL")]
        public int TotalClicks { get; set; }

        [ExportAttribute(FieldOrder = 19, Format = FormatType.Percent, Total = 1, Header="Clicks By URL%")]
        public Decimal ClickDeliPercentage { get; set; }

        [ExportAttribute(FieldOrder = 20, Format = 0, Total = 1, Header="Unique Clicks By URL")]
        public int UniqueClicks { get; set; }

        [ExportAttribute(FieldOrder = 21, Format = FormatType.Percent, Total = 1, Header="Unique Clicks By URL%")]
        public Decimal UClickDeliPercentage { get; set; }

        [ExportAttribute(FieldOrder = 22, Format = 0, Total = 1, Header="No Clicks")]
        public int NoClicks { get; set; }

        [ExportAttribute(FieldOrder = 23, Format = FormatType.Percent, Total = 1, Header="Unique No Clicks%")]
        public Decimal UNoClickOpenPercentage { get; set; }

        [ExportAttribute(FieldOrder = 24, Format = 0, Total = 1, Header = "Click Through Ratio")]
        public int ClickThrough { get; set; }

        [ExportAttribute(FieldOrder = 25, Format = FormatType.Percent, Total = 1, Header = "Click Through Ratio%")]
        public Decimal ClickThroughPercentage { get; set; }


        [ExportAttribute(FieldOrder = 26, Format = 0, Total = 1, Header="Unsub")]
        public int UnsubscribeTotal { get; set; }

        [ExportAttribute(FieldOrder = 27, Format = 0, Total = 1, Header = "Master Suppressed")]
        public int MasterSuppressed { get; set; }

        [ExportAttribute(FieldOrder = 28, Format = 0, Header="Total Abuse Complaints")]
        public int TotalAbuseComplaints { get; set; }

        [ExportAttribute(FieldOrder = 29, Format = 0, Header="Total ISP Feedback Loops")]
        public int TotalISPFeedbackLoops { get; set; }

        [ExportAttribute(FieldOrder = 30, Format = FormatType.Percent, Total = 1, Header="Total Clicks/Opens")]
        public Decimal TClicksOpenPercentage { get; set; }

        [ExportAttribute(FieldOrder = 31, Format = FormatType.Percent, Total = 1, Header="Unique Clicks/Opens")]
        public Decimal UClicksOpenPercentage { get; set; }

        [ExportAttribute(FieldOrder = 32, Format = 0, Header="Suppressed")]
        public int SuppressedTotal { get; set; }


        

    }
}
