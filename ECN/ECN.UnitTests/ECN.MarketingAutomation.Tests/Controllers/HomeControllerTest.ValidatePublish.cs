using System;
using System.Collections.Generic;
using ecn.MarketingAutomation.Controllers.Fakes;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using NUnit.Framework;
using Entities = ECN_Framework_Entities.Communicator;
using Shouldly;
using KMPlatform.Entity;
using ecn.MarketingAutomation.Models.PostModels.ECN_Objects;
using ecn.MarketingAutomation.Models.PostModels;
using ecn.MarketingAutomation.Models.PostModels.Fakes;
using ECN_Framework_BusinessLayer.Application.Fakes;
using ECN_Framework_Common.Objects;
using KM.Common.Entity.Fakes;
using ECN_Framework_BusinessLayer.FormDesigner.Fakes;
using FormEntities = ECN_Framework_Entities.FormDesigner;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using ECN_Framework_BusinessLayer.Communicator.Interfaces.Fakes;
using ECN_Framework_Entities.Accounts;

namespace ECN.MarketingAutomation.Tests.Controllers
{
    public partial class HomeControllerTest
    {
        private const string MethodValidatePublish = "ValidatePublish";
        private readonly Random _random = new Random();
        private const int CountOfZeroItems = 0;

        [Test]
        public void ValidatePublish_OnCampaignItem_ThrowsAndSetsECNException()
        {
            // Arrange
            SetupForValidatePublish();
            var campaignItem = GetCampaignItem(editableRemove: false, maControlId: 1);
            var anotherCampaignItem = GetCampaignItem();
            var controls = new List<ControlBase> { campaignItem, anotherCampaignItem };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _privateObject.Invoke(MethodValidatePublish, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.ShouldNotBeNull(),
                () => refEcnExp.Message.ShouldNotBeNullOrWhiteSpace(),
                () => refEcnExp.ErrorList.ShouldNotBeEmpty(),
                () => refEcnExp.ErrorList.Count.ShouldBeGreaterThan(CountOfZeroItems)
            );
        }

        [Test]
        public void ValidatePublish_OnClick_ThrowsAndSetsECNException()
        {
            // Arrange
            SetupForValidatePublish();
            var clickControl = ConfigureFakesAndGetClickControl();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _privateObject.Invoke(MethodValidatePublish, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.ShouldNotBeNull(),
                () => refEcnExp.Message.ShouldNotBeNullOrWhiteSpace(),
                () => refEcnExp.ErrorList.ShouldNotBeEmpty(),
                () => refEcnExp.ErrorList.Count.ShouldBeGreaterThan(CountOfZeroItems)
            );
        }

        [Test]
        public void ValidatePublish_OnDirectClick_ThrowsAndSetsECNException()
        {
            // Arrange
            SetupForValidatePublish();
            var clickControl = ConfigureFakesAndGetDirectClick();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _privateObject.Invoke(MethodValidatePublish, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.ShouldNotBeNull(),
                () => refEcnExp.Message.ShouldNotBeNullOrWhiteSpace(),
                () => refEcnExp.ErrorList.ShouldNotBeEmpty(),
                () => refEcnExp.ErrorList.Count.ShouldBeGreaterThan(CountOfZeroItems)
            );
        }

        [Test]
        public void ValidatePublish_OnDirectClickAndWaitNull_ThrowsAndSetsECNException()
        {
            // Arrange
            SetupForValidatePublish();
            var clickControl = ConfigureFakesAndGetDirectClick(isWaitNull: true);
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _privateObject.Invoke(MethodValidatePublish, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.ShouldNotBeNull(),
                () => refEcnExp.Message.ShouldNotBeNullOrWhiteSpace(),
                () => refEcnExp.ErrorList.ShouldNotBeEmpty(),
                () => refEcnExp.ErrorList.Count.ShouldBeGreaterThan(CountOfZeroItems)
            );
        }

