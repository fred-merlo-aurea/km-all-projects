using System;
using ecn.MarketingAutomation.Controllers.Fakes;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using ECN_Framework_BusinessLayer.Communicator.Fakes;
using ECN_Framework_BusinessLayer.FormDesigner.Fakes;
using FormEntities = ECN_Framework_Entities.FormDesigner;
using Entities = ECN_Framework_Entities.Communicator;
using NUnit.Framework;
using Shouldly;

namespace ECN.MarketingAutomation.Tests.Controllers
{
    public partial class HomeControllerTest
    {
        private const string MethodSaveTriggerPlan = "SaveTriggerPlan";

        [Test]
        public void SaveTriggerPlan_WhenParentControlDirectOpen_SavesAllRelatedEntities()
        {
            // Arrange
            SetUpFakesForSaveTriggerPlan();
            var trigger = new Direct_NoOpen { ECNID = 1, CampaignItemTemplateID = 1 };
            var parentWait = new Wait { Days = 1, Hours = 1, Minutes = 5 };
            var parentDirectObject = new Direct_Click();

            ShimAutomationBaseController.AllInstances.GetParentDirectOrFormWait = (dg, w) => 1;
            ShimAutomationBaseController.AllInstances.FindParentDirectEmailWait = (dg, w) => new Direct_Open
            {
                IsCancelled = true
            };

            // Act
            var savedTriggerPlanId = _privateObject.Invoke(
                MethodSaveTriggerPlan,
                trigger,
                parentWait,
                parentDirectObject, 
                true);

            // Assert
            savedTriggerPlanId.ShouldSatisfyAllConditions(
                ()=> savedTriggerPlanId.ShouldBe(1),
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
                ()=> _isLayoutPlanSaved.ShouldBeFalse(),
                ()=> _savedLayoutPlans.ShouldBeNull(),
                ()=> _isTriggerPlanDeletedForBlast.ShouldBeTrue(),
                ()=> _deletedBlastLayoutPlanId.ShouldBe(0),
                ()=> _deletedTriggerPlanId.ShouldBe(1),
                ()=> _deletedBlastId.ShouldBe(1),
                ()=> _isTriggerPlanSaved.ShouldBeTrue(),
                ()=> _savedTriggerPlans.ShouldNotBeNull()
             );
        }

        [Test]
        [TestCase(1)]
        [TestCase(0)]
        public void SaveTriggerPlan_WithParentFormAbandon_SavesAllRelatedEntities(int ecnId)
        {
            // Arrange
            SetUpFakesForSaveTriggerPlan();
            var trigger = new Direct_NoOpen { ECNID = ecnId, CampaignItemTemplateID = 1 };
            var parentWait = new Wait { Days = 1, Hours = 1, Minutes = 5 };
            var parentDirectObject = new Direct_Click();

            ShimAutomationBaseController.AllInstances.GetParentDirectOrFormWait = (dg, w) => 2;
            ShimAutomationBaseController.AllInstances.FindParentDirectEmailWait = (dg, w) => new FormAbandon
            {
                IsCancelled = true
            };

            // Act
            var savedTriggerPlanId = _privateObject.Invoke(
                MethodSaveTriggerPlan,
                trigger,
                parentWait,
                parentDirectObject,
                true);

            // Assert
            savedTriggerPlanId.ShouldSatisfyAllConditions(
                ()=> savedTriggerPlanId.ShouldBe(1),
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
                ()=> _isLayoutPlanSaved.ShouldBeFalse(),
                ()=> _savedLayoutPlans.ShouldBeNull(),
                ()=> _deletedBlastLayoutPlanId.ShouldBe(0),
                ()=> _isTriggerPlanSaved.ShouldBeTrue(),
                ()=> _savedTriggerPlans.ShouldNotBeNull()
            );
        }

        [Test]
        public void SaveTriggerPlan_WithParentControlTypeFormSubmit_SavesAllRelatedEntities()
        {
            // Arrange
            SetUpFakesForSaveTriggerPlan();
            var trigger = new Direct_NoOpen { ECNID = 1, CampaignItemTemplateID = 1 };
            var parentWait = new Wait { Days = 1, Hours = 1, Minutes = 5 };
            var parentDirectObject = new Direct_Click();

            ShimAutomationBaseController.AllInstances.GetParentDirectOrFormWait = (dg, w) => 3;
            ShimAutomationBaseController.AllInstances.FindParentDirectEmailWait = (dg, w) => new FormSubmit
            {
                IsCancelled = true
            };

            // Act
            var savedTriggerPlanId = _privateObject.Invoke(
                MethodSaveTriggerPlan,
                trigger,
                parentWait,
                parentDirectObject,
                true);

            // Assert
            savedTriggerPlanId.ShouldSatisfyAllConditions(
                ()=> savedTriggerPlanId.ShouldBe(1),
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
                ()=> _isLayoutPlanSaved.ShouldBeFalse(),
                ()=> _savedLayoutPlans.ShouldBeNull(),
                ()=> _isTriggerPlanDeletedForBlast.ShouldBeTrue(),
                ()=> _deletedBlastLayoutPlanId.ShouldBe(0),
                ()=> _deletedTriggerPlanId.ShouldBe(1),
                ()=> _deletedBlastId.ShouldBe(1),
                ()=> _isTriggerPlanSaved.ShouldBeTrue(),
                ()=> _savedTriggerPlans.ShouldNotBeNull()
            );
        }

