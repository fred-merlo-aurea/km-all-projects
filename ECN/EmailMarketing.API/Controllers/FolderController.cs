using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmailMarketing.API.Exceptions;
using EmailMarketing.API.Models;
using System.Linq;

using FrameworkModel = ECN_Framework_Entities.Communicator.Folder;
using APIModel = EmailMarketing.API.Models.Folder;
using Folder = ECN_Framework_BusinessLayer.Communicator.Folder;

namespace EmailMarketing.API.Controllers
{
    using SearchQuery = List<SearchProperty>;
    using SearchResult = SearchResult<APIModel>;

    /// <summary>
    /// API methods exposing the Email Marketing Folder object model as Resources for Create, Read, 
    /// Update and Delete via REST.  
    /// </summary>

    [RoutePrefix("api/folder")]
    public class FolderController : SearchableApiControllerBase<APIModel, FrameworkModel>
    {
        #region abstract member implementations

        /// <inheritdoc/>
        public override ECN_Framework_Common.Objects.Enums.Entity FrameworkEntity
        {
            get { return ECN_Framework_Common.Objects.Enums.Entity.Folder; }
        }

        /// <summary>
        /// Lists common properties between the (external) API model and the associated (internal) framework model by this service.
        /// </summary>
        public override string[] ExposedProperties
        {
            get { return new string[] { "FolderID", "FolderName", "FolderDescription", "ParentID", "FolderType", "CreatedDate", "CreatedUserID", "UpdatedDate", "UpdatedUserID" }; }
        }
        override public string ControllerName { get { return "folder"; } }
        public override object GetID(APIModel model)
        {
            return model.FolderID;
        }
        public override object GetID(FrameworkModel model)
        {
            return model.FolderID;
        }
        internal FrameworkModel GetInternal(int id)
        {
            FrameworkModel returnValue = null;
            try
            {
               returnValue = Folder.GetByFolderID(id, APIUser);
            }
            catch (NullReferenceException e)
            {
                // this allows us to handle 404 in the usual manner
                //System.Diagnostics.Trace.TraceError("failed to load folder: {0}",e);
            }
            return returnValue;
        }
        #endregion abstract member implementations
        #region REST
        #region GET 
        /// <summary>
        /// Retrieves the Folder resource identified by <pre>id</pre> for groups, content, and messages
        /// </summary>
        /// <param name="id">FolderID for the target resource</param>
        /// <returns>On success, a Folder API model object; otherwise,
        /// <note>In most cases returns Http Status Code <pre>200 OK</pre>; however, if the target 
        /// resource doesn't exist --for example, if it has been deleted-- the result is 
        /// <pre>Error 404 - Not Found.</pre></note> </returns>
        /// <example for="request"><![CDATA[
        /// GET http://api.ecn5.com/api/folder/99999 HTTP/1.1
        /// Accept: application/json
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
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Tue, 09 Jun 2015 19:35:04 GMT
        /// Content-Length: 158
        /// 
        /// {
        ///    "FolderID":111,
        ///    "FolderName":"MyFolder",
        ///    "ParentID":0,
        ///    "FolderDescription":"My Folder Description",
        ///    "CreatedDate":null,
        ///    "CreatedUserID":0,
        ///    "UpdatedDate":null,
        ///    "UpdatedUserID":0123
        /// }
        /// ]]></example>
        /// 
        
