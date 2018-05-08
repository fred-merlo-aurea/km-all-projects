using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAD.API
{
    /// <summary>
    /// Statically defined strings, such as for naming Request Properties (e.g. "Stash" buckets).
    /// </summary>
    public static class Strings
    {
        /// <summary>
        /// Formats errors and other messages using <see cref="String.Format"/>
        /// </summary>
        /// <param name="message">Message for to be formatted.</param>
        /// <param name="args">positional replacement token values</param>
        /// <returns></returns>
        public static string Format(string message, params object[] args)
        {
            return String.Format(message, args);
        }

        /// <summary>
        /// String constants related to HTTP request routing.
        /// </summary>
        public static class Routing
        {
            public const string DefaultApiRouteName = "DefaultApi";
            public const string RouteControllerKey = "controller";
            public const string RouteMethodKey = "action";
            public const string RouteSubjectIDKey = "id";

        }
        
        /// <summary>
        /// String constants related to HTTP headers and header processing.
        /// </summary>
        public static class Headers
        {
            /// <summary>
            /// HTTP header used to pass in the APIAccessKey
            /// </summary>
            public const string APIAccessKeyHeader = "APIAccessKey";

            /// <summary>
            /// HTTP header used to pass in the CustomerID
            /// </summary>
            public const string CustomerIdHeader = "X-Customer-ID";
        }

        /// <summary>
        /// Strings used as Keys in the request scoped Properties "stash"
        /// </summary>
        public static class Properties
        {
            /// <summary>
            /// Stash property used to store Business User object once derived from APIAccessKey
            /// </summary>
            public const string APIUserStashKey = "APIUser";

            /// <summary>
            /// Stash property used to store BaseChannel object once derived from APIAccessKey
            /// </summary>
            public const string APIBaseChannelStashKey = "APIBaseChannel";

            /// <summary>
            /// Stash property used to store Business Customer object once derived from CustomerIdHeader
            /// </summary>
            public const string APICustomerStashKey = "APICustomer";

            /// <summary>
            /// Stash property used to store Business Client object once derived from APIAccessKey
            /// </summary>
            public const string APIClientStashKey = "APIClient";
        }

        /// <summary>
        /// Business facing string constants
        /// </summary>
        public static class BusinessDefaultValues
        {
            /// <summary>
            /// Default title for new content when we receive a POST request with no (or empty) ContentTitle
            /// </summary>
            public const string ContentTitle = @"CONTENT_{0}";

            /// <summary>
            /// Default process name, used for internal tracking of which high-level process initiated an activity.
            /// </summary>
            public const string ProcessName = "WebAPI";

            /// <summary>
            /// Default FROM address when sending (or enqueuing send of) email from API methods
            /// </summary>
            public const string EmailFromAddress = @"info@knowledgemarketing.com";

            /// <summary>
            /// Default FROM Name when sending (or enqueuing send of) email from API methods
            /// </summary>
            public const string EmailFromName = @"Knowledge Marketing";

            /// <summary>
            /// The UserName for the forms user
            /// </summary>
            public const string FormsUserName = @"F0rm5U5er";
        }

        /// <summary>
        /// String constants related to error, such as exception messages and internal error data storage.
        /// </summary>
        public static class Errors
        {
            /// <summary>
            /// Exception Message attribute key for Error Code
            /// </summary>
            public const string ErrorCodeKey = "HttpStatusCode";

            /// <summary>
            /// Exception Message attribute key for Error Reference
            /// </summary>
            public const string ErrorReferenceKey = "ErrorReference";

            public const string APIAccessKeyExceptionDataKey = "APIAccessKeyExceptionMessage";
            public const string ImageExceptionDataKey = "ImageExceptionMessage";  

            public const string APIResourceNotFoundExceptionResourceKey = "NotFoundResourceType";
            public const string APIResourceNotFoundExceptionIDKey = "NotFoundIDValue";

            public const string MuligroupProfileManamangetUnsupported = "UNSUPPORTED: actions for a single profile with multiple groups are not currently supported";

            /// <summary>
            /// String constants exposed to API users in the event errors occur.
            /// </summary>
            public static class FriendlyMessages
            {
                // 404
                public const string RESOURCE_NOT_FOUND = @"Resource not found.  No {0} has ID ""{1}"".";

                // 401
                /// <summary>
                /// String constants related to errors during authentication.
                /// </summary>
                public static class Authentication
                {
                    public const string MISSING = @"Authentication Token Header ""{0}"" is missing";
                    public const string EMPTY = @"Authentication Token Header ""{0}"" is empty";
                    public const string MALFORMED = @"Authentication Token Header ""{0}"" is malformed: {1}";
                    public const string UNKNOWN = @"No {0} with {1} ""{2}""";
                    
                }

                public static class Images
                {
                    public const string NO_DIRECTORY = @"FolderName does not exist";
                    public const string NOT_NEW_IMAGE = @"ImageName already exists";
                    public const string IMAGE_DOES_NOT_EXIST = @"ImageName does not exist";
                    public const string MORE_THAN_ONE_IMAGE = @"More than one image exists in this folder";
                }

                /*
                //400
                public const string STRING_FIELD_IS_REQUIRED = @"""{0}"" is required and may not be empty";
                public const string FOLDER_DOES_NOT_EXIST = "FOLDER DOES NOT EXIST FOR CUSTOMER";
                public const string TITLE_ALREADY_EXISTS = @"""{0}"" ALREADY EXISTS FOR CUSTOMER";
                 */

            }
        }
    }
}