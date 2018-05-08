using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using APIModel = EmailMarketing.API.Models.User;
using FrameworkModel = KMPlatform.Entity.User;

using AccessKeyType = EmailMarketing.API.Infrastructure.Authentication.AuthenticationProvider.Settings.AccessKeyType;

namespace EmailMarketing.API.Controllers.Internal
{
    // aliasing inside namespace where the declarations will then be identical in each controller
    using SearchQuery = List<Models.SearchProperty>;
    using SearchResult = Models.SearchResult<APIModel>;
    using EmailMarketing.API.Attributes;

    /// <summary>INTERNAL API methods exposing base-channel</summary>
    [RoutePrefix("api/internal/user")]
    [Route("")]
    public class UserController : SearchableApiControllerBase<APIModel,FrameworkModel>
    {

        static internal FrameworkModel InternalGet(int customerID)
        {
            return KMPlatform.BusinessLogic.User.GetByCustomerID(customerID).
                Where(u => u.UserName == Strings.BusinessDefaultValues.FormsUserName).FirstOrDefault();
        }

        #region abstract methods implementation

        /// <inheritdoc/>
        public override ECN_Framework_Common.Objects.Enums.Entity FrameworkEntity
        {
            get { return ECN_Framework_Common.Objects.Enums.Entity.User; }
        }

        public override string[] ExposedProperties
        {
            get { return new string[] { "UserID", "UserName", "AccessKey", "CustomerID" }; }
        }

        public override object GetID(APIModel model)
        {
            return model.UserID;
        }

        public override object GetID(FrameworkModel model)
        {
            return model.UserID;
        }

        public override string ControllerName
        {
            get { return "user"; }
        }

        #endregion abstract methods implementation
        #region exposed methods

        /// <summary>
        /// Retrieve user detail for the forms user associated with the current customer.
        /// <em>NOTE: this method requires a <u>BaseChannel</u> level Access Key to be provided 
        /// in the APIAccessKey header.</em>
        /// </summary>
        /// <example for="request"><![CDATA[
        /// GET http://api.ecn5.com/api/internal/user/methods/GetFormsUser HTTP/1.1
        /// Accept: application/json
        /// Accept-Language: en-US
        /// APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        /// X-Customer-ID: 99999
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
        /// Date: Thu, 18 Jun 2015 17:43:32 GMT
        /// Content-Length: 105
        /// 
        /// {
        ///    "UserID":12345,
        ///    "CustomerID":77777,
        ///    "UserName":"nameOfFormsUser",
        ///    "AccessKey":"00000000-0000-0000-0000-000000000000"
        /// }
        /// ]]></example>
        [Route("methods/GetFormsUser")]
        [AuthenticationRequired(AccessKey: AccessKeyType.BaseChannel)] // override authentication properties
        [Logged]
        public APIModel GetFormsUser()
        {
            FrameworkModel formsUser = InternalGet(APICustomer.CustomerID);
            formsUser.CustomerID = APICustomer.CustomerID;
            if (null == formsUser)
            {
                //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound).EnsureSuccessStatusCode());
                RaiseNotFoundException(CustomerID, "forms user for customer");
            }
            return Transform(formsUser);
        }

        #endregion exposed methods
        #region REST

        /// <summary>
        /// This feature is not implemented
        /// </summary>
        /// <example for="request"><![CDATA[]]></example>
        /// <example for="response"><![CDATA[]]></example>
        [Route("{id}")]
        public APIModel Get(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This feature is not implemented
        /// </summary>
        /// <example for="request"><![CDATA[]]></example>
        /// <example for="response"><![CDATA[]]></example>
        public APIModel Post([FromBody]APIModel apiModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This feature is not implemented
        /// </summary>
        /// <example for="request"><![CDATA[]]></example>
        /// <example for="response"><![CDATA[]]></example>
        [Route("{id}")]
        public APIModel Put(int id, [FromBody]APIModel apiModel)
        {
            throw new NotImplementedException();
        }
                
        /// <summary>
        /// This feature is not implemented
        /// </summary>
        /// <example for="request"><![CDATA[]]></example>
        /// <example for="response"><![CDATA[]]></example>
        [Route("{id}")]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        #endregion REST
        #region search

        /// <summary>
        /// This feature is not implemented
        /// </summary>
        [Route("~/api/internal/search/user")]
        [HttpGet]
        [HttpPost]
        public IEnumerable<SearchResult> Search([FromBody] SearchQuery searchQuery)
        {
            throw new NotImplementedException();
        }

        #endregion search
        #region data cleansing
        #endregion data cleansing
    }
}
