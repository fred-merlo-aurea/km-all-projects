using System.Collections.Generic;
using ecn.MarketingAutomation.Models.PostModels;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using static ECN_Framework_Common.Objects.Communicator.Enums;
using NUnit.Framework;
using Shouldly;

namespace ECN.MarketinAutomation.Tests.Models.PostModels
{
    public partial class ControlBaseTest
    {
        [Test]
        public void GetModelsForCopyFromControlBase_WhenControlTypeIsCampaignItem_ReturnsControlsCopy()
        {
            // Arrange
            var controls = new List<ControlBase>
            {
                ConfigureFakesAndGetCampaignItem()
            };
            var origConnectors = new List<Connector> { new Connector() };
            var connectors = new List<Connector> { new Connector() };
            var mAID = 1;

            // Act
            var resultControlList = ControlBase.GetModelsForCopyFromControlBase(controls, origConnectors, ref connectors, mAID);

            // Assert
            resultControlList.ShouldNotBeNull();
            resultControlList.Count.ShouldBe(1);
            var campaignItem = resultControlList[0] as CampaignItem;
            campaignItem.ShouldNotBeNull();
            campaignItem.ShouldSatisfyAllConditions(
                () => campaignItem.CampaignID.ShouldBe(1),
                () => campaignItem.ControlID.ShouldBe("1"),
                () => campaignItem.CampaignItemName.ShouldContain("Copy of SampleCampaignItem"),
                () => campaignItem.CampaignName.ShouldContain("SampleCampaign"),
                () => campaignItem.ControlType.ShouldBe(MarketingAutomationControlType.CampaignItem),
                () => campaignItem.BlastField1.ShouldBeNullOrWhiteSpace(),
                () => campaignItem.BlastField2.ShouldBeNullOrWhiteSpace(),
                () => campaignItem.BlastField3.ShouldBeNullOrWhiteSpace(),
                () => campaignItem.BlastField4.ShouldBeNullOrWhiteSpace(),
                () => campaignItem.MarketingAutomationID.ShouldBe(1),
                () => campaignItem.MessageID.ShouldBe(1),
                () => campaignItem.MessageName.ShouldBe("SampleLayout"),
                () => campaignItem.Text.ShouldBe("Campaign Item"),
                () => campaignItem.SelectedGroupFilters.Count.ShouldBe(1),
                () => campaignItem.SelectedGroupFilters[0].GroupID.ShouldBe(1),
                () => campaignItem.SelectedGroupFilters[0].FilterID.ShouldBe(1),
                () => campaignItem.SelectedGroupFilters[0].CustomerID.ShouldBe(1),
                () => campaignItem.SelectedGroups.Count.ShouldBe(1),
                () => campaignItem.SelectedGroups[0].FolderID.ShouldBe(1),
                () => campaignItem.SuppressedGroupFilters.Count.ShouldBe(1),
                () => campaignItem.SuppressedGroupFilters[0].GroupID.ShouldBe(1),
                () => campaignItem.SuppressedGroupFilters[0].FilterID.ShouldBe(1),
                () => campaignItem.SuppressedGroupFilters[0].CustomerID.ShouldBe(1),
                () => campaignItem.SuppressedGroups.Count.ShouldBe(1),
                () => campaignItem.SuppressedGroups[0].FolderID.ShouldBe(1));
        }

        [Test]
        public void GetModelsForCopyFromControlBase_WhenControlTypeIsClick_ReturnsControlsCopy()
        {
            // Arrange
            var controls = new List<ControlBase> { ConfigureFakesAndGetClick() };
            var origConnectors = new List<Connector> { new Connector() };
            var connectors = new List<Connector> { new Connector() };
            var mAID = 1;

            // Act
            var resultControlList = ControlBase.GetModelsForCopyFromControlBase(controls, origConnectors, ref connectors, mAID);

            // Assert
            resultControlList.ShouldNotBeEmpty();
            resultControlList.Count.ShouldBe(1);
            var controlClick = resultControlList[0] as Click;
            controlClick.ShouldNotBeNull();
            controlClick.ShouldSatisfyAllConditions(
                () => controlClick.ECNID.ShouldBe(-1),
                () => controlClick.MarketingAutomationID.ShouldBe(1),
                () => controlClick.CampaignItemName.ShouldContain($"Copy of SampleItemClick"),
                () => controlClick.MAControlID.ShouldBe(0),
                () => controlClick.EstSendTime.ShouldBeNull(),
                () => controlClick.IsConfigured.ShouldBeFalse(),
                () => controlClick.editable.remove.ShouldBeTrue());
        }

