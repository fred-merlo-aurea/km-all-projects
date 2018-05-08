using System;
using System.Web.Http.Controllers;

namespace UAD.API.Infrastructure.Authentication
{
    /// <summary>
    /// Provides methods to validate a User level API Access Key from request headers
    /// </summary>
    sealed public class UserAccessKeyRequestHeaderParser : AccessKeyRequestHeaderParserBase<KMPlatform.Entity.User>
    {
        /// <inheritdoc/>
        protected override string FrameworkObjectFriendlyName
        {
            get { return "user"; }
        }

        /// <inheritdoc/>
        protected override string FrameworkObjectStashKey
        {
            get { return Strings.Properties.APIUserStashKey; }
        }

        /// <inheritdoc/>
        protected override bool TryGetFrameworkObject(HttpActionContext actionContext, Guid parsedHeaderValue, out KMPlatform.Entity.User frameworkObject)
        {
            frameworkObject = KMPlatform.BusinessLogic.User.ECN_GetByAccessKey(parsedHeaderValue.ToString(), true);
            if(null != frameworkObject)
            {
                return true;
            }

            return false;
        }
    }
}