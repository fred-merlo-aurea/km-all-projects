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
// local components
using EmailMarketing.API;
using EmailMarketing.API.Attributes;
using EmailMarketing.API.Models.Utility;
using EmailMarketing.API.ExtentionMethods;
// debug
using System.Diagnostics;

// alias the Framework and API models to standardize coding between controllers
using FrameworkModel = ECN_Framework_Entities.Communicator.Content;
using APIModel = EmailMarketing.API.Models.Content;



namespace EmailMarketing.API.Controllers
{
    // aliasing inside namespace where the declarations will then be identical in each controller
    using SearchQuery = List<Models.SearchProperty>;
    using SearchResult = Models.SearchResult<APIModel>;
    using AccessKeyType = Infrastructure.Authentication.AuthenticationProvider.Settings.AccessKeyType;

    /// <summary>
    /// API methods exposing the Email Marketing Content object model as Resources for Create, Read, 
    /// Update and Delete via REST.  
    /// </summary>
    /// <remarks>For more information on REST, try this 
    /// <a href="http://en.wikipedia.org/wiki/Representational_state_transfer">Wikipedia article</a>.</remarks>
    
    // example of overriding authentication:
    //[AuthenticationRequired(AccessKey: AccessKeyType.Customer | AccessKeyType.BaseChannel, RequiredCustomerId: false)]
    public class ContentController : SearchableApiControllerBase<APIModel,FrameworkModel>
    {
        #region abstract member implementations

        /// <inheritdoc/>
        public override ECN_Framework_Common.Objects.Enums.Entity FrameworkEntity
        {
            get { return ECN_Framework_Common.Objects.Enums.Entity.Content; }
        }

        /// <summary>
        /// Lists common properties between the (external) API model and the associated (internal) framework model by this service.
        /// </summary>
        override public string[] ExposedProperties
        {
            get { return new string[] { "ContentID", "FolderID", "ContentSource", "ContentText", "UseWYSIWYGeditor", "ContentTitle", "UpdatedDate", "Archived" }; }
        }

        override public string ControllerName { get { return "content"; } }

        [NonAction]
        public override object GetID(APIModel model)
        {
            return model.ContentID;
        }

        [NonAction]
        public override object GetID(FrameworkModel model)
        {
            return model.ContentID;
        }

        internal FrameworkModel GetInternal(int id)
        {
            return ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID(id, APIUser, false);
        }

        #endregion abstract member implementations
        #region REST

        /// <summary>
        /// Retrieves the Content resource identified by <pre>id</pre>
        /// </summary>
        /// <param name="id">ContentID for the target resource</param>
        /// <returns>On success, a Content API model object; otherwise,
        /// <note>In most cases returns Http Status Code <pre>200 OK</pre>; however, if the target 
        /// resource doesn't exist --for example, if it has been deleted-- the result is 
        /// <pre>Error 404 - Not Found.</pre></note> </returns>
        /// <example for="request"><![CDATA[
        /// GET http://api.ecn5.com/api/content/456789 HTTP/1.1
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
        ///          "ContentID":456789,
        ///          "FolderID":123,
        ///          "ContentSource":"<html><head><title>Email Title</title></head><body>Email HTML content here...</body></html>",
        ///          "ContentText":"Email text content here....",
        ///          "ContentTitle":"test bull",
        ///          "UpdatedDate":"2015-08-14"
        ///     }
        /// ]]></example>
        // GET api/content/<id-value>
        // GET /api/content/293229 \nAPIAccessKey: BF99E412-8CD7-487C-A1B3-E6AAF82E5EA3
        [HttpGet]
        public APIModel Get(int id)
        {
            FrameworkModel frameworkObject = GetInternal(id);
            if(null == frameworkObject)
            {
                //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound).EnsureSuccessStatusCode());
                RaiseNotFoundException(id);
            }

            APIModel apiObject = Transform(frameworkObject);
            return apiObject;
        }

