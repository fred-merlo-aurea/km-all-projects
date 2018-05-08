using System;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Web.Fakes;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using KM.Common.Entity.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using KMPlatformFakes = KMPlatform.Entity.Fakes;

namespace ECN.DomainTracking.Tests
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.domaintracking.Global"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class GlobalTest
    {
        private const string AccountsVirtualPathKey = "Accounts_VirtualPath";
        private const string KMCommonApplicationKey = "KMCommon_Application";
        private const string ControllerPropertyName = "controller";
        private const string ActionPropertyName = "action";
        private const string HttpHostKey = "HTTP_HOST";
        private const string PostMethod = "post";
        private const string DummyUrl = "http://dummy/";
        private const string DummyHeader = "Dummy";

        private IDisposable _shimObject;
        private PrivateObject _mvcApplication = new PrivateObject(new ecn.domaintracking.MvcApplication());
        private string _redirectionPath = String.Empty;
        private string _errorMessage = String.Empty;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            _mvcApplication = new PrivateObject(new ecn.domaintracking.MvcApplication());
            _redirectionPath = String.Empty;
            _errorMessage = String.Empty;
            ShimHttpApplication.AllInstances.ResponseGet = (instance) => new ShimHttpResponse
            {
                RedirectStringBoolean = (url, endResponse) => _redirectionPath = url,
                RedirectToRouteObject = (routeObject) =>
                {
                    var controller = routeObject
                                        .GetType()
                                        .GetProperty(ControllerPropertyName)
                                        .GetValue(routeObject, null);
                    var action = routeObject
                                    .GetType()
                                    .GetProperty(ActionPropertyName)
                                    .GetValue(routeObject, null);
                    _redirectionPath = string.Format("{0}/{1}", controller, action);
                }
            };

            ShimHttpApplication.AllInstances.ServerGet = (instance) => new ShimHttpServerUtility
            {
                TransferStringBoolean = (route, boolValue) => _redirectionPath = route
            };

            ShimHttpApplication.AllInstances.ApplicationGet = (instance) => new ShimHttpApplicationState();
            ShimECNSession.CurrentSession = () =>
            {
                var session = (ECNSession)new ShimECNSession();
                session.CurrentUser = new KMPlatformFakes.ShimUser();
                return session;
            };

            ShimApplicationLog.LogNonCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
                {
                    _errorMessage = note;
                };

            ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
                {
                    _errorMessage = note;
                    return 0;
                };

            ShimHttpContext.CurrentGet = () => new ShimHttpContext();
            ShimHttpContext.AllInstances.RequestGet = (instance) => new ShimHttpRequest
            {
                ServerVariablesGet = () => new NameValueCollection { { HttpHostKey, String.Empty } }
            };

            ShimHttpApplication.AllInstances.RequestGet = (instance) => new ShimHttpRequest
            {
                HttpMethodGet = () => PostMethod,
                UrlGet = () => new Uri(DummyUrl),
                InputStreamGet = () => new MemoryStream(Encoding.ASCII.GetBytes(DummyHeader)),
                RawUrlGet = () => String.Empty,
                UserAgentGet = () => String.Empty,
                UrlReferrerGet = () => new Uri(DummyUrl),
                HeadersGet = () => new NameValueCollection { { DummyHeader, DummyHeader } }
            };

            ShimConfigurationManager.AppSettingsGet =
                () => new NameValueCollection
                {
                    { AccountsVirtualPathKey, AccountsVirtualPathKey },
                    { KMCommonApplicationKey, KMCommonApplicationKey }
                };
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }
    }
}
