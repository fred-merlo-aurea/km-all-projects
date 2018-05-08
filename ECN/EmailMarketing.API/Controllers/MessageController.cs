using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ECN_Framework_BusinessLayer.Communicator;
using EmailMarketing.API.Models.Utility;
using static ECN_Framework_BusinessLayer.Communicator.Content;
using APIModel = EmailMarketing.API.Models.Message;
using FrameworkModel = ECN_Framework_Entities.Communicator.Layout;

namespace EmailMarketing.API.Controllers
{
    using SearchResult = Models.SearchResult<APIModel>;

    /// <summary>
    /// Methods allowing manipulation of email messages ( "layouts" ) via REST. NOTE: There is a known bug where created and updated dates always show NULL. This will be fixed after 2015 Q3 release.
    /// </summary>
    public class MessageController : SearchableApiControllerBase<APIModel, FrameworkModel>
    {
        private const string NoModelInRequestBody = "no model in request body";
        private const string ContentId = "ContentID";
        private const string IsNotValidated = "is not validated";
        private const string ContentIdForContentSlotDoesnotExist = "ContentID for ContentSlot{0} doesn't exist";

        #region Abstract Member Implementations

        /// <inheritdoc/>
        public override ECN_Framework_Common.Objects.Enums.Entity FrameworkEntity
        {
            get { return ECN_Framework_Common.Objects.Enums.Entity.Layout; }
        }

        /// <summary>
        /// Lists common properties between the (external) API model and the associated (internal) framework model by this service.
        /// </summary>
        override public string[] ExposedProperties
        {
            get { return new string[] { "LayoutID", "TemplateID", "FolderID", "LayoutName", "ContentSlot1", "ContentSlot2", "ContentSlot3", "ContentSlot4", "ContentSlot5", "ContentSlot6", "ContentSlot7", "ContentSlot8", "ContentSlot9", "CreatedUserID", "UpdatedDate", "TableOptions", "DisplayAddress", /*"SetupCost", "OutboundCost", "InboundCost", "DesignCost", "OtherCost",*/ "MessageTypeID", "CreatedDate", /*"IsDeleted",*/ "UpdatedUserID", "Archived"}; }
        }

        override public string ControllerName { get { return "message"; } }

        public override object GetID(APIModel model)
        {
            //return model.MessageID;
            return model.LayoutID;
        }

        public override object GetID(FrameworkModel model)
        {
            return model.LayoutID;
        }

        internal FrameworkModel GetInternal(int id)
        {
            return ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID(id, APIUser, false);
        }

        #endregion abstract member implementations
        #region REST

        #region GET

        /// <summary>
        /// Retrieves the Message resource identified by <pre>id</pre>
        /// </summary>
        /// <param name="id">LayoutID for the target resource</param>
        /// <returns>On success, a Message API model object; otherwise,
        /// <note>In most cases returns Http Status Code <pre>200 OK</pre>; however, if the target 
        /// resource doesn't exist --for example, if it has been deleted-- the result is 
        /// <pre>Error 404 - Not Found.</pre></note> </returns>
        /// 
        /// 
        /// <example for="request"><![CDATA[
        /// GET http://api.ecn5.com/api/message/12345 HTTP/1.1
        /// Host: api.ecn5.com
        /// APIAccessKey: <YOUR_API_ACCESS_KEY>
        /// X-Customer-ID: 123
        /// 
        /// ]]></example>
        /// 
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
        /// Date: Mon, 08 Jun 2015 15:12:49 GMT
        /// Content-Length: 488
        /// 
        /// {  
        ///    "LayoutID":12345,
        ///    "TemplateID":1234,
        ///    "FolderID":0,
        ///    "LayoutName":"Layout Name",
        ///    "ContentSlot1":54321,
        ///    "ContentSlot2":0,
        ///    "ContentSlot3":0,
        ///    "ContentSlot4":0,
        ///    "ContentSlot5":0,
        ///    "ContentSlot6":0,
        ///    "ContentSlot7":0,
        ///    "ContentSlot8":0,
        ///    "ContentSlot9":0,
        ///    "UpdatedDate":null,
        ///    "TableOptions":"border=1 bordercolor=black width=600 cellpadding=0 cellspacing=0",
        ///    "DisplayAddress":"321 Happy St, Tunsasmiles, CA 10101",
        ///    "MessageTypeID":0,
        ///    "CreatedDate":null
        /// }
        /// }
        /// ]]></example>
        // GET api/message/<id-value>
        // GET /api/message/76660 \n APIAccessKey:  8CAB09B9-BEC9-453F-A689-E85D5C9E4898 Customer-ID: 1
        public APIModel Get(int id)
        {
            FrameworkModel frameworkObject = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID(id, APIUser, false);
            if (null == frameworkObject)
            {
                RaiseNotFoundException(id);
            }

            APIModel apiObject = Transformer<FrameworkModel, APIModel>.Transform(frameworkObject, ExposedProperties);
            //apiObject.MessageID = frameworkObject.LayoutID;

            return apiObject;
        }

