using System;
using System.Diagnostics.CodeAnalysis;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using ECN_Framework_Common.Objects;
using KMPlatform.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Shouldly;
using Enums = ECN_Framework_Common.Objects.Communicator.Enums;

namespace ECN.MarketinAutomation.Tests.Models.PostModels.Controls
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FormTest: ControlBaseTest
    {
        private Form _control;

        [SetUp]
        public void Setup()
        {
            _control = new Form();
            InitParentControls();
            InitMocks();
            _control.LinkAliasManager = _linkAliasManagerMock.Object;
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
                Enums.MarketingAutomationControlType.Form,
                null,
                string.Empty,
                null,
                ControlConsts.DefaultContentColorBlue,
                ControlConsts.DefaultLargeWidth);
        }

        [Test]
        public void DefaultInstanceSerialization_NewInstanceCreatedAndSerializedToJson_CorrectJsonGenerated()
        {
            // Act
            var json = JsonConvert.SerializeObject(_control);
            var actuallJObject = JObject.Parse(json);
            var expectedJObject = JObject.Parse(ControlTestConsts.FormDefaultExpectedJson);

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
            var expectedJObject = JObject.Parse(ControlTestConsts.FormModifiedExpectedJson);

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
                _control.Validate(_parentForm, new User());
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
                _control.Validate(_parentForm, new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, ControlConsts.ControlNameTooLongErrorMessage).ShouldBeTrue();
        }

        [Test]
        public void Validate_FormIdNotSet_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidVisualControlProperties(_control);
            _control.FormID = 0;

            // Act
            try
            {
                _control.Validate(_parentForm, new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, ControlConsts.PleaseSelectFormErrorMessage).ShouldBeTrue();
        }

        [Test]
        public void Validate_SpecificLinkNotSet_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidVisualControlProperties(_control);
            _control.SpecificLink = string.Empty;
            var expectedError = string.Format(ControlConsts.LinkEmptyErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(_parentForm, new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_ActualLinkDoesNotExist_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidVisualControlProperties(_control);
            _control.SpecificLink = "SampleSpecificLink";
            _control.ActualLink = "SampleActualLink";
            _control.LinkAliasManager = GetLinkAliasManagerMock(false).Object;
            var expectedError = string.Format(ControlConsts.LinkDoesNotExistErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(_parentForm, new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }
        
        [Test]
        public void Validate_AllPropertiesAreCorrect_NoExceptionRaised()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidVisualControlProperties(_control);
            _control.SpecificLink = "SampleSpecificLink";
            _control.FormID = 123;

            // Act
            try
            {
                _control.Validate(_parentForm, new User());
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
