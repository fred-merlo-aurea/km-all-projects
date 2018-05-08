using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using EmailMarketing.API.Models.Utility; // for Transformer<inT,outT>
using KM.Common;

using APIModel = EmailMarketing.API.Models.Error;
using FrameworkModel = KM.Common.Entity.ApplicationLog;

namespace EmailMarketing.API.Controllers.Internal
{
    
    using ECN_Framework_DataLayer;
    /// <summary>INTERNAL API methods exposing base-channel</summary>
    [RoutePrefix("api/internal/error")]
    [Route("")]
    public class ErrorController : SearchableApiControllerBase<APIModel,FrameworkModel>
    {
        /// <summary>
        /// Internal Get implementation returns Framework model
        /// </summary>
        /// <param name="id">EmailDirectID</param>
        /// <returns>internal Framework model object</returns>
        /// <remarks>
        /// 
        /// This breaks the pattern of delegating to the business layers as business layer doesn't
        /// support ApplicationLog.
        /// 
        /// Additionally, the Save() method (direct member of KM.Common.Entity.ApplicationLog)
        /// takes a ref to the object to be updated/deleted and sets the LogID of the referenced
        /// object, meaning it doesn't follow the typical business layer pattern.
        /// 
        /// For the moment POST is stubbed to deal directly with the existing logic in KM common
        /// however it might be better to extract the guts of GetInternal (coded here as GetById) out to
        /// the business and data layer methods, and also add typical BL/DL functionality for ApplicationLog.
        /// 
        /// </remarks>
        internal FrameworkModel GetInternal(int id)
        {
            //return ECN_Framework_BusinessLayer.Accounts.Customer.GetByCustomerID(id, getChildren);
            Func<int, FrameworkModel> dataLayerMethod = (logID) =>
            {
                FrameworkModel returnValue = null;
                string sqlQuery = string.Format("SELECT * FROM ApplicationLog WHERE ApplicationID = {0}", logID);
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sqlQuery);
                cmd.CommandType = System.Data.CommandType.Text;

                System.Data.SqlClient.SqlDataReader rdr = DataFunctions.ExecuteReader(cmd, "KMCommon");

                var builder = DynamicBuilder<FrameworkModel>.CreateBuilder(rdr);
                if (rdr.Read())
                {
                    returnValue = builder.Build(rdr);
                }
                cmd.Connection.Close();
                return returnValue;
            };

            return dataLayerMethod(id);
        }
        #region abstract member implementations

        /// <inheritdoc/>
        public override ECN_Framework_Common.Objects.Enums.Entity FrameworkEntity
        {
            get { return ECN_Framework_Common.Objects.Enums.Entity.APILogging; }
        }

        override public string ControllerName { get { return "error"; } }
        override public string[] ExposedProperties
        {
            get { return new string[] { "LogID", "ApplicationID", "SeverityID", "SourceMethod", "Exception", "LogNote" }; }
        }

        public override object GetID(APIModel model)
        {
            return model.LogID;
        }

        public override object GetID(FrameworkModel model)
        {
            return model.LogID;
        }

        #endregion abstract member implementations
        #region Exposed Methods

