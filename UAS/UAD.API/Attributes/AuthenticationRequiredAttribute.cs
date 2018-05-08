using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;

using UAD.API.Infrastructure.Authentication;

namespace UAD.API.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class AuthenticationRequiredAttribute : OrderedActionFilterAttribute
    {
        /// <summary>
        /// get/set the authentication configuration
        /// </summary>
        public AuthenticationProvider.Settings AuthenticationSettings { get; set; }
        
        /// <summary>
        /// require authentication prior to executing an action (or any action, when applied to an API controller)
        /// </summary>
        /// <param name="order">the order in which this attribute should be applied, with zero being first. (default: 0).</param>
        /// <param name="AccessKey">what level of access key is required (default User)</param>
        /// <param name="customerIdRequired">whether a customer id is required (default: true)</param>
        public AuthenticationRequiredAttribute(
            int order=0,
            AuthenticationProvider.Settings.AccessKeyType AccessKey=AuthenticationProvider.Settings.AccessKeyType.User,
            bool RequiredCustomerId = true
            ) :base(order)
        {
            AuthenticationSettings = new AuthenticationProvider.Settings
            {
                AccessKeyRequired = AccessKey,
                CustomerIDRequired = false
            };
        }

        /// <summary>
        /// Perform authentication checks before executing the requested action
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            AuthenticationProvider.Authenticate(AuthenticationSettings, actionContext);
        }
    }
}