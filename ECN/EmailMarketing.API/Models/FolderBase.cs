using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace EmailMarketing.API.Models
{
    [Serializable]
    [DataContract]
    public class FolderBase
    {
        /// <summary>
        /// Unique Folder identification number
        /// </summary>
        /// <remarks>
        /// Property ignored when passed as parameter in PUT or POST request. 
        /// </remarks>
        [DataMember]
        [XmlElement]
        public int FolderID { get; set; }

        /// <summary>
        /// Date created
        /// </summary> 
        [IgnoreDataMember]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// ID of User who created this folder
        /// </summary>
        [IgnoreDataMember]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]

        public int CreatedUserID { get; set; }

        /// <summary>
        /// Date last updated
        /// </summary>
        [IgnoreDataMember]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        public DateTime? UpdatedDate { get; set; }
        
        /// <summary>
        /// ID of User who last updated
        /// </summary>
        [IgnoreDataMember]
        [Newtonsoft.Json.JsonIgnore]
        [XmlIgnore]
        public int UpdatedUserID { get; set; }
    }
}