using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;

namespace EmailMarketing.API.Infrastructure.Authentication
{
    /// <summary>
    /// Implements the general pattern for derived classes to parse specific types of API Access Keys (e.g. User Access Key, etc.)
    /// </summary>
    /// <typeparam name="FrameworkObjectT"></typeparam>
    abstract public class AccessKeyRequestHeaderParserBase<FrameworkObjectT> : RequestHeaderParserBase<Guid, FrameworkObjectT>
        where FrameworkObjectT : class
    {
        /// <inheritdoc/>
        protected sealed override string HttpHeader
        {
            get { return Strings.Headers.APIAccessKeyHeader; }
        }

        /// <inheritdoc/>
        protected sealed override string HeaderValueFriendlyName
        {
            get { return "authentication token"; }
        }

        /// <inheritdoc/>
        protected sealed override bool TryParseHeaderValue(HttpActionContext actionContext, string headerValue, out Guid parsedHeaderValue)
        {
            return Guid.TryParseExact(headerValue, "D", out parsedHeaderValue);
        }
    }
}