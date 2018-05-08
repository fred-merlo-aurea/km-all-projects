using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Text;

namespace EmailMarketing.API.Models
{
    [Serializable]
    [DataContract]
    [XmlRoot("Image")]
    public class Image// :  ECN_Framework_Entities.Communicator.Image
    {
        public Image()
        {
            ImageName = string.Empty;
            FolderName = string.Empty;
            ImageURL = string.Empty;

            FolderID = -1;
            ImageID = 0;
            
            ImageData = null;

        }


        /// <summary>
        /// <code>FolderName</code> for an Image object, 
        /// </summary>
        [DataMember]
        [XmlElement]
        public string FolderName { get; set; }

        /// <summary>
        /// <code>FolderID</code> for an Image object, 
        /// </summary>
        [DataMember]
        [XmlElement]
        public int FolderID { get; set; }

        ///<summary>
        /// unique <code>ImageID</code> for an Image object,
        /// </summary>
        [DataMember]
        [XmlElement]
        public int ImageID { get; set; }

        /// <summary>
        /// unique <code>ImageName</code> for an Image object, 
        /// </summary>
        [DataMember]
        [XmlElement]
        public string ImageName { get; set; }

        /// <summary>
        /// <code>ImageData</code> for an Image object, 
        /// </summary>
        [DataMember]
        [XmlElement]
        public byte[] ImageData { get; set; }

        /// <summary>
        /// <code> ImageURL </code> for an Image object,
        /// <summary>
        [DataMember]
        [XmlElement]
        public string ImageURL { get; set; }
    }

}