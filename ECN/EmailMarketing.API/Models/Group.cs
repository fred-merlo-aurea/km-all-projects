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
    /// The Email Marketing Group API object model.
    /// </summary>
    [Serializable]
    [DataContract]
    [XmlRoot("Group")]
    public class Group
    {
        /// <summary>
        /// Default/parameterless constructor
        /// </summary>
        public Group()
        {
            GroupID = -1;
            FolderID = null;
            GroupName = string.Empty;
            GroupDescription = string.Empty;
        }

        #region Properties
        /// <summary>
        /// unique <code>id</code> for a Group object, Property ignored when passed in PUT or POST method
        /// </summary>
        [DataMember]
        [XmlElement]
        public int GroupID { get; set; }

        /// <summary>
        /// Name for the Group object.
        /// </summary>
        [DataMember]
        [XmlElement]
        public string GroupName { get; set; }

        /// <summary>
        /// unique <code>id</code> for a Folder categorizing a Group object
        /// </summary>
        [DataMember]
        [XmlElement]
        public int? FolderID { get; set; }

        /// <summary>
        /// Description for the Group object.
        /// </summary>
        [DataMember]
        [XmlElement]
        public string GroupDescription { get; set; }

        /// <summary>
        /// Is Content Archived
        /// </summary>
        /// <remarks>
        /// Is Content Archived
        /// </remarks>
        [DataMember]
        [XmlElement]
        public bool Archived { get; set; }


        #endregion
    }
}
