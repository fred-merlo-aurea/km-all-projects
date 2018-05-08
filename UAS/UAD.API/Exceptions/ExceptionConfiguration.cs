using System.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;

using UAD.API.ExtentionMethods;

namespace UAD.API.Exceptions
{
    /// <summary>
    /// Generics version of the <see cref="ExceptionConfiguration"/>
    /// </summary>
    /// <typeparam name="T">Exception Type</typeparam>
    public class ExceptionConfiguration<T> : ExceptionConfiguration
    {
        public ExceptionConfiguration(string friendlyMessage = null, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) :
            base(typeof(T),friendlyMessage,statusCode)    
        {
        }

        public ExceptionConfiguration(Func<Exception, string> friendlyMessage, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) :
            base(typeof(T),friendlyMessage,statusCode)
        {
        }
    }

    /// <summary>
    /// Maps internal errors to outward profiles by adding a friendly name and (optionally) an error code and reference link.
    /// </summary>
    public class ExceptionConfiguration
    {
        //const string ARGUMENT_NULL_EXCEPTION_FMT = "Argument '{0}' cannot be null.";
        //const string ARGUMENT_MUST_INHERIT_FROM_FMT = "Type must inherit from {0}.";

        public Type ExceptionType { get; set; }
        public Func<Exception, string> FriendlyMessage { get; set; }

        public Func<Exception, bool> Handle { get; set; }
        public Func<Exception, HttpStatusCode> StatusCodeHandler { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public string ErrorCode { get; set; }
        public string ErrorReference { get; set; }

        public ExceptionConfiguration(Type exceptionType, string friendlyMessage = null, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) :
            this(exceptionType, (ex) => friendlyMessage ?? ex.Message, statusCode) { }

        public ExceptionConfiguration(Type exceptionType, Func<Exception, string> friendlyMessage, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            if(null == friendlyMessage)
            {
                throw new ArgumentException("friendlyMessage");
            }
            if (null == exceptionType)
            {
                throw new ArgumentNullException("exceptionType");
            }
            if(false == exceptionType.IsInstanceOrSubclassOf(typeof(Exception)))
            {
                throw new ArgumentException("ExceptionConfiguration: \"exceptionType\" must inherit from System.Exception");
            }
            
            ExceptionType = exceptionType;
            FriendlyMessage = friendlyMessage;
            StatusCode = statusCode;
        }
    }
}
