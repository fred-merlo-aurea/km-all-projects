using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Xml.Serialization;
namespace EmailMarketing.API.Models
{
    [Serializable]
    [DataContract]
    [XmlRoot("SimpleBlast")]
    public class SimpleBlastV2 : BlastBase
    {
        /// <summary>
        /// Create a SimpleBlast
        /// </summary>
        public SimpleBlastV2()
        {
            SendTime = DateTime.Now;
            BlastType = "Regular";
        }

        /// <summary>
        /// Provide a Campaign Name to create a new campaign for your blast
        /// Will be ignored on PUT requests.
        /// </summary>
        [DataMember]
        [XmlElement]
        public string CampaignName { get; set; }

        /// <summary>
        /// Pass a CampaignItemName to customize the name of your CampaignItem.
        /// Pass an empty string to use the standard CampaignItemName generation of EmailSubject and current date
        /// Will be ignored on PUT requests.
        /// </summary>
        [DataMember]
        [XmlElement]
        public string CampaignItemName { get; set; }

        [DataMember]
        [XmlElement]
        public SimpleBlastSchedule Schedule { get; set; }

        /// <summary>
        /// supply true to create a test blast, otherwise a standard blast will be created.
        /// </summary>  
        [DataMember]
        [XmlElement]
        public bool IsTestBlast { get; set; }

        /// <summary>
        /// Read-only, Smart Segment ID filled by ECN when a SimpleBlast is posted.
        /// </summary>
        [DataMember]
        [XmlElement(IsNullable=true)]
        public int? SmartSegmentID { get; set; }

        /// <summary>
        /// allow internal components the ability to set the smart segment ID
        /// </summary>
        /// <param name="smartSegmentID"></param>
        internal void SetSmartSegmentID(int smartSegmentID) { SmartSegmentID = smartSegmentID; }

        /// <summary>
        /// Optional, for SmartSegments an array of ID values for reference blasts.
        /// </summary>
        [DataMember]
        [XmlElement]
        public int[] ReferenceBlasts { get; set; }

        /// <summary>
        /// Blasts returned by Get or Search may fill this.  Blasts created by simple blast may 
        /// select a filter by setting FilterID.
        /// </summary>
        [DataMember]
        [XmlElement]
        public int[] Filters { get; set; }

        /// <summary>
        /// Blasts returned by Get or Search may fill this.  Blasts created by simple blast may 
        /// select a filter by setting FilterID.
        /// </summary>
        [DataMember]
        [XmlElement]
        public int[] SmartSegments { get; set; }

        /// <summary>
        /// SimpleBlasts are sent immediately.
        /// </summary>
        [DataMember]
        [XmlElement]
        public DateTime SendTime { get; set; }
        internal void SetSendTime(DateTime sendTime) { SendTime = sendTime; }

        /// <summary>
        /// Provide a CampaignID to create a new simple blast under a specific campaign.  
        /// Pass 0 to create/associate the simple blast under your "Marketing Campaign" campaign.
        /// Will be ignored on PUT requests.
        /// </summary>
        [DataMember]
        [XmlElement(IsNullable=true)]
        public int? CampaignID { get; set; }
    }
}