        [Test]
        public void GetModelsForCopyFromControlBase_WhenControlTypeIsDirectClick_ReturnsControlsCopy()
        {
            // Arrange
            var controls = new List<ControlBase> { ConfigureFakesAndGetDirectClick() };
            var origConnectors = new List<Connector> { new Connector() };
            var connectors = new List<Connector> { new Connector() };
            var mAID = 1;

            // Act
            var resultControlList = ControlBase.GetModelsForCopyFromControlBase(controls, origConnectors, ref connectors, mAID);

            // Assert
            resultControlList.ShouldNotBeEmpty();
            resultControlList.Count.ShouldBe(1);
            var controlDirectClick = resultControlList[0] as Direct_Click;
            controlDirectClick.ShouldNotBeNull();
            controlDirectClick.ShouldSatisfyAllConditions(
                () => controlDirectClick.ECNID.ShouldBe(-1),
                () => controlDirectClick.MarketingAutomationID.ShouldBe(1),
                () => controlDirectClick.CampaignItemName.ShouldContain($"Copy of SampleItemDirectClick"),
                () => controlDirectClick.MAControlID.ShouldBe(0),
                () => controlDirectClick.IsCancelled.ShouldBeTrue(),
                () => controlDirectClick.IsConfigured.ShouldBeFalse(),
                () => controlDirectClick.editable.remove.ShouldBeTrue());
        }

        [Test]
        public void GetModelsForCopyFromControlBase_WhenControlTypeIsForm_ReturnsControlsCopy()
        {
            // Arrange
            var controls = new List<ControlBase> { ConfigureFakesAndGetForm() };
            var origConnectors = new List<Connector> { new Connector() };
            var connectors = new List<Connector> { new Connector() };
            var mAID = 1;

            // Act
            var resultControlList = ControlBase.GetModelsForCopyFromControlBase(controls, origConnectors, ref connectors, mAID);

            // Assert
            resultControlList.ShouldNotBeEmpty();
            resultControlList.Count.ShouldBe(1);
            var controlForm = resultControlList[0] as Form;
            controlForm.ShouldNotBeNull();
            controlForm.ShouldSatisfyAllConditions(
                () => controlForm.ECNID.ShouldBe(-1),
                () => controlForm.MarketingAutomationID.ShouldBe(1),
                () => controlForm.MAControlID.ShouldBe(0),
                () => controlForm.IsConfigured.ShouldBeFalse(),
                () => controlForm.editable.remove.ShouldBeTrue());
        }

        [Test]
        public void GetModelsForCopyFromControlBase_WhenControlTypeIsFormAbandon_ReturnsControlsCopy()
        {
            // Arrange
            var controls = new List<ControlBase> { ConfigureFakesAndGetFormAbandon() };
            var origConnectors = new List<Connector> { new Connector() };
            var connectors = new List<Connector> { new Connector() };
            var mAID = 1;

            // Act
            var resultControlList = ControlBase.GetModelsForCopyFromControlBase(controls, origConnectors, ref connectors, mAID);

            // Assert
            resultControlList.ShouldNotBeEmpty();
            resultControlList.Count.ShouldBe(1);
            var controlFormAbandon = resultControlList[0] as FormAbandon;
            controlFormAbandon.ShouldNotBeNull();
            controlFormAbandon.ShouldSatisfyAllConditions(
                () => controlFormAbandon.ECNID.ShouldBe(-1),
                () => controlFormAbandon.MarketingAutomationID.ShouldBe(1),
                () => controlFormAbandon.CampaignItemName.ShouldContain("Copy of SampleItemFormAbandon"),
                () => controlFormAbandon.MAControlID.ShouldBe(0),
                () => controlFormAbandon.IsConfigured.ShouldBeFalse(),
                () => controlFormAbandon.editable.remove.ShouldBeTrue());
        }

