using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;

// Framework components
using ECN_Framework_Entities.Accounts; //User
using EmailMarketing.API.Search;
using FrameworkModel = ECN_Framework_Entities.Communicator.Filter;
using APIModel = EmailMarketing.API.Models.Filter;
// local components
using EmailMarketing.API;
using EmailMarketing.API.Attributes;
using EmailMarketing.API.Models.Utility;
using EmailMarketing.API.ExtentionMethods;
// debug
using System.Diagnostics;

namespace EmailMarketing.API.Controllers
{
    // aliasing inside namespace where the declarations will then be identical in each controller
    using SearchQuery = List<Models.SearchProperty>;
    using SearchResult = Models.SearchResult<APIModel>;

    /// <summary>
    /// API methods exposing the Email Marketing Filter object model as Resources for Create, Read, 
    /// Update and Delete via REST.  
    /// </summary>
    /// <remarks>For more information on REST, try this 
    /// <a href="http://en.wikipedia.org/wiki/Representational_state_transfer">Wikipedia article</a>.</remarks>
    public class FilterController : SearchableApiControllerBase<APIModel,FrameworkModel>
    {
        #region abstract member implementations

        /// <inheritdoc/>
        public override ECN_Framework_Common.Objects.Enums.Entity FrameworkEntity
        {
            get { return ECN_Framework_Common.Objects.Enums.Entity.Filter; }
        }

        override public string ControllerName { get { return "filter"; } }
        override public string[] ExposedProperties
        {
            get { return new string[] { "FilterID", "FilterName", "GroupID", "Archived" }; }
        }

        public override object GetID(APIModel model)
        {
            return model.FilterID;
        }

        public override object GetID(FrameworkModel model)
        {
            return model.FilterID;
        }

        #endregion abstract member implementations
        #region REST

        /// <summary>
        /// This feature is not implemented
        /// </summary>
        public APIModel Get(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This feature is not implemented
        /// </summary>
        public HttpResponseMessage Post(APIModel model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This feature is not implemented
        /// </summary>
        public HttpResponseMessage Put(int id, [FromBody]APIModel apiModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This feature is not implemented
        /// </summary>
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        #endregion REST        
        #region Search
        /// <summary>
        /// Provides search capabilities for Filter resources. Search supports both GET and POST methods; results will be identical.
        /// </summary>
        /// <see cref="EmailMarketing.API.Models.Filter"/>
        /// <param name="searchQuery">used to constrain the search</param>
        /// <returns>a list of matching <see cref="EmailMarketing.API.Models.Filter"/> as Location/API Object pairs.</returns>
        /// 
        /// <example for="request"><![CDATA[
        ///  GET http://api.ecn5.com/api/search/filter HTTP/1.1
        ///  Content-Type: application/json; charset=utf-8
        ///  Accept: application/json
        ///  APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        ///  X-Customer-ID: 99999
        ///  Host: api.ecn5.com
        ///  Content-Length: 422
        ///  
        ///  {
        ///     "SearchCriteria": [
        ///     {
        ///         "Name": "GroupID",
        ///         "Comparator": "=",
        ///         "ValueSet": "123"
        ///     },
        ///     {   
        ///         "Name": "Archived",
        ///         "Comparator": "=",
        ///         "ValueSet": "false"
        ///     }
        ///     ]
        /// }    
        /// ]]></example>
        /// 
        /// <example for="response"><![CDATA[
        ///   HTTP/1.1 200 OK
        ///   Cache-Control: no-cache
        ///   Pragma: no-cache
        ///   Content-Type: application/json; charset=utf-8
        ///   Expires: -1
        ///   Server: Microsoft-IIS/8.0
        ///   X-AspNet-Version: 4.0.30319
        ///   X-Powered-By: ASP.NET
        ///   Date: Tue, 21 Apr 2015 14:27:56 GMT
        ///   Content-Length: 420
        ///   
        ///   [  
        ///     {  
        ///       "ApiObject":{  
        ///          "GroupID = 321417"
        ///          "Archived=False",
        ///          "FilterID":456789,
        ///          "FilterName":"My Filter"
        ///      },
        ///      "Location":"http://api.ecn5.com/api/filter/456789"
        ///     }
        ///   ]
        /// ]]></example>
        [Route("api/search/filter")]
        [HttpGet]
        [HttpPost]
        public List<Models.SearchResult<APIModel>> Search([FromBody] Models.SearchBase searchQuery)
        {
            if (null == searchQuery)
            {
                RaiseInvalidMessageException("Search parameter can't be empty");
            }

            return SearchBaseMethod(searchQuery, (query, controller, controllerContext) => {


                int? groupID = (int?)GetConvertedQueryValue(searchQuery.SearchCriteria, "GroupID", typeof(int));
                
                bool? archived = (bool?)GetConvertedQueryValue(searchQuery.SearchCriteria, "Archived", typeof(bool));
           
                return ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterSearch(APIUser,groupID, APICustomer.CustomerID, archived);
            
            });
        }

      
        #endregion Search
    }
}