        #endregion
        #region POST

        /// <summary>
        /// Given a Messagse model object, add a new resource, assigning unique LayoutID attribute (and REST Location 
        /// property).
        /// </summary>
        /// 
        /// <param name="model">Message model object</param>
        /// 
        /// <returns>On success, returns the given model object with <pre>LayoutID</pre> filled in, 
        /// as well as a <pre>Location</pre> header.  In case of validation error(s), 
        /// Error <pre>400 - Bad Request</pre> is returned along with a message providing further 
        /// information.</returns>
        /// 
        /// <header for="request">APIAccessKey</header>
        /// <header for="response">Location</header>
        /// 
        /// <example for="request"><![CDATA[
        /// POST http://api.ecn5.com/api/message/ HTTP/1.1
        /// Host: api.ecn5.com
        /// APIAccessKey: <YOUR_API_ACCESS_KEY>
        /// X-Customer-ID: 123
        /// Content-Type: application/json
        /// Content-Length: 452
        /// 
        ///  {  
        ///    "TemplateID":1234,
        ///    "FolderID":0,
        ///    "LayoutName":"Layout Name",
        ///    "ContentSlot1":54321,
        ///    "ContentSlot2":0,
        ///    "ContentSlot3":null,
        ///    "ContentSlot4":0,
        ///    "ContentSlot5":null,
        ///    "ContentSlot6":0,
        ///    "ContentSlot7":null,
        ///    "ContentSlot8":0,
        ///    "ContentSlot9":null,
        ///    "TableOptions":"",
        ///    "DisplayAddress":"321 Happy St, Tunsasmiles, CA 10101",
        ///    "MessageTypeID":123,
        /// }
        /// ]]></example>
        /// 
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 201 Created
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Content-Type: application/json; charset=utf-8
        /// Expires: -1
        /// Location: http://api.ecn5.com/api/message/364566
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Mon, 08 Jun 2015 18:38:17 GMT
        /// Content-Length: 445
        /// 
        /// {  
        ///    "LayoutID":987654,
        ///    "TemplateID":1234,
        ///    "FolderID":0,
        ///    "LayoutName":"Layout Name",
        ///    "ContentSlot1":54321,
        ///    "ContentSlot2":null,
        ///    "ContentSlot3":null,
        ///    "ContentSlot4":null,
        ///    "ContentSlot5":null,
        ///    "ContentSlot6":null,
        ///    "ContentSlot7":null,
        ///    "ContentSlot8":null,
        ///    "ContentSlot9":null,
        ///    "TableOptions":"",
        ///    "DisplayAddress":"321 Happy St, Tunsasmiles, CA 10101",
        ///    "MessageTypeID":123
        ///     }
        /// ]]></example>
        public HttpResponseMessage Post(APIModel model)
        {
            if (model == null)
            {
                RaiseInvalidMessageException(NoModelInRequestBody);
            }

            // 1. cleanse post data
            PostCleanseInputData(model);

            // 2. transform to internal model
            var frameworkModel = Transformer<APIModel, FrameworkModel>.Transform(model, ExposedProperties);

            // 3. fill properties not exposed via API model
            POST_FillFrameworkModelInternalFields(frameworkModel);

            var contentIds = string.Empty;
            foreach (var contentSlot in GetContentSlots(frameworkModel))
            {
                if (contentSlot.Item2 > 0)
                {
                    contentIds = GetContentForSlot(contentSlot, contentIds, true);
                }
            }

            if (!string.IsNullOrWhiteSpace(contentIds))
            {
                RaiseInvalidMessageException($"{ContentId} {contentIds} {IsNotValidated}");
            }

            /*  4. delegate to business layer for validation and persistence...
                ... making sure to update the API model's ID
             */

            Layout.Save(frameworkModel, APIUser);
            var newId = frameworkModel.LayoutID;

            // 5. fetch the newly created object
            var newModel = Get(newId);

            // 6. explicitly create the HTTP response...
            // ...so we can install a Location header pointing to the created item
            return CreateResponseWithLocation(HttpStatusCode.Created, newModel, newId);
        }