        /// <summary>
        /// Transform an exception object into a standardized string suitable for logging
        /// </summary>
        /// <param name="e">an Exception to be transformed</param>
        /// <returns>string representation of the given exception</returns>
        /// <example for="request"><![CDATA[
        /// POST http://api.ecn5.com/api/internal/error/format HTTP/1.1
        /// Accept: application/json
        /// Accept-Language: en-US
        /// User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko
        /// Accept-Encoding: gzip, deflate
        /// Connection: Keep-Alive
        /// APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        /// X-Customer-ID: 99999
        /// Host: api.ecn5.com
        /// Content-Type: application/json; charset=utf-8
        /// Content-Length: 1176
        /// 
        /// { "ClassName" : "System.Exception",
        ///   "Data" :
        ///   { 
        ///       "customExceptionField" : "value for custom exception field" 
        ///   },
        ///   "ExceptionMethod" : "8\nFormatException\nEmailMarketing.API, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\nEmailMarketing.API.Controllers.Internal.ErrorController\nSystem.Exception FormatException()",
        ///   "HResult" : -2146233088,
        ///   "HelpURL" : "http://api.ecn5.com
        ///   "InnerException" :
        ///   { 
        ///       "ClassName" : "System.Exception",
        ///       "Data" : null,
        ///       "ExceptionMethod" : null,
        ///       "HResult" : -2146233088,
        ///       "HelpURL" : null,
        ///       "InnerException" : null,
        ///       "Message" : "inner exception",
        ///       "RemoteStackIndex" : 0,
        ///       "RemoteStackTraceString" : null,
        ///       "Source" : null,
        ///       "StackTraceString" : null,
        ///       "WatsonBuckets" : null
        ///    },
        ///   "Message" : "the message",
        ///   "RemoteStackIndex" : 0,
        ///   "RemoteStackTraceString" : null,
        ///   "Source" : "EmailMarketing.API",
        ///   "StackTraceString" : "   at EmailMarketing.API.Controllers.Internal.ErrorController.FormatException() in c:\\Projects\\ECN\\Dev\\2015_Q2\\EmailMarketing.API\\Controllers\\Internal\\ErrorController.cs:line 106",
        ///   "WatsonBuckets" : null
        /// }
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
        /// Date: Wed, 03 Jun 2015 15:10:08 GMT
        /// Content-Length: 801
        /// 
        /// "<table><tr><td><b>**********************</b></td></tr>\r\n<tr><td><b>-- Help Link --</b></td></tr>\r\nhttp://api.ecn5.com Source --</b></td></tr>\r\n<tr><td>EmailMarketing.API</td></tr>\r\n<tr><td><b>-- Data --</b></td></tr>\r\n<tr><td>System.Collections.DictionaryEntry</td></tr>\r\n<tr><td><b>-- Message --</b></td></tr>\r\n<tr><td>the message</td></tr>\r\n<tr><td><b>-- InnerException --</b></td></tr>\r\n<tr><td>System.Exception: inner exception</td></tr>\r\n<tr><td><b>-- Stack Trace --</b></td></tr>\r\n<tr><td>   at EmailMarketing.API.Controllers.Internal.ErrorController.FormatException() in c:\\Projects\\ECN\\Dev\\2015_Q2\\EmailMarketing.API\\Controllers\\Internal\\ErrorController.cs:line 106</td></tr>\r\n<tr><td><b>**********************</b></td></tr></table>\r\n"
        /// ]]></example>
        [HttpPost]
        [Route("format")]
        public string FormatException([FromBody] Exception e)
        {
            return KM.Common.Entity.ApplicationLog.FormatException(e);
        }

        #endregion Exposed Methods
        #region REST

