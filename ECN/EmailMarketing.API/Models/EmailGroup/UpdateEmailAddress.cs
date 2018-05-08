using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace EmailMarketing.API.Models.EmailGroup
{
    /// <summary>
    /// The model for updating an email address across a customer
    /// </summary>
    [DataContract]
    [Serializable]
    [XmlRoot("UpdateEmailAddress")]
    public class UpdateEmailAddress
    {
        /// <summary>
        /// The old email address
        /// </summary>
        [Required]
        [DataMember]        
        public string OldEmailAddress { get; set; }
        /// <summary>
        /// The new email address
        /// </summary>
        [Required]
        [DataMember]
        public string NewEmailAddress { get; set; }
    }
}
