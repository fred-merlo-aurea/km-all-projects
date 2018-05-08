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
using FrameworkModel = ECN_Framework_Entities.Communicator.GroupDataFields;
using APIModel = EmailMarketing.API.Models.CustomField;
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
    /// API methods exposing the Email Marketing CustomField object model as Resources for Read, Create, and Search.
    /// </summary>
    /// <remarks>For more information on REST, try this 
    /// <a href="http://en.wikipedia.org/wiki/Representational_state_transfer">Wikipedia article</a>.</remarks>
    public class CustomFieldController : SearchableApiControllerBase<APIModel,FrameworkModel>
    {
        #region abstract member implementations

        /// <inheritdoc/>
        public override ECN_Framework_Common.Objects.Enums.Entity FrameworkEntity
        {
            get { return ECN_Framework_Common.Objects.Enums.Entity.GroupDataFields; }
        }

        override public string[] ExposedProperties
        {
            get { return new string[] { "GroupDataFieldsID", "GroupID", "ShortName", "LongName", "IsPublic" }; }
        }
        override public string ControllerName { get { return "customfield"; } }

        public override object GetID(APIModel model)
        {
            return model.GroupDataFieldsID;
        }

        public override object GetID(FrameworkModel model)
        {
            return model.GroupDataFieldsID;
        }

        #endregion abstract member implementations
        #region REST

        /// <summary>
        /// Retrieves the Custom Field resource identified by <pre>id</pre>
        /// </summary>
        /// <param name="id">GroupDataFieldID for the target resource</param>
        /// <returns>On success, a CustomField API model object; otherwise,
        /// <note>In most cases returns Http Status Code <pre>200 OK</pre>; however, if the target 
        /// resource doesn't exist --for example, if it has been deleted-- the result is 
        /// <pre>Error 404 - Not Found.</pre></note> </returns>
        /// <example for="request"><![CDATA[
        /// GET http://api.ecn5.com/api/customfield/456789 HTTP/1.1
        /// Accept: application/xml
        /// Accept-Language: en-US
        /// User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko
        /// Accept-Encoding: gzip, deflate
        /// Connection: Keep-Alive
        /// APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        /// X-Customer-ID: 99999
        /// Host: api.ecn5.com
        /// 
        /// ]]></example>
        /// 
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 200 OK
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Content-Type: application/json; charset=utf-8
        /// Expires: -1
        /// Server: Microsoft-IIS/8.0
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Fri, 10 Apr 2015 19:27:41 GMT
        /// Content-Length: 259
        /// 
        ///     {  
        ///          "GroupDataFieldsID":456789,
        ///          "GroupID":123,
        ///          "ShortName":"MyField",
        ///          "LongName":"This is my custom field description",
        ///          "IsPublic":"Y"
        ///     }
        /// ]]></example>
        // GET api/customfield/<id-value>
        // GET /api/customfield/254723 \nAPIAccessKey: BF99E412-8CD7-487C-A1B3-E6AAF82E5EA3
        public APIModel Get(int id)
        {
            FrameworkModel frameworkObject = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByID(id, APIUser);
            if (null == frameworkObject)
            {
                //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound).EnsureSuccessStatusCode());
                RaiseNotFoundException(id);
            }

            APIModel apiObject = Transform(frameworkObject);
            return apiObject;
        }

        /// <summary>
        /// Given a CustomField model object, add a new resource, assigning unique GroupDataFieldsID attribute (and REST Location 
        /// property).
        /// </summary>
        /// 
        /// <param name="model">CustomField model object</param>
        /// 
        /// <returns>On success, returns the given model object with <pre>GroupDataFieldsID</pre> filled in, 
        /// as well as a <pre>Location</pre> header.  In case of validation error(s), 
        /// Error <pre>400 - Bad Request</pre> is returned along with a message providing further 
        /// information.</returns>
        /// 
        /// <header for="request">APIAccessKey</header>
        /// <header for="response">Location</header>
        /// 
        /// <example for="request"><![CDATA[
        /// POST http://api.ecn5.com/api/customfield HTTP/1.1
        /// Content-Type: application/json; charset=utf-8
        /// APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        /// X-Customer-ID: 99999
        /// Host: api.ecn5.com
        /// Content-Length: 237
        /// 
        ///     {  
        ///          "GroupID":123,
        ///          "ShortName":"MyField",
        ///          "LongName":"This is my custom field description",
        ///          "IsPublic":"Y"
        ///     }
        /// ]]></example>
        /// 
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 201 Created
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Content-Type: application/json; charset=utf-8
        /// Expires: -1
        /// Location: http://api.ecn5.com/api/customfield/456789
        /// Server: Microsoft-IIS/8.0
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Tue, 14 Apr 2015 16:43:57 GMT
        /// Content-Length: 259
        /// 
        ///     {  
        ///          "GroupDataFieldsID":456789,
        ///          "GroupID":123,
        ///          "ShortName":"MyField",
        ///          "LongName":"This is my custom field description",
        ///          "IsPublic":"Y"
        ///     }
        /// ]]></example>
        // POST api/customfield
        public HttpResponseMessage Post(APIModel model)
        {
            if (model == null)
            {
                RaiseInvalidMessageException("no model in request body");
            }

            // 1. cleanse post data
            //CleanseInputData(model);

            // 2. transform to internal model
            FrameworkModel frameworkModel = Transform(model);

            // 3. fill properties not exposed via API model
            POST_FillFrameworkModelInternalFields(frameworkModel);

            // 4. delegate to business layer for validation and persistence...
            // ... making sure to update the API model's ID
            var newId = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Save(frameworkModel, APIUser);

            // 5. fetch the newly created object
            var newModel = Get(newId);

            // 6. explicitly create the HTTP response...
            // ...so we can install a Location header pointing to the created item
            return CreateResponseWithLocation(HttpStatusCode.Created, newModel, newId);
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
        /// Provides search capabilities for CustomField resources.  Search supports both GET and POST methods; results will be identical.
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
        ///  {
        ///     "SearchCriteria": [
        ///     {
        ///         "Name": "GroupID",
        ///         "Comparator": "=",
        ///         "ValueSet": "123"  
        ///     }
        ///     ]
        ///  }
        ///  ]]></example>
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
        [Route("api/search/customfield")]
        [HttpGet]
        [HttpPost]
        public List<SearchResult> Search([FromBody] Models.SearchBase searchQuery)
        {
            if (null == searchQuery)
            {
                RaiseInvalidMessageException("Search parameter can't be empty");
            }
            return SearchBaseMethod(searchQuery, (controller, controllerContext, query) =>
            {
                int groupID = (int)GetConvertedQueryValue(query, "GroupID", typeof(int));
                try
                {
                    return ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByGroupID(groupID, APIUser);
                }
                catch (ECN_Framework_Common.Objects.SecurityException se)
                {
                    return new List<ECN_Framework_Entities.Communicator.GroupDataFields>();
                }
                
            });
        }

        #endregion Search
        #region Data Cleansing

        private void POST_FillFrameworkModelInternalFields(FrameworkModel frameworkModel)
        {
            frameworkModel.CreatedUserID = APIUser.UserID;
            frameworkModel.CustomerID = APICustomer.CustomerID;
        }

        #endregion Data Cleansing
    }
}