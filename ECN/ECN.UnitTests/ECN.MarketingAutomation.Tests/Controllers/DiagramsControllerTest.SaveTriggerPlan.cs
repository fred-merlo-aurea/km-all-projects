using ecn.MarketingAutomation.Controllers.Fakes;
using ecn.MarketingAutomation.Models.PostModels.Controls;
using NUnit.Framework;
using Shouldly;

namespace ecn.MarketingAutomation.Tests.Controllers
{
    public partial class DiagramsControllerTest
    {
        private const string SaveTriggerPlanMethod = "SaveTriggerPlan";

        [Test]
        public void SaveTriggerPlan_WhenParentControlDirectOpen_SavesAllRelatedEntities()
        {
            // Arrange
            SetUpFakes();
            var trigger = new Direct_NoOpen { ECNID = 1, CampaignItemTemplateID = 1 };
            var parentWait = new Wait { Days = 1, Hours = 1, Minutes = 5 };
            var parentDirectObject = new Direct_Click();

            ShimAutomationBaseController.AllInstances.GetParentDirectOrFormWait = (dg, w) => 1;
            ShimAutomationBaseController.AllInstances.FindParentDirectEmailWait = (dg, w) => new Direct_Open
            {
                IsCancelled = true
            };

            // Act
            var savedTriggerPlanId = _diagramsControllerPrivateObject.Invoke(
                SaveTriggerPlanMethod,
                trigger,
                parentWait,
                parentDirectObject,
                false);

            // Assert
            savedTriggerPlanId.ShouldBe(1);
            _isCampaignSaved.ShouldBeTrue();
            _savedCampaign.ShouldNotBeNull();
            _isCampaignItemSaved.ShouldBeTrue();
            _savedCampaignItem.ShouldNotBeNull();
            _isLinkedTrackingOptionsSaved.ShouldBeTrue();
            _isCampaignItemOptOutGroupSaved.ShouldBeTrue();
            _savedCampaignItemOptOutGroup.ShouldNotBeNull();
            _isCampaignItemBlastSaved.ShouldBeTrue();
            _savedCampaignItemBlast.ShouldNotBeNull();
            _isBlastFromCampaignItemCreated.ShouldBeTrue();
            _isLayoutPlanSaved.ShouldBeFalse();
            _savedLayoutPlans.ShouldBeNull();
            _isTriggerPlanDeletedForBlast.ShouldBeTrue();
            _deletedBlastLayoutPlanId.ShouldBe(0);
            _deletedTriggerPlanId.ShouldBe(1);
            _deletedBlastId.ShouldBe(1);
            _isTriggerPlanSaved.ShouldBeTrue();
            _savedTriggerPlans.ShouldNotBeNull();
        }

        [Test]
        public void SaveTriggerPlan_WithParentFormAbandonAndTriggerECNIDEqualsZero_SavesAllRelatedEntities()
        {
            // Arrange
            SetUpFakes();
            var trigger = new Direct_NoOpen { ECNID = 0, CampaignItemTemplateID = 1 };
            var parentWait = new Wait { Days = 1, Hours = 1, Minutes = 5 };
            var parentDirectObject = new Direct_Click();

            ShimAutomationBaseController.AllInstances.GetParentDirectOrFormWait = (dg, w) => 2;
            ShimAutomationBaseController.AllInstances.FindParentDirectEmailWait = (dg, w) => new FormAbandon
            {
                IsCancelled = true
            };

            // Act
            var savedTriggerPlanId = _diagramsControllerPrivateObject.Invoke(
                SaveTriggerPlanMethod,
                trigger,
                parentWait,
                parentDirectObject,
                false);

            // Assert
            savedTriggerPlanId.ShouldBe(1);
            _isCampaignSaved.ShouldBeTrue();
            _savedCampaign.ShouldNotBeNull();
            _isCampaignItemSaved.ShouldBeTrue();
            _savedCampaignItem.ShouldNotBeNull();
            _isLinkedTrackingOptionsSaved.ShouldBeTrue();
            _isCampaignItemOptOutGroupSaved.ShouldBeTrue();
            _savedCampaignItemOptOutGroup.ShouldNotBeNull();
            _isCampaignItemBlastSaved.ShouldBeTrue();
            _savedCampaignItemBlast.ShouldNotBeNull();
            _isBlastFromCampaignItemCreated.ShouldBeTrue();
            _isLayoutPlanSaved.ShouldBeFalse();
            _savedLayoutPlans.ShouldBeNull();
            _isTriggerPlanDeletedForBlast.ShouldBeFalse();
            _deletedBlastLayoutPlanId.ShouldBe(0);
            _deletedTriggerPlanId.ShouldBe(0);
            _deletedBlastId.ShouldBe(0);
            _isTriggerPlanSaved.ShouldBeTrue();
            _savedTriggerPlans.ShouldNotBeNull();
        }

