using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using EmailMarketing.API.Models.Reports.Blast;
using System.Text.RegularExpressions;
using ecn.common.classes;
using System.Data;
using System.Web.Mvc;


using APIModel = EmailMarketing.API.Models.PersonalizationBlast;

using FrameworkModel = ECN_Framework_Entities.Communicator.Blast;

namespace EmailMarketing.API.Controllers
{
    // aliasing inside name-space where the declarations will then be identical in each controller
    using SearchQuery = List<Models.SearchProperty>;
    using SearchResult = Models.SearchResult<APIModel>;
    using EmailMarketing.API.Attributes;

    [System.Web.Http.RoutePrefix("api/PersonalizationBlast")]
    [AuthenticationRequired(AccessKey: EmailMarketing.API.Infrastructure.Authentication.AuthenticationProvider.Settings.AccessKeyType.User, RequiredCustomerId: true)]
    [ExceptionsLogged]
    [FriendlyExceptions(CatchUnfilteredExceptions = true)]
    [Logged]
    [RaisesInvalidMessageOnModelError]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PersonalizationBlastController : SearchableApiControllerBase<APIModel, FrameworkModel>
    {
        /// <summary>
        /// Constructor, subscribes for AfterTransformation events.
        /// </summary>
        public PersonalizationBlastController()
            : base()
        {
            OnAfterTransformation += PersonalizationBlastController_OnAfterTransformation;
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

            APIModel apiModel = Models.Utility.Transformer<FrameworkModel, APIModel>.Transform(blast, new APIModel(), PersonalizationBlastExposedProperties);
            if (1 > apiModel.BlastID)
            {
                return null;
            }
            OnAfterTransformationInternal(TransformationEventArgs.TransformationDirection.ToAPIModel, apiModel, blast, user, fetchFiltersAndSmartSegments);

            return apiModel;
        }

        static readonly string[] PersonalizationBlastExposedProperties = new string[] {
            "BlastID",
            "EmailSubject", "EmailFrom", "EmailFromName", "ReplyTo", "BlastType",
            "StatusCode", "SendTime", // "TestBlast",
            "LayoutID", "GroupID", "FilterID","SuppressionGroupID", "SuppressionGroupFilterID",
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
            get { return PersonalizationBlastExposedProperties; }
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
            get { return "personalizationblast"; }
        }



        #endregion abstract methods implementation

        void PersonalizationBlastController_OnAfterTransformation(object sender, SearchableApiControllerBase<APIModel, FrameworkModel>.TransformationEventArgs args)
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

                    try
                    {
                        ECN_Framework_Entities.Communicator.Campaign c = ECN_Framework_BusinessLayer.Communicator.Campaign.GetByBlastID(apiModel.BlastID, user, false);
                        apiModel.CampaignID = c.CampaignID;
                        apiModel.CampaignName = c.CampaignName;
                        ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByBlastID(apiModel.BlastID, user, true);
                        apiModel.CampaignItemName = ci.CampaignItemName;
                        apiModel.BlastField1 = ci.BlastField1;
                        apiModel.BlastField2 = ci.BlastField2;
                        apiModel.BlastField3 = ci.BlastField3;
                        apiModel.BlastField4 = ci.BlastField4;
                        apiModel.BlastField5 = ci.BlastField5;

                        if(ci.BlastList != null && ci.BlastList.Count > 0)
                        {
                            if(ci.BlastList[0].Filters != null && ci.BlastList[0].Filters.Count > 0)
                            {
                                apiModel.FilterID = ci.BlastList[0].Filters[0].FilterID;
                            }
                        }

                        
                        if (ci.SuppressionList != null && ci.SuppressionList.Count > 0)
                        {
                            apiModel.SuppressionGroupID = ci.SuppressionList[0].GroupID;
                            if (ci.SuppressionList[0].Filters != null && ci.SuppressionList[0].Filters.Count > 0)
                                apiModel.SuppressionGroupFilterID = ci.SuppressionList[0].Filters[0].FilterID;
                        }
                        List<ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup> optOutList = ECN_Framework_BusinessLayer.Communicator.CampaignItemOptOutGroup.GetByCampaignItemID(ci.CampaignItemID, user);
                        if(optOutList != null && optOutList.Count > 0)
                        {
                            apiModel.OptOutGroupID = optOutList[0].GroupID;
                        }
                        

                    }
                    catch { }

                    break;
                case TransformationEventArgs.TransformationDirection.ToFrameworkModel:
                    //frameworkModel.TestBlast = apiModel.IsTestBlast ? "Y" : "N";
                    break;
            }
        }


        #region POST


        ///<summary>
        ///Create a personalization blast
        ///
        /// **When using XML as the Content-Type, use only '&lt;StartDate&gt;Date&lt;/StartDate&gt;' rather than &lt;Schedule&gt;&lt;StartDate&gt;...&lt;/Schedule&gt; unless you are building a ScheduledBlast.
        ///</summary>
        ///<example for="request"><![CDATA[
        ///POST http://api.ecn5.com/api/personalizationblast HTTP/1.1
        ///Content-Type: application/json; charset=utf-8
        ///Accept: application/json
        ///APIAccessKey: <YOUR_API_ACCESS_KEY>
        ///X-Customer-ID: 123
        ///Host: api.ecn5.com
        ///Content-Length: 453
        ///
        ///{
        ///  "BlastType": "Personalization",                
        ///  "LayoutID": 654321,
        ///  "GroupID": 123456,
        ///  "CampaignName":"Test Marketing Campaign",
        ///  "CampaignItemName":"Campaign Item Name",
        ///  "EmailSubject": "POSTedBlast",
        ///  "EmailFrom": "info@emailmarketing.com",        
        ///  "EmailFromName": "FromName",
        ///  "ReplyTo": "OmNom@gimme.com",
        ///  "SendTime":"2017-01-10 12:00:00"
        ///}]]></example>
        ///
        ///<example for="response"><![CDATA[
        ///HTTP/1.1 201 Created
        ///Cache-Control: no-cache
        ///Pragma: no-cache
        ///Content-Type: application/json; charset=utf-8
        ///Expires: -1
        ///Location: http://api.ecn5.com/api/personalizationblast/999999
        ///Server: Microsoft-IIS/7.5
        ///X-AspNet-Version: 4.0.30319
        ///X-Powered-By: ASP.NET
        ///Date: Mon, 10 Aug 2015 17:03:22 GMT
        ///Content-Length: 481
        ///
        ///{
        ///    "BlastID":999999,
        ///    "StatusCode":"pendingcontent",
        ///    "BlastType":"Personalization",
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
        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        [System.Web.Http.Route("~/api/personalizationblast")]
        public HttpResponseMessage Post([FromBody]APIModel model)
        {
            APIUser.CurrentClient = new KMPlatform.BusinessLogic.Client().ECN_Select(APICustomer.PlatformClientID, true);
            if (!KMPlatform.BusinessLogic.User.HasAccess(APIUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.PersonalizationBlast, KMPlatform.Enums.Access.FullAccess))
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }

            APIModel newModel = model;

            #region code
            ValidateModelState(model); // throw 400 error if the given model isn't valid


            //convert email subject if it contains emoji
            if (model.EmailSubject.Any(x => x > 255))
            {
                Regex uniRegex = new Regex(@"([\u2000-\u23FF])");
                Regex OtherRegex = new Regex(@"([\uD000-\uDBFF][\uD000-\uDFFF])");


                MatchCollection mc = uniRegex.Matches(model.EmailSubject);
                foreach (Match m in mc)
                {
                    model.EmailSubject = model.EmailSubject.Replace(m.Value, ECN_Framework_Common.Functions.EmojiFunctions.GetUnicode(m.Value));
                }

                MatchCollection mc2 = OtherRegex.Matches(model.EmailSubject);
                foreach (Match m in mc2)
                {
                    model.EmailSubject = model.EmailSubject.Replace(m.Value, ECN_Framework_Common.Functions.EmojiFunctions.GetUnicode(m.Value));
                }

            }
            #region validation and smart segment

            int? oldSmartSegmentID = CleanseInputData_ValidateForeignKeys(model);
            if (oldSmartSegmentID.HasValue)
            {
                //model.SetSmartSegmentID(ECN_Framework_BusinessLayer.Communicator.SmartSegment.GetNewIDFromOldID(oldSmartSegmentID.Value));
                model.FilterID = 0;
            }
            bool IsValidated = false;
            string ContentIDs = String.Empty;
            //Validate HTML Content
            ECN_Framework_Entities.Communicator.Layout l = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(model.LayoutID, false);
            ECN_Framework_Entities.Communicator.Template template = ECN_Framework_BusinessLayer.Communicator.Template.GetByTemplateID_NoAccessCheck(l.TemplateID.Value);
            
            if (template.SlotsTotal > 1)
            {
                RaiseInvalidMessageException("Message cannot have multiple slots");
            }
            else
            {
                if (l.ContentSlot1 > 0)
                {
                    ECN_Framework_Entities.Communicator.Content content = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck((int)l.ContentSlot1, false);
                    if (ECN_Framework_BusinessLayer.Communicator.Content.CheckForDynamicTags(content.ContentSource))
                    {
                        RaiseInvalidMessageException("Message cannot have dynamic tags");
                    }
                    else
                    {
                        IsValidated = content.IsValidated.HasValue ? (bool)content.IsValidated : false;
                    }
                    

                    if (!IsValidated) ContentIDs = ContentIDs + content.ContentID.ToString();

                }
                if (l.ContentSlot2 > 0)
                {
                    RaiseInvalidMessageException("Message cannot have multiple slots");
                }
                if (l.ContentSlot3 > 0)
                {
                    RaiseInvalidMessageException("Message cannot have multiple slots");
                }
                if (l.ContentSlot4 > 0)
                {
                    RaiseInvalidMessageException("Message cannot have multiple slots");
                }
                if (l.ContentSlot5 > 0)
                {
                    RaiseInvalidMessageException("Message cannot have multiple slots");
                }
                if (l.ContentSlot6 > 0)
                {
                    RaiseInvalidMessageException("Message cannot have multiple slots");
                }
                if (l.ContentSlot7 > 0)
                {
                    RaiseInvalidMessageException("Message cannot have multiple slots");
                }
                if (l.ContentSlot8 > 0)
                {
                    RaiseInvalidMessageException("Message cannot have multiple slots");
                }
                if (l.ContentSlot9 > 0)
                {
                    RaiseInvalidMessageException("Message cannot have multiple slots");
                }
                if (!String.IsNullOrEmpty(ContentIDs))
                    RaiseInvalidMessageException("Content for LayoutID is not validated");

            }
            #endregion validation and smart segment
            #region license check

            CheckLicense();

            #endregion license check
            #region test blast counting

            //if (model.IsTestBlast)
            //{
            //    int TestBlastCount = GetTestBlastLimit().Result;
            //    if (TestBlastGetCustomerCount(model) > TestBlastCount)
            //    {
            //        string error = "ERROR: The Group list selected for test blast, contains more than the allowed " + TestBlastCount + " emails for testing. Use a Filter or choose a Group with " + TestBlastCount + " or less emails in it.";
            //        RaiseInvalidMessageException(error);
            //    }
            //}

            #endregion test blast counting
            #region create campaign
            ECN_Framework_Entities.Communicator.Campaign campaign = null;
            if (model.CampaignID.HasValue && model.CampaignID.Value > 0)
            {
                campaign = ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCampaignID(model.CampaignID.Value, APIUser, false);

                if (campaign == null || campaign.CampaignID <= 0)
                {
                    RaiseInvalidMessageException("CampaignID is invalid or doesn't exist");
                }
                else if ( campaign.IsArchived.HasValue && campaign.IsArchived.Value)
                {
                    RaiseInvalidMessageException("Campaign is archived");
                }
            }
            else if (!string.IsNullOrEmpty(model.CampaignName) && model.CampaignName.Trim().Length > 0)
            {
                campaign = new ECN_Framework_Entities.Communicator.Campaign();
                campaign.CustomerID = CustomerID;
                campaign.CreatedUserID = APIUser.UserID;
                campaign.CampaignName = model.CampaignName;
                campaign.CampaignID = ECN_Framework_BusinessLayer.Communicator.Campaign.Save(campaign, APIUser);
            }
            else
            {
                campaign = ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCampaignName("Marketing Campaign", APIUser, false);

            }

            if (campaign == null || campaign.CampaignID <= 0)
            {
                campaign = new ECN_Framework_Entities.Communicator.Campaign();
                campaign.CustomerID = CustomerID;
                campaign.CreatedUserID = APIUser.UserID;
                campaign.CampaignName = "Marketing Campaign";
                campaign.CampaignID = ECN_Framework_BusinessLayer.Communicator.Campaign.Save(campaign, APIUser);
            }

            #endregion create campaign
            #region simple blast schedule
            

            #endregion simple blast schedule
            
            #region create campaign item

            //create campaign item
            string campaignItemName = "";
            if (!string.IsNullOrEmpty(model.CampaignItemName))
                campaignItemName = model.CampaignItemName.Trim().Length > 0 ? model.CampaignItemName.Trim() : model.EmailSubject + " " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
            else
                campaignItemName = model.EmailSubject + " " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");

            ECN_Framework_Entities.Communicator.CampaignItem campaignItem = new ECN_Framework_Entities.Communicator.CampaignItem();
            campaignItem.CustomerID = CustomerID;
            campaignItem.CreatedUserID = APIUser.UserID;
            campaignItem.CampaignItemName = campaignItemName;
            campaignItem.CampaignID = campaign.CampaignID;
            campaignItem.CampaignItemType = ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemType.Personalization.ToString();
            campaignItem.CampaignItemFormatType = ECN_Framework_Common.Objects.Communicator.Enums.CampaignItemFormatType.HTML.ToString();
            campaignItem.FromEmail = model.EmailFrom;
            campaignItem.FromName = model.EmailFromName;
            campaignItem.ReplyTo = model.ReplyTo;
            campaignItem.IsHidden = true;
            campaignItem.CampaignItemNameOriginal = campaignItem.CampaignItemName;
            campaignItem.CompletedStep = 4;
            if(!string.IsNullOrWhiteSpace(model.BlastField1))
                campaignItem.BlastField1 = model.BlastField1;
            if (!string.IsNullOrWhiteSpace(model.BlastField2))
                campaignItem.BlastField2 = model.BlastField2;
            if (!string.IsNullOrWhiteSpace(model.BlastField3))
                campaignItem.BlastField3 = model.BlastField3;
            if (!string.IsNullOrWhiteSpace(model.BlastField4))
                campaignItem.BlastField4 = model.BlastField4;
            if (!string.IsNullOrWhiteSpace(model.BlastField5))
                campaignItem.BlastField5 = model.BlastField5;



            // schedule
            campaignItem.SendTime = model.SendTime;

            ECN_Framework_BusinessLayer.Communicator.CampaignItem.Save(campaignItem, APIUser);

            campaignItem.OptOutGroupList = new List<ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup>();
            if (model.OptOutGroupID.HasValue && model.OptOutGroupID.Value > 0 && ECN_Framework_BusinessLayer.Communicator.Group.Exists(model.OptOutGroupID.Value, APICustomer.CustomerID))
            {
                ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup cioo = new ECN_Framework_Entities.Communicator.CampaignItemOptOutGroup();
                cioo.CampaignItemID = campaignItem.CampaignItemID;
                cioo.CreatedUserID = APIUser.UserID;
                cioo.CustomerID = campaignItem.CustomerID.Value;
                cioo.GroupID = model.OptOutGroupID.Value;
                cioo.IsDeleted = false;
                ECN_Framework_BusinessLayer.Communicator.CampaignItemOptOutGroup.Save(cioo, APIUser);
            }

            #endregion create campaign item
            #region create campaign item blast

            ECN_Framework_Entities.Communicator.CampaignItemBlast campaignItemBlast = new ECN_Framework_Entities.Communicator.CampaignItemBlast();
            campaignItemBlast.CampaignItemID = campaignItem.CampaignItemID;
            campaignItemBlast.CustomerID = CustomerID;
            campaignItemBlast.GroupID = model.GroupID;
            campaignItemBlast.CreatedUserID = APIUser.UserID;
            campaignItemBlast.EmailSubject = model.EmailSubject;
            campaignItemBlast.LayoutID = model.LayoutID;
            int cibID = ECN_Framework_BusinessLayer.Communicator.CampaignItemBlast.Save(campaignItemBlast, APIUser);

            #endregion create campaign item blast
            #region create campaign item blast filter

            if (model.FilterID > 0)
            {
                ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf = new ECN_Framework_Entities.Communicator.CampaignItemBlastFilter();
                cibf.CampaignItemBlastID = cibID;

                cibf.FilterID = model.FilterID;

                cibf.IsDeleted = false;
                ECN_Framework_BusinessLayer.Communicator.CampaignItemBlastFilter.Save(cibf);
            }

            #endregion create campaign item blast filter

            #region create CampaignitemSuppression
            if(model.SuppressionGroupID.HasValue && model.SuppressionGroupID.Value > 0 && ECN_Framework_BusinessLayer.Communicator.Group.Exists(model.SuppressionGroupID.Value, APICustomer.CustomerID))
            {
                ECN_Framework_Entities.Communicator.CampaignItemSuppression cis = new ECN_Framework_Entities.Communicator.CampaignItemSuppression();
                cis.CampaignItemID = campaignItem.CampaignItemID;
                cis.CreatedUserID = APIUser.UserID;
                cis.CustomerID = APICustomer.CustomerID;
                cis.GroupID = model.SuppressionGroupID.Value;
                cis.IsDeleted = false;

                if(model.SuppressionGroupFilterID.HasValue && model.SuppressionGroupFilterID.Value > 0 && ECN_Framework_BusinessLayer.Communicator.Filter.Exists(model.SuppressionGroupFilterID.Value, APICustomer.CustomerID))
                {
                    ECN_Framework_Entities.Communicator.CampaignItemBlastFilter cibf = new ECN_Framework_Entities.Communicator.CampaignItemBlastFilter();
                    cibf.FilterID = model.SuppressionGroupFilterID.Value;
                    cibf.IsDeleted = false;
                    cibf.SuppressionGroupID = cis.GroupID;
                    cis.Filters = new List<ECN_Framework_Entities.Communicator.CampaignItemBlastFilter>();
                    cis.Filters.Add(cibf);
                }

                ECN_Framework_BusinessLayer.Communicator.CampaignItemSuppression.Save(cis, APIUser);
            }

            #endregion

            #region create campaign item and blast


            //create campaign item blast and actual blast
            ECN_Framework_BusinessLayer.Communicator.Blast.CreateBlastsFromCampaignItem(campaignItem.CampaignItemID, APIUser);
            ECN_Framework_Entities.Communicator.BlastAbstract blast =
                ECN_Framework_BusinessLayer.Communicator.Blast.GetByCampaignItemBlastID(campaignItemBlast.CampaignItemBlastID, APIUser, false);
            newModel = Transform(blast);


            #endregion create campaign item and blast

            if (newModel == null || newModel.BlastID <= 0)
            {
                RaiseInternalServerError("UNKNOWN ERROR CREATING BLAST");
            }

            newModel.CampaignID = campaign.CampaignID;
            newModel.CampaignItemName = campaignItemName;
            
            #endregion


            return CreateResponseWithLocation(HttpStatusCode.Created, newModel, newModel.BlastID);
        }
        #endregion POST

        #region PUT
        ///<summary>
        ///Update a personalization blast
        ///
        /// **When using XML as the Content-Type, use only '&lt;StartDate&gt;Date&lt;/StartDate&gt;' rather than &lt;Schedule&gt;&lt;StartDate&gt;...&lt;/Schedule&gt; unless you are building a ScheduledBlast.
        ///</summary>
        ///<example for="request"><![CDATA[
        ///PUT http://api.ecn5.com/api/personalizationblast/99999 HTTP/1.1
        ///Content-Type: application/json; charset=utf-8
        ///Accept: application/json
        ///APIAccessKey: <YOUR_API_ACCESS_KEY>
        ///X-Customer-ID: 123
        ///Host: api.ecn5.com
        ///Content-Length: 453
        ///
        ///{
        ///  "BlastID":99999,
        ///  "SendTime":"2017-01-10 12:00:00"
        ///}]]></example>
        ///
        ///<example for="response"><![CDATA[
        ///HTTP/1.1 201 Created
        ///Cache-Control: no-cache
        ///Pragma: no-cache
        ///Content-Type: application/json; charset=utf-8
        ///Expires: -1
        ///Location: http://api.ecn5.com/api/personalizationblast/999999
        ///Server: Microsoft-IIS/7.5
        ///X-AspNet-Version: 4.0.30319
        ///X-Powered-By: ASP.NET
        ///Date: Mon, 10 Aug 2015 17:03:22 GMT
        ///Content-Length: 481
        ///
        ///{
        ///    "BlastID":999999,
        ///    "StatusCode":"pending",
        ///    "BlastType":"Personalization",
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
        [System.Web.Http.Route("~/api/personalizationblast/{id}")]
        [System.Web.Http.HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]APIModel model)
        {
            APIModel newModel = model;
            APIUser.CurrentClient = new KMPlatform.BusinessLogic.Client().ECN_Select(APICustomer.PlatformClientID, true);
            if (!KMPlatform.BusinessLogic.User.HasAccess(APIUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.PersonalizationBlast, KMPlatform.Enums.Access.FullAccess))
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }

            if (model == null)
            {
                RaiseInvalidMessageException("no model in request body");
            }
            else if (!ECN_Framework_BusinessLayer.Communicator.Blast.Exists(id, APICustomer.CustomerID))
            {
                RaiseNotFoundException(model.BlastID, "PersonalizedBlast");
            }

            ECN_Framework_Entities.Communicator.Blast b = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(id, APIUser, false);
            if (!b.BlastType.ToLower().Equals(ECN_Framework_Common.Objects.Communicator.Enums.BlastType.Personalization.ToString().ToLower()))
                RaiseInvalidMessageException("Blast Type is not valid");

            if(!b.StatusCode.ToLower().Equals(ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.PendingContent.ToString().ToLower()))
                RaiseInvalidMessageException("Blast Status Code is not valid");

            if (model.SendTime.HasValue)
            {
                if (model.SendTime.Value > DateTime.Now)
                {
                    ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByBlastID(id, APIUser, false);
                    ECN_Framework_BusinessLayer.Communicator.CampaignItem.UpdateSendTime(ci.CampaignItemID, model.SendTime.Value);
                    ECN_Framework_BusinessLayer.Communicator.Blast.UpdateStatus(id, ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.Pending);
                }
                else
                {
                    RaiseInvalidMessageException("SendTime cannot be in the past");
                }
            }
            else
            {
                ECN_Framework_Entities.Communicator.CampaignItem ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByBlastID(id, APIUser, false);
                if (ci.SendTime.Value > DateTime.Now)
                {
                    ECN_Framework_BusinessLayer.Communicator.Blast.UpdateStatus(id, ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.Pending);
                }
                else
                {
                    RaiseInvalidMessageException("SendTime cannot be in the past");
                }
            }

            ECN_Framework_Entities.Communicator.BlastAbstract blast =
                ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(id, false);
            newModel = Transform(blast);


            return CreateResponseWithLocation(HttpStatusCode.OK, newModel, id);
        }
        #endregion

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
            else subjectOK = true;

            bool groupOK = false;
            if (!ECN_Framework_BusinessLayer.Communicator.Group.Exists(model.GroupID, APICustomer.CustomerID))
            {
                addError("GroupID unknown or inaccessible");
            }
            else groupOK = true;

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

                        addError("FilterID");

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
                ModelState.AddModelError("PersonalizationBlast", "no model in request body");
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

                if(String.IsNullOrEmpty(model.EmailSubject))
                {
                    ModelState.AddModelError("EmailSubject", "Email Subject is required");
                }
                else if (model.EmailSubject.Contains("&#"))
                {
                    ModelState.AddModelError("EmailSubject", "Email Subject cannot contain html representation of characters");
                }

                if(model.GroupID < 0)
                {
                    ModelState.AddModelError("GroupID", "GroupID is required");
                }
                else
                {
                    ECN_Framework_Entities.Communicator.Group g = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(model.GroupID);
                    if (g != null)
                    {
                        if (g.Archived.HasValue && g.Archived.Value)
                        {
                            ModelState.AddModelError("GroupID", "GroupID specified is Archived");
                        }
                        else if (g.CustomerID != APICustomer.CustomerID)
                        {
                            ModelState.AddModelError("GroupID", "GroupID specified does not belong to Customer specified");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("GroupID", "GroupID specified does not exist");
                    }
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
                        else if(f.Archived.HasValue && f.Archived.Value)
                        {
                            ModelState.AddModelError("FilterID", "Filter specified is Archived");
                        }
                        else if (f.CustomerID != APICustomer.CustomerID)
                        {
                            ModelState.AddModelError("FilterID", "Filter specified does not belong to Customer specified");
                        }
                        else if(f.IsDeleted.HasValue && f.IsDeleted.Value)
                        {
                            ModelState.AddModelError("FilterID", "Filter specified does not exist");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("FilterID", "Filter specified does not exist");
                    }
                }

                if(model.SuppressionGroupID.HasValue && model.SuppressionGroupID.Value > 0)
                {
                    ECN_Framework_Entities.Communicator.Group suppGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(model.SuppressionGroupID.Value);
                    if (suppGroup != null)
                    {
                        if (suppGroup.Archived.HasValue && suppGroup.Archived.Value)
                        {
                            ModelState.AddModelError("SuppressionGroupID", "Suppression Group specified is Archived");
                        }
                        else if (suppGroup.CustomerID != APICustomer.CustomerID)
                        {
                            ModelState.AddModelError("SuppressionGroupID", "Suppression Group specified does not belong to Customer specified");
                        }

                        if (model.SuppressionGroupFilterID.HasValue && model.SuppressionGroupFilterID.Value > 0)
                        {
                            ECN_Framework_Entities.Communicator.Filter suppF = ECN_Framework_BusinessLayer.Communicator.Filter.GetByFilterID_NoAccessCheck(model.SuppressionGroupFilterID.Value);
                            if (suppF != null)
                            {
                                if (suppF.GroupID.Value != model.SuppressionGroupID.Value)
                                {
                                    ModelState.AddModelError("SuppressionGroupFilterID", "Suppression Group Filter specified does not belong to Suppression Group specified");
                                }
                                else if (suppF.Archived.HasValue && suppF.Archived.Value)
                                {
                                    ModelState.AddModelError("SuppressionGroupFilterID", "Suppression Group Filter specified is Archived");
                                }
                                else if (suppF.CustomerID != APICustomer.CustomerID)
                                {
                                    ModelState.AddModelError("SuppressionGroupFilterID", "Suppression Group Filter specified does not belong to Customer specified");
                                }
                                else if (suppF.IsDeleted.HasValue && suppF.IsDeleted.Value)
                                {
                                    ModelState.AddModelError("SuppressionGroupFilterID", "Suppression Group Filter specified does not exist");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("SuppressionGroupFilterID", "Suppression Group Filter specified does not exist");
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("SuppressionGroupID", "Suppression Group specified does not exist");
                    }
                }

                if(model.OptOutGroupID.HasValue && model.OptOutGroupID.Value > 0)
                {
                    ECN_Framework_Entities.Communicator.Group optOutGroup = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID_NoAccessCheck(model.OptOutGroupID.Value);
                    if(optOutGroup != null)
                    {
                        if(optOutGroup.Archived.HasValue && optOutGroup.Archived.Value)
                        {
                            ModelState.AddModelError("OptOutGroupID", "Opt Out Group specified is Archived");
                        }
                        else if(optOutGroup.CustomerID != APICustomer.CustomerID)
                        {
                            ModelState.AddModelError("OptOutGroupID", "Opt Out Group specified does not belong to Customer specified");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("OptOutGroupID", "Opt Out Group specified does not exist");
                    }
                    
                }

                if (!model.SendTime.HasValue)
                {
                    ModelState.AddModelError("SendTime","SendTime is required");
                }
                else if (model.SendTime < DateTime.Now)
                {
                    ModelState.AddModelError("SendTime", "SendTime must be in the future");
                }

                if(model.LayoutID > 0)
                {
                    if (ECN_Framework_BusinessLayer.Communicator.Layout.Exists(model.LayoutID, APICustomer.CustomerID))
                    {
                        if (ECN_Framework_BusinessLayer.Communicator.ContentFilter.HasDynamicContent(model.LayoutID))
                        {
                            ModelState.AddModelError("LayoutID", "Default Message cannot have dynamic content");
                        }
                        ECN_Framework_Entities.Communicator.Layout l = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(model.LayoutID, false);

                        if(l.Archived.HasValue && l.Archived.Value)
                        {
                            ModelState.AddModelError("LayoutID", "Default Message is Archived");
                        }

                        ECN_Framework_Entities.Communicator.Content c = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck(l.ContentSlot1.Value, false);
                        if (c.ContentSource.IndexOf("ECN.RSSFEED") > -1)
                        {
                            ModelState.AddModelError("LayoutID", "Default Message content cannot have RSS Feeds");
                        }
                        else if(c.ContentSource.ToLower().IndexOf("%%publicview%%") > -1 || c.ContentText.ToLower().IndexOf("%%publicview%%") > -1)
                        {
                            ModelState.AddModelError("LayoutID", "Default Message content cannot have KM public preview link");
                        }

                        ECN_Framework_Entities.Communicator.Template t = ECN_Framework_BusinessLayer.Communicator.Template.GetByTemplateID_NoAccessCheck(l.TemplateID.Value);
                        if (t.TemplateSource.ToLower().IndexOf("%%publicview%%") > -1 || t.TemplateText.ToLower().IndexOf("%%publicview%%") > -1)
                        {
                            ModelState.AddModelError("LayoutID", "Default Message Template cannot have an ECN public preview link");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("LayoutID", "LayoutID does not exist or is not accessible");
                    }
                }
                else
                {
                    ModelState.AddModelError("LayoutID", "LayoutID is required");
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



        void ValidatePersonalizationBlastSchedule(Models.SimpleBlastSchedule schedule)
        {
            if (null == schedule)
            {
                return;
            }

            List<ECN_Framework_Common.Objects.ECNError> errors = new List<ECN_Framework_Common.Objects.ECNError>();
            Action<string> addError = (s) =>
                new ECN_Framework_Common.Objects.ECNError(
                    ECN_Framework_Common.Objects.Enums.Entity.Blast,
                    ECN_Framework_Common.Objects.Enums.Method.Validate,
                    s);

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
            if(schedule.Type != Models.SimpleBlastSchedule.ScheduleType.OneTime)
            {
                addError("Schedule type must be OneTime");
            }
            if(schedule.Recurrence != Models.SimpleBlastSchedule.RecurrenceType.OneTime)
            {
                addError("Schedule recurrence must be OneTime");
            }

            if (errors.Count() > 0)
            {
                throw new ECN_Framework_Common.Objects.ECNException(errors);
            }
        }

    }
}