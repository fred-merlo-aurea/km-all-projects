using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using ecn.common.classes;
using EmailMarketing.API.ExtentionMethods;
using EmailMarketing.API.Models.Reports.Blast;
using APIModel = EmailMarketing.API.Models.SimpleBlastV2;
using BusinessLayerCommunicator = ECN_Framework_BusinessLayer.Communicator;
using FrameworkModel = ECN_Framework_Entities.Communicator.Blast;

namespace EmailMarketing.API.Controllers
{

    using System.Text;
    using EmailMarketing.API.Attributes;
    // aliasing inside name-space where the declarations will then be identical in each controller
    using SearchResult = Models.SearchResult<APIModel>;

    /// <summary>
    /// API methods exposing a simplified version of the Blast object model as Resources for Create, Read, 
    /// Update and Delete via REST, and providing ancillary methods such as for reporting.
    /// </summary>
    [RoutePrefix("api/SimpleBlastV2")]
    [AuthenticationRequired(AccessKey: EmailMarketing.API.Infrastructure.Authentication.AuthenticationProvider.Settings.AccessKeyType.User, RequiredCustomerId: true)]
    [ExceptionsLogged]
    [FriendlyExceptions(CatchUnfilteredExceptions = true)]
    [Logged]
    [RaisesInvalidMessageOnModelError]
    public class SimpleBlastV2Controller : SearchableApiControllerBase<APIModel, FrameworkModel>
    {
        /// <summary>
        /// Constructor, subscribes for AfterTransformation events.
        /// </summary>
        public SimpleBlastV2Controller()
            : base()
        {
            OnAfterTransformation += SimpleBlastController_OnAfterTransformation;
        }

        /// <summary>
        /// static Getter returns API (SimpleBlast) object via fetch of Framework object and explicit transform to APIModel type
        /// </summary>
        /// <param name="blastID">Blast ID</param>
        /// <param name="user">Framework User entity</param>
        /// <param name="fetchFiltersAndSmartSegments">if true, populate Filters and SmartSegments list properties</param>
        /// <returns><code>SimpleBlast</code> object, or null if object not found.</returns>
        /// <exception cref="ECN_Framework_Common.Objects.SecurityException">throw if <code>BlastID</code> exists but is not accessible by <code>user</code>.</exception>
        internal static APIModel InternalGet(int blastID, KMPlatform.Entity.User user, bool fetchFiltersAndSmartSegments = false)
        {
            FrameworkModel blast = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(blastID, user, true) as FrameworkModel;

            if (null == blast)
            {
                return null;
            }

            APIModel apiModel = Models.Utility.Transformer<FrameworkModel, APIModel>.Transform(blast, new APIModel(), SimpleBlastExposedProperties);
            if (1 > apiModel.BlastID)
            {
                return null;
            }
            OnAfterTransformationInternal(TransformationEventArgs.TransformationDirection.ToAPIModel, apiModel, blast, user, fetchFiltersAndSmartSegments);

            return apiModel;
        }

        static readonly string[] SimpleBlastExposedProperties = new string[] {
            "BlastID",
            "EmailSubject", "EmailFrom", "EmailFromName", "ReplyTo", "BlastType",
            "StatusCode", "SendTime", // "TestBlast",
            "LayoutID", "GroupID", "FilterID", "SmartSegmentIDs", "ReferenceBlasts",
            "CreatedUserID", "CreatedDate", "UpdatedUserID", "UpdatedDate","CampaignID","CampaignName","CampaignItemName"
        };

        #region abstract methods implementation

        /// <inheritdoc/>
        public override ECN_Framework_Common.Objects.Enums.Entity FrameworkEntity
        {
            get { return ECN_Framework_Common.Objects.Enums.Entity.Blast; }
        }

        public override string[] ExposedProperties
        {
            get { return SimpleBlastExposedProperties; }
        }

        public override object GetID(APIModel model)
        {
            return model.BlastID; // model.FolderID;
        }

        public override object GetID(FrameworkModel model)
        {
            return model.BlastID; // model.FolderID;
        }

        public override string ControllerName
        {
            get { return "simpleblastV2"; }
        }

        #endregion abstract methods implementation
        #region Transformation helper methods


        void SimpleBlastController_OnAfterTransformation(object sender, SearchableApiControllerBase<APIModel, FrameworkModel>.TransformationEventArgs args)
        {
            if (args != null && args.ApiModel != null && args.FrameworkModel != null)
            {
                OnAfterTransformationInternal(args.Direction, args.ApiModel, args.FrameworkModel, APIUser);
            }
        }

        /// <summary>
        /// Provide transformation customization, available from InternalGet (which explicitly calls this) or via API methods in which case
        /// this method is called from the OnAfterTransformation handler.
        /// </summary>
        /// <param name="direction">For ToAPIModel, sets IsTestBlast in ApiModel from TestBlast in Framework model, 
        /// for ToFramework model, sets TestBlast in Framework model from IsTestBlast in ApiModel</param>
        /// <param name="apiModel">SimpleBlast</param>
        /// <param name="frameworkModel">Blast</param>
        /// <param name="user">Framework User</param>
        /// <param name="fetchFiltersAndSmartSegemnts">if true, when direction is ToAPIModel, also attempt to populate Filter and SmartSegment 
        /// arrays by calling <see cref="ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByBlastID"/></param>
        static void OnAfterTransformationInternal(TransformationEventArgs.TransformationDirection direction, APIModel apiModel, FrameworkModel frameworkModel, KMPlatform.Entity.User user, bool fetchFiltersAndSmartSegemnts = false)
        {
            switch (direction)
            {
                case TransformationEventArgs.TransformationDirection.ToAPIModel:
                    apiModel.IsTestBlast = (frameworkModel.TestBlast ?? "").ToUpper() == "Y";
                    if (fetchFiltersAndSmartSegemnts)
                    {
                        InternalGetFiltersAndSmartSegments(apiModel.BlastID, apiModel, user);
                    }
                    if (apiModel.IsTestBlast)
                    {
                        try
                        {
                            ECN_Framework_Entities.Communicator.CampaignItemTestBlast citb = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.GetByBlastID(apiModel.BlastID, user, false);
                            ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(citb.CampaignItemID.Value, user, false);
                            ECN_Framework_Entities.Communicator.Campaign c = ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCampaignID(ci.CampaignID.Value, user, false);
                            apiModel.CampaignID = c.CampaignID;
                            apiModel.CampaignName = c.CampaignName;
                            apiModel.CampaignItemName = ci.CampaignItemName;
                        }
                        catch { }
                    }
                    else
                    {
                        try
                        {
                            ECN_Framework_Entities.Communicator.Campaign c = ECN_Framework_BusinessLayer.Communicator.Campaign.GetByBlastID(apiModel.BlastID, user, false);
                            apiModel.CampaignID = c.CampaignID;
                            apiModel.CampaignName = c.CampaignName;

                            apiModel.CampaignItemName = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByBlastID(apiModel.BlastID, user, false).CampaignItemName;
                        }
                        catch { }
                    }
                    break;
                case TransformationEventArgs.TransformationDirection.ToFrameworkModel:
                    frameworkModel.TestBlast = apiModel.IsTestBlast ? "Y" : "N";
                    break;
            }
        }

        internal static APIModel InternalGetFiltersAndSmartSegments(int blastID, APIModel apiModel, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Communicator.CampaignItemBlast cib =
                    ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByBlastID(blastID, user, true);
            if (cib == null || cib.Filters == null)
            {
                return apiModel;
            }
            apiModel.Filters = (
                 from x in cib.Filters
                 where x.FilterID != null && x.FilterID.HasValue
                 select x.FilterID.Value
                   ).ToArray();

            apiModel.SmartSegments = (
                 from x in cib.Filters
                 where x.SmartSegmentID != null && x.SmartSegmentID.HasValue
                 select x.SmartSegmentID.Value
                   ).ToArray();
            return apiModel;
        }

        #endregion transformation helper
        #region REST

        #region GET
        /// <summary>Get a blast.  If Email Subject contains Emoji's, they will be in Unicode Escaped Format(\uXXXX)</summary>
        /// <param name="id">Blast ID</param>
        /// <returns>SimpleBlast API model object</returns>
        ///<example for="request"><![CDATA[
        /// GET http://api.ecn5.com/api/simpleblastV2/123456 HTTP/1.1
        /// Accept: application/json
        /// Accept-Language: en-US
        /// User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko
        /// Accept-Encoding: gzip, deflate
        /// Connection: Keep-Alive
        /// APIAccessKey: <YOUR-API-KEY-HERE>
        /// Host: api.ecn5.com
        /// X-Customer-ID: 77777
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
        /// Date: Sun, 26 Jul 2015 18:59:54 GMT
        /// Content-Length: 495
        /// 
        /// {
        ///    "BlastID":123456,
        ///    "StatusCode":"Sent",
        ///    "BlastType":"HTML",
        ///    "Schedule":null,
        ///    "IsTestBlast":true,
        ///    "LayoutID":123456,
        ///    "GroupID":654321,
        ///    "FilterID":0,
        ///    "SmartSegmentID":null,
        ///    "ReferenceBlasts":null,
        ///    "EmailSubject":"Loams Test",
        ///    "EmailFrom":"info@knowledgemarketing.com",
        ///    "EmailFromName":"knowledge marketing",
        ///    "ReplyTo":"info@knowledgemarketing.com",
        ///    "Filters":[
        /// 
        ///    ],
        ///    "SmartSegments":[
        /// 
        ///    ],
        ///    "SendTime":"2015-03-24T08:28:37",
        ///    "CampaignID":123,
        ///    "CampaignName":"Marketing Campaign",
        ///    "CampaignItemName":"Campaign Item Name",
        ///    "CreatedUserID":4496,
        ///    "CreatedDate":"2015-03-24T08:28:20",
        ///    "UpdatedUserID":null,
        ///    "UpdatedDate":null
        /// }
        /// ]]></example>

