#define DEBUG_ATTRIBUTES

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
//using System.Web.Mvc;
using System.Web.Http.Filters;
using System.Web.Http;
using System.Web.Http.Controllers;

using ECN_Framework_Entities.Accounts;

using UAD.API.Exceptions;

using UAD.API.Infrastructure.Authentication;

namespace UAD.API.Attributes
{
    /// <summary>
    /// Implements custom authentication by extracting a "API Access Key" from the HTTP header.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class UserAuthTokenRequiredAttribute : OrderedActionFilterAttribute //ActionFilterAttribute
    {
        /// <summary>
        /// configures the Authentication Provider to require a User level Access Key
        /// </summary>
        public static AuthenticationProvider.Settings AuthenticationSettings = new AuthenticationProvider.Settings
        {
            AccessKeyRequired = AuthenticationProvider.Settings.AccessKeyType.User
        };
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //base.OnActionExecuting(actionContext);
            AuthenticationProvider.Authenticate(AuthenticationSettings, actionContext);
        }

        /*
        // CRITICAL NOTE: this sig is for the alternative (NON-API) implementation in System.Web.Mvc
        //    public override void OnActionExecuting(ActionExecutingContext filterContext)
        // while the one below (from System.Web.Http) is for MVC API:
        /// <summary>
        /// Overrides the base behavior of an <see cref="System.Web.Http.Filters.ActionFilterAttribute"/>
        /// to extract a valid <see cref="ECN_Framwork_Entities.Accounts.User"/> from the <code>APIAccessKey</code>
        /// HTTP header value or, if this cannot be done (for example, because the header is not valid or not provided)
        /// short-circuits the request returning a <code>401 Unauthorized</code> Error to the client.
        /// </summary>
        /// <param name="actionContext">the action context associated with the current request.</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string apiAccessKey;
            Guid guidParseTemp;

            // 1. ensure the header is present
            if (false == actionContext.Request.Headers.Contains(Strings.Headers.APIAccessKeyHeader))
            {
                /// !!!!!!!!!!
                /// !!!  SECURITY NOTE: the below line allows APIAccessKey to be passed via 
                /// !!!  any of (QueryString|Form|HTTP Cookie|Server Variable).  This is 
                /// !!!  very convenient for browser testing and maybe useful functionality
                /// !!!  however it's also exploitable, especially if we don't require HTTP.
                /// !!!
                /// !!!  Comment out the following line to accept token ONLY via HTTP header.
                apiAccessKey = null == HttpContext.Current ? null : HttpContext.Current.Request.Params[Strings.Headers.APIAccessKeyHeader];
                /// !!!!!!!!!!

                if (String.IsNullOrEmpty(apiAccessKey))
                {
                    throw new APIAccessKeyException(
                        Strings.Format(Strings.Errors.FriendlyMessages.Authentication.MISSING,Strings.Headers.APIAccessKeyHeader));
                }
                else
                {
                    System.Diagnostics.Trace.TraceWarning("WebAPI -> Authentication token header taken from QueryString");
                }
            }
            else
            {
                apiAccessKey = actionContext.Request.Headers.FirstOrDefault(x => x.Key == Strings.Headers.APIAccessKeyHeader).Value.FirstOrDefault();
            }

            // 2. verify the header is filled
            if(String.IsNullOrWhiteSpace(apiAccessKey))
            {
                throw new APIAccessKeyException(
                    Strings.Format(Strings.Errors.FriendlyMessages.Authentication.EMPTY, Strings.Headers.APIAccessKeyHeader));
            }

            // 3. verify the header is properly formed
            else if( false == Guid.TryParseExact(apiAccessKey,"D",out guidParseTemp))
            {
                throw new APIAccessKeyException(
                    Strings.Format(Strings.Errors.FriendlyMessages.Authentication.MALFORMED, Strings.Headers.APIAccessKeyHeader, apiAccessKey));
            }

            // 4. attempt retrieving user from DB by API key
            User apiUser = KMPlatform.BusinessLogic.User.GetByAccessKey(apiAccessKey, true);

            // 5. verify we collected a user from the given API token
            if (null == apiUser)
            {
                throw new APIAccessKeyException(Strings.Format(
                    Strings.Errors.FriendlyMessages.Authentication.UNKNOWN, "user", "authentication token", apiAccessKey));
            }

            // 6. Okay!  Tuck key and user away for latter reference
            actionContext.Request.Properties[Strings.Headers.APIAccessKeyHeader] = apiAccessKey;
            actionContext.Request.Properties[Strings.Properties.APIUserStashKey] = apiUser;
        }*/
    }
}