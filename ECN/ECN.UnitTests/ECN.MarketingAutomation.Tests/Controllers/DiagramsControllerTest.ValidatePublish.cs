using System.Collections.Generic;
using ecn.MarketingAutomation.Models.PostModels;
using ECN_Framework_Common.Objects;
using NUnit.Framework;
using Shouldly;

namespace ecn.MarketingAutomation.Tests.Controllers
{
    public partial class DiagramsControllerTest
    {
        private const string ValidatePublishMethodName = "ValidatePublish";
        private const string UTExceptionMessage = "UT Exception";

        [Test]
        public void ValidatePublish_WhenControlTypeCampaignItem_ThrowsAndSetsECNException()
        {
            // Arrange
            var campaignItem = GetCampaignItem(editableRemove: false, maControlId: 1);
            var anotherCampaignItem = GetCampaignItem();
            var controls = new List<ControlBase> { campaignItem, anotherCampaignItem };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(UTExceptionMessage, new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _diagramsControllerPrivateObject.Invoke(ValidatePublishMethodName, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldNotBeNull();
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.Message.ShouldBe(UTExceptionMessage),
                () => refEcnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business));
            refEcnExp.ErrorList.ShouldNotBeEmpty();
            refEcnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(
                    x => x.ErrorMessage.Contains("Missing End Control. Every automation branch requires an End Control.")),
                () => ecnExp.ErrorList.ShouldContain(
                    x => x.ErrorMessage.Contains("Please select a Campaign Item")),
                () => ecnExp.ErrorList.ShouldContain(
                    x => x.ErrorMessage.Contains("Missing End Control. Every automation branch requires an End Control.")));
        }

