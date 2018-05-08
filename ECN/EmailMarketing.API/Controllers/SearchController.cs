using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using FrameworkModel=ECN_Framework_Entities.Communicator;
using ApiModel = EmailMarketing.API.Models;

namespace EmailMarketing.API.Controllers
{

    /// <summary>
    /// The EmailMarketing Search API
    /// </summary>
    public class SearchController : AuthenticatedUserControllerBase
    {
        #region abstract member implementations

        override public string ControllerName { get { return "search"; } }

        #endregion abstract member implementations

        #region Filter Search
        /// <summary>
        /// Provides search capabilities for Filter resources
        /// </summary>
        /// <see cref="EmailMarketing.API.Models.Filter"/>
        /// <param name="query">used to constrain the search</param>
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
        ///  [
        ///    {  
        ///      "name": "GroupID", 
        ///      "comparator": "=", 
        ///      "valueSet": [ "123" ] 
        ///    }
        ///  ]
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
        ///   X-SourceFiles: =?UTF-8?B?QzpcUHJvamVjdHNcRUNOXERldlwyMDE1X1EyXEVtYWlsTWFya2V0aW5nLkFQSVxhcGlcc2VhcmNoXGNvbnRlbnQ=?=
        ///   X-Powered-By: ASP.NET
        ///   Date: Tue, 21 Apr 2015 14:27:56 GMT
        ///   Content-Length: 420
        ///   
        ///   [  
        ///     {  
        ///       "ApiObject":{  
        ///          "FilterID":456789,
        ///          "FilterName":"My Filter"
        ///      },
        ///      "Location":"http://api.ecn5.com/api/filter/456789"
        ///     }
        ///   ]
        /// ]]></example>
        /*[Route("api/search/filter")]
        [HttpGet]
        public List<Models.SearchResult<Models.Filter>> Filter([FromBody] List<Models.SearchProperty> query)
        {
            var config = Search.SearchConfiguration.Library["filter"];
            var exposedProperties = ExposedProperties;
            EnsureValidSearchQuery(config, query);

            List<Models.Filter> results = new List<Models.Filter>();

            int groupID = (int)GetConvertedQueryValue(query, "GroupID", typeof(int));

            ECN_Framework_BusinessLayer.Communicator.Filter.
                GetByGroupID(groupID, true, APIUser).
                    ForEach((x) =>
                        results.Add(ApiModel.Utility.Transformer<FrameworkModel.Filter, ApiModel.Filter>.Transform(x, exposedProperties)));

            return new List<ApiModel.SearchResult<ApiModel.Filter>>(
                from x in results
                select (new ApiModel.SearchResult<ApiModel.Filter>()
                {
                    ApiObject = x,
                    Location = Url.Link(Strings.Routing.DefaultApiRouteName, new { id = x.FilterID })
                }));
        }*/

        #endregion Filter Search

        #region Custom Field Search
        /// <summary>
        /// Provides search capabilities for CustomField resources
        /// </summary>
        /// <see cref="EmailMarketing.API.Models.CustomField"/>
        /// <param name="query">used to constrain the search</param>
        /// <returns>a list of matching <see cref="EmailMarketing.API.Models.CustomField"/> as Location/API Object pairs.</returns>
        /// 
        /// <example for="request"><![CDATA[
        ///  GET http://api.ecn5.com/api/search/customfield HTTP/1.1
        ///  Content-Type: application/json; charset=utf-8
        ///  Accept: application/json
        ///  APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        ///  X-Customer-ID: 99999
        ///  Host: api.ecn5.com
        ///  Content-Length: 422
        ///  
        ///  [
        ///    {  
        ///      "name": "GroupID", 
        ///      "comparator": "=", 
        ///      "valueSet": [ "123" ] 
        ///    }
        ///  ]
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
        ///   X-SourceFiles: =?UTF-8?B?QzpcUHJvamVjdHNcRUNOXERldlwyMDE1X1EyXEVtYWlsTWFya2V0aW5nLkFQSVxhcGlcc2VhcmNoXGNvbnRlbnQ=?=
        ///   X-Powered-By: ASP.NET
        ///   Date: Tue, 21 Apr 2015 14:27:56 GMT
        ///   Content-Length: 420
        ///   
        ///   [  
        ///     {  
        ///       "ApiObject":{  
        ///          "GroupDataFieldsID":456789,
        ///          "GroupID":123,
        ///          "ShortName":"MyField",
        ///          "LongName":"This is my custom field description",
        ///          "IsPublic":"Y"
        ///      },
        ///      "Location":"http://api.ecn5.com/api/customfield/456789"
        ///     }
        ///   ]
        /// ]]></example>
        /* [Route("api/search/customfield")]
        [HttpGet]
        public List<Models.SearchResult<Models.CustomField>> CustomField([FromBody] List<Models.SearchProperty> query)
        {
            var config = Search.SearchConfiguration.Library["customfield"];
            var exposedProperties = ExposedProperties;
            EnsureValidSearchQuery(config, query);

            List<Models.CustomField> results = new List<Models.CustomField>();

            int groupID = (int)GetConvertedQueryValue(query, "GroupID", typeof(int));

            ECN_Framework_BusinessLayer.Communicator.GroupDataFields.
                GetByGroupID(groupID, APIUser).
                    ForEach((x) =>
                        results.Add(ApiModel.Utility.Transformer<FrameworkModel.GroupDataFields, ApiModel.CustomField>.Transform(x, exposedProperties)));

            return new List<ApiModel.SearchResult<ApiModel.CustomField>>(
                from x in results
                select (new ApiModel.SearchResult<ApiModel.CustomField>()
                {
                    ApiObject = x,
                    Location = Url.Link(Strings.Routing.DefaultApiRouteName, new { id = x.GroupDataFieldsID })
                }));
        }*/

