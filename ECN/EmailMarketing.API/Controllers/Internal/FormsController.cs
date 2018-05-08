using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using APIModel = EmailMarketing.API.Models.FormsCustomer;

using AccessKeyType = EmailMarketing.API.Infrastructure.Authentication.AuthenticationProvider.Settings.AccessKeyType;
using EmailMarketing.API.Attributes;


namespace EmailMarketing.API.Controllers.Internal
{
    /// <summary>
    /// INTERNAL Special API methods to support development of the Forms application
    /// </summary>
    [RoutePrefix("api/internal/forms")]
    [Route("")]
    public class FormsController : AuthenticatedUserControllerBase
    {
        /// <inheritdoc/>
        public override ECN_Framework_Common.Objects.Enums.Entity FrameworkEntity
        {
            get { return ECN_Framework_Common.Objects.Enums.Entity.FormsSpecificAPI; }
        }
        public override string ControllerName
        {
            get { return "forms"; }
        }

        /// <summary>
        /// Returns the list of Forms Customers associated with BaseChannel inferred from the API Access Key provided. 
        /// Note: this method requires a Base-Channel level API Access Key and does NOT require Customer-ID header.
        /// </summary>
        /// <returns></returns>
        /// <example for="request"><![CDATA[
        /// GET http://api.ecn5.com/api/internal/forms/methods/GetFormsCustomersForBaseChannel HTTP/1.1
        /// Accept: application/json
        /// APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        /// Host: api.ecn5.com
        /// 
        /// ]]></example>
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 200 OK
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Content-Type: application/json; charset=utf-8
        /// Expires: -1
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Thu, 18 Jun 2015 21:32:55 GMT
        /// Content-Length: 501
        /// 
        /// [
        ///    {
        ///       "CustomerID":99999,
        ///       "CustomerName":"A Curious Customer",
        ///       "AccessKey":"00000000-0000-0000-0000-000000000001"
        ///    },
        ///    {
        ///       "CustomerID":99999,
        ///       "CustomerName":"A Curiouser Customer",
        ///       "AccessKey":"00000000-0000-0000-0000-000000000002"
        ///    }
        /// ]
        /// ]]></example>
        [AuthenticationRequired(AccessKey: AccessKeyType.BaseChannel, RequiredCustomerId: false)] // override authentication properties
        [Route("methods/GetFormsCustomersForBaseChannel")]
        [Logged]
        public IEnumerable<APIModel> GetFormsCustomersForBaseChannel()
        {
            if(null == APIBaseChannel)
            {
                throw new ApplicationException("Unable to discover Base-Channel from API Access key.  Did pass a BaseChannel level API access key?");
            }

            IEnumerable<APIModel> returnValue = from x in ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(APIBaseChannel.BaseChannelID)
                    select new APIModel()
                    {
                        CustomerID = x.CustomerID,
                        CustomerName = x.CustomerName,
                        AccessKey = GetFormsCustomerAccessKey(x.CustomerID)
                    };

            return returnValue;
        }

        private Guid GetFormsCustomerAccessKey(int customerId)
        {
            KMPlatform.Entity.User user = UserController.InternalGet(customerId);
            if (user != null)   // && user.AccessKey
            {
                return user.AccessKey;
            }

            return default(Guid);
        }
    }
}