        /// <summary>
        /// This feature is not implemented
        /// </summary>
        public APIModel Get(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Log an Error
        /// </summary>
        /// <param name="apiModel">An Error object to be added to the log</param>
        /// <returns>The provided error object, with LogID filled</returns>
        /// <example for="request"><![CDATA[
        /// POST http://api.ecn5.com/api/internal/error HTTP/1.1
        /// Accept: application/json
        /// Accept-Language: en-US
        /// User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko
        /// Accept-Encoding: gzip, deflate
        /// Connection: Keep-Alive
        /// APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        /// X-Customer-ID: 99999
        /// Host: api.ecn5.com
        /// Content-Type: application/json; charset=utf-8
        /// Content-Length: 945
        /// 
        /// {
        /// "ApplicationID": 99,
        /// "SeverityID": 3,
        /// "SourceMethod": "MyProgram.MyMethod",
        /// "Exception": "<table><tr><td><b>**********************</b></td></tr>\r\n<tr><td><b>-- Help Link --</b></td></tr>\r\nhttp://api.ecn5.com Data --</b></td></tr>\r\n<tr><td>System.Collections.DictionaryEntry</td></tr>\r\n<tr><td><b>-- Message --</b></td></tr>\r\n<tr><td>the message</td></tr>\r\n<tr><td><b>-- InnerException --</b></td></tr>\r\n<tr><td>System.Exception: inner exception</td></tr>\r\n<tr><td><b>-- Stack Trace --</b></td></tr>\r\n<tr><td>   at EmailMarketing.API.Controllers.Internal.ErrorController.FormatException() in c:\\Projects\\ECN\\Dev\\2015_Q2\\EmailMarketing.API\\Controllers\\Internal\\ErrorController.cs:line 106</td></tr>\r\n<tr><td><b>**********************</b></td></tr></table>\r\n",
        /// "LogNote": "submitted by MyApplication via api.ecn5.com"
        /// }
        /// ]]></example>
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 201 Created
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Content-Type: application/json; charset=utf-8
        /// Expires: -1
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Wed, 03 Jun 2015 15:49:22 GMT
        /// Content-Length: 931
        /// {
        ///     "LogID":12345,
        ///     "ApplicationID":99,
        ///     "SeverityID":3,
        ///     "SourceMethod":"MyProgram.MyMethod","Exception":"<table><tr><td><b>**********************</b></td></tr>\r\n<tr><td><b>-- Help Link --</b></td></tr>\r\nhttp://api.ecn5.com Data --</b></td></tr>\r\n<tr><td>System.Collections.DictionaryEntry</td></tr>\r\n<tr><td><b>-- Message --</b></td></tr>\r\n<tr><td>the message</td></tr>\r\n<tr><td><b>-- InnerException --</b></td></tr>\r\n<tr><td>System.Exception: inner exception</td></tr>\r\n<tr><td><b>-- Stack Trace --</b></td></tr>\r\n<tr><td>   at EmailMarketing.API.Controllers.Internal.ErrorController.FormatException() in c:\\Projects\\ECN\\Dev\\2015_Q2\\EmailMarketing.API\\Controllers\\Internal\\ErrorController.cs:line 106</td></tr>\r\n<tr><td><b>**********************</b></td></tr></table>\r\n",
        ///     "LogNote":"submitted by MyApplication via api.ecn5.com"
        /// }
        /// ]]></example>
        public HttpResponseMessage Post([FromBody]APIModel apiModel)
        {
            // 1. pre-process the input external model
            //CleanseInputData(apiModel);

            // 2. transform to internal model
            FrameworkModel frameworkModel = Transform(apiModel);

            // 3. pre-process the internal model (e.g. fill required fields not exposed via external model)
            //POST_FillFrameworkModelInternalFields(frameworkModel);

            // 4. delegate to framework for validation and persistence
            bool wasNotificationSent = FrameworkModel.Save(ref frameworkModel);

            // 5. handle success
            if (frameworkModel.LogID > 0)
            {
                frameworkModel.NotificationSent = wasNotificationSent;
                APIModel newModel = Transform(frameworkModel);
                return Request.CreateResponse(HttpStatusCode.Created, newModel);
            }

            // 6. handle failure
            throw new ECN_Framework_Common.Objects.ECNException(
                new List<ECN_Framework_Common.Objects.ECNError>  
                {
                    new ECN_Framework_Common.Objects.ECNError(
                        ECN_Framework_Common.Objects.Enums.Entity.APILogging,
                        ECN_Framework_Common.Objects.Enums.Method.Create,
                        "failed to create error")
                }, ECN_Framework_Common.Objects.Enums.ExceptionLayer.API);
        }

        /// <summary>
        /// This feature is not implemented
        /// </summary>
        public APIModel Put(int id, [FromBody]APIModel apiModel)
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

        private void POST_FillFrameworkModelInternalFields(FrameworkModel frameworkModel)
        {
            throw new NotImplementedException();
        }

        private void CleanseInputData(APIModel apiModel)
        {
            throw new NotImplementedException();
        }

        #endregion data cleansing
    }
}