        [Route("{id}")]
        public APIModel Get(int id)
        {
            APIModel model = InternalGet(id, APIUser);
            if (null == model)
            {
                RaiseNotFoundException(id, "blast");
            }
            return model;
        }
        #endregion GET
        #region POST


        ///<summary>
        ///Create a simple blast
        ///
        /// **When using XML as the Content-Type, use only '&lt;StartDate&gt;Date&lt;/StartDate&gt;' rather than &lt;Schedule&gt;&lt;StartDate&gt;...&lt;/Schedule&gt; unless you are building a ScheduledBlast.
        ///</summary>
        ///<example for="request"><![CDATA[
        ///POST http://api.ecn5.com/api/simpleblastV2 HTTP/1.1
        ///Content-Type: application/json; charset=utf-8
        ///Accept: application/json
        ///APIAccessKey: <YOUR_API_ACCESS_KEY>
        ///X-Customer-ID: 123
        ///Host: api.ecn5.com
        ///Content-Length: 453
        ///
        ///{
        ///  "BlastType": "regular",        
        ///  "Schedule":
        ///  {
        ///    "Type": "OneTime",
        ///    "Recurrence": "OneTime",
        ///    "Split": "Evenly",
        ///    "StartDate": "2015-08-30 12:00:00",
        ///    "EndDate": "2015-08-30 12:00:00",
        ///    "HowManyWeeks": 1,
        ///    "DayOfMonth":3,
        ///    "Day":"Monday",
        ///  },
        ///  "IsTestBlast": false,
        ///  "LayoutID": 654321,
        ///  "GroupID": 123456,
        ///  "CampaignName":"Test Marketing Campaign",
        ///  "CampaignItemName":"Campaign Item Name",
        ///  "EmailSubject": "POSTedBlast",
        ///  "EmailFrom": "info@emailmarketing.com",        
        ///  "EmailFromName": "FromName",
        ///  "ReplyTo": "OmNom@gimme.com",
        ///}]]></example>
        ///
        ///<example for="response"><![CDATA[
        ///HTTP/1.1 201 Created
        ///Cache-Control: no-cache
        ///Pragma: no-cache
        ///Content-Type: application/json; charset=utf-8
        ///Expires: -1
        ///Location: http://api.ecn5.com/api/simpleblastV2/999999
        ///Server: Microsoft-IIS/7.5
        ///X-AspNet-Version: 4.0.30319
        ///X-Powered-By: ASP.NET
        ///Date: Mon, 10 Aug 2015 17:03:22 GMT
        ///Content-Length: 481
        ///
        ///{
        ///    "BlastID":999999,
        ///    "StatusCode":"Pending",
        ///    "BlastType":"HTML",
        ///    "Schedule":null,
        ///    "IsTestBlast":false,
        ///    "LayoutID":654321,
        ///    "GroupID":123456,
        ///    "FilterID":null,
        ///    "SmartSegmentID":null,
        ///    "ReferenceBlasts":null,
        ///    "EmailSubject":"POSTedBlast",
        ///    "EmailFrom":"info@emailmarketing.com",
        ///    "EmailFromName":"FromName",
        ///    "CampaignID": 123,
        ///    "CampaignName":"Test Marketing Campaign",
        ///    "CampaignItemName":"Campaign Item Name",
        ///    "ReplyTo":"OmNom@gimme.com",
        ///    "Filters":null,
        ///    "SmartSegments":null,
        ///    "SendTime":"2015-08-30 12:00:00"
        ///}]]></example>
        [Route("~/api/simpleblastV2")]
        public HttpResponseMessage Post([FromBody]APIModel model)
        {
            ValidateModelState(model); // throw 400 error if the given model isn't valid

            model.EmailSubject = ConvertEmailSubject(model.EmailSubject);

            // validation and smart segment
            CleanseSmartSegment(model);

            if (!SimpleBlastControllerValidation.IsLayoutValid(model.LayoutID))
            {
                RaiseInvalidMessageException(SimpleBlastControllerValidation.InvalidLayoutDefaultErrorMessage);
            }

            if (!SimpleBlastControllerValidation.IsLicenseValid(APICustomer))
            {
                RaiseInvalidMessageException("NO LICENSES AVAILABLE");
            }

            TestBlastCounting(model);

            var campaign = CreateCampaign(model);
            var setupInfo = SetupScheduleOverridingSendTimeIfMissing(model);

            // validate start time
            if (model.SendTime < DateTime.Now.AddSeconds(-30))
            {
                RaiseInvalidMessageException("StartTime/Schedule.StartTime must be in the future");
            }

            //create campaign item
            var campaignItemName = string.Empty;
            var campaignItem = CreateCampaignItem(model, campaign, setupInfo, out campaignItemName);

            var campaignItemBlast = new ECN_Framework_Entities.Communicator.CampaignItemBlast
            {
                CampaignItemID = campaignItem.CampaignItemID,
                CustomerID = CustomerID,
                GroupID = model.GroupID,
                CreatedUserID = APIUser.UserID,
                EmailSubject = model.EmailSubject,
                LayoutID = model.LayoutID
            };
            var campaignItemBlastId = BusinessLayerCommunicator.CampaignItemBlast.Save(campaignItemBlast, APIUser);

            CreateCampaignItemBlastFilter(model, campaignItemBlastId);

            var blast = CreateBlastAndApplyToNewModel(model, campaignItem, campaignItemBlast);
            var newModel = Transform(blast);
            if (newModel == null || newModel.BlastID <= 0)
            {
                RaiseInternalServerError("UNKNOWN ERROR CREATING BLAST");
            }

            newModel.CampaignID = campaign.CampaignID;
            newModel.CampaignItemName = campaignItemName;

            return CreateResponseWithLocation(HttpStatusCode.Created, newModel, newModel.BlastID);
        }

        #endregion POST
        #region PUT
        /// <summary>
        /// Simple blast update
        /// </summary>
        /// <example for="request"><![CDATA[
        ///PUT http://api.ecn5.com/api/simpleblastV2/999999 HTTP/1.1
        ///Content-Type: application/json; charset=utf-8
        ///Accept: application/json
        ///APIAccessKey: <YOUR_API_ACCESS_KEY>
        ///X-Customer-ID: 123
        ///Host: api.ecn5.com
        ///Content-Length: 448
        ///
        ///{
        ///  "BlastType": "regular",
        ///  "Schedule": {
        ///    "Type": "OneTime",
        ///    "Recurrence": "OneTime",
        ///    "Split": "Evenly",        
        ///    "StartDate": "2015-08-30 12:00:00",
        ///    "EndDate": "2015-08-30 12:00:00",
        ///  },
        ///  "IsTestBlast": false,
        ///  "LayoutID": 123456,
        ///  "GroupID": 654321,
        ///  "EmailSubject": "UpdatedSubject",        
        ///  "EmailFrom": "from@email.com",
        ///  "EmailFromName": "FromName",
        ///  "ReplyTo": "replyto@email.com",
        ///}
        ///]]></example>
        /// <example for="response"> <![CDATA[
        /// HTTP/1.1 201 Created
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Content-Type: application/json; charset=utf-8
        /// Expires: -1
        /// Location: http://api.ecn5.com/api/simpleblastV2/999999
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Mon, 10 Aug 2015 17:04:00 GMT
        /// Content-Length: 489
        /// 
        /// {
        ///    "BlastID":999999,
        ///    "StatusCode":"Pending",
        ///    "BlastType":"HTML",
        ///    "Schedule":null,
        ///    "IsTestBlast":false,
        ///    "LayoutID":123456,
        ///    "GroupID":65432,
        ///    "FilterID":null,
        ///    "SmartSegmentID":null,
        ///    "ReferenceBlasts":null,
        ///    "EmailSubject":"UpdatedSubject",
        ///    "EmailFrom":"from@email.com",
        ///    "EmailFromName":"FromName",
        ///    "ReplyTo":"replyto@email.com",
        ///    "Filters":[],
        ///    "SmartSegments":[],
        ///    "SendTime":"2015-08-10T12:04:00",
        ///    "CreatedUserID":1234,
        ///    "CreatedDate":"2015-08-10T12:03:23",
        ///    "UpdatedUserID":4321,
        ///    "UpdatedDate":"2015-08-15T12:04:01"
        /// }
        /// ]]></example>

        [Route("{id}")]
        public HttpResponseMessage Put(int id, [FromBody]APIModel model)
        {
            var campaignItemBlast = BusinessLayerCommunicator.CampaignItemBlast.GetByBlastID(id, APIUser, true);
            if (campaignItemBlast?.BlastID == null)
            {
                RaiseNotFoundException(id, "blast");
            }

            var blast = BusinessLayerCommunicator.Blast.GetByBlastID(id, APIUser, true);
            if (null == blast)
            {
                RaiseNotFoundException(id, "blast");
            }

            ValidateModelState(model); // throw 400 error if the given model isn't valid

            model.EmailSubject = ConvertEmailSubject(model.EmailSubject);

            Transform(model, blast); // copy properties from input model to framework model seeded with current values

            CleanseSmartSegment(model);

            if (!SimpleBlastControllerValidation.IsLayoutValid(model.LayoutID))
            {
                RaiseInvalidMessageException(SimpleBlastControllerValidation.InvalidLayoutDefaultErrorMessage);
            }

            var setupInfo = SetupSchedule(model);

            var campaignItem = GetUpdatedCampaignItem(model, campaignItemBlast.CampaignItemID.Value, setupInfo);
            UpdateCampaignItemBlast(model, blast, campaignItem, campaignItemBlast);

            //update actual blast
            BusinessLayerCommunicator.Blast.CreateBlastsFromCampaignItem(campaignItem.CampaignItemID, APIUser);

            var newModel = InternalGet(id, APIUser, true);
            return CreateResponseWithLocation(HttpStatusCode.OK, newModel, newModel.BlastID);
        }