        // GET api/folder/<id-value>
        // GET /api/folder/111 \nAPIAccessKey: 8CAB09B9-BEC9-453F-A689-E85D5C9E4898
        [Route("{id}")]
        [HttpGet]
        public APIModel Get(int id)
        {
            FrameworkModel frameworkObject = GetInternal(id);
            if (null == frameworkObject)
            {
                RaiseNotFoundException(id);
            }
            APIModel apiObject = Transform(frameworkObject);
            return apiObject;
        }
        #endregion GET
        #region POST
        /// <summary>
        /// Given a Folder model object, add a new resource, assigning unique FolderID attribute (and REST Location 
        /// property).
        /// </summary>
        /// 
        /// <param name="model">Folder model object</param>
        /// 
        /// <returns>On success, returns the given model object with <pre>FolderID</pre> filled in, 
        /// as well as a <pre>Folder</pre> header.  In case of validation error(s), 
        /// Error <pre>400 - Bad Request</pre> is returned along with a message providing further 
        /// information.</returns>
        /// 
        /// <header for="request">APIAccessKey</header>
        /// <header for="response">Location</header>
        /// 
        /// <example for="request"><![CDATA[
        /// POST /api/Folder HTTP/1.1
        /// Accept: application/json
        /// Accept-Language: en-US
        /// User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko
        /// Accept-Encoding: gzip, deflate
        /// Connection: Keep-Alive
        /// APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        /// X-Customer-ID: 99999
        /// Content-Type: application/json
        /// Host: api.ecn5.com
        /// 
        /// { 
        ///   "FolderName":"MyFolder",
        ///   "FolderType":"CNT",
        ///   "ParentID":0,
        ///   "FolderDescription":"MyFolderDescription"
        /// }
        /// 
        /// ]]></example>
        /// 
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 201 Created
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Content-Type: application/json; charset=utf-8
        /// Expires: -1
        /// Location: http://api.ecn5.com/api/folder/99999
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Tue, 16 Jun 2015 14:48:54 GMT
        /// Content-Length: 114
        /// 
        /// {
        /// "FolderID":99999,
        /// "FolderName":"MyFolder",
        /// "ParentID":0,
        /// "FolderDescription":"MyFolderDescription",
        /// "FolderType":"CNT"
        /// }
        /// ]]></example>
        [Route("")]
        [HttpPost]
        public HttpResponseMessage Post(APIModel model)
        {
            if (model == null)
            {
                RaiseInvalidMessageException("no model in request body");
            }
            CleanseInputData(model);
            FrameworkModel frameworkModel = Transform(model);
            POST_FillFrameworkModelInternalFields(frameworkModel);
            var newId = Folder.Save(frameworkModel, APIUser);
            APIModel newModel = Transform(GetInternal(newId));
            return CreateResponseWithLocation(HttpStatusCode.Created, newModel, newId);
        }
        #endregion POST
        #region PUT
        /// <summary>
        /// Updates resource identified by <pre>id</pre> with the given model object
        /// </summary>
        /// <param name="id">FolderID for the target resource</param>
        /// <param name="apiModel">Folder model object</param>
        /// <returns>On success, returns the given model object as well as a <pre>Location</pre> header.  
        /// In case of validation error(s), 
        /// Error <pre>400 - Bad Rquest</pre> is returned along with a message providing further 
        /// information.</returns>
        /// <example for="request"><![CDATA[
        /// PUT http://api.ecn5.com/api/folder/99999 HTTP/1.1
        /// Accept: application/json
        /// Accept-Language: en-US
        /// User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko
        /// Accept-Encoding: gzip, deflate
        /// Connection: Keep-Alive
        /// APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        /// X-Customer-ID: 99999
        /// Content-Type: application/json
        /// Host: api.ecn5.com
        /// 
        /// { 
        ///   "FolderID":99999,
        ///   "FolderName":"MyFolderUpdate",
        ///   "FolderType":"CNT",
        ///   "ParentID":0,
        ///   "FolderDescription":"MyFolderDescription"
        /// }
        /// 
        /// ]]></example>
        /// 
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 201 Created
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Content-Type: application/json; charset=utf-8
        /// Expires: -1
        /// Location: http://api.ecn5.com/api/folder/99999
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Tue, 16 Jun 2015 15:13:40 GMT
        /// Content-Length: 122
        /// {
        ///     "FolderID":99999,
        ///     "FolderName":"MyFolderUpdate",
        ///     "ParentID":0,
        ///     "FolderDescription":"MyFolderDescription",
        ///      "FolderType":"CNT",
        ///     "CreatedDate":null,
        ///     "CreatedUserID":0,
        ///     "UpdatedDate":null,
        ///     "UpdatedUserID":99999
        /// }
        /// ]]></example>
        [Route("{id}")]
        [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]APIModel apiModel)
        {
            if (apiModel == null)
            {
                RaiseInvalidMessageException("no model in request body");
            }

            CleanseInputData(apiModel);

            if (id < 1 || apiModel.FolderID < 1 || apiModel.FolderID != id)
            {
                RaiseInvalidMessageException("FolderID is required and must the resource ID given in the request URI");
            }

            FrameworkModel frameworkModel = Folder.GetByFolderID(id, APIUser);
            if(null == frameworkModel || frameworkModel.FolderID < 1)
            {
                RaiseNotFoundException(id);
            }

            #region Silent FolderType ignore
            //Because we don't want to allow the user to update the FolderType, 
            //We will set the APIModel's FolderType to remain the same (as the existing FrameworkModel's)
            //I.e...
            apiModel.FolderType = frameworkModel.FolderType;
            //This way, we cannot update the FolderType of a folder
            #endregion


            Transform(apiModel, frameworkModel);
            PUT_FillFramworkModelInternalFields(frameworkModel);
            Folder.Save(frameworkModel, APIUser);

            APIModel newModel = Transform(GetInternal(id));
            return CreateResponseWithLocation(HttpStatusCode.OK, newModel, id);
        }
        #endregion PUT
        #region DELETE
        /// <summary>
        /// Removes a Folder resource.
        /// </summary>
        /// <param name="id">FolderID of the target resource</param>
        /// <example for="request"><![CDATA[
        /// DELETE http://api.ecn5.com/api/folder/99999 HTTP/1.1
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
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Tue, 16 Jun 2015 15:21:46 GMT
        /// ]]></example>
        [Route("{id}")]
        [HttpDelete]
        public void Delete(int id)
        {
            Folder.Delete(id, APIUser);
        }
        #endregion DELETE
        #endregion REST
        #region Search
        /// <summary>
        /// Provides search capabilities for Folder resources. Search supports both GET and POST methods; results will be identical.
        /// Note, <code>Type</code> is always required for folder search.
        /// </summary>
        /// <see cref="EmailMarketing.API.Models.Folder"/>
        /// <param name="searchQuery">used to constrain the search</param>
        /// <returns>a list of matching <see cref="EmailMarketing.API.Models.Folder"/> as Folder/API Object pairs.</returns>
        /// 
        /// <example for="request"><![CDATA[
        /// 
        /// * All "Content" folders
        /// 
        /// GET http://api.ecn5.com/api/search/folder HTTP/1.1
        /// Content-Type: application/json; charset=utf-8
        /// Accept: application/json
        /// APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        /// X-Customer-ID: 99999
        /// Host: api.ecn5.com
        /// Content-Length: 357
        /// 
        /// {  
        ///     "SearchCriteria": [
        ///     {
        ///         "Name": "Type",
        ///         "Comparator": "=",
        ///         "ValueSet": "CNT"
        ///     }
        ///     ]
        /// }
        /// 
        /// * All "Group" folders
        /// 
        /// GET http://api.ecn5.com/api/search/folder HTTP/1.1
        /// Content-Type: application/json; charset=utf-8
        /// Accept: application/json
        /// APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        /// X-Customer-ID: 99999
        /// Host: api.ecn5.com
        /// Content-Length: 357
        ///    
        /// {  
        ///     "SearchCriteria": [
        ///     {
        ///         "Name": "Type",
        ///         "Comparator": "=",
        ///         "ValueSet": "GRP"
        ///     }
        ///     ]
        /// }
        ///
        /// 
        /// ]]></example>
        /// 
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 200 OK
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Content-Type: application/json; charset=utf-8
        /// Expires: -1
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Wed, 17 Jun 2015 18:37:51 GMT
        /// Content-Length: 427
        /// 
        /// [  
        ///    {  
        ///       "ApiObject":
        ///       {  
        ///          "FolderID":99998,
        ///          "FolderName":"MyFolder1",
        ///          "ParentID":99999,
        ///          "FolderDescription":"MyDescription1",
        ///          "FolderType":"CNT"
        ///       },
        ///       "Location":"http://api.ecn5.com/api/folder/99999"
        ///    },
        ///    {  
        ///       "ApiObject":
        ///       {  
        ///          "FolderID":99999,
        ///          "FolderName":"MyFolder2",
        ///          "ParentID":99999,
        ///          "FolderDescription":"MyDescription2",
        ///          "FolderType":"CNT"
        ///       },
        ///       "Location":"http://api.ecn5.com/api/folder/99999"
        ///    }
        /// ]
        /// ]]></example>