        [Test]
        public void SaveTriggerPlan_WithDefaultParentDirectClick_SavesAllRelatedEntities()
        {
            // Arrange
            SetUpFakesForSaveTriggerPlan();
            var trigger = new Direct_NoOpen { ECNID = 1, CampaignItemTemplateID = 1 };
            var parentWait = new Wait { Days = 1, Hours = 1, Minutes = 5 };
            var parentDirectObject = new Direct_Click();

            ShimAutomationBaseController.AllInstances.GetParentDirectOrFormWait = (dg, w) => 99;
            ShimAutomationBaseController.AllInstances.FindParentDirectEmailWait = (dg, w) => new Direct_Click
            {
                IsCancelled = true
            };

            // Act
            var savedTriggerPlanId = _privateObject.Invoke(
                MethodSaveTriggerPlan,
                trigger,
                parentWait,
                parentDirectObject,
                true);

            // Assert
            savedTriggerPlanId.ShouldSatisfyAllConditions(
                ()=> savedTriggerPlanId.ShouldBe(1),
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
                ()=> _isLayoutPlanSaved.ShouldBeFalse(),
                ()=> _savedLayoutPlans.ShouldBeNull(),
                ()=> _isTriggerPlanDeletedForBlast.ShouldBeTrue(),
                ()=> _deletedBlastLayoutPlanId.ShouldBe(0),
                ()=> _deletedTriggerPlanId.ShouldBe(1),
                ()=> _deletedBlastId.ShouldBe(1),
                ()=> _isTriggerPlanSaved.ShouldBeTrue(),
                ()=> _savedTriggerPlans.ShouldNotBeNull()
            );
        }

        private void SetUpFakesForSaveTriggerPlan(int defaultLayoutPlanId = 1)
        {
            CleanUpAssignments();
            SetupForValidatePublish();
            ShimAutomationBaseController.AllInstances.SaveMessageTriggerControlBaseControlBaseControlBase = null;
            ShimAutomationBaseController.AllInstances.SaveFormControlTriggerControlBaseControlBaseControlBaseControlBase = null;
            ShimAutomationBaseController.AllInstances.SaveTriggerPlanControlBaseControlBaseControlBaseBoolean = null;
            ShimAutomationBaseController.AllInstances.SaveGroupTriggerControlBaseControlBase = null;
            ShimAutomationBaseController.AllInstances.FindParentCampaignItemControlBase = (c, cb) => 
                new CampaignItem { CustomerID = 1 };
            ShimAutomationBaseController.AllInstances.FindParentGroupControlBase = (c, cb) => new Group { CustomerID = 1 };
            ShimLayoutPlans.GetByLayoutPlanID_UseAmbientTransactionInt32User =
                (ecnId, user) => GetLayoutPlans(defaultLayoutPlanId);
            ShimCampaignItem.GetByCampaignItemID_NoAccessCheck_UseAmbientTransactionInt32Boolean =
                (ecnId, b) => new Entities::CampaignItem { };
            ShimCampaignItem.GetByBlastID_NoAccessCheck_UseAmbientTransactionInt32Boolean =
                (blastId, b) => GetTestCampaignItem();
            ShimCampaignItemTemplate.GetByCampaignItemTemplateIDInt32UserBoolean =
                (id, user, b) => GetCampaignItemTemplate();
            ShimCampaign.GetByBlastID_UseAmbientTransactionInt32UserBoolean = (id, user, b) => GetCampaign();
            ShimBlast.GetByCampaignItemBlastID_UseAmbientTransactionInt32UserBoolean =
                (blastId, user, t) => GetBlastAbstract();
            ShimTriggerPlans.GetByTriggerPlanIDInt32User = (cid, user) => GetTriggerPlans();
            ShimForm.GetByFormID_NoAccessCheckInt32 = (fid) => new FormEntities::Form { TokenUID = Guid.Empty };
            SetUpBusinessCommunicatorSaveFakes();
            ShimBlast.CreateBlastsFromCampaignItem_UseAmbientTransactionInt32UserBooleanBoolean = (id, user, b, t) =>
            {
                _isBlastFromCampaignItemCreated = true;
            };
        }
    }
}
