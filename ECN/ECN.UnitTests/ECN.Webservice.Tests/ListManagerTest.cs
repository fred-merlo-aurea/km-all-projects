using System;
using System.Collections.Specialized;
using System.Configuration.Fakes;
using ECN.MarketinAutomation.Tests.Models.PostModels.Controls;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;

namespace ECN.Webservice.Tests
{
    [TestFixture]
    public partial class ListManagerTest : ManagerTestBase
    {
        private IDisposable _shimObject;
        private const string _ECNAccesskey = "2B4E4F20-B642-457D-8407-DB82F1BDC401";
        private readonly NameValueCollection _appSettings = new NameValueCollection();

        [SetUp]
        public void SetUp()
        {
            _shimObject = ShimsContext.Create();
            _appSettings.Add("KMCommon", "KMCommon");
            ShimConfigurationManager.AppSettingsGet = () => _appSettings;
        }

        [TearDown]
        public void TearDown()
        {
            _appSettings.Clear();
            _shimObject.Dispose();
        }
    }
}