        /// <summary>
        /// Given a Content model object, add a new resource, assigning unique ContentID attribute (and REST Location 
        /// property).
        /// </summary>
        /// 
        /// <param name="model">Content model object</param>
        /// 
        /// <returns>On success, returns the given model object with <pre>ContentID</pre> filled in, 
        /// as well as a <pre>Location</pre> header.  In case of validation error(s), 
        /// Error <pre>400 - Bad Request</pre> is returned along with a message providing further 
        /// information.</returns>
        /// 
        /// <header for="request">APIAccessKey</header>
        /// <header for="response">Location</header>
        /// 
        /// <example for="request"><![CDATA[
        /// POST http://api.ecn5.com/api/content HTTP/1.1
        /// Content-Type: application/json; charset=utf-8
        /// APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        /// X-Customer-ID: 99999
        /// Host: api.ecn5.com
        /// Content-Length: 237
        /// 
        ///     {  
        ///          "FolderID":123,
        ///          "ContentSource":"<html><head><title>Email Title</title></head><body>Email HTML content here...</body></html>",
        ///          "ContentText":"Email text content here....",
        ///          "ContentTitle":"test bull",
        ///     }
        /// ]]></example>
        /// 
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 201 Created
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Content-Type: application/json; charset=utf-8
        /// Expires: -1
        /// Location: http://api.ecn5.com/api/content/456789
        /// Server: Microsoft-IIS/8.0
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Tue, 14 Apr 2015 16:43:57 GMT
        /// Content-Length: 259
        /// 
        ///     {  
        ///          "ContentID":456789,
        ///          "FolderID":123,
        ///          "ContentSource":"<html><head><title>Email Title</title></head><body>Email HTML content here...</body></html>",
        ///          "ContentText":"Email text content here....",
        ///          "ContentTitle":"test bull",
        ///          "UpdatedDate":"2015-08-14"
        ///     }
        /// ]]></example>
        // POST api/content
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
            ECN_Framework_BusinessLayer.Communicator.Content.Validate(frameworkModel, APIUser);
            bool bReturn = false;
            bReturn = ECN_Framework_BusinessLayer.Communicator.Content.ValidateHTMLContent(frameworkModel.ContentSource);
            ECN_Framework_BusinessLayer.Communicator.Content.ReadyContent(frameworkModel, false == frameworkModel.UseWYSIWYGeditor);
            // ... making sure to update the API model's ID
            if (bReturn)
                frameworkModel.IsValidated = bReturn;
            var newId = ECN_Framework_BusinessLayer.Communicator.Content.Save(frameworkModel,APIUser);

            // 5. fetch the newly created object
            APIModel newModel = Transform(GetInternal(newId));

            // 6. explicitly create the HTTP response...
            // ...so we can install a Location header pointing to the created item
            return CreateResponseWithLocation(HttpStatusCode.Created, newModel, newId);
        }

