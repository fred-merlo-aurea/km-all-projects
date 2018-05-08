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
    [XmlRoot("PersonalizationBlast")]
    public class PersonalizationBlast : BlastBase
    {
        /// <summary>
        /// Create a Personalization Blast
        /// </summary>
        public PersonalizationBlast()
        {
            SendTime = null;
            BlastType = "Regular";
            LayoutID = -1;
            GroupID = -1;
            OptOutGroupID = -1;
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

        /// <summary>
        /// Blast Field 1
        /// </summary>
        [DataMember]
        [XmlElement]
        public string BlastField1 { get; set; }
        /// <summary>
        /// Blast Field 2
        /// </summary>
        [DataMember]
        [XmlElement]
        public string BlastField2 { get; set; }
        /// <summary>
        /// Blast Field 3
        /// </summary>
        [DataMember]
        [XmlElement]
        public string BlastField3 { get; set; }
        /// <summary>
        /// Blast Field 4
        /// </summary>
        [DataMember]
        [XmlElement]
        public string BlastField4 { get; set; }
        /// <summary>
        /// Blast Field 5
        /// </summary>
        [DataMember]
        [XmlElement]
        public string BlastField5 { get; set; }

        /// <summary>
        /// Optional, GroupID for Group that subscriber will be unsubscribed from along with the Group that the Blast was sent to
        /// </summary>
        [DataMember]
        [XmlElement]
        public int? OptOutGroupID { get; set; }

        /// <summary>
        /// Optional, GroupID for Group that will be used as a suppression group
        /// </summary>
        [DataMember]
        [XmlElement]
        public int? SuppressionGroupID { get; set; }


        /// <summary>
        /// Optional, Filter to use on Suppression group
        /// </summary>
        [DataMember]
        [XmlElement]
        public int? SuppressionGroupFilterID { get; set; }
    


        /// <summary>
        /// Send time for Personalization Blast
        /// </summary>
        [DataMember]
        [XmlElement]
        public DateTime? SendTime { get; set; }
        
        /// <summary>
        /// Provide a CampaignID to create a new simple blast under a specific campaign.  
        /// Pass 0 to create/associate the simple blast under your "Marketing Campaign" campaign.
        /// Will be ignored on PUT requests.
        /// </summary>
        [DataMember]
        [XmlElement(IsNullable = true)]
        public int? CampaignID { get; set; }
    }
}