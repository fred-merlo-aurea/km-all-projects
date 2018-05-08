using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using ECN_Framework_Common.Objects;
using EmailMarketing.API.Models;
using static ECN_Framework_BusinessLayer.Communicator.Content;
using static ECN_Framework_BusinessLayer.Communicator.Email;
using static ECN_Framework_Common.Functions.RegexUtilities;
using APIModel = EmailMarketing.API.Models.PersonalizationContent;
using FrameworkModel = ECN_Framework_Entities.Communicator.Blast;

namespace EmailMarketing.API.Controllers
{
    using EmailMarketing.API.Attributes;

    [System.Web.Http.RoutePrefix("api/PersonalizationContent")]
    [AuthenticationRequired(AccessKey: EmailMarketing.API.Infrastructure.Authentication.AuthenticationProvider.Settings.AccessKeyType.User, RequiredCustomerId: true)]
    [ExceptionsLogged]
    [FriendlyExceptions(CatchUnfilteredExceptions = true)]
    //[Logged]
    [RaisesInvalidMessageOnModelError]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PersonalizationContentController : SearchableApiControllerBase<APIModel, FrameworkModel>
    {
        private const string Created = "Created";
        private const int ErrorCodeValidatedEmailAddress = 6;
        private const int ErrorCodeTextContent = 4;
        private const int ErrorCodeEmailSubject = 5;
        private const int ErrorCodeValidatedEmailSubject = 22;
        private const int ErrorCodeEmailAddress = 2;
        private const int ErrorCodeHtmlContent = 3;
        private const int ErrorCodeHtmlContentDynamicTags = 18;
        private const int ErrorCodeHtmlConentRssFeed = 19;
        private const int ErrorCodeHtmlContentPublicView = 20;
        private const int ErrorCodeHtmlContentEcnId = 21;
        private const string EcnRssfeed = "ECN.RSSFEED";
        private const string PublicView = "%%publicview%%";
        private const string EcnId = "ecn_id=";
        private const string ContentContainsAnAnchorTagWithBlankUrl = "Content contains an &lt;a&gt; tag with a blank URL.";
        private const string BaseTagsAreNotAllowedInContent = "Base Tags are not allowed in Content";
        private const string ContentContainsAClosingHtmlTagWithoutAnOpeningTag = "Content contains a closing HTML tag without an opening tag";
        private const string ContentContainsAnOpeningHtmlTagWithoutAClosingTag = "Content contains an opening HTML tag without a closing tag";
        private const string ContentContainsAClosingHeadTagWithoutAnOpeningTag = "Content contains a closing Head tag without an opening tag";
        private const string ContentContainsAnOpeningHeadTagWithoutAClosingTag = "Content contains an opening Head tag without a closing tag";
        private const string ContentContainsAClosingBodyTagWithoutAnOpeningTag = "Content contains a closing Body tag without an opening tag";
        private const string ContentContainsAnOpeningBodyTagWithoutAClosingTag = "Content contains an opening Body tag without a closing tag";
        private const string ContentCannotContainMultipleHtmlTags = "Content cannot contain multiple HTML tags";
        private const string ContentCannotContainMultipleHeadTags = "Content cannot contain multiple Head tags";
        private const string ContentCannotContainMultipleBodyTags = "Content cannot contain multiple Body tags";
        private const int ErrorCodeAnchorWithBlankUrl = 7;
        private const int ErrorCodeBaseTagsAreNotAllowedInContent = 8;
        private const int ErrorCodeClosingTagWithoutOpeningTag = 9;
        private const int ErrorCodeOpeningTagWithoutClosingTag = 10;
        private const int ErrorCodeClosingHeadTagWithoutOpeningTag = 11;
        private const int ErrorCodeOpeningHeadTagWithoutClosingTag = 12;
        private const int ErrorCodeClosingBodyTagWithoutOpeningTag = 13;
        private const int ErrorCodeOpeningBodyTagWithoutClosingTag = 14;
        private const int ErrorCodeContentContainsMultipleHtmlTag = 15;
        private const int ErrorCodeContentContainsMultipleHeadTags = 16;
        private const int ErrorCodeContentContainsMultipleBodyTags = 17;

        ConcurrentDictionary<string, HashSet<Models.PersonalizationContentErrorCodes>> ccDictionary = new ConcurrentDictionary<string, HashSet<Models.PersonalizationContentErrorCodes>>();
        ConcurrentDictionary<string, int> resultsDictionary = new ConcurrentDictionary<string, int>();
        HashSet<Models.PersonalizationContentErrorCodes> globalErrorCodeList = new HashSet<Models.PersonalizationContentErrorCodes>();
        /// <summary>
        /// Constructor, subscribes for AfterTransformation events.
        /// </summary>
        public PersonalizationContentController()
            : base()
        {
            OnAfterTransformation += PersonalizationContentController_OnAfterTransformation;
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

            APIModel apiModel = Models.Utility.Transformer<FrameworkModel, APIModel>.Transform(blast, new APIModel(), PersonalizationContentExposedProperties);
            if (1 > apiModel.BlastID)
            {
                return null;
            }
            OnAfterTransformationInternal(TransformationEventArgs.TransformationDirection.ToAPIModel, apiModel, blast, user, fetchFiltersAndSmartSegments);

            return apiModel;
        }

        static readonly string[] PersonalizationContentExposedProperties = new string[] {
            "BlastID",
            "Emails"
        };

        #region abstract methods implementation

        /// <inheritdoc/>
        public override ECN_Framework_Common.Objects.Enums.Entity FrameworkEntity
        {
            get { return ECN_Framework_Common.Objects.Enums.Entity.Blast; }
        }

        public override string[] ExposedProperties
        {
            get { return PersonalizationContentExposedProperties; }
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
            get { return "personalizationcontent"; }
        }



        #endregion abstract methods implementation

        ///<summary>
        ///Add emails to a personalization blast.  Returns summary of results.
        ///        
        ///</summary>
        ///<example for="request"><![CDATA[
        ///POST http://api.ecn5.com/api/personalizationcontent HTTP/1.1
        ///Content-Type: application/json; charset=utf-8
        ///Accept: application/json
        ///APIAccessKey: <YOUR_API_ACCESS_KEY>
        ///X-Customer-ID: 123
        ///Host: api.ecn5.com
        ///Content-Length: 453
        ///
        ///{
        ///  "BlastID": 999999,        
        ///  "Emails":[
        ///     {
        ///         "EmailAddress":"test@test.com",
        ///         "HTMLContent":"<html><head><title>Email Title</head><body>Email HTML content here…</body></html>",
        ///         "TextContent":"Email Text content here….",
        ///         "EmailSubject":"Personalization Email Subject 1"
        ///     },
        ///     {
        ///         "EmailAddress":"test2@test.com",
        ///         "HTMLContent":"<html><head><title>Email Title</head><body>Email HTML content here…</body></html>",
        ///         "TextContent":"Email Text content here….",
        ///         "EmailSubject":"Personalization Email Subject 2"
        ///     }
        ///   ]
        ///}
        ///]]></example>
        ///
        ///<example for="response"><![CDATA[
        ///HTTP/1.1 201 Created
        ///Cache-Control: no-cache
        ///Pragma: no-cache
        ///Content-Type: application/json; charset=utf-8
        ///Expires: -1
        ///Location: http://api.ecn5.com/api/personalizationcontent/999999
        ///Server: Microsoft-IIS/7.5
        ///X-AspNet-Version: 4.0.30319
        ///X-Powered-By: ASP.NET
        ///Date: Mon, 10 Aug 2015 17:03:22 GMT
        ///Content-Length: 481
        ///
        ///{
        ///    "Created":1,
        ///    "Failed":
        ///         {
        ///             "Total":1,
        ///             "Failures":
        ///             [
        ///                 {
        ///                     "EmailAddress":"test@test.com",
        ///                     "ErrorCode":1
        ///                 }
        ///             ]
        ///         },
        ///    "Total":2
        ///}]]></example>
        [System.Web.Http.Route("~/api/personalizationcontent")]
        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public Models.PersonalizationContentResponse Post([FromBody]APIModel model)
        {
            
            if (model == null)
            {
                RaiseInvalidMessageException("no model in request body");
            }
            else if(model.BlastID <= 0)
            {
                RaiseInvalidMessageException("BlastID is required");
            }
            else if(!ECN_Framework_BusinessLayer.Communicator.Blast.Exists(model.BlastID, APICustomer.CustomerID))
            {
                
                RaiseInvalidMessageException("BlastID does not exist or is not accessible");
            }

            ECN_Framework_Entities.Communicator.Blast b = ECN_Framework_BusinessLayer.Communicator.Blast.GetByBlastID_NoAccessCheck(model.BlastID,false);
            if(b.StatusCode.ToLower() != ECN_Framework_Common.Objects.Communicator.Enums.BlastStatusCode.PendingContent.ToString().ToLower())
            {
                RaiseInvalidMessageException("Content cannot be added to this blast");
            }
            else if(b.BlastType.ToLower() != ECN_Framework_Common.Objects.Communicator.Enums.BlastType.Personalization.ToString().ToLower())
            {
                RaiseInvalidMessageException("Content cannot be added to this blast");
            }



            APIUser.CurrentClient = new KMPlatform.BusinessLogic.Client().ECN_Select(APICustomer.PlatformClientID, true);
            if (!KMPlatform.BusinessLogic.User.HasAccess(APIUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.PersonalizationBlast, KMPlatform.Enums.Access.FullAccess))
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }

            List<ECN_Framework_Entities.Content.PersonalizedContentErrorCodes> tempErrors = ECN_Framework_BusinessLayer.Content.PersonalizedContentErrorCodes.GetAll();
            foreach (ECN_Framework_Entities.Content.PersonalizedContentErrorCodes error in tempErrors)
            {
                globalErrorCodeList.Add(new Models.PersonalizationContentErrorCodes() { ErrorCode = error.ErrorCode, ErrorMessage = error.ErrorMessage });
            }


            ParallelOptions po = new ParallelOptions();

            try
            {
                po.MaxDegreeOfParallelism = Convert.ToInt32(ConfigurationManager.AppSettings["ParallelThreads"].ToString());
            }
            catch
            {
                po.MaxDegreeOfParallelism = 1;
            }

            Parallel.ForEach<Models.PersonalizationEmail>(model.Emails, po, emails =>
            {
                ValidateAndInsertContent(emails, model.BlastID);
            });

            Models.PersonalizationContentResponse response = new Models.PersonalizationContentResponse();

            if (resultsDictionary.ContainsKey("Created"))
                response.Created = resultsDictionary["Created"];
            else
                response.Created = 0;
            
            Models.FailedContent fc = new Models.FailedContent();
            List<Models.FailedEmail> fe = new List<Models.FailedEmail>();
            foreach(var kvp in ccDictionary)
            {
                Models.FailedEmail failed = new Models.FailedEmail();
                failed.EmailAddress = kvp.Key;
                failed.ErrorCode = kvp.Value.Select(x => x.ErrorCode).ToList();
                fe.Add(failed);
            }
            
            fc.Failures = fe;
            fc.Total = fe.Count;
            response.Failed = fc;


            response.Total = response.Failed.Total + response.Created;
            tempErrors.Clear();
            tempErrors = null;
            resultsDictionary.Clear();
            resultsDictionary = null;
            ccDictionary.Clear();
            ccDictionary = null;
            globalErrorCodeList.Clear();
            globalErrorCodeList = null;
            return response;
        }

