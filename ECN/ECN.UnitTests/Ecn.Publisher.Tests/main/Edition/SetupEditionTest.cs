using System.Diagnostics.CodeAnalysis;
using System.IO;
using ecn.publisher.main.Edition;
using MasterPages = ecn.publisher.MasterPages;
using ecn.publisher.main.Edition.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Entities.Accounts;
using ECN.Tests.Helpers;
using KMPlatform.Entity;
using KM.Common.Utilities.Email;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using NTestContext = NUnit.Framework.TestContext;
using EntityEdition = ECN_Framework_Entities.Publisher.Edition;

namespace Ecn.Publisher.Tests.main.Edition
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public partial class SetupEditionTest : PageHelper
    {
        private const string EditionIDKey = "EditionID";
        private const string SampleBaseChannel = "SampleBaseChannel";
        private const string SampleCustomer = "SampleCustomer";
        private const int TestEditionID = 1;
        private const string CompletedStep = "CompletedStep";
        private const string EditionId = "EditionID";
        private static readonly string TestBasePath = NTestContext.CurrentContext.TestDirectory + "\\customers\\";
        private SetupEdition _testEntity;
        private PrivateObject _privateTestObject;
        private bool _isPdfConverted;
        private bool _isStepLoaded;
        private EntityEdition _savedEdition;
        private EmailMessage _sendEmailMessage;

        [SetUp]
        protected override void SetPageSessionContext()
        {
            base.SetPageSessionContext();
            _testEntity = new SetupEdition();
            _privateTestObject = new PrivateObject(_testEntity);
            InitializeAllControls(_testEntity);
            InitializeSessionFakes();
            _isPdfConverted = false;
            _isStepLoaded = false;
            _savedEdition = null;
            _sendEmailMessage = null;
        }

        private void InitializeSessionFakes()
        {
            var shimSession = new ShimECNSession();
            shimSession.Instance.CurrentCustomer = new Customer
            {
                CustomerID = 1,
                CustomerName = SampleCustomer
            };
            shimSession.Instance.CurrentUser = new User()
            {
                UserID = 1,
                UserName = "TestUser",
                IsActive = true,
                CustomerID = 1,
            };
            shimSession.Instance.CurrentBaseChannel = new BaseChannel
            {
                BaseChannelID = 1,
                BaseChannelName = SampleBaseChannel
            };
            ShimECNSession.CurrentSession = () => shimSession.Instance;
            ShimSetupEdition.AllInstances.MasterGet = (s) => new MasterPages.Publisher();
            MasterPages.Fakes.ShimPublisher.AllInstances.UserSessionGet = (m) => shimSession.Instance;
        }

        [TearDown]
        public new void CleanUp()
        {
            base.CleanUp();
            if (Directory.Exists(TestBasePath))
            {
                Directory.Delete(TestBasePath, true);
            }
        }

        [TestCase(CompletedStep, 1)]
        [TestCase(EditionId, 0)]
        public void IntegerProperty_DefaultValueSetValue_ReturnsDefaultValueOrSetValue(string propertyName, int defaultValueExp)
        {
            // Arrange
            using(var testObject = new SetupEdition())
            {
                var privateObject = new PrivateObject(testObject);

                // Act
                var defaultValue = (int)privateObject.GetFieldOrProperty(propertyName);
                privateObject.SetFieldOrProperty(propertyName, 10);

                // Assert
                testObject.ShouldSatisfyAllConditions(
                    () => defaultValue.ShouldBe(defaultValueExp),
                    () => ((int)privateObject.GetFieldOrProperty(propertyName)).ShouldBe(10));
            }
        }
    }
}
