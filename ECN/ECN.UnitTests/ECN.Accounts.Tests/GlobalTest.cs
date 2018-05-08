using ECN_Framework_BusinessLayer.Application;
using ECN_Framework_BusinessLayer.Application.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Web.Fakes;

namespace ECN.Accounts.Tests
{
    /// <summary>
    ///     Unit tests for <see cref="ecn.accounts.Global"/>
    /// </summary>
    [TestFixture, ExcludeFromCodeCoverage]
    public partial class GlobalTest
    {
        private IDisposable _shimObject;
        private PrivateObject _mvcApplication = new PrivateObject(new ecn.accounts.Global());
        private string _redirectionPath = String.Empty;
        private string _errorMessage = String.Empty;

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            _mvcApplication = new PrivateObject(new ecn.accounts.Global());
            _redirectionPath = String.Empty;
            _errorMessage = String.Empty;
            ShimHttpApplication.AllInstances.ResponseGet = (instance) => new ShimHttpResponse
            {
                RedirectStringBoolean = (url, endResponse) => _redirectionPath = url
            };

            ShimHttpApplication.AllInstances.ApplicationGet = (instance) => new ShimHttpApplicationState();
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
                };

            KM.Common.Entity.Fakes.ShimApplicationLog.LogCriticalErrorExceptionStringInt32StringInt32Int32 =
                (ex, sourceMethod, applicationID, note, gdCharityID, ecnCustomerID) =>
                {
                    _errorMessage = note;
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
                    { "Accounts_VirtualPath", String.Empty },
                    { "Communicator_VirtualPath", String.Empty },                    
                    { "KMCommon_Application", "1" }
                };
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }
    }
}
