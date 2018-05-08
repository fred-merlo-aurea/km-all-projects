using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using EmailMarketing.API.Models.Utility; // for Transformer<inT,outT>

    using APIModel = EmailMarketing.API.Models.BaseChannel;
    using FrameworkModel = ECN_Framework_Entities.Accounts.BaseChannel;

namespace EmailMarketing.API.Controllers.Internal
{
    // aliasing inside namespace where the declarations will then be identical in each controller
    using SearchQuery = List<Models.SearchProperty>;
    using SearchResult = Models.SearchResult<APIModel>;

    /// <summary>INTERNAL API methods exposing base-channel</summary>
    [RoutePrefix("api/internal/basechannel")]
    [Route("")]
    public class BaseChannelController : SearchableApiControllerBase<APIModel,FrameworkModel>
    {
        
        /// <summary>
        /// Internal Get implementation returns Framework model
        /// </summary>
        /// <param name="id">EmailDirectID</param>
        /// <returns>internal Framework model object</returns>
        internal static FrameworkModel GetInternal(int id)
        {
            return ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetByBaseChannelID(id);
        }

        #region abstract member implementations

        public override ECN_Framework_Common.Objects.Enums.Entity FrameworkEntity
        {
            get { return ECN_Framework_Common.Objects.Enums.Entity.BaseChannel; }
        }

        override public string ControllerName { get { return "basechannel"; } }
        override public string[] ExposedProperties
        {
            get { return new string[] { "BaseChannelID", "BaseChannelName", "AccessKey" }; }
        }

        public override object GetID(APIModel model)
        {
            return model.BaseChannelID;
        }

        public override object GetID(FrameworkModel model)
        {
            return model.BaseChannelID;
        }

        #endregion abstract member implementations
        #region REST

        /// <summary>
        /// Retrieve the BaseChannel inferred by the CustomerID header
        /// </summary>
        /// <returns>BaseChannel API model object</returns>
        /// <example for="request"><![CDATA[
        /// GET http://api.ecn5.com/api/internal/basechannel HTTP/1.1
        /// Accept: application/json
        /// Accept-Language: en-US
        /// APIAccessKey: <YOUR-API-ACCESS-KEY-HERE>
        /// X-Customer-ID: 99999
        /// Host: api.ecn5.com
        /// 
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
        /// Date: Tue, 02 Jun 2015 15:35:20 GMT
        /// Content-Length: 114
        /// 
        /// {
        ///    "BaseChannelID":77777,
        ///    "BaseChannelName":"My Channel",
        ///    "AccessKey":"00000000-0000-0000-0000-000000000000"
        /// }
        /// ]]></example>
        public APIModel Get()
        {
            if(false == APICustomer.BaseChannelID.HasValue)
            {
                RaiseInternalServerError("API Customer {0} is missing base-channel ID", APICustomer.CustomerID);
            }
            
            int id = APICustomer.BaseChannelID.Value;
            FrameworkModel frameworkObject = GetInternal(id);
            if (null == frameworkObject)
            {
                //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound).EnsureSuccessStatusCode());
                RaiseNotFoundException(id);
            }

            APIModel apiObject = Transform(frameworkObject);
            return apiObject;
        }

        /// <summary>
        /// This feature is not implemented
        /// </summary>
        public APIModel Get(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This feature is not implemented
        /// </summary>
        public APIModel Post([FromBody]APIModel apiModel)
        {
            throw new NotImplementedException();
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
        #region search

        /// <summary>
        /// This feature is not implemented
        /// </summary>
        [Route("~/api/internal/search/basechannel")]
        [HttpGet]
        [HttpPost]
        public IEnumerable<SearchResult> Search([FromBody] SearchQuery searchQuery)
        {
            throw new NotImplementedException();
        }

        #endregion search
        #region data cleansing
        #endregion data cleansing
    }
}
