using System.Diagnostics.CodeAnalysis;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using ECN_Framework_Common.Objects.Communicator;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace ECN.MarketinAutomation.Tests.Models.PostModels.Controls
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class StartTest: ControlBaseTest
    {
        private const decimal StartControlHeight = 38;
        private const decimal StartControlWidth = 138;

        private Start _control;

        [SetUp]
        public void Setup()
        {
            _control = new Start();
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void Constructor_NewInstanceCreated_DefaultPropertiesInitialized()
        {
            // Assert
            AssertVisualControlBaseDefaultValues(
                _control,
                Enums.MarketingAutomationControlType.Start,
                null,
                ControlConsts.ControlTextStart,
                ControlConsts.AllignCenterMiddle,
                ControlConsts.DefaultContentColorBlue,
                StartControlWidth,
                StartControlHeight);
        }

        [Test]
        public void DefaultInstanceSerialization_NewInstanceCreatedAndSerializedToJson_CorrectJsonGenerated()
        {
            // Act
            var json = JsonConvert.SerializeObject(_control);
            var actuallJObject = JObject.Parse(json);
            var expectedJObject = JObject.Parse(ControlTestConsts.StartDefaultExpectedJson);

            // Assert
            Assert.IsTrue(JToken.DeepEquals(actuallJObject, expectedJObject));
        }

        [Test]
        public void UpdatedInstanceSerialization_InstancePropertiesUpdatedAndSerializedToJson_CorrectJsonGenerated()
        {
            // Arrange
            UpdateVisualControlBaseWithTestValues(_control);

            // Act
            var json = JsonConvert.SerializeObject(_control);
            var actuallJObject = JObject.Parse(json);
            var expectedJObject = JObject.Parse(ControlTestConsts.StartModifiedExpectedJson);

            // Assert
            Assert.IsTrue(JToken.DeepEquals(actuallJObject, expectedJObject));
        }
    }
}