        [Test]
        public void GetModelsForCopyFromControlBase_WhenControlTypeIsFormSubmit_ReturnsControlsCopy()
        {
            // Arrange
            var controls = new List<ControlBase> { ConfigureFakesAndGetFormSubmit() };
            var origConnectors = new List<Connector> { new Connector() };
            var connectors = new List<Connector> { new Connector() };
            var mAID = 1;

            // Act
            var resultControlList = ControlBase.GetModelsForCopyFromControlBase(controls, origConnectors, ref connectors, mAID);

            // Assert
            resultControlList.ShouldNotBeEmpty();
            resultControlList.Count.ShouldBe(1);
            var controlFormSubmit = resultControlList[0] as FormSubmit;
            controlFormSubmit.ShouldNotBeNull();
            controlFormSubmit.ShouldSatisfyAllConditions(
                () => controlFormSubmit.ECNID.ShouldBe(-1),
                () => controlFormSubmit.MarketingAutomationID.ShouldBe(1),
                () => controlFormSubmit.CampaignItemName.ShouldContain("Copy of SampleItemFormSubmit"),
                () => controlFormSubmit.MAControlID.ShouldBe(0),
                () => controlFormSubmit.IsConfigured.ShouldBeFalse(),
                () => controlFormSubmit.editable.remove.ShouldBeTrue());
        }

        [Test]
        public void GetModelsForCopyFromControlBase_WhenControlTypeIsDirectOpen_ReturnsControlsCopy()
        {
            // Arrange
            var controls = new List<ControlBase> { ConfigureFakesAndGetDirectOpen() };
            var origConnectors = new List<Connector> { new Connector() };
            var connectors = new List<Connector> { new Connector() };
            var mAID = 1;

            // Act
            var resultControlList = ControlBase.GetModelsForCopyFromControlBase(controls, origConnectors, ref connectors, mAID);

            // Assert
            resultControlList.ShouldNotBeEmpty();
            resultControlList.Count.ShouldBe(1);
            var controlDirectOpen = resultControlList[0] as Direct_Open;
            controlDirectOpen.ShouldNotBeNull();
            controlDirectOpen.ShouldSatisfyAllConditions(
                () => controlDirectOpen.ECNID.ShouldBe(-1),
                () => controlDirectOpen.MarketingAutomationID.ShouldBe(1),
                () => controlDirectOpen.CampaignItemName.ShouldContain("Copy of SampleItemDirectOpen"),
                () => controlDirectOpen.MAControlID.ShouldBe(0),
                () => controlDirectOpen.IsConfigured.ShouldBeFalse(),
                () => controlDirectOpen.editable.remove.ShouldBeTrue());
        }

        [Test]
        public void GetModelsForCopyFromControlBase_WhenControlTypeIsDirectNoOpen_ReturnsControlsCopy()
        {
            // Arrange
            var controls = new List<ControlBase> { ConfigureFakesAndGetDirectNoOpen() };
            var origConnectors = new List<Connector> { new Connector() };
            var connectors = new List<Connector> { new Connector() };
            var mAID = 1;

            // Act
            var resultControlList = ControlBase.GetModelsForCopyFromControlBase(controls, origConnectors, ref connectors, mAID);

            // Assert
            resultControlList.ShouldNotBeEmpty();
            resultControlList.Count.ShouldBe(1);
            var controlDirectNoOpen = resultControlList[0] as Direct_NoOpen;
            controlDirectNoOpen.ShouldNotBeNull();
            controlDirectNoOpen.ShouldSatisfyAllConditions(
                () => controlDirectNoOpen.ECNID.ShouldBe(-1),
                () => controlDirectNoOpen.MarketingAutomationID.ShouldBe(1),
                () => controlDirectNoOpen.CampaignItemName.ShouldContain("Copy of SampleItemDirectNoOpen"),
                () => controlDirectNoOpen.MAControlID.ShouldBe(0),
                () => controlDirectNoOpen.IsConfigured.ShouldBeFalse(),
                () => controlDirectNoOpen.editable.remove.ShouldBeTrue());
        }

