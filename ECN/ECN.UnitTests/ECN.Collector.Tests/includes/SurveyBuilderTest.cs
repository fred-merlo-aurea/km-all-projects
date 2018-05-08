using System;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using System.Diagnostics.CodeAnalysis;
using ecn.collector.includes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Accounts;
using KMPlatform.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace ECN.Collector.Tests.includes
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class SurveyBuilderTest : PageHelper
    {
        private const string TestUser = "TestUser";
        private SurveyBuilder _testEntity;
        private PrivateObject _privateObject;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();

            var settings = new NameValueCollection();
            settings.Add("ECNEngineAccessKey", Guid.NewGuid().ToString());
            ShimConfigurationManager.AppSettingsGet = () => settings;

            _testEntity = new SurveyBuilder();
            _privateObject = new PrivateObject(_testEntity);
            InitializeAllControls(_testEntity);
            InitializeSessionFakes();
            QueryString.Clear();
        }

        private void InitializeSessionFakes()
        {
            var shimSession = new ShimECNSession();
            shimSession.Instance.CurrentUser = new User() { UserID = 1, UserName = TestUser, IsActive = true };
            shimSession.Instance.CurrentBaseChannel = new BaseChannel { BaseChannelID = 1 };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
        }

    }
}
