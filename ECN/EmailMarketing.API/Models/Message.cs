using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Runtime.Serialization;
using System.Xml.Serialization;

using ECN_Framework_Common.Objects;
using ECN_Framework_Entities.Communicator;

namespace EmailMarketing.API.Models
{
    /// <summary>
    /// The Email Marketing Layout API object model.
    /// </summary>
    [Serializable]
    [DataContract]
    [XmlRoot("Message")]
    public class Message : FolderBase
    {
        /// <summary>
        /// Default/parameterless constructor
        /// </summary>
        public Message()
        {
            LayoutID = -1; 
            TemplateID = -1; 
            FolderID = -1;
            LayoutName = string.Empty;
            ContentSlot1 = -1; 
            ContentSlot2 = -1;
            ContentSlot2 = -1;
            ContentSlot3 = -1;
            ContentSlot4 = -1;
            ContentSlot5 = -1;
            ContentSlot6 = -1;
            ContentSlot7 = -1;
            ContentSlot8 = -1;
            ContentSlot9 = -1;
            TableOptions = string.Empty;
            DisplayAddress = string.Empty;
            MessageTypeID = -1;//post?
            CreatedDate = null;
            UpdatedDate = null;
            UpdatedUserID = -1;

            CreatedUserID = -1;
            

        }

        #region Properties

        /// <summary>
        /// Unique <code>id</code> for a Layout object, Property ignored when passed in PUT or POST method
        /// </summary>
          [DataMember]
        [XmlElement]
        public int LayoutID { get; set; }
        
        /// <summary>
        /// Unique <code>id</code> for a Template object, 
        /// </summary>
        [DataMember]
          [XmlElement]
          public int TemplateID { get; set; }

        /// <summary>
        /// Unique <code>name</code> for a Layout object, 
        /// </summary>
        [DataMember]
        [XmlElement]
        public string LayoutName { get; set; }
        /// <summary>
        /// Unique <code>id</code> for a Content object, 
        /// </summary>
        [DataMember]
        [XmlElement]
        public int? ContentSlot1 { get; set; }
        /// <summary>
        /// Unique <code>id</code> for a Content object, 
        /// </summary>
        [DataMember]
        [XmlElement]
        public int? ContentSlot2 { get; set; }
        /// <summary>
        /// Unique <code>id</code> for a Content object, 
        /// </summary>
        [DataMember]
        [XmlElement]
        public int? ContentSlot3 { get; set; }
        /// <summary>
        /// Unique <code>id</code> for a Content object, 
        /// </summary>
        [DataMember]
        [XmlElement]
        public int? ContentSlot4 { get; set; }
        /// <summary>
        /// Unique <code>id</code> for a Content object, 
        /// </summary>
        [DataMember]
        [XmlElement]
        public int? ContentSlot5 { get; set; }
        /// <summary>
        /// Unique <code>id</code> for a Content object, 
        /// </summary>
        [DataMember]
        [XmlElement]
        public int? ContentSlot6 { get; set; }
        /// <summary>
        /// Unique <code>id</code> for a Content object, 
        /// </summary>
        [DataMember]
        [XmlElement]
        public int? ContentSlot7 { get; set; }
        /// <summary>
        /// Unique <code>id</code> for a Content object, 
        /// </summary>
        [DataMember]
        [XmlElement]
        public int? ContentSlot8 { get; set; }
        /// <summary>
        /// Unique <code>id</code> for a Content object, 
        /// </summary>
        [DataMember]
        [XmlElement]
        public int? ContentSlot9 { get; set; }

        /// <summary>
        /// </summary>
        [DataMember]
        [XmlElement]
        public string TableOptions { get; set; }
        /// <summary>
        /// </summary>
        [DataMember]
        [XmlElement]
        public string DisplayAddress { get; set; }
        
       /// <summary>
       /// Identifier for the Message Type
       /// </summary>
       [DataMember]
        [XmlElement]
        public int? MessageTypeID { get; set; }
                
        /// <remarks>
        /// Is Content Archived
        /// </remarks>
        [DataMember]
        [XmlElement]
        public bool Archived { get; set; }

       
        #endregion
    }
}
