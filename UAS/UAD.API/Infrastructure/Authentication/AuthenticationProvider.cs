using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;

namespace UAD.API.Infrastructure.Authentication
{
    /// <summary>
    /// Provides the high-level logic for authentication.
    /// </summary>
    public class AuthenticationProvider
    {
        /// <summary>Provides authentication configuration settings</summary>
        public class Settings
        {
            /// <summary>
            /// Specifies what level of AccessKey will be required, if any.
            /// </summary>
            [Flags]
            public enum AccessKeyType
            {
                /// <summary>
                /// no AccessKey is required
                /// </summary>
                None = 0,
                /// <summary>
                /// a User level AccessKey is required
                /// </summary>
                User = 1,
                /// <summary>
                /// a Customer level AccessKey is required
                /// </summary>
                Customer = 2 ,
                /// <summary>
                /// a BaseChannel level AccessKey is required
                /// </summary>
                BaseChannel = 4,
                /// <summary>
                /// a valid AccessKey of any level will be accepted
                /// </summary>
                Any = AccessKeyType.User | AccessKeyType.Customer | AccessKeyType.BaseChannel
            }

            /// <summary>
            /// If true, a CustomerID must be provided.
            /// </summary>
            public bool CustomerIDRequired { get; set; }

            /// <summary>
            /// Indicates the level of AccessKey will be required, if any.
            /// </summary>
            public AccessKeyType AccessKeyRequired { get; set; }
        }

        private static CustomerIdRequestHeaderParser CustomerIdRequestHeaderParser = new CustomerIdRequestHeaderParser();
        private static UserAccessKeyRequestHeaderParser UserAccessKeyRequestHeaderParser = new UserAccessKeyRequestHeaderParser();
        private static BaseChannelAccessKeyRequestHeaderParser BaseChannelAccessKeyRequestHeaderParser = new BaseChannelAccessKeyRequestHeaderParser();
        private static ClientRequestHeaderParser ClientRequestHeaderParser = new ClientRequestHeaderParser();
        /// <summary>
        /// Ensured that the current action context is authenticated base on the given settings
        /// </summary>
        /// <param name="actionContext">the context for the current request</param>
        public static void Authenticate(Settings authenticationSettings, HttpActionContext actionContext)
        {
            // check that we have been passed (one of) the allowed AccessKey header(s)
            if (authenticationSettings.AccessKeyRequired != Settings.AccessKeyType.None)
            {
                bool foundAccessKey = false;
                List<string> messages = new List<string>();
                if (Settings.AccessKeyType.User == (authenticationSettings.AccessKeyRequired & Settings.AccessKeyType.User))
                {
                    string failureMessage, headerValue;
                    Guid accessKey;
                    KMPlatform.Entity.User apiUser;
                    if(UserAccessKeyRequestHeaderParser.TryValidateRequestHeader(actionContext,out failureMessage,out headerValue,out accessKey,out apiUser))
                    {
                        foundAccessKey = true;
                        UserAccessKeyRequestHeaderParser.StoreProperties(actionContext, accessKey, apiUser);
                        KMPlatform.Entity.Client c = new KMPlatform.BusinessLogic.Client().Select(apiUser.DefaultClientID, true);
                        ClientRequestHeaderParser.StoreProperties(actionContext, accessKey, c);
                    }
                    else
                    {
                        messages.Add(failureMessage);
                    }
                }

                if (Settings.AccessKeyType.Customer == (authenticationSettings.AccessKeyRequired & Settings.AccessKeyType.Customer))
                {
                    //actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);

                    throw new NotImplementedException();
                }

                if (Settings.AccessKeyType.BaseChannel == (authenticationSettings.AccessKeyRequired & Settings.AccessKeyType.BaseChannel))
                {
                    string failureMessage, headerValue;
                    Guid accessKey;
                    ECN_Framework_Entities.Accounts.BaseChannel apiUser;
                    if (BaseChannelAccessKeyRequestHeaderParser.TryValidateRequestHeader(actionContext, out failureMessage, out headerValue, out accessKey, out apiUser))
                    {
                        foundAccessKey = true;
                        BaseChannelAccessKeyRequestHeaderParser.StoreProperties(actionContext, accessKey, apiUser);
                    }
                    else
                    {
                        messages.Add(failureMessage);
                    }
                }

                if(false == foundAccessKey)
                {
                    throw new Exceptions.APIAccessKeyException(String.Join(",", messages));
                }
            }

            // check the Customer-ID header, if necessary
            if (authenticationSettings.CustomerIDRequired)
            {
                CustomerIdRequestHeaderParser.ValidateRequestHeader(actionContext);
            }
        }
    }
}