        [Test]
        public void ValidatePublish_OnForm_ThrowsAndSetsECNException()
        {
            // Arrange
            SetupForValidatePublish();
            var clickControl = ConfigureFakesAndGetForm();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _privateObject.Invoke(MethodValidatePublish, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.ShouldNotBeNull(),
                () => refEcnExp.Message.ShouldNotBeNullOrWhiteSpace(),
                () => refEcnExp.ErrorList.ShouldNotBeEmpty(),
                () => refEcnExp.ErrorList.Count.ShouldBeGreaterThan(CountOfZeroItems)
            );
        }

        [Test]
        public void ValidatePublish_OnFormSubmittClick_ThrowsAndSetsECNException()
        {
            // Arrange
            SetupForValidatePublish();
            var clickControl = ConfigureFakesAndGetFormSubmit();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _privateObject.Invoke(MethodValidatePublish, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.ShouldNotBeNull(),
                () => refEcnExp.Message.ShouldNotBeNullOrWhiteSpace(),
                () => refEcnExp.ErrorList.ShouldNotBeEmpty(),
                () => refEcnExp.ErrorList.Count.ShouldBeGreaterThan(CountOfZeroItems)
            );
        }

        [Test]
        public void ValidatePublish_OnFormSubmitAndWaitNull_ThrowsAndSetsECNException()
        {
            // Arrange
            SetupForValidatePublish();
            var clickControl = ConfigureFakesAndGetFormSubmit(isWaitNull: true);
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _privateObject.Invoke(MethodValidatePublish, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.ShouldNotBeNull(),
                () => refEcnExp.Message.ShouldNotBeNullOrWhiteSpace(),
                () => refEcnExp.ErrorList.ShouldNotBeEmpty(),
                () => refEcnExp.ErrorList.Count.ShouldBeGreaterThan(CountOfZeroItems)
            );
        }

        [Test]
        public void ValidatePublish_OnFormAbandon_ThrowsAndSetsECNException()
        {
            // Arrange
            SetupForValidatePublish();
            var clickControl = ConfigureFakesAndGetFormAbandon();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _privateObject.Invoke(MethodValidatePublish, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.ShouldNotBeNull(),
                () => refEcnExp.Message.ShouldNotBeNullOrWhiteSpace(),
                () => refEcnExp.ErrorList.ShouldNotBeEmpty(),
                () => refEcnExp.ErrorList.Count.ShouldBeGreaterThan(CountOfZeroItems)
            );
        }

        [Test]
        public void ValidatePublish_OnFormAbandonAndWaitNull_ThrowsAndSetsECNException()
        {
            // Arrange
            SetupForValidatePublish();
            var clickControl = ConfigureFakesAndGetFormAbandon(isWaitNull: true);
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _privateObject.Invoke(MethodValidatePublish, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.ShouldNotBeNull(),
                () => refEcnExp.Message.ShouldNotBeNullOrWhiteSpace(),
                () => refEcnExp.ErrorList.ShouldNotBeEmpty(),
                () => refEcnExp.ErrorList.Count.ShouldBeGreaterThan(CountOfZeroItems)
            );
        }

        [Test]
        public void ValidatePublish_OnDirectOpen_ThrowsAndSetsECNException()
        {
            // Arrange
            SetupForValidatePublish();
            var clickControl = ConfigureFakesAndGetDirectOpen();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _privateObject.Invoke(MethodValidatePublish, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.ShouldNotBeNull(),
                () => refEcnExp.Message.ShouldNotBeNullOrWhiteSpace(),
                () => refEcnExp.ErrorList.ShouldNotBeEmpty(),
                () => refEcnExp.ErrorList.Count.ShouldBeGreaterThan(CountOfZeroItems)
            );
        }

