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
using FrameworkModel = ECN_Framework_Entities.Communicator.Group;
using APIModel = EmailMarketing.API.Models.Group;
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
    /// API methods exposing the Email Marketing Group object model as Resources for Create, Read, 
    /// Update and Delete via REST.  
    /// </summary>
    [RoutePrefix("api/group")]
    public class GroupController : SearchableApiControllerBase<APIModel,FrameworkModel>
    {
        #region abstract member implementations

        /// <inheritdoc/>
        public override ECN_Framework_Common.Objects.Enums.Entity FrameworkEntity
        {
            get { return ECN_Framework_Common.Objects.Enums.Entity.Group; }
        }
        
        override public string ControllerName { get { return "group"; } }
        override public string[] ExposedProperties
        {
            get { return new string[] { "GroupID", "FolderID", "GroupName", "GroupDescription","Archived" }; }
        }

        public override object GetID(APIModel model)
        {
            return model.GroupID;
        }
        public override object GetID(FrameworkModel model)
        {
            return model.GroupID;
        }

        #endregion abstract member implementations
        #region REST
        #region GET
        /// <summary>
        /// Retrieves the Group resource identified by <pre>id</pre>
        /// </summary>
        /// <param name="id">GroupID for the target resource</param>
        /// <returns>On success, a Group API model object; otherwise,
        /// <note>In most cases returns Http Status Code <pre>200 OK</pre>; however, if the target 
        /// resource doesn't exist --for example, if it has been deleted-- the result is 
        /// <pre>Error 404 - Not Found.</pre></note> </returns>
        /// <example for="request"><![CDATA[
        /// GET http://api.ecn5.com/api/group/456789 HTTP/1.1
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
        ///          "GroupID":456789,
        ///          "FolderID":123,
        ///          "GroupName":"My Group",
        ///          "GroupDescription":"This is my group description"
        ///     }
        /// ]]></example>
        // GET api/group/<id-value>
        // GET /api/customfield/49195 \nAPIAccessKey: BF99E412-8CD7-487C-A1B3-E6AAF82E5EA3
        public APIModel Get(int id)
        {
            FrameworkModel frameworkObject =
                ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(id, APIUser);
            if (null == frameworkObject)
            {
                //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound).EnsureSuccessStatusCode());
                RaiseNotFoundException(id);
            }

            APIModel apiObject = Transform(frameworkObject);
            return apiObject;
        }
        #endregion GET
        #region POST
        /// <summary>
        /// Given a Group model object, add a new resource, assigning unique GroupID attribute (and REST Location 
        /// property).
        /// </summary>
        /// 
        /// <param name="model">Group model object</param>
        /// 
        /// <returns>On success, returns the given model object with <pre>GroupID</pre> filled in, 
        /// as well as a <pre>Location</pre> header.  In case of validation error(s), 
        /// Error <pre>400 - Bad Request</pre> is returned along with a message providing further 
        /// information.</returns>
        /// 
        /// <header for="request">APIAccessKey</header>
        /// <header for="response">Location</header>
        /// 
        /// <example for="request"><![CDATA[
        /// POST http://api.ecn5.com/api/group HTTP/1.1
        /// Content-Type: application/json; charset=utf-8
        /// APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        /// X-Customer-ID: 99999
        /// Host: api.ecn5.com
        /// Content-Length: 237
        /// 
        ///     {  
        ///          "FolderID":123,
        ///          "GroupName":"My Group",
        ///          "GroupDescription":"This is my group description"
        ///     }
        /// ]]></example>
        /// 
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 201 Created
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Content-Type: application/json; charset=utf-8
        /// Expires: -1
        /// Location: http://api.ecn5.com/api/group/456789
        /// Server: Microsoft-IIS/8.0
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Tue, 14 Apr 2015 16:43:57 GMT
        /// Content-Length: 259
        /// 
        ///     {  
        ///          "GroupID":456789,
        ///          "FolderID":123,
        ///          "GroupName":"My Group",
        ///          "GroupDescription":"This is my group description"
        ///     }
        /// ]]></example>
        // POST api/group
        public HttpResponseMessage Post(APIModel model)
        {
            if (model == null)
            {
                RaiseInvalidMessageException("no model in request body");
            }

            // 1. cleanse post data
            CleanseInputData(model);

            // 2. transform to internal model
            FrameworkModel frameworkModel = Transform(model);

            // 3. fill properties not exposed via API model
            POST_FillFrameworkModelInternalFields(frameworkModel);

            // 4. delegate to business layer for validation and persistence...
            // ... making sure to update the API model's ID
            var newId = ECN_Framework_BusinessLayer.Communicator.Group.Save(frameworkModel, APIUser);

            // 5. fetch the newly created object
            var newModel = Get(newId);

            // 6. explicitly create the HTTP response...
            // ...so we can install a Location header pointing to the created item
            return CreateResponseWithLocation(HttpStatusCode.Created, newModel, newId);
        }
        #endregion POST
        #region PUT
        /// <summary>
        /// This feature is not implemented
        /// </summary>
        public HttpResponseMessage Put(int id, [FromBody]APIModel apiModel)
        {
            throw new NotImplementedException();
        }
        #endregion PUT
        #region DELETE
        /// <summary>
        /// This feature is not implemented
        /// </summary>
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
        #endregion DELETE

        #endregion REST
        #region Search
        /// <summary>
        /// Provides search capabilities for Group resources. Search supports both GET and POST methods; results will be identical.
        /// </summary>
        /// <see cref="EmailMarketing.API.Models.Group"/>
        /// <param name="query">used to constrain the search.  You may provide either or both support criteria, or no parameters to get all.</param>
        /// <returns>a list of matching <see cref="EmailMarketing.API.Models.Group"/> as Location/API Object pairs.</returns>
        /// 
        /// <example for="request"><![CDATA[
        ///  POST http://api.ecn5.com/api/search/group HTTP/1.1
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
        ///         "Name": "Name",
        ///         "Comparator": "contains",
        ///         "ValueSet": "test"
        ///     },
        ///     {
        ///         "Name": "FolderID",
        ///         "Comparator": "=",
        ///         "ValueSet": "123"
        ///     },
        ///     {   
        ///         "Name": "Archived",
        ///         "Comparator": "=",
        ///         "ValueSet": "false"
        ///     }
        ///     ]
        ///  }
        ///  
        ///  OR pass no parameters to get all
        ///  
        ///  POST http://api.ecn5.com/api/search/group HTTP/1.1
        ///  Content-Type: application/json; charset=utf-8
        ///  Accept: application/json
        ///  APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        ///  X-Customer-ID: 99999
        ///  Host: api.ecn5.com
        ///  Content-Length: 0
        ///  
        /// {
        ///  "SearchCriteria": []
        /// }
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
        ///   X-Powered-By: ASP.NET
        ///   Date: Tue, 21 Apr 2015 14:27:56 GMT
        ///   Content-Length: 420
        ///   
        ///   [  
        ///     {  
        ///       "ApiObject":{  
        ///          "Archived=False",
        ///          "GroupID":456789,
        ///          "GroupName":"test",
        ///          "FolderID":456789,
        ///          "GroupDescription":"test description",
        ///      },
        ///      "Location":"http://api.ecn5.com/api/group/456789"
        ///     }
        ///   ]
        /// ]]></example>
        [Route("~/api/search/group")]
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
                string name = (string)GetConvertedQueryValue(query, "Name", typeof(string));
                int? folderID = (int?)GetConvertedQueryValue(query, "FolderID", typeof(int));
                bool? archived = (bool?)GetConvertedQueryValue(query, "Archived", typeof(bool));
                APIUser.CustomerID = APICustomer.CustomerID;
                return ECN_Framework_BusinessLayer.Communicator.Group.
                    GetByGroupSearch(name, folderID, APIUser, archived);
            });
        }

        #endregion Search
        #region Web Methods (none)
        #endregion Web Methods
        #region Data Cleansing

        /// <summary>
        /// Prepares the incoming APIModel from a POST for transformation to Framework model
        /// </summary>
        /// <param name="model">The API Model instance provided for insert via HTTP POST</param>
        private void CleanseInputData(APIModel apiModel)
        {
            // default the folder-ID
            if (null == apiModel.FolderID)
            {
                apiModel.FolderID = 0;
            }
        }

        private void POST_FillFrameworkModelInternalFields(FrameworkModel frameworkModel)
        {
            frameworkModel.CreatedUserID = APIUser.UserID;
            frameworkModel.OwnerTypeCode = "customer";
            frameworkModel.PublicFolder = 0;
            frameworkModel.CustomerID = APICustomer.CustomerID;
            frameworkModel.AllowUDFHistory = "N";
            frameworkModel.IsSeedList = false;
        }

        #endregion Data Cleansing
    }
}