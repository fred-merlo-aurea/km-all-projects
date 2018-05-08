using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using AMSServicesDocumentation.Attributes;

namespace AMSServicesDocumentation.Models
{
    /// <summary>
    /// The UAD Services Content API object model.
    /// </summary>
    [Serializable]
    [DataContract]
    [XmlRoot("Content")]
    public class Content
    {
        /// <summary>
        /// Default/parameterless constructor
        /// </summary>
        public Content()
        {
            ContentID = -1;
            FolderID = 0;
            UseWYSIWYGeditor = false;
            ContentSource = string.Empty;
            ContentText = string.Empty;
            ContentTitle = string.Empty;
          //  Archived = false;
            UpdatedDate = DateTime.Now;
        }

        #region Properties
        /// <summary>
        /// unique <code>id</code> for a Content object, Property ignored when passed as parameter in PUT or POST request. 
        /// </summary>
        [DataMember]
        [XmlElement]
        public int ContentID { get; set; }

        /// <summary>
        /// unique <code>id</code> for a Folder categorizing a Content object
        /// </summary>
        [DataMember]
        [XmlElement]public int FolderID { get; set; }

        /// <summary>
        /// Supply to the POST method to specify whether the WYSIWYG editor should
        /// be used when creating Content. Once Content is created this setting cannot be changed using the PUT method. 
        /// Property ignored when passed as parameter in PUT METHOD.
        /// </summary>
        [DataMember]
        [XmlElement]
        public bool UseWYSIWYGeditor { get; set; }

        /// <summary>
        /// Formatted source data for the ContentObject, typically represented as HTML.  Will not be returned in Search requests.
        /// </summary>
        [DataMember]
        [XmlElement]
        [ExcludeFromSearch(Exclude = true, ExclusionMessage = "Please use Location property to retrieve Content Source")]
        public string ContentSource { get; set; }

        /// <summary>
        /// Plain text source data for the Content object. If NULL is passed, the property defaults to ContentSource - html version.
        /// Will not be returned in Search requests.
        /// </summary>
        [DataMember]
        [XmlElement]
        [ExcludeFromSearch(Exclude = true, ExclusionMessage = "Please use Location property to retrieve Content Text")]
        public string ContentText { get; set; }

        /// <summary>
        /// Title for the Content object.
        /// </summary>
        /// <remarks>
        /// Test Content Title
        /// </remarks>
        [DataMember]
        [XmlElement]
        public string ContentTitle { get; set; }

        
        /// <summary>
        /// Is Content Archived
        /// </summary>
        /// <remarks>
        /// Is Content Archived
        /// </remarks>
        [DataMember]
        [XmlElement]
        public bool? Archived { get; set; }


        /// <summary>
        /// Date/Time of the last update to the Content object. Property ignored when passed as parameter in request. 
        /// </summary>
        [DataMember] 
        [Newtonsoft.Json.JsonIgnore]
        [IgnoreDataMember]
        
        public DateTime UpdatedDate { get; set; }

        #endregion
    }
}