        [Test]
        public void ValidatePublish_OnDirectOpenAndWaitNull_ThrowsAndSetsECNException()
        {
            // Arrange
            SetupForValidatePublish();
            var clickControl = ConfigureFakesAndGetDirectOpen(isWaitNull: true);
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _privateObject.Invoke(MethodValidatePublish, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.ShouldNotBeNull(),
                () => refEcnExp.Message.ShouldNotBeNullOrWhiteSpace(),
                () => refEcnExp.ErrorList.ShouldNotBeEmpty(),
                () => refEcnExp.ErrorList.Count.ShouldBeGreaterThan(CountOfZeroItems)
            );
        }

        [Test]
        public void ValidatePublish_OnDirectNoOpen_ThrowsAndSetsECNException()
        {
            // Arrange
            SetupForValidatePublish();
            var clickControl = ConfigureFakesAndGetDirectNoOpen();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _privateObject.Invoke(MethodValidatePublish, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.ShouldNotBeNull(),
                () => refEcnExp.Message.ShouldNotBeNullOrWhiteSpace(),
                () => refEcnExp.ErrorList.ShouldNotBeEmpty(),
                () => refEcnExp.ErrorList.Count.ShouldBeGreaterThan(CountOfZeroItems)
            );
        }

        [Test]
        public void ValidatePublish_OnDirectNoOpenAndWaitNull_ThrowsAndSetsECNException()
        {
            // Arrange
            SetupForValidatePublish();
            var clickControl = ConfigureFakesAndGetDirectNoOpen(isWaitNull: true);
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _privateObject.Invoke(MethodValidatePublish, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.ShouldNotBeNull(),
                () => refEcnExp.Message.ShouldNotBeNullOrWhiteSpace(),
                () => refEcnExp.ErrorList.ShouldNotBeEmpty(),
                () => refEcnExp.ErrorList.Count.ShouldBeGreaterThan(CountOfZeroItems)
            );
        }

        [Test]
        public void ValidatePublish_OnEnd_ThrowsAndSetsECNException()
        {
            // Arrange
            SetupForValidatePublish();
            var clickControl = ConfigureFakesAndGetEnd(isWaitNull: true);
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _privateObject.Invoke(MethodValidatePublish, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.ShouldNotBeNull(),
                () => refEcnExp.Message.ShouldNotBeNullOrWhiteSpace(),
                () => refEcnExp.ErrorList.ShouldBeEmpty(),
                () => refEcnExp.ErrorList.Count.ShouldBe(CountOfZeroItems)
            );
        }

        [Test]
        public void ValidatePublish_OnGroup_ThrowsAndSetsECNException()
        {
            // Arrange
            SetupForValidatePublish();
            var clickControl = ConfigureFakesAndGetGroup();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _privateObject.Invoke(MethodValidatePublish, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.ShouldNotBeNull(),
                () => refEcnExp.Message.ShouldNotBeNullOrWhiteSpace(),
                () => refEcnExp.ErrorList.ShouldNotBeEmpty(),
                () => refEcnExp.ErrorList.Count.ShouldBeGreaterThan(CountOfZeroItems)
            );
        }

        [Test]
        public void ValidatePublish_OnNoClick_ThrowsAndSetsECNException()
        {
            // Arrange
            SetupForValidatePublish();
            var clickControl = ConfigureFakesAndGetNoClick();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _privateObject.Invoke(MethodValidatePublish, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.ShouldNotBeNull(),
                () => refEcnExp.Message.ShouldNotBeNullOrWhiteSpace(),
                () => refEcnExp.ErrorList.ShouldNotBeEmpty(),
                () => refEcnExp.ErrorList.Count.ShouldBeGreaterThan(CountOfZeroItems)
            );
        }

        [Test]
        public void ValidatePublish_OnNoOpen_ThrowsAndSetsECNException()
        {
            // Arrange
            SetupForValidatePublish();
            var clickControl = ConfigureFakesAndGetNoOpen();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _privateObject.Invoke(MethodValidatePublish, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.ShouldNotBeNull(),
                () => refEcnExp.Message.ShouldNotBeNullOrWhiteSpace(),
                () => refEcnExp.ErrorList.ShouldNotBeEmpty(),
                () => refEcnExp.ErrorList.Count.ShouldBeGreaterThan(CountOfZeroItems)
            );
        }