        [Test]
        public void GetModelsForCopyFromControlBase_WhenControlTypeIsEnd_ReturnsControlsCopy()
        {
            // Arrange
            var controls = new List<ControlBase> { ConfigureFakesAndGetEnd() };
            var origConnectors = new List<Connector> { new Connector() };
            var connectors = new List<Connector> { new Connector() };
            var mAID = 1;

            // Act
            var resultControlList = ControlBase.GetModelsForCopyFromControlBase(controls, origConnectors, ref connectors, mAID);

            // Assert
            resultControlList.ShouldNotBeEmpty();
            resultControlList.Count.ShouldBe(1);
            var controlEnd = resultControlList[0] as End;
            controlEnd.ShouldNotBeNull();
            controlEnd.ShouldSatisfyAllConditions(
                () => controlEnd.ECNID.ShouldBe(0),
                () => controlEnd.MarketingAutomationID.ShouldBe(1),
                () => controlEnd.ExtraText.ShouldBeNullOrWhiteSpace(),
                () => controlEnd.Text.ShouldBeNullOrWhiteSpace(),
                () => controlEnd.MAControlID.ShouldBe(0),
                () => controlEnd.IsConfigured.ShouldBeTrue(),
                () => controlEnd.editable.remove.ShouldBeTrue());
        }

        [Test]
        public void GetModelsForCopyFromControlBase_WhenControlTypeIsGroup_ReturnsControlsCopy()
        {
            // Arrange
            var controls = new List<ControlBase> { ConfigureFakesAndGetGroup() };
            var origConnectors = new List<Connector> { new Connector() };
            var connectors = new List<Connector> { new Connector() };
            var mAID = 1;

            // Act
            var resultControlList = ControlBase.GetModelsForCopyFromControlBase(controls, origConnectors, ref connectors, mAID);

            // Assert
            resultControlList.ShouldNotBeEmpty();
            resultControlList.Count.ShouldBe(1);
            var controlGroup = resultControlList[0] as Group;
            controlGroup.ShouldNotBeNull();
            controlGroup.ShouldSatisfyAllConditions(
                () => controlGroup.ECNID.ShouldBe(0),
                () => controlGroup.MarketingAutomationID.ShouldBe(1),
                () => controlGroup.ExtraText.ShouldBeNullOrWhiteSpace(),
                () => controlGroup.Text.ShouldBeNullOrWhiteSpace(),
                () => controlGroup.MAControlID.ShouldBe(0),
                () => controlGroup.IsConfigured.ShouldBeFalse(),
                () => controlGroup.editable.remove.ShouldBeTrue());
        }

        [Test]
        public void GetModelsForCopyFromControlBase_WhenControlTypeIsNoClick_ReturnsControlsCopy()
        {
            // Arrange
            var controls = new List<ControlBase> { ConfigureFakesAndGetNoClick() };
            var origConnectors = new List<Connector> { new Connector() };
            var connectors = new List<Connector> { new Connector() };
            var mAID = 1;

            // Act
            var resultControlList = ControlBase.GetModelsForCopyFromControlBase(controls, origConnectors, ref connectors, mAID);

            // Assert
            resultControlList.ShouldNotBeEmpty();
            resultControlList.Count.ShouldBe(1);
            var controlNoClick = resultControlList[0] as NoClick;
            controlNoClick.ShouldNotBeNull();
            controlNoClick.ShouldSatisfyAllConditions(
                () => controlNoClick.ECNID.ShouldBe(-1),
                () => controlNoClick.MarketingAutomationID.ShouldBe(1),
                () => controlNoClick.CampaignItemName.ShouldContain("Copy of SampleItemNoClick"),
                () => controlNoClick.MAControlID.ShouldBe(0),
                () => controlNoClick.EstSendTime.ShouldBeNull(),
                () => controlNoClick.IsConfigured.ShouldBeFalse(),
                () => controlNoClick.editable.remove.ShouldBeTrue());
        }