        [Test]
        public void SaveTriggerPlan_WithParentControlTypeFormSubmit_SavesAllRelatedEntities()
        {
            // Arrange
            SetUpFakes();
            var trigger = new Direct_NoOpen { ECNID = 1, CampaignItemTemplateID = 1 };
            var parentWait = new Wait { Days = 1, Hours = 1, Minutes = 5 };
            var parentDirectObject = new Direct_Click();

            ShimAutomationBaseController.AllInstances.GetParentDirectOrFormWait = (dg, w) => 3;
            ShimAutomationBaseController.AllInstances.FindParentDirectEmailWait = (dg, w) => new FormSubmit
            {
                IsCancelled = true
            };

            // Act
            var savedTriggerPlanId = _diagramsControllerPrivateObject.Invoke(
                SaveTriggerPlanMethod,
                trigger,
                parentWait,
                parentDirectObject,
                false);

            // Assert
            savedTriggerPlanId.ShouldBe(1);
            _isCampaignSaved.ShouldBeTrue();
            _savedCampaign.ShouldNotBeNull();
            _isCampaignItemSaved.ShouldBeTrue();
            _savedCampaignItem.ShouldNotBeNull();
            _isLinkedTrackingOptionsSaved.ShouldBeTrue();
            _isCampaignItemOptOutGroupSaved.ShouldBeTrue();
            _savedCampaignItemOptOutGroup.ShouldNotBeNull();
            _isCampaignItemBlastSaved.ShouldBeTrue();
            _savedCampaignItemBlast.ShouldNotBeNull();
            _isBlastFromCampaignItemCreated.ShouldBeTrue();
            _isLayoutPlanSaved.ShouldBeFalse();
            _savedLayoutPlans.ShouldBeNull();
            _isTriggerPlanDeletedForBlast.ShouldBeTrue();
            _deletedBlastLayoutPlanId.ShouldBe(0);
            _deletedTriggerPlanId.ShouldBe(1);
            _deletedBlastId.ShouldBe(1);
            _isTriggerPlanSaved.ShouldBeTrue();
            _savedTriggerPlans.ShouldNotBeNull();
        }

        [Test]
        public void SaveTriggerPlan_WithDefaultParentDirectClick_SavesAllRelatedEntities()
        {
            // Arrange
            SetUpFakes();
            var trigger = new Direct_NoOpen { ECNID = 1, CampaignItemTemplateID = 1 };
            var parentWait = new Wait { Days = 1, Hours = 1, Minutes = 5 };
            var parentDirectObject = new Direct_Click();

            ShimAutomationBaseController.AllInstances.GetParentDirectOrFormWait = (dg, w) => 99; // defult case
            ShimAutomationBaseController.AllInstances.FindParentDirectEmailWait = (dg, w) => new Direct_Click
            {
                IsCancelled = true
            };

            // Act
            var savedTriggerPlanId = _diagramsControllerPrivateObject.Invoke(
                SaveTriggerPlanMethod,
                trigger,
                parentWait,
                parentDirectObject,
                false);

            // Assert
            savedTriggerPlanId.ShouldBe(1);
            _isCampaignSaved.ShouldBeTrue();
            _savedCampaign.ShouldNotBeNull();
            _isCampaignItemSaved.ShouldBeTrue();
            _savedCampaignItem.ShouldNotBeNull();
            _isLinkedTrackingOptionsSaved.ShouldBeTrue();
            _isCampaignItemOptOutGroupSaved.ShouldBeTrue();
            _savedCampaignItemOptOutGroup.ShouldNotBeNull();
            _isCampaignItemBlastSaved.ShouldBeTrue();
            _savedCampaignItemBlast.ShouldNotBeNull();
            _isBlastFromCampaignItemCreated.ShouldBeTrue();
            _isLayoutPlanSaved.ShouldBeFalse();
            _savedLayoutPlans.ShouldBeNull();
            _isTriggerPlanDeletedForBlast.ShouldBeTrue();
            _deletedBlastLayoutPlanId.ShouldBe(0);
            _deletedTriggerPlanId.ShouldBe(1);
            _deletedBlastId.ShouldBe(1);
            _isTriggerPlanSaved.ShouldBeTrue();
            _savedTriggerPlans.ShouldNotBeNull();
        }
    }
}