        [Test]
        public void ValidatePublish_OnNotSent_ThrowsAndSetsECNException()
        {
            // Arrange
            SetupForValidatePublish();
            var clickControl = ConfigureFakesAndGetNotSent();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _privateObject.Invoke(MethodValidatePublish, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.ShouldNotBeNull(),
                () => refEcnExp.Message.ShouldNotBeNullOrWhiteSpace(),
                () => refEcnExp.ErrorList.ShouldNotBeEmpty(),
                () => refEcnExp.ErrorList.Count.ShouldBeGreaterThan(CountOfZeroItems)
            );
        }

        [Test]
        public void ValidatePublish_OnOpen_ThrowsAndSetsECNException()
        {
            // Arrange
            SetupForValidatePublish();
            var clickControl = ConfigureFakesAndGetOpen();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _privateObject.Invoke(MethodValidatePublish, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.ShouldNotBeNull(),
                () => refEcnExp.Message.ShouldNotBeNullOrWhiteSpace(),
                () => refEcnExp.ErrorList.ShouldNotBeEmpty(),
                () => refEcnExp.ErrorList.Count.ShouldBeGreaterThan(CountOfZeroItems)
            );
        }

        [Test]
        public void ValidatePublish_OnOpenNoClick_ThrowsAndSetsECNException()
        {
            // Arrange
            SetupForValidatePublish();
            var clickControl = ConfigureFakesAndGetOpenNoClick();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _privateObject.Invoke(MethodValidatePublish, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.ShouldNotBeNull(),
                () => refEcnExp.Message.ShouldNotBeNullOrWhiteSpace(),
                () => refEcnExp.ErrorList.ShouldNotBeEmpty(),
                () => refEcnExp.ErrorList.Count.ShouldBeGreaterThan(CountOfZeroItems)
            );
        }

        [Test]
        public void ValidatePublish_OnSent_ThrowsAndSetsECNException()
        {
            // Arrange
            SetupForValidatePublish();
            var clickControl = ConfigureFakesAndGetSent();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _privateObject.Invoke(MethodValidatePublish, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.ShouldNotBeNull(),
                () => refEcnExp.Message.ShouldNotBeNullOrWhiteSpace(),
                () => refEcnExp.ErrorList.ShouldNotBeEmpty(),
                () => refEcnExp.ErrorList.Count.ShouldBeGreaterThan(CountOfZeroItems)
            );
        }

        [Test]
        public void ValidatePublish_OnStart_ThrowsAndSetsECNException()
        {
            // Arrange
            SetupForValidatePublish();
            var clickControl = ConfigureFakesAndGetStart();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _privateObject.Invoke(MethodValidatePublish, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.ShouldNotBeNull(),
                () => refEcnExp.Message.ShouldNotBeNullOrWhiteSpace(),
                () => refEcnExp.ErrorList.ShouldBeEmpty(),
                () => refEcnExp.ErrorList.Count.ShouldBe(CountOfZeroItems)
            );
        }

        [Test]
        public void ValidatePublish_OnSubscribe_ThrowsAndSetsECNException()
        {
            // Arrange
            SetupForValidatePublish();
            var clickControl = ConfigureFakesAndGetSubscribe();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _privateObject.Invoke(MethodValidatePublish, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.ShouldNotBeNull(),
                () => refEcnExp.Message.ShouldNotBeNullOrWhiteSpace(),
                () => refEcnExp.ErrorList.ShouldNotBeEmpty(),
                () => refEcnExp.ErrorList.Count.ShouldBeGreaterThan(CountOfZeroItems)
            );
        }

