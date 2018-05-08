using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace EmailMarketing.API.Models
{
    /// <summary>
    /// SearchBase model 
    /// </summary>
    [Serializable]
    [DataContract(Namespace="")]
    [XmlRoot("SearchBase")]
    public class SearchBase
    {
        /// <summary>
        /// List of SeachProperty objects
        /// </summary>
        [DataMember]
        public List<SearchProperty> SearchCriteria { get; set; }
    }
}