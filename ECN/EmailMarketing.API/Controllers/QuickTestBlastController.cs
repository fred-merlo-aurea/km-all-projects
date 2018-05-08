using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;
using ecn.common.classes;
using ECN_Framework_Common.Objects;
using APIModel = EmailMarketing.API.Models.QuickTestBlast;
using FrameworkModel = ECN_Framework_Entities.Communicator.Blast;

namespace EmailMarketing.API.Controllers
{
    using EmailMarketing.API.Attributes;

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
    public class QuickTestBlastController : SearchableApiControllerBase<APIModel, FrameworkModel>
    {
        /// <summary>
        /// Constructor, subscribes for AfterTransformation events.
        /// </summary>
        public QuickTestBlastController()
            : base()
        {
            OnAfterTransformation += SimpleBlastController_OnAfterTransformation;
        }
        #region abstract methods implementation

        /// <inheritdoc/>
        public override ECN_Framework_Common.Objects.Enums.Entity FrameworkEntity
        {
            get { return ECN_Framework_Common.Objects.Enums.Entity.Blast; }
        }

        public override string[] ExposedProperties
        {
            get { return QuickTestBlastExposedProperties; }
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

            APIModel apiModel = Models.Utility.Transformer<FrameworkModel, APIModel>.Transform(blast, new APIModel(), QuickTestBlastExposedProperties);
            if (1 > apiModel.BlastID)
            {
                return null;
            }
            OnAfterTransformationInternal(TransformationEventArgs.TransformationDirection.ToAPIModel, apiModel, blast, user);

            return apiModel;
        }

        static readonly string[] QuickTestBlastExposedProperties = new string[] {
            "EmailAddresses",
            "EmailSubject", "EmailFrom", "EmailFromName", "ReplyTo", 
            "EmailPreview", 
            "LayoutID", "GroupID", "CreateGroup", "CampaignItemID", "CampaignItemName",
            "CampaignName", "EnableCacheBuster", "SendTextVersion", "GroupName"
        };

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
        static void OnAfterTransformationInternal(TransformationEventArgs.TransformationDirection direction, APIModel apiModel, FrameworkModel frameworkModel, KMPlatform.Entity.User user)
        {
            switch (direction)
            {
                case TransformationEventArgs.TransformationDirection.ToAPIModel:
                    
                    break;
                case TransformationEventArgs.TransformationDirection.ToFrameworkModel:
                    
                    break;
            }
        }


