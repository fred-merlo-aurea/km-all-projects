using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using EcnExceptionEntity=ECN_Framework_Common.Objects.Enums.Entity;
using EcnExceptionMethod=ECN_Framework_Common.Objects.Enums.Method;

namespace EmailMarketing.API.Search
{
    /// <summary>
    /// contains search configuration items organized by search property name
    /// </summary>
    public class SearchConfigurationGroup : Dictionary<string, SearchConfigurationItem>
    {
        /// <summary>
        /// Provides a default value for whether to allow empty queries (e.g. search without constraints)
        /// </summary>
        public const bool AllowEmptySearchDefault = false;

        /// <summary>
        /// If false, empty queries (e.g. searches without any constraints) will cause an exception.
        /// </summary>
        /// <seealso cref="AllowEmptySearchDefault"/>
        public bool AllowEmptySearch { get; set; }

        /// <summary>
        /// The ECN_Framework_Common.Objects.Enums.Entity to be associated in case of validation issues
        /// </summary>
        public ECN_Framework_Common.Objects.Enums.Entity ExceptionEntity { get; set; }

        /// <summary>
        /// The ECN_Framework_Common.Objects.Enums.Method to be associated in case of validation issues
        /// </summary>
        public ECN_Framework_Common.Objects.Enums.Method ExceptionMethod { get; set; }

        /// <summary>
        /// Creates an SearchConfigurationLibrary, optionally setting the Exception Entity  and Exception Method
        /// to be associated with any exceptions raised while validating the search query.
        /// </summary>
        /// <param name="exceptionEntity">Entity associated with exceptions raised while validation the search query</param>
        /// <param name="exceptionMethod">Methods associated with exceptions raised while validation the search query</param>
        public SearchConfigurationGroup(
            bool allowEmptySearch = AllowEmptySearchDefault,
            EcnExceptionEntity exceptionEntity = EcnExceptionEntity.None, 
            EcnExceptionMethod exceptionMethod = EcnExceptionMethod.None)
        {
            AllowEmptySearch = allowEmptySearch;
            ExceptionEntity = exceptionEntity;
            ExceptionMethod = exceptionMethod;
        }
    }
}