        #endregion
        #region PUT

        /// <summary>
        /// Updates resource identified by <pre>id</pre> with the given model object
        /// </summary>
        /// <param name="id">LayoutID for the target resource</param>
        /// <param name="apiModel">Message model object</param>
        /// <returns>On success, returns the given model object as well as a <pre>Location</pre> header.  
        /// In case of validation error(s), 
        /// Error <pre>400 - Bad Rquest</pre> is returned along with a message providing further 
        /// information.</returns>
        /// <example for="request"><![CDATA[
        /// PUT http://api.ecn5.com/api/message/123456 HTTP/1.1
        /// Host: api.ecn5.com
        /// APIAccessKey: <YOUR_API_ACCESS_KEY>
        /// X-Customer-ID: 123
        /// Content-Type: application/json
        /// 
        /// {  
        ///    "TemplateID":1234,
        ///    "FolderID":0,
        ///    "LayoutName":"Layout Name",
        ///    "ContentSlot1":98765,
        ///    "ContentSlot2":12345,
        ///    "ContentSlot3":0,
        ///    "ContentSlot4":null,
        ///    "ContentSlot5":0,
        ///    "ContentSlot6":null,
        ///    "ContentSlot7":0,
        ///    "ContentSlot8":null,
        ///    "ContentSlot9":0,
        ///    "TableOptions":"default",
        ///    "DisplayAddress":"321 Happy St, Tunsasmiles, CA 10101",
        ///    "MessageTypeID":123,
        /// }
        /// ]]></example>
        /// 
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 201 Created
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Content-Type: application/json; charset=utf-8
        /// Expires: -1
        /// Location: http://api.ecn5.com/api/message/123456
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Mon, 08 Jun 2015 18:50:31 GMT
        /// Content-Length: 515
        /// 
        /// {  
        ///    "LayoutID":123456,
        ///    "TemplateID":1234,
        ///    "FolderID":0,
        ///    "LayoutName":"Layout Name",
        ///    "ContentSlot1":98765,
        ///    "ContentSlot2":12345,
        ///    "ContentSlot3":null,
        ///    "ContentSlot4":null,
        ///    "ContentSlot5":null,
        ///    "ContentSlot6":null,
        ///    "ContentSlot7":null,
        ///    "ContentSlot8":null,
        ///    "ContentSlot9":null,
        ///    "UpdatedDate":":"2015-12-30T09:23:08.22",
        ///    "TableOptions":"border=1 bordercolor=black width=600 cellpadding=0 cellspacing=0",
        ///    "DisplayAddress":"321 Happy St, Tunsasmiles, CA 10101",
        ///    "MessageTypeID":123,
        ///    "CreatedDate":":"2015-11-30T09:23:08.22
        /// }
        /// 
        /// 
        /// }
        /// ]]></example>
        public HttpResponseMessage Put(int id, [FromBody]APIModel apiModel)
        {
            if (apiModel == null)
            {
                RaiseInvalidMessageException(NoModelInRequestBody);
            }

            // 1. cleanse input data
            PutCleanseInputData(id, apiModel);

            // 2. GET subject  
            var frameworkModel = Layout.GetByLayoutID(id, APIUser, false);

            // 3. property-wise copy from API Model to subject's current Framework Model
            Transformer<APIModel, FrameworkModel>.Transform(apiModel, frameworkModel, ExposedProperties);

            // 4. fill/update fields existent only for the internal model
            PUT_FillFrameworkModelInternalFields(frameworkModel);
            var contentIds = string.Empty;

            // Validate the HTML content
            foreach (var contentSlot in GetContentSlots(frameworkModel))
            {
                if (contentSlot.Item2 > 0)
                {
                    contentIds = GetContentForSlot(contentSlot, contentIds);
                }
            }
           
            if (!string.IsNullOrWhiteSpace(contentIds))
            {
                RaiseInvalidMessageException($"{ContentId}  {contentIds} {IsNotValidated}");
            } 

            // 5. delegate to Framework for validation and persistence
            Layout.Save(frameworkModel, APIUser);

            // 6. fetch the updated object
            var newModel = Get(id);
            return CreateResponseWithLocation(HttpStatusCode.OK, newModel, id);
        }