        /// <summary>
        /// Updates resource identified by <pre>id</pre> with the given model object
        /// </summary>
        /// <param name="id">ContentID for the target resource</param>
        /// <param name="apiModel">Content model object</param>
        /// <returns>On success, returns the given model object as well as a <pre>Location</pre> header.  
        /// In case of validation error(s), 
        /// Error <pre>400 - Bad Rquest</pre> is returned along with a message providing further 
        /// information.</returns>
        /// <example for="request"><![CDATA[
        /// PUT http://api.ecn5.com/api/content/456789 HTTP/1.1
        /// Content-Type: application/json; charset=utf-8
        /// APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        /// X-Customer-ID: 99999
        /// Host: api.ecn5.com
        /// Content-Length: 259
        /// 
        ///     {  
        ///          "ContentID":456789,
        ///          "FolderID":123,
        ///          "ContentSource":"<html><head><title>Email Title</title></head><body>Email HTML content here...</body></html>",
        ///          "ContentText":"Email text content here....",
        ///          "ContentTitle":"new title"
        ///     }
        /// ]]></example>
        /// 
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 200 OK
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Content-Type: application/json; charset=utf-8
        /// Expires: -1
        /// Location: http://api.ecn5.com/api/content/456789
        /// Server: Microsoft-IIS/8.0
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Tue, 14 Apr 2015 16:43:57 GMT
        /// Content-Length: 259
        /// 
        ///     {  
        ///          "ContentID":456789,
        ///          "FolderID":123,
        ///          "ContentSource":"<html><head><title>Email Title</title></head><body>Email HTML content here...</body></html>",
        ///          "ContentText":"Email text content here....",
        ///          "ContentTitle":"new title",
        ///          "UpdatedDate":"2015-08-14"
        ///     }
        /// ]]></example>
        // PUT api/content/5
        public HttpResponseMessage Put(int id, [FromBody]APIModel apiModel)
        {

            if(apiModel == null)
            {
                RaiseInvalidMessageException("no model in request body");
            }

            if(apiModel == null)
            {
                RaiseInvalidMessageException("no model in request body");
            }

            // 1. cleanse input data
            CleanseInputData(apiModel);

            // 2. GET subject  
            FrameworkModel frameworkModel =
                // ZZZ: this argues against our internal/external model methodology; if we used public/internal 
                //      attributes here this would be an excellent opportunity to code against our own API
                    ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID(id, APIUser, false);
            if(null == frameworkModel)
            {
                RaiseNotFoundException(id);
            }

            // UseWYSIWYGeditor is ignored
            apiModel.UseWYSIWYGeditor = frameworkModel.UseWYSIWYGeditor.Value;

            //Archived is updated if different from saved content

            if (apiModel.Archived==null)
            {
                apiModel.Archived = frameworkModel.Archived;
            }
            // If UseWYSIWYGeditor is false, don't strip comments
            bool keepComments = false == frameworkModel.UseWYSIWYGeditor;


            // 3. property-wise copy from API Model to subject's current Framework Model
            Transform(apiModel, frameworkModel);

            // 4. fill/update fields existent only for the internal model
            PUT_FillFramworkModelInternalFields(frameworkModel);

            // 5. delegate to Framework for validation and persistence
            ECN_Framework_BusinessLayer.Communicator.Content.Validate(frameworkModel, APIUser);
            bool bReturn = false;
            bReturn=ECN_Framework_BusinessLayer.Communicator.Content.ValidateHTMLContent(frameworkModel.ContentSource);
            ECN_Framework_BusinessLayer.Communicator.Content.ReadyContent(frameworkModel, keepComments);
            if (bReturn)
                frameworkModel.IsValidated = bReturn;
            ECN_Framework_BusinessLayer.Communicator.Content.Save(frameworkModel,APIUser);

            // 6. fetch the updated object
            APIModel newModel = Transform(GetInternal(id));

            return CreateResponseWithLocation(HttpStatusCode.OK, newModel, id);
        }

        /// <summary>
        /// Removes a Content resource.
        /// </summary>
        /// <param name="id">ContentID of the target resource</param>
        /// <example for="request"><![CDATA[
        /// DELETE http://api.ecn5.com/api/content/456789 HTTP/1.1
        /// Accept: application/xml
        /// APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        /// X-Customer-ID: 99999
        /// Host: api.ecn5.com
        /// ]]></example>
        /// 
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 204 No Content
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Expires: -1
        /// Server: Microsoft-IIS/8.0
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Tue, 14 Apr 2015 18:52:35 GMT
        /// ]]></example>
        // DELETE api/content/5
        public void Delete(int id)
        {
            ECN_Framework_BusinessLayer.Communicator.Content.Delete(id, APIUser);
        }

        #endregion REST
        #region Search

