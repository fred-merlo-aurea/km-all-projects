using System;
using EmailMarketing.Site.Controllers;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmailMarketing.Site.Infrastructure.Abstract.Fakes;
using EmailMarketing.Site.Infrastructure.Abstract.Settings.Fakes;
using EmailMarketing.Site.Models;
using NUnit.Framework;

namespace EmailMarketing.Site.Tests.Controllers
{
    [TestFixture]
    public partial class ResetControllerTest
    {
        private IDisposable _shimObject;
        private ResetController resetController;
        private StubIAuthenticationProvider stubIAuthenticationProvider;
        private StubIUserSessionProvider stubIUserSessionProvider;
        private StubIPathProvider stubIPathProvider;
        private StubIBaseChannelProvider stubIBaseChannelProvider;
        private StubIAccountProvider stubIAccountProvider;

        [SetUp]
        public void SetUp()
        {
            _shimObject = ShimsContext.Create();
            stubIAuthenticationProvider = new StubIAuthenticationProvider();
            stubIUserSessionProvider = new StubIUserSessionProvider();
            stubIPathProvider = new StubIPathProvider();
            stubIBaseChannelProvider = new StubIBaseChannelProvider();
            stubIAccountProvider = new StubIAccountProvider();
            resetController = new ResetController(
                stubIUserSessionProvider,
                stubIPathProvider,
                stubIAuthenticationProvider,
                stubIBaseChannelProvider,
                stubIAccountProvider);
        }

        [TearDown]
        public void TestCleanUp()
        {
            _shimObject.Dispose();
        }
    }
}
