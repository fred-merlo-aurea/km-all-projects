using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailMarketing.API.Search
{
    public class SearchConfigurationItem
    {
        /// <summary>
        /// A method for validating a search criterion.
        /// </summary>
        /// <param name="g">Group containing the configuration item being validated</param>
        /// <param name="q">the complete search query property set of which the current property value is a member</param>
        /// <param name="c">Configuration item being validated</param>
        /// <param name="p">property value supplied for the configuration item being validated</param>
        /// <returns>true if property value provides supported search criteria</returns>
        public delegate bool ValidationDelegate(
            SearchConfigurationGroup g,
            IEnumerable<Models.SearchProperty> q,
            SearchConfigurationItem c, 
            Models.SearchProperty p);

        /// <summary>
        /// Collection of methods to provide basic property validation
        /// </summary>
        public static class BasicValidationMethods
        {  
            public static ValidationDelegate AlwaysOK = (g, q, c, p) => true;
            //public static ValidationDelegate SingleValueOnly = (g, q, c, p) => p.ValueSet.Count == 1;
            public static ValidationDelegate ComparatorAllowed = (g, q, c, p) => c.Comparators.Contains(p.Comparator);
            public static ValidationDelegate ValuesMatchType = (g, q, c, p) => false == string.IsNullOrEmpty(p.ValueSet.ToString()) || p.ValueSet.GetType() != c.PropertyType;//.Any(x => x == null || x.GetType() != c.PropertyType);
            public static ValidationDelegate Default = (g,q,c,p) => ComparatorAllowed(g,q,c,p) && ValuesMatchType(g,q,c,p);
            public static ValidationDelegate DiscreteCriteria = (g, q, c, p) => q.Count() == 1 && Default(g, q, c, p);
        }

        /// <summary>
        /// The ECN_Framework_Common.Objects.Enums.Entity to be associated in case of validation issues
        /// </summary>
        public bool ExceptionEntity { get; set; }

        public string PropertyName { get; set; }
        public Type PropertyType { get; set; }
        public string[] Comparators { get; set; }
        public ValidationDelegate ValidationMethod { get; set; }

        public SearchConfigurationItem()
        {
            ValidationMethod = BasicValidationMethods.Default;
        }
    }
}