using System;
using System.Web.Http.Controllers;
using KMPlatform.Entity;
using ECN_Framework_Entities.Accounts;

namespace UAD.API.Infrastructure.Authentication
{
    /// <summary>
    /// provides methods to parse the Customer ID header and create a Customer object
    /// </summary>
    public sealed class CustomerIdRequestHeaderParser : RequestHeaderParserBase<int,Customer>
    {
        /// <inheritdoc/>
        protected override string HttpHeader
        {
            get { return Strings.Headers.CustomerIdHeader; }
        }

        /// <inheritdoc/>
        protected override string HeaderValueFriendlyName
        {
            get { return "ID"; }
        }

        /// <inheritdoc/>
        protected override string FrameworkObjectFriendlyName
        {
            get { return "customer"; }
        }

        /// <inheritdoc/>
        protected override string FrameworkObjectStashKey
        {
            get { return Strings.Properties.APICustomerStashKey; }
        }

        /// <inheritdoc/>
        protected override bool TryParseHeaderValue(HttpActionContext actionContext, string headerValue, out int parsedHeaderValue)
        {
            return Int32.TryParse(headerValue, out parsedHeaderValue);
        }

        /// <inheritdoc/>
        protected override bool TryGetFrameworkObject(HttpActionContext actionContext, int parsedHeaderValue, out Customer frameworkObject)
        {
            frameworkObject = null;
            try
            {
                frameworkObject = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(parsedHeaderValue, true);
            } catch
            {
                // unknown customerID caused throw from business layer
                // catch here so that we return a 401 Unauthorized with a custom message, 
                // instead of the generic 403 Forbidden that we normally show for a security access violation
            }
            if (null != frameworkObject)
            {
                // if a user was collected it's customer ID must match the one given
                // once Platform User is implemented this section will change...
                if (actionContext.Request.Properties.ContainsKey(Strings.Properties.APIUserStashKey))
                {
                    User apiUser = (User)actionContext.Request.Properties[Strings.Properties.APIUserStashKey];
                    if (null != apiUser)
                    {
                        if (apiUser.CustomerID != null)
                        {
                            // we received a User level API access key and it has the same CustomerID as the header
                            ECN_Framework_Entities.Accounts.Customer c = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(parsedHeaderValue, false);
                            if (apiUser.IsPlatformAdministrator || apiUser.UserClientSecurityGroupMaps.Exists(x => x.ClientID == c.PlatformClientID && x.IsActive == true))
                            {
                                apiUser.CustomerID = c.CustomerID;
                                actionContext.Request.Properties[Strings.Properties.APIUserStashKey] = apiUser;
                                return true;
                            }
                            else
                                return false;
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
                else if(actionContext.Request.Properties.ContainsKey(Strings.Properties.APIBaseChannelStashKey))
                {
                    // we got a base-channel API access key, validate that the customer ID belongs to the given basechannel
                    BaseChannel apiChannel = (BaseChannel)actionContext.Request.Properties[Strings.Properties.APIBaseChannelStashKey];
                    if (null != apiChannel)
                    {
                        if(frameworkObject.BaseChannelID.HasValue && apiChannel.BaseChannelID == frameworkObject.BaseChannelID.Value)
                        {
                            
                            // customer has the same base-channel as the API Access Key, OK.
                            return true;
                        }
                        else
                        {
                            // special case: we received a customer-ID for a customer that is not associated with the base-channel
                            // inferred from the API Access Key header, return 403 Forbidden instead of the usual 401 Unauthorized
                            throw new ECN_Framework_Common.Objects.SecurityException();
                        }
                    }
                    else
                    {
                        // we did not receive a valid BaseChannel from the API Access Key, this should be unreachable
                    }
                }
                else
                {
                    // we did not receive a User level API access key
                }
            }
            else
            {
                // we did not get an object from the business layer
            }
            return false;
        }
    }
}