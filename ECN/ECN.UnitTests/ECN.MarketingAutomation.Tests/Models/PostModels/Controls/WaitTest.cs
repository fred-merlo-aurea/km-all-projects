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
    public class WaitTest: ControlBaseTest
    {
        private Wait _control;

        [SetUp]
        public void Setup()
        {
            _control = new Wait();
            InitParentControls();
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
                Enums.MarketingAutomationControlType.Wait,
                ControlConsts.ControlTextWait,
                String.Empty,
                null,
                string.Empty
                );
        }

        [Test]
        public void DefaultInstanceSerialization_NewInstanceCreatedAndSerializedToJson_CorrectJsonGenerated()
        {
            // Act
            var json = JsonConvert.SerializeObject(_control);
            var actuallJObject = JObject.Parse(json);
            var expectedJObject = JObject.Parse(ControlTestConsts.WaitDefaultExpectedJson);

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
            var expectedJObject = JObject.Parse(ControlTestConsts.WaitModifiedExpectedJson);

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
                _control.Validate(_parentForm, GetMarketingAutomation(), new User());
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
                _control.Validate(_parentForm, GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, ControlConsts.ControlNameTooLongErrorMessage).ShouldBeTrue();
        }

        [Test]
        public void Validate_DaysHoursMinutesNotSet_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidVisualControlProperties(_control);
            var errorMessage = string.Format(ControlConsts.InvalidWaitTimeErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(_parentForm, GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, errorMessage).ShouldBeTrue();
        }

        [Test]
        public void Validate_WrongDaysValue_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidVisualControlProperties(_control);
            _control.Days = -1;

            // Act
            try
            {
                _control.Validate(_parentForm, GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, ControlConsts.InvalidDaysErrorMessage).ShouldBeTrue();
        }

        [Test]
        public void Validate_WrongHoursValue_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidVisualControlProperties(_control);
            _control.Hours = -1;

            // Act
            try
            {
                _control.Validate(_parentForm, GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, ControlConsts.InvalidHoursErrorMessage).ShouldBeTrue();
        }

        [Test]
        public void Validate_WrongMinutesValue_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidVisualControlProperties(_control);
            _control.Minutes = -1;

            // Act
            try
            {
                _control.Validate(_parentForm, GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, ControlConsts.InvalidMinutesErrorMessage).ShouldBeTrue();
        }

        [Test]
        public void Validate_ParentCampaignItemAndSendTimeOutsideOfRange_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidVisualControlProperties(_control);
            _control.IsDirty = true;
            _control.Days = 3;

            // Act
            try
            {
                _control.Validate(_parentCampaignItem, GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, ControlConsts.WaitTimeOutsideOfRangeErrorMessage).ShouldBeTrue();
        }

        [Test]
        public void Validate_ParentCampaignItemAndSendTimeInThePast_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidVisualControlProperties(_control);
            _control.IsDirty = true;
            _parentCampaignItem.SendTime = DateTime.Today.AddDays(-1);
            
            // Act
            try
            {
                _control.Validate(_parentCampaignItem, GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, ControlConsts.WaitTimeInThePastErrorMessage).ShouldBeTrue();
        }

        [Test]
        public void Validate_ParentDirectClickAndSendTimeOutsideOfRange_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidVisualControlProperties(_control);
            _control.IsDirty = true;
            _control.Days = 3;

            // Act
            try
            {
                _control.Validate(new Direct_Click(), GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, ControlConsts.WaitTimeOutsideOfRangeErrorMessage).ShouldBeTrue();
        }

        [Test]
        public void Validate_ParentDirectOpenAndSendTimeOutsideOfRange_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidVisualControlProperties(_control);
            _control.IsDirty = true;
            _control.Days = 3;

            // Act
            try
            {
                _control.Validate(new Direct_Open(), GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, ControlConsts.WaitTimeOutsideOfRangeErrorMessage).ShouldBeTrue();
        }

        [Test]
        public void Validate_ParentGroupAndSendTimeOutsideOfRange_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidVisualControlProperties(_control);
            _control.IsDirty = true;
            _control.Days = 3;

            // Act
            try
            {
                _control.Validate(new Group(), GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, ControlConsts.WaitTimeOutsideOfRangeErrorMessage).ShouldBeTrue();
        }

        [Test]
        public void Validate_ParentClickItemAndSendTimeInThePast_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidVisualControlProperties(_control);
            _control.IsDirty = true;
            var parentClick = new Click
            {
                EstSendTime = DateTime.Today.AddDays(-3)
            };

            // Act
            try
            {
                _control.Validate(parentClick, GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, ControlConsts.WaitTimeInThePastErrorMessage).ShouldBeTrue();
        }

        [Test]
        public void Validate_ParentClickAndSendTimeOutsideOfRange_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidVisualControlProperties(_control);
            _control.IsDirty = true;
            _control.Days = 3;
            var parentClick = new Click
            {
                EstSendTime = DateTime.Today
            };

            // Act
            try
            {
                _control.Validate(parentClick, GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, ControlConsts.WaitTimeOutsideOfRangeErrorMessage).ShouldBeTrue();
        }

        [Test]
        public void Validate_AllPropertiesAreCorrect_NoExceptionRaised()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidVisualControlProperties(_control);
            _control.Days = 0;
            _control.Hours = 0;
            _control.Minutes = 0;
            _control.IsDirty = true;

            // Act
            try
            {
                _control.Validate(_parentForm, GetMarketingAutomation(), new User());
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
