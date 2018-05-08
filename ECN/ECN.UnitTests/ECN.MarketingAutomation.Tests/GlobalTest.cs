using ecn.MarketingAutomation;
using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Web.Fakes;

namespace ECN.MarketingAutomation.Tests
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.MarketingAutomation.MvcApplication"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class MvcApplicationTest
    {
        private IDisposable _shimObject;
        private int _applicationID;
        private Dictionary<string, object> _applicationState = new Dictionary<string, object>();

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            _mvcApplication = new PrivateObject(new MvcApplication());
            _redirectionPath = String.Empty;
            _errorMessage = String.Empty;
            ShimHttpApplication.AllInstances.ResponseGet = (instance) => new ShimHttpResponse
            {
                RedirectStringBoolean = (url, endResponse) => _redirectionPath = url,
                StatusCodeSetInt32 = (statusCode) => _statusCode = statusCode
            };

            ShimHttpApplication.AllInstances.ApplicationGet = (instance) => new ShimHttpApplicationState();
            ShimHttpApplicationState.AllInstances.ItemSetStringObject = (@this, key, val) => { _applicationState[key] = val; }; 
            ShimECNSession.CurrentSession = () =>
            {
                var session = (ECNSession)new ShimECNSession();
                session.CurrentUser = new KMPlatform.Entity.Fakes.ShimUser();
                return session;
            };

            KM.Common.Entity.Fakes.ShimApplicationLog.LogNonCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
                {
                    _errorMessage = note;
                    _applicationID = applicationID;
                    _sourceMethod = sourceMethod;
                };

            KM.Common.Entity.Fakes.ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
                {
                    _errorMessage = note;
                    _applicationID = applicationID;
                    _sourceMethod = sourceMethod;
                    return 0;
                };

            ShimHttpContext.CurrentGet = () => new ShimHttpContext();
            ShimHttpContext.AllInstances.RequestGet = (instance) => new ShimHttpRequest
            {
                ServerVariablesGet = () => new NameValueCollection { { "HTTP_HOST", String.Empty } }
            };

            ShimHttpApplication.AllInstances.RequestGet = (instance) => new ShimHttpRequest
            {
                HttpMethodGet = () => "post",
                UrlGet = () => new Uri("http://KMWeb/"),
                InputStreamGet = () => new MemoryStream(Encoding.ASCII.GetBytes("Dummy")),
                RawUrlGet = () => String.Empty,
                UserAgentGet = () => String.Empty,
                UrlReferrerGet = () => new Uri("http://dummy/"),
                HeadersGet = () => new NameValueCollection { { "Dummy", "Dummy" } }
            };

            ShimConfigurationManager.AppSettingsGet =
                () => new NameValueCollection
                {
                    { "marketingautomation_VirtualPath", String.Empty },
                    { "KMCommon_Application", ExpectedApplicationId.ToString() }
                };
        }

        [TearDown]
        public void TestCleanUp()
        {
            _errorMessage = null;
            _applicationState.Clear();
            _applicationID = default(int);
            _shimObject.Dispose();
        }
    }
}
