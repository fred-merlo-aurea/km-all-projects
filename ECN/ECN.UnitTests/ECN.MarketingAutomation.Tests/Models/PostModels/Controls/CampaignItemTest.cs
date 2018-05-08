using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using ecn.MarketingAutomation.Models.PostModels.ECN_Objects;
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
    public class CampaignItemTest: ControlBaseTest
    {
        private CampaignItem _control;

        [SetUp]
        public void Setup()
        {
            _control = new CampaignItem();
            InitParentControls();
            InitMocks();
            _control.GroupManager = _groupManagerMock.Object;
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
                Enums.MarketingAutomationControlType.CampaignItem,
                "Campaign Item",
                string.Empty,
                ControlConsts.AllignCenterMiddle,
                ControlConsts.DefaultVeryLargeWidth);
        }

        [Test]
        public void DefaultInstanceSerialization_NewInstanceCreatedAndSerializedToJson_CorrectJsonGenerated()
        {
            // Act
            var json = JsonConvert.SerializeObject(_control);
            var actuallJObject = JObject.Parse(json);
            var expectedJObject = JObject.Parse(ControlTestConsts.CampaignItemDefaultExpectedJson);

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
            var expectedJObject = JObject.Parse(ControlTestConsts.CampaignItemModifiedExpectedJson);

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
                _control.Validate(new User());
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
                _control.Validate(new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, ControlConsts.ControlNameTooLongErrorMessage).ShouldBeTrue();
        }
        
        [Test]
        public void Validate_SendTimeInThePast_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.SendTime = DateTime.Today.AddDays(-1);
            _control.CreateCampaignItem = true;
            var expectedErrorMessage = string.Format(
                ControlConsts.CampaignItemSendTimeInThePastErrorMessage,
                _control.Text);

            // Act
            try
            {
                _control.Validate(new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedErrorMessage).ShouldBeTrue();
        }

        [Test]
        public void Validate_CreateCampaignItemTrueAndCampaignItemNameIsEmpty_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.CreateCampaignItem = true;
            _control.CampaignItemName = String.Empty;
            var expectedError = string.Format(ControlConsts.CampaignItemNameEmptyErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_CreateCampaignItemTrueAndCampaignItemAlreadyExists_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);

            _control.CreateCampaignItem = true;
            _control.ECNID = -1;
            _control.CampaignID = 1;
            _control.CampaignItemManager = GetCampaignItemMock(GetCampaignItem(), true).Object;
            var expectedError = string.Format(ControlConsts.CampaignItemAlreadyExistsErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }
        
        [Test]
        public void Validate_CreateCampaignItemTrueAndCampaignNameEmpty_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.CreateCampaignItem = true;
            _control.CreateCampaign = true;
            _control.CampaignID = -1;
            _control.CampaignName = string.Empty;
            var expectedError = string.Format(ControlConsts.CampaignNameEmptyErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_CreateCampaignItemTrueAndCampaignNameAlreadyExists_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.CampaignName = "SampleCampaignName";
            _control.CreateCampaignItem = true;
            _control.CreateCampaign = true;
            _control.CampaignID = -1;
            _control.CampaignManager = GetCampaignMock(true).Object;

            var expectedError = string.Format(ControlConsts.CampaignNameAlreadyExistsErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_CreateCampaignItemTrueAndCampaignNotSelected_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.CampaignName = "SampleCampaignName";
            _control.CreateCampaignItem = true;
            _control.CreateCampaign = false;
            _control.CampaignID = -1;
            _control.CampaignManager = GetCampaignMock(true).Object;

            var expectedError = string.Format(ControlConsts.SelectCampaignErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_CreateCampaignItemTrueAndCampaignItemTemplateIdNotSet_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.CreateCampaignItem = true;
            _control.CampaignItemTemplateID = 0;
            _control.UseCampaignItemTemplate = true;
            var expectedError = string.Format(ControlConsts.CampaignTemplateEmptyErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_CreateCampaignItemTrueAndFromEmailIsEmpty_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.CreateCampaignItem = true;
            _control.FromEmail = String.Empty;
            var expectedError = string.Format(ControlConsts.FromEmailEmptyErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_CreateCampaignItemTrueFromAndEmailNotValid_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.CreateCampaignItem = true;
            _control.EmailManager = GetEmailManagerMock(false).Object;
            var expectedError = string.Format(ControlConsts.FromEmailInvalidErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_CreateCampaignItemTrueAndFromNameIsEmpty_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.CreateCampaignItem = true;
            _control.FromName = String.Empty;
            var expectedError = string.Format(ControlConsts.FromNameEmptyErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_CreateCampaignItemTrueAndEmailSubjectIsEmpty_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.CreateCampaignItem = true;
            _control.EmailSubject = String.Empty;
            var expectedError = string.Format(ControlConsts.EmailSubjectEmptyErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_CreateCampaignItemTrueAndEmailSubjectTooLong_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.CreateCampaignItem = true;
            _control.EmailSubject = new string('A', 256);
            var expectedError = string.Format(ControlConsts.EmailSubjectTooLongErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_CreateCampaignItemTrueAndReplyToIsEmpty_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.CreateCampaignItem = true;
            _control.ReplyTo = String.Empty;
            var expectedError = string.Format(ControlConsts.ReplyToEmptyErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_CreateCampaignItemTrueAndReplyToNotValid_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.CreateCampaignItem = true;
            _control.EmailManager = GetEmailManagerMock(false).Object;
            var expectedError = string.Format(ControlConsts.ReplyToInvalidErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_CreateCampaignItemTrueAndMessageIdNotSet_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.CreateCampaignItem = true;
            _control.MessageID = 0;
            var expectedError = string.Format(ControlConsts.MessageEmptyErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_CreateCampaignItemTrueAndMessageDoesNotExist_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.CreateCampaignItem = true;
            _control.MessageID = 123;
            _control.LayoutManager = GetLayoutManagerMock(false).Object;
            var expectedError = string.Format(ControlConsts.MessageDoesNotExistErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_CreateCampaignItemTrueAndMessageIsArchived_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.CreateCampaignItem = true;
            _control.MessageID = 123;
            _control.LayoutManager = GetLayoutManagerMock(true, true).Object;
            var expectedError = string.Format(ControlConsts.MessageArchivedErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_CreateCampaignItemTrueAndMessageContentNotValidated_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.CreateCampaignItem = true;
            _control.MessageID = 123;
            _control.LayoutManager = GetLayoutManagerMock(true, false, false).Object;
            var expectedError = string.Format(ControlConsts.MessageNotValidErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_CreateCampaignItemTrueAndAllPropertiesAreCorrect_NoExceptionRaised()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.SendTime = DateTime.Today.AddDays(1);
            _control.CampaignID = 123;
            _control.SelectedGroups = new List<GroupSelect>();

            _control.CreateCampaignItem = true;

            // Act
            try
            {
                _control.Validate(new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            raisedException.ShouldBeNull();
        }

        [Test]
        public void Validate_CreateCampaignItemFalseAndCampaignItemNameIsEmpty_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.CreateCampaign = false;
            _control.CampaignItemName = String.Empty;
            var expectedError = string.Format(ControlConsts.SelectCampaignItemErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_CreateCampaignItemFalseAndBlastEmailFromEmpty_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            var campaignItem = GetCampaignItem();
            campaignItem.BlastList.First().Blast.EmailFrom = string.Empty;
            _control.CreateCampaignItem = false;
            _control.CampaignItemManager = GetCampaignItemMock(campaignItem).Object;
            
            var expectedError = string.Format(ControlConsts.FromEmailEmptyErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_CreateCampaignItemFalseAndBlastEmailFromNameEmpty_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            var campaignItem = GetCampaignItem();
            campaignItem.BlastList.First().Blast.EmailFromName = string.Empty;
            _control.CreateCampaignItem = false;
            _control.CampaignItemManager = GetCampaignItemMock(campaignItem).Object;

            var expectedError = string.Format(ControlConsts.FromNameEmptyErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_CreateCampaignItemFalseAndEmailSubjectEmpty_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            var campaignItem = GetCampaignItem();
            campaignItem.BlastList.First().Blast.EmailSubject = string.Empty;
            _control.CreateCampaignItem = false;
            _control.CampaignItemManager = GetCampaignItemMock(campaignItem).Object;

            var expectedError = string.Format(ControlConsts.EmailSubjectEmptyErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }
        
        [Test]
        public void Validate_CreateCampaignItemFalseAndReplyToEmpty_ErrorCreated()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            var campaignItem = GetCampaignItem();
            campaignItem.BlastList.First().Blast.ReplyTo = string.Empty;
            _control.CreateCampaignItem = false;
            _control.CampaignItemManager = GetCampaignItemMock(campaignItem).Object;

            var expectedError = string.Format(ControlConsts.ReplyToEmptyErrorMessage, _control.Text);

            // Act
            try
            {
                _control.Validate(new User());
            }
            catch (ECNException exception)
            {
                raisedException = exception;
            }

            // Assert
            ContainsError(raisedException, expectedError).ShouldBeTrue();
        }

        [Test]
        public void Validate_CreateCampaignItemFalseAllPropertiesAreCorrect_NoExceptionRaised()
        {
            // Arrange
            ECNException raisedException = null;
            InitValidCampaignControlProperties(_control);
            _control.CreateCampaignItem = false;

            // Act
            try
            {
                _control.Validate(new User());
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