        [Test]
        public void ValidatePublish_OnSubscribeIsWaitNull_ThrowsAndSetsECNException()
        {
            // Arrange
            SetupForValidatePublish();
            var clickControl = ConfigureFakesAndGetSubscribe(isWaitNull: true);
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _privateObject.Invoke(MethodValidatePublish, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.ShouldNotBeNull(),
                () => refEcnExp.Message.ShouldNotBeNullOrWhiteSpace(),
                () => refEcnExp.ErrorList.ShouldNotBeEmpty(),
                () => refEcnExp.ErrorList.Count.ShouldBeGreaterThan(CountOfZeroItems)
            );
        }

        [Test]
        public void ValidatePublish_OnSuppressed_ThrowsAndSetsECNException()
        {
            // Arrange
            SetupForValidatePublish();
            var clickControl = ConfigureFakesAndGetSuppressed();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _privateObject.Invoke(MethodValidatePublish, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.ShouldNotBeNull(),
                () => refEcnExp.Message.ShouldNotBeNullOrWhiteSpace(),
                () => refEcnExp.ErrorList.ShouldNotBeEmpty(),
                () => refEcnExp.ErrorList.Count.ShouldBeGreaterThan(CountOfZeroItems)
            );
        }

        [Test]
        public void ValidatePublish_OnUnsubscribe_ThrowsAndSetsECNException()
        {
            // Arrange
            SetupForValidatePublish();
            var clickControl = ConfigureFakesAndGetUnsubscribe();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _privateObject.Invoke(MethodValidatePublish, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.ShouldNotBeNull(),
                () => refEcnExp.Message.ShouldNotBeNullOrWhiteSpace(),
                () => refEcnExp.ErrorList.ShouldNotBeEmpty(),
                () => refEcnExp.ErrorList.Count.ShouldBeGreaterThan(CountOfZeroItems)
            );
        }

        [Test]
        public void ValidatePublish_OnUnSubscribeIsWaitNull_ThrowsAndSetsECNException()
        {
            // Arrange
            SetupForValidatePublish();
            var clickControl = ConfigureFakesAndGetUnsubscribe(isWaitNull: true);
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _privateObject.Invoke(MethodValidatePublish, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.ShouldNotBeNull(),
                () => refEcnExp.Message.ShouldNotBeNullOrWhiteSpace(),
                () => refEcnExp.ErrorList.ShouldNotBeEmpty(),
                () => refEcnExp.ErrorList.Count.ShouldBeGreaterThan(CountOfZeroItems)
            );
        }

        [Test]
        public void ValidatePublish_OnWait_ThrowsAndSetsECNException()
        {
            // Arrange
            SetupForValidatePublish();
            var clickControl = ConfigureFakesAndGetWait();
            var controls = new List<ControlBase> { clickControl };
            var marketingAutomation = GetMarketingAutomationObj();
            var ecnExp = new ECNException(new List<ECNError>());

            var parameters = new object[]
            {
                controls,
                marketingAutomation,
                ecnExp
            };

            // Act
            _privateObject.Invoke(MethodValidatePublish, parameters);
            var refEcnExp = parameters[2] as ECNException;

            // Assert
            refEcnExp.ShouldSatisfyAllConditions(
                () => refEcnExp.ShouldNotBeNull(),
                () => refEcnExp.Message.ShouldNotBeNullOrWhiteSpace(),
                () => refEcnExp.ErrorList.ShouldNotBeEmpty(),
                () => refEcnExp.ErrorList.Count.ShouldBeGreaterThan(CountOfZeroItems)
            );
        }

