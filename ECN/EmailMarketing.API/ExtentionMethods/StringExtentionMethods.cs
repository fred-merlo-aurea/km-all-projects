using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailMarketing.API.ExtentionMethods
{
    /// <summary>
    /// Extension methods providing additional capabilities for <see cref="System.String"/>
    /// </summary>
    public static class StringExtentionMethods
    {
        /// <summary>
        /// Returns the <code>context</code> string with the first letter converted to upper-case.
        /// </summary>
        /// <param name="context">a <code>System.String</code></param>
        /// <returns>the content of <code>context</code> with the first letter converted to upper-case.</returns>
        public static string ToUpperFirst(this string context)
        {
            return string.IsNullOrEmpty(context)
                ? string.Empty:
                char.ToUpper(context[0]) + context.Substring(1);
        }
    }
}