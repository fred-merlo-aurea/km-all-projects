using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using EmailMarketing.API.Models.Utility; // for Transformer<inT,outT>

using APIModel = EmailMarketing.API.Models.Customer;
using FrameworkModel = ECN_Framework_Entities.Accounts.Customer;

namespace EmailMarketing.API.Controllers.Internal
{
    // aliasing inside namespace where the declarations will then be identical in each controller
    using SearchQuery = List<Models.SearchProperty>;
    using SearchResult = Models.SearchResult<APIModel>;

    /// <summary>INTERNAL API methods exposing base-channel</summary>
    [RoutePrefix("api/internal/customer")]
    [Route("")]
    /* public */ class CustomerController : SearchableApiControllerBase<APIModel,FrameworkModel>
    {
        /// <summary>
        /// Internal Get implementation returns Framework model
        /// </summary>
        /// <param name="id">EmailDirectID</param>
        /// <returns>internal Framework model object</returns>
        internal FrameworkModel GetInternal(int id, bool getChildren=false)
        {
            return ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(id, getChildren);
        }

        #region abstract member implementations

        /// <inheritdoc/>
        public override ECN_Framework_Common.Objects.Enums.Entity FrameworkEntity
        {
            get { return ECN_Framework_Common.Objects.Enums.Entity.Customer; }
        }

        override public string ControllerName { get { return "customer"; } }
        override public string[] ExposedProperties
        {
            get { return new string[] { "CustomerID", "CustomerName", "BaseChannelID", "APIAccessKey" }; }
        }

        public override object GetID(APIModel model)
        {
            return model.CustomerID;
        }

        public override object GetID(FrameworkModel model)
        {
            return model.CustomerID;
        }

        #endregion abstract member implementations
        #region REST

        /// <summary>
        /// This feature is not implemented
        /// </summary>
        /// <example for="request"><![CDATA[]]></example>
        /// <example for="response"><![CDATA[]]></example>
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
        public APIModel Put(int id, [FromBody]APIModel apiModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This feature is not implemented
        /// </summary>
        /// <example for="request"><![CDATA[]]></example>
        /// <example for="response"><![CDATA[]]></example>
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        #endregion REST
        #region Search

        /// <summary>
        /// Provide search capabilities for Customer objects. Search supports both GET and POST methods; results will be identical.
        /// </summary>
        /// <param name="searchQuery">used to constrain the search</param>
        /// <returns>a list of matching <see cref="EmailMarketing.API.Models.Customer"/> as Location/API Object pairs.</returns>
        /// <example for="request"><![CDATA[
        /// POST http://api.ecn5.com/api/internal/customer/api/search/customer HTTP/1.1
        /// Accept: application/json
        /// APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        /// X-Customer-ID: 99999
        /// Host: api.ecn5.com
        /// Content-Type: application/json; charset=utf-8
        /// Content-Length: 88
        /// 
        /// [
        ///  {  
        ///    "name": "BaseChannelID", 
        ///    "comparator": "=", 
        ///    "valueSet": [ 123 ] 
        ///  },
        /// ]
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
        /// Date: Tue, 02 Jun 2015 20:45:56 GMT
        /// Content-Length: 2937
        /// 
        /// [
        ///    {
        ///       "ApiObject":{
        ///             "CustomerID":99998,
        ///             "CustomerName":"A Curious Customer",
        ///             "BaseChannelID":77777,
        ///             "APIAccessKey":"00000000-0000-0000-0000-000000000001"
        ///       },
        ///       "Location":"http://api.ecn5.com/api/customer/99998"
        ///    },
        ///    {
        /// "ApiObject":{
        ///             "CustomerID":99999,
        ///             "CustomerName":"A Curiouser Customer",
        ///             "BaseChannelID":77777,
        ///             "APIAccessKey":"00000000-0000-0000-0000-000000000002"
        ///       },
        ///       "Location":"http://api.ecn5.com/api/customer/99999"
        ///    }
        /// ]
        /// ]]></example>
        //[Attributes.AuthenticationRequired(AccessKey:Infrastructure.Authentication.AuthenticationProvider.Settings.AccessKeyType.BaseChannel, RequiredCustomerId: false)]
        [Route("api/search/customer")]
        [HttpGet]
        [HttpPost]
        public IEnumerable<SearchResult> Search([FromBody] Models.SearchBase searchQuery)
        {
            return SearchBaseMethod(searchQuery, (controller, controllerContext, query) =>
            {
                if (searchQuery.SearchCriteria.Any(x => x.Name.Equals("BaseChannelID")))
                {

                    int baseChannelID = (int)GetConvertedQueryValue(query, "BaseChannelID", typeof(int));

                    return ECN_Framework_BusinessLayer.Accounts.Customer.GetByBaseChannelID(baseChannelID);
                }

                throw new NotImplementedException();
            });
        }

        #endregion Search
        #region data cleansing
        #endregion data cleansing
    }
}
