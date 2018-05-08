using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace EmailMarketing.API.Models.EmailGroup
{

    [XmlRoot("Email")]
    public class Email : IHasEmailAddress
    {
        /// <summary>
        /// default/parameterless constructor
        /// </summary>
        public Email()
        {
            EmailAddress = null;
            GroupId = -1;
            Format = Formats.HTML;
            SubscribeType = SubscribeTypes.Subscribe;
        }

        #region properties

        /// <summary>
        /// Address to add
        /// </summary>
        public string EmailAddress { get; set; }
        /// <summary>
        /// Group to add address to
        /// </summary>
        public int GroupId { get; set; }
        public Formats Format { get; set; }
        /// <summary>
        /// Subscription type to add the address as
        /// </summary>
        public SubscribeTypes SubscribeType { get; set; }

        #endregion properties
    }
}