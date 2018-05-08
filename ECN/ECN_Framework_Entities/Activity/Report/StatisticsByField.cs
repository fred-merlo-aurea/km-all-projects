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
    public class StatisticsByField
    {
        public StatisticsByField() 
        { 
        }
        
        [ExportAttribute(FieldOrder = 1)]
        public string Field { get; set; }
        [ExportAttribute(FieldOrder = 2)]
        public int USend { get; set; }
        [ExportAttribute(FieldOrder = 3)]
        public int UHBounce { get; set; }
        [ExportAttribute(FieldOrder = 4)]
        public int USBounce { get; set; }
        [ExportAttribute(FieldOrder = 6)]
        public int TOpen { get; set; }
        [ExportAttribute(FieldOrder = 8)]
        public int UOpen { get; set; }
        [ExportAttribute(FieldOrder = 10, Header="Total Clicks By URL")]
        public int TClick { get; set; }
        [ExportAttribute(FieldOrder = 12, Header = "Unique Clicks By URL")]
        public int UClick { get; set; }
        [ExportAttribute(FieldOrder = 5)]
        public int Delivered { get; set; }
        [ExportAttribute(FieldOrder = 7)]
        public Decimal TotalOpenPercentage { get; set; }
        [ExportAttribute(FieldOrder = 9)]
        public Decimal UniqueOpenPercentage { get; set; }
        [ExportAttribute(FieldOrder = 11, Header = "Total Clicks By URL%")]
        public Decimal TotalClickPercentage { get; set; }
        [ExportAttribute(FieldOrder = 13, Header = "Unique Clicks By URL%")]
        public Decimal UniqueClickPercentage { get; set; }
        [ExportAttribute(FieldOrder = 14, Header = "Click Through Ratio")]
        public int ClickThrough { get; set; }
        [ExportAttribute(FieldOrder = 15, Header = "Click Through Ratio%")]
        public Decimal ClickThroughPercentage { get; set; }
        
    }
}
