using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Caching;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ecn.activityengines.Fakes;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Activity.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using ECN.Tests.Helpers;
using KMPlatform.BusinessLogic.Fakes;
using KMPlatform.Entity;
using CommonEncryption = KM.Common.Entity.Encryption;
using CommonFakes = KM.Common.Entity.Fakes;
using NUnit.Framework;
using Shouldly;

namespace ecn.activityengines.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SPreviewTest : PageHelper
    {
        private const string MethoPageLoad = "Page_Load";
        private const string TestUser = "TestUser";
        private const string DummyString = "dummyString";
        private const string LayoutValueString = "1";
        private const string One = "1";
        private const int LayoutId = 1;
        private const string HttpUserAgentKey = "HTTP_USER_AGENT";
        private const string MobileKeywordiPhone = "iphone";
        private NameValueCollection _queryStringCollection;
        private string _queryString;
        private SPreview _testEntity;
        private PrivateObject _privateTestObject;
        private IDisposable _context;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            _context = ShimsContext.Create();
            base.SetPageSessionContext();
            _testEntity = new SPreview {  };
            _privateTestObject = new PrivateObject(_testEntity);
            InitializeAllControls(_testEntity);
            InitializeSessionFakes();
        }

        [TearDown]
        public void TearDwond()
        {
            _context.Dispose();
        }

        [Test]
        public void Page_Load_Success_PreviewLabelContainsData()
        {
            // Arrange 
            Initialize();
            CreateShims();
            var socialMediaList = new List<SocialMedia> { CreateInstance(typeof(SocialMedia)) };
            ShimSocialMedia.GetSocialMediaCanShare = () => socialMediaList;

            // Act
            _privateTestObject.Invoke(MethoPageLoad, null, EventArgs.Empty);

            // Assert
            var previewLabel = GetField(_testEntity, "LabelPreview") as Label;
            _testEntity.ShouldSatisfyAllConditions(
                () => previewLabel.ShouldNotBeNull(),
                () => previewLabel.Text.ShouldNotBeNullOrWhiteSpace(),
                () => previewLabel.Text.ShouldBe(DummyString));
        }

        [Test]
        public void Page_Load_Fail_PreviewLabelIsEmpty()
        {
            // Arrange 
            Initialize();
            CreateShims();

            // Act
            _privateTestObject.Invoke(MethoPageLoad, null, EventArgs.Empty);

            // Assert
            var previewLabel = GetField(_testEntity, "LabelPreview") as Label;
            _testEntity.ShouldSatisfyAllConditions(
                () => previewLabel.ShouldNotBeNull(),
                () => previewLabel.Text.ShouldBeEmpty());
        }

        [Test]
        public void IsMobileBrowser_ValidMobileType_ReturnTrue()
        {
            // Arrange
            ShimForIsMobileBrowser(MobileKeywordiPhone);
            
            // Act
            var result = SPreview.isMobileBrowser();

            // Assert
            result.ShouldBeTrue();
        }

        [Test]
        public void IsMobileBrowser_InvalidMobileType_ReturnFalse()
        {
            // Arrange
            ShimForIsMobileBrowser(DummyString);

            // Act
            var result = SPreview.isMobileBrowser();

            // Assert
            result.ShouldBeFalse();
        }

        private void ShimForIsMobileBrowser(string headerValue)
        {
            var shimHttpBrowserCapabilities = new ShimHttpBrowserCapabilities();
            ShimHttpContext.AllInstances.RequestGet = (instance) => new ShimHttpRequest
            {
                BrowserGet = () => shimHttpBrowserCapabilities.Instance,
                ServerVariablesGet = () => new NameValueCollection
                {
                    { HttpUserAgentKey, headerValue }
                }
            };
        }

        private void Initialize()
        {
            _queryString = "bid=1&eid=1&gid=1&m=1";
        }

        private void CreateShims()
        {
            ConfigurationManager.AppSettings["ECNEngineAccessKey"] = DummyString;
            ConfigurationManager.AppSettings["ValidateB4Tracking"] = DummyString;
            ConfigurationManager.AppSettings["Activity_DomainPath"] = DummyString;
            ConfigurationManager.AppSettings["SocialPreview"] = DummyString;
            ConfigurationManager.AppSettings["FBAPPID"] = DummyString;
            ConfigurationManager.AppSettings["KMCommon_Application"] = One;
            ShimPage.AllInstances.CacheGet = (x) => new Cache();
            ShimUser.GetByAccessKeyStringBoolean = (x, y) => new User();
            ShimHelper.DeCrypt_DeCode_EncryptedQueryStringString = (x) => _queryString;
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (x, y) => CreateInstance(typeof(BlastRegular));
            ShimCampaignItem.GetByBlastID_NoAccessCheckInt32Boolean = (x, y) => CreateInstance(typeof(BlastRegular));
            ShimBlastActivitySocial.InsertBlastActivitySocial = (x) => 0;
            ShimSPreview.isMobileBrowser = () => true;
            ShimBaseChannel.GetByBaseChannelIDInt32 = (x) => CreateInstance(typeof(BaseChannel));
            ShimLayout.GetPreviewNoAccessCheckInt32EnumsContentTypeCodeBooleanInt32NullableOfInt32NullableOfInt32NullableOfInt32 = (a, b, c, d, e, f, g) => DummyString;
            var campaignItemMetaTag = CreateInstance(typeof(CampaignItemMetaTag));
            campaignItemMetaTag.SocialMediaID = 1;
            var campaignItemMetaTagList = new List<CampaignItemMetaTag> { campaignItemMetaTag };
            ShimCampaignItemMetaTag.GetByCampaignItemIDInt32 = (x) => campaignItemMetaTagList;
            ShimCampaignItemTestBlast.GetByBlastID_NoAccessCheckInt32Boolean = (x, y) => CreateInstance(typeof(CampaignItemTestBlast));
            CommonFakes.ShimEncryption.GetCurrentByApplicationIDInt32 = (x) => CreateInstance(typeof(CommonEncryption));
            ShimPage.AllInstances.HeaderGet = (x) => CreateInstance(typeof(HtmlHead));
        }

        private void InitializeSessionFakes()
        {
            var shimSession = new ShimECNSession();
            var client = CreateInstance(typeof(Client));
            shimSession.Instance.CurrentUser = new User()
            {
                UserID = 1,
                UserName = TestUser,
                IsActive = true,
                CurrentClient = client
            };
            shimSession.Instance.CurrentBaseChannel = new BaseChannel { BaseChannelID = 1 };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
        }

        private dynamic CallMethod(Type type, string methodName, object[] parametersValues, object instance = null)
        {
            return ReflectionHelper.CallMethod(type, methodName, parametersValues, instance);
        }

        private dynamic CreateInstance(Type type)
        {
            return ReflectionHelper.CreateInstance(type);
        }

        private dynamic GetField(dynamic obj, string fieldName)
        {
            return ReflectionHelper.GetFieldValue(obj, fieldName);
        }

        private void SetField(dynamic obj, string fieldName, dynamic fieldValue)
        {
            ReflectionHelper.SetField(obj, fieldName, fieldValue);
        }

        private void SetProperty(dynamic obj, string fieldName, dynamic value)
        {
            ReflectionHelper.SetProperty(obj, fieldName, value);
        }

        private dynamic GetProperty(dynamic obj, string fieldName)
        {
            return ReflectionHelper.GetPropertyValue(obj, fieldName);
        }
    }
}
