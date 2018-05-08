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
    public class Direct_ClickTest: ControlBaseTest
    {
        private Direct_Click _control;

        [SetUp]
        public void Setup()
        {
            _control = new Direct_Click();
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
            AssertCampaignControlBaseDefaultValues(_control, Enums.MarketingAutomationControlType.Direct_Click);
        }

        [Test]
        public void DefaultInstanceSerialization_NewInstanceCreatedAndSerializedToJson_CorrectJsonGenerated()
        {
            // Act
            var json = JsonConvert.SerializeObject(_control);
            var actuallJObject = JObject.Parse(json);
            var expectedJObject = JObject.Parse(ControlTestConsts.DirectClickDefaultExpectedJson);

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
            var expectedJObject = JObject.Parse(ControlTestConsts.DirectClickModifiedExpectedJson);

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
        public void Validate_NotAnyLinkAndActualLinkNotSet_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.ActualLink = string.Empty;
            _control.AnyLink = false;
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
        public void Validate_NotAnyLinkAndActualLinkDoesNotExist_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.AnyLink = false;
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

        [Test]
        public void Validate_AnyLinkTrueAndActualLinkNotSet_NoErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.ActualLink = string.Empty;
            _control.AnyLink = true;

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

        [Test]
        public void Validate_AnyLinkTrueAndActualLinkDoesNotExist_NoErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.AnyLink = true;
            _control.ActualLink = "SampleActualLink";
            _control.LinkAliasManager = GetLinkAliasManagerMock(false).Object;

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