        #endregion Custom Field Search

        #region Group Search
        /// <summary>
        /// Provides search capabilities for Group resources
        /// </summary>
        /// <see cref="EmailMarketing.API.Models.Group"/>
        /// <param name="query">used to constrain the search(or no parameters to get all)</param>
        /// <returns>a list of matching <see cref="EmailMarketing.API.Models.Group"/> as Location/API Object pairs.</returns>
        /// 
        /// <example for="request"><![CDATA[
        ///  GET http://api.ecn5.com/api/search/group HTTP/1.1
        ///  Content-Type: application/json; charset=utf-8
        ///  Accept: application/json
        ///  APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        ///  X-Customer-ID: 99999
        ///  Host: api.ecn5.com
        ///  Content-Length: 422
        ///  
        ///  [
        ///    {  
        ///      "name": "Name", 
        ///      "comparator": "contains", 
        ///      "valueSet": [ "test" ] 
        ///    },
        ///    {  
        ///      "name": "FolderID", 
        ///      "comparator": "=", 
        ///      "valueSet": [ "123" ] 
        ///    }
        ///  ]
        ///  
        ///  OR pass no parameters to get all
        ///  
        ///  GET http://api.ecn5.com/api/search/group HTTP/1.1
        ///  Content-Type: application/json; charset=utf-8
        ///  Accept: application/json
        ///  APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        ///  Host: api.ecn5.com
        ///  Content-Length: 0
        ///  
        ///  [
        ///    
        ///  ]
        /// 
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
        ///   X-SourceFiles: =?UTF-8?B?QzpcUHJvamVjdHNcRUNOXERldlwyMDE1X1EyXEVtYWlsTWFya2V0aW5nLkFQSVxhcGlcc2VhcmNoXGNvbnRlbnQ=?=
        ///   X-Powered-By: ASP.NET
        ///   Date: Tue, 21 Apr 2015 14:27:56 GMT
        ///   Content-Length: 420
        ///   
        ///   [  
        ///     {  
        ///       "ApiObject":{  
        ///          "GroupID":456789,
        ///          "GroupName":"test",
        ///          "FolderID":456789,
        ///          "GroupDescription":"test description",
        ///      },
        ///      "Location":"http://api.ecn5.com/api/group/456789"
        ///     }
        ///   ]
        /// ]]></example>
        /* [Route("api/search/group")]
        [HttpGet]
        public List<Models.SearchResult<Models.Group>> Group([FromBody] List<Models.SearchProperty> query)
        {
            var config = Search.SearchConfiguration.Library["group"];
            var exposedProperties = ExposedProperties;
            EnsureValidSearchQuery(config, query);

            List<Models.Group> results = new List<Models.Group>();

            string name = (string)GetConvertedQueryValue(query, "Name", typeof(string));
            int? folderID = (int?)GetConvertedQueryValue(query, "FolderID", typeof(int));

            ECN_Framework_BusinessLayer.Communicator.Group.
                GetByGroupSearch(name, folderID, APIUser).
                    ForEach((x) =>
                        results.Add(ApiModel.Utility.Transformer<FrameworkModel.Group, ApiModel.Group>.Transform(x, exposedProperties)));

            return new List<ApiModel.SearchResult<ApiModel.Group>>(
                from x in results
                select (new ApiModel.SearchResult<ApiModel.Group>()
                {
                    ApiObject = x,
                    Location = Url.Link(Strings.Routing.DefaultApiRouteName, new { id = x.GroupID })
                }));
        }

        #endregion Group Search
        #region Folder Search

        /// <summary>
        /// This feature is not implemented
        /// </summary>
        /// 
        /// <example for="request"><![CDATA[]]></example>
        /// <example for="response"><![CDATA[]]></example>
        [Route("api/search/folder")]
        [HttpGet]
        public void Folder([FromBody] List<Models.SearchProperty> query)
        {
            throw new NotImplementedException();
        }
        */
        #endregion Folder Search
        #region Customer Search