        private void ValidateAndInsertContent(PersonalizationEmail email, int blastId)
        {
            var errorList = new HashSet<PersonalizationContentErrorCodes>();
            var canInsert = true;

            var isValid = CheckIsValid(email, errorList, ref canInsert);

            if (!isValid)
            {
                // errors so add to error concurrent dictionary
                ccDictionary.AddOrUpdate(email.EmailAddress, errorList, (key, oldValue) => AddToListForDict(oldValue, errorList));
            }
            else
            {
                resultsDictionary.AddOrUpdate(Created, 1, (key, oldValue) => oldValue + 1);
            }

            if (canInsert)
            {
                var personalizedContent = new ECN_Framework_Entities.Content.PersonalizedContent
                                          {
                                              BlastID = blastId,
                                              CreatedUserID = APIUser.UserID,
                                              EmailAddress = email.EmailAddress,
                                              EmailSubject = email.EmailSubject,
                                              HTMLContent = email.HTMLContent,
                                              IsDeleted = false,
                                              IsProcessed = false,
                                              IsValid = isValid,
                                              TEXTContent = email.TextContent
                                          };

                ECN_Framework_BusinessLayer.Content.PersonalizedContent.Save(personalizedContent, APIUser);
            }
        }

        private bool CheckIsValid(PersonalizationEmail email, ISet<PersonalizationContentErrorCodes> errorList, ref bool canInsert)
        {
            var isValid = !CheckValueIsNotValid(() => string.IsNullOrWhiteSpace(email.EmailAddress), errorList, ErrorCodeEmailAddress);

            if (!isValid)
            {
                canInsert = false;
            }
            else if (CheckValueIsNotValid(() => !IsValidEmailAddress(email.EmailAddress), errorList, ErrorCodeValidatedEmailAddress))
            {
                isValid = false;
                canInsert = false;
            }

            if (CheckValueIsNotValid(() => string.IsNullOrWhiteSpace(email.TextContent), errorList, ErrorCodeTextContent))
            {
                isValid = false;
            }

            if (CheckValueIsNotValid(() => string.IsNullOrWhiteSpace(email.EmailSubject), errorList, ErrorCodeEmailSubject))
            {
                isValid = false;
            }
            else if (CheckValueIsNotValid(() => !string.IsNullOrWhiteSpace(IsValidEmailSubject(email.EmailSubject)), errorList, ErrorCodeValidatedEmailSubject))
            {
                isValid = false;
            }

            if (CheckValueIsNotValid(() => string.IsNullOrWhiteSpace(email.HTMLContent), errorList, ErrorCodeHtmlContent))
            {
                isValid = false;
            }
            else
            {
                try
                {
                    ValidateHTMLContent(email.HTMLContent);
                }
                catch (ECNException ecnException)
                {
                    isValid = false;
                    HandleErrorMessages(ecnException, errorList);
                }

                if (CheckForDynamicTags(email.HTMLContent))
                {
                    errorList.Add(globalErrorCodeList.First(x => x.ErrorCode == ErrorCodeHtmlContentDynamicTags));
                    isValid = false;
                }

                var loweredHtml = email.HTMLContent.ToLower();

                if (CheckValueIsNotValid(() => email.HTMLContent.Contains(EcnRssfeed), errorList, ErrorCodeHtmlConentRssFeed))
                {
                    isValid = false;
                }

                if (CheckValueIsNotValid(
                        () => loweredHtml.Contains(PublicView) || email.TextContent.ToLower().Contains(PublicView),
                        errorList,
                        ErrorCodeHtmlContentPublicView))
                {
                    isValid = false;
                }

                if (CheckValueIsNotValid(() => loweredHtml.Contains(EcnId), errorList, ErrorCodeHtmlContentEcnId))
                {
                    isValid = false;
                }
            }

            return isValid;
        }

