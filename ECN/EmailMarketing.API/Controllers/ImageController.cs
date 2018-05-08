using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Web.Http;
using System.Web;

// Framework components
using ECN_Framework_Entities.Accounts; //User
using EmailMarketing.API.Search;
using FrameworkModel = ECN_Framework_Entities.Communicator.Image;

// local components
using APIModel = EmailMarketing.API.Models.Image;
using EmailMarketing.API;
using EmailMarketing.API.Attributes;
using EmailMarketing.API.Models.Utility;
using EmailMarketing.API.ExtentionMethods;
using EmailMarketing.API.Exceptions;

// debug
using System.Diagnostics;

namespace EmailMarketing.API.Controllers
{
    using SearchQuery = List<Models.SearchProperty>;
    using SearchResult = Models.SearchResult<APIModel>;
    /// <summary>
    /// API methods exposing the Image object model as Resources for Create, Read, 
    /// Update and Delete via REST.  
    /// </summary>
    public class ImageController : SearchableApiControllerBase<APIModel, FrameworkModel>
    {
        #region Abstract Member Implementation

        /// <inheritdoc/>
        public override ECN_Framework_Common.Objects.Enums.Entity FrameworkEntity
        {
            get { return ECN_Framework_Common.Objects.Enums.Entity.Image; }
        }

        /// <summary>
        /// Lists common properties between the (external) API model and the associated (internal) framework model by this service.
        /// </summary>
        override public string[] ExposedProperties
        {
            get { return new string[] { "FolderName", "FolderID", "ImageID", "ImageName", "ImageData", "ImageURL"}; }
        }

        override public string ControllerName { get { return "image"; } }

        [NonAction]public override object GetID(APIModel model)
        {
            return 0;
        }

        [NonAction]public override object GetID(FrameworkModel model)
        {
            return 0;
        }

        internal FrameworkModel GetInternal(int id)
        {
            throw new NotImplementedException();
        }

        #endregion
        #region REST

        #region GET

        /// <summary>This feature is not implemented</summary>
        /// <param name="id">ImageID for the target resource</param>
        /// <returns>On success, a Image API model object; otherwise,
        /// <note>In most cases returns Http Status Code <pre>200 OK</pre>; however, if the target 
        /// resource doesn't exist --for example, if it has been deleted-- the result is 
        /// <pre>Error 404 - Not Found.</pre></note> </returns>
        /// <example for="request"><![CDATA[
        /// ]]></example>
        /// 
        /// <example for="response"><![CDATA[
        /// ]]></example>
        // GET api/message/<id-value>
        // GET /api/message/76660 \n APIAccessKey: 8CAB09B9-BEC9-453F-A689-E85D5C9E4898 Customer-ID: 1
        //public IEnumerable<APIModel> Get(/*int id*/string imageName, string folderName, User user)
        public IEnumerable<APIModel> Get(int id)
        {
            throw new NotImplementedException();
        }

        #endregion
        #region POST
        #region POST XML Documentation
        /// <summary>
        /// Given a Image model object, add a new resource (and REST Location 
        /// property).
        /// </summary>
        /// 
        /// <param name="model">Image model object</param>
        /// 
        /// <returns>On success, returns the given model object, 
        /// with a <pre>Location</pre> header.  In case of validation error(s), 
        /// Error <pre>400 - Bad Request</pre> is returned along with a message providing further 
        /// information.</returns>
        /// 
        /// <header for="request">APIAccessKey</header>
        /// <header for="response">Location</header>
        /// 
        /// <example for="request"><![CDATA[
        /// POST /api/image HTTP/1.1
        /// Content-Type: application/json; charset=utf-8
        /// Accept: application/json
        /// APIAccessKey: <YOUR_API_ACCESS_KEY>
        /// X-Customer-ID: 123
        /// Host: api.ecn5.com
        /// Content-Length: 18621
        /// 
        /// {
        ///     "FolderName":"MyFolder",
        ///     "ImageName":"NewImage.jpg",
        ///     "ImageData":[ <YOUR_BYTE_ARRAY> ] 
        /// }
        /// ]]></example>
        /// 
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 201 Created
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Content-Type: application/json; charset=utf-8
        /// Expires: -1
        /// Location: /api/image/0
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Tue, 16 Jun 2015 14:51:01 GMT
        /// Content-Length: 4731
        /// 
        /// {
        ///     "FolderID": 123,
        ///     "FolderName": "MyFolder",
        ///     "ImageName": "NewImage.jpg",
        ///     "ImageID":0,
        ///     "ImageURL":,
        ///     "ImageData":[ <YOUR_BYTE_ARRAY> ]
        ///}
        /// ]]></example>
        /// 
        #endregion POST XML Documentation