        [Test]
        public void ValidatePublish_WhenControlTypeClick_ThrowsAndSetsECNException()
        {
            // Arrange
            var clickControl = ConfigureFakesAndGetClickControl();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(UTExceptionMessage, new List<ECNError>());
            
            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _diagramsControllerPrivateObject.Invoke(ValidatePublishMethodName, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldNotBeNull();
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.Message.ShouldBe(UTExceptionMessage),
                () => refEcnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business));
            refEcnExp.ErrorList.ShouldNotBeEmpty();
            refEcnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Missing End Control. Every automation branch requires an End Control.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Message for Group Email Click")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a Campaign Item Name for Group Email Click")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Email is missing for Group Email Click")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Name is missing for Group Email Click")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Email Subject is missing for Group Email Click")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Reply To is missing for Group Email Click")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Parent has not been set up for Group Email Click")));
        }

        [Test]
        public void ValidatePublish_WhenControlTypeDirectClick_ThrowsAndSetsECNException()
        {
            // Arrange
            var clickControl = ConfigureFakesAndGetDirectClick();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(UTExceptionMessage, new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _diagramsControllerPrivateObject.Invoke(ValidatePublishMethodName, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldNotBeNull();
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.Message.ShouldBe(UTExceptionMessage),
                () => refEcnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business));
            refEcnExp.ErrorList.ShouldNotBeEmpty();
            refEcnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Missing End Control. Every automation branch requires an End Control.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a name for Direct_Click")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a Campaign Item Name for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Message for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Email is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Name is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Email Subject is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Reply To is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Parent has not been set up for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Link Alias for ")));
        }

        [Test]
        public void ValidatePublish_WhenControlTypeDirectClickAndWaitNull_ThrowsAndSetsECNException()
        {
            // Arrange
            var clickControl = ConfigureFakesAndGetDirectClick(isWaitNull: true);
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(UTExceptionMessage, new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _diagramsControllerPrivateObject.Invoke(ValidatePublishMethodName, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldNotBeNull();
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.Message.ShouldBe(UTExceptionMessage),
                () => refEcnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business));
            refEcnExp.ErrorList.ShouldNotBeEmpty();
            refEcnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Missing End Control. Every automation branch requires an End Control.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a name for Direct_Click")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a Campaign Item Name for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Message for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Email is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Name is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Email Subject is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Reply To is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Parent has not been set up for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Link Alias for ")));
        }

        [Test]
        public void ValidatePublish_WhenControlTypeForm_ThrowsAndSetsECNException()
        {
            // Arrange
            var clickControl = ConfigureFakesAndGetForm();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(UTExceptionMessage, new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _diagramsControllerPrivateObject.Invoke(ValidatePublishMethodName, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldNotBeNull();
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.Message.ShouldBe(UTExceptionMessage),
                () => refEcnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business));
            refEcnExp.ErrorList.ShouldNotBeEmpty();
            refEcnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Missing End Control. Every automation branch requires an End Control.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a name for Form")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Form")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Link Alias for ")));
        }

        [Test]
        public void ValidatePublish_WhenControlTypeFormSubmittClick_ThrowsAndSetsECNException()
        {
            // Arrange
            var clickControl = ConfigureFakesAndGetFormSubmit();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(UTExceptionMessage, new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _diagramsControllerPrivateObject.Invoke(ValidatePublishMethodName, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldNotBeNull();
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.Message.ShouldBe(UTExceptionMessage),
                () => refEcnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business));
            refEcnExp.ErrorList.ShouldNotBeEmpty();
            refEcnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Missing End Control. Every automation branch requires an End Control.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a name for FormSubmit")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a Campaign Item Name for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Message for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Link Alias for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Email is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Name is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Email Subject is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Reply To is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Parent has not been set up for ")));
        }

        [Test]
        public void ValidatePublish_WhenControlTypeFormSubmitAndWaitNull_ThrowsAndSetsECNException()
        {
            // Arrange
            var clickControl = ConfigureFakesAndGetFormSubmit(isWaitNull: true);
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(UTExceptionMessage, new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _diagramsControllerPrivateObject.Invoke(ValidatePublishMethodName, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldNotBeNull();
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.Message.ShouldBe(UTExceptionMessage),
                () => refEcnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business));
            refEcnExp.ErrorList.ShouldNotBeEmpty();
            refEcnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Missing End Control. Every automation branch requires an End Control.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a name for FormSubmit")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a Campaign Item Name for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Message for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Link Alias for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Email is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Name is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Email Subject is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Reply To is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Parent has not been set up for ")));
        }

        [Test]
        public void ValidatePublish_WhenControlTypeFormAbandon_ThrowsAndSetsECNException()
        {
            // Arrange
            var clickControl = ConfigureFakesAndGetFormAbandon();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(UTExceptionMessage, new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _diagramsControllerPrivateObject.Invoke(ValidatePublishMethodName, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldNotBeNull();
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.Message.ShouldBe(UTExceptionMessage),
                () => refEcnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business));
            refEcnExp.ErrorList.ShouldNotBeEmpty();
            refEcnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Missing End Control. Every automation branch requires an End Control.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a name for FormAbandon")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a Campaign Item Name for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Message for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Link Alias for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Email is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Name is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Email Subject is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Reply To is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Parent has not been set up for ")));
        }

        [Test]
        public void ValidatePublish_WhenControlTypeFormAbandonAndWaitNull_ThrowsAndSetsECNException()
        {
            // Arrange
            var clickControl = ConfigureFakesAndGetFormAbandon(isWaitNull: true);
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(UTExceptionMessage, new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _diagramsControllerPrivateObject.Invoke(ValidatePublishMethodName, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldNotBeNull();
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.Message.ShouldBe(UTExceptionMessage),
                () => refEcnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business));
            refEcnExp.ErrorList.ShouldNotBeEmpty();
            refEcnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Missing End Control. Every automation branch requires an End Control.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a name for FormAbandon")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a Campaign Item Name for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Message for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Link Alias for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Email is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Name is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Email Subject is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Reply To is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Parent has not been set up for ")));
        }

        [Test]
        public void ValidatePublish_WhenControlTypeDirectOpen_ThrowsAndSetsECNException()
        {
            // Arrange
            var clickControl = ConfigureFakesAndGetDirectOpen();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(UTExceptionMessage, new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _diagramsControllerPrivateObject.Invoke(ValidatePublishMethodName, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldNotBeNull();
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.Message.ShouldBe(UTExceptionMessage),
                () => refEcnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business));
            refEcnExp.ErrorList.ShouldNotBeEmpty();
            refEcnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Missing End Control. Every automation branch requires an End Control.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a name for Direct_Open")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a Campaign Item Name for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Message for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Email is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Name is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Email Subject is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Reply To is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Parent has not been set up for ")));
        }

        [Test]
        public void ValidatePublish_WhenControlTypeDirectOpenAndWaitNull_ThrowsAndSetsECNException()
        {
            // Arrange
            var clickControl = ConfigureFakesAndGetDirectOpen(isWaitNull: true);
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(UTExceptionMessage, new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _diagramsControllerPrivateObject.Invoke(ValidatePublishMethodName, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldNotBeNull();
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.Message.ShouldBe(UTExceptionMessage),
                () => refEcnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business));
            refEcnExp.ErrorList.ShouldNotBeEmpty();
            refEcnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Missing End Control. Every automation branch requires an End Control.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a name for Direct_Open")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a Campaign Item Name for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Message for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Email is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Name is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Email Subject is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Reply To is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Parent has not been set up for ")));
        }

        [Test]
        public void ValidatePublish_WhenControlTypeDirectNoOpen_ThrowsAndSetsECNException()
        {
            // Arrange
            var clickControl = ConfigureFakesAndGetDirectNoOpen();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(UTExceptionMessage, new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _diagramsControllerPrivateObject.Invoke(ValidatePublishMethodName, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldNotBeNull();
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.Message.ShouldBe(UTExceptionMessage),
                () => refEcnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business));
            refEcnExp.ErrorList.ShouldNotBeEmpty();
            refEcnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Missing End Control. Every automation branch requires an End Control.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a name for Direct_NoOpen")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a Campaign Item Name for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Message for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Email is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Name is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Email Subject is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Reply To is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Parent has not been set up for ")));
        }

        [Test]
        public void ValidatePublish_WhenControlTypeDirectNoOpenAndWaitNull_ThrowsAndSetsECNException()
        {
            // Arrange
            var clickControl = ConfigureFakesAndGetDirectNoOpen(isWaitNull: true);
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(UTExceptionMessage, new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _diagramsControllerPrivateObject.Invoke(ValidatePublishMethodName, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldNotBeNull();
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.Message.ShouldBe(UTExceptionMessage),
                () => refEcnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business));
            refEcnExp.ErrorList.ShouldNotBeEmpty();
            refEcnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Missing End Control. Every automation branch requires an End Control.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a name for Direct_NoOpen")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a Campaign Item Name for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Message for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Email is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Name is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Email Subject is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Reply To is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Parent has not been set up for ")));
        }

        [Test]
        public void ValidatePublish_WhenControlTypeEnd_ThrowsAndSetsECNException()
        {
            // Arrange
            var clickControl = ConfigureFakesAndGetEnd(isWaitNull: true);
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(UTExceptionMessage, new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _diagramsControllerPrivateObject.Invoke(ValidatePublishMethodName, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldNotBeNull();
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.Message.ShouldBe(UTExceptionMessage),
                () => refEcnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business));
            refEcnExp.ErrorList.ShouldBeEmpty();
        }

        [Test]
        public void ValidatePublish_WhenControlTypeGroup_ThrowsAndSetsECNException()
        {
            // Arrange
            var clickControl = ConfigureFakesAndGetGroup();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(UTExceptionMessage, new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _diagramsControllerPrivateObject.Invoke(ValidatePublishMethodName, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldNotBeNull();
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.Message.ShouldBe(UTExceptionMessage),
                () => refEcnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business));
            refEcnExp.ErrorList.ShouldNotBeEmpty();
            refEcnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Missing End Control. Every automation branch requires an End Control.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a name for Group")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Group for Group control")));
        }

        [Test]
        public void ValidatePublish_WhenControlTypeNoClick_ThrowsAndSetsECNException()
        {
            // Arrange
            var clickControl = ConfigureFakesAndGetNoClick();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(UTExceptionMessage, new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _diagramsControllerPrivateObject.Invoke(ValidatePublishMethodName, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldNotBeNull();
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.Message.ShouldBe(UTExceptionMessage),
                () => refEcnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business));
            refEcnExp.ErrorList.ShouldNotBeEmpty();
            refEcnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Missing End Control. Every automation branch requires an End Control.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a Campaign Item Name for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a name for NoClick")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Message for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Email is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Name is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Email Subject is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Reply To is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Parent has not been set up for ")));
        }

        [Test]
        public void ValidatePublish_WhenControlTypeNoOpen_ThrowsAndSetsECNException()
        {
            // Arrange
            var clickControl = ConfigureFakesAndGetNoOpen();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(UTExceptionMessage, new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _diagramsControllerPrivateObject.Invoke(ValidatePublishMethodName, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldNotBeNull();
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.Message.ShouldBe(UTExceptionMessage),
                () => refEcnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business));
            refEcnExp.ErrorList.ShouldNotBeEmpty();
            refEcnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Missing End Control. Every automation branch requires an End Control.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a Campaign Item Name for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a name for NoOpen")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Message for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Email is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Name is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Email Subject is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Reply To is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Parent has not been set up for ")));
        }

        [Test]
        public void ValidatePublish_WhenControlTypeNotSent_ThrowsAndSetsECNException()
        {
            // Arrange
            var clickControl = ConfigureFakesAndGetNotSent();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(UTExceptionMessage, new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _diagramsControllerPrivateObject.Invoke(ValidatePublishMethodName, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldNotBeNull();
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.Message.ShouldBe(UTExceptionMessage),
                () => refEcnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business));
            refEcnExp.ErrorList.ShouldNotBeEmpty();
            refEcnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Missing End Control. Every automation branch requires an End Control.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a Campaign Item Name for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a name for NotSent")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Message for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Email is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Name is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Email Subject is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Reply To is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Parent has not been set up for ")));
        }

        [Test]
        public void ValidatePublish_WhenControlTypeOpen_ThrowsAndSetsECNException()
        {
            // Arrange
            var clickControl = ConfigureFakesAndGetOpen();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(UTExceptionMessage, new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _diagramsControllerPrivateObject.Invoke(ValidatePublishMethodName, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldNotBeNull();
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.Message.ShouldBe(UTExceptionMessage),
                () => refEcnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business));
            refEcnExp.ErrorList.ShouldNotBeEmpty();
            refEcnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Missing End Control. Every automation branch requires an End Control.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a Campaign Item Name for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a name for Open")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Message for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Email is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Name is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Email Subject is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Reply To is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Parent has not been set up for ")));
        }

        [Test]
        public void ValidatePublish_WhenControlTypeOpenNoClick_ThrowsAndSetsECNException()
        {
            // Arrange
            var clickControl = ConfigureFakesAndGetOpenNoClick();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(UTExceptionMessage, new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _diagramsControllerPrivateObject.Invoke(ValidatePublishMethodName, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldNotBeNull();
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.Message.ShouldBe(UTExceptionMessage),
                () => refEcnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business));
            refEcnExp.ErrorList.ShouldNotBeEmpty();
            refEcnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Missing End Control. Every automation branch requires an End Control.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a name for Open_NoClick")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a Campaign Item Name for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Message for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Email is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Name is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Email Subject is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Reply To is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Parent has not been set up for ")));
        }

        [Test]
        public void ValidatePublish_WhenControlTypeSent_ThrowsAndSetsECNException()
        {
            // Arrange
            var clickControl = ConfigureFakesAndGetSent();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(UTExceptionMessage, new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _diagramsControllerPrivateObject.Invoke(ValidatePublishMethodName, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldNotBeNull();
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.Message.ShouldBe(UTExceptionMessage),
                () => refEcnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business));
            refEcnExp.ErrorList.ShouldNotBeEmpty();
            refEcnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Missing End Control. Every automation branch requires an End Control.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a name for Sent")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a Campaign Item Name for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Message for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Email is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Name is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Email Subject is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Reply To is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Parent has not been set up for ")));
        }

        [Test]
        public void ValidatePublish_WhenControlTypeStart_ThrowsAndSetsECNException()
        {
            // Arrange
            var clickControl = ConfigureFakesAndGetStart();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(UTExceptionMessage, new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _diagramsControllerPrivateObject.Invoke(ValidatePublishMethodName, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldNotBeNull();
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.Message.ShouldBe(UTExceptionMessage),
                () => refEcnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business));
            refEcnExp.ErrorList.ShouldBeEmpty();
        }

        [Test]
        public void ValidatePublish_WhenControlTypeSubscribe_ThrowsAndSetsECNException()
        {
            // Arrange
            var clickControl = ConfigureFakesAndGetSubscribe();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(UTExceptionMessage, new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _diagramsControllerPrivateObject.Invoke(ValidatePublishMethodName, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldNotBeNull();
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.Message.ShouldBe(UTExceptionMessage),
                () => refEcnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business));
            refEcnExp.ErrorList.ShouldNotBeEmpty();
            refEcnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Missing End Control. Every automation branch requires an End Control.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a Campaign Item Name for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a name for Subscribe")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Message for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Email is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Name is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Email Subject is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Reply To is missing for ")));
        }

        [Test]
        public void ValidatePublish_WhenControlTypeSubscribeIsWaitNull_ThrowsAndSetsECNException()
        {
            // Arrange
            var clickControl = ConfigureFakesAndGetSubscribe(isWaitNull: true);
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(UTExceptionMessage, new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _diagramsControllerPrivateObject.Invoke(ValidatePublishMethodName, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldNotBeNull();
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.Message.ShouldBe(UTExceptionMessage),
                () => refEcnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business));
            refEcnExp.ErrorList.ShouldNotBeEmpty();
            refEcnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Missing End Control. Every automation branch requires an End Control.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a Campaign Item Name for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a name for Subscribe")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Message for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Email is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Name is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Email Subject is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Reply To is missing for ")));
        }

        [Test]
        public void ValidatePublish_WhenControlTypeSuppressed_ThrowsAndSetsECNException()
        {
            // Arrange
            var clickControl = ConfigureFakesAndGetSuppressed();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(UTExceptionMessage, new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _diagramsControllerPrivateObject.Invoke(ValidatePublishMethodName, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldNotBeNull();
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.Message.ShouldBe(UTExceptionMessage),
                () => refEcnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business));
            refEcnExp.ErrorList.ShouldNotBeEmpty();
            refEcnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Missing End Control. Every automation branch requires an End Control.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a name for Suppressed")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please enter a Campaign Item Name for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Please select a Message for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Email is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("From Name is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Email Subject is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Reply To is missing for ")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Parent has not been set up for ")));
        }

        [Test]
        public void ValidatePublish_WhenControlTypeUnsubscribe_ThrowsAndSetsECNException()
        {
            // Arrange
            var clickControl = ConfigureFakesAndGetUnsubscribe();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(UTExceptionMessage, new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _diagramsControllerPrivateObject.Invoke(ValidatePublishMethodName, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldNotBeNull();
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.Message.ShouldBe(UTExceptionMessage),
                () => refEcnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business));
            refEcnExp.ErrorList.ShouldNotBeEmpty();
            refEcnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Missing End Control. Every automation branch requires an End Control.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Unable to validate ")));
        }

        [Test]
        public void ValidatePublish_WhenControlTypeUnSubscribeIsWaitNull_ThrowsAndSetsECNException()
        {
            // Arrange
            var clickControl = ConfigureFakesAndGetUnsubscribe(isWaitNull: true);
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(UTExceptionMessage, new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _diagramsControllerPrivateObject.Invoke(ValidatePublishMethodName, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldNotBeNull();
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.Message.ShouldBe(UTExceptionMessage),
                () => refEcnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business));
            refEcnExp.ErrorList.ShouldNotBeEmpty();
            refEcnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Missing End Control. Every automation branch requires an End Control.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Unable to validate ")));
        }

        [Test]
        public void ValidatePublish_WhenControlTypeWait_ThrowsAndSetsECNException()
        {
            // Arrange
            var clickControl = ConfigureFakesAndGetWait();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(UTExceptionMessage, new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _diagramsControllerPrivateObject.Invoke(ValidatePublishMethodName, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldNotBeNull();
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.Message.ShouldBe(UTExceptionMessage),
                () => refEcnExp.ExceptionLayer.ShouldBe(Enums.ExceptionLayer.Business));
            refEcnExp.ErrorList.ShouldNotBeEmpty();
            refEcnExp.ErrorList.ShouldSatisfyAllConditions(
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Missing End Control. Every automation branch requires an End Control.")),
                () => ecnExp.ErrorList.ShouldContain(x => x.ErrorMessage.Contains("Wait time is not valid for Wait")));
        }
    }
}
