using System;
using System.Web.Http.Controllers;
using KMPlatform.Entity;
using ECN_Framework_Entities.Accounts;

using FrameworkModel = KMPlatform.Entity.Client;
using FrameworkObjectManager = KMPlatform.BusinessLogic.Client;

namespace UAD.API.Infrastructure.Authentication
{
    public class ClientRequestHeaderParser : AccessKeyRequestHeaderParserBase<FrameworkModel>
    {

        /// <inheritdoc/>
        protected override string FrameworkObjectFriendlyName
        {
            get { return "client"; }
        }

        /// <inheritdoc/>
        protected override string FrameworkObjectStashKey
        {
            get { return Strings.Properties.APIClientStashKey; }
        }



        /// <inheritdoc/>
        protected override bool TryGetFrameworkObject(HttpActionContext actionContext, Guid parsedHeaderValue, out Client frameworkObject)
        {
            frameworkObject = null;
            // if a user was collected it's customer ID must match the one given
            // once Platform User is implemented this section will change...
            if (actionContext.Request.Properties.ContainsKey(Strings.Properties.APIUserStashKey))
            {
                User apiUser = (User) actionContext.Request.Properties[Strings.Properties.APIUserStashKey];
                if (null != apiUser)
                {
                    if (apiUser.DefaultClientID > 0)
                    {
                        // we received a User level API access key and it has the same CustomerID as the header

                        KMPlatform.Entity.Client c = new FrameworkObjectManager().Select(apiUser.DefaultClientID, true);
                        frameworkObject = c;
                        if (apiUser.IsPlatformAdministrator || apiUser.UserClientSecurityGroupMaps.Exists(x => x.ClientID == c.ClientID && x.IsActive == true))
                        {

                            actionContext.Request.Properties[Strings.Properties.APIClientStashKey] = c;
                            frameworkObject = c;
                            return true;
                        }
                        else
                        {
                            
                            return false;
                        }
                    }
                    else
                    {
                        // we received a user level API access key but it has a different (or no) customer ID
                        // special case: explicitly throw so we generate a 403 Forbidden instead of the usual 401 Unauthorized
                        throw new ECN_Framework_Common.Objects.SecurityException();
                    }
                }
                else
                {
                    // we did not receive a User level API access key (this should be effectively unreachable)
                }
            }
            else
            {
                // we did not receive a User level API access key
            }
            
            return false;
        }
    }
}