        public HttpResponseMessage Post(APIModel model)
        {
            if (model == null)
            {
                RaiseInvalidMessageException("no model in request body");
            }

            // 1. cleanse post data
            //CleanseInputData(model); // noop

            // 2. transform to internal model
            FrameworkModel frameworkModel = Transformer<APIModel, FrameworkModel>.Transform(model, ExposedProperties);

            // 3. fill properties not exposed via API model
            //POST_FillFrameworkModelInternalFields(frameworkModel); // noop

            // 4. delegate to business layer for validation and persistence...
            // ... making sure to update the API model's ID


            Infrastructure.Framework.ImageUtil.CreateNewImage(APICustomer.CustomerID, frameworkModel);
            var newID = frameworkModel.ImageID;
            

            // 5. fetch the newly created object
            //var newModel = Get(newId);

            // copy properties back from the framework model to get current values of any changed fields
            Transform(frameworkModel, model);

            // 6. explicitly create the HTTP response...
            // ...so we can install a Location header pointing to the created item
            //return CreatedResponseWithLocation(HttpStatusCode.Created, newModel, newId); //note the difference
            return CreateResponseWithLocation(HttpStatusCode.Created, model, newID);

            //^^^^ based on input. In theory, new model should be what is returned after we create a new image and fetch it. We cannot get a single image without search, so we are returning what they inputted despite what is actually being created.
        }

        #endregion
        #region PUT

        /// <summary>
        /// This feature is not implemented 
        /// </summary>
        /// <param name="apiModel">Image model object</param>
        /// <returns>On success, returns the given model object as well as a <pre>Location</pre> header.  
        /// In case of validation error(s), 
        /// Error <pre>400 - Bad Rquest</pre> is returned along with a message providing further 
        /// information.</returns>
        /// <example for="request"><![CDATA[
        /// ]]></example>
        /// 
        /// <example for="response"><![CDATA[
        /// ]]></example>
        

        public HttpResponseMessage Put([FromBody]APIModel apiModel)
        {
            throw new NotImplementedException();
        }

        #endregion
        #region DELETE

        /// <summary>
        /// Removes target resource.
        /// </summary>
        /// <param name="model">Model containing the ImageName and the FolderName of the target resource</param>
        /// <example for="request"><![CDATA[
        /// DELETE /api/image HTTP/1.1
        /// Content-Type: application/json; charset=utf-8
        /// Accept: application/json
        /// APIAccessKey: <YOUR_API_ACCESS_KEY>
        /// X-Customer-ID: 123
        /// Host: api.ecn5.com
        /// 
        /// {
        ///     "FolderName":"MyFolder",
        ///     "ImageName":"ImageToBeDeleted.jpg"
        /// }
        /// 
        /// ]]></example>
        ///<example for="response"><![CDATA[
        /// HTTP/1.1 204 No Content
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Expires: -1
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Tue, 16 Jun 2015 16:00:44 GMT
        /// 
        /// ]]></example>
        // DELETE/api/message/76660 
        public void Delete(APIModel model/*string imageName, string folderName,*/)
        {
            if(model == null)
            {
                RaiseInvalidMessageException("no API model supplied to delete");
            }

            string imageDirectory = Infrastructure.Framework.ImageUtil.MakeCustomerImageDirectoryPath(APICustomer.CustomerID, model.FolderName);
            
            //Get all images in folder with the same image name
            string[] ImagesWithDesiredName = Directory.GetFiles(imageDirectory, model.ImageName);

            if (ImagesWithDesiredName.Length == 1)
            {
                File.Delete(Path.Combine(imageDirectory, model.ImageName));
            }
            else if (ImagesWithDesiredName.Length > 1)
            {
                throw new ImageException(Strings.Errors.FriendlyMessages.Images.MORE_THAN_ONE_IMAGE);
            }
            else
            {
                throw new ImageException(Strings.Errors.FriendlyMessages.Images.IMAGE_DOES_NOT_EXIST);
            }

           
        }

        #endregion

        #endregion
        #region Search
        #region Search XML Documentation

