using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace EmailMarketing.API.Models.Reports.Blast
{
    public enum ReportRowType
    {
        [XmlEnum(Name="Unknown")]
        Unknown = 0,
        [XmlEnum(Name="Sends")]
        Sends,
        [XmlEnum(Name="Opens")]
        Opens,
        [XmlEnum(Name="Clicks")]
        Clicks,
        [XmlEnum(Name = "Bounces")]
        Bounces,
        [XmlEnum(Name = "Resends")]
        Resends,
        [XmlEnum(Name = "Forwards")]
        Forwards,
        [XmlEnum(Name = "Unsubscribes")]
        Unsubscribes,
        [XmlEnum(Name = "MasterSupressed")]
        MasterSupressed,
        [XmlEnum(Name = "AbuseComplaints")]
        AbuseComplaints,
        [XmlEnum(Name = "ISPFeedbackLoops")]
        ISPFeedbackLoops
    }
}