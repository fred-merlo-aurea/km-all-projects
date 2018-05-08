using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace EmailMarketing.API.Models
{
    /// <summary>
    /// The Email Marketing Search API object model for composing queries. 
    /// A valid query includes one or more search properties, each composed
    /// per this model.  Please see individual object model.
    /// </summary>
    [XmlRoot("SearchProperty")]    
    public class SearchProperty
    {
        /// <summary>
        /// Name of a property that will be used to constrain the search.
        /// </summary>
        public string Name { get; set; }

        /// <summary>Operation to be used in comparing the named properties to. The valid values are "&gt;" , "&lt;" ,  "=" and "contains".<see cref="ValueSet"/></summary>
    
        public string Comparator { get; set; }

        /// <summary>
        /// A value to be considered per <see cref="Comparator"/>
        /// </summary>
        public string ValueSet { get; set; }
    }
}