        ///<summary>
        ///Create a simple test blast
        ///
        /// 
        ///</summary>
        ///<example for="request"><![CDATA[
        ///POST http://api.ecn5.com/api/QuickTestBlast HTTP/1.1
        ///Content-Type: application/json; charset=utf-8
        ///Accept: application/json
        ///APIAccessKey: <YOUR_API_ACCESS_KEY>
        ///X-Customer-ID: 123
        ///Host: api.ecn5.com
        ///Content-Length: 453
        ///
        ///{
        ///  "LayoutID": 654321,
        ///  "CampaignName":"Test Marketing Campaign",
        ///  "CampaignItemName":"Campaign Item Name",
        ///  "EmailSubject": "POSTedBlast",
        ///  "EmailFrom": "info@emailmarketing.com",        
        ///  "EmailFromName": "FromName",
        ///  "ReplyTo": "OmNom@gimme.com",
        ///  "EnableCacheBuster":true,
        ///  "SendText":true,
        ///  "EmailPreview":true,
        ///  "EmailAddress":"someone@test123.com,secondpersion@test123.com"
        ///}]]></example>
        ///
        ///<example for="response"><![CDATA[
        ///HTTP/1.1 201 Created
        ///Cache-Control: no-cache
        ///Pragma: no-cache
        ///Content-Type: application/json; charset=utf-8
        ///Expires: -1
        ///Location: http://api.ecn5.com/api/QuickTestBlast/999999
        ///Server: Microsoft-IIS/7.5
        ///X-AspNet-Version: 4.0.30319
        ///X-Powered-By: ASP.NET
        ///Date: Mon, 10 Aug 2015 17:03:22 GMT
        ///Content-Length: 481
        ///
        ///{
        ///  "BlastID":999999
        ///  "LayoutID": 654321,
        ///  "CampaignName":"Test Marketing Campaign",
        ///  "CampaignItemName":"Campaign Item Name",
        ///  "EmailSubject": "POSTedBlast",
        ///  "EmailFrom": "info@emailmarketing.com",        
        ///  "EmailFromName": "FromName",
        ///  "ReplyTo": "OmNom@gimme.com",
        ///  "EnableCacheBuster":true,
        ///  "SendTextVersion":true,
        ///  "EmailPreview":true,
        ///  "EmailAddress":"someone@test123.com,secondpersion@test123.com",
        ///}]]></example>
        [Route("~/api/quicktestblast")]
        public HttpResponseMessage Post([FromBody]APIModel model)
        {

            APIModel newModel = null;

            

            CleanseInputData_ValidateForeignKeys(model);

            if (!model.EmailPreview.HasValue)
                model.EmailPreview = false;
            if (!model.SendText.HasValue)
                model.SendText = false;
            if (!model.EnableCacheBuster.HasValue)
                model.EnableCacheBuster = true;


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

            #region license check
            LicenseCheck lc = new LicenseCheck();
            string BlastLicensed = lc.Current(APICustomer.CustomerID.ToString());
            string BlastAvailable = lc.Available(APICustomer.CustomerID.ToString());

            if (BlastLicensed.Equals("UNLIMITED"))
            {
                BlastAvailable = "N/A";
            }

            if (BlastAvailable == "NO LICENSE")
            {
                RaiseInvalidMessageException("NO LICENSES AVAILABLE");
            }
            #endregion

            int? groupID = null;
            if (model.GroupID.HasValue && model.GroupID.Value > 0)
                groupID = model.GroupID.Value;

            int? campaignItemID = null;
            if (model.CampaignItemID.HasValue && model.CampaignItemID.Value > 0)
                campaignItemID = model.CampaignItemID.Value;

            string campaignItemName = "";
            if (!string.IsNullOrWhiteSpace(model.CampaignItemName))
                campaignItemName = model.CampaignItemName;

            int? campaignID = null;
            if (model.CampaignID.HasValue && model.CampaignID.Value > 0)
            {                
                campaignID = model.CampaignID.Value;
            }


            string campaignName = "";
            if (!string.IsNullOrWhiteSpace(model.CampaignName))
                campaignName = model.CampaignName;


            int retBlastID = ECN_Framework_BusinessLayer.Communicator.QuickTestBlast.CreateQuickTestBlast(APICustomer.CustomerID, APIBaseChannel.BaseChannelID,  groupID,model.GroupName , model.EmailAddress, model.LayoutID, campaignItemID, campaignItemName, campaignID, campaignName, model.EmailPreview.Value, model.EnableCacheBuster.Value, model.SendText.Value, model.EmailFrom, model.ReplyTo, model.EmailFromName, model.EmailSubject, APIUser);

            ECN_Framework_Entities.Communicator.Campaign c = new ECN_Framework_Entities.Communicator.Campaign();
            ECN_Framework_Entities.Communicator.CampaignItem ci = new ECN_Framework_Entities.Communicator.CampaignItem();
            ECN_Framework_Entities.Communicator.CampaignItemTestBlast cib = new ECN_Framework_Entities.Communicator.CampaignItemTestBlast();
            cib = ECN_Framework_BusinessLayer.Communicator.CampaignItemTestBlast.GetByBlastID(retBlastID, APIUser, false);
            ci = ECN_Framework_BusinessLayer.Communicator.CampaignItem.GetByCampaignItemID(cib.CampaignItemID.Value, APIUser, false);
            c = ECN_Framework_BusinessLayer.Communicator.Campaign.GetByCampaignID(ci.CampaignID.Value, APIUser, false);
            ECN_Framework_Entities.Communicator.Blast b = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID(retBlastID, APIUser, false);
            ECN_Framework_Entities.Communicator.Group g = ECN_Framework_BusinessLayer.Communicator.Group.GetByGroupID(b.GroupID.Value, APIUser);
            newModel = model;
            newModel.BlastID = retBlastID;
            newModel.CampaignID = c.CampaignID;
            newModel.CampaignItemID = ci.CampaignItemID;
            newModel.CampaignItemName = ci.CampaignItemName;
            newModel.CampaignName = c.CampaignName;
            newModel.GroupID = b.GroupID.Value;
            newModel.GroupName = g.GroupName;
            return CreateResponseWithLocation(HttpStatusCode.Created, newModel, retBlastID);
        }


