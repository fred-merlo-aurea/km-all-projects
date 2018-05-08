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
    [XmlRoot("PersonalizationContent")]
    public class PersonalizationContent
    {
        /// <summary>
        /// Create a PersonalizationContent
        /// </summary>
        public PersonalizationContent()
        {
            BlastID = -1;
            Emails = new List<PersonalizationEmail>();
        }
        /// <summary>
        /// Blast ID for the Personalization Blast
        /// </summary>
        [DataMember]
        [XmlElement]
        public int BlastID { get; set; }

        /// <summary>
        /// List of Emails with content and email subject
        /// </summary>
        [DataMember]
        [XmlElement]
        
        public List<PersonalizationEmail> Emails { get; set; }

    }

    [Serializable]
    [DataContract]
    
    public class PersonalizationEmail
    {
        /// <summary>
        /// Container for Personalization emails
        /// </summary>
        public PersonalizationEmail()
        {
            EmailAddress = "";
            HTMLContent = "";
            TextContent = "";
            EmailSubject = "";
        }

        /// <summary>
        /// Email address of the subscriber
        /// </summary>
        [DataMember]
        [XmlElement]
        public string EmailAddress {
            get
            {
                return this._EmailAddress;
            }
            set
            {
                this._EmailAddress = value.TrimEnd();
            }
        }

        /// <summary>
        /// Personalized HTML content for the subscriber
        /// </summary>
        [DataMember]
        [XmlElement]
        public string HTMLContent {
            get
            {
                return this._HTMLContent;
            }
            set
            {
                this._HTMLContent = value.TrimEnd();
            }
        }

        /// <summary>
        /// Personalized Text content for the subscriber
        /// </summary>
        [DataMember]
        [XmlElement]
        public string TextContent {
            get
            {
                return this._TextContent;
            }
            set
            {
                this._TextContent = value.TrimEnd();
            }
        }

        /// <summary>
        /// Personalized Email Subject for the subscriber
        /// </summary>
        [DataMember]
        [XmlElement]
        public string EmailSubject {
            get
            {
                return this._EmailSubject;
            }
            set
            {
                this._EmailSubject = value.TrimEnd();
            }
        }

        private string _EmailAddress { get; set; }
        private string _EmailSubject { get; set; }
        private string _HTMLContent { get; set; }
        private string _TextContent { get; set; }

    }


}