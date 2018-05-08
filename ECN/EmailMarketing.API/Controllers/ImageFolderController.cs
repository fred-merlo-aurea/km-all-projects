using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using APIModel = EmailMarketing.API.Models.ImageFolder;
using FrameworkModel = ECN_Framework_Entities.Communicator.ImageFolder;

namespace EmailMarketing.API.Controllers
{
    // aliasing inside namespace where the declarations will then be identical in each controller
    using SearchQuery = List<Models.SearchProperty>;
    using SearchResult = Models.SearchResult<APIModel>;

    /// <summary>
    /// API methods exposing the Image Folder object model as Resources for Create, Read, 
    /// Update and Delete via REST.  
    /// </summary>
    [RoutePrefix("api/imagefolder")]
    public class ImageFolderController : SearchableApiControllerBase<APIModel,FrameworkModel>
    {

        static internal FrameworkModel InternalGet(string folderFullName)
        {
            throw new NotImplementedException();
        }

        #region abstract methods implementation


        /// <inheritdoc/>
        public override ECN_Framework_Common.Objects.Enums.Entity FrameworkEntity
        {
            get { return ECN_Framework_Common.Objects.Enums.Entity.ImageFolder; }
        }

        public override string[] ExposedProperties
        {
            get { return new string[] { "FolderID", "FolderName", "FolderFullName", "FolderUrl" }; }
        }

        public override object GetID(APIModel model)
        {
            return 0; // model.FolderID;
        }

        public override object GetID(FrameworkModel model)
        {
            return 0; // model.FolderID;
        }

        public override string ControllerName
        {
            get { return "imagefolder"; }
        }

        #endregion abstract methods implementation
        #region exposed methods

        /*
        /// <summary>
        /// describe the method, please
        /// </summary>
        /// <example for="request"><![CDATA[
        /// ]]></example>
        /// <example for="response"><![CDATA[
        /// ]]></example>
        
        [Route("methods/MyMethod")]
        [AuthenticationRequired(AccessKey: AccessKeyType.BaseChannel)] // override authentication properties, if necessary
        public APIModel GetFormsUser()
        {
           throw new NotImplementedException();
        }

        */ 

        #endregion exposed methods
        #region REST