        #endregion PUT
        #region DELETE
        /// <summary>
        /// Simple blast removal
        /// </summary>       
        /// <example for="request"><![CDATA[
        /// DELETE http://api.ecn5.com/api/SimpleBlastV2/999999 HTTP/1.1
        /// Accept: application/json
        /// APIAccessKey: <YOUR_API_ACCESS_KEY>
        /// X-Customer-ID: 123
        /// Host: api.ecn5.com
        /// ]]></example>
        /// <example for="response"><![CDATA[
        /// HTTP/1.1 204 No Content
        /// Cache-Control: no-cache
        /// Pragma: no-cache
        /// Expires: -1
        /// Server: Microsoft-IIS/7.5
        /// X-AspNet-Version: 4.0.30319
        /// X-Powered-By: ASP.NET
        /// Date: Fri, 31 Jul 2015 16:44:15 GMT
        /// ]]></example>
        [Route("{id}")]
        public void Delete(int id)
        {
            ECN_Framework_Entities.Communicator.CampaignItemBlast ciBlast = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.GetByBlastID(id, APIUser, false);
            ECN_Framework_Entities.Communicator.CampaignItemTestBlast ciTestBlast = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.GetByBlastID(id, APIUser, false);
            if (ciBlast != null)
            {
                ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastRefBlast.Delete(ciBlast.CampaignItemBlastID, APIUser);
                ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.Delete(ciBlast.CampaignItemID.Value, ciBlast.CampaignItemBlastID, APIUser);
            }
            else if (ciTestBlast != null)
            {
                ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.Delete(ciTestBlast.CampaignItemID.Value, ciTestBlast.CampaignItemTestBlastID, APIUser);
            }
            else
            {
                RaiseNotFoundException(id, "blast");
            }
        }
        #endregion DELETE

        #endregion REST
        #region Search

        /// <summary>
        /// Search by EmailSubject, GroupID, IsTest, StatusCode string, CampaignID, CampaignName, CampaignItemName, and/or ModifiedTo and/or ModifiedFrom dates.
        /// </summary>
        /// <example for="request"><![CDATA[
        /// GET http://api.ecn5.com/api/search/SimpleBlastV2 HTTP/1.1
        /// Accept: application/json
        /// Content-Type: application/json
        /// APIAccessKey: <YOUR_API_ACCESS_KEY>
        /// X-Customer-ID: 123
        /// Host: api.ecn5.com
        /// 
        ///{
        ///     "SearchCriteria": [
        ///     {
        ///         "Name": "EmailSubject",
        ///         "Comparator": "contains",
        ///         "ValueSet": "test"
        ///     },
        ///     {
        ///         "Name": "GroupID",
        ///         "Comparator": "=",
        ///         "ValueSet": "12345"
        ///     },
        ///     {
        ///         "Name": "IsTest",
        ///         "Comparator": "=",
        ///         "ValueSet": "false"
        ///     },
        ///     {
        ///         "Name": "StatusCode",
        ///         "Comparator": "=",
        ///         "ValueSet": "Sent"
        ///     },
        ///     {
        ///         "Name": "ModifiedTo",
        ///         "Comparator": "<=",
        ///         "ValueSet": "2015-09-01"
        ///     },
        ///     {
        ///         "Name": "ModifiedFrom",
        ///         "Comparator": ">=",
        ///         "ValueSet": "2015-01-01"
        ///     },
        ///     {
        ///         "Name": "CampaignID",
        ///         "Comparator": "=",
        ///         "ValueSet": "123"
        ///     },
        ///     {
        ///         "Name": "CampaignName",
        ///         "Comparator": "=",
        ///         "ValueSet":"Marketing Campaign"
        ///     },
        ///     {
        ///         "Name": "CampaignItemName",
        ///         "Comparator": "=",
        ///         "ValueSet":"Campaign Item Name"
        ///     }
        ///     ] 
        ///}
        /// ]]></example>
        /// <example for="response"><![CDATA[
        ///HTTP/1.1 200 OK
        ///Cache-Control: no-cache
        ///Pragma: no-cache
        ///Content-Type: application/json; charset=utf-8
        ///Expires: -1
        ///Server: Microsoft-IIS/7.5
        ///X-AspNet-Version: 4.0.30319
        ///X-Powered-By: ASP.NET
        ///Date: Mon, 10 Aug 2015 18:16:45 GMT
        ///Content-Length: 1765
        ///
        ///[
        ///   {
        ///   "ApiObject":
        ///      {
        ///         "BlastID":999999,
        ///         "StatusCode":"Sent",
        ///         "BlastType":"HTML",
        ///         "Schedule":null,
        ///         "IsTestBlast":false,
        ///         "LayoutID":888888,
        ///         "GroupID":12345,
        ///         "FilterID":null,
        ///         "SmartSegmentID":null,
        ///         "ReferenceBlasts":null,
        ///         "EmailSubject":"TestBlast1",
        ///         "EmailFrom":"from1@email.com",
        ///         "EmailFromName":"Knowledge Marketing",
        ///         "ReplyTo":"reply1@email.com",
        ///         "CampaignID":123,
        ///         "CampaignName":"Marketing Campaign",
        ///         "CampaignItemName":"Campaign Item Name",
        ///         "Filters":null,
        ///         "SmartSegments":null,
        ///         "SendTime":"2015-07-02T15:36:41",
        ///         "CreatedUserID":4444,
        ///         "CreatedDate":"2015-07-02T15:36:25",
        ///         "UpdatedUserID":null,
        ///         "UpdatedDate":null
        ///      },
        ///      "Location":"http://test.api.ecn5.com/api/simpleblast/999999"
        ///   },
        ///   {
        ///   "ApiObject":
        ///      {
        ///         "BlastID":111111,
        ///         "StatusCode":"Sent",
        ///         "BlastType":"HTML",
        ///         "Schedule":null,
        ///         "IsTestBlast":false,
        ///         "LayoutID":222222,
        ///         "GroupID":12345,
        ///         "FilterID":null,
        ///         "SmartSegmentID":null,
        ///         "ReferenceBlasts":null,
        ///         "EmailSubject":"TestBlast2",
        ///         "EmailFrom":"from2@email.com",
        ///         "EmailFromName":"Knowledge Marketing",
        ///         "ReplyTo":"reply@email.com",
        ///         "CampaignID":123,
        ///         "CampaignName":"Marketing Campaign",
        ///         "CampaignItemName":"Campaign Item Name",
        ///         "Filters":null,
        ///         "SmartSegments":null,
        ///         "SendTime":"2015-07-23T14:24:00",
        ///         "CreatedUserID":4444,
        ///         "CreatedDate":"2015-07-23T14:23:44",
        ///         "UpdatedUserID":null,
        ///         "UpdatedDate":null
        ///      },
        ///      "Location":"http://test.api.ecn5.com/api/simpleblast/111111"
        ///   }
        ///]
        ///]]></example>
        [Route("~/api/search/SimpleBlastV2")]
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
                string emailSubject = (string)GetConvertedQueryValue(query, "EmailSubject", typeof(string)) ?? "";

                emailSubject = ConvertEmailSubject(emailSubject);

                int? groupID = (int?)GetConvertedQueryValue(query, "GroupID", typeof(int));
                bool? isTest = (bool?)GetConvertedQueryValue(query, "IsTest", typeof(bool));
                string statusCode = (string)GetConvertedQueryValue(query, "StatusCode", typeof(string)) ?? "";
                DateTime? modifiedFrom = (DateTime?)GetConvertedQueryValue(query, "ModifiedFrom", typeof(DateTime));
                DateTime? modifiedTo = (DateTime?)GetConvertedQueryValue(query, "ModifiedTo", typeof(DateTime));
                int? CampaignID = (int?)GetConvertedQueryValue(query, "CampaignID", typeof(int));
                string CampaignName = (string)GetConvertedQueryValue(query, "CampaignName", typeof(string)) ?? "";
                string CampaignItemName = (string)GetConvertedQueryValue(query, "CampaignItemName", typeof(string)) ?? "";

