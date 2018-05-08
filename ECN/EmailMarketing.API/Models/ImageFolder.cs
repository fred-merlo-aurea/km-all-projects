using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace EmailMarketing.API.Models
{
    /// <summary>
    /// Represents an folder containing customer images
    /// </summary>
    [Serializable]
    [DataContract]
    [XmlRoot("ImageFolder")]
    public class ImageFolder // : ECN_Framework_Entities.Communicator.ImageFolder
    {
        public ImageFolder() {
            FolderID = -1;
            FolderName = string.Empty;
            FolderFullName = string.Empty;
            FolderUrl = string.Empty;
        }

        /// <summary>
        /// Unique identifier for an image folder.  Note: this is currently always 0 (zero).
        /// </summary>
        [DataMember]
        [XmlElement]
        public int FolderID { get; set; }

        /// <summary>
        /// Name of the image folder, not including any path information.
        /// </summary>
        [DataMember]
        [XmlElement]
        public string FolderName { get; set; }

        /// <summary>
        /// Name of the image folder including the full path.  Full path is relative to the root folder for customer images; 
        /// ignored when creating new folders.
        /// </summary>
        [DataMember]
        [XmlElement]
        public string FolderFullName { get; set; }

        /// <summary>
        /// A URI suitable for creating a link to the image folder.
        /// </summary>
        [DataMember]
        [XmlElement]
        public string FolderUrl { get; set; }
    }
}