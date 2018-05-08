using System;
using System.Collections.Generic;
using ecn.MarketingAutomation.Controllers.Fakes;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using ecn.MarketingAutomation.Models.PostModels;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_BusinessLayer.FormDesigner.Fakes;
using FormEntities = ECN_Framework_Entities.FormDesigner;
using CommunicatorEntities = ECN_Framework_Entities.Communicator;
using NUnit.Framework;
using Shouldly;

namespace ECN.MarketingAutomation.Tests.Controllers
{
    public partial class HomeControllerTest
    {
        private const string MethodSaveMessageTrigger = "SaveMessageTrigger";

        [Test]
        public void SaveMessageTrigger_WithDirectClickTrigger_SavesAllRelatedEntities(
            [ValueSource(nameof(ControlTypesTestSource))] ControlBase parentControl,
            [Values(true, false)] bool anylink)
        {
            // Arrange
            SetUpFakesForSaveMessageTrigger();
            var trigger = new Direct_Click
            {
                ECNID = 1,
                CampaignItemTemplateID = 1,
                IsCancelled = true,
                AnyLink = anylink
            };
            var parentWait = new Wait { Days = 1, Hours = 1, Minutes = 5 };
            var parentCI = parentControl;

            // Act
            _privateObject.Invoke(MethodSaveMessageTrigger, trigger, parentWait, parentCI);

            // Assert
            _isCampaignSaved.ShouldSatisfyAllConditions(
                ()=>_isCampaignSaved.ShouldBeTrue(),
                ()=>_savedCampaign.ShouldNotBeNull(),
                ()=>_isCampaignItemSaved.ShouldBeTrue(),
                ()=>_savedCampaignItem.ShouldNotBeNull(),
                ()=>_isLinkedTrackingOptionsSaved.ShouldBeTrue(),
                ()=>_isCampaignItemOptOutGroupSaved.ShouldBeTrue(),
                ()=>_savedCampaignItemOptOutGroup.ShouldNotBeNull(),
                ()=>_isCampaignItemBlastSaved.ShouldBeTrue(),
                ()=>_savedCampaignItemBlast.ShouldNotBeNull(),
                ()=>_isBlastFromCampaignItemCreated.ShouldBeTrue(),
                ()=>_isLayoutPlanSaved.ShouldBeTrue(),
                ()=>_savedLayoutPlans.ShouldNotBeNull(),
                ()=>_isBlastDeletedForLayoutPlan.ShouldBeTrue(),
                ()=>_deletedBlastLayoutPlanId.ShouldBe(1)
            );
        }

        [Test]
        public void SaveMessageTrigger_WithDirectOpenTriggerAndLayoutPlainIdLessThanZero_SavesAllRelatedEntities()
        {
            // Arrange
            SetUpFakesForSaveMessageTrigger(defaultLayoutPlanId: 0);
            var trigger = new Direct_Open { ECNID = 0, CampaignItemTemplateID = 1, IsCancelled = true };
            var parentWait = new Wait { Days = 1, Hours = 1, Minutes = 5 };
            var parentCI = ControlTypesTestSource[0];

            // Act
            _privateObject.Invoke(MethodSaveMessageTrigger, trigger, parentWait, parentCI);

            // Assert
            _isCampaignSaved.ShouldSatisfyAllConditions(
                ()=> _isCampaignSaved.ShouldBeTrue(),
                ()=> _savedCampaign.ShouldNotBeNull(),
                ()=> _isCampaignItemSaved.ShouldBeTrue(),
                ()=> _savedCampaignItem.ShouldNotBeNull(),
                ()=> _isLinkedTrackingOptionsSaved.ShouldBeTrue(),
                ()=> _isCampaignItemOptOutGroupSaved.ShouldBeTrue(),
                ()=> _savedCampaignItemOptOutGroup.ShouldNotBeNull(),
                ()=> _isCampaignItemBlastSaved.ShouldBeTrue(),
                ()=> _savedCampaignItemBlast.ShouldNotBeNull(),
                ()=> _isBlastFromCampaignItemCreated.ShouldBeTrue(),
                ()=> _isLayoutPlanSaved.ShouldBeTrue(),
                ()=> _savedLayoutPlans.ShouldNotBeNull(),
                ()=> _isBlastDeletedForLayoutPlan.ShouldBeFalse(),
                ()=> _deletedBlastLayoutPlanId.ShouldBe(0)
            );
        }

        private void SetUpFakesForSaveMessageTrigger(int defaultLayoutPlanId = 1)
        {
            CleanUpAssignments();
            SetupForValidatePublish();
            ShimAutomationBaseController.AllInstances.SaveMessageTriggerControlBaseControlBaseControlBase = null;
            ShimAutomationBaseController.AllInstances
                .SaveFormControlTriggerControlBaseControlBaseControlBaseControlBase = null;
            ShimAutomationBaseController.AllInstances.SaveTriggerPlanControlBaseControlBaseControlBaseBoolean = null;
            ShimAutomationBaseController.AllInstances.SaveGroupTriggerControlBaseControlBase = null;
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (c, cb) => 
                new CampaignItem { CustomerID = 1 };
            ShimAutomationBaseController.AllInstances.FindParentGroupControlBase = (c, cb) => new Group { CustomerID = 1 };
            ShimLayoutPlans.GetByLayoutPlanID_UseAmbientTransactionInt32User =
                (ecnId, user) => GetLayoutPlans(defaultLayoutPlanId);
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheck_UseAmbientTransactionInt32Boolean =
                (ecnId, b) => new CommunicatorEntities.CampaignItem { };
            ShimCampaignItem.GetByBlastID_NoAccessCheck_UseAmbientTransactionInt32Boolean =
                (blastId, b) => GetTestCampaignItem();
            ShimCampaignItemTemplate.GetByCampaignItemTemplateIDInt32UserBoolean =
                (id, user, b) => GetCampaignItemTemplate();
            ShimCampaign.GetByBlastID_UseAmbientTransactionInt32UserBoolean = (id, user, b) => GetCampaign();
            ShimBlast.GetByCampaignItemBlastID_UseAmbientTransactionInt32UserBoolean =
                (blastId, user, t) => GetBlastAbstract();
            ShimTriggerPlans.GetByTriggerPlanIDInt32User = (cid, user) => GetTriggerPlans();
            ShimForm.GetByFormID_NoAccessCheckInt32 = (fid) => new FormEntities.Form { TokenUID = Guid.Empty };
            SetUpBusinessCommunicatorSaveFakes();
            ShimBlast.CreateBlastsFromCampaignItem_UseAmbientTransactionInt32UserBooleanBoolean = (id, user, b, t) =>
            {
                _isBlastFromCampaignItemCreated = true;
            };
        }
    }
}
