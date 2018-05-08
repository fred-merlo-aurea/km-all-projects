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
    public class DataDumpReport
    {
        public DataDumpReport() { }
        [ExportAttribute(FieldOrder = 1)]
        public DateTime SendTime { get; set; }
        
        [ExportAttribute(FieldOrder = 2)]
        public string DayOfWeek { get; set; }

        [ExportAttribute(FieldOrder = 3)]
        public int Year { get; set; }

        [ExportAttribute(FieldOrder = 4)]
        public int BlastID { get; set; }
        [ExportAttribute(FieldOrder = 5, Header = "Campaign Name")]
        public string CampaignName { get; set; }

        [ExportAttribute(FieldOrder = 6, Header = "Campaign Item Name")]
        public string CampaignItemName { get; set; }

        [ExportAttribute(FieldOrder = 7, Header = "Subject")]
        public string EmailSubject { get; set; }

        [ExportAttribute(FieldOrder = 8, Header = "Message Name")]
        public string MessageName { get; set; }

        [ExportAttribute(FieldOrder = 9, Header = "Group Name")]
        public string GroupName { get; set; }

        [ExportAttribute(FieldOrder = 10, Header = "Suppressed Groups")]
        public string SuppressionGroups { get; set; }

        [ExportAttribute(FieldOrder = 11)]
        public string BlastField1 { get; set; }

        [ExportAttribute(FieldOrder = 12)]
        public string BlastField2 { get; set; }

        [ExportAttribute(FieldOrder = 13)]
        public string BlastField3 { get; set; }

        [ExportAttribute(FieldOrder = 14)]
        public string BlastField4 { get; set; }

        [ExportAttribute(FieldOrder = 15)]
        public string BlastField5 { get; set; }

        [ExportAttribute(FieldOrder = 16, Header = "Sent")]
        public int usend { get; set; }
        [ExportAttribute(FieldOrder = 17, Header = "Bounce")]
        public int tbounce { get; set; }

        [ExportAttribute(FieldOrder = 18, Header = "Total Bounce %", Format = FormatType.Percent)]
        public decimal tbouncePerc { get; set; }

        [ExportAttribute(FieldOrder = 19, Header = "Delivery")]
        public int Delivery { get; set; }

        [ExportAttribute(FieldOrder = 20, Header = "Success %", Format = FormatType.Percent)]
        public decimal SuccessPerc { get; set; }

        [ExportAttribute(FieldOrder = 21, Header = "Sends Total %", Format = FormatType.Percent)]
        public decimal sendPerc { get; set; }

        [ExportAttribute(FieldOrder = 22, Header = "Total Opens")]
        public int topen { get; set; }

        [ExportAttribute(FieldOrder = 23, Header = "Opens Of Delivered Total", Format = FormatType.Percent)]
        public decimal OpensPercentOfDelivered { get; set; }

        [ExportAttribute(FieldOrder = 24, Header = "Unique Opens")]
        public int uopen { get; set; }

        [ExportAttribute(FieldOrder = 25, Header = "Unique Opens %", Format = FormatType.Percent)]
        public decimal uOpensPerc { get; set; }

        [ExportAttribute(FieldOrder = 26, Header = "Total Clicks by URL")]
        public int tClick { get; set; }

        [ExportAttribute(FieldOrder = 27, Header = "Total Clicks by URL %", Format = FormatType.Percent)]
        public decimal tClickPerc { get; set; }

        [ExportAttribute(FieldOrder = 28, Header = "Unique Clicks by URL")]
        public int uClick { get; set; }

        [ExportAttribute(FieldOrder = 29, Header = "Unique Clicks by URL %", Format = FormatType.Percent)]
        public decimal uClickPerc { get; set; }

        [ExportAttribute(FieldOrder = 30, Header = "Click Through Ratio")]
        public int ClickThrough { get; set; }

        [ExportAttribute(FieldOrder = 31, Header = "Click Through Ratio %", Format = FormatType.Percent)]
        public decimal ClickThroughPerc { get; set; }



        [ExportAttribute(FieldOrder = 32, Header = "Unsubscribes")]
        public int uSubscribe { get; set; }

        [ExportAttribute(FieldOrder = 33, Header = "Unsubscribe %", Format = FormatType.Percent)]
        public decimal uSubscribePerc { get; set; }

        [ExportAttribute(FieldOrder = 34, Header = "Total Clicks/Opens", Format = FormatType.Percent)]
        public decimal tClicksOpensPerc { get; set; }

        [ExportAttribute(FieldOrder = 35, Header = "Unique Clicks/Opens", Format = FormatType.Percent)]
        public decimal uClicksOpensPerc { get; set; }

        [ExportAttribute(FieldOrder = 36, Header = "Forwards")]
        public int trefer { get; set; }

        [ExportAttribute(FieldOrder = 37, Header = "Forwards %", Format = FormatType.Percent)]
        public decimal treferPerc { get; set; }

        [ExportAttribute(FieldOrder = 38, Header = "Total Resends")]
        public int tresend { get; set; }
        [ExportAttribute(FieldOrder = 39, Header = "Total Resends %", Format = FormatType.Percent)]
        public decimal tresendPerc { get; set; }

        [ExportAttribute(FieldOrder = 40, Header = "Suppressed")]
        public int Suppressed { get; set; }

        [ExportAttribute(FieldOrder = 41, Header = "Suppressed %", Format = FormatType.Percent)]
        public decimal SuppressedPerc {get; set; }

        [ExportAttribute(FieldOrder = 42, Header = "Hard Bounces")]
        public int uHardBounce { get; set; }

        [ExportAttribute(FieldOrder = 43, Header = "Hard Bounce %", Format = FormatType.Percent)]
        public decimal uHardBouncePerc { get; set; }


        [ExportAttribute(FieldOrder = 44, Header = "Soft Bounces")]
        public int uSoftBounce { get; set; }

        [ExportAttribute(FieldOrder = 45, Header = "Soft Bounce %", Format = FormatType.Percent)]
        public decimal uSoftBouncePerc { get; set; }

        
        [ExportAttribute(FieldOrder = 46, Header = "Other Bounces")]
        public int uOtherBounce { get; set; }

        [ExportAttribute(FieldOrder = 47, Header = "Other Bounce %", Format = FormatType.Percent)]
        public decimal uOtherBouncePerc { get; set; }

        [ExportAttribute(FieldOrder = 48 , Header = "Master Suppressed")]
        public int uMastSup_Unsub { get; set; }

        [ExportAttribute(FieldOrder = 49, Header = "Master Suppressed %", Format = FormatType.Percent)]
        public decimal uMastSup_UnsubPerc { get; set; }

        [ExportAttribute(FieldOrder = 50, Header = "Abuse Complaints")]
        public int uAbuseRpt_Unsub { get; set; }

        [ExportAttribute(FieldOrder = 51, Header = "Abuse Complaint %", Format = FormatType.Percent)]
        public decimal uAbuseRpt_UnsubPerc { get; set; }

        [ExportAttribute(FieldOrder = 52, Header = "ISP Feedback")]
        public int uFeedBack_Unsub { get; set; }

        [ExportAttribute(FieldOrder = 53, Header = "ISP Feedback %", Format = FormatType.Percent)]
        public decimal uFeedBack_UnsubPerc { get; set; }

        [ExportAttribute(FieldOrder = 54, Header = "Omniture Tracking Code")]
        public string OmnitureValues { get; set; }

        [ExportAttribute(FieldOrder = 55, Header = "Omniture Tracking Domains")]
        public string OmnitureDomains { get; set; }

        [ExportAttribute(FieldOrder = 56, Header = "Template")]
        public string TemplateName { get; set; }

        [ExportAttribute(FieldOrder = 57, Header = "AB Split")]
        public int ABAmount { get; set; }

        [ExportAttribute(FieldOrder = 58, Header = "AB Is Amount")]
        public string ABIsAmount { get; set; }


        

    }
}
