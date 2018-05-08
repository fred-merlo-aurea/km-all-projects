using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace EmailMarketing.API.Models.Reports.Blast
{
    [Serializable]
    [DataContract]
    [XmlRoot("DeliveryReport")]
    public class DeliveryReport
    {
        public DeliveryReport()
        {
            clickThrough = -1;
            blastID = -1;
            sendTime = string.Empty;
            emailSubject = string.Empty;
            groupName = string.Empty;
            filterName = string.Empty;
            campaignName = string.Empty;
            sendTotal = -1;
            delivered = -1;
            softBounceTotal = -1;
            hardBounceTotal = -1;
            otherBounceTotal = -1;
            bounceTotal = -1;
            uniqueOpens = -1;
            totalOpens = -1;
            uniqueClicks = -1;
            totalClicks = -1;
            unsubscribeTotal = -1;
            suppressedTotal = -1;
            mobileOpens = -1;
            fromEmail = string.Empty;
            campaignItemName = string.Empty;
            customerName = string.Empty;
            field1 = string.Empty;
            field2 = string.Empty;
            field3 = string.Empty;
            field4 = string.Empty;
            field5 = string.Empty;
            abuse = -1;
            feedback = -1;
            spamCount = -1;
        }

        [XmlElement]
        [DataMember]
        public int blastID;

        [XmlElement]
        [DataMember]
        public string sendTime;

        [XmlElement]
        [DataMember]
        public string emailSubject;

        [XmlElement]
        [DataMember]
        public string groupName;

        [XmlElement]
        [DataMember]
        public string filterName;

        [XmlElement]
        [DataMember]
        public string campaignName;

        [XmlElement]
        [DataMember]
        public int sendTotal;

        [XmlElement]
        [DataMember]
        public int delivered;

        [XmlElement]
        [DataMember]
        public int softBounceTotal;

        [XmlElement]
        [DataMember]
        public int hardBounceTotal;

        [XmlElement]
        [DataMember]
        public int otherBounceTotal;

        [XmlElement]
        [DataMember]
        public int bounceTotal;

        [XmlElement]
        [DataMember]
        public int uniqueOpens;

        [XmlElement]
        [DataMember]
        public int totalOpens;

        [XmlElement]
        [DataMember]
        public int uniqueClicks;

        [XmlElement]
        [DataMember]
        public int totalClicks;

        [XmlElement]
        [DataMember]
        public int unsubscribeTotal;

        [XmlElement]
        [DataMember]
        public int suppressedTotal;

        [XmlElement]
        [DataMember]
        public int mobileOpens;

        [XmlElement]
        [DataMember]
        public string fromEmail;

        [XmlElement]
        [DataMember]
        public string campaignItemName;

        [XmlElement]
        [DataMember]
        public string customerName;

        [XmlElement]
        [DataMember]
        public string field1;

        [XmlElement]
        [DataMember]
        public string field2;

        [XmlElement]
        [DataMember]
        public string field3;

        [XmlElement]
        [DataMember]
        public string field4;

        [XmlElement]
        [DataMember]
        public string field5;

        [XmlElement]
        [DataMember]
        public int abuse;

        [XmlElement]
        [DataMember]
        public int feedback;

        [XmlElement]
        [DataMember]
        public int spamCount;

        [XmlElement]
        [DataMember]
        public int clickThrough;

    }
}