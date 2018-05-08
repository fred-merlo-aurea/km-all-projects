using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using EmailMarketing.API.Models.Utility;

using APIModel = EmailMarketing.API.Models.EmailDirectMessage;
using FrameworkModel = ECN_Framework_Entities.Communicator.EmailDirect;
using EmailMarketing.API.Attributes;
namespace EmailMarketing.API.Controllers.Internal
{
    // aliasing inside namespace where the declarations will then be identical in each controller
    using SearchQuery = List<Models.SearchProperty>;
    using SearchResult = Models.SearchResult<APIModel>;

    /// <summary>INTERNAL API methods exposing direct the email service</summary>
    [RoutePrefix("api/internal/emaildirect")]
    [Route("")]
    public class EmailDirectController : SearchableApiControllerBase<APIModel,FrameworkModel>
    {
        #region abstract member implementations
        /// <inheritdoc/>
        public override ECN_Framework_Common.Objects.Enums.Entity FrameworkEntity
        {
            get { return ECN_Framework_Common.Objects.Enums.Entity.EmailDirect; }
        }

        override public string ControllerName { get { return "emaildirect"; } }
        override public string[] ExposedProperties
        {
            get { return new string[] { "EmailDirectID", "Source", "EmailAddress", "FromName", "FromEmailAddress", "ReplyEmailAddress", "EmailSubject", "Content" }; }
        }

        public override object GetID(APIModel model)
        {
            return model.EmailDirectID;
        }

        public override object GetID(FrameworkModel model)
        {
            return model.EmailDirectID;
        }

        #endregion abstract member implementations
        #region REST
        #region GET
        /// <summary>
        /// This feature is not implemented
        /// </summary>
        public APIModel Get(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Internal Get implementation support POST because no public GET is exposed
        /// </summary>
        /// <param name="id">EmailDirectID</param>
        /// <returns>internal Framework model object</returns>
        internal FrameworkModel GetInternal(int id)
        {
            return ECN_Framework_BusinessLayer.Communicator.EmailDirect.GetByEmailDirectID(id);
        }
        #endregion Get

        /// <summary>
        /// Send a new custom message directly to a single recipient
        /// </summary>
        /// <param name="model">specifications for the new message</param>
        /// <returns>Message information as provided with the EmailDirectID filled-in.</returns>
        /// <example for="request"><![CDATA[
        /// POST http://api.ecn5.com/api/internal/emaildirect HTTP/1.1
        /// Accept: application/json
        /// APIAccessKey: <YOUR_API_ACCESS_KEY>
        /// X-Customer-ID: 99999
        /// Host: api.ecn5.com
        /// Content-Type: application/json; charset=utf-8
        /// Content-Length: 204
        /// 
        /// {
        ///    "Source":"Fiddler",
        ///    "EmailAddress":"karen.jones@example.com",
        ///    "EmailSubject":"A Direct Message",
        ///    "Content":"<html><body><h1>This is a test</h1><h2>this is only a test...</h2></body></html>",
        ///    "ReplyEmailAddress": "john.smith@example.com",
        ///    "FromEmailAddress": "john.smith@example.com",
        ///    "FromName": "John Smith",
        /// }]]></example>
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 201 Created
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Content-Type: application/json; charset=utf-8
        /// Expires: -1
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Mon, 01 Jun 2015 20:29:39 GMT
        /// Content-Length: 207
        /// 
        /// {  
        ///    "EmailDirectID":123,
        ///    "Source":"Fiddler",
        ///    "EmailAddress":"nobody@special.com",
        ///    "EmailSubject":"A Direct Message",
        ///    "Content":"<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN"><html><body><h1>This is a test</h1><h2>this is only a test...</h2></body></html>",
        ///    "FromEmailAddress": "emaildirect@ecn5.com",
        ///    "ReplyEmailAddress":somebody@special.com",
        /// }]]></example>

        [HttpPost]
        //[RaisesInvalidMessageOnModelError]
        public HttpResponseMessage Post(APIModel model)
        {
            //EnsureModelIsValid(model);

            // 1. cleanse post data
            CleanseInputData(model);

            // 2. transform to internal model
            FrameworkModel frameworkModel = Transform(model);

            // 3. fill properties not exposed via API model
            POST_FillFrameworkModelInternalFields(frameworkModel);

            // 4. delegate to business layer for validation and persistence...
            // ... making sure to update the API model's ID
            int newId = ECN_Framework_BusinessLayer.Communicator.EmailDirect.Save(frameworkModel);

            // 5. fetch the newly created object
            APIModel newModel = Transform(GetInternal(newId));

            // 6. explicitly create the HTTP response...
            // ...so we can install a Location header pointing to the created item
            return Request.CreateResponse(HttpStatusCode.Created, newModel);
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
        #region data cleansing 

        /// <summary>
        /// Remove invalid values and fill in anything missing from the external (API) model.
        /// </summary>
        /// <param name="model"></param>
        private void CleanseInputData(APIModel model)
        {
        }

        /// <summary>
        /// Fill in fields of the internal (framework) model that aren't exposed via the external model
        /// </summary>
        /// <param name="frameworkModel"></param>
        private void POST_FillFrameworkModelInternalFields(FrameworkModel frameworkModel)
        {
            frameworkModel.CustomerID = APICustomer.CustomerID;
            frameworkModel.Process = Strings.BusinessDefaultValues.ProcessName;
            frameworkModel.Status = "pending";
            frameworkModel.SendTime = DateTime.Now;
            frameworkModel.CreatedDate = DateTime.Now;
            frameworkModel.CreatedUserID = APIUser.UserID;
        }

        #endregion data cleansing
    }
}
