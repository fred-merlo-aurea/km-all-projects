using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Caching;
using System.Web.Fakes;
using System.Web.Mvc;
using System.Web.Mvc.Fakes;
using System.Web.Routing;
using System.Xml.Linq;
using ecn.activity.Areas.Clicks.Controllers;
using ecn.activity.Areas.Clicks.Controllers.Fakes;
using ECN.TestHelpers;
using ECN_Framework_BusinessLayer.Accounts.Fakes;
using ECN_Framework_BusinessLayer.Activity.View.Fakes;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_BusinessLayer.DomainTracker.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN_Framework_Entities.Communicator;
using KM.Common.Entity.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessFakes = KMPlatform.BusinessLogic.Fakes;
using EntityUser = KMPlatform.Entity.User;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace ecn.activity.Tests.Areas.Clicks.Controllers
{
    /// <summary>
    /// Unit test for <see cref="CreateTrackingLink"/> class.
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public class IndexControllerTest
    {
        private const string CreateTrackingLink = "CreateTrackingLink";
        private const string OmnitureDefault = "omniture";
        private const string BlastId = "blastid";
        private const int BlastIdValue = 1;
        private const string GroupName = "groupname";
        private const string FolderName = "FolderName";
        private const int GroupNameValue = 2;
        private const string SamleGroup = "SamleGroup";
        private const string LinkToStore = "http://km.all.com?a=test";
        private const string KmCommonApplication = "KMCommon_Application";
        private const string KmCommonApplicationValue = "1";
        private const string ECNEngineAccessKey = "ECNEngineAccessKey";
        private const string ECNEngineAccessKeyValue = "1";
        private const string CacheUserAccessKey = "cache_user_by_AccessKey_";
        private const string DummyString = "dummyString";
        private const string ActionIndex = "Index";
        private const string VariableHttpUrl = "HTTP_URL";
        private const string ValidateB4Tracking = "ValidateB4Tracking";
        private const string AdminNotify = "Admin_Notify";
        private const string AdminNotifyValue = "true";
        private const string TestUrl = "http://km.all.com";
        private const string ClickIndexView = "~/Areas/Clicks/Views/Index.cshtml";
        private const string HttpHost = "HTTP_HOST";
        private const string HttpHostValue = "true";
        private const string QueryString = "b=1&e=1&lid=1&ulid=1&l=1";
        private const string QueryStringWithCommas = "b=1,1&e=1&lid=1&ulid=1&l=1";
        private const string LinkToStoreWithPercent = "http://test.com/test/%23topic=dummy%%ConversionTrkCDE%%";
        private const string LinkToStoreWithHash = "http://test.com/test/#topic=dummy%%ConversionTrkCDE%%";
        private const string UsePatchForDouble = "UsePatchForDouble";
        private const string UsePatchForDoubleValue = "true";
        private string _validateB4TrackingValue = "true";
        private string _queryString;
        private string _linkUrl;
        private Mock<HttpRequestBase> _request;
        private NameValueCollection _serverVariables;
        private NameValueCollection _appSettings;
        private IndexController _indexController;
        private PrivateObject _privateObject;
        private Cache _runtimeCache;
        private EntityUser _user;
        private IDisposable _context;

        [SetUp]
        public void Setup()
        {
            _context = ShimsContext.Create();

            _indexController = new IndexController();
            _privateObject = new PrivateObject(_indexController);
            ShimCampaignItem.GetByBlastID_NoAccessCheckInt32Boolean = (x, y) => new CampaignItem { CustomerID = 1 };
            ShimCustomer.GetByCustomerIDInt32Boolean = (x, y) => new Customer { BaseChannelID = 1, CustomerID = 1 };
            var appSettings = new NameValueCollection
            {
                [AdminNotify] = AdminNotifyValue, 
                [ECNEngineAccessKey] = ECNEngineAccessKeyValue ,
                [ValidateB4Tracking] = _validateB4TrackingValue ,
                [KmCommonApplication] = KmCommonApplicationValue 
            };
            ShimConfigurationManager.AppSettingsGet = () => appSettings;
            ShimApplicationLog.LogNonCriticalErrorStringStringInt32StringInt32Int32
                = (error, sourceMethod, applicationId, note, charityId, customerId) => 1;
            SetupControllerContext();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [TestCase("", QueryStringWithCommas)]
        [TestCase(LinkToStoreWithHash, QueryString)]
        [TestCase(LinkToStoreWithPercent, QueryString)]
        public void Index_Success_ReturnsIndexView(string linkToStore, string queryString)
        {
            // Arrange
            _validateB4TrackingValue = bool.TrueString;
            _linkUrl = linkToStore;
            _queryString = queryString;
            SetupForMethodIndex();

            // Act
            var parameters = new object[] { _queryString };
            var result = _privateObject.Invoke(ActionIndex, parameters) as ViewResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                 () => result.ShouldNotBeNull(),
                 () => result.ViewName.Equals(ClickIndexView));
        }

        [TestCase("https://ad.doubleclick.net/", QueryString)]
        public void Index_WhenUsePatchForDoubleIsTrue_RedirectsToLinkToStore(string linkToStore, string queryString)
        {
            // Arrange
            _validateB4TrackingValue = bool.FalseString;
            _linkUrl = linkToStore;
            _queryString = queryString;
            SetupForMethodIndex();
            ShimController.AllInstances.RedirectString = (x, redirectUrl) => 
            {
                return new RedirectResult(redirectUrl);
            };

            // Act
            var parameters = new object[] { _queryString };
            var result = _privateObject.Invoke(ActionIndex, parameters) as RedirectResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                 () => result.ShouldNotBeNull(),
                 () => result.Url.Equals(linkToStore));
        }

        [TestCase("https://ad.doubleclick.net/", QueryString)]
        public void Index_WhenTimeOutExceptionIsThrwn_RedirectsToLinkToStore(string linkToStore, string queryString)
        {
            // Arrange
            _validateB4TrackingValue = bool.FalseString;
            _linkUrl = linkToStore;
            _queryString = queryString;
            SetupForMethodIndex();
            var throwException = true;
            ShimController.AllInstances.RedirectString = (x, redirectUrl) => 
            {
                throwException = !throwException;
                if (!throwException)
                {
                    throw new TimeoutException();
                }
                else
                {
                    return new RedirectResult(redirectUrl);
                }
            };

            // Act
            var parameters = new object[] { _queryString };
            var result = _privateObject.Invoke(ActionIndex, parameters) as RedirectResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                 () => result.ShouldNotBeNull(),
                 () => result.Url.Equals(linkToStore));
        }

        [TestCase("https://ad.doubleclick.net/", QueryString)]
        public void Index_WhenExceptionIsThrwn_ReturnsLinkToStoreView(string linkToStore, string queryString)
        {
            // Arrange
            _validateB4TrackingValue = bool.FalseString;
            _linkUrl = linkToStore;
            _queryString = queryString;
            SetupForMethodIndex();
            var throwException = true;
            ShimController.AllInstances.RedirectString = (x, redirectUrl) =>
            {

                throwException = !throwException;
                if (!throwException)
                {
                    throw new Exception();
                }
                else
                {
                    return new RedirectResult(redirectUrl);
                }
            };

            // Act
            var parameters = new object[] { _queryString };
            var result = _privateObject.Invoke(ActionIndex, parameters) as ViewResult;

            // Assert
            result.ShouldSatisfyAllConditions(
                 () => result.ShouldNotBeNull(),
                 () => result.ViewName.Equals(linkToStore));
        }

        [TestCase("9", "-1", true)]
        [TestCase("9", "1", true)]
        [TestCase("9", "2", false)]
        [TestCase("9", "3", false)]
        [TestCase("10", "-1", true)]
        [TestCase("10", "1", true)]
        [TestCase("10", "2", false)]
        [TestCase("10", "3", false)]
        public void CreateTrackingLink_ConversionTrackingExistsIsTrue_ReturnsResultObject(string omnitureValue, string ltpoid, bool allowCustomerOverride)
        {
            // Arrange
            ShimLinkTrackingSettings.GetByBaseChannelID_LTIDInt32Int32 = (x, y) => new LinkTrackingSettings { XMLConfig = CreateSettingXml(allowCustomerOverride) };
            ShimLinkTrackingSettings.GetByCustomerID_LTIDInt32Int32 = (x, y) => new LinkTrackingSettings { XMLConfig = CreateSettingXml(allowCustomerOverride) };
            var dataTable = CreateDataTableObjectForOmitor(string.Concat(OmnitureDefault, omnitureValue), ltpoid);

            var parameters = new object[] { dataTable, true };
            ShimBlast.GetLinkTrackingParamInt32String = (blastID, param) =>
            {
                if (param == GroupName)
                {
                    throw new Exception();
                }
                return param;
            };

            ShimLinkTrackingParamOption.GetByLTPOIDInt32 = CreateLinkTrackingParamOptionObject;

            // Act
            var result = _privateObject.Invoke(CreateTrackingLink, parameters) as string;

            // Assert
            result.ShouldSatisfyAllConditions(
                 () => result.ShouldNotBeNullOrEmpty(),
                 () => result.ShouldNotBeNullOrWhiteSpace());
        }

        [TestCase(true, true)]
        [TestCase(false, true)]
        [TestCase(true, false)]
        public void CreateTrackingLink_OmnitureNotExistInDataTable_ReturnsResultObject(bool creatXmlConfile, bool isDefaultSettingNode)
        {
            // Arrange
            var xmlConfig = creatXmlConfile ? CreateSettingXml(false, isDefaultSettingNode) : string.Empty;

            ShimLinkTrackingSettings.GetByBaseChannelID_LTIDInt32Int32 = (x, y) => new LinkTrackingSettings { XMLConfig = xmlConfig };
            ShimLinkTrackingSettings.GetByCustomerID_LTIDInt32Int32 = (x, y) => new LinkTrackingSettings { XMLConfig = xmlConfig };
            var dataTable = CreateDataTableObject();
            _indexController.LinkToStore = LinkToStore;

            var parameters = new object[] { dataTable, false };
            ShimBlast.GetLinkTrackingParamInt32String = (blastID, param) =>
            {
                if (param == FolderName)
                {
                    throw new Exception();
                }
                return param;
            };

            ShimLinkTrackingParamOption.GetByLTPOIDInt32 = CreateLinkTrackingParamOptionObject;

            // Act
            var result = _privateObject.Invoke(CreateTrackingLink, parameters) as string;

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNullOrEmpty(),
                () => result.ShouldNotBeNullOrWhiteSpace());
        }

        private LinkTrackingParamOption CreateLinkTrackingParamOptionObject(int ltPoId)
        {
            var linkTrackingParamOption = new LinkTrackingParamOption();
            if (ltPoId == BlastIdValue)
            {
                linkTrackingParamOption.Value = BlastId;
                linkTrackingParamOption.IsDynamic = true;
            }
            else if (ltPoId == GroupNameValue)
            {
                linkTrackingParamOption.Value = GroupName;
                linkTrackingParamOption.IsDynamic = true;
            }
            else
            {
                linkTrackingParamOption.Value = SamleGroup;
                linkTrackingParamOption.IsDynamic = false;
            }
            return linkTrackingParamOption;
        }

        private string CreateSettingXml(bool allowCustomerOverride = false, bool defaultMainNode = true)
        {
            var xmlMainNode = defaultMainNode ? "Settings" : "Settings1";
            var element = new XElement(xmlMainNode,
             new XElement("Override", allowCustomerOverride),
             new XElement("QueryString", "a"),
             new XElement("Delimiter", ","),
             new XElement("AllowCustomerOverride", allowCustomerOverride));
            return element.ToString();
        }

        private DataTable CreateDataTableObjectForOmitor(string omnitureValue, string ltpoid)
        {
            var table = new DataTable();
            table.Columns.Add("ColumnName", typeof(string));
            table.Columns.Add("LTPOID", typeof(string));
            table.Columns.Add("CustomValue", typeof(string));
            table.Columns.Add("LTID", typeof(string));
            table.Rows.Add(omnitureValue, ltpoid, "100%20", "3");
            table.Rows.Add(omnitureValue, 1, "100%20", "3");
            return table;
        }

        private DataTable CreateDataTableObject()
        {
            var table = new DataTable();
            table.Columns.Add("ColumnName", typeof(string));
            table.Columns.Add("DisplayName", typeof(string));
            table.Columns.Add("LTID", typeof(string));
            table.Rows.Add("KnowledgeMarketing", "1", "3");
            table.Rows.Add("FolderName", "UnitTest", "3");
            table.Rows.Add("GroupName", "A", "3");
            table.Rows.Add("LayoutName", "AdminLayout", "3");
            table.Rows.Add("EmailSubject", "Unit Test", "3");
            table.Rows.Add("CustomValue", "100%20", "3");
            table.Rows.Add("BlastID", "1", "3");
            table.Rows.Add("eid", "1", "3");
            table.Rows.Add("bid", "1", "3");
            table.Rows.Add("default", "1", "3");
            return table;
        }

        private void SetupControllerContext()
        {
            _request = new Mock<HttpRequestBase>();
            _request.Setup(x => x.HttpMethod)
                .Returns("GET");
            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(x => x.Request)
                .Returns(_request.Object);
            var server = new Mock<HttpServerUtilityBase>();
            server.Setup(x => x.UrlEncode(It.IsAny<string>()))
                .Returns<string>(x => x);
            mockHttpContext.Setup(x => x.Server)
                .Returns(server.Object);
            var session = new Mock<HttpSessionStateBase>();
            mockHttpContext.Setup(x => x.Session)
                .Returns(session.Object);
            _indexController.ControllerContext = new ControllerContext(
                mockHttpContext.Object,
                new RouteData(),
                new Mock<ControllerBase>().Object);
        }

        private void SetupForMethodIndex()
        {
            _appSettings = new NameValueCollection
            {
                [AdminNotify] = AdminNotifyValue ,
                [UsePatchForDouble] = UsePatchForDoubleValue,
                [ECNEngineAccessKey] = ECNEngineAccessKeyValue,
                [ValidateB4Tracking] = _validateB4TrackingValue,
                [KmCommonApplication] = KmCommonApplicationValue
            };
            ShimConfigurationManager.AppSettingsGet = () => _appSettings;
            _runtimeCache = new Cache();
            _user = new EntityUser();
            _serverVariables = new NameValueCollection()
            {
                { VariableHttpUrl, $"clicks/{_queryString}"},
                { HttpHost, TestUrl }
            };
            _request.Setup(x => x.ServerVariables).
                Returns(_serverVariables);
            _request.Setup(x => x.UserHostAddress).
                Returns(TestUrl);
            _request.Setup(x => x.UserAgent).
                Returns(DummyString);
            _request.Setup(x => x.RawUrl).
                Returns(DummyString);
            _request.Setup(x => x.UrlReferrer).
                Returns(new Uri(TestUrl));
            _request.Setup(x => x.Headers).
                Returns(new NameValueCollection { { DummyString, DummyString } });
            ShimHttpRuntime.CacheGet = () => _runtimeCache;
            BusinessFakes.ShimUser.GetByAccessKeyStringBoolean = (x, y) => _user;
            var blastLink = ReflectionHelper.CreateInstance(typeof(BlastLink));
            blastLink.LinkURL = _linkUrl;
            ShimBlastLink.GetByBlastLinkIDInt32Int32 = (x, y) => blastLink;
            var blastRegular = ReflectionHelper.CreateInstance(typeof(BlastRegular));
            blastRegular.BlastType = "LAYOUT";
            ShimBlast.GetByBlastID_NoAccessCheckInt32Boolean = (x, y) => blastRegular;
            ShimEmailGroup.ValidForTrackingInt32Int32 = (x, y) => true;
            ShimEmailDataValues.RecordTopicsValueInt32Int32String = (x, y, z) => 0;
            ShimLinkTracking.GetByCampaignItemIDInt32 = (x) => new List<LinkTracking> { ReflectionHelper.CreateInstance(typeof(LinkTracking)) };
            ShimLinkTracking.CreateLinkTrackingParamsInt32StringInt32 = (x, y, z) => true;
            var paramInfoTable = new DataTable();
            paramInfoTable.Columns.Add(DummyString);
            paramInfoTable.Rows.Add(DummyString);
            ShimCampaignItemLinkTracking.GetParamInfoInt32Int32 = (x, y) => paramInfoTable;
            ShimIndexController.AllInstances.CreateTrackingLinkDataTableBoolean = (x, y, z) => DummyString;
            ShimBaseChannel.HasProductFeatureInt32EnumsServicesEnumsServiceFeatures = (a, b, c) => true;
            ShimBlastActivity.FilterEmailsAllWithSmartSegmentInt32Int32 = (x, y) => paramInfoTable;
            ShimDomainTracker.ExistsStringInt32 = (x, y) => true;
        }
    }
}
