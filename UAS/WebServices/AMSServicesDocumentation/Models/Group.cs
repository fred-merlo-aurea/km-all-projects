using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace AMSServicesDocumentation.Models
{
    /// <summary>
    /// The UAD Services Group API object model.
    /// </summary>
    [Serializable]
    [DataContract]
    [XmlRoot("Group")]
    public class Group
    {
        #region Properties
        /// <summary>
        /// unique <code>id</code> for a Group object, Property ignored when passed in PUT or POST method
        /// </summary>
        [DataMember]
        [XmlElement]
        public int GroupID { get; set; }

        /// <summary>
        /// Name of the Group object.
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