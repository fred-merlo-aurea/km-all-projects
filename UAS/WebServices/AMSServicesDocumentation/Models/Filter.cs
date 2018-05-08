using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace AMSServicesDocumentation.Models
{
    /// <summary>
    /// The UAD Services Filter API object model.
    /// </summary>
    [Serializable]
    [DataContract]
    [XmlRoot("Filter")]
    public class Filter
    {
        #region Properties
        /// <summary>
        /// Unique <code>id</code> for a Filter object, Property ignored when passed as parameter in PUT or POST request. 
        /// </summary>
        [DataMember]
        [XmlElement]
        public int FilterID { get; set; }
        /// <summary>
        /// Name of the Filter object.
        /// </summary>
        [DataMember]
        [XmlElement]
        public string FilterName { get; set; }
        /// <summary>
        /// Group the Filter is applied to.
        /// </summary>
        /// <remarks>
        /// What Group Filter is associated with.
        /// </remarks>
        [DataMember]
        [XmlElement]
        public int GroupID { get; set; }
        /// <summary>
        /// Is Filter Archived.
        /// </summary>
        /// <remarks>
        /// Is Filter Archived.
        /// </remarks>
        [DataMember]
        [XmlElement]
        public bool Archived { get; set; }
        #endregion
    }
}