        private static IEnumerable<Tuple<int, int>> GetContentSlots(FrameworkModel model)
        {
            var slots = new List<int?>()
            {
                model.ContentSlot1,
                model.ContentSlot2,
                model.ContentSlot3,
                model.ContentSlot4,
                model.ContentSlot5,
                model.ContentSlot6,
                model.ContentSlot7,
                model.ContentSlot8,
                model.ContentSlot9
            };

            for (var slotIndex = 0; slotIndex < slots.Count; slotIndex++)
            {
                if (slots[slotIndex] > 0)
                {
                    yield return new Tuple<int, int>(slotIndex + 1, (int)slots[slotIndex]);
                }
            }
        }

        private string GetContentForSlot(Tuple<int, int> contentSlot, string contentIds, bool checkContent = false)
        {
            var newContent = contentIds;
            var content = GetByContentID_NoAccessCheck(contentSlot.Item2, false);

            if (checkContent && content == null)
            {
                RaiseInvalidMessageException(string.Format(ContentIdForContentSlotDoesnotExist, contentSlot.Item1));
            }

            var isValidated = content.IsValidated ?? false;
            if (!isValidated)
            {
                newContent = !string.IsNullOrWhiteSpace(contentIds)
                                 ? $"{contentIds},{content.ContentID}"
                                 : content.ContentID.ToString();
            }

            return newContent;
        }

        #endregion
        #region DELETE

        /// <summary>
        /// Removes target resource.
        /// </summary>
        /// <param name="id">LayoutID of the target resource</param>
        /// <example for="request"><![CDATA[
        /// DELETE http://api.ecn5.com/api/message/12345 HTTP/1.1
        /// Host: api.ecn5.com
        /// APIAccessKey: <YOUR_API_ACCESS_KEY>
        /// X-Customer-ID: 123
        /// 
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
        /// Date: Fri, 05 Jun 2015 14:50:10 GMT
        /// 
        /// ]]></example>
        // DELETE/api/message/76660 
        public void Delete(int id)
        {
            ECN_Framework_BusinessLayer.Communicator.Layout.Delete(id, APIUser);
        }

        #endregion

        #endregion
        #region Search

