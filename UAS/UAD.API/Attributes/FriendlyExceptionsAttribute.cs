using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;

using UAD.API.Exceptions;
using UAD.API.ExtentionMethods;

namespace UAD.API.Attributes
{
    /// <summary>
    /// Wraps Exceptions raised with additional content (such as "Friendly" Message, Error Code and Error Reference) for consumption by clients.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class FriendlyExceptionsAttribute : OrderedExceptionFilterAttribute
    {

        public static readonly List<Exceptions.ExceptionConfiguration> DefaultExceptionHandlers = 
            new List<ExceptionConfiguration>(Exceptions.ExceptionConfigurationLibrary.GetExceptionHandlers());

        /// <summary>
        /// Establish default exception handlers
        /// </summary>
        public FriendlyExceptionsAttribute() : base() { ExceptionHandlers = DefaultExceptionHandlers;  }

        /// <summary>
        /// Transforms a <see cref="System.Net.HttpStatusCode"/> into a string containing
        /// both the numeric and textual portions.
        /// </summary>
        /// <param name="statusCode">an HTTP status code</param>
        /// <returns>a string containing a concatenation of the numeric and textual representations of the 
        /// given HTTP status code.</returns>
        public static string FormatErrorStatus(HttpStatusCode statusCode)
        {
            return String.Format("{0} {1}", (int)statusCode, statusCode.ToString());
        }

        /// <summary>
        /// List of enriched exception details, mapped to raised exceptions at runtime via Type and (optionally) handler.
        /// </summary>
        public List<ExceptionConfiguration> ExceptionHandlers { get; set; }

        /// <summary>
        /// If true, all exceptions will be wrapped with additional content.
        /// </summary>
        public bool CatchUnfilteredExceptions { get; set; }

        /// <summary>
        /// Constructor accepting two optional parameters controlling the invocation order
        /// and whether to wrap all exceptions or only those cataloged in the
        /// <see cref="EmailMarketing.API.Exceptions.ExceptionConfigurationLibrary"/>
        /// </summary>
        /// <param name="order">identifies the precedence of the instance with respect to other filters
        /// irrespective of Filter type.  Lower values indicate higher precedence and more immediate 
        /// invocation.</param>
        /// <param name="catchUnfilteredExceptions">If true, all exceptions will be wrapped irrespective
        /// of whether they are mapped explicitly to additional content (e.g. message, status, and reference 
        /// detail) with the global <see cref="EmailMarketing.API.Exceptions.ExceptionConfigurationLibrary"/>
        /// </param>
        public FriendlyExceptionsAttribute(int order = 0, bool catchUnfilteredExceptions = true)
        {
            this.Order = order;
            ExceptionHandlers = new List<ExceptionConfiguration>();
            CatchUnfilteredExceptions = catchUnfilteredExceptions;
        }

        /// <summary>
        /// Implements global exception handling to provide consistent output from exceptions raised
        /// such as human readable Message text describing the problem and an HTTP Status Code presented 
        /// as text mirroring the servers response code.
        /// </summary>
        /// <param name="actionExecutedContext">the HTTP Action Context of the request having generated the exception.</param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var exception = actionExecutedContext.Exception;
            string errorCodeString = null;
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            ExceptionConfiguration exceptionDefinition = null;
            LookupException(actionExecutedContext.Exception, out exceptionDefinition);
            
            if (CatchUnfilteredExceptions || null != exceptionDefinition)
            {
                // set the friendly message
                string friendlyMessage = exceptionDefinition != null ? exceptionDefinition.FriendlyMessage(exception) : exception.Message;

                // create the friendly HTTP error
                var friendlyHttpError = new HttpError(friendlyMessage);

                // if we found a globalExceptionDefinition then set properties of our friendly httpError object accordingly
                if (exceptionDefinition != null)
                {
                    // set the status code
                    if(exceptionDefinition.StatusCodeHandler != null)
                    {
                        statusCode = exceptionDefinition.StatusCodeHandler(exception);
                    }
                    else
                    {
                        statusCode = exceptionDefinition.StatusCode;
                    }                 

                    // error code uses configured value otherwise custom stringification of the statusCode
                    errorCodeString = false == string.IsNullOrEmpty(exceptionDefinition.ErrorCode)
                                    ? exceptionDefinition.ErrorCode
                                    : FormatErrorStatus(statusCode);

                    // add optional error reference
                    if (!string.IsNullOrEmpty(exceptionDefinition.ErrorReference))
                    {
                        friendlyHttpError[Strings.Errors.ErrorReferenceKey] = exceptionDefinition.ErrorReference;
                    }
                }

                // error messages always include an Error Code
                friendlyHttpError[Strings.Errors.ErrorCodeKey] = false == String.IsNullOrEmpty(errorCodeString) 
                                                               ? errorCodeString
                                                               : FormatErrorStatus(statusCode);

                // set the response to our friendly HTTP error
                actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(statusCode, friendlyHttpError);

            }

            // flow through to the base
            //base.OnException(actionExecutedContext);
        }

        /// <summary>
        /// Interrogates the configured <see cref="EmailMarketing.API.Exceptions.ExceptionConfigurationLibrary"/>,
        /// returning the first <see cref="EmailMarketing.API.Exceptions.ExceptionConfiguration"/> entry associated 
        /// to <code>exception</code> based on type and which does not defined a Handler or which does not decline to handle
        /// the <code>exception</code>.
        /// </summary>
        /// <param name="exception">the exception to be handled</param>
        /// <param name="exceptionMatch">Output paramater; the first
        /// <see cref="EmailMarketing.API.Exceptions.ExceptionConfiguration"/> matching <code>exception</code>, or <code>NULL</code>
        /// if none could be found.</param>
        /// <returns>true if an <see cref="EmailMarketing.API.Exceptions.ExceptionConfiguration"/> was found and 
        /// <code>exceptionMatch</code> was set to a non-null value; otherwise false.</returns>
        private bool LookupException(Exception exception, out ExceptionConfiguration exceptionMatch)
        {
            exceptionMatch = null;

            //var possibleMatches = ExceptionHandlers.Where(e => e.ExceptionType.IsInstanceOrSubclassOf(exception.GetType()));
            var possibleMatches = ExceptionHandlers.Where(e => e.ExceptionType == exception.GetType());
            foreach (var possibleMatch in possibleMatches)
            {
                if (possibleMatch.Handle == null || possibleMatch.Handle(exception))
                {
                    exceptionMatch = possibleMatch;

                    return true;
                }
            }

            return false;
        }       

    }
}