        /// <summary>
        /// This feature is not implemented
        /// </summary>
        [Route("{id}")]
        public APIModel Get(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create an image folder
        /// </summary>
        /// <example for="request"><![CDATA[
        /// POST http://api.ecn5.com/api/imagefolder HTTP/1.1
        /// Content-Type: application/json; charset=utf-8
        /// Accept: application/json
        /// APIAccessKey: 15bcb4b3-197e-4ad9-9e83-b9ce837e45ab
        /// X-Customer-ID: 1
        /// Host: api.ecn5.com
        /// Content-Length: 35
        /// 
        /// {
        ///    "FolderName":"ExampleFolderName",
        /// }
        /// ]]></example>
        ///
        ///<example for="response"><![CDATA[
        ///HTTP/1.1 201 Created
        ///Cache-Control: no-cache
        ///Pragma: no-cache
        ///Content-Type: application/json; charset=utf-8
        ///Expires: -1
        ///Location: http://api.ecn5.com/api/imagefolder/0
        ///Server: Microsoft-IIS/7.5
        ///X-AspNet-Version: 4.0.30319
        ///X-Powered-By: ASP.NET
        ///Date: Fri, 14 Aug 2015 23:38:54 GMT
        ///Content-Length: 153
        ///
        ///{
        ///   "FolderID":0,
        ///   "FolderName":"AddedViaAPI\",
        ///   "FolderFullName":"AddedViaAPI\",
        ///   "FolderUrl":"http://www.ecn5.com/ecn.images/Customers/1/Images/AddedViaAPI/"
        ///}
        ///]]></example>

        public HttpResponseMessage Post([FromBody]APIModel apiModel)
        {
            if (apiModel == null)
            {
                RaiseInvalidMessageException("no model in request body");
            }

            string folderName = apiModel.FolderName;

            // this will raise a validation exception if the folderName isn't valid
            string folderFullPath = Infrastructure.Framework.ImageUtil.MakeCustomerImageDirectoryPath(APICustomer.CustomerID, folderName);

            // explicit check for prior existence
            if (Directory.Exists(folderFullPath))
            {
                throw new ECN_Framework_Common.Objects.ECNException(new List<ECN_Framework_Common.Objects.ECNError>
                {
                    new ECN_Framework_Common.Objects.ECNError(
                        Infrastructure.Framework.ImageUtil.RelatedFrameworkEntity,
                        ECN_Framework_Common.Objects.Enums.Method.Create,
                        "folder already exists"
                        )
                });
            }

            // this creates the folder (or raises a validation exception)
            Infrastructure.Framework.ImageUtil.MakeCustomerImageDirectoryPath(APICustomer.CustomerID, folderName, true);

            // use the "business layer" to fetch a new copy of the framework model
            FrameworkModel frameworkModel = Infrastructure.Framework.ImageUtil.CreateFolderFromFullPath(APICustomer.CustomerID, folderFullPath);

            // transform to API model
            APIModel newModel = Transform( frameworkModel );

            // return customized response message with 201 status and a Location header pointed to the newly created entity
            return CreateResponseWithLocation(HttpStatusCode.Created, newModel, newModel.FolderID);
        }

        /// <summary>
        /// This feature is not implemented
        /// </summary>
        [Route("{id}")]
        public APIModel Put(int id, [FromBody]APIModel apiModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This feature is not implemented
        /// </summary>
        [Route("{id}")]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        #endregion REST
        #region search

        /// <summary>
        /// Return information about each Image Folder matching the given search criteria.
        /// </summary>
        /// <example for="request"><![CDATA[
        /// Search for "MySubFolder" within all sub-folders
        /// 
        /// GET http://api.ecn5.com/api/search/imagefolder HTTP/1.1
        /// Content-Type: application/json; charset=utf-8
        /// Accept: application/json
        /// APIAccessKey: <YOUR_API_ACCESS_KEY>
        /// X-Customer-ID: 123
        /// Host: api.ecn5.com
        /// Content-Length: 161
        /// 
        ///{
        ///     "SearchCriteria": [
        ///     {
        ///         "Name": "FolderName",
        ///         "Comparator": "=",
        ///         "ValueSet": "MySubFolder"
        ///     },
        ///     {
        ///         "Name": "Recursive",
        ///         "Comparator": "=",
        ///         "ValueSet": "true"
        ///     }
        ///     ]
        ///}
        /// 
        /// Search for all folders with a name containing "My" within all sub-folders
        /// 
        /// GET http://api.ecn5.com/api/search/imagefolder HTTP/1.1
        /// Content-Type: application/json; charset=utf-8
        /// Accept: application/json
        /// APIAccessKey: <YOUR_API_ACCESS_KEY>
        /// X-Customer-ID: 123
        /// Host: api.ecn5.com
        /// Content-Length: 161
        /// 
        ///{
        ///     "SearchCriteria": [
        ///     {
        ///         "Name": "FolderName",
        ///         "Comparator": "contains",
        ///         "ValueSet": "My"
        ///     },
        ///     {
        ///         "Name": "Recursive",
        ///         "Comparator": "=",
        ///         "ValueSet": "true"
        ///     }
        ///     ]
        ///}
        /// 
        /// Search for "MySubFolder", not including sub-folders.
        /// 
        /// GET http://api.ecn5.com/api/search/imagefolder HTTP/1.1
        /// Content-Type: application/json; charset=utf-8
        /// Accept: application/json
        /// APIAccessKey: <YOUR_API_ACCESS_KEY>
        /// X-Customer-ID: 123
        /// Host: api.ecn5.com
        /// Content-Length: 68
        /// 
        ///{
        ///     "SearchCriteria": [
        ///     {
        ///         "Name": "FolderName",
        ///         "Comparator": "=",
        ///         "ValueSet": "MySubFolder"
        ///     }
        ///     ]
        ///}
        /// 
        /// Search for sub-folders within the images root folder.
        /// 
        /// GET http://api.ecn5.com/api/search/imagefolder HTTP/1.1
        /// Content-Type: application/json; charset=utf-8
        /// Accept: application/json
        /// APIAccessKey: <YOUR_API_ACCESS_KEY>
        /// X-Customer-ID: 123
        /// Host: api.ecn5.com
        /// Content-Length: 2
        /// 
        ///{
        ///     "SearchCriteria": []
        ///}
        /// 
        /// Search for all image folders.
        /// 
        /// GET http://api.ecn5.com/api/search/imagefolder HTTP/1.1
        /// Content-Type: application/json; charset=utf-8
        /// Accept: application/json
        /// APIAccessKey: <YOUR_API_ACCESS_KEY>
        /// X-Customer-ID: 123
        /// Host: api.ecn5.com
        /// Content-Length: 64
        /// 
        ///{
        ///     "SearchCriteria": [
        ///     {
        ///         "Name": "Recursive",
        ///         "Comparator": "=",
        ///         "ValueSet": "true"
        ///     }
        ///     ]
        ///}
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
        /// Date: Tue, 30 Jun 2015 20:41:51 GMT
        /// Content-Length: 220
        /// [
        ///    {
        ///       "ApiObject":
        ///       { 
        ///          "FolderID":0,
        ///          "FolderName":"myFolder",
        ///          "FolderFullName":"myFolder",
        ///          "FolderUrl":"http://api.ecn5.com/ecn.images/Customers/123/Images/myFolder/"
        ///       },
        ///       "Location":"http://api.ecn5.com/api/imagefolder/0"
        ///    }
        /// ]
        /// ]]></example>
        [Route("~/api/search/imagefolder")]
        [HttpGet]
        [HttpPost]
        public IEnumerable<SearchResult> Search([FromBody] Models.SearchBase searchQuery)
        {
            if (null == searchQuery)
            {
                RaiseInvalidMessageException("Search parameter can't be empty");
            }
            return SearchBaseMethod(searchQuery, (controller, controllerContext, query) =>
            {
                string folderName = (string)GetConvertedQueryValue(query, "FolderName", typeof(string));
                string folderComparator = GetQueryComparator(query, "FolderName")??"=";
                bool recursive = (bool)(GetConvertedQueryValue(query, "Recursive", typeof(bool))??false);
                bool partial = folderComparator == "contains";

                return Infrastructure.Framework.ImageUtil.SearchFolders(APICustomer.CustomerID, folderName, partial, recursive);
            });
        }

        #endregion search
        #region data cleansing
        #endregion data cleansing
    }
}
