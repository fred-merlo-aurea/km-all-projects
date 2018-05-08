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
using KMPlatform.Entity;
using ECN_Framework_Entities.Accounts;

using UAD.API.Exceptions;

namespace UAD.API.Attributes
{
    /// <summary>
    /// Implements custom authentication by extracting a "API Access Key" from the HTTP header.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class CustomerIdRequiredAttribute : OrderedActionFilterAttribute //ActionFilterAttribute
    {
        /// <summary>
        /// Extracts the Customer ID from HTTP header value or, if this cannot be done (for example, 
        /// because the header is not valid or not provided) short-circuits the request returning a 
        /// <code>401 Unauthorized</code> Error to the client.
        /// </summary>
        /// <param name="actionContext">the action context associated with the current request.</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string customerIdString = String.Empty;
            int customerID = -1;

            // 1. ensure the header is present
            if (false == actionContext.Request.Headers.Contains(Strings.Headers.CustomerIdHeader))
            {
                customerIdString = null == HttpContext.Current ? null : HttpContext.Current.Request.Params[Strings.Headers.CustomerIdHeader];
                if (String.IsNullOrWhiteSpace(customerIdString))
                {
                    throw new APIAccessKeyException(
                        Strings.Format(Strings.Errors.FriendlyMessages.Authentication.MISSING, Strings.Headers.CustomerIdHeader));
                }
                else
                {
                    System.Diagnostics.Trace.TraceWarning("WebAPI -> Authentication CustomerID header taken from QueryString");
                }
            }
            else
            {
                customerIdString = actionContext.Request.Headers.FirstOrDefault(x => x.Key == Strings.Headers.CustomerIdHeader).Value.FirstOrDefault();
            }

            // 2. verify the header is filled
            if(String.IsNullOrWhiteSpace(customerIdString))
            {
                throw new APIAccessKeyException(
                    Strings.Format(Strings.Errors.FriendlyMessages.Authentication.EMPTY, Strings.Headers.CustomerIdHeader));
            }

            // 3. verify the header is properly formed
            else if( false == Int32.TryParse(customerIdString, out customerID))
            {
                throw new APIAccessKeyException(
                    Strings.Format(Strings.Errors.FriendlyMessages.Authentication.MALFORMED, Strings.Headers.CustomerIdHeader, customerIdString));
            }

            // 4. attempt retrieving user from DB by API key
            Customer apiCustomer = null;
            try { apiCustomer = ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(customerID, false); } 
            catch { }

            // 5. verify we collected a user from the given API token
            if (null == apiCustomer)
            {
                throw new APIAccessKeyException(Strings.Format(
                    Strings.Errors.FriendlyMessages.Authentication.UNKNOWN, "customer", "ID", customerID));
            }

            // 6. ensure that the given customer matches that of the API User;
            User apiUser = (User)actionContext.Request.Properties[Strings.Properties.APIUserStashKey];
            if(null == apiUser) // sanity check
            {
                // AuthToken required must be processed first, therefor must have a have a lower Order value than this one!
                throw new ApplicationException("Incorrectly ordered custom HTTP header handlers!");
            }
            else if(null == apiUser.CustomerID)
            {
                throw new NotImplementedException("API access for users not associated with a customer is not currently supported");
            }
            else if(apiCustomer.CustomerID != apiUser.CustomerID)
            {
                throw new ECN_Framework_Common.Objects.SecurityException("user/customer mismatch");
            }

            // 7. Okay!  Tuck id and customer away for latter reference
            actionContext.Request.Properties[Strings.Headers.CustomerIdHeader] = customerID;
            actionContext.Request.Properties[Strings.Properties.APICustomerStashKey] = apiCustomer;
        }
    }
}