        /// <summary>
        /// Provides search capabilities for Message resources. Search supports both GET and POST methods; results will be identical.
        /// </summary>
        /// <see cref="EmailMarketing.API.Models.Message"/>
        /// <param name="searchQuery">Used to constrain the search. You may provide one or more of the criteria shown in the example.</param>
        /// <returns>a list of matching <see cref="EmailMarketing.API.Models.Message"/> as Location/API Object pairs.</returns>
        /// 
        /// <example for="request"><![CDATA[
        /// GET http://api.ecn5.com/api/search/message HTTP/1.1
        /// Content-Type: application/json; charset=utf-8
        /// Accept: application/json
        /// APIAccessKey: <YOUR_API_ACCESS_KEY>
        /// X-Customer-ID: 123
        /// Host: api.ecn5.com
        /// Content-Length: 357
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
        ///         "ValueSet": "0"
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
        ///     {
        ///         "Name": "LastUpdatedByUser",
        ///         "Comparator": "=",
        ///         "ValueSet": "1234"
        ///     },
        ///     {   
        ///         "Name": "Archived",
        ///         "Comparator": "=",
        ///         "ValueSet": "false"
        ///     }
        ///     ]
        ///}
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
        /// Date: Mon, 08 Jun 2015 19:38:07 GMT
        /// Content-Length: 2651
        /// 
        /// [  
        ///    {  
        ///       "ApiObject":{  
        ///          "Archived"="False",
        ///          "LayoutID":-1,
        ///          "TemplateID":1234,
        ///          "FolderID":0,
        ///          "LayoutName":"TestSearch1",
        ///          "ContentSlot1":123456,
        ///          "ContentSlot2":null,
        ///          "ContentSlot3":null,
        ///          "ContentSlot4":null,
        ///          "ContentSlot5":null,
        ///          "ContentSlot6":null,
        ///          "ContentSlot7":null,
        ///          "ContentSlot8":null,
        ///          "ContentSlot9":null,
        ///          "UpdatedDate":null,
        ///          "TableOptions":"",
        ///          "DisplayAddress":"",
        ///          "MessageTypeID":null,
        ///          "CreatedDate":null
        ///       },
        ///       "Location":"http://api.ecn5.com/api/message/123456"
        ///    },
        ///    {  
        ///       "ApiObject":{  
        ///          "Archived=False",
        ///          "LayoutID":-1,
        ///          "TemplateID":2468,
        ///          "FolderID":0,
        ///          "LayoutName":"TestSearch2",
        ///          "ContentSlot1":987654,
        ///          "ContentSlot2":999999,
        ///          "ContentSlot3":888888,
        ///          "ContentSlot4":777777,
        ///          "ContentSlot5":666666,
        ///          "ContentSlot6":555555,
        ///          "ContentSlot7":444444,
        ///          "ContentSlot8":333333,
        ///          "ContentSlot9":222222,
        ///          "UpdatedDate":null,
        ///          "TableOptions":"",
        ///          "DisplayAddress":"123 Sesame St., New York, NY 12345",
        ///          "MessageTypeID":123,
        ///          "CreatedDate":null
        ///       },
        ///       "Location":"http://api.ecn5.com/api/message/987654"
        ///    },
        ///    {  
        ///       "ApiObject":{  
        ///          "LayoutID":-1,
        ///          "TemplateID":0369,
        ///          "FolderID":0,
        ///          "LayoutName":"TestSearch3",
        ///          "ContentSlot1":123456,
        ///          "ContentSlot2":234567,
        ///          "ContentSlot3":345678,
        ///          "ContentSlot4":456789,
        ///          "ContentSlot5":567890,
        ///          "ContentSlot6":678901,
        ///          "ContentSlot7":789012,
        ///          "ContentSlot8":890123,
        ///          "ContentSlot9":901234,
        ///          "UpdatedDate":null,
        ///          "TableOptions":"border=1 bordercolor=black width=600 cellpadding=0 cellspacing=0",
        ///          "DisplayAddress":"9999 99th St. NW, Ricketts, WA 12345",
        ///          "MessageTypeID":987,
        ///          "CreatedDate":null
        ///       },
        ///       "Location":"http://api.ecn5.com/api/message/666666"
        ///    }
        /// ]
        /// ]]></example>

