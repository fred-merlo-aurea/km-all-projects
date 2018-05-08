using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmailMarketing.API.ExtentionMethods
{
    /// <summary>
    /// Extension methods providing additional capabilities for <see cref="System.Type"/>
    /// </summary>
    public static class TypeExtentionMethods
    {
        /// <summary>
        /// Determines if Type <code>subject</code> is the same or class or a subclass of type <code>context</code>.
        /// </summary>
        /// <param name="context">a System.Type</param>
        /// <param name="subject">another System.Type</param>
        /// <returns>true of Type <code>subject</code> is the same or a class inheriting from Type <code>context</code></returns>
        public static bool IsInstanceOrSubclassOf(this Type context, Type subject)
        {
            return context == subject || context.IsSubclassOf(subject);
        }
    }

}