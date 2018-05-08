using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml.Serialization;

namespace EmailMarketing.API.Models
{
    /// <summary>
    /// The Email Marketing Search Result API object model.
    /// </summary>
    /// <typeparam name="T">API object model of result constituents (e.g. Models.Content, 
    /// when searching Content)</typeparam>
    [Serializable]
    [DataContract(Namespace="")]
    [XmlRoot("ApiObject")]
    public class SearchResult<T>
    {
        /// <summary>
        /// An API model object 
        /// </summary>
        [XmlElement("ApiObject")]
        [DataMember]public T ApiObject { get; set; }

        /// <summary>
        /// URI for direct access to <see cref=" ApiObject"/>
        /// </summary>
        [XmlElement("Location")]
        [DataMember]public string Location { get; set; }
    }
}