        /// <summary>
        /// Provide search capabilities for Customer objects
        /// </summary>
        /// <param name="query">used to constrain the search</param>
        /// <returns>a list of matching <see cref="EmailMarketing.API.Models.Customer"/> as Location/API Object pairs.</returns>
        /// <example for="request"><![CDATA[
        /// GET http://api.ecn5.com/api/search/customer HTTP/1.1
        /// Accept: application/json
        /// Accept-Language: en-US
        /// User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko
        /// Accept-Encoding: gzip, deflate
        /// Connection: Keep-Alive
        /// APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        /// X-Customer-ID: 99999
        /// Host: api.ecn5.com
        /// Content-Type: application/json; charset=utf-8
        /// Content-Length: 88
        /// 
        /// [
        ///  {  
        /// "name": "BaseChannelID", 
        /// "comparator": "=", 
        /// "valueSet": [ 123 ] 
        ///  },
        /// ]
        /// ]]></example>
        /// <example for="response"><![CDATA[]]></example>
        /*
        [Route("api/search/customer")]
        [HttpGet]
        public IEnumerable<ApiModel.SearchResult<ApiModel.Customer>> Customer([FromBody] List<ApiModel.SearchProperty> query)
        {
            var config = Search.SearchConfiguration.Library["customer"];
            var exposedProperties = ExposedProperties;
            EnsureValidSearchQuery(config, query);

            List<Models.Customer> results = new List<Models.Customer>();

            if (query.Any(x => x.Name.Equals("BaseChannelID")))
            {
                int baseChannelID = (int)GetConvertedQueryValue(query, "BaseChannelID", typeof(int));

                ECN_Framework_BusinessLayer.Accounts.Customer.
                    GetByBaseChannelID(baseChannelID).
                        ForEach((x) =>
                            results.Add(ApiModel.Utility.Transformer<ECN_Framework_Entities.Accounts.Customer, ApiModel.Customer>.
                                Transform(x, exposedProperties)));
            }

            return new List<ApiModel.SearchResult<ApiModel.Customer>>(
                from x in results
                select (new ApiModel.SearchResult<ApiModel.Customer>()
                {
                    ApiObject = x,
                    Location = Url.Link(Strings.Routing.DefaultApiRouteName, new { id = x.CustomerID, controller = "customer" })
                }));
        }
        */
        #endregion Customer Search
        #region internals

        /// <summary>
        /// Validates the supplied search property list for the request against search configuration from the controller.
        /// </summary>
        /// <param name="query">specifications for a search</param>
        /// <exception cref="ECN_Framework_Common.Objects.ECNException">Exception raised in the event of an invalid query</exception>
        internal static void EnsureValidSearchQuery(Search.SearchConfigurationGroup searchConfiguration, List<Models.SearchProperty> query)
        {
            List<ECN_Framework_Common.Objects.ECNError> errors = new List<ECN_Framework_Common.Objects.ECNError>();
            Action<string> addError = (s) =>
                errors.Add(new ECN_Framework_Common.Objects.ECNError()
                {
                    Entity = searchConfiguration.ExceptionEntity,
                    Method = searchConfiguration.ExceptionMethod,
                    ErrorMessage = s
                });
            Action raiseException = () => 
            {
                throw new ECN_Framework_Common.Objects.ECNException(errors, ECN_Framework_Common.Objects.Enums.ExceptionLayer.API);
            };

            if(false == searchConfiguration.AllowEmptySearch)
            {
                if(null == query || 1 > query.Count)
                {
                    addError("search must be constrained; did you forget to supply a query?");
                    raiseException();
                }
            }

            // check for duplicate constraints on the same property
            // ZZZ: roll this into SearchConfiguration.cs as a validation method to enable us to allow things like:
            //      FieldName like '%foo%' and FieldName != 'foobar'
            foreach (var searchProperty in query)
            {
                if (query.Where((x) => x.Name == searchProperty.Name).Count() > 1)
                {
                    addError(String.Format(@"duplicate constraint on property ""{0}""", searchProperty.Name));
                }
            }

            // check properties using configured validation
            foreach (var searchProperty in query)
            {
                if (false == searchConfiguration.ContainsKey(searchProperty.Name))
                {
                    addError(String.Format(@"cannot constrain search on property ""{0}""", searchProperty.Name));
                }
                else
                {
                    var config = searchConfiguration[searchProperty.Name];
                    if (false == config.ValidationMethod(searchConfiguration, query, config, searchProperty))
                    {
                        addError(String.Format(@"invalid search on ""{0}""", searchProperty.Name));
                    }
                }
            }

            if (errors.Count > 0)
            {
                raiseException();
            }
        }
        #endregion internals
    }
}