        [Test]
        public void GetModelsForCopyFromControlBase_WhenControlTypeIsNoOpen_ReturnsControlsCopy()
        {
            // Arrange
            var controls = new List<ControlBase> { ConfigureFakesAndGetNoOpen() };
            var origConnectors = new List<Connector> { new Connector() };
            var connectors = new List<Connector> { new Connector() };
            var mAID = 1;

            // Act
            var resultControlList = ControlBase.GetModelsForCopyFromControlBase(controls, origConnectors, ref connectors, mAID);

            // Assert
            resultControlList.ShouldNotBeEmpty();
            resultControlList.Count.ShouldBe(1);
            var controlNoOpen = resultControlList[0] as NoOpen;
            controlNoOpen.ShouldNotBeNull();
            controlNoOpen.ShouldSatisfyAllConditions(
                () => controlNoOpen.ECNID.ShouldBe(-1),
                () => controlNoOpen.MarketingAutomationID.ShouldBe(1),
                () => controlNoOpen.CampaignItemName.ShouldContain("Copy of SampleItemNoOpen"),
                () => controlNoOpen.MAControlID.ShouldBe(0),
                () => controlNoOpen.EstSendTime.ShouldBeNull(),
                () => controlNoOpen.IsConfigured.ShouldBeFalse(),
                () => controlNoOpen.editable.remove.ShouldBeTrue());
        }

        [Test]
        public void GetModelsForCopyFromControlBase_WhenControlTypeIsNotSent_ReturnsControlsCopy()
        {
            // Arrange
            var controls = new List<ControlBase> { ConfigureFakesAndGetNotSent() };
            var origConnectors = new List<Connector> { new Connector() };
            var connectors = new List<Connector> { new Connector() };
            var mAID = 1;

            // Act
            var resultControlList = ControlBase.GetModelsForCopyFromControlBase(controls, origConnectors, ref connectors, mAID);

            // Assert
            resultControlList.ShouldNotBeEmpty();
            resultControlList.Count.ShouldBe(1);
            var controlNotSent = resultControlList[0] as NotSent;
            controlNotSent.ShouldNotBeNull();
            controlNotSent.ShouldSatisfyAllConditions(
                () => controlNotSent.ECNID.ShouldBe(-1),
                () => controlNotSent.MarketingAutomationID.ShouldBe(1),
                () => controlNotSent.CampaignItemName.ShouldContain("Copy of SampleItemNotSent"),
                () => controlNotSent.MAControlID.ShouldBe(0),
                () => controlNotSent.EstSendTime.ShouldBeNull(),
                () => controlNotSent.IsConfigured.ShouldBeFalse(),
                () => controlNotSent.editable.remove.ShouldBeTrue());
        }

        [Test]
        public void GetModelsForCopyFromControlBase_WhenControlTypeIsOpen_ReturnsControlsCopy()
        {
            // Arrange
            var controls = new List<ControlBase> { ConfigureFakesAndGetOpen() };
            var origConnectors = new List<Connector> { new Connector() };
            var connectors = new List<Connector> { new Connector() };
            var mAID = 1;

            // Act
            var resultControlList = ControlBase.GetModelsForCopyFromControlBase(controls, origConnectors, ref connectors, mAID);

            // Assert
            resultControlList.ShouldNotBeEmpty();
            resultControlList.Count.ShouldBe(1);
            var controlOpen = resultControlList[0] as Open;
            controlOpen.ShouldNotBeNull();
            controlOpen.ShouldSatisfyAllConditions(
                () => controlOpen.ECNID.ShouldBe(-1),
                () => controlOpen.MarketingAutomationID.ShouldBe(1),
                () => controlOpen.CampaignItemName.ShouldContain("Copy of SampleItemOpen"),
                () => controlOpen.MAControlID.ShouldBe(0),
                () => controlOpen.EstSendTime.ShouldBeNull(),
                () => controlOpen.IsConfigured.ShouldBeFalse(),
                () => controlOpen.editable.remove.ShouldBeTrue());
        }

        [Test]
        public void GetModelsForCopyFromControlBase_WhenControlTypeIsOpenNoClick_ReturnsControlsCopy()
        {
            // Arrange
            var controls = new List<ControlBase> { ConfigureFakesAndGetOpenNoClick() };
            var origConnectors = new List<Connector> { new Connector() };
            var connectors = new List<Connector> { new Connector() };
            var mAID = 1;

            // Act
            var resultControlList = ControlBase.GetModelsForCopyFromControlBase(controls, origConnectors, ref connectors, mAID);

            // Assert
            resultControlList.ShouldNotBeEmpty();
            resultControlList.Count.ShouldBe(1);
            var controlOpenNoClick = resultControlList[0] as Open_NoClick;
            controlOpenNoClick.ShouldNotBeNull();
            controlOpenNoClick.ShouldSatisfyAllConditions(
                () => controlOpenNoClick.ECNID.ShouldBe(-1),
                () => controlOpenNoClick.MarketingAutomationID.ShouldBe(1),
                () => controlOpenNoClick.CampaignItemName.ShouldContain("Copy of SampleItemOpenNoClick"),
                () => controlOpenNoClick.MAControlID.ShouldBe(0),
                () => controlOpenNoClick.EstSendTime.ShouldBeNull(),
                () => controlOpenNoClick.IsConfigured.ShouldBeFalse(),
                () => controlOpenNoClick.editable.remove.ShouldBeTrue());
        }

