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
    public class FormSubmitTest: ControlBaseTest
    {
        private FormSubmit _control;

        [SetUp]
        public void Setup()
        {
            _control = new FormSubmit();
            InitParentControls();
            InitMocks();
            _control.GroupManager = _groupManagerMock.Object;
            _control.LayoutManager = _layoutManagerMock.Object;
            _control.LinkAliasManager = _linkAliasManagerMock.Object;
            _control.EmailManager = _emailManagerMock.Object;
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void Constructor_NewInstanceCreated_DefaultPropertiesInitialized()
        {
            // Assert
            AssertCampaignControlBaseDefaultValues(_control, Enums.MarketingAutomationControlType.FormSubmit);
        }

        [Test]
        public void DefaultInstanceSerialization_NewInstanceCreatedAndSerializedToJson_CorrectJsonGenerated()
        {
            // Act
            var json = JsonConvert.SerializeObject(_control);
            var actuallJObject = JObject.Parse(json);
            var expectedJObject = JObject.Parse(ControlTestConsts.FormSubmitDefaultExpectedJson);

            // Assert
            JToken.DeepEquals(actuallJObject, expectedJObject).ShouldBeTrue();
        }

        [Test]
        public void UpdatedInstanceSerialization_InstancePropertiesUpdatedAndSerializedToJson_CorrectJsonGenerated()
        {
            // Arrange
            UpdateCampaignControlBaseWithTestValues(_control);

            // Act
            var json = JsonConvert.SerializeObject(_control);
            var actuallJObject = JObject.Parse(json);
            var expectedJObject = JObject.Parse(ControlTestConsts.FormSubmitModifiedExpectedJson);

            // Assert
            JToken.DeepEquals(actuallJObject, expectedJObject).ShouldBeTrue();
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
                _control.Validate(_parentWait, false, _parentCampaignItem, _parentGroup, _parentForm, new User());
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
                _control.Validate(_parentWait, false, _parentCampaignItem, _parentGroup, _parentForm, new User());
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
                _control.Validate(_parentWait, false, _parentCampaignItem, _parentGroup, _parentForm, new User());
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
                _control.Validate(_parentWait, false, _parentCampaignItem, _parentGroup, _parentForm, new User());
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
                _control.Validate(_parentWait, false, _parentCampaignItem, _parentGroup, _parentForm, new User());
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
                _control.Validate(_parentWait, false, _parentCampaignItem, _parentGroup, _parentForm, new User());
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
                _control.Validate(_parentWait, false, _parentCampaignItem, _parentGroup, _parentForm, new User());
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
                _control.Validate(_parentWait, false, _parentCampaignItem, _parentGroup, _parentForm, new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_ActualLinkNotSet_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.ActualLink = string.Empty;
            var expectedError = string.Format(ControlConsts.LinkEmptyErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(_parentWait, false, _parentCampaignItem, _parentGroup, _parentForm, new User());
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
            InitValidCampaignControlProperties(_control);
            _control.ActualLink = "SampleActualLink";
            _control.LinkAliasManager = GetLinkAliasManagerMock(false).Object;
            var expectedError = string.Format(ControlConsts.LinkDoesNotExistErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(_parentWait, false, _parentCampaignItem, _parentGroup, _parentForm, new User());
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
                _control.Validate(_parentWait, false, _parentCampaignItem, _parentGroup, _parentForm, new User());
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
                _control.Validate(_parentWait, false, _parentCampaignItem, _parentGroup, _parentForm, new User());
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
                _control.Validate(_parentWait, false, _parentCampaignItem, _parentGroup, _parentForm, new User());
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
                _control.Validate(_parentWait, false, _parentCampaignItem, _parentGroup, _parentForm, new User());
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
                _control.Validate(_parentWait, false, _parentCampaignItem, _parentGroup, _parentForm, new User());
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
                _control.Validate(_parentWait, false, _parentCampaignItem, _parentGroup, _parentForm, new User());
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
                _control.Validate(_parentWait, false, _parentCampaignItem, _parentGroup, _parentForm, new User());
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
            InitValidCampaignControlProperties(_control);
            _control.ActualLink = "SampleActualLink";

            // Act
            try
            {
                _control.Validate(_parentWait, false, _parentCampaignItem, _parentGroup, _parentForm, new User());
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
