using System;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Web.Caching;
using System.Web.Fakes;
using System.Web.UI.WebControls;
using ecn.activityengines.Fakes;
using ecn.activityengines.Tests.Helpers;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using KM.Common.Entity.Fakes;
using KMPlatform.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using ShimUser = KMPlatform.BusinessLogic.Fakes.ShimUser;

namespace ecn.activityengines.Tests.Engines
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class LinkFormTestPageLoad : PageHelper
    {
        private const int KMCommonApplication = 1;
        private const int Zero = 0;
        private const string ECNEngineAccessKey = "ECNEngineAccessKey";
        private linkfrom _page;
        private PrivateObject _pagePrivate;
        private NameValueCollection _appSettings;
        private Exception _applicationLogException;
        private string _applicationLogSourceMethod;
        private User _user;
        private string _applicationLogError;
        private readonly Random _random = new Random();

        [SetUp]
        public void Setup()
        {
            _user = new User();
            _applicationLogException = null;
            _applicationLogSourceMethod = null;
            _appSettings = new NameValueCollection();
            _appSettings.Add("KMCommon_Application", KMCommonApplication.ToString());
            _appSettings.Add("ECNEngineAccessKey", ECNEngineAccessKey);
            _page = new linkfrom();
            _pagePrivate = new PrivateObject(_page);
            InitializeAllControls(_page);
            CommonShims();
        }

        [Test]
        public void PageLoad_TimeoutException_ShouldLogNonCriticalErrorAndRedirect()
        {
            //Arrange
            var exception = new TimeoutException();
            Shimlinkfrom.AllInstances.GetUserAgent = instance => { throw exception; };
            _page.RedirectOnError = true;
            _page.LinkToStore = GetString();
            var redirectCalled = false;
            Shimlinkfrom.AllInstances.Redirect = instance =>
            {
                redirectCalled = true;
            };
            //Act
            CallPageLoad();
            //Assert
            _applicationLogException.ShouldBe(exception);
            _applicationLogSourceMethod.ShouldBe("LinkFrom.Page_Load(Unknown Issue)");
            redirectCalled.ShouldBeTrue();
        }

        [Test]
        public void PageLoad_TimeoutExceptionAndRedirectOnErrorFalse_ShouldLogNonCriticalErrorAndDisplayError()
        {
            //Arrange
            var exception = new TimeoutException();
            Shimlinkfrom.AllInstances.GetUserAgent = instance => { throw exception; };
            _page.RedirectOnError = false;
            _page.LinkToStore = GetString();
            var redirectCalled = false;
            Shimlinkfrom.AllInstances.Redirect = instance =>
            {
                redirectCalled = true;
            };
            var errorMsgPanel = GetReferenceField<Panel>("errorMsgPanel");
            //Act
            CallPageLoad();
            //Assert
            _applicationLogException.ShouldBe(exception);
            _applicationLogSourceMethod.ShouldBe("LinkFrom.Page_Load(Unknown Issue)");
            redirectCalled.ShouldBeFalse();
            errorMsgPanel.ShouldNotBeNull();
            errorMsgPanel.Visible.ShouldBeTrue();
        }

        [Test]
        public void PageLoad_Exception_ShouldLogNonCriticalErrorAndRedirect()
        {
            //Arrange
            var exception = new Exception();
            Shimlinkfrom.AllInstances.GetUserAgent = instance => { throw exception; };
            _page.RedirectOnError = true;
            _page.LinkToStore = GetString();
            var redirectCalled = false;
            Shimlinkfrom.AllInstances.Redirect = instance =>
            {
                redirectCalled = true;
            };
            //Act
            CallPageLoad();
            //Assert
            _applicationLogException.ShouldBe(exception);
            _applicationLogSourceMethod.ShouldBe("LinkFrom.Page_Load(Unknown Issue)");
            redirectCalled.ShouldBeTrue();
        }

        [Test]
        public void PageLoad_ExceptionAndRedirectOnErrorFalse_ShouldLogNonCriticalErrorAndDisplayError()
        {
            //Arrange
            var exception = new Exception();
            Shimlinkfrom.AllInstances.GetUserAgent = instance => { throw exception; };
            _page.RedirectOnError = false;
            _page.LinkToStore = GetString();
            var redirectCalled = false;
            Shimlinkfrom.AllInstances.Redirect = instance =>
            {
                redirectCalled = true;
            };
            var errorMsgPanel = GetReferenceField<Panel>("errorMsgPanel");
            //Act
            CallPageLoad();
            //Assert
            _applicationLogException.ShouldBe(exception);
            _applicationLogSourceMethod.ShouldBe("LinkFrom.Page_Load(Unknown Issue)");
            redirectCalled.ShouldBeFalse();
            errorMsgPanel.ShouldNotBeNull();
            errorMsgPanel.Visible.ShouldBeTrue();
        }

        [Test]
        public void PageLoad_MaintenanceModeAndMailToUrl_ShouldSetupControls()
        {
            //Arrange
            var maintenanceMode = true;
            _appSettings.Add("Maintenance_Mode", maintenanceMode.ToString());
            var linkFormUrl = "mailto:";
            Shimlinkfrom.AllInstances.getLink = instance => linkFormUrl;
            var pnlCloseBrowser = GetReferenceField<Panel>("pnlCloseBrowser");
            //Act
            CallPageLoad();
            //Assert
            pnlCloseBrowser.Visible.ShouldBeTrue();
        }

        [Test]
        public void PageLoad_MaintenanceModeAndBlackberryAgent_ShouldRedirect()
        {
            //Arrange
            var maintenanceMode = true;
            _appSettings.Add("Maintenance_Mode", maintenanceMode.ToString());
            var linkFormUrl = "Some dummy url";
            Shimlinkfrom.AllInstances.getLink = instance => linkFormUrl;
            _page.UserAgent = "blackberry";
            var responseRedirectCalled = false;
            ShimHttpResponse.AllInstances.RedirectStringBoolean = (instance, url, endResponse) =>
            {
                responseRedirectCalled = true;
            };
            //Act
            CallPageLoad();
            //Assert
            responseRedirectCalled.ShouldBeTrue();
        }

        [Test]
        public void PageLoad_MaintenanceModeAndNotBlackberryAgent_ShouldWriteToResponse()
        {
            //Arrange
            var maintenanceMode = true;
            _appSettings.Add("Maintenance_Mode", maintenanceMode.ToString());
            var linkFormUrl = "Some dummy url";
            Shimlinkfrom.AllInstances.getLink = instance => linkFormUrl;
            _page.UserAgent = "Some Dummy Agent";
            var responseWriteCalled = false;
            ShimHttpResponse.AllInstances.WriteString = (instance, content) =>
            {
                responseWriteCalled = true;
            };
            //Act
            CallPageLoad();
            //Assert
            responseWriteCalled.ShouldBeTrue();
        }

        [Test]
        public void PageLoad_CacheHasNoAccessKey_ShouldAddToCache()
        {
            //Arrange
            var maintenanceMode = false;
            _appSettings.Add("Maintenance_Mode", maintenanceMode.ToString());
            var key = $"cache_user_by_AccessKey_{ECNEngineAccessKey}";
            _page.Cache.Remove(key);
            //Act
            CallPageLoad();
            //Assert
            var user = GetReferenceField<User>("User");
            user.ShouldBe(_user);
            var cachedUser = _page.Cache[key];
            cachedUser.ShouldNotBeNull();
            cachedUser.ShouldBe(user);
        }

        [Test]
        public void PageLoad_CacheHasAccessKey_ShouldUseIt()
        {
            //Arrange
            var maintenanceMode = false;
            _appSettings.Add("Maintenance_Mode", maintenanceMode.ToString());
            var key = $"cache_user_by_AccessKey_{ECNEngineAccessKey}";
            _page.Cache.Add(key, _user, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(15),
                CacheItemPriority.Normal, null);
            //Act
            CallPageLoad();
            //Assert
            var user = GetReferenceField<User>("User");
            user.ShouldBe(_user);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void PageLoad_LinkFromBlastLinkIDZeroAndLinkFromURLEmpty_ShouldThrowExceptionAndCatchIt(
            bool validateB4Tracking)
        {
            //Arrange
            var maintenanceMode = false;
            _appSettings.Add("Maintenance_Mode", maintenanceMode.ToString());
            _appSettings.Add("ValidateB4Tracking", validateB4Tracking.ToString());
            var blastId = GetNumber();
            var emailId = GetNumber();
            var blastLinkId = GetNumber();
            var linkFormUrl = GetString();
            var linkFromBlastLinkId = string.Empty;
            var refBlastId = GetNumber();
            var uniqueLinkId = GetNumber();
            QueryString.Add("b", blastId.ToString());
            QueryString.Add("e", emailId.ToString());
            QueryString.Add("lid", blastLinkId.ToString());
            QueryString.Add("ulid", uniqueLinkId.ToString());
            Shimlinkfrom.AllInstances.getLinkFromBlastLinkID = instance => linkFromBlastLinkId;
            Shimlinkfrom.AllInstances.getRefBlastID = instance => refBlastId;
            Shimlinkfrom.AllInstances.getLink = instance => linkFormUrl;
            Shimlinkfrom.AllInstances.TrackData = instance => GetNumber();
            Shimlinkfrom.AllInstances.ContainsTopicsString = (instance, text) => true;
            var logTransactionalUDFCalled = false;
            Shimlinkfrom.AllInstances.LogTransactionalUDF = instance =>
            {
                logTransactionalUDFCalled = true;
            };
            var redirectCalled = false;
            Shimlinkfrom.AllInstances.Redirect = instance =>
            {
                redirectCalled = true;
            };
            var expectedMessage = $"BlastLinkID = ";
            //Act
            CallPageLoad();
            //Assert
            _applicationLogError.ShouldNotBeNull();
            _applicationLogError.ShouldContain(expectedMessage);
            logTransactionalUDFCalled.ShouldBeTrue();
            redirectCalled.ShouldBeTrue();
        }

        private void CommonShims()
        {
            ShimConfigurationManager.AppSettingsGet = () => _appSettings;
            ShimApplicationLog.LogNonCriticalErrorExceptionStringInt32StringInt32Int32 =
                (exception, sourceMethod, applicationId, note, charityId, customerId) =>
                {
                    _applicationLogException = exception;
                    _applicationLogSourceMethod = sourceMethod;
                };
            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (exception, sourceMethod, applicationId, note, charityId, customerId) =>
                {
                    _applicationLogException = exception;
                    _applicationLogSourceMethod = sourceMethod;
                    return Zero;
                };
            ShimApplicationLog.LogNonCriticalErrorStringStringInt32StringInt32Int32 =
                (error, sourceMethod, applicationId, note, charityId, customerId) =>
                {
                    _applicationLogError = error;
                    _applicationLogSourceMethod = sourceMethod;
                    return Zero;
                };
            ShimUser.GetByAccessKeyStringBoolean = (accessKey, includeChildren) => _user;
            ShimEmailGroup.ValidForTrackingInt32Int32 = (blastId, emailId) => true;
        }

        private void CallPageLoad()
        {
            _pagePrivate.Invoke("Page_Load", new object[] { null, EventArgs.Empty });
        }

        private T GetReferenceField<T>(string name) where T : class
        {
            var result = _pagePrivate.GetField(name) as T;
            result.ShouldNotBeNull();
            return result;
        }

        private string GetString()
        {
            return Guid.NewGuid().ToString();
        }

        private int GetNumber()
        {
            return _random.Next(10, 100);
        }
    }
}