        [Test]
        public void GetModelsForCopyFromControlBase_WhenControlTypeIsSent_ReturnsControlsCopy()
        {
            // Arrange
            var controls = new List<ControlBase> { ConfigureFakesAndGetSent() };
            var origConnectors = new List<Connector> { new Connector() };
            var connectors = new List<Connector> { new Connector() };
            var mAID = 1;

            // Act
            var resultControlList = ControlBase.GetModelsForCopyFromControlBase(controls, origConnectors, ref connectors, mAID);

            // Assert
            resultControlList.ShouldNotBeEmpty();
            resultControlList.Count.ShouldBe(1);
            var controlSent = resultControlList[0] as Sent;
            controlSent.ShouldNotBeNull();
            controlSent.ShouldSatisfyAllConditions(
                () => controlSent.ECNID.ShouldBe(-1),
                () => controlSent.MarketingAutomationID.ShouldBe(1),
                () => controlSent.CampaignItemName.ShouldContain("Copy of SampleItemSent"),
                () => controlSent.MAControlID.ShouldBe(0),
                () => controlSent.EstSendTime.ShouldBeNull(),
                () => controlSent.IsConfigured.ShouldBeFalse(),
                () => controlSent.editable.remove.ShouldBeTrue());
        }

        [Test]
        public void GetModelsForCopyFromControlBase_WhenControlTypeIsStart_ReturnsControlsCopy()
        {
            // Arrange
            var controls = new List<ControlBase> { ConfigureFakesAndGetStart() };
            var origConnectors = new List<Connector> { new Connector() };
            var connectors = new List<Connector> { new Connector() };
            var mAID = 1;

            // Act
            var resultControlList = ControlBase.GetModelsForCopyFromControlBase(controls, origConnectors, ref connectors, mAID);

            // Assert
            resultControlList.ShouldNotBeEmpty();
            resultControlList.Count.ShouldBe(1);
            var controlStart = resultControlList[0] as Start;
            controlStart.ShouldNotBeNull();
            controlStart.ShouldSatisfyAllConditions(
                () => controlStart.ECNID.ShouldBe(0),
                () => controlStart.ExtraText.ShouldBeNullOrWhiteSpace(),
                () => controlStart.Text.ShouldBeNullOrWhiteSpace(),
                () => controlStart.MarketingAutomationID.ShouldBe(1),
                () => controlStart.MAControlID.ShouldBe(0),
                () => controlStart.IsConfigured.ShouldBeTrue(),
                () => controlStart.editable.remove.ShouldBeTrue());
        }

        [Test]
        public void GetModelsForCopyFromControlBase_WhenControlTypeIsSubscribe_ReturnsControlsCopy()
        {
            // Arrange
            var controls = new List<ControlBase> { ConfigureFakesAndGetSubscribe() };
            var origConnectors = new List<Connector> { new Connector() };
            var connectors = new List<Connector> { new Connector() };
            var mAID = 1;

            // Act
            var resultControlList = ControlBase.GetModelsForCopyFromControlBase(controls, origConnectors, ref connectors, mAID);

            // Assert
            resultControlList.ShouldNotBeEmpty();
            resultControlList.Count.ShouldBe(1);
            var controlSubscribe = resultControlList[0] as Subscribe;
            controlSubscribe.ShouldNotBeNull();
            controlSubscribe.ShouldSatisfyAllConditions(
                () => controlSubscribe.ECNID.ShouldBe(-1),
                () => controlSubscribe.MarketingAutomationID.ShouldBe(1),
                () => controlSubscribe.CampaignItemName.ShouldContain("Copy of SampleItemSubscribe"),
                () => controlSubscribe.MAControlID.ShouldBe(0),
                () => controlSubscribe.IsConfigured.ShouldBeFalse(),
                () => controlSubscribe.editable.remove.ShouldBeTrue());
        }