        /// <summary>
        /// Provides search capabilities for Image resources. Search supports both GET and POST methods; results will be identical.  Note, search does not currently ImageData for the returned images. 
        /// To retrieve image file content make a separate GET request for ImageURL of a particular image.
        /// </summary>
        /// <see cref="EmailMarketing.API.Models.Image"/>
        /// <param name="searchQuery">used to constrain the search</param>
        /// <returns>a list of matching <see cref="EmailMarketing.API.Models.Image"/> as Location/API Object pairs.</returns>
        /// 
        /// <example for="request"><![CDATA[
        /// 
        /// Search for "MyImage.jpg" in "MyFolder" and all sub-folders
        /// 
        /// GET http://api.ecn5.com/api/search/image HTTP/1.1
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
        ///         "Name": "ImageName",
        ///         "Comparator": "=",
        ///         "ValueSet": "MyImage.jpg"
        ///     },
        ///     {
        ///         "Name": "FolderName",
        ///         "Comparator": "=",
        ///         "ValueSet": "MyFolder"
        ///     },
        ///     {
        ///         "Name": "Recursive",
        ///         "Comparator": "=",
        ///         "ValueSet": "true"
        ///     }
        ///     ]
        ///}
        /// 
        /// Search for all images in any folder with a filename containing "MyImage"
        /// 
        /// GET http://api.ecn5.com/api/search/image HTTP/1.1
        /// Content-Type: application/json; charset=utf-8
        /// Accept: application/json
        /// APIAccessKey: <YOUR_API_ACCESS_KEY>
        /// X-Customer-ID: 123
        /// Host: api.ecn5.com
        /// Content-Length: 87
        /// 
        ///{
        ///     "SearchCriteria": [
        ///     {
        ///         "Name": "ImageName",
        ///         "Comparator": "contains",
        ///         "ValueSet": "MyImage.jpg"
        ///     },
        ///     {
        ///         "Name": "Recursive",
        ///         "Comparator": "=",
        ///         "ValueSet": "true"
        ///     }
        ///     ]
        ///}
        /// 
        /// Search for all images in the folder "MyFolder", not including sub-folders.
        /// 
        /// GET http://api.ecn5.com/api/search/image HTTP/1.1
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
        ///         "ValueSet": "MyFolder"
        ///     }
        ///     ]
        ///}
        /// 
        /// Search for images in the images root folder.
        /// 
        /// GET http://api.ecn5.com/api/search/image HTTP/1.1
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
        /// Search for all images.
        /// 
        /// GET http://api.ecn5.com/api/search/image HTTP/1.1
        /// Content-Type: application/json; charset=utf-8
        /// Accept: application/json
        /// APIAccessKey: <YOUR_API_ACCESS_KEY>
        /// X-Customer-ID: 123
        /// Host: api.ecn5.com
        /// Content-Length: 64
        /// 
        /// {  
        ///    "SearchCriteria": [ 
        ///    { 
        ///          "name":"Recursive",
        ///          "comparator":"=",
        ///          "valueSet":"true"
        ///     }
        ///     ]
        /// }
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
        /// Date: Wed, 17 Jun 2015 17:31:45 GMT
        /// Content-Length: 286
        /// 
        /// [  
        ///    {  
        ///       "ApiObject":{  
        ///          "FolderName":"F1/F2/F3",
        ///          "FolderID":-1,
        ///          "ImageID":0,
        ///          "ImageName":"MyImage.jgp",
        ///          "ImageData":,
        ///          "ImageURL":"http://www.ecn5.com/ecn.images/Customers/123/Images/F1/F2/F3/MyImage.jpg"
        ///       },
        ///       "Location":"http://api.ecn5.com/EmailMarketing.API/api/image/0"
        ///    }
        /// ]
        /// ]]></example>
        
        #endregion Search XML Documentation

        [Route("api/Search/Image")]
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
                string imageName = (string)GetConvertedQueryValue(query, "ImageName", typeof(string));
                string imageComparator = GetQueryComparator(query, "ImageName") ?? "=";
                string folderName = (string)GetConvertedQueryValue(query, "FolderName", typeof(string));
                bool recursive = (bool)(GetConvertedQueryValue(query, "Recursive", typeof(bool)) ?? false);
                bool partial = imageComparator == "contains";

                return Infrastructure.Framework.ImageUtil.SearchImages(APICustomer.CustomerID, imageName, folderName, partial, recursive);
            });
        }

        #endregion
        #region Data Cleansing

        /// <summary>
        /// Default data assignment to avoid code breaking errors
        /// </summary>
        /// <param name="apiModel">The APIModel that is being acted upon by HTTP PUT or HTTP POST</param>
        private void CleanseInputData(APIModel apiModel)
        {
           //noop
        }

        /// <summary>
        /// Prepares APIModel from a POST for transformation to a FrameworkModel
        /// </summary>
        /// <param name="frameworkModel">The API Model instance provided for insert via HTTP POST</param>
        private void POST_FillFrameworkModelInternalFields(FrameworkModel frameworkModel)
        {
            //noop
        }

        /// <summary>
        /// Prepares APIModel from a PUT for transformation to a FrameworkModel
        /// </summary>
        /// <param name="frameworkModel">The APIModel instance provided for update via HTTP PUT</param>
        private void PUT_FillFrameworkModelInternalFields(FrameworkModel frameworkModel)
        {
            //noop
        }


        #endregion
    }
}