        private bool CheckValueIsNotValid(Func<bool> predicate, ISet<PersonalizationContentErrorCodes> errorList, int errorCode)
        {
            if (predicate())
            {
                errorList.Add(globalErrorCodeList.First(x => x.ErrorCode == errorCode));
                return true;
            }
            
            return false;
        }

        private void HandleErrorMessages(ECNException ecn, ISet<PersonalizationContentErrorCodes> errorList)
        {
            foreach (var error in ecn.ErrorList)
            {
                switch (error.ErrorMessage)
                {
                    case ContentContainsAnAnchorTagWithBlankUrl:
                        errorList.Add(globalErrorCodeList.First(x => x.ErrorCode == ErrorCodeAnchorWithBlankUrl));
                        break;
                    case BaseTagsAreNotAllowedInContent:
                        errorList.Add(globalErrorCodeList.First(x => x.ErrorCode == ErrorCodeBaseTagsAreNotAllowedInContent));
                        break;
                    case ContentContainsAClosingHtmlTagWithoutAnOpeningTag:
                        errorList.Add(globalErrorCodeList.First(x => x.ErrorCode == ErrorCodeClosingTagWithoutOpeningTag));
                        break;
                    case ContentContainsAnOpeningHtmlTagWithoutAClosingTag:
                        errorList.Add(globalErrorCodeList.First(x => x.ErrorCode == ErrorCodeOpeningTagWithoutClosingTag));
                        break;
                    case ContentContainsAClosingHeadTagWithoutAnOpeningTag:
                        errorList.Add(globalErrorCodeList.First(x => x.ErrorCode == ErrorCodeClosingHeadTagWithoutOpeningTag));
                        break;
                    case ContentContainsAnOpeningHeadTagWithoutAClosingTag:
                        errorList.Add(globalErrorCodeList.First(x => x.ErrorCode == ErrorCodeOpeningHeadTagWithoutClosingTag));
                        break;
                    case ContentContainsAClosingBodyTagWithoutAnOpeningTag:
                        errorList.Add(globalErrorCodeList.First(x => x.ErrorCode == ErrorCodeClosingBodyTagWithoutOpeningTag));
                        break;
                    case ContentContainsAnOpeningBodyTagWithoutAClosingTag:
                        errorList.Add(globalErrorCodeList.First(x => x.ErrorCode == ErrorCodeOpeningBodyTagWithoutClosingTag));
                        break;
                    case ContentCannotContainMultipleHtmlTags:
                        errorList.Add(globalErrorCodeList.First(x => x.ErrorCode == ErrorCodeContentContainsMultipleHtmlTag));
                        break;
                    case ContentCannotContainMultipleHeadTags:
                        errorList.Add(globalErrorCodeList.First(x => x.ErrorCode == ErrorCodeContentContainsMultipleHeadTags));
                        break;
                    case ContentCannotContainMultipleBodyTags:
                        errorList.Add(globalErrorCodeList.First(x => x.ErrorCode == ErrorCodeContentContainsMultipleBodyTags));
                        break;
                }
            }
        }

        private HashSet<Models.PersonalizationContentErrorCodes> AddToListForDict(HashSet<Models.PersonalizationContentErrorCodes> old, HashSet<Models.PersonalizationContentErrorCodes> newValues)
        {

            old.UnionWith(newValues);

            return old;
        }



        void PersonalizationContentController_OnAfterTransformation(object sender, SearchableApiControllerBase<APIModel, FrameworkModel>.TransformationEventArgs args)
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

                    break;
                case TransformationEventArgs.TransformationDirection.ToFrameworkModel:
                    //frameworkModel.TestBlast = apiModel.IsTestBlast ? "Y" : "N";
                    break;
            }
        }

    }
}
 