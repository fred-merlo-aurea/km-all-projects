using System;
using System.Diagnostics.CodeAnalysis;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using ECN_Framework_Common.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Shouldly;
using Enums = ECN_Framework_Common.Objects.Communicator.Enums;

namespace ECN.MarketinAutomation.Tests.Models.PostModels.Controls
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class GroupTest: ControlBaseTest
    {
        private Group _control;

        [SetUp]
        public void Setup()
        {
            _control = new Group();
            InitParentControls();
            InitMocks();
            _control.GroupManager = _groupManagerMock.Object;
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
                Enums.MarketingAutomationControlType.Group,
                null,
                string.Empty,
                string.Empty,
                string.Empty,
                ControlConsts.DefaultVeryLargeWidth);
        }

        [Test]
        public void DefaultInstanceSerialization_NewInstanceCreatedAndSerializedToJson_CorrectJsonGenerated()
        {
            // Act
            var json = JsonConvert.SerializeObject(_control);
            var actuallJObject = JObject.Parse(json);
            var expectedJObject = JObject.Parse(ControlTestConsts.GroupDefaultExpectedJson);

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
            var expectedJObject = JObject.Parse(ControlTestConsts.GroupModifiedExpectedJson);

            // Assert
            Assert.IsTrue(JToken.DeepEquals(actuallJObject, expectedJObject));
        }

        [Test]
        public void Validate_TextIsEmpty_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidVisualControlProperties(_control);
            _control.Text = String.Empty;
            var expectedError = string.Format(ControlConsts.ControlNameEmptyErrorMessage, _control.ControlType);

            // Act
            try
            {
                _control.Validate();
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_TextTooLong_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidVisualControlProperties(_control);
            _control.Text = new string('A', ControlConsts.MaxLength + 1);

            // Act
            try
            {
                _control.Validate();
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, ControlConsts.ControlNameTooLongErrorMessage).ShouldBeTrue();
        }

        [Test]
        public void Validate_GroupIdNotSet_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidVisualControlProperties(_control);
            _control.GroupID = 0;
            var expectedError = string.Format(
                ControlConsts.PleaseSelectGroupForControlTypeErrorMesage,
                _control.ControlType);

            // Act
            try
            {
                _control.Validate();
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ValidateException(raisedException, ECN_Framework_Common.Objects.Enums.Entity.Group, expectedError);
        }

        [Test]
        public void Validate_GroupDoesNotExist_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidVisualControlProperties(_control);
            _control.GroupID = 123;
            _control.GroupName = "SampleGroupName";
            _control.GroupManager = GetGroupManagerMock(false).Object;
            var expectedError = string.Format(ControlConsts.GroupDoesNotExistErrorMessage, _control.GroupName);

            // Act
            try
            {
                _control.Validate();
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ValidateException(raisedException, ECN_Framework_Common.Objects.Enums.Entity.Group, expectedError);
        }

        [Test]
        public void Validate_GroupArchived_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidVisualControlProperties(_control);
            _control.GroupID = 123;
            _control.GroupName = "SampleGroupName";
            _control.GroupManager = GetGroupManagerMock(true, true).Object;
            var expectedError = string.Format(ControlConsts.ArchivedGroupNotAllowedErrorMessage, _control.GroupName);

            // Act
            try
            {
                _control.Validate();
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ValidateException(raisedException, ECN_Framework_Common.Objects.Enums.Entity.Group, expectedError);
        }

        [Test]
        public void Validate_AllPropertiesAreCorrect_NoExceptionRaised()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidVisualControlProperties(_control);
            _control.GroupID = 123;

            // Act
            try
            {
                _control.Validate();
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            raisedException.ShouldBeNull();
        }
    }
}
