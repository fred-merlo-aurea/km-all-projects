using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace EmailMarketing.API.Models.EmailGroup
{
    
    [Serializable]
    [DataContract(Namespace = "")]
    [XmlRoot("ManageProfile")]
    public class ManageProfile
    {
        /// <summary>
        /// The GroupID 
        /// </summary>
        [DataMember]
        [Required]
        public int GroupID { get; set; }
        /// <summary>
        /// The format type code
        /// </summary>
        [DataMember]
        [Required]
        public Formats Format { get; set; }
        /// <summary>
        /// The subscribe type code
        /// </summary>
        [DataMember]
        [Required]
        public SubscribeTypes SubscribeType { get; set; }
        /// <summary>
        /// List of one or many profiles
        /// </summary>
        [DataMember]
        public List<Profile> Profiles { get; set; }
    }
}