        private CampaignItem GetCampaignItem()
        {
            return new CampaignItem
            {
                ControlType = MarketingAutomationControlType.CampaignItem,
                Text = "SampleText",
                CreateCampaignItem = false,
                SendTime = DateTime.Now.AddDays(7),
                CampaignItemName = "SampleCampignItem",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private Click ConfigureFakesAndGetClickControl()
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => new Wait();
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (d, cb) => GetCampaignItem();
            return new Click
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private Direct_Click ConfigureFakesAndGetDirectClick(bool isWaitNull = false)
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => isWaitNull ? null : new Wait();
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (d, cb) => GetCampaignItem();
            ShimAutomationBaseController.AllInstances.FindParentGroupControlBase = (d, g) => new Group();
            return new Direct_Click
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private Form ConfigureFakesAndGetForm()
        {
            ShimAutomationBaseController.AllInstances.FindParentControlBase = (f, cb) => GetCampaignItem();
            return new Form
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private FormSubmit ConfigureFakesAndGetFormSubmit(bool isWaitNull = false)
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => isWaitNull ? null : new Wait();
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (d, cb) => GetCampaignItem();
            ShimAutomationBaseController.AllInstances.FindParentGroupControlBase = (d, g) => new Group();
            return new FormSubmit
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private FormAbandon ConfigureFakesAndGetFormAbandon(bool isWaitNull = false)
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => isWaitNull ? null : new Wait();
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (d, cb) => GetCampaignItem();
            ShimAutomationBaseController.AllInstances.FindParentGroupControlBase = (d, g) => new Group();
            return new FormAbandon
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private Direct_Open ConfigureFakesAndGetDirectOpen(bool isWaitNull = false)
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => isWaitNull ? null : new Wait();
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (d, cb) => GetCampaignItem();
            ShimAutomationBaseController.AllInstances.FindParentGroupControlBase = (d, g) => new Group();
            return new Direct_Open
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private Direct_NoOpen ConfigureFakesAndGetDirectNoOpen(bool isWaitNull = false)
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => isWaitNull ? null : new Wait();
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (d, cb) => GetCampaignItem();
            ShimAutomationBaseController.AllInstances.FindParentGroupControlBase = (d, g) => new Group();
            return new Direct_NoOpen
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private End ConfigureFakesAndGetEnd(bool isWaitNull = false)
        {
            return new End
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private Group ConfigureFakesAndGetGroup()
        {
            return new Group
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private NoClick ConfigureFakesAndGetNoClick()
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => new Wait();
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (d, cb) => GetCampaignItem();
            return new NoClick
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private NoOpen ConfigureFakesAndGetNoOpen()
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => new Wait();
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (d, cb) => GetCampaignItem();
            return new NoOpen
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private NotSent ConfigureFakesAndGetNotSent()
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => new Wait();
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (d, cb) => GetCampaignItem();
            return new NotSent
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private Open ConfigureFakesAndGetOpen()
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => new Wait();
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (d, cb) => GetCampaignItem();
            return new Open
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private Open_NoClick ConfigureFakesAndGetOpenNoClick()
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => new Wait();
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (d, cb) => GetCampaignItem();
            return new Open_NoClick
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private Sent ConfigureFakesAndGetSent()
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => new Wait();
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (d, cb) => GetCampaignItem();
            return new Sent
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private Start ConfigureFakesAndGetStart()
        {
            return new Start
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private Subscribe ConfigureFakesAndGetSubscribe(bool isWaitNull = false)
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => isWaitNull ? null : new Wait();
            ShimAutomationBaseController.AllInstances.FindParentGroupControlBase = (d, cb) => new Group();
            return new Subscribe
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private Suppressed ConfigureFakesAndGetSuppressed()
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => new Wait();
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (d, cb) => GetCampaignItem();
            return new Suppressed
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private Unsubscribe ConfigureFakesAndGetUnsubscribe(bool isWaitNull = false)
        {
            ShimAutomationBaseController.AllInstances.FindParentWaitControlBase = (d, cb) => isWaitNull ? null : new Wait();
            ShimAutomationBaseController.AllInstances.FindParentGroupControlBase = (d, cb) => new Group();
            return new Unsubscribe
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private Wait ConfigureFakesAndGetWait()
        {
            ShimAutomationBaseController.AllInstances.FindParentControlBase = (d, cb) => GetCampaignItem();
            return new Wait
            {
                ControlID = "1",
                CampaignItemManager = GetStubICampaignItemManager()
            };
        }

        private StubICampaignItemManager GetStubICampaignItemManager()
        {
            return new StubICampaignItemManager()
            {
                GetByCampaignItemID_NoAccessCheckInt32Boolean = (id, children) => new Entities::CampaignItem
                {
                    CampaignItemType = ControlConsts.CampaignItemTypeChampion.ToLower()
                },
                GetByCampaignItemIDInt32UserBoolean = (id, user, children) => null
            };
        }

        private Entities::MarketingAutomation GetMarketingAutomationObj()
        {
            return new Entities::MarketingAutomation
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(3)
            };
        }

        private CampaignItem GetCampaignItem(
            bool editableRemove,
            int maControlId,
            bool createCampaignItem = false,
            bool isDirty = false,
            string subCategory = "",
            DateTime? sendTime = null)
        {
            return new CampaignItem
            {
                MAControlID = maControlId,
                editable = new shapeEditable
                {
                    remove = editableRemove
                },
                Text = GetUniqueString(),
                xPosition = GetAnyDecimal(),
                yPosition = GetAnyDecimal(),
                CreateCampaignItem = createCampaignItem,
                IsDirty = isDirty,
                SubCategory = subCategory,
                ControlID = GetUniqueString(),
                SendTime = sendTime ?? GetAnyTime(),
                CampaignItemID = GetAnyNumber()
            };
        }

        private Click GetClick(
            bool editableRemove = false,
            int? maControlId = null,
            int? ecnId = null)
        {
            return new Click
            {
                ControlID = GetUniqueString(),
                Text = GetUniqueString(),
                xPosition = GetAnyDecimal(),
                yPosition = GetAnyDecimal(),
                editable = new shapeEditable
                {
                    remove = editableRemove
                },
                MAControlID = maControlId ?? GetAnyNumber(),
                ECNID = ecnId ?? GetAnyNumber(),
                IsDirty = true
            };
        }

        private string GetUniqueString()
        {
            return Guid.NewGuid().ToString();
        }

        private decimal GetAnyDecimal()
        {
            var random = new Random();
            return (decimal)random.NextDouble();
        }

        private int GetAnyNumber(int? rangeMinimum = null, int? rangeMaximum = null)
        {
            const int RandomRangeMinimum = 1000000;
            const int RandomRangeMaximum = RandomRangeMinimum * 10;
            return _random.Next(rangeMinimum ?? RandomRangeMinimum, rangeMaximum ?? RandomRangeMaximum);
        }

        private DateTime GetAnyTime()
        {
            return DateTime.Now.AddSeconds(GetAnyNumber());
        }

        private void SetupForValidatePublish()
        {
            ShimCurrentUser();
            SetAllConnectorsProperty();
            SetAllControlsProperty();
        }

        private void ShimCurrentUser()
        {
            ShimECNSession.Constructor = (instance) => { };
            var shimSession = new ShimECNSession();
            shimSession.ClearSession = () => { }; 
            shimSession.Instance.CurrentUser = new User { UserID = 1, UserName = "TestUser" };
            shimSession.Instance.CurrentCustomer = new Customer { CustomerID = DummyId };
            shimSession.Instance.CurrentBaseChannel = new BaseChannel { BaseChannelID = 1};
            ShimECNSession.CurrentSession = () => shimSession.Instance;
        }

        private void SetAllConnectorsProperty()
        {
            var allConnectors = new List<Connector>();
            _privateObject.SetProperty(AllConnectorsProperty, allConnectors);
        }

        private void SetAllControlsProperty()
        {
            var allControls = new List<ControlBase>();
            _privateObject.SetProperty(AllControlsProperty, allControls);
        }
    }
}