        [Test]
        public void GetModelsForCopyFromControlBase_WhenControlTypeIsSuppressed_ReturnsControlsCopy()
        {
            // Arrange
            var controls = new List<ControlBase> { ConfigureFakesAndGetSuppressed() };
            var origConnectors = new List<Connector> { new Connector() };
            var connectors = new List<Connector> { new Connector() };
            var mAID = 1;

            // Act
            var resultControlList = ControlBase.GetModelsForCopyFromControlBase(controls, origConnectors, ref connectors, mAID);

            // Assert
            resultControlList.ShouldNotBeEmpty();
            resultControlList.Count.ShouldBe(1);
            var controlSuppressed = resultControlList[0] as Suppressed;
            controlSuppressed.ShouldNotBeNull();
            controlSuppressed.ShouldSatisfyAllConditions(
                () => controlSuppressed.ECNID.ShouldBe(-1),
                () => controlSuppressed.MarketingAutomationID.ShouldBe(1),
                () => controlSuppressed.CampaignItemName.ShouldContain("Copy of SampleItemSuppressed"),
                () => controlSuppressed.MAControlID.ShouldBe(0),
                () => controlSuppressed.EstSendTime.ShouldBeNull(),
                () => controlSuppressed.IsConfigured.ShouldBeFalse(),
                () => controlSuppressed.editable.remove.ShouldBeTrue());
        }

        [Test]
        public void GetModelsForCopyFromControlBase_WhenControlTypeIsUnsubscribe_ReturnsControlsCopy()
        {
            // Arrange
            var controls = new List<ControlBase> { ConfigureFakesAndGetUnsubscribe() };
            var origConnectors = new List<Connector> { new Connector() };
            var connectors = new List<Connector> { new Connector() };
            var mAID = 1;

            // Act
            var resultControlList = ControlBase.GetModelsForCopyFromControlBase(controls, origConnectors, ref connectors, mAID);

            // Assert
            resultControlList.ShouldNotBeEmpty();
            resultControlList.Count.ShouldBe(1);
            var controlUnsubscribe = resultControlList[0] as Unsubscribe;
            controlUnsubscribe.ShouldNotBeNull();
            controlUnsubscribe.ShouldSatisfyAllConditions(
                () => controlUnsubscribe.ECNID.ShouldBe(-1),
                () => controlUnsubscribe.MarketingAutomationID.ShouldBe(1),
                () => controlUnsubscribe.CampaignItemName.ShouldContain("Copy of SampleItemUnsubscribe"),
                () => controlUnsubscribe.MAControlID.ShouldBe(0),
                () => controlUnsubscribe.IsConfigured.ShouldBeFalse(),
                () => controlUnsubscribe.IsCancelled.ShouldBeTrue(),
                () => controlUnsubscribe.editable.remove.ShouldBeTrue());
        }

        [Test]
        public void GetModelsForCopyFromControlBase_WhenControlTypeIsWait_ReturnsControlsCopy()
        {
            // Arrange
            var controls = new List<ControlBase> { ConfigureFakesAndGetWait() };
            var origConnectors = new List<Connector> { new Connector() };
            var connectors = new List<Connector> { new Connector() };
            var mAID = 1;

            // Act
            var resultControlList = ControlBase.GetModelsForCopyFromControlBase(controls, origConnectors, ref connectors, mAID);

            // Assert
            resultControlList.ShouldNotBeEmpty();
            resultControlList.Count.ShouldBe(1);
            var controlWait = resultControlList[0] as Wait;
            controlWait.ShouldNotBeNull();
            controlWait.ShouldSatisfyAllConditions(
                () => controlWait.ECNID.ShouldBe(0),
                () => controlWait.MarketingAutomationID.ShouldBe(1),
                () => controlWait.Text.ShouldContain("SampleItemWait"),
                () => controlWait.MAControlID.ShouldBe(0),
                () => controlWait.IsConfigured.ShouldBeFalse(),
                () => controlWait.editable.remove.ShouldBeTrue());
        }
    }
}