                return from x in ECN_Framework_BusinessLayer.Communicator.Blast.
                    GetBySearch(CustomerID, emailSubject, null, groupID, isTest, statusCode, modifiedFrom, modifiedTo, CampaignID, CampaignName, CampaignItemName, APIUser, false)
                       where x is ECN_Framework_Entities.Communicator.BlastRegular
                       select x as ECN_Framework_Entities.Communicator.BlastRegular;
            });
        }

        #endregion search
        #region exposed methods

        #region Get Blast Bounce Report - GetBlastBounceReport()

        /// <summary>
        /// Simple Report of Blast Bounces.  
        /// </summary>
        /// <param name="id">Blast ID</param>
        /// <param name="withDetail">pass true to get a report including detail</param>
        /// <returns>data table</returns>
        /// <example for="request"><![CDATA[
        /// GET http://api.ecn5.com/api/simpleblastV2/999999/Report/Bounces?withDetail=false HTTP/1.1
        /// Accept: application/json
        /// Accept-Language: en-US
        /// User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko
        /// Accept-Encoding: gzip, deflate
        /// Connection: Keep-Alive
        /// APIAccessKey: <YOUR_API_ACCESS_KEY>
        /// X-Customer-ID: 123
        /// Host: api.ecn5.com
        /// ]]></example>
        /// 
        /// <example for="response"><![CDATA[
        ///HTTP/1.1 200 OK
        ///Cache-Control: no-cache
        ///Pragma: no-cache
        ///Content-Type: application/json; charset=utf-8
        ///Expires: -1
        ///Server: Microsoft-IIS/7.5
        ///X-AspNet-Version: 4.0.30319
        ///X-Powered-By: ASP.NET
        ///Date: Mon, 10 Aug 2015 19:07:17 GMT
        ///Content-Length: 94368
        ///
        ///[
        ///   {
        ///      "BounceTime":"6/21/2015 12:49:07 PM",
        ///      "EmailAddress":"email1@email.com",
        ///      "BounceType":"softbounce",
        ///      "BounceSignature":"unable to connect"
        ///   },
        ///   {
        ///      "BounceTime":"6/17/2015 10:20:52 PM",
        ///      "EmailAddress":"email2@email.com",
        ///      "BounceType":"blocks",
        ///      "BounceSignature":"smtp; 554 5.7.1 email2@email.com: relay access denied"
        ///   },
        ///   {
        ///      "BounceTime":"6/17/2015 9:47:16 PM",
        ///      "EmailAddress":"email3@email.com",
        ///      "BounceType":"hardbounce",
        ///      "BounceSignature":"smtp;400 4.4.7 message delayed"
        ///   }
        ///]]></example>
        [Route("{id}/Report/Bounces")]
        public IEnumerable<BounceReport> GetBlastBounceReport(int id, bool withDetail = false)
        {
            if (false == ECN_Framework_BusinessLayer.Communicator.Blast.Exists(id, CustomerID))
            {
                RaiseNotFoundException(id, "blast");
            }

            string[] fieldsToUse = new string[] { "BounceTime", "EmailAddress", "BounceType", "BounceSignature" };
            DataTable dt = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetBlastEmails(id, "bounce", "", APIUser);
            dt.TableName = "Report";
            if (!withDetail)
            {
                DataView view = new DataView(dt);
                dt = view.ToTable(true, fieldsToUse);
            }

            return dt.ToListOfBounceReports();
        }

        #endregion Get Blast Bounce Report - GetBlastBounceReport()
        #region Get Blast Clicks Report - GetBlastClicksReport()
        /// <summary>
        /// Simple Report of Blast Clicks.  
        /// </summary>
        /// <param name="id">Blast ID</param>
        /// <param name="filterType">All or Unique</param>
        /// <param name="withDetail">pass true to get a report including detail</param>
        /// <returns></returns>
        /// <example for="request"><![CDATA[
        /// GET http://api.ecn5.com/api/SimpleBlastV2/99999/Report/Clicks?filterType=0&withDetail=false HTTP/1.1
        /// Accept: application/json
        /// APIAccessKey: <YOUR_API_ACCESS_KEY>
        /// X-Customer-ID: 123
        /// Host: api.ecn5.com
        /// 
        /// ]]></example>
        /// <example for="response"><![CDATA[
        ///HTTP/1.1 200 OK
        ///Cache-Control: no-cache
        ///Pragma: no-cache
        ///Content-Type: application/json; charset=utf-8
        ///Expires: -1
        ///Server: Microsoft-IIS/7.5
        ///X-AspNet-Version: 4.0.30319
        ///X-Powered-By: ASP.NET
        ///Date: Mon, 10 Aug 2015 18:43:44 GMT
        ///Content-Length: 539
        ///
        ///[
        ///   {
        ///      "ClickTime":"7/17/2015 8:59:55 AM",
        ///      "EmailAddress":"emailaddress1@email.com",
        ///      "Link":"http://www.knowledgemarketing.com"
        ///   },
        ///   {
        ///      "ClickTime":"7/16/2015 11:44:41 AM",
        ///      "EmailAddress":"emailaddress1@email.com",
        ///      "Link":"http://www.kmlearningcenter.com"
        ///   },
        ///   {
        ///      "ClickTime":"7/13/2015 12:35:58 PM",
        ///      "EmailAddress":"emailaddress2@email.com",
        ///      "Link":"http://www.kmlearningcenter.com"
        ///   }
        ///]
        /// ]]></example>
        [Route("{id}/Report/Clicks")]
        [HttpGet]
        [Attributes.FriendlyExceptions]
        public IEnumerable<ClickReport> GetBlastClicksReport(int id, Models.Reports.ClickReportFilterType filterType = Models.Reports.ClickReportFilterType.All, bool withDetail = false)
        {
            if (false == ECN_Framework_BusinessLayer.Communicator.Blast.Exists(id, CustomerID))
            {
                //throw new FileNotFoundException("no blast with id " + id);
                //RaiseNotFoundException(id, "blast");
                throw new HttpResponseException(Request.CreateErrorResponse(
                    HttpStatusCode.NotFound,
                    Exceptions.APIResourceNotFoundException.GetAPIResourceNotFoundExceptionMessage(
                        Exceptions.APIResourceNotFoundException.Factory("blast", id))));
                //throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            string[] fieldsToUse = new string[] { "ClickTime", "EmailAddress", "Link" };
            DataTable dt = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetBlastEmails(id, "click", filterType.ToString(), APIUser);
            dt.TableName = "Report";
            if (!withDetail)
            {
                DataView view = new DataView(dt);
                dt = view.ToTable(true, fieldsToUse);
            }
            return dt.ToListOfClickReports();
        }
        #endregion Get Blast Clicks Report - GetBlastClicksReport()
        #region Get Blast Delivery Report - GetBlastDeliveryReport()

        /// <summary>
        /// Simple Report of Blast Delivery detail.  
        /// </summary>
        /// <param name="id">Blast ID</param>
        /// <param name="fromDate">required, inclusive date after which the blast must have been created/updated to be included in the report</param>
        /// <param name="toDate">required, inclusive date before which the blast must have been created/updated to be included in the report</param>
        /// <returns>data table</returns>
        /// <example for="request"><![CDATA[
        ///GET http://api.ecn5.com/api/simpleblastV2/999999/Report/Delivery?fromDate=2015-07-13&toDate=2015-07-30 HTTP/1.1
        ///Accept: application/json
        ///Accept-Language: en-US
        ///User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko
        ///Accept-Encoding: gzip, deflate
        ///Connection: Keep-Alive
        ///APIAccessKey: <YOUR_API_ACCESS_KEY>
        ///X-Customer-ID: 123
        ///Host: api.ecn5.com
        /// ]]></example>
        /// <example for="response"><![CDATA[
        ///HTTP/1.1 200 OK
        ///Cache-Control: no-cache
        ///Pragma: no-cache
        ///Content-Type: application/json; charset=utf-8
        ///Expires: -1
        ///Server: Microsoft-IIS/7.5
        ///X-AspNet-Version: 4.0.30319
        ///X-Powered-By: ASP.NET
        ///Date: Fri, 31 Jul 2015 18:44:12 GMT
        ///Content-Length: 1268
        ///
        ///[
        ///   {
        ///      "blastID":999999,
        ///      "sendtime":"2015-07-29T12:06:13.413",
        ///      "emailsubject":"EmailSubject",
        ///      "groupname":"GroupName",
        ///      "FilterName":"",
        ///      "CampaignName":"CampName",
        ///      "sendtotal":129,
        ///      "Delivered":110,
        ///      "softbouncetotal":9,
        ///      "hardbouncetotal":5,
        ///      "OtherBouncetotal":5,
        ///      "bouncetotal":19,
        ///      "UniqueOpens":105,
        ///      "TotalOpens":153,
        ///      "UniqueClicks":52,
        ///      "TotalClicks":79,
        ///      "UnsubscribeTotal":0,
        ///      "suppressedtotal":0,
        ///      "MobileOpens":12,
        ///      "FromEmail":"from@email.com",
        ///      "CampaignItemName":"CampItemName",
        ///      "CustomerName":"CustomerName",
        ///      "Field1":null,
        ///      "Field2":null,
        ///      "Field3":null,
        ///      "Field4":null,
        ///      "Field5":null,
        ///      "Abuse":0,
        ///      "Feedback":0,
        ///      "SpamCount":0
        ///   },
        ///]
        /// ]]></example>
        [Route("{id}/Report/Delivery")]
        public IEnumerable<DeliveryReport> GetBlastDeliveryReport(DateTime fromDate, DateTime toDate)
        {

            DataTable dt = ECN_Framework_BusinessLayer.Activity.Report.BlastDelivery.Get(CustomerID.ToString(), fromDate, toDate);
            dt.TableName = "Report";
            return dt.ToListOfDeliveryReports();
        }

        #endregion Get Blast Delivery Report - GetBlastDeliveryReport()
        #region Get Blast Opens Report - GetBlastOpensReport()

        /// <summary>
        /// Simple Report of Blast Opens.  
        /// </summary>
        /// <param name="id">Blast ID</param>
        /// <param name="filterType">All or Unique</param>
        /// <param name="withDetail">pass true to get a report including detail</param>
        /// <returns></returns>
        /// <example for="request"><![CDATA[
        ///GET http://api.ecn5.com/api/simpleblastV2/999999/Report/Opens?filterType=1&withDetail=false HTTP/1.1
        ///Accept: application/json
        ///Accept-Language: en-US
        ///User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko
        ///Accept-Encoding: gzip, deflate
        ///Connection: Keep-Alive
        ///APIAccessKey: <YOUR_API_ACCESS_KEY>
        ///X-Customer-ID: 123
        ///Host: api.ecn5.com
        /// ]]></example>
        /// 
        /// <example for="response"><![CDATA[
        ///HTTP/1.1 200 OK
        ///Cache-Control: no-cache
        ///Pragma: no-cache
        ///Content-Type: application/json; charset=utf-8
        ///Expires: -1
        ///Server: Microsoft-IIS/7.5
        ///X-AspNet-Version: 4.0.30319
        ///X-Powered-By: ASP.NET
        ///Date: Mon, 10 Aug 2015 18:57:16 GMT
        ///Content-Length: 335
        ///[
        ///   {
        ///      "OpenTime":"8/7/2015 1:54:26 PM",
        ///      "EmailAddress":"emailaddress@email.com",
        ///      "Info":"70.89.199.237 | Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/7.0; SLCC2; .NET CLR 2.0.50727;
        ///      .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; Microsoft Outlook 15.0.4737; ms-office; MSOffice 15)"
        ///   }
        ///]
        /// ]]></example>

        [Route("{id}/Report/Opens")]
        public IEnumerable<OpensReport> GetBlastOpensReport(int id, Models.Reports.ClickReportFilterType filterType = Models.Reports.ClickReportFilterType.All, bool withDetail = false)
        {
            if (false == ECN_Framework_BusinessLayer.Communicator.Blast.Exists(id, CustomerID))
            {
                RaiseNotFoundException(id, "blast");
            }

            DataTable dt = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetBlastEmails(id, "open", filterType.ToString(), APIUser);
            dt.TableName = "Report";
            string[] fieldsToUse = new string[] { "OpenTime", "EmailAddress", "Info" };
            if (!withDetail)
            {
                DataView view = new DataView(dt);
                dt = view.ToTable(true, fieldsToUse);
            }
            return dt.ToListOfOpensReports();
        }

        #endregion Get Blast Opens Report - GetBlastOpensReport()
        #region Get Blast Report

        /// <summary>
        /// Simple blast report given BlastID.  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <example for="request"><![CDATA[
        /// GET http://api.ecn5.com/api/simpleblastV2/1234567/Report HTTP/1.1
        /// Accept: application/json
        /// Accept-Language: en-US
        /// User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko
        /// Accept-Encoding: gzip, deflate
        /// Connection: Keep-Alive
        /// APIAccessKey: <YOUR-API-KEY-HERE>
        /// Host: api.ecn5.com
        /// X-Customer-ID: 77777
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
        /// Date: Sun, 26 Jul 2015 15:48:35 GMT
        /// Content-Length: 519
        /// {
        ///    "Sends":{
        ///       "RowType":"Sends",
        ///       "DistinctCount":11,
        ///       "TotalCount":11,
        ///       "UniquePercent":100.0,
        ///       "TotalPercent":100.0
        ///    },
        ///    "Opens":{
        ///       "RowType":"Opens",
        ///       "DistinctCount":5,
        ///       "TotalCount":9,
        ///       "UniquePercent":45.0,
        ///       "TotalPercent":81.0
        ///    },
        ///    "Bounces":{
        ///       "RowType":"Bounces",
        ///       "DistinctCount":0,
        ///       "TotalCount":0,
        ///       "UniquePercent":0.0,
        ///       "TotalPercent":0.0
        ///    },
        ///    "Resends":{
        ///       "RowType":"Resends",
        ///       "DistinctCount":0,
        ///       "TotalCount":0,
        ///       "UniquePercent":0.0,
        ///       "TotalPercent":0.0
        ///    },
        ///    "Clicks":{
        ///       "RowType":"Clicks",
        ///       "DistinctCount":5,
        ///       "TotalCount":7,
        ///       "UniquePercent":71.4,
        ///       "TotalPercent":63.0
        ///    }
        /// }
        /// ]]></example>
        /// 
        [Route("{id}/Report")]
        public Models.Reports.Blast.BlastReport GetBlastReport(int id)
        {
            DataTable dt = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetBlastReport(id, "", "", APIUser);
            if (dt == null)
            {
                RaiseNotFoundException(id, "blast");
            }
            dt.TableName = "BlastReport";

            Models.Reports.Blast.BlastReport blastReport = new Models.Reports.Blast.BlastReport();

            Models.Reports.Blast.ReportRow sendReportRow =
               (from row in dt.AsEnumerable()
                where row["ActionTypeCode"].ToString().ToLower().Equals("send")
                select MakeReportRow(blastReport, row)).FirstOrDefault();

            if (sendReportRow != null)
            {
                blastReport.Sends = sendReportRow;
            }

            foreach (DataRow row in dt.Rows)
            {
                switch (row["ActionTypeCode"].ToString().ToLower())
                {
                    case "open":
                        blastReport.Opens = MakeReportRow(blastReport, row);
                        break;
                    case "click":
                        blastReport.Clicks = MakeReportRow(blastReport, row);
                        break;
                    case "bounce":
                        blastReport.Bounces = MakeReportRow(blastReport, row);
                        break;
                    case "resend":
                        blastReport.Resends = MakeReportRow(blastReport, row);
                        break;
                }
            }

            return blastReport;
        }
        Models.Reports.Blast.ReportRow MakeReportRow(Models.Reports.Blast.BlastReport report, DataRow row)
        {
            Models.Reports.Blast.ReportRow reportRow = new Models.Reports.Blast.ReportRow
            {
                RowType = Models.Reports.Blast.Report.ActionTypeCodeToRowType(row["ActionTypeCode"].ToString()),
                DistinctCount = row["DistinctCount"] != DBNull.Value ? Convert.ToInt32(row["DistinctCount"]) : 0,
                TotalCount = row["total"] != DBNull.Value ? Convert.ToInt32(row["total"]) : 0
            };

            switch (reportRow.RowType)
            {
                case Models.Reports.Blast.ReportRowType.Sends:
                    reportRow.UniquePercent = 100;
                    reportRow.TotalPercent = 100;
                    break;
                case Models.Reports.Blast.ReportRowType.Opens:
                case Models.Reports.Blast.ReportRowType.Clicks:
                    if (report.HasSends && report.Sends.DistinctCount > 0 && reportRow.DistinctCount > 0)
                    {
                        reportRow.UniquePercent = reportRow.DistinctCount * 100 / report.Sends.DistinctCount;
                    }
                    if (report.HasSends && report.Sends.TotalCount > 0 && reportRow.TotalCount > 0)
                    {
                        reportRow.TotalPercent = reportRow.TotalCount * 100 / report.Sends.TotalCount;
                    }
                    break;
            }

            return reportRow;
        }

        #endregion Get Blast Report
        #region Get Blast Report By ISP - GetBlastReportByISP()

        /// <summary>
        /// Simple Report of Blast detail by ISP.  
        /// </summary>
        /// <param name="id">Blast ID</param>
        /// <param name="ISPList">list of ISPs, as strings</param>
        /// <returns>data table</returns>
        /// <example for="request"><![CDATA[
        /// POST http://api.ecn5.com/api/simpleblastV2/999999/Report/ISP HTTP/1.1
        /// Accept: application/json
        /// Accept-Language: en-US
        /// Content-Type: application/json
        /// User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko
        /// Accept-Encoding: gzip, deflate
        /// Connection: Keep-Alive
        /// APIAccessKey: <YOUR_API_ACCESS_KEY>
        /// X-Customer-ID: 123
        /// Host: api.ecn5.com
        /// Content-Length: 73
        /// 
        /// [
        ///    "Comcast",
        ///    "Verizon",
        ///    "Frontier",
        /// ]
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
        /// Date: Mon, 10 Aug 2015 19:13:15 GMT
        /// Content-Length: 704
        /// 
        /// [
        ///    {
        ///       "ISPs":"Comcast",
        ///       "Sends":"0",
        ///       "opens":"0",
        ///       "clicks":"0",
        ///       "bounces":"0",
        ///       "unsubscribes":"0",
        ///       "resends":"0",
        ///       "forwards":"0",
        ///       "feedbackUnsubs":"0"
        ///    },
        ///    {
        ///       "ISPs":"Frontier",
        ///       "Sends":"0",
        ///       "opens":"0",
        ///       "clicks":"0",
        ///       "bounces":"0",
        ///       "unsubscribes":"0",
        ///       "resends":"0",
        ///       "forwards":"0"
        ///       ,"feedbackUnsubs":"0"
        ///    },
        ///    {
        ///       "ISPs":"Verizon",
        ///       "Sends":"0",
        ///       "opens":"0",
        ///       "clicks":"0",
        ///       "bounces":"0",
        ///       "unsubscribes":"0",
        ///       "resends":"0",
        ///       "forwards":"0",
        ///       "feedbackUnsubs":"0"
        ///    }
        /// ]
        /// ]]></example>
        [Route("{id}/report/ISP")]
        [HttpPost]
        public IEnumerable<ISPReport> GetBlastReportByISP(int id, [FromBody] string[] ISPList)
        {
            if (null == ISPList || false == ISPList.Any(s => false == String.IsNullOrWhiteSpace(s)))
            {
                RaiseInvalidMessageException("ISPList may not be empty");
            }

            string resultsXML = string.Empty;
            //string ISPs = BuildISPString(XMLSearch);
            DataTable dt = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetISPReport(id, String.Join(",", from isp in ISPList where false == String.IsNullOrWhiteSpace(isp) select isp));
            if (dt == null)
            {
                RaiseNotFoundException(id, "blast");
            }
            return dt.ToListOfISPReports();
        }
        #endregion Get Blast Report By ISP - GetBlastReportByISP()
        #region Get Blast Unsubscribe Report - GetBlastUnsubscribeReport()

        /// <summary>
        /// Report blast unsubscribe activity.  
        /// </summary>
        /// <param name="id">BlastID</param>
        /// <returns>a data-table, as JSON or XML per Content-Accept header.</returns>
        /// <example for="request"><![CDATA[
        /// GET http://api.ecn5.com/api/simpleblastV2/999999/Report/Unsubscribe HTTP/1.1
        /// Accept: application/json
        /// Accept-Language: en-US
        /// User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko
        /// Accept-Encoding: gzip, deflate
        /// Connection: Keep-Alive
        /// APIAccessKey: <YOUR_API_ACCESS_KEY>
        /// X-Customer-ID: 123
        /// Host: api.ecn5.com
        /// 
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
        /// Date: Mon, 10 Aug 2015 19:21:46 GMT
        /// Content-Length: 36170
        /// 
        /// [
        ///    {
        ///       "EmailAddress":"address@email.com",
        ///       "UnsubscribeTime":"6/17/2015 2:15:25 PM",
        ///       "SubscriptionChange":"unsubscribe",
        ///       "Reason":"UNSUBSCRIBED THRU BLAST: 999999 FOR GROUP: 1234",
        ///       "Title":"",
        ///       "FirstName":"First",
        ///       "LastName":"Last",
        ///       "FullName":"",
        ///       "Company":"Knowledge Marketing",
        ///       "Occupation":"Tester",
        ///       "Address":""3650 Annapolis Lane North,
        ///       "Address2":"",
        ///       "City":"Plymouth",
        ///       "State":"MN",
        ///       "Zip":"55447",
        ///       "Country":"USA",
        ///       "Voice":"8668446275",
        ///       "Mobile":"",
        ///       "Fax":"",
        ///       "Website":"http://www.knowledgemarketing.com",
        ///       "Age":"25",
        ///       "Income":"",
        ///       "Gender":"F",
        ///       "User1":"",
        ///       "User2":"",
        ///       "User3":"",
        ///       "User4":"",
        ///       "User5":"",
        ///       "User6":"",
        ///       "Birthdate":"01/01/1990",
        ///       "UserEvent1":"",
        ///       "UserEvent1Date":"",
        ///       "UserEvent2":"",
        ///       "UserEvent2Date":"",
        ///       "CreatedOn":"5/4/2015 10:03:27 AM",
        ///       "LastChanged":"6/17/2015 2:15:25 PM",
        ///       "FormatTypeCode":"html",
        ///       "SubscribeTypeCode":"U",
        ///       "tmp_EmailID":"",
        ///       "BUSINESS":"",
        ///       "CAT":"",
        ///       "Demo10":"",
        ///       "Demo11":"",
        ///       "Demo13":"",
        ///       "Demo9":"",
        ///       "EmailType":"",
        ///       "EMPLOYER":"",
        ///       "ForZip":"",
        ///       "FUNCTION":"",
        ///       "FUNCTION1":"",
        ///       "Job":"",
        ///       "MailStop":"",
        ///       "Plus4":"",
        ///       "PROMOCODE":"",
        ///       "Pub":"",
        ///       "PubCode":"",
        ///       "SubscriberID":"",
        ///       "XACT":""
        ///    },
        /// ]
        /// ]]></example>
        [Route("{id}/Report/Unsubscribe")]
        public IEnumerable<UnsubscribeReport> GetBlastUnsubscribeReport(int id)
        {

            if (false == ECN_Framework_BusinessLayer.Communicator.Blast.Exists(id, CustomerID))
            {
                RaiseNotFoundExceptionViaDirectThrow(id, "blast");
            }

            DataTable dt = ECN_Framework_BusinessLayer.Activity.View.BlastActivity.GetBlastEmails(id, "unsubscribe", "subscribe", APIUser);
            dt.TableName = "Report";

            //int rowNumber = 0;
            //return dt.AsEnumerable().ToDictionary( r => ++rowNumber );

            /*DataColumnCollection columns = dt.Columns;
            foreach (DataRow row in dt.AsEnumerable())
            {
                //yield return from r in dt.Columns
                Dictionary<string, string> rowDictionary = new Dictionary<string, string>();
                foreach (DataColumn col in columns)
                {
                    rowDictionary[col.ColumnName] = (row.Field<object>(col) ?? "").ToString(); //row[col].ToString();
                }
                yield return rowDictionary;
            }*/
            return dt.ToListOfUnsubscribeReports();
        }

        #endregion Get Blast Unsubscribe Report - GetBlastUnsubscribeReport()

        #region get test blast limit

        /// <summary>
        /// Get test blast limit
        /// </summary>
        /// <returns></returns>
        /// <example for="request"><![CDATA[
        /// GET http://api.ecn5.com/api/simpleblastV2/methods/TestBlastLimit HTTP/1.1
        /// Host: api.ecn5.com
        /// Accept: application/json
        /// APIAccessKey: <YOUR-API-KEY-HERE>
        /// X-Customer-ID: 77777
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
        /// Date: Mon, 27 Jul 2015 16:05:11 GMT
        /// Content-Length: 2
        /// 
        /// 10
        /// ]]></example>

        [Route("methods/TestBlastLimit")]
        public Models.IntResult GetTestBlastLimit()
        {
            ECN_Framework_Entities.Accounts.Customer c = APICustomer ?? new ECN_Framework_Entities.Accounts.Customer();
            if (null != c.TestBlastLimit && c.TestBlastLimit > 0)
            {
                return new Models.IntResult() { Result = c.TestBlastLimit.Value };
            }

            ECN_Framework_Entities.Accounts.BaseChannel b = APIBaseChannel;
            if (b == null)
            {
                if (c != null && c.BaseChannelID.HasValue)
                {
                    b = Internal.BaseChannelController.GetInternal(APICustomer.BaseChannelID.Value);
                }
            }
            if (b != null && b.TestBlastLimit.HasValue && b.TestBlastLimit > 0)
            {
                return new Models.IntResult() { Result = b.TestBlastLimit.Value };
            }

            return new Models.IntResult() { Result = 10 };
        }

        #endregion get test blast limit
        #endregion exposed methods
        #region Test Blast helper methods

        int TestBlastGetCustomerCount(APIModel model)
        {
            List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter> listFilters = new List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter>();
            if (model.FilterID > 0)
            {
                ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf = new ECN_Framework_Entities.Communicator.CampaignItemBlastFilter();
                cibf.FilterID = model.FilterID;
                cibf.CampaignItemTestBlastID = 1;//setting this to 1 so it will work
                listFilters.Add(cibf);
            }
            //really dont need this as only test blasts are calling it and test blasts cant use smart segments
            //if(SmartSegmentID > 0 && !string.IsNullOrEmpty(refBlastID))
            //{
            //    ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf = new ECN_Framework_Entities.Communicator.CampaignItemBlastFilter();
            //    cibf.SmartSegmentID = SmartSegmentID;
            //    cibf.RefBlastIDs = refBlastID;
            //    cibf.CampaignItemTestBlastID = 1;//setting this to 1 so it will work
            //    listFilters.Add(cibf);
            //}

            return ECN_Framework_BusinessLayer.Communicator.Blast.GetBlastEmailsListCount(CustomerID, 0, model.GroupID, listFilters, "", "", true, APIUser);

        }



        #endregion test blast helper
        #region data validation and cleansing     

        /// <summary>
        /// Validates GroupID, LayoutID, FilterID (if present and greater than zero), determines if a smart-segment is used,
        /// and validates layout content.  If issues are found, throws <see cref="ECN_Framework_Common.Objects.ECNException"/>.
        /// If LayoutID is invalid neither FilterID nor layout content can be validated.
        /// LayoutContent validation cannot be completed if content is uses custom fields which have not been defined.
        /// </summary>
        /// <param name="model">an API Object</param>
        /// <returns>An exception is thrown if any validation error is found. Otherwise, true if a smart segment is used.</returns>
        private int? CleanseInputData_ValidateForeignKeys(APIModel model)
        {
            int? smartSegmentID = null;

            List<ECN_Framework_Common.Objects.ECNError> validationErrorList = new List<ECN_Framework_Common.Objects.ECNError>();
            Action<string> addError = (s) =>
                validationErrorList.Add(new ECN_Framework_Common.Objects.ECNError(ECN_Framework_Common.Objects.Enums.Entity.Blast, ECN_Framework_Common.Objects.Enums.Method.Validate, s));

            if (model == null)
            {
                addError("bad request");
                throw new ECN_Framework_Common.Objects.ECNException(validationErrorList, ECN_Framework_Common.Objects.Enums.ExceptionLayer.API);
            }

            bool subjectOK = false;
            if (String.IsNullOrWhiteSpace(model.EmailSubject))
            {
                addError("missing EmailSubject");
            }
            else if (model.EmailSubject.Contains("\r") || model.EmailSubject.Contains("\n"))
            {
                addError("Email Subject contains newline characters");
            }
            else
            {
                subjectOK = true;
            }

            bool groupOK = false;
            if (!ECN_Framework_BusinessLayer.Communicator.Group.Exists(model.GroupID, APICustomer.CustomerID))
            {
                addError("GroupID unknown or inaccessible");
            }
            else
                groupOK = true;

            if (!ECN_Framework_BusinessLayer.Communicator.Layout.Exists(model.LayoutID, APICustomer.CustomerID))
            {
                addError("LayoutID unknown or inaccessible");
            }
            else
            {
                if (model.FilterID.HasValue && model.FilterID > 0)
                {
                    if (!ECN_Framework_BusinessLayer.Communicator.Filter.Exists(model.FilterID.Value, APICustomer.CustomerID))
                    {
                        if (ECN_Framework_BusinessLayer.Communicator.SmartSegment.SmartSegmentOldExists(model.FilterID.Value) && model.ReferenceBlasts.Count() > 0)
                        {
                            smartSegmentID = model.FilterID.Value;
                            if (!ECN_Framework_BusinessLayer.Communicator.Blast.RefBlastsExists(String.Join(",", model.ReferenceBlasts), APICustomer.CustomerID, model.SendTime))
                            {
                                addError("Reference Blast unknown or inaccessible");
                            }
                        }
                        else
                        {
                            addError("FilterID");
                        }
                    }
                }

                if (groupOK && subjectOK)
                {
                    try
                    {
                        System.Collections.Generic.List<string> listLY = ECN_Framework_BusinessLayer.Communicator.Layout.ValidateLayoutContent(model.LayoutID);
                        listLY.Add(model.EmailSubject.Trim().ToLower());
                        ECN_Framework_BusinessLayer.Communicator.Group.ValidateDynamicStrings(listLY, model.GroupID, APIUser);
                    }
                    catch (ECN_Framework_Common.Objects.ECNException)
                    {
                        addError("Content contains an invalid codesnippet");
                    }
                    catch (ECN_Framework_Common.Objects.SecurityException)
                    {

                        throw new ECN_Framework_Common.Objects.SecurityException();
                    }
                    catch (Exception ex)
                    {
                        LogError(1, ex, "unspecified exception validating blast layout content");
                        addError("Content contains an invalid codesnippet");
                    }

                    try
                    {
                        ECN_Framework_Entities.Communicator.Layout l = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(model.LayoutID, false);
                        if (l != null && l.TemplateID.HasValue && l.TemplateID.Value > 0)
                        {
                            ECN_Framework_Entities.Communicator.Template t = ECN_Framework_BusinessLayer.Communicator.Template.GetByTemplateID_NoAccessCheck(l.TemplateID.Value);
                            List<string> templateList = new List<string>();
                            templateList.Add(t.TemplateSource);
                            templateList.Add(t.TemplateText);

                            ECN_Framework_BusinessLayer.Communicator.Group.ValidateDynamicStringsForTemplate(templateList, model.GroupID, APIUser);

                        }
                    }
                    catch (ECN_Framework_Common.Objects.ECNException)
                    {
                        addError("Template contains an invalid codesnippet");
                    }
                    catch (ECN_Framework_Common.Objects.SecurityException)
                    {

                        throw new ECN_Framework_Common.Objects.SecurityException();
                    }
                    catch (Exception ex)
                    {
                        LogError(1, ex, "unspecified exception validating blast layout template");
                        addError("Template contains an invalid codesnippet");
                    }
                }
            }

            if (validationErrorList.Count > 0)
            {
                throw new ECN_Framework_Common.Objects.ECNException(validationErrorList, ECN_Framework_Common.Objects.Enums.ExceptionLayer.API);
            }

            return smartSegmentID;
        }

        void ValidateModelState(APIModel model)
        {
            List<ECN_Framework_Common.Objects.ECNError> errors = new List<ECN_Framework_Common.Objects.ECNError>();
            if (model == null)
            {
                ModelState.AddModelError("SimpleBlast", "no model in request body");
            }
            else
            {
                if (String.IsNullOrEmpty(model.EmailFrom))
                {
                    ModelState.AddModelError("EmailFrom", "EmailFrom is required");
                }
                if (String.IsNullOrEmpty(model.ReplyTo))
                {
                    ModelState.AddModelError("ReplyTo", "ReplyTo is required");
                }
                if (!String.IsNullOrEmpty(model.CampaignName) && (model.CampaignID.HasValue))
                {
                    ModelState.AddModelError("CampaignName", "Must supply CampaignID OR CampaignName, not both.");
                }

                if (model.EmailSubject.Contains("&#"))
                {
                    ModelState.AddModelError("EmailSubject", "Email Subject cannot contain html representation of characters");
                }

                //verify filter belongs to group and customer
                if (model.FilterID.HasValue && model.FilterID.Value > 0)
                {
                    ECN_Framework_Entities.Communicator.Filter f = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID_NoAccessCheck(model.FilterID.Value);
                    if (f != null && f.FilterID > 0)
                    {
                        if (f.GroupID != model.GroupID)
                        {
                            ModelState.AddModelError("FilterID", "Filter specified does not belong to group specified");
                        }
                        else if (f.CustomerID != APICustomer.CustomerID)
                        {
                            ModelState.AddModelError("FilterID", "Filter specified does not belong to Customer specified");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("FilterID", "Filter specified does not exist");
                    }
                }

            }
            if (false == this.ModelState.IsValid)
            {
                foreach (var s in ModelState.Values)
                    foreach (var e in s.Errors)
                        errors.Add(new ECN_Framework_Common.Objects.ECNError(
                            ECN_Framework_Common.Objects.Enums.Entity.Blast,
                            ECN_Framework_Common.Objects.Enums.Method.Validate,
                            e.ErrorMessage));
            }
            if (errors.Count() > 0)
            {
                throw new ECN_Framework_Common.Objects.ECNException(errors, ECN_Framework_Common.Objects.Enums.ExceptionLayer.API);
            }
        }

        void ValidateSimpleBlastSchedule(Models.SimpleBlastSchedule schedule)
        {
            if (null == schedule)
            {
                return;
            }

            List<ECN_Framework_Common.Objects.ECNError> errors = new List<ECN_Framework_Common.Objects.ECNError>();
            Action<string> addError = (s) => errors.Add(
                new ECN_Framework_Common.Objects.ECNError(
                    ECN_Framework_Common.Objects.Enums.Entity.Blast,
                    ECN_Framework_Common.Objects.Enums.Method.Validate,
                    s));
            if (schedule.StartDate == null)
            {
                addError("Schedule must have a StartDate");
            }
            if (schedule.EndDate == null)
            {
                addError("Schedule must have an EndDate");
            }
            if (schedule.StartDate.HasValue && schedule.StartDate < DateTime.Now)
            {
                addError("StartDate must be in the future");
            }
            if (schedule.EndDate.HasValue && schedule.EndDate < DateTime.Now)
            {
                addError("EndDate must be in the future");
            }
            if (schedule.EndDate.HasValue && schedule.StartDate.HasValue && schedule.StartDate > schedule.EndDate)
            {
                addError("EndDate must be after StartDate");
            }

            if (errors.Count() > 0)
            {
                throw new ECN_Framework_Common.Objects.ECNException(errors);
            }
        }

        #endregion data cleansing

        private void CleanseSmartSegment(APIModel model)
        {
            var oldSmartSegmentID = CleanseInputData_ValidateForeignKeys(model);
            if (oldSmartSegmentID.HasValue)
            {
                model.SetSmartSegmentID(BusinessLayerCommunicator.SmartSegment.GetNewIDFromOldID(oldSmartSegmentID.Value));
                model.FilterID = 0;
            }
        }

        private ECN_Framework_Entities.Communicator.CampaignItem GetUpdatedCampaignItem(
            APIModel model,
            int campaignItemID,
            ECN_Framework_Entities.Communicator.BlastSetupInfo setupInfo)
        {
            var campaignItem = BusinessLayerCommunicator.CampaignItem.GetByCampaignItemID(
                campaignItemID,
                APIUser,
                false);

            //update campaign item
            campaignItem.UpdatedUserID = APIUser.UserID;
            campaignItem.FromEmail = model.EmailFrom;
            campaignItem.FromName = model.EmailFromName;
            campaignItem.ReplyTo = model.ReplyTo;

            if (null != setupInfo && setupInfo.SendTime.HasValue)
            {
                campaignItem.SendTime = setupInfo.SendTime;
                campaignItem.BlastScheduleID = setupInfo.BlastScheduleID.Value;
            }
            BusinessLayerCommunicator.CampaignItem.Save(campaignItem, APIUser);
            return campaignItem;
        }

        private void UpdateCampaignItemBlast(
            APIModel model,
            ECN_Framework_Entities.Communicator.BlastAbstract blast,
            ECN_Framework_Entities.Communicator.CampaignItem campaignItem,
            ECN_Framework_Entities.Communicator.CampaignItemBlast itemBlastToSave)
        {
            itemBlastToSave.CampaignItemID = campaignItem.CampaignItemID;
            itemBlastToSave.GroupID = model.GroupID;
            itemBlastToSave.UpdatedUserID = APIUser.UserID;
            itemBlastToSave.EmailSubject = model.EmailSubject;
            itemBlastToSave.LayoutID = blast.LayoutID;

            var cibID = BusinessLayerCommunicator.CampaignItemBlast.Save(itemBlastToSave, APIUser);
            if (model.FilterID > 0)
            {
                BusinessLayerCommunicator.CampaignItemBlastFilter.DeleteByCampaignItemBlastID(cibID);
                var cibf = new ECN_Framework_Entities.Communicator.CampaignItemBlastFilter
                {
                    CampaignItemBlastID = cibID,
                    FilterID = model.FilterID,
                    IsDeleted = false
                };
                BusinessLayerCommunicator.CampaignItemBlastFilter.Save(cibf);
            }
            else
            {
                BusinessLayerCommunicator.CampaignItemBlastFilter.DeleteByCampaignItemBlastID(cibID);
            }
        }

        private ECN_Framework_Entities.Communicator.BlastSetupInfo SetupSchedule(APIModel model)
        {
            ECN_Framework_Entities.Communicator.BlastSetupInfo setupInfo = null;
            if (model.Schedule != null)
            {
                ValidateSimpleBlastSchedule(model.Schedule);
                setupInfo = CreateSetupInfo(model);

                if (setupInfo.SendTime.HasValue)
                {
                    model.SetSendTime(setupInfo.SendTime.Value);
                }
            }

            return setupInfo;
        }

        private ECN_Framework_Entities.Communicator.BlastSetupInfo SetupScheduleOverridingSendTimeIfMissing(APIModel model)
        {
            ECN_Framework_Entities.Communicator.BlastSetupInfo setupInfo = null;
            if (model.Schedule != null)
            {
                setupInfo = CreateSetupInfo(model);
                model.SetSendTime(setupInfo.SendTime ?? DateTime.Now.AddSeconds(15));
            }

            return setupInfo;
        }

        private ECN_Framework_Entities.Communicator.BlastSetupInfo CreateSetupInfo(APIModel model)
        {
            ECN_Framework_Entities.Communicator.BlastSetupInfo setupInfo = null;
            var schedule =
                BusinessLayerCommunicator.BlastSchedule.CreateScheduleFromXML(model.Schedule.ToString(), APIUser.UserID);

            if (null != schedule && schedule.BlastScheduleID.HasValue)
            {
                setupInfo = BusinessLayerCommunicator.BlastSetupInfo.GetNextScheduledBlastSetupInfo(schedule.BlastScheduleID.Value, true);
            }

            if (null == setupInfo)
            {
                throw new ECN_Framework_Common.Objects.ECNException(
                    new List<ECN_Framework_Common.Objects.ECNError>(){
                                new ECN_Framework_Common.Objects.ECNError(
                                    ECN_Framework_Common.Objects.Enums.Entity.Blast,
                                    ECN_Framework_Common.Objects.Enums.Method.Validate,
                                    "bad schedule")});
            }

            return setupInfo;
        }

        /// <summary>
        /// Converts string if it contains emoji
        /// </summary>
        /// <returns>Modified string</returns>
        private string ConvertEmailSubject(string emailSubject)
        {
            //convert email subject if it contains emoji
            if (emailSubject.Any(x => x > 255))
            {
                var uniRegex = new Regex(@"([\u2000-\u23FF])");
                var otherRegex = new Regex(@"([\uD000-\uDBFF][\uD000-\uDFFF])");

                var matches = uniRegex.Matches(emailSubject);
                foreach (Match m in matches)
                {
                    emailSubject = emailSubject.Replace(m.Value, ECN_Framework_Common.Functions.EmojiFunctions.GetUnicode(m.Value));
                }

                matches = otherRegex.Matches(emailSubject);
                foreach (Match m in matches)
                {
                    emailSubject = emailSubject.Replace(m.Value, ECN_Framework_Common.Functions.EmojiFunctions.GetUnicode(m.Value));
                }

            }
            return emailSubject;
        }

        private ECN_Framework_Entities.Communicator.BlastAbstract CreateBlastAndApplyToNewModel(
            APIModel model,
            ECN_Framework_Entities.Communicator.CampaignItem campaignItem,
            ECN_Framework_Entities.Communicator.CampaignItemBlast campaignItemBlast)
        {
            // create campaign item and blast
            if (model.IsTestBlast)
            {
                //create campaign item test blast and actual blast
                return CreateTestBlast(model, campaignItem);
            }
            else
            {
                //create campaign item blast and actual blast
                BusinessLayerCommunicator.Blast.CreateBlastsFromCampaignItem(campaignItem.CampaignItemID, APIUser);
                return BusinessLayerCommunicator.Blast.GetByCampaignItemBlastID(campaignItemBlast.CampaignItemBlastID, APIUser, false);
            }
        }

        private static void CreateCampaignItemBlastFilter(APIModel model, int campaignItemBlastId)
        {
            if (model.FilterID > 0 || model.SmartSegmentID.HasValue)
            {
                var cibf = new ECN_Framework_Entities.Communicator.CampaignItemBlastFilter
                {
                    CampaignItemBlastID = campaignItemBlastId
                };
                if (model.SmartSegmentID.HasValue && model.ReferenceBlasts.Count() > 0)
                {
                    cibf.SmartSegmentID = model.SmartSegmentID.Value;
                    cibf.RefBlastIDs = String.Join(",", model.ReferenceBlasts);
                }
                else
                {
                    cibf.FilterID = model.FilterID;
                }
                cibf.IsDeleted = false;
                BusinessLayerCommunicator.CampaignItemBlastFilter.Save(cibf);
            }
        }

        private ECN_Framework_Entities.Communicator.BlastAbstract CreateTestBlast(APIModel model, ECN_Framework_Entities.Communicator.CampaignItem campaignItem)
        {
            var citb = new ECN_Framework_Entities.Communicator.CampaignItemTestBlast
            {
                CampaignItemID = campaignItem.CampaignItemID,
                GroupID = model.GroupID,
                CreatedUserID = APIUser.UserID,
                CustomerID = CustomerID,
                HasEmailPreview = false
            };
            BusinessLayerCommunicator.CampaignItemTestBlast.Insert(citb, APIUser);

            if (model.FilterID > 0)
            {
                var cibf = new ECN_Framework_Entities.Communicator.CampaignItemBlastFilter
                {
                    CampaignItemTestBlastID = citb.CampaignItemTestBlastID,
                    FilterID = model.FilterID,
                    IsDeleted = false
                };
                BusinessLayerCommunicator.CampaignItemBlastFilter.Save(cibf);
            }

            var blast = BusinessLayerCommunicator.Blast.GetByCampaignItemTestBlastID(citb.CampaignItemTestBlastID, APIUser, false);
            if (blast != null && model.FilterID.HasValue && model.FilterID > 0)
            {
                BusinessLayerCommunicator.Blast.UpdateFilterForAPITestBlasts(blast.BlastID, model.FilterID.Value);
            }

            return blast;
        }

        private ECN_Framework_Entities.Communicator.CampaignItem CreateCampaignItem(
            APIModel model,
            ECN_Framework_Entities.Communicator.Campaign campaign,
            ECN_Framework_Entities.Communicator.BlastSetupInfo setupInfo,
            out string campaignItemName)
        {
            ECN_Framework_Entities.Communicator.CampaignItem campaignItem;
            if (!string.IsNullOrEmpty(model.CampaignItemName))
            {
                campaignItemName = model.CampaignItemName.Trim().Length > 0 ? model.CampaignItemName.Trim() : model.EmailSubject + " " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
            }
            else
            {
                campaignItemName = model.EmailSubject + " " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
            }

            campaignItem = new ECN_Framework_Entities.Communicator.CampaignItem
            {
                CustomerID = CustomerID,
                CreatedUserID = APIUser.UserID,
                CampaignItemName = campaignItemName,
                CampaignID = campaign.CampaignID,
                CampaignItemType = ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Regular.ToString(),
                CampaignItemFormatType = ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemFormatType.HTML.ToString(),
                FromEmail = model.EmailFrom,
                FromName = model.EmailFromName,
                ReplyTo = model.ReplyTo,
                IsHidden = model.IsTestBlast // hide campaign item for test blast
            };
            campaignItem.CampaignItemNameOriginal = campaignItem.CampaignItemName;
            campaignItem.CompletedStep = 4;

            // schedule
            if (null != setupInfo)
            {
                campaignItem.SendTime = setupInfo.SendTime;
                campaignItem.BlastScheduleID = setupInfo.BlastScheduleID.Value;
            }
            else
            {
                campaignItem.SendTime = DateTime.Now.AddSeconds(15);
            }

            BusinessLayerCommunicator.CampaignItem.Save(campaignItem, APIUser);
            return campaignItem;
        }

        private void TestBlastCounting(APIModel model)
        {
            if (model.IsTestBlast)
            {
                var TestBlastCount = GetTestBlastLimit().Result;
                if (TestBlastGetCustomerCount(model) > TestBlastCount)
                {
                    var error = "ERROR: The Group list selected for test blast, contains more than the allowed " + TestBlastCount + " emails for testing. Use a Filter or choose a Group with " + TestBlastCount + " or less emails in it.";
                    RaiseInvalidMessageException(error);
                }
            }
        }

        private ECN_Framework_Entities.Communicator.Campaign CreateCampaign(APIModel model)
        {
            ECN_Framework_Entities.Communicator.Campaign campaign = null;
            if (model.CampaignID.HasValue && model.CampaignID.Value > 0)
            {
                campaign = BusinessLayerCommunicator.Campaign.GetByCampaignID(model.CampaignID.Value, APIUser, false);

                if (campaign == null || campaign.CampaignID <= 0)
                {
                    RaiseInvalidMessageException("CampaignID is invalid or doesn't exist");
                }
            }
            else if (!string.IsNullOrEmpty(model.CampaignName) && model.CampaignName.Trim().Length > 0)
            {
                campaign = new ECN_Framework_Entities.Communicator.Campaign
                {
                    CustomerID = CustomerID,
                    CreatedUserID = APIUser.UserID,
                    CampaignName = model.CampaignName
                };
                campaign.CampaignID = BusinessLayerCommunicator.Campaign.Save(campaign, APIUser);
            }
            else
            {
                campaign = BusinessLayerCommunicator.Campaign.GetByCampaignName("Marketing Campaign", APIUser, false);

            }

            if (campaign == null || campaign.CampaignID <= 0)
            {
                campaign = new ECN_Framework_Entities.Communicator.Campaign
                {
                    CustomerID = CustomerID,
                    CreatedUserID = APIUser.UserID,
                    CampaignName = "Marketing Campaign"
                };
                campaign.CampaignID = BusinessLayerCommunicator.Campaign.Save(campaign, APIUser);
            }

            return campaign;
        }   
    }
}

