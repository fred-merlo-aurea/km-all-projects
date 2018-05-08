using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace EmailMarketing.API.Models
{
    [Serializable]
    [DataContract]
    [XmlRoot("Folder")]
    public class Folder : FolderBase
    {
        #region Properties

        /// <summary>
        /// Unique folder name
        /// </summary>
        [DataMember]
        [XmlElement]
        public string FolderName { get; set; }
        /// <summary>
        /// ID of parent folder
        /// </summary>
        [DataMember]
        [XmlElement]
        public int ParentID { get; set; }
        /// <summary>
        /// Short user-defined description of folder
        /// </summary>
        [DataMember]
        [XmlElement]
        public string FolderDescription { get; set; }
        /// <summary>
        /// Options: "CNT", "GRP" ...
        /// </summary>
        [DataMember]
        [XmlElement]
        public string FolderType { get; set; }
        /// <summary>
        /// Identifier for Customer
        /// </summary>
        [DataMember]
        [XmlElement]
        public int CustomerID { get; set; }

        #endregion
    }
}