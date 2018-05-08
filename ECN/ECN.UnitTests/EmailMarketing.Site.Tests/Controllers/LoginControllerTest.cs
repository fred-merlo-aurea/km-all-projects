using System;
using EmailMarketing.Site.Controllers;
using Microsoft.QualityTools.Testing.Fakes;
using EmailMarketing.Site.Infrastructure.Abstract.Fakes;
using EmailMarketing.Site.Infrastructure.Abstract.Settings.Fakes;
using EmailMarketing.Site.Models;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using NUnit.Framework;

namespace EmailMarketing.Site.Tests.Controllers
{
    [TestFixture]
    public partial class LoginControllerTest
    {
        private IDisposable _shimObject;
        private LoginController _loginController;
        private StubIAuthenticationProvider _stubIAuthenticationProvider;
        private StubIUserSessionProvider _stubIUserSessionProvider;
        private StubIPathProvider _stubIPathProvider;
        private StubIBaseChannelProvider _stubIBaseChannelProvider;
        private StubIAccountProvider _stubIAccountProvider;
        private readonly NameValueCollection _appSettings = new NameValueCollection();

        public StubIUserSessionProvider StubIUserSessionProvider { get => _stubIUserSessionProvider; set => _stubIUserSessionProvider = value; }

        [SetUp]
        public void TestInitialize()
        {
            _shimObject = ShimsContext.Create();
            _appSettings.Add("Accounts_VirtualPath", "");
            ShimConfigurationManager.AppSettingsGet = () => _appSettings;
            _stubIAuthenticationProvider = new StubIAuthenticationProvider();
            _stubIUserSessionProvider = new StubIUserSessionProvider();
            _stubIPathProvider = new StubIPathProvider();
            _stubIBaseChannelProvider = new StubIBaseChannelProvider();
            _stubIAccountProvider = new StubIAccountProvider();
            _loginController = new LoginController(
                _stubIUserSessionProvider,
                _stubIPathProvider,
                _stubIAuthenticationProvider,
                _stubIBaseChannelProvider,
                _stubIAccountProvider);
        }

        [TearDown]
        public void TestCleanUp()
        {
            _appSettings.Clear();
            _shimObject.Dispose();
        }

        [Test]
        public void NotImplemented()
        {
            Assert.Inconclusive("CC-14401 - This test has not been implemented yet.");
        }
    }
}