        // Note: The search function fails to populate the query hits with the MessageID b/c apiModel.MessageID = frameworkModel.LayoutID 
        // and the function maps based on variable name.
        [Route("api/Search/Message")]
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
                string title = (string)GetConvertedQueryValue(query, "Title", typeof(string)) ?? "";
                int? folderID = (int?)GetConvertedQueryValue(query, "FolderID", typeof(int));
                int? userID = (int?)GetConvertedQueryValue(query, "LastUpdatedByUser", typeof(int));
                bool? archived = (bool?)GetConvertedQueryValue(query, "Archived", typeof(bool));
                DateTime? updatedDateFrom = (DateTime?)GetConvertedQueryValue(query, "UpdatedDateFrom", typeof(DateTime));
                DateTime? updatedDateTo = (DateTime?)GetConvertedQueryValue(query, "UpdatedDateTo", typeof(DateTime));
                APIUser.CustomerID = APICustomer.CustomerID;
                return ECN_Framework_BusinessLayer.Communicator.Layout.
                    GetByLayoutSearch(title, folderID, userID, updatedDateFrom, updatedDateTo, APIUser, false, archived);
            });
        }

        #endregion
        #region Data Cleansing

        /// <summary>
        /// Data rectification to avoid code breaking errors with POST method
        /// </summary>
        /// <param name="apiModel">The APIModel that is being acted upon by POST</param>
        private void PostCleanseInputData(APIModel apiModel)
        {
            if (apiModel.LayoutID > 0) apiModel.LayoutID = -1;
            else { /* if you have a valid LayoutID, POST will PUT */ }
            GeneralCleanseInputData(apiModel);
        }

        /// <summary>
        /// Data rectification to avoid code breaking errors with PUT method
        /// </summary>
        /// <param name="id">LayoutID of specified layout/message passed through URL.</param>
        /// <param name="apiModel">The APIModel that is being acted upon by PUT</param>
        private void PutCleanseInputData(int id, APIModel apiModel)
        {
            if (apiModel.LayoutID < 1) apiModel.LayoutID = id;
            else { /* if you have no LayoutID or it is invalid, PUT will POST. */ }
            GeneralCleanseInputData(apiModel);
        }

        /// <summary>
        /// Default data assignment to avoid code breaking errors (Intersection of necessary rectification between POST and PUT)
        /// </summary>
        /// <param name="apiModel">The APIModel that is being acted upon by HTTP PUT or HTTP POST</param>
        private void GeneralCleanseInputData(APIModel apiModel)
        {
            if (string.IsNullOrEmpty(apiModel.TableOptions))
            {
                apiModel.TableOptions = "";
            }
            else if (apiModel.TableOptions == "N")
            {
                apiModel.TableOptions = "";
            }
            else
            {
                apiModel.TableOptions = "border=1 bordercolor=black width=600 cellpadding=0 cellspacing=0";
            }
            if (apiModel.MessageTypeID != null && apiModel.MessageTypeID.Value > 0)
            {
                apiModel.MessageTypeID = apiModel.MessageTypeID.Value;
            }
            if (apiModel.ContentSlot1 == 0) apiModel.ContentSlot1 = null;
            if (apiModel.ContentSlot2 == 0) apiModel.ContentSlot2 = null;
            if (apiModel.ContentSlot3 == 0) apiModel.ContentSlot3 = null;
            if (apiModel.ContentSlot4 == 0) apiModel.ContentSlot4 = null;
            if (apiModel.ContentSlot5 == 0) apiModel.ContentSlot5 = null;
            if (apiModel.ContentSlot6 == 0) apiModel.ContentSlot6 = null;
            if (apiModel.ContentSlot7 == 0) apiModel.ContentSlot7 = null;
            if (apiModel.ContentSlot8 == 0) apiModel.ContentSlot8 = null;
            if (apiModel.ContentSlot9 == 0) apiModel.ContentSlot9 = null;

            apiModel.UpdatedUserID = APIUser.UserID;
        }

        /// <summary>
        /// Prepares APIModel from a POST for transformation to a FrameworkModel
        /// </summary>
        /// <param name="frameworkModel">The API Model instance provided for insert via HTTP POST</param>
        private void POST_FillFrameworkModelInternalFields(FrameworkModel frameworkModel)
        {
            CommonFrameworkModelInternalFill(frameworkModel);
        }

        /// <summary>
        /// Prepares APIModel from a PUT for transformation to a FrameworkModel
        /// </summary>
        /// <param name="frameworkModel">The APIModel instance provided for update via HTTP PUT</param>
        private void PUT_FillFrameworkModelInternalFields(FrameworkModel frameworkModel)
        {
            CommonFrameworkModelInternalFill(frameworkModel);   
        }

        private void CommonFrameworkModelInternalFill(FrameworkModel frameworkModel)
        {
            frameworkModel.CreatedUserID = APIUser.UserID;
            frameworkModel.CustomerID = APICustomer.CustomerID;
           
        }
        #endregion
    }
}