        /// <summary>
        /// Validates GroupID, LayoutID, FilterID (if present and greater than zero), determines if a smart-segment is used,
        /// and validates layout content.  If issues are found, throws <see cref="ECN_Framework_Common.Objects.ECNException"/>.
        /// If LayoutID is invalid neither FilterID nor layout content can be validated.
        /// LayoutContent validation cannot be completed if content is uses custom fields which have not been defined.
        /// </summary>
        /// <param name="model">an API Object</param>
        /// <returns>An exception is thrown if any validation error is found. Otherwise, true if a smart segment is used.</returns>
        private void CleanseInputData_ValidateForeignKeys(APIModel model)
        {
            var validationErrors = new List<string>();
            if (model == null)
            {
                validationErrors.Add("bad request");
                ThrowErrorsException(validationErrors);
            }
            if (!model.EmailPreview.HasValue)
            {
                model.EmailPreview = false;
            }
            if (!model.SendText.HasValue)
            {
                model.SendText = false;
            }
            if (!model.EnableCacheBuster.HasValue)
            {
                model.EnableCacheBuster = true;
            }

            ValidateCampaignDetails(model, validationErrors);
            ValidateEmailDetails(model, validationErrors);
            ValidateHtmlContent(model, validationErrors);

            if (validationErrors.Count > 0)
            {
                ThrowErrorsException(validationErrors);
            }
        }
        
        private static void ValidateCampaignDetails(APIModel model, List<string> validationErrors)
        {
            if (model.CampaignItemID.HasValue && model.CampaignItemID.Value > 0)
            {
                if (!string.IsNullOrWhiteSpace(model.CampaignItemName))
                {
                    validationErrors.Add("Cannot supply CampaignItemID and CampaignItemName");
                }
                else if (model.CampaignID.HasValue && model.CampaignID.Value > 0)
                {
                    validationErrors.Add("Cannot supply CampaignItemID and CampaignID");
                }
                else if (!string.IsNullOrWhiteSpace(model.CampaignName))
                {
                    validationErrors.Add("Cannot supply CampaignItemID and CampaignName");
                }
                else if ((model.CampaignID.HasValue && model.CampaignID.Value > 0) && !string.IsNullOrWhiteSpace(model.CampaignName))
                {
                    validationErrors.Add("Cannot supply CampaignID and CampaignName");
                }
            }
            else if (!string.IsNullOrWhiteSpace(model.CampaignItemName))
            {
                if (model.CampaignItemID.HasValue && model.CampaignItemID.Value > 0)
                {
                    validationErrors.Add("Cannot supply CampaignItemID and CampaignItemName");
                }
                else if ((model.CampaignID.HasValue && model.CampaignID.Value > 0) && !string.IsNullOrWhiteSpace(model.CampaignName))
                {
                    validationErrors.Add("Cannot supply CampaignID and CampaignName");
                }
                else if ((!model.CampaignID.HasValue || model.CampaignID.Value <= 0) && string.IsNullOrWhiteSpace(model.CampaignName))
                {
                    validationErrors.Add("Must supply either CampaignID or CampaignName");
                }
            }
            else if (!model.CampaignItemID.HasValue && string.IsNullOrWhiteSpace(model.CampaignItemName))
            {
                if (!string.IsNullOrWhiteSpace(model.CampaignName))
                {
                    validationErrors.Add("CampaignItemName is required");
                }
                else
                {
                    validationErrors.Add("CampaignItemID or CampaignItemName is required");
                }
            }
            else if (!model.CampaignID.HasValue && !model.CampaignItemID.HasValue && string.IsNullOrWhiteSpace(model.CampaignItemName) && string.IsNullOrWhiteSpace(model.CampaignName))
            {
                validationErrors.Add("Missing required data");
            }
        }

