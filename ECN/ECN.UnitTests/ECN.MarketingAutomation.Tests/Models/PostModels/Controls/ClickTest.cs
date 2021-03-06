﻿using System;
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
    public class ClickTest: ControlBaseTest
    {
        private Click _control;

        [SetUp]
        public void Setup()
        {
            _control = new Click();
            InitParentControls();
            InitMocks();
            _control.LayoutManager = _layoutManagerMock.Object;
            _control.EmailManager = _emailManagerMock.Object;
            _control.CampaignItemManager = _campaignItemManagerMock.Object;
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void Constructor_NewInstanceCreated_DefaultPropertiesInitialized()
        {
            // Assert
            AssertCampaignControlBaseDefaultValues(
                _control,
                Enums.MarketingAutomationControlType.Click,
                ControlConsts.ControlTextClick);
        }

        [Test]
        public void DefaultInstanceSerialization_NewInstanceCreatedAndSerializedToJson_CorrectJsonGenerated()
        {
            // Act
            var json = JsonConvert.SerializeObject(_control);
            var actuallJObject = JObject.Parse(json);
            var expectedJObject = JObject.Parse(ControlTestConsts.ClickDefaultExpectedJson);

            // Assert
            Assert.IsTrue(JToken.DeepEquals(actuallJObject, expectedJObject));
        }

        [Test]
        public void UpdatedInstanceSerialization_InstancePropertiesUpdatedAndSerializedToJson_CorrectJsonGenerated()
        {
            // Arrange
            UpdateCampaignControlBaseWithTestValues(_control);

            // Act
            var json = JsonConvert.SerializeObject(_control);
            var actuallJObject = JObject.Parse(json);
            var expectedJObject = JObject.Parse(ControlTestConsts.ClickModifiedExpectedJson);

            // Assert
            Assert.IsTrue(JToken.DeepEquals(actuallJObject, expectedJObject));
        }

        [Test]
        public void Validate_TextIsEmpty_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.Text = String.Empty;
            var expectedError = string.Format(ControlConsts.ControlNameEmptyErrorMessage, _control.ControlType);

            // Act
            try
            {
                _control.Validate(_parentWait, _parentCampaignItem, GetMarketingAutomation(), new User());
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
            InitValidCampaignControlProperties(_control);
            _control.Text = new string('A', ControlConsts.MaxLength + 1);

            // Act
            try
            {
                _control.Validate(_parentWait, _parentCampaignItem, GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, ControlConsts.ControlNameTooLongErrorMessage).ShouldBeTrue();
        }

        [Test]
        public void Validate_CampaignItemNameIsEmpty_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.CampaignItemName = String.Empty;
            var expectedError = string.Format(ControlConsts.CampaignItemNameEmptyErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(_parentWait, _parentCampaignItem, GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_CampaignItemAlreadyExists_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.ECNID = -1;
            _control.CampaignItemManager = GetCampaignItemMock(new ECN_Framework_Entities.Communicator.CampaignItem(), true).Object;
            var expectedError = string.Format(ControlConsts.CampaignItemAlreadyExistsErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(_parentWait, _parentCampaignItem, GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_CampaignItemTemplateIdNotSet_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.CampaignItemTemplateID = 0;
            _control.UseCampaignItemTemplate = true;
            var expectedError = string.Format(ControlConsts.CampaignTemplateEmptyErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(_parentWait, _parentCampaignItem, GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_MessageIdNotSet_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.MessageID = 0;
            var expectedError = string.Format(ControlConsts.MessageEmptyErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(_parentWait, _parentCampaignItem, GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_MessageDoesNotExist_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.MessageID = 123;
            _control.LayoutManager = GetLayoutManagerMock(false).Object;
            var expectedError = string.Format(ControlConsts.MessageDoesNotExistErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(_parentWait, _parentCampaignItem, GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_MessageIsArchived_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.MessageID = 123;
            _control.LayoutManager = GetLayoutManagerMock(true, true).Object;
            var expectedError = string.Format(ControlConsts.MessageArchivedErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(_parentWait, _parentCampaignItem, GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_MessageContentNotValidated_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.MessageID = 123;
            _control.LayoutManager = GetLayoutManagerMock(true, false, false).Object;
            var expectedError = string.Format(ControlConsts.MessageNotValidErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(_parentWait, _parentCampaignItem, GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_FromEmailIsEmpty_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.FromEmail = String.Empty;
            var expectedError = string.Format(ControlConsts.FromEmailEmptyErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(_parentWait, _parentCampaignItem, GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_FromEmailNotValid_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.EmailManager = GetEmailManagerMock(false).Object;
            var expectedError = string.Format(ControlConsts.FromEmailInvalidErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(_parentWait, _parentCampaignItem, GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_FromNameIsEmpty_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.FromName = String.Empty;
            var expectedError = string.Format(ControlConsts.FromNameEmptyErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(_parentWait, _parentCampaignItem, GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_EmailSubjectIsEmpty_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.EmailSubject = String.Empty;
            var expectedError = string.Format(ControlConsts.EmailSubjectEmptyErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(_parentWait, _parentCampaignItem, GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_EmailSubjectTooLong_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.EmailSubject = new string('A', 256);
            var expectedError = string.Format(ControlConsts.EmailSubjectTooLongErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(_parentWait, _parentCampaignItem, GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_ReplyToIsEmpty_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.ReplyTo = String.Empty;
            var expectedError = string.Format(ControlConsts.ReplyToEmptyErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(_parentWait, _parentCampaignItem, GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_ReplyToNotValid_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.EmailManager = GetEmailManagerMock(false).Object;
            var expectedError = string.Format(ControlConsts.ReplyToInvalidErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(_parentWait, _parentCampaignItem, GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_SendTimeNotInAllowedRange_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _parentCampaignItem.SendTime = DateTime.Today.AddDays(4);
            _parentWait.WaitTime = 0;

            // Act
            try
            {
                _control.Validate(_parentWait, _parentCampaignItem, GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, "Group Click Email is outside of the Automation date range").ShouldBeTrue();
        }

        [Test]
        public void Validate_SendTimeInThePast_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _parentCampaignItem.SendTime = DateTime.Today.AddDays(-1);
            _parentWait.WaitTime = 0;

            // Act
            try
            {
                _control.Validate(_parentWait, _parentCampaignItem, GetMarketingAutomation(), new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, "Group Click Email Send Time is in the past").ShouldBeTrue();
        }

        [Test]
        public void Validate_AllPropertiesAreCorrect_NoExceptionRaised()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);

            // Act
            try
            {
                _control.Validate(_parentWait, _parentCampaignItem, GetMarketingAutomation(), new User());
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
