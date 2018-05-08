using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace EmailMarketing.API.Models.EmailGroup
{
    ///<Summary>
    ///Model for updating an email address and profile data.
    ///Inherits from Profile
    ///</Summary>
    [Serializable]
    [DataContract]
    [XmlRoot("UpdateEmailAddressForGroup")]
    public class UpdateEmailAddressForGroup : Profile
    {
        /// <summary>
        /// The old email address
        /// </summary>
        [Required]
        [DataMember]
        public string OldEmailAddress { get; set; }
        /// <summary>
        /// The group id the email address is in
        /// </summary>
        [Required]
        [DataMember]
        public int GroupID { get; set; }

    }
}