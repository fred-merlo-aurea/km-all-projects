using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;

namespace EmailMarketing.API.Models
{
    /// <summary>
    /// The Email Marketing CustomField API object model.
    /// </summary>
    [Serializable]
    [DataContract]
    [XmlRoot("CustomField")]
    public class CustomField
    {
        /// <summary>
        /// Default/parameterless constructor
        /// </summary>
        public CustomField()
        {
            GroupDataFieldsID = -1;
            GroupID = -1;
            ShortName = string.Empty;
            LongName = string.Empty;
            IsPublic = string.Empty;
        }

        #region Properties
        /// <summary>
        /// Unique <code>id</code> for a CustomField object. Property ignored when passed as parameter in PUT or POST request
        /// </summary>
        [DataMember]
        [XmlElement]
        public int GroupDataFieldsID { get; set; }

        /// <summary>
        /// Group ID for a CustomField object, 
        /// </summary>
        [DataMember]
        [XmlElement]
        public int GroupID { get; set; }

        /// <summary>
        /// Name for the CustomField object (Cannot contain spaces).
        /// </summary>
        [DataMember]
        [XmlElement]
        public string ShortName { get; set; }

        /// <summary>
        /// Description for the CustomField object.
        /// </summary>
        [DataMember]
        [XmlElement]
        public string LongName { get; set; }

        /// <summary>
        /// Public property for the CustomField object ("Y"/"N").
        /// </summary>
        [DataMember]
        [XmlElement]
        public string IsPublic { get; set; }

        #endregion
    }
}

