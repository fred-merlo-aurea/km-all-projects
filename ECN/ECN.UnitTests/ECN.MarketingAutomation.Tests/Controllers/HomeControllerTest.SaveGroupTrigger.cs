using System;
using System.Reflection;
using ecn.MarketingAutomation.Controllers.Fakes;
using ecn.MarketingAutomation.Models.PostModels.Controls;
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
        private const string MethodSaveGroupTrigger = "SaveGroupTrigger";

        [Test]
        public void SaveGroupTrigger_WhenTriggerTypeSubscribeAndParentWait_SavesRelatedEntities()
        {
            // Arrange
            SetUpFakesForSaveGroupTrigger();
            var trigger = new Subscribe
            {
                ECNID = 1,
                CampaignItemTemplateID = 1,
                IsCancelled = true
            };
            var parentWait = new Wait { };

            // Act
            var savedLayoutPlanId = _privateObject.Invoke(MethodSaveGroupTrigger, trigger, parentWait);

            // Assert
            _savedCampaignItem.ShouldSatisfyAllConditions(
                () => savedLayoutPlanId.ShouldBe(1),
                () => _isCampaignSaved.ShouldBeTrue(),
                () => _savedCampaign.ShouldNotBeNull(),
                () => _isCampaignItemSaved.ShouldBeTrue(),
                () => _savedCampaignItem.ShouldNotBeNull(),
                () => _isLinkedTrackingOptionsSaved.ShouldBeTrue(),
                () => _isCampaignItemOptOutGroupSaved.ShouldBeTrue(),
                () => _savedCampaignItemOptOutGroup.ShouldNotBeNull(),
                () => _isCampaignItemBlastSaved.ShouldBeTrue(),
                () => _savedCampaignItemBlast.ShouldNotBeNull(),
                () => _isBlastFromCampaignItemCreated.ShouldBeTrue(),
                () => _isLayoutPlanSaved.ShouldBeTrue(),
                () => _savedLayoutPlans.ShouldNotBeNull(),
                () => _isBlastDeletedForLayoutPlan.ShouldBeTrue(),
                () => _deletedBlastLayoutPlanId.ShouldBe(1)
            );
        }

        [Test]
        public void SaveGroupTrigger_WhenUnSubscribeTriggerAndParentWaitGroup_SavesRelatedEntities()
        {
            // Arrange
            SetUpFakesForSaveGroupTrigger(defaultLayoutPlanId: 0);
            var trigger = new Unsubscribe
            {
                ECNID = 0,
                CampaignItemTemplateID = 1
            };
            var parentWait = new Group { GroupID = 1 };

            // Act
            var exception = Should.NotThrow(() =>
                 _privateObject.Invoke(MethodSaveGroupTrigger, trigger, parentWait));

            // Assert
            exception.ShouldSatisfyAllConditions(
                () => exception.ShouldNotBeNull(),
                () => _isCampaignSaved.ShouldBeTrue(),
                () => _savedCampaign.ShouldNotBeNull(),
                () => _isCampaignItemSaved.ShouldBeTrue(),
                () => _savedCampaignItem.ShouldNotBeNull(),
                () => _isLinkedTrackingOptionsSaved.ShouldBeTrue(),
                () => _isCampaignItemOptOutGroupSaved.ShouldBeTrue(),
                () => _savedCampaignItemOptOutGroup.ShouldNotBeNull(),
                () => _isCampaignItemBlastSaved.ShouldBeTrue(),
                () => _savedCampaignItemBlast.ShouldNotBeNull(),
                () => _isBlastFromCampaignItemCreated.ShouldBeTrue(),
                () => _isLayoutPlanSaved.ShouldBeTrue(),
                () => _savedLayoutPlans.ShouldNotBeNull(),
                () => _isBlastDeletedForLayoutPlan.ShouldBeFalse(),
                () => _deletedBlastLayoutPlanId.ShouldBe(0)
            );
        }

        private void SetUpFakesForSaveGroupTrigger(int defaultLayoutPlanId = 1)
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
