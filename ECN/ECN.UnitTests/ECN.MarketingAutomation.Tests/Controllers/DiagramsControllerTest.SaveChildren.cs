using System;
using System.Collections.Generic;
using ecn.MarketingAutomation.Models.PostModels;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using ECN_Framework_Common.Objects;
using Moq;
using NUnit.Framework;
using Shouldly;
using MAControlEntity = ECN_Framework_Entities.Communicator.MAControl;

namespace ecn.MarketingAutomation.Tests.Controllers
{
    public partial class DiagramsControllerTest
    {
        private const int MAControlIdAboveZero = 10;
        private const int Zero = 0;
        private const int ECNID = 100;
        private const int MarketingAutomationID = 20;
        private const string GroupSubCategory = "Group";
        private List<Connector> _allConnectors;
        private List<ControlBase> _allControls;
        private string _connectorId;

        [Test]
        public void SaveChildren_CampignItemNotEditableRemoveAndHasMAControlId_ShouldSaveItem()
        {
            //Arrange
            var parent = GetForm();
            var campaignItem = GetCampaignItem(false, MAControlIdAboveZero, false);
            var children = new List<ControlBase>
            {
                campaignItem
            };
            var keepPaused = false;
            MAControlEntity passedMAControlEntity = null;
            var toReturnMAControl = GetMAControlEntity(MAControlIdAboveZero);
            _externalFakesContext.MAControlMock
                .Setup(maControl => maControl.Save(It.IsAny<MAControlEntity>()))
                .Callback<MAControlEntity>(macControlEntity =>
                {
                    passedMAControlEntity = macControlEntity;
                });
            _externalFakesContext.MAControlMock
                .Setup(maControl => maControl.GetByControlID(It.IsAny<string>(), MarketingAutomationID))
                .Returns(toReturnMAControl);
            SetAllConnectorsProperty();
            SetAllControlsProperty();

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, keepPaused);

            //Assert
            passedMAControlEntity.ShouldNotBeNull();
            passedMAControlEntity.ShouldBe(toReturnMAControl);
            passedMAControlEntity.MAControlID.ShouldBe(MAControlIdAboveZero);
            passedMAControlEntity.xPosition.ShouldBe(campaignItem.xPosition);
            passedMAControlEntity.yPosition.ShouldBe(campaignItem.yPosition);
            passedMAControlEntity.Text.ShouldBe(campaignItem.Text);
        }

