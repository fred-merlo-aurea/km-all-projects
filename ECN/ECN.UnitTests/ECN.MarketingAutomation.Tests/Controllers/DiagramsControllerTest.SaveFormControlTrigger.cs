using ecn.MarketingAutomation.Models.PostModels.Controls;
using ecn.MarketingAutomation.Models.PostModels;
using NUnit.Framework;
using Shouldly;

namespace ecn.MarketingAutomation.Tests.Controllers
{
    public partial class DiagramsControllerTest
    {
        private const string SaveFormControlTriggerMethod = "SaveFormControlTrigger";

        [Test]
        [TestCaseSource(nameof(ControlTypesTestSource))]
        public void SaveFormControlTrigger_WhenFormSubmitTriggerType_SavesAllRelatedEntities(ControlBase parentControl)
        {
            // Arrange
            SetUpFakes();
            var trigger = new FormSubmit { ECNID = 1, CampaignItemTemplateID = 1, IsCancelled = true };
            var parentWait = new Wait { Days = 1, Hours = 1, Minutes = 5 };
            var parentCI = parentControl;
            var parentForm = new Form { FormID = 1 };

            // Act
            _diagramsControllerPrivateObject.Invoke(SaveFormControlTriggerMethod, trigger, parentWait, parentCI, parentForm);
            
            // Assert
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
            _isLayoutPlanSaved.ShouldBeTrue();
            _savedLayoutPlans.ShouldNotBeNull();
            _isBlastDeletedForLayoutPlan.ShouldBeTrue();
            _deletedBlastLayoutPlanId.ShouldBe(1);
        }

        [Test]
        public void SaveFormControlTrigger_WithFormAbandonTriggerAndLayoutPlainIdLessThanZero_SavesAllRelatedEntities()
        {
            // Arrange
            SetUpFakes(defaultLayoutPlanId: 0);
            var trigger = new FormAbandon { ECNID = 0, CampaignItemTemplateID = 1, IsCancelled = true };
            var parentWait = new Wait { Days = 1, Hours = 1, Minutes = 5 };
            var parentCI = ControlTypesTestSource[0];
            var parentForm = new Form { FormID = 1 };

            // Act
            _diagramsControllerPrivateObject.Invoke(SaveFormControlTriggerMethod, trigger, parentWait, parentCI, parentForm);

            // Assert
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
            _isLayoutPlanSaved.ShouldBeTrue();
            _savedLayoutPlans.ShouldNotBeNull();
            _isBlastDeletedForLayoutPlan.ShouldBeFalse();
            _deletedBlastLayoutPlanId.ShouldBe(0);
        }
    }
}