        [Route("~/api/search/folder")]
        [HttpGet]
        [HttpPost]
        public IEnumerable<SearchResult> Search([FromBody] Models.SearchBase searchQuery)
        {
            if (searchQuery == null)
            {
                RaiseInvalidMessageException("Search parameter can't be empty");

            }

            return SearchBaseMethod(searchQuery, (controller, controllerContext, query) =>
            {
                string type = (string)GetConvertedQueryValue(query, "Type", typeof(string));
                int? parentID = (int?)GetConvertedQueryValue(query, "ParentID", typeof(int));
                if(String.IsNullOrWhiteSpace(type))
                {
                    RaiseInvalidMessageException("Type is required");
                }
                List<FrameworkModel> results = Folder.GetByContentSearch(Convert.ToInt32(APICustomer.CustomerID), type, APIUser);
                return parentID.HasValue ? from x in results where x.ParentID == parentID select x : results;
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
            // set default values on APIModel
        }
        private void POST_FillFrameworkModelInternalFields(FrameworkModel frameworkModel)
        {
            Set_FillFrameworkModelInternalFields(frameworkModel);
        }
        private void PUT_FillFramworkModelInternalFields(FrameworkModel frameworkModel)
        {
            Set_FillFrameworkModelInternalFields(frameworkModel);
        }
        private void Set_FillFrameworkModelInternalFields(FrameworkModel frameworkModel)
        {
            frameworkModel.CreatedUserID = APIUser.UserID;
            frameworkModel.CustomerID = APICustomer.CustomerID;
            //
            frameworkModel.UpdatedUserID = APIUser.UserID;
            frameworkModel.IsSystem = false;
        }
        #endregion Data Cleansing
    }
}