        /// <summary>
        /// Provides search capabilities for Content resources. Search supports both GET and POST methods; results will be identical.
        /// </summary>
        /// <see cref="EmailMarketing.API.Models.Content"/>
        /// <param name="searchQuery">used to constrain the search</param>
        /// <returns>a list of matching <see cref="EmailMarketing.API.Models.Content"/> as Location/API Object pairs.</returns>
        /// 
        /// <example for="request"><![CDATA[
        ///  GET http://api.ecn5.com/api/search/content HTTP/1.1
        ///  Content-Type: application/json; charset=utf-8
        ///  Accept: application/json
        ///  APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        ///  X-Customer-ID: 99999
        ///  Host: api.ecn5.com
        ///  Content-Length: 422
        ///  
        ///{
        ///     "SearchCriteria": [
        ///     {
        ///         "Name": "Title",
        ///         "Comparator": "contains",
        ///         "ValueSet": "test"
        ///     },
        ///     {   
        ///         "Name": "FolderID",
        ///         "Comparator": "=",
        ///         "ValueSet": "123"
        ///     },
        ///     {   
        ///         "Name": "UpdatedDateFrom",
        ///         "Comparator": ">=",
        ///         "ValueSet": "2014-10-17 07:45:00"
        ///     },
        ///     {   
        ///         "Name": "UpdatedDateTo",
        ///         "Comparator": "<=",
        ///         "ValueSet": "2015-01-01 00:00:00"
        ///     },
        ///      {   
        ///         "Name": "Archived",
        ///         "Comparator": "=",
        ///         "ValueSet": "false"
        ///     }
        ///     
        ///     ]
        /// }
        /// 
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
        ///          "ContentID":456789,
        ///          "FolderID":123,
        ///          "ContentSource":"Please use Location property to retrieve Content Source",
        ///          "ContentText":"Please use Location property to retrieve Content Text",
        ///          "ContentTitle":"test bull",
        ///          "UseWYSIWYGeditor":null
        ///      },
        ///      "Location":"http://api.ecn5.com/api/content/456789"
        ///     }
        ///   ]
        /// ]]></example>
    
        [Route("api/Search/Content")]
        [HttpGet]
        [HttpPost]
        public List<Models.SearchResult<APIModel>> Search([FromBody] Models.SearchBase searchQuery)
        {
            if (null == searchQuery)
            {
                RaiseInvalidMessageException("Search parameter can't be empty");
            }
            return SearchBaseMethod(searchQuery, (controller,controllerContext,query) =>
            {
                string title = (string)GetConvertedQueryValue(query, "Title", typeof(string)) ?? "";
                int? folderID = (int?)GetConvertedQueryValue(query, "FolderID", typeof(int));
                int? userID = null;
                bool? archived = (bool?)GetConvertedQueryValue(query, "Archived", typeof(bool));
                DateTime? updatedDateFrom = (DateTime?)GetConvertedQueryValue(query, "UpdatedDateFrom", typeof(DateTime));
                DateTime? updatedDateTo = (DateTime?)GetConvertedQueryValue(query, "UpdatedDateTo", typeof(DateTime));
                APIUser.CustomerID = APICustomer.CustomerID;
                return ECN_Framework_BusinessLayer.Communicator.Content.
                    GetByContentSearch(title, folderID, userID, updatedDateFrom, updatedDateTo, APIUser, false, archived);
            });
        }

        #endregion Search
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

            // generate default title if it's empty
            if (String.IsNullOrWhiteSpace(apiModel.ContentTitle))
            {
                apiModel.ContentTitle = Strings.Format(Strings.BusinessDefaultValues.ContentTitle, DateTime.Now.ToString("yyyyMMdd-HH:mm:ss"));
            }
        }

        private void POST_FillFrameworkModelInternalFields(FrameworkModel frameworkModel)
        {
            frameworkModel.ContentTypeCode = ECN_Framework_Common.Objects.Communicator.Enums.ContentTypeCode.HTML.ToString();
            frameworkModel.CreatedUserID = APIUser.UserID;
            frameworkModel.CustomerID = APICustomer.CustomerID;
            frameworkModel.LockedFlag = "N";
            frameworkModel.Sharing = "N";
            frameworkModel.ContentMobile = frameworkModel.ContentSource ?? "";
            frameworkModel.ContentText = frameworkModel.ContentText ?? "";

        }

        private void PUT_FillFramworkModelInternalFields(FrameworkModel frameworkModel)
        {
            frameworkModel.UpdatedUserID = APIUser.UserID;
            frameworkModel.ContentMobile = frameworkModel.ContentSource;
            frameworkModel.ContentText = frameworkModel.ContentText ?? "";
        }

        #endregion Data Cleansing
    }
}