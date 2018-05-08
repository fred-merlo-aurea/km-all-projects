using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Http.Filters;
using System.Web.Http;
using System.Web.Http.Controllers;

using UAD.API.Exceptions;

namespace UAD.API.Infrastructure.Authentication
{
    /// <summary>
    /// Base class for authenticating HTTP requests by parsing an HTTP header to create a Framework Object
    /// </summary>
    /// <typeparam name="HeaderValueT">Type for the HTTPHeader value, once it has been parsed</typeparam>
    /// <typeparam name="FrameworkObjectT">Type for the Framework Object</typeparam>
    abstract public class RequestHeaderParserBase<HeaderValueT, FrameworkObjectT>
        where HeaderValueT : struct
        where FrameworkObjectT : class
    {
        /// <summary>The HTTPHeader to be parsed, as a string</summary>
        abstract protected string HttpHeader { get; }
        /// <summary>A friendly name for the HTTPHeader (e.g. "authentication token")</summary>
        abstract protected string HeaderValueFriendlyName { get; }
        /// <summary>A friendly name for the Framework Object (e.g. "User")</summary>
        abstract protected string FrameworkObjectFriendlyName { get; }
        /// <summary>The Request.Properties key in which to store the Framework Object created</summary>
        abstract protected string FrameworkObjectStashKey { get; }
        /// <summary>When overridden in a derived class, attempts to create a strongly typed value from the string value 
        /// of the HTTP header</summary>
        /// <param name="headerValue">HTTP header value, as a string</param>
        /// <param name="parsedHeaderValue">strongly type HTTP header value</param>
        /// <returns>True if the string value of the HTTP header was successfully parsed into a strongly typed value, otherwise false.</returns>
        abstract protected bool TryParseHeaderValue(HttpActionContext actionContext, string headerValue, out HeaderValueT parsedHeaderValue);
        /// <summary>When overridden in a derived class, attempts to create a Framework object from the parsed header value</summary>
        /// <param name="parsedHeaderValue">The strongly typed value from the HTTP header</param>
        /// <param name="frameworkObject">An object into which the created framework object will be stored.</param>
        /// <returns>True if a framework object was successfully created, otherwise false.</returns>
        abstract protected bool TryGetFrameworkObject(HttpActionContext actionContext, HeaderValueT parsedHeaderValue, out FrameworkObjectT frameworkObject);

        /// <summary>
        /// Provides a uniform method for extracting an HTTP header value as a string, parsing that into a strongly typed value
        /// and using the strongly typed value to create a Framework object and storing this for use by API Controllers.
        /// If this cannot be done (for example, because the header is not valid or not provided), uniform error handling is 
        /// provided to ensure API users understand the specific error conditions causing authentication failure (e.g. returning
        /// a message describing the specific problem in addition to a <code>401 Unauthorized</code> Error to the client.)
        /// </summary>
        /// <param name="actionContext">the action context associated with the current request.</param>
        public virtual void ValidateRequestHeader(HttpActionContext actionContext)
        {
            string failureMessage;
            string headerValue;
            HeaderValueT parsedHeaderValue;
            FrameworkObjectT frameworkObject;

            if(TryValidateRequestHeader(actionContext,out failureMessage, out headerValue, out parsedHeaderValue, out frameworkObject))
            {
                StoreProperties(actionContext, parsedHeaderValue, frameworkObject);
                return;
            }

            RaiseException(failureMessage);
        }

        /// <summary>
        /// Responsible for stashing away the parsed header value and framework object for later use
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="parsedHeaderValue"></param>
        /// <param name="frameworkObject"></param>
        public virtual void StoreProperties(HttpActionContext actionContext, HeaderValueT parsedHeaderValue, FrameworkObjectT frameworkObject)
        {
            actionContext.Request.Properties[HttpHeader] = parsedHeaderValue;
            actionContext.Request.Properties[FrameworkObjectStashKey] = frameworkObject;
        }

        /// <summary>
        /// Responsible for raising authentication exceptions
        /// </summary>
        /// <param name="messages"></param>
        public virtual void RaiseException(params string[] messages)
        {
            throw new APIAccessKeyException(String.Join(",", messages));
        }

        /// <summary>
        /// Responsible for validating and parsing the header and creating the framework object or, in
        /// failure cases, generating the failure message.
        /// </summary>
        /// <param name="actionContext">action context for the current request</param>
        /// <param name="failureMessage">if unsuccessful, this will describe the reason for failure, otherwise null</param>
        /// <param name="headerValue">set to the string value of the HTTP header, if present, otherwise null.</param>
        /// <param name="parsedHeaderValue">set to the strongly typed value of the HTTP header if the header can be parsed, otherwise set to the default value of HeaderValueT</param>
        /// <param name="frameworkObject">set to the framework object created/retrieved if successful, otherwise null.</param>
        /// <returns>true if the header is present, and be parsed, and yields a framework object; otherwise and failureMessage is set to a non-null value.</returns>
        public bool TryValidateRequestHeader(HttpActionContext actionContext, out string failureMessage, out string headerValue, out HeaderValueT parsedHeaderValue, out FrameworkObjectT frameworkObject)
        {
            failureMessage = "An unknown error occurred";
            headerValue = null;
            parsedHeaderValue = default(HeaderValueT);
            frameworkObject = null;
            IEnumerable<string> values;

            // 1. ensure the header is present
            if (false == actionContext.Request.Headers.Contains(HttpHeader))
            {
                failureMessage = Strings.Format(Strings.Errors.FriendlyMessages.Authentication.MISSING, HttpHeader);
                return false;
            }
            else
            {
                        headerValue = actionContext.Request.Headers.FirstOrDefault(x => x.Key.ToLower() == HttpHeader.ToLower()).Value.FirstOrDefault();
            }

            // 2. verify the header is filled
            if (String.IsNullOrWhiteSpace(headerValue))
            {
                failureMessage = Strings.Format(Strings.Errors.FriendlyMessages.Authentication.EMPTY, HttpHeader);
                return false;
            }

            // 3. verify the header can be parsed
            else if (false == TryParseHeaderValue(actionContext, headerValue, out parsedHeaderValue))
            {
                failureMessage = Strings.Format(Strings.Errors.FriendlyMessages.Authentication.MALFORMED, HttpHeader, headerValue);
                return false;
            }

            // 4. verify we are able to create a framework object from the supplied value
            if (false == TryGetFrameworkObject(actionContext, parsedHeaderValue, out frameworkObject))
            {
                failureMessage = Strings.Format(Strings.Errors.FriendlyMessages.Authentication.UNKNOWN, FrameworkObjectFriendlyName, HeaderValueFriendlyName, parsedHeaderValue);
                return false;
            }

            // 5. Okay!
            failureMessage = null;
            return true;
        }
    }
}