        [Test]
        public void SaveChildren_CampignItemWithMAControlIdAboveZeroAndCreateCampaignItem_ShouldSaveItem()
        {
            //Arrange
            var parent = GetForm();
            var campaignItem = GetCampaignItem(true, MAControlIdAboveZero, true, true);
            var children = new List<ControlBase>
            {
                campaignItem
            };
            var keepPaused = false;
            MAControlEntity passedMAControlEntity = null;
            var toReturnMAControl = GetMAControlEntity(MAControlIdAboveZero);
            _externalFakesContext.MAControlMock
                .Setup(maControl => maControl.Save(It.IsAny<MAControlEntity>()))
                .Callback<MAControlEntity>(macControlEntity =>
                {
                    passedMAControlEntity = macControlEntity;
                });
            _externalFakesContext.MAControlMock
                .Setup(maControl => maControl.GetByControlID(It.IsAny<string>(), MarketingAutomationID))
                .Returns(toReturnMAControl);
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.SaveCampaignItem(It.IsAny<CampaignItem>(), It.IsAny<bool>()))
                .Returns<CampaignItem, bool>((item, keepPausedParameter) => ECNID);
            SetAllConnectorsProperty();
            SetAllControlsProperty();

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, keepPaused);

            //Assert
            passedMAControlEntity.ShouldNotBeNull();
            passedMAControlEntity.ShouldBe(toReturnMAControl);
            passedMAControlEntity.MAControlID.ShouldBe(MAControlIdAboveZero);
            passedMAControlEntity.xPosition.ShouldBe(campaignItem.xPosition);
            passedMAControlEntity.yPosition.ShouldBe(campaignItem.yPosition);
            passedMAControlEntity.Text.ShouldBe(campaignItem.Text);
            _externalFakesContext.DiagramsControllerMock.Verify(controller =>
                controller.UpdateGlobalControlListWithECNID(It.IsAny<CampaignItem>()), Times.Once);
        }

        [Test]
        public void SaveChildren_CampignItemWithMAControlIdAboveZeroAndSubCategoryGroup_ShouldSaveItem()
        {
            //Arrange
            var parent = GetForm();
            var campaignItem = GetCampaignItem(true, MAControlIdAboveZero, false, true, GroupSubCategory);
            var children = new List<ControlBase>
            {
                campaignItem
            };
            var keepPaused = false;
            MAControlEntity passedMAControlEntity = null;
            var wait = GetWait();
            var toReturnMAControl = GetMAControlEntity(MAControlIdAboveZero);
            var sendTime = GetAnyTime();
            var parentCampaignItem = GetCampaignItem(false, GetAnyNumber(), sendTime: sendTime);
            _externalFakesContext.MAControlMock
                .Setup(maControl => maControl.Save(It.IsAny<MAControlEntity>()))
                .Callback<MAControlEntity>(macControlEntity =>
                {
                    passedMAControlEntity = macControlEntity;
                });
            _externalFakesContext.MAControlMock
                .Setup(maControl => maControl.GetByControlID(It.IsAny<string>(), MarketingAutomationID))
                .Returns(toReturnMAControl);
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.FindParentWait(campaignItem))
                .Returns(wait);
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.FindParentCampaignItem(wait))
                .Returns(parentCampaignItem);
            SetAllConnectorsProperty();
            SetAllControlsProperty();

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, keepPaused);

            //Assert
            _externalFakesContext.CampaignItemMock
                .Verify(item => item.UpdateSendTime(campaignItem.CampaignItemID,
                    sendTime.AddDays((double)wait.WaitTime)));
            passedMAControlEntity.MarketingAutomationID.ShouldBe(MarketingAutomationID);
            passedMAControlEntity.ControlID.ShouldBe(campaignItem.ControlID);
            passedMAControlEntity.ExtraText.ShouldBeEmpty();
        }

        [Test]
        public void SaveChildren_CampignItemWithMAControlIdZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetForm();
            var campaignItem = GetCampaignItem(true, Zero, false, subCategory: GroupSubCategory);
            var children = new List<ControlBase>
            {
                campaignItem
            };
            var wait = GetWait();
            var parentCampaignItem = GetCampaignItem(false, GetAnyNumber());
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.FindParentWait(campaignItem))
                .Returns(wait);
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.FindParentCampaignItem(wait))
                .Returns(parentCampaignItem);
            AddConnectorFor(parent, campaignItem, GetUniqueString());

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.CampaignItemMock
                .Verify(item => item.UpdateSendTime(campaignItem.CampaignItemID,
                    parentCampaignItem.SendTime.Value.AddDays((double)wait.WaitTime)));
        }

        [Test]
        public void SaveChildren_CampignItemWithMAControlIdZeroAndCreateCampaignItemTrue_ShouldSaveItem()
        {
            //Arrange
            var parent = GetForm();
            var campaignItem = GetCampaignItem(true, Zero, true);
            var children = new List<ControlBase>
            {
                campaignItem
            };
            var keepPaused = true;
            AddConnectorFor(parent, campaignItem, _connectorId);
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.SaveCampaignItem(It.IsAny<CampaignItem>(), It.IsAny<bool>()))
                .Returns(ECNID);
            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, keepPaused);

            //Assert
            _externalFakesContext.DiagramsControllerMock.Verify(controller =>
                controller.UpdateGlobalControlListWithECNID(It.Is<CampaignItem>(item => item.ECNID == ECNID)),
                    Times.Once);
        }

        [Test]
        public void SaveChildren_ClickAndParentNotWait_ShouldThrowECNException()
        {
            //Arrange
            var parent = GetForm();
            var children = new List<ControlBase>
            {
                GetClick()
            };

            //Act, Assert
            Assert.That(() => CallSaveChildren(parent, children, MarketingAutomationID, true),
                Throws.Exception.With.InnerException.TypeOf<ECNException>());
        }

        [Test]
        public void SaveChildren_ClickAndEditableRemoveFalseAndMAControlIDAboveZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetWait();
            var click = GetClick();
            var children = new List<ControlBase>
            {
                click
            };
            var maControlEntity = GetMAControlEntity(click.MAControlID);
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(click.ControlID, MarketingAutomationID))
                .Returns(maControlEntity);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControlEntity));
            maControlEntity.Text.ShouldBe(click.Text);
            maControlEntity.xPosition.ShouldBe(click.xPosition);
            maControlEntity.yPosition.ShouldBe(click.yPosition);
        }

        [Test]
        public void SaveChildren_ClickAndECNIDAboveZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetWait();
            var click = GetClick(true);
            var children = new List<ControlBase>
            {
                click
            };
            var newECNID = GetAnyNumber();
            var maControl = GetMAControlEntity(click.MAControlID);
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.SaveSmartSegmentEmailClick(click, parent, true))
                .Returns(newECNID);
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(click.ControlID, MarketingAutomationID))
                .Returns(maControl);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControl));
            maControl.Text.ShouldBe(click.Text);
            maControl.xPosition.ShouldBe(click.xPosition);
            maControl.yPosition.ShouldBe(click.yPosition);
            _externalFakesContext.DiagramsControllerMock.Verify(controller =>
                controller.UpdateGlobalControlListWithECNID(It.Is<ControlBase>(control => control.ECNID == newECNID)));
        }

        [Test]
        public void SaveChildren_ClickAndECNIDZero_ShouldSaveItem()
        {
            //Arrange
            var keepPaused = true;
            var parent = GetWait();
            var click = GetClick(true, ecnId: Zero);
            var newECNID = GetAnyNumber();
            var children = new List<ControlBase>
            {
                click
            };
            MAControlEntity passedControlEntity = null;
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.SaveSmartSegmentEmailClick(click, parent, keepPaused))
                .Returns(newECNID);
            _externalFakesContext.MAControlMock.Setup(control => control.Save(It.IsAny<MAControlEntity>()))
                .Callback<MAControlEntity>(controlEntity => passedControlEntity = controlEntity);
            AddConnectorFor(parent, click);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, keepPaused);

            //Assert
            passedControlEntity.ShouldNotBeNull();
            passedControlEntity.ControlID.ShouldBe(click.ControlID);
            passedControlEntity.ControlType.ShouldBe(click.ControlType);
            passedControlEntity.ECNID.ShouldBe(newECNID);
            passedControlEntity.ExtraText.ShouldBeEmpty();
            passedControlEntity.MarketingAutomationID.ShouldBe(MarketingAutomationID);
            passedControlEntity.Text.ShouldBe(click.Text);
            passedControlEntity.xPosition.ShouldBe(click.xPosition);
            passedControlEntity.yPosition.ShouldBe(click.yPosition);
        }

        [Test]
        public void SaveChildren_DirectClickAndECNIDAboveZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetForm();
            var directClick = GetControl<Direct_Click>(GetAnyNumber());
            var parentWait = GetWait();
            var children = new List<ControlBase>
            {
                directClick
            };
            var maControlEntity = GetMAControlEntity(directClick.MAControlID);
            _externalFakesContext.DiagramsControllerMock.Setup(controller => controller.FindParentWait(directClick))
                .Returns(parentWait);
            _externalFakesContext.DiagramsControllerMock.Setup(controller => controller.FindParent(parentWait))
                .Returns(parent);
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(directClick.ControlID, MarketingAutomationID))
                .Returns(maControlEntity);
            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.DiagramsControllerMock.Verify(controller =>
                controller.SaveMessageTrigger(directClick, parentWait, parent));
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControlEntity));
            maControlEntity.Text.ShouldBe(directClick.Text);
            maControlEntity.xPosition.ShouldBe(directClick.xPosition);
            maControlEntity.yPosition.ShouldBe(directClick.yPosition);
        }

        [Test]
        public void SaveChildren_DirectClickAndECNIDZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetForm();
            var directClick = GetControl<Direct_Click>(Zero);
            var parentWait = GetWait();
            var children = new List<ControlBase>
            {
                directClick
            };
            var newECNID = GetAnyNumber();
            MAControlEntity passedControlEntity = null;
            _externalFakesContext.DiagramsControllerMock.Setup(controller => controller.FindParentWait(directClick))
                .Returns(parentWait);
            _externalFakesContext.DiagramsControllerMock.Setup(controller => controller.FindParent(parentWait))
                .Returns(parent);
            _externalFakesContext.MAControlMock.Setup(control => control.Save(It.IsAny<MAControlEntity>()))
                .Callback<MAControlEntity>(controlEntity => passedControlEntity = controlEntity);
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.SaveMessageTrigger(directClick, parentWait, parent))
                .Returns(newECNID);
            AddConnectorFor(parent, directClick);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            passedControlEntity.ShouldNotBeNull();
            passedControlEntity.ControlID.ShouldBe(directClick.ControlID);
            passedControlEntity.ControlType.ShouldBe(directClick.ControlType);
            passedControlEntity.ECNID.ShouldBe(newECNID);
            passedControlEntity.ExtraText.ShouldBeEmpty();
            passedControlEntity.MarketingAutomationID.ShouldBe(MarketingAutomationID);
            passedControlEntity.Text.ShouldBe(directClick.Text);
            passedControlEntity.xPosition.ShouldBe(directClick.xPosition);
            passedControlEntity.yPosition.ShouldBe(directClick.yPosition);
            _externalFakesContext.DiagramsControllerMock
                .Verify(controller => controller.UpdateGlobalControlListWithECNID(directClick));
        }

        [Test]
        public void SaveChildren_FormWithECNIDAboveZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetForm();
            var form = GetForm();
            var children = new List<ControlBase>
            {
                form
            };
            var maControlEntity = GetMAControlEntity(form.MAControlID);
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(form.ControlID, MarketingAutomationID))
                .Returns(maControlEntity);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControlEntity));
            maControlEntity.Text.ShouldBe(form.Text);
            maControlEntity.xPosition.ShouldBe(form.xPosition);
            maControlEntity.yPosition.ShouldBe(form.yPosition);
            maControlEntity.ECNID.ShouldBe(form.FormID);
            form.ECNID.ShouldBe(form.FormID);
            _externalFakesContext.DiagramsControllerMock
                .Verify(controller => controller.UpdateGlobalControlListWithECNID(form));
        }

        [Test]
        public void SaveChildren_FormWithECNIDZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetForm();
            var form = GetForm(Zero);
            var children = new List<ControlBase>
            {
                form
            };
            MAControlEntity passedControlEntity = null;
            _externalFakesContext.MAControlMock.Setup(control => control.Save(It.IsAny<MAControlEntity>()))
                .Callback<MAControlEntity>(cotrolEntity => passedControlEntity = cotrolEntity);
            AddConnectorFor(parent, form);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            passedControlEntity.ShouldNotBeNull();
            passedControlEntity.ControlID.ShouldBe(form.ControlID);
            passedControlEntity.ControlType.ShouldBe(form.ControlType);
            passedControlEntity.ECNID.ShouldBe(form.FormID);
            passedControlEntity.ExtraText.ShouldBeEmpty();
            passedControlEntity.MarketingAutomationID.ShouldBe(MarketingAutomationID);
            passedControlEntity.Text.ShouldBe(form.Text);
            passedControlEntity.xPosition.ShouldBe(form.xPosition);
            passedControlEntity.yPosition.ShouldBe(form.yPosition);
        }

        [Test]
        public void SaveChildren_FormSubmitWithParentWaitAndECNIDAboveZero_ShouldSaveItem()
        {
            //Arrange
            var formSubmitWaitParent = GetForm();
            var formSubmitWait = GetWait();
            var formSubmitParent = GetForm();
            var formSubmit = GetControl<FormSubmit>();
            ControlBase parent = null;
            var children = new List<ControlBase>
            {
                formSubmit
            };
            var maControlEntity = GetMAControlEntity(formSubmit.MAControlID);
            _externalFakesContext.DiagramsControllerMock.Setup(controller => controller.FindParentWait(formSubmit))
                .Returns(formSubmitWait);
            _externalFakesContext.DiagramsControllerMock.Setup(controller => controller.FindFormParent(formSubmitWait))
                .Returns(formSubmitParent);
            _externalFakesContext.DiagramsControllerMock.Setup(controller => controller.FindParent(formSubmitWait))
                .Returns(formSubmitWaitParent);
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(formSubmit.ControlID, MarketingAutomationID))
                .Returns(maControlEntity);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.DiagramsControllerMock
                .Verify(controller => controller.SaveFormControlTrigger(formSubmit, formSubmitWait, formSubmitParent,
                    formSubmitWaitParent));
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControlEntity));
            maControlEntity.Text.ShouldBe(formSubmit.Text);
            maControlEntity.xPosition.ShouldBe(formSubmit.xPosition);
            maControlEntity.yPosition.ShouldBe(formSubmit.yPosition);
            _externalFakesContext.DiagramsControllerMock
                .Verify(controller => controller.UpdateGlobalControlListWithECNID(formSubmit));
        }

        [Test]
        public void SaveChildren_FormSubmitWithoutParentWaitAndECNIDZero_ShouldSaveItem()
        {
            //Arrange
            var formSubmitWaitParent = GetForm();
            var formSubmitWait = default(ControlBase);
            var formSubmitParent = GetForm();
            var formSubmit = GetControl<FormSubmit>(Zero);
            ControlBase parent = GetForm();
            var children = new List<ControlBase>
            {
                formSubmit
            };
            var maControlEntity = GetMAControlEntity(formSubmit.MAControlID);
            var newECNID = GetAnyNumber();
            MAControlEntity passedControlEntity = null;
            _externalFakesContext.DiagramsControllerMock.Setup(controller => controller.FindFormParent(formSubmit))
                .Returns(formSubmitParent);
            _externalFakesContext.DiagramsControllerMock.Setup(controller => controller.FindParent(formSubmitWait))
                .Returns(default(ControlBase));
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.SaveFormControlTrigger
                    (formSubmit, formSubmitWait, formSubmitParent, default(ControlBase)))
                .Returns(newECNID);
            _externalFakesContext.MAControlMock.Setup(control => control.Save(It.IsAny<MAControlEntity>()))
                .Callback<MAControlEntity>(controlEntity => passedControlEntity = controlEntity);
            AddConnectorFor(parent, formSubmit);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            passedControlEntity.ShouldNotBeNull();
            passedControlEntity.ControlID.ShouldBe(formSubmit.ControlID);
            passedControlEntity.ControlType.ShouldBe(formSubmit.ControlType);
            passedControlEntity.ECNID.ShouldBe(newECNID);
            passedControlEntity.ExtraText.ShouldBeEmpty();
            passedControlEntity.MarketingAutomationID.ShouldBe(MarketingAutomationID);
            passedControlEntity.Text.ShouldBe(formSubmit.Text);
            passedControlEntity.xPosition.ShouldBe(formSubmit.xPosition);
            passedControlEntity.yPosition.ShouldBe(formSubmit.yPosition);
        }

        [Test]
        public void SaveChildren_FormAbandonWithParentWaitAndECNIDAboveZero_ShouldSaveItem()
        {
            //Arrange
            var formAbandonWaitParent = GetForm();
            var formAbandonWait = GetWait();
            var formAbandonParent = GetForm();
            var formAbandon = GetControl<FormAbandon>();
            ControlBase parent = null;
            var children = new List<ControlBase>
            {
                formAbandon
            };
            var maControlEntity = GetMAControlEntity(formAbandon.MAControlID);
            _externalFakesContext.DiagramsControllerMock.Setup(controller => controller.FindParentWait(formAbandon))
                .Returns(formAbandonWait);
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.FindFormParent(formAbandonWait))
                .Returns(formAbandonParent);
            _externalFakesContext.DiagramsControllerMock.Setup(controller => controller.FindParent(formAbandonWait))
                .Returns(formAbandonWaitParent);
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(formAbandon.ControlID, MarketingAutomationID))
                .Returns(maControlEntity);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.DiagramsControllerMock
                .Verify(controller => controller.SaveFormControlTrigger(formAbandon, formAbandonWait,
                    formAbandonParent, formAbandonWaitParent));
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControlEntity));
            maControlEntity.Text.ShouldBe(formAbandon.Text);
            maControlEntity.xPosition.ShouldBe(formAbandon.xPosition);
            maControlEntity.yPosition.ShouldBe(formAbandon.yPosition);
            _externalFakesContext.DiagramsControllerMock
                .Verify(controller => controller.UpdateGlobalControlListWithECNID(formAbandon));
        }

        [Test]
        public void SaveChildren_FormAbandonWithoutParentWaitAndECNIDZero_ShouldSaveItem()
        {
            //Arrange
            var formAbandonWaitParent = GetForm();
            var formAbandonWait = default(ControlBase);
            var formAbandonParent = GetForm();
            var formAbandon = GetControl<FormAbandon>(Zero);
            ControlBase parent = GetForm();
            var children = new List<ControlBase>
            {
                formAbandon
            };
            var maControlEntity = GetMAControlEntity(formAbandon.MAControlID);
            var newECNID = GetAnyNumber();
            MAControlEntity passedControlEntity = null;
            _externalFakesContext.DiagramsControllerMock.Setup(controller => controller.FindFormParent(formAbandon))
                .Returns(formAbandonParent);
            _externalFakesContext.DiagramsControllerMock.Setup(controller => controller.FindParent(formAbandonWait))
                .Returns(default(ControlBase));
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.SaveFormControlTrigger
                    (formAbandon, formAbandonWait, formAbandonParent, default(ControlBase)))
                .Returns(newECNID);
            _externalFakesContext.MAControlMock.Setup(control => control.Save(It.IsAny<MAControlEntity>()))
                .Callback<MAControlEntity>(controlEntity => passedControlEntity = controlEntity);
            AddConnectorFor(parent, formAbandon);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            passedControlEntity.ShouldNotBeNull();
            passedControlEntity.ControlID.ShouldBe(formAbandon.ControlID);
            passedControlEntity.ControlType.ShouldBe(formAbandon.ControlType);
            passedControlEntity.ECNID.ShouldBe(newECNID);
            passedControlEntity.ExtraText.ShouldBeEmpty();
            passedControlEntity.MarketingAutomationID.ShouldBe(MarketingAutomationID);
            passedControlEntity.Text.ShouldBe(formAbandon.Text);
            passedControlEntity.xPosition.ShouldBe(formAbandon.xPosition);
            passedControlEntity.yPosition.ShouldBe(formAbandon.yPosition);
        }

        [Test]
        public void SaveChildren_DirectOpenWithEcnAboveZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetForm();
            var directOpen = GetControl<Direct_Open>();
            var parentWait = GetWait();
            var children = new List<ControlBase>
            {
                directOpen
            };
            var maControlEntity = GetMAControlEntity(directOpen.MAControlID);
            _externalFakesContext.DiagramsControllerMock.Setup(controller => controller.FindParentWait(directOpen))
                .Returns(parentWait);
            _externalFakesContext.DiagramsControllerMock.Setup(controller => controller.FindParent(parentWait))
                .Returns(parent);
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(directOpen.ControlID, MarketingAutomationID))
                .Returns(maControlEntity);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.DiagramsControllerMock.Verify(controller =>
                controller.SaveMessageTrigger(directOpen, parentWait, parent));
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControlEntity));
            maControlEntity.Text.ShouldBe(directOpen.Text);
            maControlEntity.xPosition.ShouldBe(directOpen.xPosition);
            maControlEntity.yPosition.ShouldBe(directOpen.yPosition);
        }

        [Test]
        public void SaveChildren_DirectOpenWithEcnZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetForm();
            var directOpen = GetControl<Direct_Open>(Zero);
            var parentWait = GetWait();
            var children = new List<ControlBase>
            {
                directOpen
            };
            var newECNID = GetAnyNumber();
            MAControlEntity passedControlEntity = null;
            _externalFakesContext.DiagramsControllerMock.Setup(controller => controller.FindParentWait(directOpen))
                .Returns(parentWait);
            _externalFakesContext.DiagramsControllerMock.Setup(controller => controller.FindParent(parentWait))
                .Returns(parent);
            _externalFakesContext.MAControlMock.Setup(control => control.Save(It.IsAny<MAControlEntity>()))
                .Callback<MAControlEntity>(controlEntity => passedControlEntity = controlEntity);
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.SaveMessageTrigger(directOpen, parentWait, parent))
                .Returns(newECNID);
            AddConnectorFor(parent, directOpen);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            passedControlEntity.ShouldNotBeNull();
            passedControlEntity.ControlID.ShouldBe(directOpen.ControlID);
            passedControlEntity.ControlType.ShouldBe(directOpen.ControlType);
            passedControlEntity.ECNID.ShouldBe(newECNID);
            passedControlEntity.ExtraText.ShouldBeEmpty();
            passedControlEntity.MarketingAutomationID.ShouldBe(MarketingAutomationID);
            passedControlEntity.Text.ShouldBe(directOpen.Text);
            passedControlEntity.xPosition.ShouldBe(directOpen.xPosition);
            passedControlEntity.yPosition.ShouldBe(directOpen.yPosition);
            _externalFakesContext.DiagramsControllerMock
                .Verify(controller => controller.UpdateGlobalControlListWithECNID(directOpen));
        }

        [Test]
        public void SaveChildren_DirectNoOpenWithEcnAboveZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetForm();
            var directNoOpen = GetControl<Direct_NoOpen>();
            var parentWait = GetWait();
            var children = new List<ControlBase>
            {
                directNoOpen
            };
            var maControlEntity = GetMAControlEntity(directNoOpen.MAControlID);
            _externalFakesContext.DiagramsControllerMock.Setup(controller => controller.FindParentWait(directNoOpen))
                .Returns(parentWait);
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.FindParentDirectEmail(parentWait))
                .Returns(parent);
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(directNoOpen.ControlID, MarketingAutomationID))
                .Returns(maControlEntity);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.DiagramsControllerMock.Verify(controller =>
                controller.SaveTriggerPlan(directNoOpen, parentWait, parent));
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControlEntity));
            maControlEntity.Text.ShouldBe(directNoOpen.Text);
            maControlEntity.xPosition.ShouldBe(directNoOpen.xPosition);
            maControlEntity.yPosition.ShouldBe(directNoOpen.yPosition);
        }

        [Test]
        public void SaveChildren_DirectNoOpenWithEcnZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetForm();
            var directNoOpen = GetControl<Direct_NoOpen>(Zero);
            var parentWait = GetWait();
            var children = new List<ControlBase>
            {
                directNoOpen
            };
            var newECNID = GetAnyNumber();
            MAControlEntity passedControlEntity = null;
            _externalFakesContext.DiagramsControllerMock.Setup(controller => controller.FindParentWait(directNoOpen))
                .Returns(parentWait);
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.FindParentDirectEmail(parentWait))
                .Returns(parent);
            _externalFakesContext.MAControlMock.Setup(control => control.Save(It.IsAny<MAControlEntity>()))
                .Callback<MAControlEntity>(controlEntity => passedControlEntity = controlEntity);
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.SaveTriggerPlan(directNoOpen, parentWait, parent))
                .Returns(newECNID);
            AddConnectorFor(parent, directNoOpen);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            passedControlEntity.ShouldNotBeNull();
            passedControlEntity.ControlID.ShouldBe(directNoOpen.ControlID);
            passedControlEntity.ControlType.ShouldBe(directNoOpen.ControlType);
            passedControlEntity.ECNID.ShouldBe(newECNID);
            passedControlEntity.ExtraText.ShouldBeEmpty();
            passedControlEntity.MarketingAutomationID.ShouldBe(MarketingAutomationID);
            passedControlEntity.Text.ShouldBe(directNoOpen.Text);
            passedControlEntity.xPosition.ShouldBe(directNoOpen.xPosition);
            passedControlEntity.yPosition.ShouldBe(directNoOpen.yPosition);
            _externalFakesContext.DiagramsControllerMock
                .Verify(controller => controller.UpdateGlobalControlListWithECNID(directNoOpen));
        }

        [Test]
        public void SaveChildren_EndWithMAControlIdAboveZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetForm();
            var end = GetControl<End>(maControlId: GetAnyNumber());
            var children = new List<ControlBase>
            {
                end
            };
            var maControlEntity = GetMAControlEntity(end.MAControlID);
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(end.ControlID, MarketingAutomationID))
                .Returns(maControlEntity);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControlEntity));
            maControlEntity.Text.ShouldBe(end.Text);
            maControlEntity.xPosition.ShouldBe(end.xPosition);
            maControlEntity.yPosition.ShouldBe(end.yPosition);
        }

        [Test]
        public void SaveChildren_EndWithMAControlIdZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetForm();
            var end = GetControl<End>(maControlId: Zero);
            var children = new List<ControlBase>
            {
                end
            };
            MAControlEntity passedControlEntity = null;
            _externalFakesContext.MAControlMock.Setup(control => control.Save(It.IsAny<MAControlEntity>()))
                .Callback<MAControlEntity>(control => passedControlEntity = control);
            AddConnectorFor(parent, end);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            passedControlEntity.ShouldNotBeNull();
            passedControlEntity.ControlID.ShouldBe(end.ControlID);
            passedControlEntity.ControlType.ShouldBe(end.ControlType);
            passedControlEntity.ECNID.ShouldBeNegative();
            passedControlEntity.ExtraText.ShouldBeEmpty();
            passedControlEntity.MarketingAutomationID.ShouldBe(MarketingAutomationID);
            passedControlEntity.Text.ShouldBe(end.Text);
            passedControlEntity.xPosition.ShouldBe(end.xPosition);
            passedControlEntity.yPosition.ShouldBe(end.yPosition);
        }

        [Test]
        public void SaveChildren_GroupWithEditableRemoveFalseAndMAControlIdAboveZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetForm();
            var group = GetControl<Group>(editableRemove: false);
            var children = new List<ControlBase>
            {
                group
            };
            var maControlEntity = GetMAControlEntity(group.MAControlID);
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(group.ControlID, MarketingAutomationID))
                .Returns(maControlEntity);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControlEntity));
            maControlEntity.Text.ShouldBe(group.Text);
            maControlEntity.xPosition.ShouldBe(group.xPosition);
            maControlEntity.yPosition.ShouldBe(group.yPosition);
        }

        [Test]
        public void SaveChildren_GroupWithEditableRemoveTrueAndMAControlAboveZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetForm();
            var group = GetControl<Group>();
            group.GroupID = GetAnyNumber();
            var children = new List<ControlBase>
            {
                group
            };
            var maControlEntity = GetMAControlEntity(group.MAControlID);
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(group.ControlID, MarketingAutomationID))
                .Returns(maControlEntity);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControlEntity));
            maControlEntity.ECNID.ShouldBe(group.GroupID);
            maControlEntity.Text.ShouldBe(group.Text);
            maControlEntity.xPosition.ShouldBe(group.xPosition);
            maControlEntity.yPosition.ShouldBe(group.yPosition);
        }

        [Test]
        public void SaveChildren_GroupWithEditableRemoveTrueAndMAControlZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetForm();
            var group = GetControl<Group>(maControlId: Zero);
            group.GroupID = GetAnyNumber();
            var children = new List<ControlBase>
            {
                group
            };
            MAControlEntity passedControlEntity = null;
            _externalFakesContext.MAControlMock
                .Setup(control => control.Save(It.IsAny<MAControlEntity>()))
                .Callback<MAControlEntity>(control => passedControlEntity = control);
            AddConnectorFor(parent, group);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            passedControlEntity.ShouldNotBeNull();
            passedControlEntity.ControlID.ShouldBe(group.ControlID);
            passedControlEntity.ControlType.ShouldBe(group.ControlType);
            passedControlEntity.ECNID.ShouldBe(group.GroupID);
            passedControlEntity.ExtraText.ShouldBeEmpty();
            passedControlEntity.MarketingAutomationID.ShouldBe(MarketingAutomationID);
            passedControlEntity.Text.ShouldBe(group.Text);
            passedControlEntity.xPosition.ShouldBe(group.xPosition);
            passedControlEntity.yPosition.ShouldBe(group.yPosition);
            _externalFakesContext.DiagramsControllerMock
                .Verify(control => control.UpdateGlobalControlListWithECNID(group));
        }

        [Test]
        public void SaveChildren_NoClickWithoutWaitParent_ShouldThrowECNException()
        {
            //Arrange
            var parent = GetForm();
            var noClick = GetControl<NoClick>();
            var children = new List<ControlBase>
            {
                noClick
            };

            //Act, Assert
            Assert.That(() => CallSaveChildren(parent, children, MarketingAutomationID, true),
                Throws.Exception.InnerException.TypeOf<ECNException>());
        }

        [Test]
        public void SaveChildren_NoClickWithEditableRemoveFalseAndMAControlIDAboveZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetWait();
            var noClick = GetControl<NoClick>(editableRemove: false);
            var children = new List<ControlBase>
            {
                noClick
            };
            var maControlEntity = GetMAControlEntity(noClick.MAControlID);
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(noClick.ControlID, MarketingAutomationID))
                .Returns(maControlEntity);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControlEntity));
            maControlEntity.Text.ShouldBe(noClick.Text);
            maControlEntity.xPosition.ShouldBe(noClick.xPosition);
            maControlEntity.yPosition.ShouldBe(noClick.yPosition);
        }

        [Test]
        public void SaveChildren_NoClickWithMAControlZeroAndECNIDAboveZero()
        {
            //Arrange
            var parent = GetWait();
            var noClick = GetControl<NoClick>();
            var children = new List<ControlBase>
            {
                noClick
            };
            var maControlEntity = GetMAControlEntity(noClick.MAControlID);
            var newECNID = GetAnyNumber();
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(noClick.ControlID, MarketingAutomationID))
                .Returns(maControlEntity);
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.SaveSmartSegmentEmailClick(noClick, parent, true))
                .Returns(newECNID);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControlEntity));
            noClick.ECNID.ShouldBe(newECNID);
            maControlEntity.Text.ShouldBe(noClick.Text);
            maControlEntity.xPosition.ShouldBe(noClick.xPosition);
            maControlEntity.yPosition.ShouldBe(noClick.yPosition);
            _externalFakesContext.DiagramsControllerMock
                .Verify(controller => controller.UpdateGlobalControlListWithECNID(noClick));
        }

        [Test]
        public void SaveChildren_NoClickWithMaControlZeroAndECNIDZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetWait();
            var noClick = GetControl<NoClick>(maControlId: Zero, ecnId: Zero);
            var children = new List<ControlBase>
            {
                noClick
            };
            MAControlEntity passedControlEntity = null;
            var newECNID = GetAnyNumber();
            _externalFakesContext.MAControlMock
                .Setup(control => control.Save(It.IsAny<MAControlEntity>()))
                .Callback<MAControlEntity>(control => passedControlEntity = control);
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.SaveSmartSegmentEmailClick(noClick, parent, true))
                .Returns(newECNID);
            AddConnectorFor(parent, noClick);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            noClick.ECNID.ShouldBe(newECNID);
            passedControlEntity.ShouldNotBeNull();
            passedControlEntity.ControlID.ShouldBe(noClick.ControlID);
            passedControlEntity.ControlType.ShouldBe(noClick.ControlType);
            passedControlEntity.ECNID.ShouldBe(newECNID);
            passedControlEntity.ExtraText.ShouldBeEmpty();
            passedControlEntity.MarketingAutomationID.ShouldBe(MarketingAutomationID);
            passedControlEntity.Text.ShouldBe(noClick.Text);
            passedControlEntity.xPosition.ShouldBe(noClick.xPosition);
            passedControlEntity.yPosition.ShouldBe(noClick.yPosition);
            _externalFakesContext.DiagramsControllerMock
                .Verify(control => control.UpdateGlobalControlListWithECNID(noClick));
        }

        [Test]
        public void SaveChildren_NoOpenWithoutWaitParent_ShouldThrowECNException()
        {
            //Arrange
            var parent = GetForm();
            var noOpen = GetControl<NoOpen>();
            var children = new List<ControlBase>
            {
                noOpen
            };

            //Act, Assert
            Assert.That(() => CallSaveChildren(parent, children, MarketingAutomationID, true),
                Throws.Exception.InnerException.TypeOf<ECNException>());
        }

        [Test]
        public void SaveChildren_NoOpenWithEditableRemoveFalseAndMAControlIDAboveZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetWait();
            var noOpen = GetControl<NoOpen>(editableRemove: false);
            var children = new List<ControlBase>
            {
                noOpen
            };
            var maControlEntity = GetMAControlEntity(noOpen.MAControlID);
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(noOpen.ControlID, MarketingAutomationID))
                .Returns(maControlEntity);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControlEntity));
            maControlEntity.Text.ShouldBe(noOpen.Text);
            maControlEntity.xPosition.ShouldBe(noOpen.xPosition);
            maControlEntity.yPosition.ShouldBe(noOpen.yPosition);
        }

        [Test]
        public void SaveChildren_NoOpenWithMAControlZeroAndECNIDAboveZero()
        {
            //Arrange
            var parent = GetWait();
            var noOpen = GetControl<NoOpen>();
            var children = new List<ControlBase>
            {
                noOpen
            };
            var maControlEntity = GetMAControlEntity(noOpen.MAControlID);
            var newECNID = GetAnyNumber();
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(noOpen.ControlID, MarketingAutomationID))
                .Returns(maControlEntity);
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.SaveSmartSegmentEmailClick(noOpen, parent, true))
                .Returns(newECNID);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControlEntity));
            noOpen.ECNID.ShouldBe(newECNID);
            maControlEntity.Text.ShouldBe(noOpen.Text);
            maControlEntity.xPosition.ShouldBe(noOpen.xPosition);
            maControlEntity.yPosition.ShouldBe(noOpen.yPosition);
            _externalFakesContext.DiagramsControllerMock
                .Verify(controller => controller.UpdateGlobalControlListWithECNID(noOpen));
        }

        [Test]
        public void SaveChildren_NoOpenkWithMaControlZeroAndECNIDZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetWait();
            var noOpen = GetControl<NoOpen>(maControlId: Zero, ecnId: Zero);
            var children = new List<ControlBase>
            {
                noOpen
            };
            MAControlEntity passedControlEntity = null;
            var newECNID = GetAnyNumber();
            _externalFakesContext.MAControlMock
                .Setup(control => control.Save(It.IsAny<MAControlEntity>()))
                .Callback<MAControlEntity>(control => passedControlEntity = control);
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.SaveSmartSegmentEmailClick(noOpen, parent, true))
                .Returns(newECNID);
            AddConnectorFor(parent, noOpen);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            noOpen.ECNID.ShouldBe(newECNID);
            passedControlEntity.ShouldNotBeNull();
            passedControlEntity.ControlID.ShouldBe(noOpen.ControlID);
            passedControlEntity.ControlType.ShouldBe(noOpen.ControlType);
            passedControlEntity.ECNID.ShouldBe(newECNID);
            passedControlEntity.ExtraText.ShouldBeEmpty();
            passedControlEntity.MarketingAutomationID.ShouldBe(MarketingAutomationID);
            passedControlEntity.Text.ShouldBe(noOpen.Text);
            passedControlEntity.xPosition.ShouldBe(noOpen.xPosition);
            passedControlEntity.yPosition.ShouldBe(noOpen.yPosition);
            _externalFakesContext.DiagramsControllerMock
                .Verify(control => control.UpdateGlobalControlListWithECNID(noOpen));
        }

        [Test]
        public void SaveChildren_NotSentWithoutWaitParent_ShouldThrowECNException()
        {
            //Arrange
            var parent = GetForm();
            var notSent = GetControl<NotSent>();
            var children = new List<ControlBase>
            {
                notSent
            };

            //Act, Assert
            Assert.That(() => CallSaveChildren(parent, children, MarketingAutomationID, true),
                Throws.Exception.InnerException.TypeOf<ECNException>());
        }

        [Test]
        public void SaveChildren_NotSentWithEditableRemoveFalseAndMAControlIDAboveZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetWait();
            var notSent = GetControl<NotSent>(editableRemove: false);
            var children = new List<ControlBase>
            {
                notSent
            };
            var maControlEntity = GetMAControlEntity(notSent.MAControlID);
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(notSent.ControlID, MarketingAutomationID))
                .Returns(maControlEntity);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControlEntity));
            maControlEntity.Text.ShouldBe(notSent.Text);
            maControlEntity.xPosition.ShouldBe(notSent.xPosition);
            maControlEntity.yPosition.ShouldBe(notSent.yPosition);
        }

        [Test]
        public void SaveChildren_NotSentWithMAControlZeroAndECNIDAboveZero()
        {
            //Arrange
            var parent = GetWait();
            var notSent = GetControl<NotSent>();
            var children = new List<ControlBase>
            {
                notSent
            };
            var maControlEntity = GetMAControlEntity(notSent.MAControlID);
            var newECNID = GetAnyNumber();
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(notSent.ControlID, MarketingAutomationID))
                .Returns(maControlEntity);
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.SaveSmartSegmentEmailClick(notSent, parent, true))
                .Returns(newECNID);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControlEntity));
            notSent.ECNID.ShouldBe(newECNID);
            maControlEntity.Text.ShouldBe(notSent.Text);
            maControlEntity.xPosition.ShouldBe(notSent.xPosition);
            maControlEntity.yPosition.ShouldBe(notSent.yPosition);
            _externalFakesContext.DiagramsControllerMock
                .Verify(controller => controller.UpdateGlobalControlListWithECNID(notSent));
        }

        [Test]
        public void SaveChildren_NotSentkWithMaControlZeroAndECNIDZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetWait();
            var notSent = GetControl<NotSent>(maControlId: Zero, ecnId: Zero);
            var children = new List<ControlBase>
            {
                notSent
            };
            MAControlEntity passedControlEntity = null;
            var newECNID = GetAnyNumber();
            _externalFakesContext.MAControlMock
                .Setup(control => control.Save(It.IsAny<MAControlEntity>()))
                .Callback<MAControlEntity>(control => passedControlEntity = control);
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.SaveSmartSegmentEmailClick(notSent, parent, true))
                .Returns(newECNID);
            AddConnectorFor(parent, notSent);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            notSent.ECNID.ShouldBe(newECNID);
            passedControlEntity.ShouldNotBeNull();
            passedControlEntity.ControlID.ShouldBe(notSent.ControlID);
            passedControlEntity.ControlType.ShouldBe(notSent.ControlType);
            passedControlEntity.ECNID.ShouldBe(newECNID);
            passedControlEntity.ExtraText.ShouldBeEmpty();
            passedControlEntity.MarketingAutomationID.ShouldBe(MarketingAutomationID);
            passedControlEntity.Text.ShouldBe(notSent.Text);
            passedControlEntity.xPosition.ShouldBe(notSent.xPosition);
            passedControlEntity.yPosition.ShouldBe(notSent.yPosition);
            _externalFakesContext.DiagramsControllerMock
                .Verify(control => control.UpdateGlobalControlListWithECNID(notSent));
        }

        [Test]
        public void SaveChildren_OpenWithoutWaitParent_ShouldThrowECNException()
        {
            //Arrange
            var parent = GetForm();
            var open = GetControl<Open>();
            var children = new List<ControlBase>
            {
                open
            };

            //Act, Assert
            Assert.That(() => CallSaveChildren(parent, children, MarketingAutomationID, true),
                Throws.Exception.InnerException.TypeOf<ECNException>());
        }

        [Test]
        public void SaveChildren_OpenWithEditableRemoveFalseAndMAControlIDAboveZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetWait();
            var open = GetControl<Open>(editableRemove: false);
            var children = new List<ControlBase>
            {
                open
            };
            var maControlEntity = GetMAControlEntity(open.MAControlID);
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(open.ControlID, MarketingAutomationID))
                .Returns(maControlEntity);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControlEntity));
            maControlEntity.Text.ShouldBe(open.Text);
            maControlEntity.xPosition.ShouldBe(open.xPosition);
            maControlEntity.yPosition.ShouldBe(open.yPosition);
        }

        [Test]
        public void SaveChildren_OpenWithMAControlZeroAndECNIDAboveZero()
        {
            //Arrange
            var parent = GetWait();
            var open = GetControl<Open>();
            var children = new List<ControlBase>
            {
                open
            };
            var maControlEntity = GetMAControlEntity(open.MAControlID);
            var newECNID = GetAnyNumber();
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(open.ControlID, MarketingAutomationID))
                .Returns(maControlEntity);
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.SaveSmartSegmentEmailClick(open, parent, true))
                .Returns(newECNID);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControlEntity));
            open.ECNID.ShouldBe(newECNID);
            maControlEntity.Text.ShouldBe(open.Text);
            maControlEntity.xPosition.ShouldBe(open.xPosition);
            maControlEntity.yPosition.ShouldBe(open.yPosition);
            _externalFakesContext.DiagramsControllerMock
                .Verify(controller => controller.UpdateGlobalControlListWithECNID(open));
        }

        [Test]
        public void SaveChildren_OpenkWithMaControlZeroAndECNIDZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetWait();
            var open = GetControl<Open>(maControlId: Zero, ecnId: Zero);
            var children = new List<ControlBase>
            {
                open
            };
            MAControlEntity passedControlEntity = null;
            var newECNID = GetAnyNumber();
            _externalFakesContext.MAControlMock
                .Setup(control => control.Save(It.IsAny<MAControlEntity>()))
                .Callback<MAControlEntity>(control => passedControlEntity = control);
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.SaveSmartSegmentEmailClick(open, parent, true))
                .Returns(newECNID);
            AddConnectorFor(parent, open);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            open.ECNID.ShouldBe(newECNID);
            passedControlEntity.ShouldNotBeNull();
            passedControlEntity.ControlID.ShouldBe(open.ControlID);
            passedControlEntity.ControlType.ShouldBe(open.ControlType);
            passedControlEntity.ECNID.ShouldBe(newECNID);
            passedControlEntity.ExtraText.ShouldBeEmpty();
            passedControlEntity.MarketingAutomationID.ShouldBe(MarketingAutomationID);
            passedControlEntity.Text.ShouldBe(open.Text);
            passedControlEntity.xPosition.ShouldBe(open.xPosition);
            passedControlEntity.yPosition.ShouldBe(open.yPosition);
            _externalFakesContext.DiagramsControllerMock
                .Verify(control => control.UpdateGlobalControlListWithECNID(open));
        }

        [Test]
        public void SaveChildren_OpenNoClickWithoutWaitParent_ShouldThrowECNException()
        {
            //Arrange
            var parent = GetForm();
            var openNoClick = GetControl<Open_NoClick>();
            var children = new List<ControlBase>
            {
                openNoClick
            };

            //Act, Assert
            Assert.That(() => CallSaveChildren(parent, children, MarketingAutomationID, true),
                Throws.Exception.InnerException.TypeOf<ECNException>());
        }

        [Test]
        public void SaveChildren_OpenNoClickWithEditableRemoveFalseAndMAControlIDAboveZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetWait();
            var openNoClick = GetControl<Open_NoClick>(editableRemove: false);
            var children = new List<ControlBase>
            {
                openNoClick
            };
            var maControlEntity = GetMAControlEntity(openNoClick.MAControlID);
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(openNoClick.ControlID, MarketingAutomationID))
                .Returns(maControlEntity);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControlEntity));
            maControlEntity.Text.ShouldBe(openNoClick.Text);
            maControlEntity.xPosition.ShouldBe(openNoClick.xPosition);
            maControlEntity.yPosition.ShouldBe(openNoClick.yPosition);
        }

        [Test]
        public void SaveChildren_OpenNoClickWithMAControlZeroAndECNIDAboveZero()
        {
            //Arrange
            var parent = GetWait();
            var openNoClick = GetControl<Open_NoClick>();
            var children = new List<ControlBase>
            {
                openNoClick
            };
            var maControlEntity = GetMAControlEntity(openNoClick.MAControlID);
            var newECNID = GetAnyNumber();
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(openNoClick.ControlID, MarketingAutomationID))
                .Returns(maControlEntity);
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.SaveSmartSegmentEmailClick(openNoClick, parent, true))
                .Returns(newECNID);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControlEntity));
            openNoClick.ECNID.ShouldBe(newECNID);
            maControlEntity.Text.ShouldBe(openNoClick.Text);
            maControlEntity.xPosition.ShouldBe(openNoClick.xPosition);
            maControlEntity.yPosition.ShouldBe(openNoClick.yPosition);
            _externalFakesContext.DiagramsControllerMock
                .Verify(controller => controller.UpdateGlobalControlListWithECNID(openNoClick));
        }

        [Test]
        public void SaveChildren_OpenNoClickkWithMaControlZeroAndECNIDZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetWait();
            var openNoClick = GetControl<Open_NoClick>(maControlId: Zero, ecnId: Zero);
            var children = new List<ControlBase>
            {
                openNoClick
            };
            MAControlEntity passedControlEntity = null;
            var newECNID = GetAnyNumber();
            _externalFakesContext.MAControlMock
                .Setup(control => control.Save(It.IsAny<MAControlEntity>()))
                .Callback<MAControlEntity>(control => passedControlEntity = control);
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.SaveSmartSegmentEmailClick(openNoClick, parent, true))
                .Returns(newECNID);
            AddConnectorFor(parent, openNoClick);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            openNoClick.ECNID.ShouldBe(newECNID);
            passedControlEntity.ShouldNotBeNull();
            passedControlEntity.ControlID.ShouldBe(openNoClick.ControlID);
            passedControlEntity.ControlType.ShouldBe(openNoClick.ControlType);
            passedControlEntity.ECNID.ShouldBe(newECNID);
            passedControlEntity.ExtraText.ShouldBeEmpty();
            passedControlEntity.MarketingAutomationID.ShouldBe(MarketingAutomationID);
            passedControlEntity.Text.ShouldBe(openNoClick.Text);
            passedControlEntity.xPosition.ShouldBe(openNoClick.xPosition);
            passedControlEntity.yPosition.ShouldBe(openNoClick.yPosition);
            _externalFakesContext.DiagramsControllerMock
                .Verify(control => control.UpdateGlobalControlListWithECNID(openNoClick));
        }

        [Test]
        public void SaveChildren_SentWithoutWaitParent_ShouldThrowECNException()
        {
            //Arrange
            var parent = GetForm();
            var sent = GetControl<Sent>();
            var children = new List<ControlBase>
            {
                sent
            };

            //Act, Assert
            Assert.That(() => CallSaveChildren(parent, children, MarketingAutomationID, true),
                Throws.Exception.InnerException.TypeOf<ECNException>());
        }

        [Test]
        public void SaveChildren_SentWithEditableRemoveFalseAndMAControlIDAboveZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetWait();
            var sent = GetControl<Sent>(editableRemove: false);
            var children = new List<ControlBase>
            {
                sent
            };
            var maControlEntity = GetMAControlEntity(sent.MAControlID);
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(sent.ControlID, MarketingAutomationID))
                .Returns(maControlEntity);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControlEntity));
            maControlEntity.Text.ShouldBe(sent.Text);
            maControlEntity.xPosition.ShouldBe(sent.xPosition);
            maControlEntity.yPosition.ShouldBe(sent.yPosition);
        }

        [Test]
        public void SaveChildren_SentWithMAControlZeroAndECNIDAboveZero()
        {
            //Arrange
            var parent = GetWait();
            var sent = GetControl<Sent>();
            var children = new List<ControlBase>
            {
                sent
            };
            var maControlEntity = GetMAControlEntity(sent.MAControlID);
            var newECNID = GetAnyNumber();
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(sent.ControlID, MarketingAutomationID))
                .Returns(maControlEntity);
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.SaveSmartSegmentEmailClick(sent, parent, true))
                .Returns(newECNID);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControlEntity));
            sent.ECNID.ShouldBe(newECNID);
            maControlEntity.Text.ShouldBe(sent.Text);
            maControlEntity.xPosition.ShouldBe(sent.xPosition);
            maControlEntity.yPosition.ShouldBe(sent.yPosition);
            _externalFakesContext.DiagramsControllerMock
                .Verify(controller => controller.UpdateGlobalControlListWithECNID(sent));
        }

        [Test]
        public void SaveChildren_SentkWithMaControlZeroAndECNIDZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetWait();
            var sent = GetControl<Sent>(maControlId: Zero, ecnId: Zero);
            var children = new List<ControlBase>
            {
                sent
            };
            MAControlEntity passedControlEntity = null;
            var newECNID = GetAnyNumber();
            _externalFakesContext.MAControlMock
                .Setup(control => control.Save(It.IsAny<MAControlEntity>()))
                .Callback<MAControlEntity>(control => passedControlEntity = control);
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.SaveSmartSegmentEmailClick(sent, parent, true))
                .Returns(newECNID);
            AddConnectorFor(parent, sent);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            sent.ECNID.ShouldBe(newECNID);
            passedControlEntity.ShouldNotBeNull();
            passedControlEntity.ControlID.ShouldBe(sent.ControlID);
            passedControlEntity.ControlType.ShouldBe(sent.ControlType);
            passedControlEntity.ECNID.ShouldBe(newECNID);
            passedControlEntity.ExtraText.ShouldBeEmpty();
            passedControlEntity.MarketingAutomationID.ShouldBe(MarketingAutomationID);
            passedControlEntity.Text.ShouldBe(sent.Text);
            passedControlEntity.xPosition.ShouldBe(sent.xPosition);
            passedControlEntity.yPosition.ShouldBe(sent.yPosition);
            _externalFakesContext.DiagramsControllerMock
                .Verify(control => control.UpdateGlobalControlListWithECNID(sent));
        }

        [Test]
        public void SaveChildren_SuppressedWithoutWaitParent_ShouldThrowECNException()
        {
            //Arrange
            var parent = GetForm();
            var suppressed = GetControl<Suppressed>();
            var children = new List<ControlBase>
            {
                suppressed
            };

            //Act, Assert
            Assert.That(() => CallSaveChildren(parent, children, MarketingAutomationID, true),
                Throws.Exception.InnerException.TypeOf<ECNException>());
        }

        [Test]
        public void SaveChildren_SuppressedWithEditableRemoveFalseAndMAControlIDAboveZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetWait();
            var suppressed = GetControl<Suppressed>(editableRemove: false);
            var children = new List<ControlBase>
            {
                suppressed
            };
            var maControlEntity = GetMAControlEntity(suppressed.MAControlID);
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(suppressed.ControlID, MarketingAutomationID))
                .Returns(maControlEntity);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControlEntity));
            maControlEntity.Text.ShouldBe(suppressed.Text);
            maControlEntity.xPosition.ShouldBe(suppressed.xPosition);
            maControlEntity.yPosition.ShouldBe(suppressed.yPosition);
        }

        [Test]
        public void SaveChildren_SuppressedWithMAControlZeroAndECNIDAboveZero()
        {
            //Arrange
            var parent = GetWait();
            var suppressed = GetControl<Suppressed>();
            var children = new List<ControlBase>
            {
                suppressed
            };
            var maControlEntity = GetMAControlEntity(suppressed.MAControlID);
            var newECNID = GetAnyNumber();
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(suppressed.ControlID, MarketingAutomationID))
                .Returns(maControlEntity);
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.SaveSmartSegmentEmailClick(suppressed, parent, true))
                .Returns(newECNID);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControlEntity));
            suppressed.ECNID.ShouldBe(newECNID);
            maControlEntity.Text.ShouldBe(suppressed.Text);
            maControlEntity.xPosition.ShouldBe(suppressed.xPosition);
            maControlEntity.yPosition.ShouldBe(suppressed.yPosition);
            _externalFakesContext.DiagramsControllerMock
                .Verify(controller => controller.UpdateGlobalControlListWithECNID(suppressed));
        }

        [Test]
        public void SaveChildren_SuppressedkWithMaControlZeroAndECNIDZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetWait();
            var suppressed = GetControl<Suppressed>(maControlId: Zero, ecnId: Zero);
            var children = new List<ControlBase>
            {
                suppressed
            };
            MAControlEntity passedControlEntity = null;
            var newECNID = GetAnyNumber();
            _externalFakesContext.MAControlMock
                .Setup(control => control.Save(It.IsAny<MAControlEntity>()))
                .Callback<MAControlEntity>(control => passedControlEntity = control);
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.SaveSmartSegmentEmailClick(suppressed, parent, true))
                .Returns(newECNID);
            AddConnectorFor(parent, suppressed);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            suppressed.ECNID.ShouldBe(newECNID);
            passedControlEntity.ShouldNotBeNull();
            passedControlEntity.ControlID.ShouldBe(suppressed.ControlID);
            passedControlEntity.ControlType.ShouldBe(suppressed.ControlType);
            passedControlEntity.ECNID.ShouldBe(newECNID);
            passedControlEntity.ExtraText.ShouldBeEmpty();
            passedControlEntity.MarketingAutomationID.ShouldBe(MarketingAutomationID);
            passedControlEntity.Text.ShouldBe(suppressed.Text);
            passedControlEntity.xPosition.ShouldBe(suppressed.xPosition);
            passedControlEntity.yPosition.ShouldBe(suppressed.yPosition);
            _externalFakesContext.DiagramsControllerMock
                .Verify(control => control.UpdateGlobalControlListWithECNID(suppressed));
        }

        [Test]
        public void SaveChildren_SubscribeWithECNIDAboveZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetForm();
            var subscribe = GetControl<Subscribe>();
            var children = new List<ControlBase>
            {
                subscribe
            };
            var newECNID = GetAnyNumber();
            var maControlEntity = GetMAControlEntity(subscribe.MAControlID);
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.SaveGroupTrigger(subscribe, parent))
                .Returns(newECNID);
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(subscribe.ControlID, MarketingAutomationID))
                .Returns(maControlEntity);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControlEntity));
            _externalFakesContext.DiagramsControllerMock
                .Verify(controller => controller.UpdateGlobalControlListWithECNID(subscribe));
            subscribe.ECNID.ShouldBe(newECNID);
            maControlEntity.Text.ShouldBe(subscribe.Text);
            maControlEntity.xPosition.ShouldBe(subscribe.xPosition);
            maControlEntity.yPosition.ShouldBe(subscribe.yPosition);
        }

        [Test]
        public void SaveChildren_SubscribeWithECNIDZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetForm();
            var subscribe = GetControl<Subscribe>(ecnId: Zero);
            var children = new List<ControlBase>
            {
                subscribe
            };
            var newECNID = GetAnyNumber();
            MAControlEntity savedControlEntity = null;
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.SaveGroupTrigger(subscribe, parent))
                .Returns(newECNID);
            _externalFakesContext.MAControlMock
                .Setup(control => control.Save(It.IsAny<MAControlEntity>()))
                .Callback<MAControlEntity>(controlEntity => savedControlEntity = controlEntity);
            AddConnectorFor(parent, subscribe);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            subscribe.ECNID.ShouldBe(newECNID);
            savedControlEntity.ShouldNotBeNull();
            savedControlEntity.ControlID.ShouldBe(subscribe.ControlID);
            savedControlEntity.ControlType.ShouldBe(subscribe.ControlType);
            savedControlEntity.ECNID.ShouldBe(subscribe.ECNID);
            savedControlEntity.ExtraText.ShouldBeEmpty();
            savedControlEntity.MarketingAutomationID.ShouldBe(MarketingAutomationID);
            savedControlEntity.Text.ShouldBe(subscribe.Text);
            savedControlEntity.xPosition.ShouldBe(subscribe.xPosition);
            savedControlEntity.yPosition.ShouldBe(subscribe.yPosition);
        }

        [Test]
        public void SaveChildren_UnsubscribeWithECNIDAboveZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetForm();
            var unsubscribe = GetControl<Unsubscribe>();
            var children = new List<ControlBase>
            {
                unsubscribe
            };
            var newECNID = GetAnyNumber();
            var maControlEntity = GetMAControlEntity(unsubscribe.MAControlID);
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.SaveGroupTrigger(unsubscribe, parent))
                .Returns(newECNID);
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(unsubscribe.ControlID, MarketingAutomationID))
                .Returns(maControlEntity);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControlEntity));
            _externalFakesContext.DiagramsControllerMock
                .Verify(controller => controller.UpdateGlobalControlListWithECNID(unsubscribe));
            unsubscribe.ECNID.ShouldBe(newECNID);
            maControlEntity.Text.ShouldBe(unsubscribe.Text);
            maControlEntity.xPosition.ShouldBe(unsubscribe.xPosition);
            maControlEntity.yPosition.ShouldBe(unsubscribe.yPosition);
        }

        [Test]
        public void SaveChildren_UnsubscribeWithECNIDZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetForm();
            var unsubscribe = GetControl<Unsubscribe>(ecnId: Zero);
            var children = new List<ControlBase>
            {
                unsubscribe
            };
            var newECNID = GetAnyNumber();
            MAControlEntity savedControlEntity = null;
            _externalFakesContext.DiagramsControllerMock
                .Setup(controller => controller.SaveGroupTrigger(unsubscribe, parent))
                .Returns(newECNID);
            _externalFakesContext.MAControlMock
                .Setup(control => control.Save(It.IsAny<MAControlEntity>()))
                .Callback<MAControlEntity>(controlEntity => savedControlEntity = controlEntity);
            AddConnectorFor(parent, unsubscribe);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            unsubscribe.ECNID.ShouldBe(newECNID);
            savedControlEntity.ShouldNotBeNull();
            savedControlEntity.ControlID.ShouldBe(unsubscribe.ControlID);
            savedControlEntity.ControlType.ShouldBe(unsubscribe.ControlType);
            savedControlEntity.ECNID.ShouldBe(unsubscribe.ECNID);
            savedControlEntity.ExtraText.ShouldBeEmpty();
            savedControlEntity.MarketingAutomationID.ShouldBe(MarketingAutomationID);
            savedControlEntity.Text.ShouldBe(unsubscribe.Text);
            savedControlEntity.xPosition.ShouldBe(unsubscribe.xPosition);
            savedControlEntity.yPosition.ShouldBe(unsubscribe.yPosition);
        }

        [Test]
        public void SaveChildren_WaitEditableRemoveFalseAndMAControlIDAboveZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetForm();
            var wait = GetWait(editableRemove: false);
            var children = new List<ControlBase>
            {
                wait
            };
            var maControlEntity = GetMAControlEntity(wait.MAControlID);
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(wait.ControlID, MarketingAutomationID))
                .Returns(maControlEntity);
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(It.IsNotIn(wait.ControlID), MarketingAutomationID))
                .Returns(GetMAControlEntity(GetAnyNumber()));
            var childWait = GetWait();
            AddConnectorFor(wait, childWait);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControlEntity));
            maControlEntity.Text.ShouldBe(wait.Text);
            maControlEntity.xPosition.ShouldBe(wait.xPosition);
            maControlEntity.yPosition.ShouldBe(wait.yPosition);
        }

        [Test]
        public void SaveChildren_WaitEditableRemoveTrueAndMAControlIDAboveZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetForm();
            var wait = GetWait(editableRemove: true);
            var children = new List<ControlBase>
            {
                wait
            };
            var maControlEntity = GetMAControlEntity(wait.MAControlID);
            _externalFakesContext.MAControlMock
                .Setup(control => control.GetByControlID(wait.ControlID, MarketingAutomationID))
                .Returns(maControlEntity);
            AddConnectorFor(parent, wait);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            _externalFakesContext.MAControlMock.Verify(control => control.Save(maControlEntity));
            maControlEntity.Text.ShouldBe(wait.Text);
            maControlEntity.xPosition.ShouldBe(wait.xPosition);
            maControlEntity.yPosition.ShouldBe(wait.yPosition);
        }

        [Test]
        public void SaveChildren_WaitEditableRemoveTrueAndMAControlIDZero_ShouldSaveItem()
        {
            //Arrange
            var parent = GetForm();
            var wait = GetWait(editableRemove: true, maControlId: Zero);
            var children = new List<ControlBase>
            {
                wait
            };
            MAControlEntity savedMAControlEntity = null;
            _externalFakesContext.MAControlMock.Setup(control => control.Save(It.IsAny<MAControlEntity>()))
                .Callback<MAControlEntity>(controlEntity => savedMAControlEntity = controlEntity);
            AddConnectorFor(parent, wait);

            //Act
            CallSaveChildren(parent, children, MarketingAutomationID, true);

            //Assert
            savedMAControlEntity.ShouldNotBeNull();
            savedMAControlEntity.ControlID.ShouldBe(wait.ControlID);
            savedMAControlEntity.ControlType.ShouldBe(wait.ControlType);
            savedMAControlEntity.ECNID.ShouldBeNegative();
            savedMAControlEntity.ExtraText.ShouldBeEmpty();
            savedMAControlEntity.MarketingAutomationID.ShouldBe(MarketingAutomationID);
            savedMAControlEntity.Text.ShouldBe(wait.Text);
            savedMAControlEntity.xPosition.ShouldBe(wait.xPosition);
            savedMAControlEntity.yPosition.ShouldBe(wait.yPosition);
        }

        private T GetControl<T>(
            int? ecnId = null,
            int? maControlId = null,
            bool editableRemove = true) where T : ControlBase, new()
        {
            return new T
            {
                MAControlID = maControlId ?? GetAnyNumber(),
                ControlID = GetUniqueString(),
                xPosition = GetAnyDecimal(),
                yPosition = GetAnyDecimal(),
                ECNID = ecnId ?? GetAnyNumber(),
                IsDirty = true,
                Text = GetUniqueString(),
                editable = new shapeEditable
                {
                    remove = editableRemove
                }
            };
        }

        private void AddConnectorFor(ControlBase fromControl, ControlBase toControl, string id = null)
        {
            AddConnectorFor(_allControls, _allConnectors, fromControl, toControl, id);
        }
        private void AddConnectorFor(
            List<ControlBase> controls,
            List<Connector> connectors,
            ControlBase parentControl,
            ControlBase childControl,
            string id = null,
            bool addParentToControls = true)
        {
            connectors.Add(new Connector
            {
                from = new from
                {
                    shapeId = parentControl.ControlID,
                },
                to = new to
                {
                    shapeId = childControl.ControlID
                },
                id = id ?? GetUniqueString()
            });
            Action<ControlBase> addToListIfNotExist = control =>
            {
                if (!controls.Contains(control))
                {
                    controls.Add(control);
                }
            };
            if (addParentToControls)
            {
                addToListIfNotExist(parentControl);
            }
            addToListIfNotExist(childControl);
        }

        private Form GetForm(int? ecnId = null)
        {
            var result = GetControl<Form>(ecnId);
            result.FormID = GetAnyNumber();
            return result;
        }

        private Wait GetWait(
            int? ecnId = null,
            int? maControlId = null,
            bool editableRemove = true)
        {
            const int daysMinimum = 100;
            const int daysMaximum = daysMinimum * 10;
            var result = GetControl<Wait>(ecnId, maControlId, editableRemove);
            result.WaitTime = GetAnyNumber(daysMinimum, daysMaximum);
            return result;
        }

        private void SetAllConnectorsProperty()
        {
            const string AllConnectorsPropertyName = "AllConnectors";
            _allConnectors = new List<Connector>();
            _diagramsControllerPrivateObject.SetProperty(AllConnectorsPropertyName, _allConnectors);
        }

        private void SetAllControlsProperty()
        {
            const string AllControlsPropertyName = "AllControls";
            _allControls = new List<ControlBase>();
            _diagramsControllerPrivateObject.SetProperty(AllControlsPropertyName, _allControls);
        }

        private CampaignItem GetCampaignItem(
            bool editableRemove,
            int maControlId,
            bool createCampaignItem = false,
            bool isDirty = false,
            string subCategory = EmptyString,
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

        private MAControlEntity GetMAControlEntity(int maControlId)
        {
            return new MAControlEntity
            {
                MAControlID = maControlId
            };
        }

        private void CallSaveChildren(
            ControlBase parent,
            List<ControlBase> children,
            int marketingAutomationID,
            bool keepPaused)
        {
            const string SaveChildrenMethodName = "SaveChildren";
            _diagramsControllerPrivateObject.Invoke(SaveChildrenMethodName,
                parent, children, marketingAutomationID, keepPaused);
        }
    }
}
