using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmailMarketing.API.Models.Reports.Blast;
using System.Text.RegularExpressions;
using ecn.common.classes;
using System.Web.Http.Description;
using System.Data;

using APIModel = EmailMarketing.API.Models.PersonalizationContentErrorCodes;

using FrameworkModel = ECN_Framework_Entities.Content.PersonalizedContentErrorCodes;

namespace EmailMarketing.API.Controllers
{
    using SearchQuery = List<Models.SearchProperty>;
    using SearchResult = Models.SearchResult<APIModel>;
    using EmailMarketing.API.Attributes;

    [RoutePrefix("api/PersonalizationContentErrorCodes")]
    [AuthenticationRequired(AccessKey: EmailMarketing.API.Infrastructure.Authentication.AuthenticationProvider.Settings.AccessKeyType.User, RequiredCustomerId: true)]
    [ExceptionsLogged]
    [FriendlyExceptions(CatchUnfilteredExceptions = true)]
    //[Logged]
    [RaisesInvalidMessageOnModelError]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PersonalizationContentErrorCodesController : SearchableApiControllerBase<APIModel, FrameworkModel>
    {
        // GET: PersonalizationContentErrorCodes
        /// <summary>
        /// Constructor, subscribes for AfterTransformation events.
        /// </summary>
        public PersonalizationContentErrorCodesController()
            : base()
        {
            OnAfterTransformation += PersonalizationContentErrorCodesController_OnAfterTransformation;
        }


        /// <summary>
        /// static Getter returns API (SimpleBlast) object via fetch of Framework object and explicit transform to APIModel type
        /// </summary>        
        /// <param name="user">Framework User entity</param>        
        /// <returns><code>ErrorCodes</code> object, or null if object not found.</returns>
        /// <exception cref="ECN_Framework_Common.Objects.SecurityException">throw if not accessible by <code>user</code>.</exception>
        internal static List<APIModel> InternalGet(KMPlatform.Entity.User user)
        {
            List<FrameworkModel> errorCodes = ECN_Framework_BusinessLayer.Content.PersonalizedContentErrorCodes.GetAll();

            if (null == errorCodes)
            {
                return null;
            }
            List<APIModel> apiList = new List<Models.PersonalizationContentErrorCodes>();
            foreach(FrameworkModel f in errorCodes)
            {
                APIModel apiModel = Models.Utility.Transformer<FrameworkModel, APIModel>.Transform(f, new APIModel(), PersonalizationContentErrorCodesExposedProperties);
            }
                        
            //OnAfterTransformationInternal(TransformationEventArgs.TransformationDirection.ToAPIModel, apiModel, blast, user, fetchFiltersAndSmartSegments);

            return apiList;
        }

        static readonly string[] PersonalizationContentErrorCodesExposedProperties = new string[] {
            "ErrorCode",
            "ErrorMessage"
        };


        #region abstract methods implementation

        /// <inheritdoc/>
        public override ECN_Framework_Common.Objects.Enums.Entity FrameworkEntity
        {
            get { return ECN_Framework_Common.Objects.Enums.Entity.Blast; }
        }

        public override string[] ExposedProperties
        {
            get { return PersonalizationContentErrorCodesExposedProperties; }
        }

        public override object GetID(APIModel model)
        {
            return model.ErrorCode; // model.FolderID;
        }

        public override object GetID(FrameworkModel model)
        {
            return model.ErrorCode; // model.FolderID;
        }

        public override string ControllerName
        {
            get { return "personalizationcontent"; }
        }



        #endregion abstract methods implementation

        #region GET
        /// <summary>Get a list of personalization blast error codes</summary>        
        /// <returns>List of personalization error codes</returns>
        ///<example for="request"><![CDATA[
        /// GET http://api.ecn5.com/api/personalizationcontenterrorcodes HTTP/1.1
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
        /// [{
        ///    "ErrorCode":1,
        ///    "ErrorMessage":"Security Access Violation"  
        ///  },
        ///  {
        ///     "ErrorCode":2,
        ///     "ErrorMessage":"Email Address is Required"
        ///  },
        ///  {
        ///     "ErrorCode":3,
        ///     "ErrorMessage":HTML Content is Required"
        ///  }
        /// ]
        /// ]]></example>
        [Route("~/api/personalizationcontenterrorcodes")]
        public List<APIModel> Get([FromBody]APIModel model)
        {
            List<APIModel> retList = new List<Models.PersonalizationContentErrorCodes>();
            APIUser.CurrentClient = new KMPlatform.BusinessLogic.Client().ECN_Select(APICustomer.PlatformClientID, true);
            if (!KMPlatform.BusinessLogic.User.HasAccess(APIUser, KMPlatform.Enums.Services.EMAILMARKETING, KMPlatform.Enums.ServiceFeatures.PersonalizationBlast, KMPlatform.Enums.Access.FullAccess))
            {
                throw new ECN_Framework_Common.Objects.SecurityException();
            }

            List<ECN_Framework_Entities.Content.PersonalizedContentErrorCodes> frameworkList = ECN_Framework_BusinessLayer.Content.PersonalizedContentErrorCodes.GetAll();

            foreach(ECN_Framework_Entities.Content.PersonalizedContentErrorCodes pc in frameworkList)
            {
                APIModel current = new APIModel();
                current.ErrorCode = pc.ErrorCode;
                current.ErrorMessage = pc.ErrorMessage;

                retList.Add(current);
            }

            return retList;
        }
            #endregion



            void PersonalizationContentErrorCodesController_OnAfterTransformation(object sender, SearchableApiControllerBase<APIModel, FrameworkModel>.TransformationEventArgs args)
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