        private void ValidateEmailDetails(APIModel model, List<string> validationErrors)
        {
            if (string.IsNullOrWhiteSpace(model.EmailFrom))
            {
                validationErrors.Add("EmailFrom is required");
            }
            if (string.IsNullOrWhiteSpace(model.EmailFromName))
            {
                validationErrors.Add("EmailFromName is required");
            }
            if (string.IsNullOrWhiteSpace(model.ReplyTo))
            {
                validationErrors.Add("ReplyTo is required");
            }

            if (string.IsNullOrWhiteSpace(model.EmailSubject))
            {
                validationErrors.Add("Missing EmailSubject");
            }
            else if (model.EmailSubject.Contains("\r") || model.EmailSubject.Contains("\n"))
            {
                validationErrors.Add("Email Subject contains newline characters");
            }

            if (model.GroupID.HasValue && model.GroupID.Value > 0 && !ECN_Framework_BusinessLayer.Communicator.Group.Exists(model.GroupID.Value, APICustomer.CustomerID))
            {
                validationErrors.Add("GroupID unknown or inaccessible");
            }
        }

        private static void ValidateHtmlContent(APIModel model, List<string> validationErrors)
        {
            var isValidated = true;
            var contentIds = new StringBuilder();
            if (model.LayoutID > 0)
            {
                var layout = ECN_Framework_BusinessLayer.Communicator.Layout.GetByLayoutID_NoAccessCheck(model.LayoutID, false);
                if (layout != null)
                {
                    ValidateContentSlot(layout.ContentSlot1, contentIds, ref isValidated);
                    ValidateContentSlot(layout.ContentSlot2, contentIds, ref isValidated);
                    ValidateContentSlot(layout.ContentSlot3, contentIds, ref isValidated);
                    ValidateContentSlot(layout.ContentSlot4, contentIds, ref isValidated);
                    ValidateContentSlot(layout.ContentSlot5, contentIds, ref isValidated);
                    ValidateContentSlot(layout.ContentSlot6, contentIds, ref isValidated);
                    ValidateContentSlot(layout.ContentSlot7, contentIds, ref isValidated);
                    ValidateContentSlot(layout.ContentSlot8, contentIds, ref isValidated);
                    ValidateContentSlot(layout.ContentSlot9, contentIds, ref isValidated);

                    if (!isValidated)
                    {
                        validationErrors.Add("Content for LayoutID is not validated");
                    }
                }
                else
                {
                    validationErrors.Add("LayoutID unknown or inaccessible");
                }
            }
            else
            {
                validationErrors.Add("LayoutID is missing from request");
            }
        }

        private static void ValidateContentSlot(int? contentSlot, StringBuilder contentIds, ref bool isValidated)
        {
            if (contentSlot > 0 && isValidated)
            {
                var content = ECN_Framework_BusinessLayer.Communicator.Content.GetByContentID_NoAccessCheck(contentSlot.Value, false);
                
                isValidated = content.IsValidated.HasValue
                    ? (bool)content.IsValidated 
                    : false;
                
                if (!isValidated)
                {
                    contentIds.Append(content.ContentID);
                }
            }
        }
        
        private static void ThrowErrorsException(IEnumerable<string> validationErrors)
        {
            // convert error string array to ECNErrors list
            var ecnErrors = validationErrors
                .Select(error => new ECNError(Enums.Entity.QuickTestBlast, Enums.Method.Validate, error))
                .ToList();
            
            throw new ECNException(ecnErrors, Enums.ExceptionLayer.API);
        }
    }
}
