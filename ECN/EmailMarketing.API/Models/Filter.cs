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
    /// The Email Marketing Filter API object model.
    /// </summary>
    [Serializable]
    [DataContract]
    [XmlRoot("Filter")]
    public class Filter
    {
        /// <summary>
        /// Default/parameterless constructor
        /// </summary>
        public Filter()
        {
            FilterID = -1;
            FilterName = string.Empty;
           
        }

        #region Properties
        /// <summary>
        /// unique <code>id</code> for a Filter object, Property ignored when passed as parameter in PUT or POST request. 
        /// </summary>
        [DataMember]
        [XmlElement]
        public int FilterID { get; set; }

        /// <summary>
        /// Name for the Filter object.
        /// </summary>
        [DataMember]
        [XmlElement]
        public string FilterName { get; set; }


        /// <summary>
        /// Group the Filter is applied to  
        /// </summary>
        /// <remarks>
        /// What Group Filter is associated with 
        /// </remarks>
        [DataMember]
        [XmlElement]
        public int GroupID { get; set; }


        /// <summary>
        /// Is Filter Archived
        /// </summary>
        /// <remarks>
        /// Is Filter Archived
        /// </remarks>
        [DataMember]
        [XmlElement]
        public bool Archived { get; set; }

        
        #endregion
    }
}
