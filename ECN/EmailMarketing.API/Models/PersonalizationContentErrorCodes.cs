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
    [XmlRoot("PersonalizationContentErrorCodes")]
    public class PersonalizationContentErrorCodes
    {

        /// <summary>
        /// Default constructor
        /// </summary>
        public PersonalizationContentErrorCodes()
        {
            ErrorCode = -1;
            ErrorMessage = "";
        }

        /// <summary>
        /// ID for the Error
        /// </summary>
        [DataMember]
        [XmlElement]
        public int ErrorCode { get; set; }

        /// <summary>
        /// Friendly error message
        /// </summary>
        [DataMember]
        [XmlElement]
        public string ErrorMessage { get; set; }

    }
}