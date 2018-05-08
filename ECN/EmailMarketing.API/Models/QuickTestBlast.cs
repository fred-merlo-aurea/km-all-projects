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
    public class QuickTestBlast
    {
        /// <summary>
        /// Create a Quick Test Blast
        /// </summary>
        public QuickTestBlast()
        {
            GroupID = -1;
        }

        /// <summary>
        /// Email Addresses to send test blast to
        /// </summary>
        [DataMember]
        [XmlElement]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Required, the ID of an existing layout
        /// </summary>
        [DataMember]
        [XmlElement]
        public int LayoutID { get; set; }

        /// <summary>
        /// Required, pass true to use Email Preview
        /// </summary>
        [DataMember]
        [XmlElement]
        public bool? EmailPreview { get; set; }

        /// <summary>
        /// Required, Blast From e-mail address
        /// </summary>
        [DataMember]
        [XmlElement]
        public string EmailFrom { get; set;  }

        /// <summary>
        /// Required, Blast Reply To e-mail address
        /// </summary>
        [DataMember]
        [XmlElement]
        public string ReplyTo { get; set; }

        /// <summary>
        /// Required, Blast From e-mail name
        /// </summary>
        [DataMember]
        [XmlElement]
        public string EmailFromName { get; set; }

        /// <summary>
        /// Required, Subject line.  If passing emojis, they should be in unicode escape format(\uXXXX)
        /// </summary>
        [DataMember]
        [XmlElement]
        public string EmailSubject { get; set; }


        /// <summary>
        /// Optional, group to send test blast to
        /// </summary>
        [DataMember]
        [XmlElement]
        public int? GroupID { get; set; }

        /// <summary>
        /// Optional, name for new Group
        /// </summary>
        [DataMember]
        [XmlElement]
        public string GroupName { get; set; }

        /// <summary>
        /// Optional, CampaignItemID to associate this test blast with
        /// If you pass CampaignItemID, do not pass CampaignID or CampaignItemName
        /// </summary>
        [DataMember]
        [XmlElement]
        public int? CampaignItemID { get; set; }

        /// <summary>
        /// Optional, CampaignItemName for new CampaignItem
        /// If you pass CampaignItemName, do not pass CampaignItemID
        /// </summary>
        [DataMember]
        [XmlElement]
        
        public string CampaignItemName { get; set; }

        /// <summary>
        /// Optional, CampaignID to associate new CampaignItem
        /// If you pass CampaignID, do not pass CampaignName
        /// </summary>
        [DataMember]
        [XmlElement]
        public int? CampaignID { get; set; }

        /// <summary>
        /// Optional, CampaignName for new CampaignName
        /// If you pass CampaignName, do not pass CampaignID or CampaignItemID
        /// </summary>
        [DataMember]
        [XmlElement]
        public string CampaignName { get; set; }


        /// <summary>
        /// Required, set to true to enable the Google Cache Buster
        /// </summary>
        [DataMember]
        [XmlElement]
        public bool? EnableCacheBuster { get; set; }

        /// <summary>
        /// Required, set to true to also send a text version
        /// </summary>
        [DataMember]
        [XmlElement]
        public bool? SendText { get; set; }


        /// <summary>
        /// New Quick Test Blast ID
        /// Do not send when creating a new Quick Test Blast.  
        /// This field will be populated in the return object.
        /// </summary>
        [DataMember]
        [XmlElement]
        public int? BlastID { get; set; }
    }
}