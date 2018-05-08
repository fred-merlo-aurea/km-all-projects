using System;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Web.Fakes;
using System.Web.UI.Fakes;
using ecn.communicator.main.ECNWizard;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;

namespace ECN.Communicator.Tests.Main.ECNWizard
{
    /// <summary>
    /// Unit Test class for <see cref="wizardSetup"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class WizardSetupTest
    {
        private const string CampaignItemId = "CampaignItemID";
        private const string GetCampaignItemType = "getCampaignItemType";
        private const string GetCampaignItemId = "getCampaignItemID";
        private const int TestValue = 10;
        private const string CampaignItemType = "CampaignItemType";
        private wizardSetup _testObject;
        private PrivateObject _privateObject;
        private IDisposable _shimsContext;
        private NameValueCollection _queryString;

        [SetUp]
        public void InitTest()
        {
            _shimsContext = ShimsContext.Create();
            ShimPage.AllInstances.RequestGet = _ => new ShimHttpRequest();
            _queryString = new NameValueCollection();
            ShimHttpRequest.AllInstances.QueryStringGet = _ => _queryString;
            _testObject = new wizardSetup();
            _privateObject = new PrivateObject(_testObject);
        }

        [TearDown]
        public void CleanUp()
        {
            _queryString = null;
            _shimsContext.Dispose();
            _testObject.Dispose();
            _privateObject = null;
        }

        [Test]
        public void CampaignItemId_DefaultValueSetValue_ReturnsZeroOrSetValue()
        {
            // Arrange
            var getCampaignItemIdValue = new Func<int>(() => (int)_privateObject.GetFieldOrProperty(CampaignItemId));

            // Act
            var defaultValue = getCampaignItemIdValue();
            _privateObject.SetFieldOrProperty(CampaignItemId, TestValue);

            // Assert
            _testObject.ShouldSatisfyAllConditions(
                () => defaultValue.ShouldBe(0),
                () => getCampaignItemIdValue().ShouldBe(TestValue));
        }

        [Test]
        public void GetCampaignItemType_DefaultValue_ReturnsEmptyString()
        {
            // Arrange
            var getCampaignItemType = new Func<string>(() => (string)_privateObject.Invoke(GetCampaignItemType));

            // Act
            _queryString = null;

            // Assert
            getCampaignItemType().ShouldBeEmpty();
        }

        [Test]
        public void GetCampaignItemType_SetQueryString_ReturnsSetValue()
        {
            // Arrange
            var getCampaignItemType = new Func<string>(() => (string)_privateObject.Invoke(GetCampaignItemType));

            // Act
            _queryString.Add(CampaignItemType, CampaignItemType);

            // Assert
            getCampaignItemType().ShouldBe(CampaignItemType);
        }

        [Test]
        public void GetCampaignItemId_DefaultValue_ReturnsZero()
        {
            // Arrange
            var getCampaignItemId = new Func<int>(() => (int)_privateObject.Invoke(GetCampaignItemId));

            // Act
            _queryString = null;

            // Assert
            getCampaignItemId().ShouldBe(0);
        }

        [Test]
        public void GetCampaignItemId_SetQueryString_ReturnsSetValue()
        {
            // Arrange
            var getCampaignItemId = new Func<int>(() => (int)_privateObject.Invoke(GetCampaignItemId));

            // Act
            _queryString.Add(CampaignItemId, TestValue.ToString());

            // Assert
            getCampaignItemId().ShouldBe(TestValue);
        }
    }
}
